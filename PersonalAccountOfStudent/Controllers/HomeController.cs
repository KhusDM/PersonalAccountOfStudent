using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccountOfStudent.Models;

namespace PersonalAccountOfStudent.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SchoolContext db;

        public HomeController(SchoolContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            ViewData["UserType"] = user.UserType;
            return View();
            //return Content(User.Identity.Name);
        }

        public IActionResult Personal()
        {
            return RedirectToAction("Index", "Personal");
        }

        public IActionResult Assessment()
        {
            return RedirectToAction("Assessment", "Assessment");
        }

        public IActionResult Progress()
        {
            return RedirectToAction("Index", "Progress");
        }

        public IActionResult Schedule()
        {
            return RedirectToAction("Index", "Schedule");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
