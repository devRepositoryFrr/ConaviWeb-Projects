using ConaviWeb.Data.Minuta;
using ConaviWeb.Model.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Minutas
{
    public class CapturaMinutaController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public CapturaMinutaController(IMinutaRepository minutaRepository, IWebHostEnvironment environment)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var reuniones = await _minutaRepository.GetReunion();
            var responsable = await _minutaRepository.GetResponsable();
            ViewData["Responsable"] = responsable;
            ViewData["Reuniones"] = reuniones;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Minuta/CapturaMinuta");
        }
        [HttpPost]
        public async Task<IActionResult> CreateMinutaAsync(Minuta minuta)
        {
            var success = await _minutaRepository.InsertMinuta(minuta);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la minuta");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se agrego la minuta con exito");
            return RedirectToAction("Index");

        }
    }
}
