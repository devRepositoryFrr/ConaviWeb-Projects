using ConaviWeb.Commons;
using ConaviWeb.Data.RH;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.RH;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.RH
{
    public class AdminSolicitudController : Controller
    {
        private readonly IRHRepository _rHRepository;
        public AdminSolicitudController(IRHRepository rHRepository)
        {
            _rHRepository = rHRepository;
        }
        public IActionResult Index()
        {
            return View("../RH/AdminSolicitud");
        }
        [HttpGet]
        public async Task<IActionResult> GetSolicitudesAsync()
        {

            var success = await _rHRepository.GetSolicitudes();
            if (!success.Any())
            {
                return BadRequest();
            }
            return Json(new { data = success });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSolicitudAsync([FromBody] Viaticos viaticos)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            viaticos.IdUsuario = user.Id;

            var success = await _rHRepository.UpdateViaticos(viaticos);
            if (!success)
            {
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar los datos de vuelo");
                return Ok(alertJson);
            }
            var alert = AlertService.ShowAlert(Alerts.Success, "Se registraron los vuelos con exito");
            return Ok(alert);

        }

    }
}
