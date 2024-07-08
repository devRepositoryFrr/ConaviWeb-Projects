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
            var cat = await _expedienteRepository.GetTiposSoporte();
            ViewData["Catalogo"] = cat;
            var catArea = await _expedienteRepository.GetAreas();
            ViewData["AreaCatalogo"] = catArea;
            var catTipoDoc = await _expedienteRepository.GetTiposDocumentales();
            ViewData["CatTipoDoc"] = catTipoDoc;
            /*List<Catalogo> catAnios = new List<Catalogo>();
            DateTime date1 = new DateTime();
            for (int i = 1900; i<=date1.Year; i++)
            {
                catAnios.Add(new Catalogo { i, i });
            }*/
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var id_inventario = await _expedienteRepository.GetIdInventarioBibliohemerografico(user.Area);
            ViewBag.IdInv = id_inventario;
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
        public async Task<IActionResult> InsertExpedienteBibliohemerografico(ExpedienteBibliohemerografico expediente)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente.IdUser = user.Id;

            var success = await _expedienteRepository.InsertExpedienteBibliohemerografico(expediente);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el expediente");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ExpedientesBibliohemerograficos()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var id_inventario = await _expedienteRepository.GetIdInventarioBibliohemerografico(user.Area);

            IEnumerable<ExpedienteBibliohemerografico> expedientes = new List<ExpedienteBibliohemerografico>();
            expedientes = await _expedienteRepository.GetExpedientesBibliohemerograficos(user.Id, id_inventario);
            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
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
    }
}
