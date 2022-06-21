using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.RH
{
    public class ViaticosComprobacionController : Controller
    {
        public IActionResult Index()
        {
            return View("../RH/ViaticosComprobacion");
        }
    }
}
