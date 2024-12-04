using ConaviWeb.Commons;
using ConaviWeb.Data.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Mvc;
using static ConaviWeb.Models.AlertsViewModel;
using System.Threading.Tasks;
using ConaviWeb.Model.Minuta;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using ConaviWeb.Model;
using System;

namespace ConaviWeb.Controllers.Minutas
{
    public class MinutaSedatuController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        public MinutaSedatuController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //var participantes = await _minutaRepository.GetParticipantes();
            //ViewData["Participantes"] = participantes;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Minuta/Minuta");
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateMinutaAsync(Minuta minuta)
        //{
        //    //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
        //    //viaticos.IdUsuario = user.Id;
        //    var date = DateTime.Now.ToString("ddMMyyyyss");
        //    minuta.Folio = string.Concat("MI-", date);
        //    var IdMinuta = await _minutaRepository.InsertMinuta(minuta);
        //    foreach (var item in minuta.Participantes)
        //    {
        //        item.IdMinuta = IdMinuta;  
        //    }
        //    var PSuccess = await _minutaRepository.InsertParticipantes(minuta.Participantes);
        //    foreach (var item in minuta.Acuerdos)
        //    {
        //        item.IdMinuta = IdMinuta;
        //    }
        //    var ASuccess = await _minutaRepository.InsertAcuerdos(minuta.Acuerdos);


        //    if (!PSuccess && !ASuccess)
        //    {
        //        TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la minuta");
        //        return RedirectToAction("Index");
        //    }
        //    TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se agrego la minuta con exito");
        //    return RedirectToAction("Index");

        //}
        //[HttpGet]
        //public async Task<IActionResult> GetParticipantesAsync() {
        //    var participantes = await _minutaRepository.GetParticipantes();
        //    return Json(new { data = participantes });
        //        }
    }
}
