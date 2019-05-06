using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.ViewModels
{
    public class AssessmentModel
    {
        [Required(ErrorMessage = "Не указан класс")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Не указан учащийся")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Не указан предмет")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Не указана оценка")]
        public double Mark { get; set; }
    }
}
