using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.Models
{
    public class Class
    {
        public int Id { get; set; }
        [Required]
        public string NumberClass { get; set; }
        public int? CountStudent { get; set; }
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
