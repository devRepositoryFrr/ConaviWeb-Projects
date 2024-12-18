using ConaviWeb.Data.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConaviWeb.Model.Response;
using System.Linq;

namespace ConaviWeb.Controllers.Minutas
{
    [Route("ListaReuniones")]
    public class ListaReunionesController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        public ListaReunionesController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Minuta/ListaReuniones");
        }

        [HttpGet("GetReuniones")]
        public async Task<IActionResult> GetReuniones()
        {
            var reuniones = await _minutaRepository.GetReuniones();

            if (reuniones != null)
            {
                return Json(new { data = reuniones });
            }

            return BadRequest();
        }

    }
}
