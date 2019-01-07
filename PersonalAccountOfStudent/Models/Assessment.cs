using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        [Required]
        public string UserGUID { get; set; }
        public User User { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public double Mark { get; set; }
    }
}
