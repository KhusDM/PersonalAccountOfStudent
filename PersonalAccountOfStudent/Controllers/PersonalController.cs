using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            path = "images/avatars";
        }

        public IActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            if (user != null)
            {
                string avatarPath = Path.Combine(path, user.Avatar);
                ViewData["AvatarPath"] = avatarPath;
                if (user.UserType == "Student")
                {
                    var person = db.Students.FirstOrDefault(s => s.UserGUID == user.GUID);
                    ViewData["FIO"] = person.FIO;
                    ViewData["DateBirth"] = person.DateBirth.ToShortDateString();
                    int age = DateTime.Now.Year - person.DateBirth.Year;
                    if (DateTime.Now.Month < person.DateBirth.Month ||
                        (DateTime.Now.Month == person.DateBirth.Month && DateTime.Now.Day < person.DateBirth.Day))
                    {
                        age--;
                    }
                    ViewData["Age"] = age.ToString();
                    ViewData["Telephone"] = person.Telephone;
                    ViewData["NumberClass"] = db.Classes.FirstOrDefault(c => c.Id == person.ClassId).NumberClass;
                }
                else if (user.UserType == "Teacher")
                {
                    var person = db.Teachers.FirstOrDefault(t => t.UserGUID == user.GUID);
                    ViewData["FIO"] = person.FIO;
                    ViewData["DateBirth"] = person.DateBirth.ToShortDateString();
                    int age = DateTime.Now.Year - person.DateBirth.Year;
                    if (DateTime.Now.Month < person.DateBirth.Month ||
                        (DateTime.Now.Month == person.DateBirth.Month && DateTime.Now.Day < person.DateBirth.Day))
                    {
                        age--;
                    }
                    ViewData["Age"] = age.ToString();
                    ViewData["Telephone"] = person.Telephone;
                    ViewData["SubjectName"] = db.Subjects.FirstOrDefault(s => s.Id == person.SubjectId).SubjectName;
                }

                ViewData["UserType"] = user.UserType;
            }

            return View();
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