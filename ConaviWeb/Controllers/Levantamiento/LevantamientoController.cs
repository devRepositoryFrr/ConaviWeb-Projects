using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Data.Levantamientos;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.PrediosAdquisicion;
using ConaviWeb.Commons;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ConaviWeb.Controllers.Levantamiento
{
    public class LevantamientoController : Controller
    {
        private readonly ILevantamientoRepository _levantamientoRepository;
        private readonly IWebHostEnvironment _environment;
        public LevantamientoController(ILevantamientoRepository levantamientoRepository, IWebHostEnvironment environment)
        {
            _levantamientoRepository = levantamientoRepository;
            _environment = environment;
        }
        public IActionResult IndexAsync()
        {
            return View("../Levantamiento/ListaPredios");
        }
        public IActionResult HomologacionFiles()
        {
            return View("../Levantamiento/ArchivosHomologacion");
        }
        public async Task<IActionResult> Predios()
        {
            var catEstados = await _levantamientoRepository.GetEstados();
            ViewBag.EstadoCatalogo = (new SelectList(catEstados, "Clave", "Descripcion"));
            return View("../Levantamiento/FormatoHomologacion");
        }
        public IActionResult PropuestaConceptual()
        {
            //var catEstados = await _levantamientoRepository.GetEstados();
            //ViewBag.EstadoCatalogo = (new SelectList(catEstados, "Clave", "Descripcion"));
            return View("../Levantamiento/PropuestaConceptual");
        }
        [HttpPost]
        public async Task<IActionResult> GetMunicipios(string cveedo)
        {
            IEnumerable<Catalogo> municipios = new List<Catalogo>();
            municipios = await _levantamientoRepository.GetMunicipios(cveedo);
            return Json(new { data = municipios });
        }
        [HttpPost]
        public async Task<IActionResult> GetLocalidades(string cveedo, string cvemun)
        {
            IEnumerable<Catalogo> localidades = new List<Catalogo>();
            localidades = await _levantamientoRepository.GetLocalidades(cveedo, cvemun);
            return Json(new { data = localidades });
        }
        [HttpPost]
        public async Task<IActionResult> InsertFormatoLevantamiento(Predio predio)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //predio.IdUser = user.Id;
            predio.IdUser = 21;

            var success = false;
            success = await _levantamientoRepository.InsertFormatoLevantamiento(predio);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar el formato");
                return RedirectToAction("Predios");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetFormatoLevantamiento([FromForm] int idPredio)
        {
            Predio predio = new();
            predio = await _levantamientoRepository.GetFormatoLevantamiento(idPredio);
            if (predio == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Id de Predio no encontrado");
                return Ok(alert);
            }
            return Ok(predio);
        }
        [HttpGet]
        public async Task<IActionResult> PrediosAdquisicion()
        {
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var inventario = await _expedienteRepository.GetInventarioControl(user.Cargo);

            IEnumerable<Predio> predios = new List<Predio>();
            predios = await _levantamientoRepository.GetPrediosAdquisicion();

            if (predios == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = predios });
        }
        [HttpPost]
        public async Task<IActionResult> FullPrediosAdquisicion()
        {
            //var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            //var inventario = await _expedienteRepository.GetInventarioControl(user.Cargo);

            IEnumerable<Predio> predios = new List<Predio>();
            predios = await _levantamientoRepository.GetFullPrediosAdquisicion();

            if (predios == null)
            {
                var alert = AlertService.ShowAlert(Alerts.Danger, "Sin registros");
                return Ok(alert);
            }
            return Json(new { data = predios });
        }
        [HttpPost]
        public async Task<IActionResult> DropPredio(Predio predio)
        {
            var success = await _levantamientoRepository.DropPredio(predio.Id);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al eliminar el registro");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se eliminó el registro con éxito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult UpLoad()
        {
            var json = Request.Form["json"].ToString();
            var data = JObject.Parse(json);
            var idPredio = data.GetValue("idPredio").ToString();
            var idFile = data.GetValue("idFile").ToString();
            string alert;
            //string[] arrperiodo = periodo.Split(@"-");
            var files = Request.Form.Files;
            try
            {
                foreach (var file in files)
                {
                    string[] arrpath = file.FileName.Split(@"/");
                    string dirpath = "";//Directory where the file is located (including one or two levels of directories)
                    string fulldir = Path.Combine(arrpath[0]);
                    string filename = arrpath[arrpath.Length - 1].ToString();//The file name
                    var extension = Path.GetExtension(filename);
                    if (!extension.Contains(".pdf") && !extension.Contains(".docx") && !extension.Contains(".xlsx"))
                    {
                        alert = AlertService.ShowAlert(Alerts.Danger, "Solo están permitidos archivos con extensión .pdf");
                        return Ok(new
                        {
                            success = false,
                            message = alert
                        });
                    }
                    string rootpath = Path.Combine(_environment.WebRootPath, "doc", "PrediosAdquisicion", idPredio, idFile);
                    for (int i = 1; i < arrpath.Length; i++)
                    {
                        if (i == arrpath.Length - 1)
                        {
                            break;
                        }
                        dirpath += arrpath[i] + @"/";
                    }
                    dirpath = Path.Combine(rootpath, dirpath);
                    DicCreate(dirpath);//Create the directory if it does not exist

                    string filepath = Path.Combine(rootpath, fulldir);
                    using (var addFile = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        if (file != null)
                        {
                            file.CopyTo(addFile);
                        }
                        else
                        {
                            Request.Body.CopyTo(addFile);
                        }
                        addFile.Close();
                    }
                    _levantamientoRepository.InsertFilePredio(idPredio, idFile, filename, extension);
                }
                alert = AlertService.ShowAlert(Alerts.Success, "Se cargaron " + files.Count + " archivos");
                
                return Ok(new
                {
                    success = true,
                    message = alert
                });
            }
            catch (Exception ex)
            {
                alert = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al cargar los archivos");
                return Ok(new
                {
                    success = false,
                    message = alert
                });
            }
        }
        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult UploadRepFoto()
        {
            var json = Request.Form["json"].ToString();
            var data = JObject.Parse(json);
            var idPredio = data.GetValue("idPredio").ToString();
            var idSedatu = data.GetValue("idSedatu").ToString();
            //var idFile = data.GetValue("idFile").ToString();
            string alert;
            var files = Request.Form.Files;
            try
            {
                foreach (var file in files)
                {
                    string[] arrpath = file.FileName.Split(@"/");
                    string dirpath = "";//Directory where the file is located (including one or two levels of directories)
                    string fulldir = Path.Combine(arrpath[0]);
                    string filename = arrpath[arrpath.Length - 1].ToString();//The file name
                    var extension = Path.GetExtension(filename);
                    if (!extension.Contains(".pdf"))
                    {
                        alert = AlertService.ShowAlert(Alerts.Danger, "Solo están permitidos archivos con extensión .pdf");
                        return Ok(new
                        {
                            success = false,
                            message = alert
                        });
                    }
                    string rootpath = Path.Combine(_environment.WebRootPath, "doc", "PrediosAdquisicion", "RepFoto", idPredio, idSedatu);
                    for (int i = 1; i < arrpath.Length; i++)
                    {
                        if (i == arrpath.Length - 1)
                        {
                            break;
                        }
                        dirpath += arrpath[i] + @"/";
                    }
                    dirpath = Path.Combine(rootpath, dirpath);
                    DicCreate(dirpath);//Create the directory if it does not exist

                    string filepath = Path.Combine(rootpath, fulldir);
                    using (var addFile = new FileStream(filepath, FileMode.OpenOrCreate))
                    {
                        if (file != null)
                        {
                            file.CopyTo(addFile);
                        }
                        else
                        {
                            Request.Body.CopyTo(addFile);
                        }
                        addFile.Close();
                    }
                    _levantamientoRepository.InsertRepFoto(idPredio, filename);
                }
                alert = AlertService.ShowAlert(Alerts.Success, "Se cargaron " + files.Count + " archivos");
                
                return Ok(new
                {
                    success = true,
                    message = alert
                });
            }
            catch (Exception ex)
            {
                alert = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al cargar los archivos");
                return Ok(new
                {
                    success = false,
                    message = alert
                });
            }
        }
        private void DicCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetFile(int idPredio, int idFile)
        {
            Catalogo file = new Catalogo();
            file = await _levantamientoRepository.GetFile(idPredio, idFile);
            return Json(new { data = file });
        }
        [HttpPost]
        public async Task<IActionResult> GetRepFoto(int idPredio)
        {
            Catalogo file = new Catalogo();
            file = await _levantamientoRepository.GetRepFoto(idPredio);
            return Json(new { data = file });
        }
        [HttpPost]
        public async Task<IActionResult> ValidarArchivo(string idPredio, int idFile)
        {
            var success = await _levantamientoRepository.ValidarArchivo(idPredio, idFile);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al validar el archivo");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se validó el archivo con éxito");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RechazarArchivo(int idPredio, int idFile)
        {
            var success = await _levantamientoRepository.RechazarArchivo(idPredio, idFile);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al rechazar el archivo");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se rechazó el archivo con éxito");
            return RedirectToAction("Index");
        }
    }
}
