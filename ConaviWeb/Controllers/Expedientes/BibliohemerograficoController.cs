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
using ConaviWeb.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConaviWeb.Controllers.Expedientes
{
    public class BibliohemerograficoController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;
        public BibliohemerograficoController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var idUserArea = await _expedienteRepository.GetIdUserArea(user.Area);
            var inventario = await _expedienteRepository.GetInventarioBibliohemerografico(user.Area);
            var cat = await _expedienteRepository.GetTiposSoporte();
            ViewData["Catalogo"] = cat;
            var catArea = await _expedienteRepository.GetAreas();
            //ViewData["AreaCatalogo"] = catArea;
            ViewBag.AreaCatalogo = (new SelectList(catArea, "Id", "Clave", idUserArea));
            ViewBag.NombreResponsable = inventario != null ? inventario.NombreResponsableAT : "";
            ViewBag.IdInv = inventario != null ? inventario.Id : 0;
            ViewBag.FechaElab = inventario != null ? inventario.FechaElaboracion.ToString("dd/MM/yyyy") : "";
            ViewBag.FechaTrans = inventario != null ? inventario.FechaTransferencia?.ToString("dd/MM/yyyy") : "";
            if (user.Id == 212 || user.Id == 323)
                ViewData["btnShowValidacion"] = true;
            else
                ViewData["btnShowValidacion"] = false;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/Bibliohemerografico");
        }
        [HttpPost]
        public async Task<IActionResult> InsertInventarioBibliohemerografico(Inventario inventario)
        {
            var success = await _expedienteRepository.InsertInventarioBibliohemerografico(inventario);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el inventario");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetInventarioBiblioById([FromForm] int id)
        {
            Inventario inventario = new();
            inventario = await _expedienteRepository.GetInventarioBiblioById(id);
            if (inventario == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de inventario no encontrado");
                return Ok(alert);
            }
            return Ok(inventario);
        }
        [HttpPost]
        public async Task<IActionResult> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente.IdUser = user.Id;

            var success = false;
            if (expediente.Id == 0)
            {
                success = await _expedienteRepository.InsertExpedienteBibliohemerografico(expediente);
            }
            else
            {
                success = await _expedienteRepository.UpdateExpedienteBibliohemerografico(expediente);
            }
    
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al guardar el expediente");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ExpedientesBibliohemerograficos()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var inventario = await _expedienteRepository.GetInventarioBibliohemerografico(user.Area);

            IEnumerable<ExpedienteBibliohemerografico> expedientes = new List<ExpedienteBibliohemerografico>();
            if(inventario != null)
            {
                expedientes = await _expedienteRepository.GetExpedientesBibliohemerograficos(user.Id, inventario.Id);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesBiblioByIdInv([FromForm] int id)
        {
            IEnumerable<ExpedienteBibliohemerografico> expedientes = new List<ExpedienteBibliohemerografico>();
            if(id != 0)
            {
                expedientes = await _expedienteRepository.GetExpedientesBibliohemerograficosByIdInv(id);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> DropExpediente(ExpedienteBibliohemerografico expediente)
        {
            var success = await _expedienteRepository.DropExpedienteBibliohemerografico(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al eliminar el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se eliminó el expediente con exito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedienteBibliohemerografico([FromForm] int id)
        {
            //ExpedienteBibliohemerografico expediente = new();
            ExpedienteBibliohemerografico expediente = await _expedienteRepository.GetExpedienteBibliohemerografico(id);
            //expediente = await _expedienteRepository.GetExpedienteBibliohemerografico(id);
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            if (expediente == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
            expediente.UserName = user.Name;
            return Ok(expediente);
        }
        //[HttpPost]
        //public async Task<IActionResult> SendValExpedienteBiblio(ExpedienteBibliohemerografico expediente)
        //{
        //    var success = await _expedienteRepository.SendValExpedienteBiblio(expediente.Id);
        //    if (!success)
        //    {
        //        TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el expediente bibliohemerográfico");
        //        return RedirectToAction("Index");
        //    }
        //    TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se envió el expediente bibliohemerográfico a revisión con exito");
        //    return RedirectToAction("Index");
        //}
    }
}
