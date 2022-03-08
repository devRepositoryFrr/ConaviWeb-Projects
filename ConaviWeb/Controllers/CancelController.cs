using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("CancelaSat")]
    public class CancelController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepository _userRepository;
        private readonly IProcessSignRepository _processSignRepository;
        private readonly IProcessCancelService _processCancelService;
        public CancelController(IWebHostEnvironment environment, IUserRepository userRepository, IProcessSignRepository processSignRepository, IProcessCancelService processCancelService)
        {
            _environment = environment;
            _userRepository = userRepository;
            _processSignRepository = processSignRepository;
            _processCancelService = processCancelService;
        }
        public IActionResult Index()
        {
            return View("../EFirma/CancelaSat");
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            //Send to the Index Action method which will prevent resubmission.
            return RedirectToAction("Index");
        }

        [Route("/Cancel/CancelFiles", Name = "Cancel")]
        public async Task<IActionResult> CancelFiles([FromForm] DataSignRequest dataSignRequest)
        {
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<FileResponse> files;
            bool success = false;
            //if (user.Rol.ToString() == "FirmanteInterno")
            //{
            files = await _processSignRepository.GetFilesForCancel(user.IdSystem, dataSignRequest.ArrayFiles);
            //}
            //else
            //{
            //    files = await _processSignRepository.GetExternalFiles(user.Integrador);
            //}
            if (files.Any())
            {
                success = await _processCancelService.ProcessFileSatAsync(user, dataSignRequest, files);
            }
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Error al cancelar " + files.Count() + " archivos.");
                //return View("../EFirma/Lista");
                return RedirectToAction("List", "ListCancel");
            }

            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se cancelaron " + files.Count() + " archivos.");
            //return View("../EFirma/Lista");
            return RedirectToAction("List", "ListCancel");
        }
    }
}
