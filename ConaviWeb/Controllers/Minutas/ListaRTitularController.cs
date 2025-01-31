using ConaviWeb.Data.Minuta;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    [Route("ListaRtitular")]
    public class ListaRTitularController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public ListaRTitularController(IMinutaRepository minutaRepository, IWebHostEnvironment environment)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Minuta/ListaRtitular");
        }

        [HttpGet("GetRtitular")]
        public async Task<IActionResult> GetRtitular()
        {
            var reuniones = await _minutaRepository.GetRTitulares();

            if (reuniones != null)
            {
                return Json(new { data = reuniones });
            }

            return BadRequest();
        }

    }
}
