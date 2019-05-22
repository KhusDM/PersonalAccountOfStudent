using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalAccountOfStudent.Models;
using PersonalAccountOfStudent.ViewModels;

namespace PersonalAccountOfStudent
{
    public class StateUserStudent : IUserState
    {
        public void GetUserPersonalInfo(User user, SchoolContext context, out PersonalInfoView personalInfo)
        {
            var person = context.Students.FirstOrDefault(s => s.UserGUID == user.GUID);
            personalInfo = new PersonalInfoView();

            personalInfo.FIO = person.FIO;
            personalInfo.DateBirth = person.DateBirth.ToShortDateString();
            int age = DateTime.Now.Year - person.DateBirth.Year;
            if (DateTime.Now.Month < person.DateBirth.Month ||
                (DateTime.Now.Month == person.DateBirth.Month && DateTime.Now.Day < person.DateBirth.Day))
            {
                age--;
            }
            personalInfo.Age = age.ToString();
            personalInfo.Telephone = person.Telephone;
            personalInfo.UserType = user.UserType;
            personalInfo.NumberClass = context.Classes.FirstOrDefault(c => c.Id == person.ClassId).NumberClass;
        }
    }
}
