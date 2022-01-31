using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.UI.Controllers
{
    public class BaseController : Controller
    {
        public int GetCurrentUserId()
        {
            var id = Convert.ToInt32(HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return id;
        }

        public int GetCurrentUserRole()
        {
            var role = Convert.ToInt32(HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);

            return role;
        }

        public string GetCurrentUserName()
        {
            var firstName = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return firstName;
        }
    }
}
