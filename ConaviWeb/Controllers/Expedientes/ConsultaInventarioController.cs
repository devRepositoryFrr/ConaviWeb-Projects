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
    public class ConsultaInventarioController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;

        public ConsultaInventarioController(IExpedienteRepository expedienteRepository)
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
            return View("../Expedientes/ConsultaInventario");
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesTP([FromHeader] int slcArea)
        {
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var id_inventario = await _expedienteRepository.GetIdInventario(user.Area);

            IEnumerable<Expediente> expedientes = await _expedienteRepository.GetExpedientesValidacionTP(slcArea);
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
    }
}