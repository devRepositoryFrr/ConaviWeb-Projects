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
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventario(user.Area);
            //ViewBag.IdInv = id_inventario;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/InventarioValidacion");
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesTP([FromHeader] int slcArea)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventario(user.Area);

            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesValidacionTP(slcArea);
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

            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesValidacionInventarioControl(slcArea);

            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> VoBoExpedienteTP(Expediente expediente)
        {
            var success = await _expedienteRepository.VoBoExpediente(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el VoBo del expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se dió el VoBo al expediente con exito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RevalidacionExpedienteTP(Expediente expediente)
        {
            var success = await _expedienteRepository.RevalidacionExpediente(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar a revalidación el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se envió a revalidación el expediente con exito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> VoBoExpedienteControl(Expediente expediente)
        {
            var success = await _expedienteRepository.VoBoExpedienteControl(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el VoBo del expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se dió el VoBo al expediente con exito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RevalidacionExpedienteControl(Expediente expediente)
        {
            var success = await _expedienteRepository.RevalidacionExpedienteControl(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar a revalidación el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se envió a revalidación el expediente con exito");
            return RedirectToAction("Index");
        }
    }
}