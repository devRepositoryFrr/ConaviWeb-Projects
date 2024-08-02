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
        public IActionResult Areas()
        {
            return View("../Expedientes/Areas");
        }
        [HttpGet]
        public async Task<IActionResult> ListaAjax()
        {
            //IEnumerable<SerieDocumental> series = new List<SerieDocumental>();
            IEnumerable<SerieDocumental> series = await _expedientesRepository.GetSeries();
            //series = await _expedientesRepository.GetSeries();
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
            //SerieDocumental serie = new();
            SerieDocumental serie = await _expedientesRepository.GetSerieDocumental(id);
            //serie = await _expedientesRepository.GetSerieDocumental(id);
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
            //SerieDocumental serie = new();
            SerieDocumental serie = await _expedientesRepository.GetSerieDocumental(id);
            //serie = await _expedientesRepository.GetSerieDocumental(id);
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
        [HttpGet]
        public async Task<IActionResult> ListaAreas()
        {
            //IEnumerable<Area> areas = new List<Area>();
            IEnumerable<Area> areas = await _expedientesRepository.GetAreasLista();
            //areas = await _expedientesRepository.GetAreasLista();
            if (areas == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = areas });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateArea(Area area)
        {
            var success = await _expedientesRepository.UpdateArea(area);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al editar el área");
                return RedirectToAction("Areas");
            }
            return RedirectToAction("Areas");
        }
        [HttpPost]
        public async Task<IActionResult> GetAreaAsync([FromForm] int id)
        {
            //Area area = new();
            Area area = await _expedientesRepository.GetArea(id);
            //area = await _expedientesRepository.GetArea(id);
            if (area == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id no encontrado");
                return Ok(alert);
            }
            return Ok(area);
        }
    }
}
