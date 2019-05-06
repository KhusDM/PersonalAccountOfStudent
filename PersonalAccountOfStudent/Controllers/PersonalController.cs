using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PersonalAccountOfStudent.Models;

namespace PersonalAccountOfStudent.Controllers
{
    public class PersonalController : Controller
    {
        private IHostingEnvironment Env { get; }
        private SchoolContext db;
        private string path;

        public PersonalController(IHostingEnvironment env, SchoolContext context)
        {
            Env = env;
            db = context;
            path = Path.Combine(Env.WebRootPath, "images/avatars");
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}