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
    public class InventarioValidacionController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;

        public InventarioValidacionController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var catArea = await _expedienteRepository.GetAreas();
            ViewData["AreaCatalogo"] = catArea;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventario(user.Area);
            //ViewBag.IdInv = id_inventario;
            if (user.Id == 212 || user.Id == 323)
                ViewData["btnShowValidacion"] = true;
            else
                ViewData["btnShowValidacion"] = false;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/InventarioValidacion");
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesBiblio([FromHeader] int slcArea)
        {
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventario(user.Area);

            IEnumerable<ExpedienteBibliohemerografico> expedientes = await _expedienteRepository.GetExpedientesValidacionBiblio(slcArea);
            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesControl([FromHeader] int slcArea)
        {
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventarioControl(user.Area);

            IEnumerable<Expediente> expedientes = await _expedienteRepository.GetExpedientesValidacionInventarioControl(slcArea);

            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> VoBoExpedienteBiblio(int idExp)
        {
            var success = await _expedienteRepository.VoBoExpedienteBiblio(idExp);
            if (!success)
            {
                var alerJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el VoBo del expediente bibliohemerográfico");
                return Ok(alerJson);
            }
            var alert = AlertService.ShowAlert(Alerts.Success, "Se dió el VoBo al expediente bibliohemerográfico con éxito");
            return Ok(alert);
        }
        [HttpPost]
        public async Task<IActionResult> RevalidacionExpedienteBiblio(int idExp)
        {
            var success = await _expedienteRepository.RevalidacionExpedienteBiblio(idExp);
            if (!success)
            {
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar a revalidación el expediente bibliohemerográfico");
                return Ok(alertJson);
            }
            var alert = AlertService.ShowAlert(Alerts.Success, "Se envió a revalidación el expediente bibliohemerográfico con exito");
            return Ok(alert);
        }
        [HttpPost]
        public async Task<IActionResult> VoBoExpedienteControl(int idExp)
        {
            var success = await _expedienteRepository.VoBoExpedienteControl(idExp);
            if (!success)
            {
                //TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el VoBo del expediente");
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el VoBo del expediente");
                return Ok(alertJson);
            }
            //TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se dió el VoBo al expediente con exito");
            var alert = AlertService.ShowAlert(Alerts.Success, "Se dió el VoBo al expediente con éxito");
            return Ok(alert);
        }
        [HttpPost]
        public async Task<IActionResult> RevalidacionExpedienteControl(int idExp)
        {
            var success = await _expedienteRepository.RevalidacionExpedienteControl(idExp);
            if (!success)
            {
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar a revalidación el expediente");
                return Ok(alertJson);
            }
            var alert = AlertService.ShowAlert(Alerts.Success, "Se envió a revalidación el expediente con exito");
            return Ok(alert);
        }
    }
}