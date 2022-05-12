using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers
{
    public class NavigationComponent : ViewComponent
    {
        private readonly ISecurityRepository _securityRepository;
        public NavigationComponent(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }
        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var user = User as ClaimsPrincipal;
        //    //var Rol = Int32.Parse(user.FindFirstValue(ClaimTypes.Role));
        //    IEnumerable<Module> modules = (List<Module>)await _securityRepository.GetModules(Convert.ToInt32("2"), 1);
        //    return View("../NavigationBar", modules);
        //}
    }
}
