using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Commons;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Expedientes
{
    public class NoExpedientableController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;
        public NoExpedientableController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var cat = await _expedienteRepository.GetTiposSoporte();
            ViewData["Catalogo"] = cat;
            var catArea = await _expedienteRepository.GetAreas();
            ViewData["AreaCatalogo"] = catArea;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var id_inventario = await _expedienteRepository.GetIdInventarioNoExpedientable(user.Area);
            ViewBag.IdInv = id_inventario;
            if (user.Id == 212 || user.Id == 323)
                ViewData["btnShowValidacion"] = true;
            else
                ViewData["btnShowValidacion"] = false;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/NoExpedientable");
        }
        [HttpPost]
        public async Task<IActionResult> InsertInventarioNoExpedientable(Inventario inventario)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");

            var success = await _expedienteRepository.InsertInventarioNoExpedientable(inventario);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el inventario");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente.IdUser = user.Id;

            var success = await _expedienteRepository.InsertExpedienteNoExpedientable(expediente);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el expediente");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ExpedientesNoExpedientables()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var id_inventario = await _expedienteRepository.GetIdInventarioNoExpedientable(user.Area);

            IEnumerable<ExpedienteNoExpedientable> expedientes = new List<ExpedienteNoExpedientable>();
            expedientes = await _expedienteRepository.GetExpedientesNoExpedientables(user.Id, id_inventario);
            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> DropExpediente(ExpedienteNoExpedientable expediente)
        {
            var success = await _expedienteRepository.DropExpedienteNoExpedientable(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al eliminar el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se eliminó el expediente con éxito");
            return RedirectToAction("Index");
        }
    }
}
