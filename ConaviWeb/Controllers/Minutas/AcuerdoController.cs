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
    public class AcuerdoController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        public AcuerdoController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
        }
        [Route("DetalleAcuerdo/{id?}")]
        public async Task<IActionResult> DetalleAcuerdoAsync(int id)
        {
            var acuerdo = await _minutaRepository.GetAcuerdoDetail(id);
            ViewData["Acuerdo"] = acuerdo;
            return View("../Minuta/Acuerdo");
        }
    }
}
