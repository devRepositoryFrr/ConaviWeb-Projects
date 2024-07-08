using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConaviWeb.Data.Expedientes;
using ConaviWeb.Model.Expedientes;
using ConaviWeb.Services;
using static ConaviWeb.Models.AlertsViewModel;
using ConaviWeb.Commons;
using ConaviWeb.Model.Response;

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
        [HttpPost]
        public async Task<IActionResult> InsertCaratulaExpedienteTP(Caratula caratula)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            caratula.IdUser = user.Id;

            var success = await _expedienteRepository.InsertCaratulaExpedienteTP(caratula);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar la carátula");
                return RedirectToAction("Index");
            }
            //return RedirectToAction("Index");
            return Redirect("/Caratula?id=" + caratula.IdExpediente);
        }
    }
}
