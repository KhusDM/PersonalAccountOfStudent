using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string UserGUID { get; set; }
        public User User { get; set; }
        [Required]
        public string FIO { get; set; }
        public int? Age { get; set; }
        public string Telephone { get; set; }
        public int? ClassId { get; set; }
        public Class Class { get; set; }
    }
}
