using ConaviWeb.Data.Minuta;
using ConaviWeb.Model;
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
            IEnumerable<Catalogo> meses = await _minutaRepository.GetMeses();
            string [] mes = {"", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            foreach(Catalogo m in meses)
            {
                int num = Int32.Parse(m.Clave.Substring(4,2));
                int anio = Int32.Parse(m.Clave.Substring(0,4));
                m.Descripcion = String.Concat(mes[num]," - ",anio);
            }
            ViewData["Gestion"] = gestion;
            ViewData["Meses"] = meses;
            return View("../Minuta/IndicadoresAcuerdo");
        }
        [HttpGet("GetIndAcuerdo")]
        public async Task<IActionResult> GetIndAcuerdo(int id , string clave )
        {
            var indicadores = await _minutaRepository.GetIndAcuerdo(id, clave);
            string[] mes = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            
            if (indicadores != null)
            {
                return Json(new { data = indicadores });
            }

            return BadRequest();
        }
        [HttpGet("GetIndAcuerdoMes")]
        public async Task<IActionResult> GetIndAcuerdoMes(int id, string clave)
        {
            var indicadores = await _minutaRepository.GetIndAcuerdoMes(id, clave);
            string[] mes = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            foreach (var i in indicadores)
            {
                i.Mes = mes[i.NuMes];
            }
            if (indicadores != null)
            {
                return Json(new { data = indicadores });
            }

            return BadRequest();
        }
    }
}
