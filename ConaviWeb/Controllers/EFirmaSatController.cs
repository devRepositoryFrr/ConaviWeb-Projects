using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("EFirmaSat")]
    public class EFirmaSatController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepository _userRepository;
        private readonly IProcessSignRepository _processSignRepository;
        private readonly IProcessSigningService _processSigningService;
        public EFirmaSatController(IWebHostEnvironment environment, IUserRepository userRepository, IProcessSignRepository processSignRepository, IProcessSigningService processSigningService)
        {
            _environment = environment;
            _userRepository = userRepository;
            _processSignRepository = processSignRepository;
            _processSigningService = processSigningService;
        }
        public IActionResult Index()
        {
            return View("../EFirma/EFirmaSat");
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            //Send to the Index Action method which will prevent resubmission.
            return RedirectToAction("Index");
        }

        [Route("/EFirmaSat/SignFiles", Name = "Register")]
        public async Task<IActionResult> SignFiles([FromForm] DataSignRequest dataSignRequest)
        {
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<FileResponse> files;
            bool success = false;
            //if (user.Rol.ToString() == "FirmanteInterno")
            //{
                files = await _processSignRepository.GetFilesForSign(user.IdSystem, dataSignRequest.ArrayFiles);
            //}
            //else
            //{
            //    files = await _processSignRepository.GetExternalFiles(user.Integrador);
            //}
            if (files.Any())
            {
                success = await _processSigningService.ProcessFileSatAsync(user, dataSignRequest, files);
            }
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Error al firmar " + files.Count() + " archivos.");
                //return View("../EFirma/Lista");
                return RedirectToAction("List","Lista");
            }

            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se firmo " + files.Count() + " archivos.");
            //return View("../EFirma/Lista");
            return RedirectToAction("List", "Lista");
        }


    }
}
