using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Models
{
    public class Lecturer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        [NotMapped]
        public List<Subject> AvailableSubjects { get; set; } = new List<Subject>();

        public ICollection<LecturerSubject> LecturerSubjects { get; set; } = new List<LecturerSubject>();


        public void AddSubject(Subject subject)
        {
            var studentSubject = new LecturerSubject
            {
                Lecturer = this,
                Subject = subject
            };
            LecturerSubjects.Add(studentSubject);
        }
    }
}
