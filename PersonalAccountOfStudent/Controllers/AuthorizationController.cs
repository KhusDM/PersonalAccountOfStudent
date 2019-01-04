using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalAccountOfStudent.Models;

namespace PersonalAccountOfStudent.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Authorize()
        {
            return View();
        }
    }
}