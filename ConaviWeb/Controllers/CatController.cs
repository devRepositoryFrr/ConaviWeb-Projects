using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers
{
    public class CatController : Controller
    {
        private readonly ISourceFileRepository _sourceFileRepository;
        private readonly IUserRepository _userRepository;

        public CatController(ISourceFileRepository sourceFileRepository, IUserRepository userRepository)
        {
            _sourceFileRepository = sourceFileRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View("../EFirma/_AddItem");
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] Partition partition)
        {
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            bool success = await _sourceFileRepository.InsertPartition(partition.Text, user);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la partición");
                return RedirectToAction("Index", "UploadFile");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se registro con exito la partición");
            return RedirectToAction("Index","UploadFile");
        }


    }
}
