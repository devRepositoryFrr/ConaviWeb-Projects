using ConaviWeb.Data.RH;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.RH
{
    public class ContasSolicitudController : Controller
    {
        private readonly IRHRepository _rHRepository;
        private readonly IWebHostEnvironment _environment;
        public ContasSolicitudController(IRHRepository rHRepository, IWebHostEnvironment environment)
        {
            _rHRepository = rHRepository;
            _environment = environment;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../RH/ContasSolicitud");
        }
        [HttpGet]
        public async Task<IActionResult> GetSolicitudesAsync()
        {

            var success = await _rHRepository.GetSolicitudes(3);
            if (!success.Any())
            {
                return BadRequest();
            }
            return Json(new { data = success });
        }
        public async Task<IActionResult> VoBoContasAsync(int id)
        {
            var success = await _rHRepository.UpdateEstatus(id,4);
            if (!success)
            {
                TempData["Alert"]=AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al actualizar estatus");
                return RedirectToAction("Index");
            }
            TempData["Alert"]=AlertService.ShowAlert(Alerts.Success, "Se verifico la solicitud con exito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CancelarSolicitudAsync(string obs, int idReg)
        {
            
            var success = await _rHRepository.UpdateEstatus(idReg, 9, obs);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al actualizar estatus");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se actualizo la solicitud con exito");
            return RedirectToAction("Index");
        }
    }
}
