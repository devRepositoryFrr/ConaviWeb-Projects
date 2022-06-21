using ConaviWeb.Commons;
using ConaviWeb.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Sisevive
{
    [Authorize]
    [Route("Home")]
    public class HomeController : Controller
    {
        public IActionResult Index(int pass)
        {

            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            if (pass == 2)
            {
                
                ViewData["UpdatePass"] = 0;
            }
            else
            {
                ViewData["UpdatePass"] = user.UpdatePass;
            }
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Home/Home");
        }
    }
}
