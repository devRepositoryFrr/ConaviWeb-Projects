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
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var idUserArea = await _expedienteRepository.GetIdUserArea(user.Area);
            var inventario = await _expedienteRepository.GetInventarioControl(user.Area);
            ViewBag.IdInv = inventario != null ? inventario.Id : 0;
            var cat = await _expedienteRepository.GetTiposSoporte();
            ViewData["Catalogo"] = cat;
            var catTipoDoc = await _expedienteRepository.GetTiposDocumentales();
            ViewData["CatTipoDoc"] = catTipoDoc;
            var catArea = await _expedienteRepository.GetAreas();
            ViewBag.AreaCatalogo = new SelectList(catArea, "Id", "Clave", idUserArea);
            var catClave = await _expedienteRepository.GetCodigosExp();
            ViewData["ClaveInterna"] = catClave;
            ViewBag.NombreR = inventario != null ? inventario.NombreResponsableAT : "";
            //ViewBag.FechaElab = inventario != null ? inventario.FechaElaboracion.ToString("dd/MM/yyyy") : "";
            ViewBag.FechaElab = inventario != null ? inventario.FechaElaboracion : "";
            ViewBag.FechaTrans = inventario != null ? inventario.FechaTransferencia : "";
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
        //[HttpPost]
        //public async Task<IActionResult> InsertExpedienteNoExpedientable(ExpedienteNoExpedientable expediente)
        //{
        //    var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
        //    expediente.IdUser = user.Id;

        //    var success = await _expedienteRepository.InsertExpedienteNoExpedientable(expediente);
        //    if (!success)
        //    {
        //        TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el expediente");
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}
        [HttpGet]
        public async Task<IActionResult> ExpedientesNoExpedientables()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var inventario = await _expedienteRepository.GetInventarioControl(user.Area);

            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesNoExpedientables(user.Id, inventario!=null ? inventario.Id : 0);
            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesNoExpedientablesByIdInv([FromForm] int id)
        {
            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesNoExpedientablesByIdInv(id);
            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetNoExpedientable([FromForm] int id)
        {
            Expediente expediente = new();
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente = await _expedienteRepository.GetNoExpedientable(id);
            if (expediente == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
            expediente.UserName = user.Name;
            return Ok(expediente);
        }
        [HttpPost]
        public async Task<IActionResult> GetCaratulaNoExpedientable([FromForm] int id, int legajo)
        {
            //Caratula caratula = new();
            Caratula caratula = await _expedienteRepository.GetCaratulaNoExpedientable(id, legajo);
            //caratula = await _expedienteRepository.GetCaratulaNoExpedientable(id);
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            if (caratula == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
            caratula.UserName = user.Name;
            return Ok(caratula);
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
