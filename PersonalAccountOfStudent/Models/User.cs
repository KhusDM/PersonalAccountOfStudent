using PersonalAccountOfStudent.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.Models
{
    public class User
    {
        [Key]
        public string GUID { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Avatar { get; set; } = "Avatar.png";
        [NotMapped]
        public IUserState State { get; set; }

        public void GetUserPersonalInfo(SchoolContext context, out PersonalInfoView personalInfo)
        {
            State.GetUserPersonalInfo(this, context, out personalInfo);
        }
    }
}
