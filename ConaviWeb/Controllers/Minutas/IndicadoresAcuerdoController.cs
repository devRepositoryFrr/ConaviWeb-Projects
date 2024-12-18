using ConaviWeb.Data.Minuta;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    [Route("IndicadoresAcuerdo")]
    public class IndicadoresAcuerdoController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public IndicadoresAcuerdoController(IMinutaRepository minutaRepository, IWebHostEnvironment environment)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var gestion = await _minutaRepository.GetGestion();
            ViewData["Gestion"] = gestion;
            return View("../Minuta/IndicadoresAcuerdo");
        }
        [HttpGet("GetIndAcuerdo")]
        public async Task<IActionResult> GetIndAcuerdo(int id)
        {
            var indicadores = await _minutaRepository.GetIndAcuerdo(id);

            if (indicadores != null)
            {
                return Json(new { data = indicadores });
            }

            return BadRequest();
        }
    }
}
