using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Commons;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;
using ConaviWeb.Model.Response;

namespace ConaviWeb.Controllers.Expedientes
{
    public class CaratulaControlController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;

        public CaratulaControlController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/CaratulaControl");
        }
        [HttpPost]
        public async Task<IActionResult> InsertCaratulaExpedienteIC(Caratula caratula)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            caratula.IdUser = user.Id;

            var success = await _expedienteRepository.InsertCaratulaExpedienteIC(caratula);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la carátula");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Registro de carátula exitoso!");
            return Redirect("/CaratulaControl?id=" + caratula.IdExpediente);
        }
        [HttpPost]
        public async Task<IActionResult> GetCaratulaExpedienteControl([FromForm] int id, int legajo)
        {
            Caratula caratula = new();
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            caratula = await _expedienteRepository.GetCaratulaExpedienteControl(id, legajo, user.Id);
            if (caratula == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
            if(caratula.UserName == null)
                caratula.UserName = user.Name;
            return Ok(caratula);
        }
    }
}
