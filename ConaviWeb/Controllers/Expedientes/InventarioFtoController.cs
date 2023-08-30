using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Expedientes
{
    public class InventarioFtoController : Controller
    {
        public IActionResult Index()
        {
            return View("../Expedientes/InventarioFto");
        }
    }
}