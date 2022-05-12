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

namespace ConaviWeb.Controllers.Sisevive
{
    public class SiseviveController : Controller
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly ISecurityTools _securityTools;
        public SiseviveController(ISecurityRepository securityRepository, ISecurityTools securityTools)
        {
            _securityRepository = securityRepository;
            _securityTools = securityTools;
        }
        public IActionResult Index()
        {
            return View("../Sisevive/Index");
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
                userResponse.Modules = await _securityRepository.GetModules(Convert.ToInt32(userResponse.Rol), userResponse.Id, userResponse.Sistema);
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
                return View("../Sisevive/Login");
            }

        }
    }
}
