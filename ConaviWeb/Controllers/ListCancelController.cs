using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("ListCancel")]
    public class ListCancelController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProcessSignRepository _processSignRepository;
        private readonly ISecurityRepository _securityRepository;
        public ListCancelController(IUserRepository userRepository, IProcessSignRepository processSignRepository, ISecurityRepository securityRepository)
        {
            _userRepository = userRepository;
            _processSignRepository = processSignRepository;
            _securityRepository = securityRepository;
        }

        [Route("List")]
        public async Task<IActionResult> Index()
        {
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<Partition> partitions = await _securityRepository.GetPartitions(user.IdSystem);

            ViewData["Partitions"] = partitions;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../EFirma/ListCancel");
        }

        [HttpGet("ListaFirmadosAjax/{idPartition?}")]
        public async Task<IActionResult> ListAllSigned(int idPartition)
        {
            Response response = new Response();
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<FileResponse> files;

            //if (user.Rol.ToString() == "FirmanteInterno")
            //{
            if (idPartition == 0)
            {
                files = await _processSignRepository.GetSignedFilesCancel(user.IdSystem);
            }
            else
            {
                files = await _processSignRepository.GetPartitionFilesCancel(idPartition);
            }



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
