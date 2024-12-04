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
    public class ReunionController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        public ReunionController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
        }
        [Route("DetalleReunion/{id?}")]
        public async Task<IActionResult> DetalleReunionAsync(int id)
        {
            var reunion = await _minutaRepository.GetReunionDetail(id);
            ViewData["Reunion"] = reunion;
            return View("../Minuta/Reunion");
        }
    }
}
