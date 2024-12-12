using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Data.Levantamientos;
using ConaviWeb.Model;

namespace ConaviWeb.Controllers.Levantamiento
{
    public class LevantamientoController : Controller
    {
        private readonly ILevantamientoRepository _levantamientoRepository;
        public LevantamientoController(ILevantamientoRepository levantamientoRepository)
        {
            _levantamientoRepository = levantamientoRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var catEstados = await _levantamientoRepository.GetEstados();
            ViewBag.EstadoCatalogo = (new SelectList(catEstados, "Clave", "Descripcion"));
            return View("../Levantamiento/FormatoHomologacion");
        }
        public IActionResult HomologacionFiles()
        {
            return View("../Levantamiento/ArchivosHomologacion");
        }
        [HttpPost]
        public async Task<IActionResult> GetMunicipios(string cveedo)
        {
            IEnumerable<Catalogo> municipios = new List<Catalogo>();
            municipios = await _levantamientoRepository.GetMunicipios(cveedo);
            //if (municipios == null)
            //{
            //    var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
            //    return Ok(alert);
            //}
            return Json(new { data = municipios });
        }
        [HttpPost]
        public async Task<IActionResult> GetLocalidades(string cveedo, string cvemun)
        {
            IEnumerable<Catalogo> localidades = new List<Catalogo>();
            localidades = await _levantamientoRepository.GetLocalidades(cveedo, cvemun);
            //if (municipios == null)
            //{
            //    var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
            //    return Ok(alert);
            //}
            return Json(new { data = localidades });
        }
    }
}
