using PersonalAccountOfStudent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent
{
    public static class SampleData
    {
        public static void Initialize(SchoolContext context)
        {
            if (!context.Users.Any())
            {
                string guid1 = Guid.NewGuid().ToString(), guid2 = Guid.NewGuid().ToString();
                User user1 = new User { GUID = guid1, Login = "DKhus1995", Password = "Lovasa1995" },
                    user2 = new User { GUID = guid2, Login = "IBars1984", Password = "Lovasa1984" };
                Teacher teacher1 = new Teacher { UserGUID = guid2, User = user2, FIO = "Maxim Makarov", Age = 35, Telephone = "+7900000000", Subject = "Informatika" };
                Class class1 = new Class { NumberClass = "5A", CountStudent = 22, TeacherId = 1, Teacher = teacher1 };
                Student student1 = new Student { UserGUID = guid1, User = user1, FIO = "Khusnetdinov Dmitry", Age = 23, Telephone = "+7900000000", ClassId = 1, Class = class1 };

                context.Users.AddRange(user1, user2);
                context.Teachers.Add(teacher1);
                context.Classes.Add(class1);
                context.Students.Add(student1);

                context.SaveChanges();
            }
        }
    }
}
