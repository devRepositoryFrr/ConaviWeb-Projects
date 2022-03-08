using ConaviWeb.Data.Repositories;
using ConaviWeb.Model.Request;
using ConaviWeb.Services;
using ConaviWeb.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Security
{
    public class UserController : Controller
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly ISecurityTools _securityTools;
        public UserController(ISecurityRepository securityRepository, ISecurityTools securityTools)
        {
            _securityRepository = securityRepository;
            _securityTools = securityTools;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Login/CreateUser");
        }

        public async Task<IActionResult> CreateUserAsync(UserRequest userRequest)
        {
            string spassword = _securityTools.GetSHA256(userRequest.Password);
            userRequest.Password = spassword;
            bool success = await _securityRepository.CreateUserSisevive(userRequest);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Error al procesar el archivo");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se ha enviado un correo electrónico con la información de su cuenta, una vez confirmado favor de ingresar con correo y contraseña");
                return RedirectToAction("Index");
            }
            
        }
    }
}
