using PersonalAccountOfStudent.Models;
using PersonalAccountOfStudent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent
{
    public interface IUserState
    {
        void GetUserPersonalInfo(User user, SchoolContext context, out PersonalInfoView personalInfo);
    }
}
