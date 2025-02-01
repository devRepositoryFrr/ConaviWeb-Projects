using ConaviWeb.Data.Minuta;
using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    public class AcuerdoController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        public AcuerdoController(IMinutaRepository minutaRepository, IWebHostEnvironment environment, IMailService mailService, IUserRepository userRepository)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
            _mailService = mailService;
            _userRepository = userRepository;
        }
        [Route("DetalleAcuerdo/{id?}")]
        public async Task<IActionResult> DetalleAcuerdoAsync(int id)
        {
            var acuerdo = await _minutaRepository.GetAcuerdoDetail(id);
            var archivo = acuerdo.Archivo;
            ViewData["Acuerdo"] = acuerdo;
            ViewData["Archivo"] = archivo;
            return View("../Minuta/Acuerdo");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAcuerdoAsync(IFormFile file, int estatus, int acuerdo)
        {
            //User user = await _userRepository.GetUserDetails(Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var archivo = new ArchivoAcuerdo();
            if (file != null)
            {
                archivo.NombreArchivo = file.FileName;
                archivo.IdAcuerdo = acuerdo;
                archivo.Usuario = "frojas@conavi.gob.mx";
                var filePath = Path.Combine(_environment.WebRootPath, "doc", "EvidenciaAcuerdo", file.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                var success = await _minutaRepository.InsertArchivoAcuerdo(archivo);
            }
            var upAcuerdo = await _minutaRepository.UpdateEstatusAcuerdo(acuerdo,estatus);
            return Ok();
        }
        [HttpGet("downAAcuerdo/{archivo?}")]
        public IActionResult DownAAcuerdo(string archivo)
        {
            var path = Path.Combine(_environment.WebRootPath, "doc", "EvidenciaAcuerdo", archivo);
            var fs = new FileStream(path, FileMode.Open);
            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", archivo);
        }
    }
}
