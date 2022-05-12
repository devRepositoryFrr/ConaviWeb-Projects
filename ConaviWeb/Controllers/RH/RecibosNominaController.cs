using ConaviWeb.Commons;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.RH
{
    public class RecibosNominaController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public RecibosNominaController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../RH/RecibosNomina");
        }
        public async Task DownloadZipRN(string anio, string periodo)
        {
            try
            {
            string[] arrperiodo = periodo.Split(@"-");
            var rootPath = Path.Combine(_environment.WebRootPath, "doc", "RH", "ReciboNomina");

            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");

            Response.ContentType = "application/octet-stream";
            DateTime date = DateTime.Now;
            Response.Headers.Add("Content-Disposition", "attachment; filename=\"Files" + "Recibo_"+ anio + arrperiodo[0] + arrperiodo[1] + ".zip\"");

            var botsFolderPath = Path.Combine(rootPath, anio, arrperiodo[0], arrperiodo[1], user.NuEmpleado);
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
            catch (Exception e)
            {
            }
        }
    }
}
