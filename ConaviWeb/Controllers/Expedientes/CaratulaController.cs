using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.Expedientes
{
    public class CaratulaController : Controller
    {
        private readonly IExpedienteRepository _expedienteRepository;

        public CaratulaController(IExpedienteRepository expedienteRepository)
        {
            _expedienteRepository = expedienteRepository;
        }
        public IActionResult Index()
        {
            return View("../Expedientes/Caratula");
        }
    }
}
