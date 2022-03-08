using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using ConaviWeb.Commons;
using ConaviWeb.Services;
using ConaviWeb.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers
{
    [Authorize]
    [Route("UploadFile")]
    public class UploadFileController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISourceFileRepository _sourceFileRepository;
        private readonly ISecurityTools _securityTools;
        private readonly ISecurityRepository _securityRepository;
        private readonly IUserRepository _userRepository;
        public UploadFileController(IWebHostEnvironment environment, ISourceFileRepository sourceFileRepository, ISecurityTools securityTools, ISecurityRepository securityRepository, IUserRepository userRepository)
        {
            _environment = environment;
            _sourceFileRepository = sourceFileRepository;
            _securityTools = securityTools;
            _securityRepository = securityRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            IEnumerable<Partition> partitions = await _securityRepository.GetPartitions(user.IdSystem);
            
            ViewData["Partitions"] = partitions;
            ViewBag.Alert = TempData["Alert"];
            return View("../EFirma/UploadFile");
        }

        
        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> LoadFile([FromForm] FileRequest formFiles)
        {
            try
            {
                var partition = await _securityRepository.GetPartition(formFiles.Partition);

                SourceFile sourceFile = new SourceFile();
            DateTime dateTime = DateTime.Now;
            sourceFile.IdUser = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool success = true;
            Response respuesta = new Response();
                var shortPath = Path.Combine("doc", "EFirma", "Original", dateTime.Year.ToString(), dateTime.Month.ToString(), partition.Text);
            var currentPath = Path.Combine(_environment.WebRootPath, shortPath);
            int count = formFiles.FileCollection.Count;
            foreach (var file in formFiles.FileCollection)
            {
                if (file.Length > 0)
                {
                    sourceFile.Hash = FileTools.GetHashDocument(file);
                    if (!Directory.Exists(currentPath))
                        FileTools.CreateDirectory(currentPath);
                    sourceFile.FilePath = shortPath;
                    sourceFile.FileName = file.FileName;
                        sourceFile.IdPartition = formFiles.Partition;
                    var filePath = Path.Combine(currentPath, file.FileName);
                    await FileTools.SaveFileAsync(file, filePath);
                    success = await _sourceFileRepository.InsertSourceFile(sourceFile);
                }
            }
            if (!success)
            {
                respuesta.Success = 0;
                respuesta.Message = "Ocurrio un error al cargas los archivos";
                    TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al cargas los archivos");
                return RedirectToAction("Index","UploadFile");
            }

            respuesta.Success = 1;
            respuesta.Message = "Se cargaron " + count + " archivos.";
                
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se cargaron " + count + " archivos.");

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("Index", "UploadFile");
        }
    }
}
