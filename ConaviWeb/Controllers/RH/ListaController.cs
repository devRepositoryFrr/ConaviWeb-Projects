using ConaviWeb.Commons;
using ConaviWeb.Data.RH;
using ConaviWeb.Model.Response;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.RH
{
    public class ListaController : Controller
    {
        private readonly IRHRepository _rHRepository;
        private readonly IMailService _mailService;
        public ListaController(IRHRepository rHRepository, IMailService mailService)
        {
            _rHRepository = rHRepository;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return View("../RH/ListaSolicitudes");
        }
        public async Task<IActionResult> GetSolicitudesAsync()
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var solicitudes = await _rHRepository.GetSolicitudesUser(user.Id);
            return Json(new { data = solicitudes });
        }
    }
}
