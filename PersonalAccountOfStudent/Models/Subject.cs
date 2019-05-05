using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string SubjectName { get; set; }
        public double? NumberHours { get; set; }
    }
}
