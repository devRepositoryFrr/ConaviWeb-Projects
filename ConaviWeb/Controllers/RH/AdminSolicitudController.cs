using ConaviWeb.Data.RH;
using ConaviWeb.Model.RH;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
