using ConaviWeb.Data.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    [Route("ListaAcuerdos")]
    public class ListaAcuerdosController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        public ListaAcuerdosController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return View("../Minuta/ListaAcuerdos");
        }
        [HttpGet("GetAcuerdos")]
        public async Task<IActionResult> GetAcuerdos()
        {
            var acuerdos = await _minutaRepository.GetAcuerdos();

            if (acuerdos != null)
            {
                return Json(new { data = acuerdos });
            }

            return BadRequest();
        }
    }
}
