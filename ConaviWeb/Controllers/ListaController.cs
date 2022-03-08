using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("Lista")]
    public class ListaController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProcessSignRepository _processSignRepository;

        public ListaController(IUserRepository userRepository, IProcessSignRepository processSignRepository)
        {
            _userRepository = userRepository;
            _processSignRepository = processSignRepository;
        }
        [Route("List")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../EFirma/Lista");
        }

        [HttpGet("ListaAjax")]
        public async Task<IActionResult> ListAllFiles()
        {
            Response response = new Response();
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<FileResponse> files;

            //if (user.Rol.ToString() == "FirmanteInterno")
            //{
                files = await _processSignRepository.GetFiles(user.IdSystem);


            //}
            //else
            //{
            //    files = await _processSignRepository.GetExternalFiles(user.Integrador);
            //}

            if (files != null)
            {
                response.Success = 1;
                response.Message = "El usuario cuenta con " + files.Count() + " archivos";
                response.Data = files;
                return Json(new { data=files });
            }

            return BadRequest();
        }

        [HttpGet("ListDetailAjax/{idPartition?}")]
        public async Task<IActionResult> ListDetailFiles(int idPartition)
        {
            Response response = new Response();
            IEnumerable<FileResponse> files;

            //if (user.Rol.ToString() == "FirmanteInterno")
            //{
            files = await _processSignRepository.GetPartitionSourceFiles(idPartition);


            //}
            //else
            //{
            //    files = await _processSignRepository.GetExternalFiles(user.Integrador);
            //}

            if (files != null)
            {
                response.Success = 1;
                response.Message = "El usuario cuenta con " + files.Count() + " archivos";
                response.Data = files;
                return Json(new { data = files });
            }

            return BadRequest();
        }
    }
}
