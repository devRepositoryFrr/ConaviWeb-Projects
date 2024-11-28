using ConaviWeb.Model.Minuta;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Minutas
{
    public class RepoCapturaController : Controller
    {
        public IActionResult Index()
        {
            return View("../Minuta/Captura");
        }

        public IActionResult CrearCaptura(RepoCaptura repoCaptura)
        {
            return Ok();
        }
    }
}
