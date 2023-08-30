using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;
using ConaviWeb.Model.Request;

namespace ConaviWeb.Controllers.Expedientes
{
    public class CatalogosController : Controller
    {
        private readonly IExpedienteRepository _expedientesRepository;
        public CatalogosController(IExpedienteRepository expedientesRepository)
        {
            _expedientesRepository = expedientesRepository;
        }
        public IActionResult Index()
        {
            return View("../Expedientes/Catalogos");
        }
        [HttpGet]
        public async Task<IActionResult> ListaAjax()
        {
            IEnumerable<SerieDocumental> series = new List<SerieDocumental>();
            series = await _expedientesRepository.GetSeries();
            if (series == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = series });
        }
        [HttpPost]
        public async Task<IActionResult> GetSerieDocCatAsync([FromForm] int id)
        {
            SerieDocumental serie = new();
            serie = await _expedientesRepository.GetSerieDocumental(id);
            if (serie == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id no válido");
                return Ok(alert);
            }
            return Ok(serie);
        }
        [HttpPost]
        public async Task<IActionResult> GetSerieDocumentalAsync([FromForm] int id)
        {
            SerieDocumental serie = new();
            serie = await _expedientesRepository.GetSerieDocumental(id);
            if (serie == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id no encontrado");
                return Ok(alert);
            }
            return Ok(serie);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSerieDocCat(SerieDocumental serie)
        {
            var success = await _expedientesRepository.UpdateSerieDocCat(serie);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al editar la serie documental");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
