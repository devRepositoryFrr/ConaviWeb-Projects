﻿using Microsoft.AspNetCore.Mvc;
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
            var cat = await _expedienteRepository.GetCodigosExp();
            ViewData["Catalogo"] = cat;
            var catArea = await _expedienteRepository.GetAreas();
            ViewData["AreaCatalogo"] = catArea;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var id_inventario = await _expedienteRepository.GetIdInventarioControl(user.Area);
            ViewBag.IdInv = id_inventario;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Expedientes/InventarioControl");
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

            var success = await _expedienteRepository.InsertExpedienteInventarioControl(expediente);
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
            var id_inventario = await _expedienteRepository.GetIdInventarioControl(user.Area);

            IEnumerable<Expediente> expedientes = new List<Expediente>();
            expedientes = await _expedienteRepository.GetExpedientesInventarioControl(user.Id, id_inventario);

            if (expedientes == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = expedientes });
        }
        [HttpPost]
        public async Task<IActionResult> GetExpedienteControl([FromForm] int id)
        {
            Expediente expediente = new();
            expediente = await _expedienteRepository.GetExpedienteControl(id);
            if (expediente == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de expediente no encontrado");
                return Ok(alert);
            }
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
        [HttpPost]
        public async Task<IActionResult> VoBoExpediente(Expediente expediente)
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
        public async Task<IActionResult> RevalidacionExpediente(Expediente expediente)
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