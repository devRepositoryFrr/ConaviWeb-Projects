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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ConaviWeb.Controllers.Expedientes
{
    [Authorize]
    public class InventarioControlController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;
        //public IActionResult Index()
        //{
        //    return View("../Expedientes/InventarioAcervoDocumental");
        //}
        public InventarioControlController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var idUserArea = await _expedienteRepository.GetIdUserArea(user.Area);
            var idUserPuesto = await _expedienteRepository.GetIdUserPuesto(user.Cargo);
            var inventario = await _expedienteRepository.GetInventarioControl(user.Cargo);
            var catSoporte = await _expedienteRepository.GetTiposSoporte();
            ViewData["CatalogoSoporte"] = catSoporte;
            var catTipoDoc = await _expedienteRepository.GetTiposDocumentales();
            ViewData["CatTipoDoc"] = catTipoDoc;
            var cat = await _expedienteRepository.GetCodigosExp();
            ViewData["Catalogo"] = cat;
            ViewBag.NombreResponsable = inventario != null ? inventario.NombreResponsableAT : "";
            ViewBag.IdInv = inventario!=null ? inventario.Id : 0;
            ViewBag.FechaElab = inventario!=null ? inventario.FechaElaboracion : "";
            ViewBag.FechaEnt = inventario!=null ? inventario.FechaEntrega : "";
            ViewBag.Ubicacion = inventario!=null ? inventario.Ubicacion : "";
            ViewBag.Peso = inventario!=null ? inventario.PesoElectronico : 0;
            ViewBag.Almacenamiento = inventario!=null ? inventario.Almacenamiento : "";
            int rol = (int) user.Rol;
            //if (user.Id == 212 || user.Id == 323)
            if (rol == 15)
            {
                var catArea = await _expedienteRepository.GetPuestosLista();
                ViewBag.AreaCatalogo = (new SelectList(catArea, "IdPuesto", "Puesto", idUserPuesto));
                //ViewData["btnShowValidacion"] = true;
            }
            else
            {
                var catArea = await _expedienteRepository.GetPuestoUser(idUserPuesto);
                ViewBag.AreaCatalogo = new SelectList(catArea, "IdPuesto", "Puesto", idUserPuesto);
                //ViewData["btnShowValidacion"] = false;
            }
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/InventarioControl");
        }
        public IActionResult ICFormato()
        {
            return View("../Expedientes/InventarioControlFto");
        }
        [HttpPost]
        public async Task<IActionResult> InsertInventarioControl(Inventario inventario)
        {
            var success = await _expedienteRepository.InsertInventarioControl(inventario);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el inventario");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> InsertExpedienteInventarioControl(Expediente expediente)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente.IdUser = user.Id;

            var success = false;
            if(expediente.Id == 0)
            {
                success = await _expedienteRepository.InsertExpedienteInventarioControl(expediente);
            }
            else
            {
                success = await _expedienteRepository.UpdateExpedienteInventarioControl(expediente);
            }
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el expediente");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ExpedientesControl()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var inventario = await _expedienteRepository.GetInventarioControl(user.Cargo);

            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesInventarioControl(user.Id, inventario!=null ? inventario.Id : 0);

            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedientesControlByIdInv([FromForm] int id)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            IEnumerable<Expediente> expedientes = new List<Expediente>();
            //expedientes = await _expedienteRepository.GetExpedientesInventarioControlByIdInv(id);
            if (id != 0)
            {
                expedientes = await _expedienteRepository.GetExpedientesInventarioControl(user.Id, id);
            }

            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetInventarioControl([FromForm] string puesto)
        {
            Inventario inventario = new();
            inventario = await _expedienteRepository.GetInventarioControl(puesto);
            if (inventario == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de inventario no encontrado");
                return Ok(alert);
            }
            return Ok(inventario);
        }
        [HttpPost]
        public async Task<IActionResult> GetInventarioControlById([FromForm] int id)
        {
            Inventario inventario = new();
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            inventario = await _expedienteRepository.GetInventarioControlById(id);
            if (inventario == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de inventario no encontrado");
                return Ok(alert);
            }
            return Ok(inventario);
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedienteControl([FromForm] int id)
        {
            Expediente expediente = new();
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            expediente = await _expedienteRepository.GetExpedienteControl(id);
            if (expediente == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
            expediente.UserName = user.Name;
            return Ok(expediente);
        }
        [HttpPost]
        public async Task<IActionResult> DropExpediente(Expediente expediente)
        {
            var success = await _expedienteRepository.DropExpedienteControl(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al eliminar el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se eliminó el expediente con éxito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SendValExpedienteControl(Expediente expediente)
        {
            var success = await _expedienteRepository.SendValExpedienteControl(expediente.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el expediente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se envió el expediente a revisión con exito");
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public async Task<IActionResult> SendRevalExpedienteControl(Expediente expediente)
        //{
        //    var success = await _expedienteRepository.RevalidacionExpedienteControl(expediente.Id);
        //    if (!success)
        //    {
        //        TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al enviar el expediente");
        //        return RedirectToAction("Index");
        //    }
        //    TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se envió el expediente a revalidación con exito");
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> MigrarExpedienteControlInvTP(Expediente expediente)
        {
            var sepuedemigrar = await _expedienteRepository.sePuedeMigrarExpediente(expediente.Id, 1);
            if (sepuedemigrar > 0)
            {
                var success = await _expedienteRepository.MigrarExpedienteInvTP(expediente.Id);
                if (!success)
                {
                    TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al migrar el expediente");
                    return RedirectToAction("Index");
                }
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se migró el expediente al Inventario de Transferencia Primaria con exito");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Necesitas editar los campos de Años de resguardo y número de Fojas, para poder migrar el expediente!");
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> MigrarExpedienteControlInvNE(Expediente expediente)
        {
            var sepuedemigrar = await _expedienteRepository.sePuedeMigrarExpediente(expediente.Id, 2);
            if (sepuedemigrar > 0) {
                var success = await _expedienteRepository.MigrarExpedienteInvNE(expediente.Id);
                if (!success)
                {
                    TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al migrar el expediente");
                    return RedirectToAction("Index");
                }
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se migró el expediente al Inventario de Documentación No Expedientable con exito!");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Necesitas editar los campos necesarios para poder migrar el expediente!");
                return RedirectToAction("Index");
            }
        }
    }
}