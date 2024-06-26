using Students.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Common.Models;

public class Subject
{
    public int Id { get; set; }

    [Required]
    [ValidSubjectName]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Credits { get; set; }

    public List<Student> Students { get; set; } = new List<Student>();

    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public ICollection<LecturerSubject> LecturerSubjects { get; set; } = new List<LecturerSubject>();

    [NotMapped]
    public List<Student> AvailableStudents { get; set; } = new List<Student>();


    public void AddStudent(Student student)
    {
        var subjectStudent = new StudentSubject
        {
            Subject = this,
            Student = student
        };
        StudentSubjects.Add(subjectStudent);
    }

    public Subject()
    {
    }

    public Subject(string name, int credits)
    {
        Name = name;
        Credits = credits;
    }
}
