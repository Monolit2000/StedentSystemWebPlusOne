using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Models
{
    // One Major -> many students 
    public class Major
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<MajorStudent> MajorStudents { get; set; } = new List<MajorStudent>();

        [NotMapped]
        public List<Student> AvailableStudents { get; set; } = new List<Student>();

        public void AddStudent(Student student)
        {
            var MajorStudent = new MajorStudent
            {
                Major = this,
                Student = student
            };
            MajorStudents.Add(MajorStudent);
        }

    }
}
