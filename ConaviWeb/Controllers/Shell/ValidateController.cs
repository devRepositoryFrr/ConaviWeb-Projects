using ConaviWeb.Commons;
using ConaviWeb.Data.Shell;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Shell
{
    public class ValidateController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IProcessEDRepository _processEDRepository;
        public ValidateController(IWebHostEnvironment environment, IProcessEDRepository processEDRepository)
        {
            _environment = environment;
            _processEDRepository = processEDRepository;
        }

        public class Items
        {
            public string FileName { get; set; }
        }
        public async Task<IActionResult> IndexAsync()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            TempData["rol"] = Convert.ToInt32(user.Rol);
            var procesos = await _processEDRepository.GetProcesos(Convert.ToInt32(user.Rol));
            ViewData["Procesos"] = procesos;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Shell/Validate");
        }


        [HttpGet]
        public IActionResult ListaAjax(string type, string process)
        {
            var script = String.Concat(type, " ", process);
            var fileName = Path.Combine(_environment.WebRootPath, "doc", "Shell");
            var list = new List<Items>();
            //Use different shell files according to the system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = Path.Combine(fileName, "win.bat");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                fileName = Path.Combine(fileName, "linux.sh");
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
                            list.Add(new Items
                            {
                                FileName = sr.ReadLine()

                            });
                        }

                        while (!proc.HasExited) Thread.Sleep(5);

                    }
                    Console.WriteLine("---------------Read end------------------");
                    Console.WriteLine($"Exited Code ： {proc.ExitCode}");
                    return Json(new { data = list });
                }
                catch (Exception e) { }
            }

            return BadRequest();
        }

        [Route("ShellValidateS/{file?}/{path?}/{ed?}")]
        public async Task<IActionResult> ShellValidateSAsync(string file, string path, string ed)
        {
            var success = false;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            path = path.Replace('-', '/');
            DateTime dateProcess = DateTime.Now;
            var rol = Convert.ToInt32(user.Rol);

            success = await _processEDRepository.InsertVoBo(file, path, dateProcess, user.Id, ed);


            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Archivo validado anteriormente");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Archivo validado correctamente");
            return RedirectToAction("Index");
        }

        [Route("ShellEDWV/{file?}/{processED?}/{path?}")]
        public async Task<IActionResult> ShellEDAsync(string file, string processED, string path)
        {
            bool success = false;
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            DateTime dateProcess = DateTime.Now;
            path = path.Replace('-', '/');
            var script = String.Concat(file, " ", processED, " ", path);
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

    }
}
