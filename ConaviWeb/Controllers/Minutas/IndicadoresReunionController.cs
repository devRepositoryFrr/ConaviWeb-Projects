using ConaviWeb.Data.Minuta;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    [Route("IndicadoresReunion")]
    public class IndicadoresReunionController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public IndicadoresReunionController(IMinutaRepository minutaRepository, IWebHostEnvironment environment)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var gestion = await _minutaRepository.GetGestion();
            ViewData["Gestion"] = gestion;
            return View("../Minuta/IndicadoresReunion");
        }
        [HttpGet("GetIndReunion")]
        public async Task<IActionResult> GetIndReunion(int id)
        {
            var indicadores = await _minutaRepository.GetIndReunion(id);

            if (indicadores != null)
            {
                return Json(new { data = indicadores });
            }

            return BadRequest();
        }
    }
}
