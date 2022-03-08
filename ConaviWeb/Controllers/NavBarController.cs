using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Response;
using ConaviWeb.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers
{
    public class NavBarController : Controller
    {
        public IActionResult Index()
        {
            var sessionData = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            List<Module> modules = sessionData.Modules.Select(m => new Module
            {
                Url = m.Url,
                Text = m.Text,
                Ico = m.Ico
            }).ToList();
            //return Json(modules);
            return PartialView("_NavMenu",modules);
        }
    }
}
