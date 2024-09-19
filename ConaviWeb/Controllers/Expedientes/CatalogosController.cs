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
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/Catalogos");
        }
        public IActionResult Areas()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/Areas");
        }
        public IActionResult Puestos()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/Puestos");
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
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al editar la serie documental");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ActivarSerieDoc(SerieDocumental serie)
        {
            var success = await _expedientesRepository.ActivarSerieDocCat(serie.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al activar la serie documental");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DesactivarSerieDoc(SerieDocumental serie)
        {
            var success = await _expedientesRepository.DesactivarSerieDocCat(serie.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al desactivar la serie documental");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ActivarArea(Area area)
        {
            var success = await _expedientesRepository.ActivarArea(area.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al activar el área");
                return RedirectToAction("Areas");
            }
            return RedirectToAction("Areas");
        }
        [HttpPost]
        public async Task<IActionResult> DesactivarArea(Area area)
        {
            var success = await _expedientesRepository.DesactivarArea(area.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al desactivar el área");
                return RedirectToAction("Areas");
            }
            return RedirectToAction("Areas");
        }
        [HttpGet]
        public async Task<IActionResult> ListaAreas()
        {
            IEnumerable<Area> areas = await _expedientesRepository.GetAreasLista();
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
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error en el registro del área");
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
        [HttpPost]
        public async Task<IActionResult> ActivarPuesto(Area puesto)
        {
            var success = await _expedientesRepository.ActivarPuesto(puesto.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al activar el puesto");
                return RedirectToAction("Puestos");
            }
            return RedirectToAction("Puestos");
        }
        [HttpPost]
        public async Task<IActionResult> DesactivarPuesto(Area puesto)
        {
            var success = await _expedientesRepository.DesactivarPuesto(puesto.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error al desactivar el puesto");
                return RedirectToAction("Puestos");
            }
            return RedirectToAction("Puestos");
        }
        [HttpGet]
        public async Task<IActionResult> ListaPuestos()
        {
            IEnumerable<Area> puestos = await _expedientesRepository.GetPuestosLista();
            if (puestos == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = puestos });
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePuesto(Area puesto)
        {
            var success = await _expedientesRepository.UpdatePuesto(puesto);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrió un error en el registro del puesto");
                return RedirectToAction("Puestos");
            }
            return RedirectToAction("Puestos");
        }
        [HttpPost]
        public async Task<IActionResult> GetPuestoAsync([FromForm] int id)
        {
            //Area area = new();
            Area puesto = await _expedientesRepository.GetPuesto(id);
            //area = await _expedientesRepository.GetArea(id);
            if (puesto == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id no encontrado");
                return Ok(alert);
            }
            return Ok(puesto);
        }
    }
}
