using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalAccountOfStudent.Models;
using PersonalAccountOfStudent.TablesGateway;
using PersonalAccountOfStudent.ViewModels;

namespace PersonalAccountOfStudent.Controllers
{
    public class AssessmentController : Controller
    {
        private SchoolContext db;

        public AssessmentController(SchoolContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Assessment()
        {
            var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            if (user != null)
            {
                ViewData["UserType"] = user.UserType;

                var classes = db.Classes.OrderBy(c => c.NumberClass).ToList();
                ViewData["Classes"] = new SelectList(classes, "Id", "NumberClass");
                ViewData["Students"] = new SelectList(db.Students.Where(s => s.ClassId == classes[0].Id).OrderBy(s => s.FIO).ToList(), "Id", "FIO");
                ViewData["Subjects"] = new SelectList(db.Subjects.OrderBy(s => s.SubjectName).ToList(), "Id", "SubjectName");

                return View();
            }

            return new EmptyResult();
        }

        public IActionResult GetStudents(int id)
        {
            var students = db.Students.Where(s => s.ClassId == id);
            if (students != null)
            {
                return Json(students);
            }

            return new EmptyResult();
        }


        [HttpPost]
        public async Task<IActionResult> Assessment(AssessmentModel model)
        {
            var user = db.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
            if (user != null)
            {
                ViewData["UserType"] = user.UserType;

                ViewData["Success"] = false;
                if (ModelState.IsValid)
                {
                    var student = db.Students.FirstOrDefault(s => s.Id == model.StudentId && s.ClassId == model.ClassId);

                    AssessmentGateway.InsertAssessment(db, student.UserGUID, model.SubjectId, model.Mark);
                    ViewData["Success"] = true;
                    //ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }

                ViewData["Classes"] = new SelectList(db.Classes.OrderBy(c => c.NumberClass).ToList(), "Id", "NumberClass", model.ClassId);
                ViewData["Students"] = new SelectList(db.Students.Where(s => s.ClassId == model.ClassId).OrderBy(s => s.FIO).ToList(), "Id", "FIO", model.StudentId);
                ViewData["Subjects"] = new SelectList(db.Subjects.OrderBy(s => s.SubjectName).ToList(), "Id", "SubjectName");

                return View();
            }

            return new EmptyResult();
        }
    }
}