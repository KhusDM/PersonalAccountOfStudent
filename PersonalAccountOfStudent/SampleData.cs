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
                User user1 = new User { GUID = guid1, Login = "DKhus1995", Password = "Lovasa1995", UserType = "Student" },
                    user2 = new User { GUID = guid2, Login = "IBars1984", Password = "Lovasa1984", UserType = "Teacher" };
                Subject subject1 = new Subject { SubjectName = "Informatika", NumberHours = 100 };
                Teacher teacher1 = new Teacher { UserGUID = guid2, User = user2, FIO = "Maxim Makarov", DateBirth = new DateTime(1970, 1, 11), Telephone = "+7900000000", SubjectId = subject1.Id, Subject = subject1 };
                Class class1 = new Class { NumberClass = "5A", CountStudents = 22, TeacherId = 1, Teacher = teacher1 };
                Student student1 = new Student { UserGUID = guid1, User = user1, FIO = "Khusnetdinov Dmitry", DateBirth = new DateTime(1995, 12, 6), Telephone = "+7900000000", ClassId = 1, Class = class1 };

                context.Users.AddRange(user1, user2);
                context.Subjects.Add(subject1);
                context.Teachers.Add(teacher1);
                context.Classes.Add(class1);
                context.Students.Add(student1);
                context.Assessments.AddRange(
                    new Assessment { UserGUID = guid1, User = user1, SubjectId = subject1.Id, Subject = subject1, Mark = 4 },
                    new Assessment { UserGUID = guid1, User = user1, SubjectId = subject1.Id, Subject = subject1, Mark = 5 },
                    new Assessment { UserGUID = guid1, User = user1, SubjectId = subject1.Id, Subject = subject1, Mark = 5 }
                    );

                context.SaveChanges();
            }
        }
    }
}
