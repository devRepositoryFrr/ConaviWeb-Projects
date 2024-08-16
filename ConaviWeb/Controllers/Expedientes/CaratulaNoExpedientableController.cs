using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;
using ConaviWeb.Model.Response;
using ConaviWeb.Commons;

namespace ConaviWeb.Controllers.Expedientes
{
    public class CaratulaNoExpedientableController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;

        public CaratulaNoExpedientableController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public IActionResult Index()
        {
            return View("../Expedientes/CaratulaNoExpedientable");
        }
        //[HttpPost]
        //public async Task<IActionResult> InsertCaratulaNoExpedientable(Caratula caratula)
        //{
        //    var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
        //    caratula.IdUser = user.Id;

        //    var success = await _expedienteRepository.InsertCaratulaNoExpedientable(caratula);
        //    if (!success)
        //    {
        //        TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la carátula");
        //        return RedirectToAction("Index");
        //    }
        //    //return RedirectToAction("Index");
        //    return Redirect("/CaratulaNoExpedientable?id=" + caratula.IdExpediente);
        //}
    }
}
