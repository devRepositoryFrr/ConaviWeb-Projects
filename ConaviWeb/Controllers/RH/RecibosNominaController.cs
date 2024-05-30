using ConaviWeb.Commons;
using ConaviWeb.Data.RH;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.RH;
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
        private readonly INominaRepository _nominaRepository;

        public RecibosNominaController(IWebHostEnvironment environment, INominaRepository nominaRepository)
        {
            _environment = environment;
            _nominaRepository = nominaRepository;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../RH/RecibosNomina");
        }
        public async Task<IActionResult> DownloadZipRN(string anio, string periodo, string acept)
        {
            string[] arrperiodo = periodo.Split(@"-");
                string[] meses = {"Enero",
                                    "Febrero",
                                    "Marzo",
                                    "Abril",
                                    "Mayo",
                                    "Junio",
                                    "Julio",
                                    "Agosto",
                                    "Septiembre",
                                    "Octubre",
                                    "Noviembre",
                                    "Diciembre"};
                var numes = Array.FindIndex(meses, row => row.Contains(arrperiodo[0])) + 1;
            var rootPath = Path.Combine(_environment.WebRootPath, "doc", "RH", "ReciboNomina");

            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");

            var botsFolderPath = Path.Combine(rootPath, anio, arrperiodo[0], arrperiodo[1], user.NuEmpleado);
            var zipFileMemoryStream = new MemoryStream();
            try
            {
                var botFilePaths = Directory.GetFiles(botsFolderPath);

                using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
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

               
                Aceptacion aceptacion = new();
                aceptacion.Anio = anio;
                aceptacion.Mes = numes.ToString();
                aceptacion.Quincena = arrperiodo[1].Substring(1);
                aceptacion.Acepta = acept.Equals("on") ? 1 : 2;
                aceptacion.NuEmpleado = user.NuEmpleado;

                var success = await _nominaRepository.InsertAceptacion(aceptacion);
            }
            catch (Exception e)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "El periodo consultado no cuenta con archivos");
                return RedirectToAction("Index");
            }
            zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
            return File(zipFileMemoryStream, "application/octet-stream", "Recibo_" + anio + arrperiodo[0] + arrperiodo[1] + ".zip");
        }
    }
}
