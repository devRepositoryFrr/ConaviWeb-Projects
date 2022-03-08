using ConaviWeb.Commons;
using ConaviWeb.Data.Shell;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.Shell;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Shell
{
    //[Authorize(Roles = "Operador")]
    public class ShellController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IProcessEDRepository _processEDRepository;
        public ShellController(IWebHostEnvironment environment, IProcessEDRepository processEDRepository)
        {
            _environment = environment;
            _processEDRepository = processEDRepository;
        }
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            TempData["rol"] = Convert.ToInt32(user.Rol);
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Shell/Index");
        }

        public class Items
        {
            public string FileName { get; set; }
        }

        

        [HttpGet]
        public async Task<IActionResult> ListaAjaxBDAsync(string type, string process)
        {
            if (type == "encrypt")
            {
                type = "Encriptado";
            }
            else
            {
                type = "Desencriptado";
            }
            IEnumerable<ProcessED> listFiles = new List<ProcessED>();
            listFiles = await _processEDRepository.SelectVoBo(type, process);
            return Json(new { data = listFiles }); ;
        }

        //[Authorize(Roles = "Administrador")]
        [Route("ShellED/{file?}/{processED?}/{path?}")]
        public async Task<IActionResult> ShellEDAsync(string file, string processED,string path)
        {
            bool success = false;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            DateTime dateProcess = DateTime.Now;
            path = path.Replace('-', '/');
            var script = String.Concat(file, " ", processED, " ",path);
            var fileName = Path.Combine(_environment.WebRootPath, "doc", "Shell");
            
            //Use different shell files according to the system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = Path.Combine(fileName, "win.bat");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                fileName = Path.Combine(fileName, "proccess_ed.sh");
            }
            //else
            //{
            //    fileName += "OSX.sh";
            //}
            //Create a processstartinfo object to use the system shell to specify command and parameter settings for standard output
            var psi = new ProcessStartInfo(fileName) { RedirectStandardOutput = true, Arguments = script };
            // start up
            var proc = Process.Start(psi);
            if (proc == null)
            {
                Console.WriteLine("Can not exec.");
            }
            else
            {
                Console.WriteLine("-------------Start read standard output--------------");
                //Start reading
                try
                {
                    using (var sr = proc.StandardOutput)
                    {
                        for (int i = 0; !sr.EndOfStream; i++)
                        {
                            Console.WriteLine(sr.ReadLine());
                        }

                        while (!proc.HasExited) Thread.Sleep(5);

                    }
                    Console.WriteLine("---------------Read end------------------");
                    Console.WriteLine($"Exited Code ： {proc.ExitCode}");

                    success = await _processEDRepository.InsertED(file, path, dateProcess, user.Id, processED);
                }
                catch (Exception e) { }
                
            }
            if (success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se realizo el proceso con exito");
                return RedirectToAction("Index");

            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Error al procesar el archivo");
            return RedirectToAction("Index");
        }

        [Route("ShellValidate/{file?}/{path?}/{ed?}")]
        public async Task<IActionResult> ShellValidateAsync(string file, string path, string ed)
        {
            var success = false;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            path = path.Replace('-', '/');
            DateTime dateProcess = DateTime.Now;
            var rol =  Convert.ToInt32(user.Rol);
            
            success = await _processEDRepository.UpdateVoBo(file, path, dateProcess, user.Id, ed);
            
            
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "El archivo no puede ser validado por el mismo usuario dos veces");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Archivo validado correctamente");
            return RedirectToAction("Index");
        }

    }
}
