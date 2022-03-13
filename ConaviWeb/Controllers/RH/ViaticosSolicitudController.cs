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
    public class ViaticosSolicitudController : Controller
    {
        private readonly IRHRepository _rHRepository;
        public ViaticosSolicitudController(IRHRepository rHRepository)
        {
            _rHRepository = rHRepository;
        }
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            TempData["nombre"] = user.Name;
            ViewBag.Nombre = TempData["nombre"];
            TempData["puesto"] = user.Cargo;
            ViewBag.Cargo = TempData["puesto"];
            TempData["area"] = user.Area;
            ViewBag.Area = TempData["area"];

            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../RH/ViaticosSolicitud");
        }
        [HttpPost]
        public async Task<IActionResult> GuardarSolicitudAsync([FromBody] Viaticos viaticos) {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            viaticos.IdUsuario = user.Id;
            viaticos.Folio = viaticos.IdUsuario + DateTime.Now.ToString("MMddyyyyHHmmss");
            viaticos.Periodo_comision_i = Convert.ToDateTime(viaticos.Periodo_comision_i).ToString("yyyy-MM-dd"); 
            viaticos.Periodo_comision_f = Convert.ToDateTime(viaticos.Periodo_comision_f).ToString("yyyy-MM-dd"); 
            viaticos.Fecha_salida = Convert.ToDateTime(viaticos.Fecha_salida).ToString("yyyy-MM-dd"); 
            viaticos.Fecha_regreso = Convert.ToDateTime(viaticos.Fecha_regreso).ToString("yyyy-MM-dd"); 

            var success = await _rHRepository.InsertViaticos(viaticos);
            if (!success)
            {
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la solicitud de viaticos");
                return Ok(alertJson);
            }
            var alert =  AlertService.ShowAlert(Alerts.Success, "Su solicitud de viaticos ha registrado con el folio " + viaticos.Folio);
            return Ok(alert);
        }
    }
}
