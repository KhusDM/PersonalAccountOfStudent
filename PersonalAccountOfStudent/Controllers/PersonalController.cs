using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalAccountOfStudent.Models;
using PersonalAccountOfStudent.ViewModels;

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
            path = "images/avatars";
        }

        public IActionResult Index()
        {
            PersonalInfoView personalInfo = null;
            var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (user.UserType == "Student")
                    user.State = new StateUserStudent();
                else if (user.UserType == "Teacher")
                    user.State = new StateUserTeacher();

                user.GetUserPersonalInfo(db, out personalInfo);

                personalInfo.AvatarPath = Path.Combine(path, user.Avatar);

                ViewData["UserType"] = user.UserType;
            }

            return View(personalInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
                if (user != null)
                {
                    string avatarPath = Path.Combine(Env.WebRootPath, path, uploadedFile.FileName);
                    using (var fileStream = new FileStream(avatarPath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    user.Avatar = uploadedFile.FileName;
                    db.Users.Update(user);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}