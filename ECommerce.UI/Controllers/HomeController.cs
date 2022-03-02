using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.UI.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var currentUserRole = GetCurrentUserRole();
            var currentUserName = GetCurrentUserName();

            if (currentUserRole == 1)
            {
                ViewBag.Name = currentUserName;
                return View();
            }

            return RedirectToAction("Index", "Product");
        }
    }
}
