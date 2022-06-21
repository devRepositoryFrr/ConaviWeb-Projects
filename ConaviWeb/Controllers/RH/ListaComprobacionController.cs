using ConaviWeb.Commons;
using ConaviWeb.Data.RH;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.RH
{
    public class ListaComprobacionController : Controller
    {
        private readonly IRHRepository _rHRepository;
        private readonly IMailService _mailService;
        public ListaComprobacionController(IRHRepository rHRepository, IMailService mailService)
        {
            _rHRepository = rHRepository;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return View("../RH/ListaComprobacion");
        }
        public async Task<IActionResult> GetSolicitudesAsync()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var solicitudes = await _rHRepository.GetSolicitudes(4);
            return Json(new { data = solicitudes });
        }
    }
}
