using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("ListSigned")]
    public class ListSignedFilesController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepository _userRepository;
        private readonly IProcessSignRepository _processSignRepository;
        private readonly ISecurityRepository _securityRepository;

        public ListSignedFilesController(IWebHostEnvironment environment, IUserRepository userRepository, IProcessSignRepository processSignRepository, ISecurityRepository securityRepository)
        {
            _environment = environment;
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
            return View("../EFirma/ListFirmados");
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
                    files = await _processSignRepository.GetSignedFiles(user.IdSystem);
                }
                else
                {
                    files = await _processSignRepository.GetPartitionFiles(idPartition);
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
        [Route("DownloadZipFile/{id?}")]
        public async Task DownloadZipFile(int id)
        {
            Partition partition = await _securityRepository.GetPartition(id);

            Response.ContentType = "application/octet-stream";
            DateTime date = DateTime.Now;
            Response.Headers.Add("Content-Disposition", "attachment; filename=\"Files"+date.ToString("ddMMyyyy") +".zip\"");

            var botsFolderPath = Path.Combine(_environment.WebRootPath, partition.PathPartition ,partition.Text);
            var botFilePaths = Directory.GetFiles(botsFolderPath);
            using (ZipArchive archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create))
            {
                foreach (var botFilePath in botFilePaths)
                {
                    var botFileName = Path.GetFileName(botFilePath);
                    var entry = archive.CreateEntry(botFileName);
                    using (var entryStream = entry.Open())
                    using (var fileStream = System.IO.File.OpenRead(botFilePath))
                    {
                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }
        }
    }
}
