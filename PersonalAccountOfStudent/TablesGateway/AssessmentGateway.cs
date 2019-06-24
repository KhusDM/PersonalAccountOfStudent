using PersonalAccountOfStudent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.TablesGateway
{
    public class AssessmentGateway
    {
        public static List<Assessment> FindAssessments(SchoolContext context, string userGuid, string subject)
        {
            return context.Assessments.Where(a => a.UserGUID == userGuid && a.Subject.SubjectName == subject).ToList();
        }

        public static void InsertAssessment(SchoolContext context, string userGuid, int subjectId, double mark)
        {
            var assessment = new Assessment { UserGUID = userGuid, SubjectId = subjectId, Mark = mark };
            context.Assessments.Add(assessment);
            context.SaveChanges();
        }
    }
}
