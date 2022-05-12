using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.RH
{
    public class CargaRecibosNController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public CargaRecibosNController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../RH/CargaRecibosN");
        }

        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult UpLoad()
        {
            var files = Request.Form.Files;
            var json = Request.Form["json"].ToString();
            var data = JObject.Parse(json);
            var anio = data.GetValue("anio").ToString();
            var periodo = data.GetValue("periodo").ToString();
            string alert;
            string[] arrperiodo = periodo.Split(@"-");
            try
            {
                foreach (var file in files)
                {
                    string[] arrpath = file.FileName.Split(@"/");
                    string dirpath = "";//Directory where the file is located (including one or two levels of directories)
                    string fulldir = Path.Combine(arrpath[arrpath.Length - 2].ToString(), arrpath[arrpath.Length - 1].ToString());
                    string filename = arrpath[arrpath.Length - 1].ToString();//The file name
                    var extension = Path.GetExtension(filename);
                    if (!extension.Contains(".pdf") && !extension.Contains(".xml"))
                    {
                        alert = AlertService.ShowAlert(Alerts.Danger, "Solo están permitidos archivos con extensión .pdf y .xml");
                        return Ok(new
                        {
                            success = false,
                            message = alert
                        });
                    }
                    string rootpath = Path.Combine(_environment.WebRootPath, "doc", "RH", "ReciboNomina", anio, arrperiodo[0], arrperiodo[1]);
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
                }
                alert = AlertService.ShowAlert(Alerts.Success, "Se cargaron " + files.Count + " archivos" );
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
        /// <summary>
        /// If the file directory does not exist, create a new directory
        /// </summary>
        /// <param name="path"></param>
        private void DicCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
