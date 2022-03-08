using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using ConaviWeb.Commons;
using ConaviWeb.Models;
using ConaviWeb.Services;
using ConaviWeb.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly ISecurityTools _securityTools;
        public LoginController(ISecurityRepository securityRepository, ISecurityTools securityTools)
        {
            _securityRepository = securityRepository;
            _securityTools = securityTools;
        }
        public IActionResult Index()
        {
                return View("../Login/Login");   
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Auth([FromForm] UserRequest userRequest)
        {
            Response response = new Response();
            string spassword = _securityTools.GetSHA256(userRequest.Password);
            userRequest.Password = spassword;
            var userResponse = await _securityRepository.GetLoginByCredentials(userRequest);
            List<Module> modules = new();
            if (userResponse != null)
            {
                userResponse.AccessToken = await _securityTools.GetToken(userResponse);
                userResponse.Modules = await _securityRepository.GetModules(Convert.ToInt32(userResponse.Rol), userResponse.Sistema);
                if (userResponse.AccessToken != null)
                {
                    HttpContext.Session.SetObject("ComplexObject", userResponse);
                    HttpContext.Session.SetString("Token", userResponse.AccessToken);
                    return (RedirectToAction("Index", userResponse.Controller));
                }
                else
                {
                    return (RedirectToAction("Error"));
                }
                
            }
            else
            {
                ViewBag.Alert = AlertService.ShowAlert(Alerts.Danger, "Usuario y/o Contraseña incorrectos.");
                return View("../Login/Login");
            }
            
        }

        [Authorize]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("ComplexObject");
            return RedirectToAction("Index","Inicio");
        }

        [Authorize]
        [Route("mainwindow")]
        [HttpGet]
        public IActionResult MainWindow()
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return (RedirectToAction("Index"));
            }
            if (Convert.ToInt32(_securityTools.GetUserFromAccessToken(token)) == 0)
            {
                return (RedirectToAction("Index"));
            }
            ViewBag.Message = BuildMessage(token, 50);
            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Message = "An error occured...";
            return View();
        }

        private string BuildMessage(string stringToSplit, int chunkSize)
        {
            var data = Enumerable.Range(0, stringToSplit.Length / chunkSize).Select(i => stringToSplit.Substring(i * chunkSize, chunkSize));
            string result = "The generated token is:";
            foreach (string str in data)
            {
                result += Environment.NewLine + str;
            }
            return result;
        }
    

    // GET: api/Users
    [HttpPost("GetUserByAccessToken")]
        public async Task<IActionResult> GetUserByAccessToken([FromBody] string accessToken)
        {
            Response response = new Response();
            int userId = _securityTools.GetUserFromAccessToken(accessToken);

            if (userId != 0)
            {
                var userResponse = await _securityRepository.GetLoginByUserId(userId);
                userResponse.AccessToken = accessToken;
                response.Success = 1;
                response.Data = userResponse;
                return Ok(response);
            }

            response.Success = 0;
            response.Message = "Token expirado";
            response.Data = null;
            return BadRequest(response);
        }
    }
}
