using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Students.Common.Data;
using Students.Common.Models;
using Students.Interfaces;

using System.Diagnostics.Metrics;

namespace Students.Services;

public class DatabaseService : IDatabaseService
{
    #region Ctor and Properties

    private readonly StudentsContext _context;

    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(
        ILogger<DatabaseService> logger,
        StudentsContext context)
    {
        _logger = logger;
        _context = context;
    }

    #endregion // Ctor and Properties


    #region Public Methods

    #region Subjects 

    public async Task<List<Subject>> GetOllSubjectsAsync()
    {
        var subjects = await _context.Subject.ToListAsync();
        return subjects;
    }



    public async Task<Subject?> GetSubjectAsync(int? id)
    {
        var subject = await _context.Subject
            .FirstOrDefaultAsync(m => m.Id == id);

        return subject;
    }

    public async Task<Subject?> GetSubjectWithStudentsAsync(int? id)
    {

        var subject = await GetSubjectAsync(id);

        if (subject == null)
            return subject;

        var chosenStudents = _context.StudentSubject
            .Where(ss => ss.SubjectId == id)
            .Select(ss => ss.Student)
            .ToList();

        var availableStudents = _context.Student
            .Where(s => !chosenStudents.Contains(s))
            .ToList();

        subject.StudentSubjects = _context.StudentSubject
            .Where(x => x.SubjectId == id)
            .ToList();

        subject.AvailableStudents = availableStudents;

        return subject;

    }

    public async Task CreateSubjectAsync(Subject subject, int[] subjectIdDst)
    {

        var chosenStudents = _context.Student
           .Where(s => subjectIdDst.Contains(s.Id))
           .ToList();

        var availableStudents = _context.Student
            .Where(s => !subjectIdDst.Contains(s.Id))
            .ToList();

        var newSubject = new Subject()
        {
            Name = subject.Name,
            Credits = subject.Credits,           
            AvailableStudents = availableStudents
        };
        foreach (var chosenStud in chosenStudents)
        {
            newSubject.AddStudent(chosenStud);
        }

        _context.Add(newSubject);
        await _context.SaveChangesAsync();

    }

    public async Task UpdateSubjectAsync(Subject subjectModel, int[] subjectIdDst)
    {

        var subject = await GetSubjectAsync(subjectModel.Id);

        if (subject == null)
            return;


        subject.Name = subjectModel.Name;
        subject.Credits = subjectModel.Credits;

        var chosenStudents = _context.Student
            .Where(s => subjectIdDst.Contains(s.Id))
            .ToList();

        var StudentSubject = _context.StudentSubject
            .Where(ss => ss.SubjectId == subjectModel.Id)
            .ToList();
        _context.StudentSubject.RemoveRange(StudentSubject);

        foreach (var student in chosenStudents)
        {
            var studentSubject = new StudentSubject
            {
                Student = student,
                Subject = subject
            };
            _context.StudentSubject.Add(studentSubject);
        }

        var resultInt = _context.SaveChanges();
    }

    public async Task DeleteSubjectAsync(int id)
    {
        var subject = await _context.Subject.FindAsync(id);

        if (subject == null)
            return;
        
        _context.Subject.Remove(subject);

        await _context.SaveChangesAsync();
    }

    public bool SubjectExists(int id)
    {
        return _context.Subject.Any(e => e.Id == id);
    }

    public List<Subject> GetChosenSubjects(int? id)
    {
        var chosenSubjects = _context.StudentSubject
            .Where(ss => ss.StudentId == id)
            .Select(ss => ss.Subject)
            .ToList();

        return chosenSubjects;
    }

    public List<Subject> GetAvailableSubjects(int? id)
    {
        var chosenSubjects = GetChosenSubjects(id);

        var availableSubjects = _context.Subject
            .Where(s => !chosenSubjects.Contains(s))
            .ToList();

        return availableSubjects;
    }

    public List<StudentSubject> GetStudentSubjects(int? id)
    {
        var studentSubjects = _context.StudentSubject
            .Where(x => x.StudentId == id)
            .ToList();

        return studentSubjects;
    }

    #endregion


    #region Major


    public async Task<Major?> GetMajorAsync(int? Id)
    {

        var majorArea = await _context.Major
            .FirstOrDefaultAsync(m => m.Id == Id);

        return majorArea;
    }

    public async Task<Major> GetMajorWithStudentAsync(int? Id)
    {

        var major = await GetMajorAsync(Id);

        if (major == null)
            return major;

        var chosenStudents = _context.MajorStudent
            .Where(ss => ss.MajorId == Id)
            .Select(ss => ss.Student)
            .ToList();

        var availableStudents = _context.Student
            .Where(s => !chosenStudents.Contains(s))
            .ToList();

        major.MajorStudents = _context.MajorStudent
            .Where(x => x.MajorId == Id)
            .ToList();

        major.AvailableStudents = availableStudents;

        return major;

    }

    public async Task<IList<Major>> GetOllMajorsAsync()
    {
        var major = await _context.Major.ToListAsync();

        return major;
    }

    public async Task<Major?> GetMajorWithStudentsAsync(int? id)
    {

        var major = await GetMajorAsync(id);

        if (major == null)
            return major;

        var chosenStudent = _context.MajorStudent
            .Where(ss => ss.MajorId == id)
            .Select(ss => ss.Student)
            .ToList();

        var availableStudent = _context.Student
            .Where(s => !chosenStudent.Contains(s))
            .ToList();

        major.MajorStudents = _context.MajorStudent
            .Where(x => x.MajorId == id)
            .ToList();

        major.AvailableStudents = availableStudent;

        return major;

    }

    public async Task<Major> CreateMajorAsync(int id, string name, int[] studentIdDst)
    {


        var chosenStudents = _context.Student
          .Where(s => studentIdDst.Contains(s.Id))
          .ToList();

        var availableStudents = _context.Student
            .Where(s => !studentIdDst.Contains(s.Id))
            .ToList();

        var newmajor = new Major()
        {
            Name = name,
            AvailableStudents = availableStudents
        };

        foreach (var chosenStud in chosenStudents)
        {
            newmajor.AddStudent(chosenStud);
        }

        _context.Add(newmajor);
        await _context.SaveChangesAsync();

        return newmajor;
    }





    public async Task DeleteMajor(int id)
    {
        var major = await GetMajorAsync(id);

        if (major == null)
        {
            return;
        }

        _context.Major.Remove(major);

        await _context.SaveChangesAsync();
    }

   

    public async Task UpdateMajorAsync(Major majorModel, int[] studentIdDst)
    {

        var major = await GetMajorAsync(majorModel.Id);

        if (major == null)
            return;


        major.Name = majorModel.Name;
       

        var chosenStudents = _context.Student
            .Where(s => studentIdDst.Contains(s.Id))
            .ToList();

        var majorStudent = _context.MajorStudent
            .Where(ss => ss.MajorId == majorModel.Id)
            .ToList();
        _context.MajorStudent.RemoveRange(majorStudent);

        foreach (var student in chosenStudents)
        {
            var majorStudents = new MajorStudent
            {
                Student = student,
                Major = major
            };
            _context.MajorStudent.Add(majorStudents);
        }

        var resultInt = _context.SaveChanges();

    }

    public bool MajorExists(int id)
    {
        throw new NotImplementedException();
    }

    #endregion


    #region Book

    public async Task<Book?> GetBookAsync(int? Id)
    {
        var book = await _context.Book
          .FirstOrDefaultAsync(m => m.Id == Id);

        return book;
    }

    public async Task<IList<Book>> GetOllBookAsync()
    {
        var lectureHalls = await _context.Book.ToListAsync();

        return lectureHalls;
    }

    public async Task CreateBookAsync(Book book)
    {
        _context.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        _context.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await GetBookAsync(id);

        if (book == null)
        {
            return;
        }

        _context.Book.Remove(book);

        await _context.SaveChangesAsync();
    }

    public bool BookExists(int id)
    {
        return _context.Book.Any(e => e.Id == id);
    }

    #endregion // Book


    #region Student

    public async Task<IList<Student>> GetOllStudentsAsync()
    {
        var students = await _context.Student.ToListAsync();
        return students;
    }

    public async Task<Student?> GetStudentAsync(int? id)
    {
        var student = await _context.Student
                   .FirstOrDefaultAsync(m => m.Id == id);

        return student;
    }

    public async Task<Student?> GetStudentWithSubjectsAsync(int? id)
    {
        var student = await GetStudentAsync(id);

        if (student == null)
            return student;

        var chosenSubjects = _context.StudentSubject
            .Where(ss => ss.StudentId == id)
            .Select(ss => ss.Subject)
            .ToList();

        var availableSubjects = _context.Subject
            .Where(s => !chosenSubjects.Contains(s))
            .ToList();

        student.StudentSubjects = _context.StudentSubject
            .Where(x => x.StudentId == id)
            .ToList();

        student.AvailableSubjects = availableSubjects;

        return student;

    }

    public async Task<bool> CreateStudentAsync(int id, string name, int age, string major, int[] subjectIdDst)
    {
        var chosenSubjects = _context.Subject
            .Where(s => subjectIdDst.Contains(s.Id))
            .ToList();

        var availableSubjects = _context.Subject
            .Where(s => !subjectIdDst.Contains(s.Id))
            .ToList();

        var student = new Student()
        {
            Id = id,
            Name = name,
            Age = age,
            Major = major,
            AvailableSubjects = availableSubjects
        };
        foreach (var chosenSubject in chosenSubjects)
        {
            student.AddSubject(chosenSubject);
        }

        _context.Add(student);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteStudentAsync(int? id)
    {
        var student = await _context.Student.FindAsync(id);

        if (student == null)
        {
            return false;
        }
       
         _context.Student.Remove(student);

        await _context.SaveChangesAsync();

        return true;
    }


    public bool EditStudent(int id, string name, int age, string major, int[] subjectIdDst)
    {
        var result = false;

        var student = _context.Student.Find(id);
        if (student != null)
        {
            student.Name = name;
            student.Age = age;
            student.Major = major;

            var chosenSubjects = _context.Subject
                .Where(s => subjectIdDst.Contains(s.Id))
                .ToList();

            var studentSubjects = _context.StudentSubject
                .Where(ss => ss.StudentId == id)
                .ToList();
            _context.StudentSubject.RemoveRange(studentSubjects);

            foreach (var subject in chosenSubjects)
            {
                var studentSubject = new StudentSubject
                {
                    Student = student,
                    Subject = subject
                };
                _context.StudentSubject.Add(studentSubject);
            }

            var resultInt = _context.SaveChanges();
            result = resultInt > 0;
        }

        return result;
    }

    public Student? DisplayStudent(int? id)
    {
        Student? student = null;
        try
        {
            student = _context.Student
                .FirstOrDefault(m => m.Id == id);
            if (student is not null)
            {
                var studentSubjects = _context.StudentSubject
                    .Where(ss => ss.StudentId == id)
                    .Include(ss => ss.Subject)
                    .ToList();
                student.StudentSubjects = studentSubjects;
            }
        }
        catch (Exception ex)
        {
           _logger.LogError("Exception caught in DisplayStudent: " + ex);
        }

        return student;
    }


    public bool StudentExists(int id)
    {
        var result = _context.Student.Any(e => e.Id == id);
        return result;
    }

    #endregion //Student


    #region Lecturer

    public async Task<Lecturer?> GetLecturerAsync(int? id)
    {
        var lecturer = await _context.Lecturer
                 .FirstOrDefaultAsync(m => m.Id == id);

        return lecturer;
    }

    public async Task<List<Lecturer>> GetOllLecturersAsync()
    {
        var lecturers = await _context.Lecturer.ToListAsync();
        return lecturers;
    }

    public async Task<Lecturer?> GetLecturerWithSubjectsAsync(int? id)
    {
        var lecturer = await GetLecturerAsync(id);

        if (lecturer == null)
            return lecturer;

        var chosenSubjects = _context.LecturerSubject
            .Where(ss => ss.LecturerId == id)
            .Select(ss => ss.Subject)
            .ToList();

        var availableSubjects = _context.Subject
            .Where(s => !chosenSubjects.Contains(s))
            .ToList();

        lecturer.LecturerSubjects = _context.LecturerSubject
            .Where(x => x.LecturerId == id)
            .ToList();

        lecturer.AvailableSubjects = availableSubjects;

        return lecturer;
    }

    public async Task<Lecturer> CreateLecturerAsync(Lecturer lecturer, int[] subjectIdDst)
    {
        var chosenSubjects = _context.Subject
            .Where(s => subjectIdDst.Contains(s.Id))
            .ToList();

        var availableSubjects = _context.Subject
            .Where(s => !subjectIdDst.Contains(s.Id))
            .ToList();

        foreach (var chosenSubject in chosenSubjects)
        {
            lecturer.AddSubject(chosenSubject);
        }

        _context.Add(lecturer);
        await _context.SaveChangesAsync();

        return lecturer;
    }

    public async Task UpdateLecturerAsync(Lecturer lecturerModel, int[] subjectIdDst)
    {
        var lecturer = await GetLecturerAsync(lecturerModel.Id);

        if (lecturer == null)
            return;

        lecturer.Name = lecturerModel.Name;
        lecturer.Age = lecturerModel.Age;


        var chosenSubjects = _context.Subject
            .Where(s => subjectIdDst.Contains(s.Id))
            .ToList();

        var lecturerSubject = _context.LecturerSubject
            .Where(ss => ss.LecturerId == lecturerModel.Id)
            .ToList();

        _context.LecturerSubject.RemoveRange(lecturerSubject);

        foreach (var subject in chosenSubjects)
        {
            var researchAreaStudents = new LecturerSubject
            {
                Subject = subject,
                Lecturer = lecturer
            };
            _context.LecturerSubject.Add(researchAreaStudents);
        }

        var resultInt = _context.SaveChanges();
    }

    public async Task<bool> DeleteLecturerAsync(int id)
    {
        var lecturer = await _context.Lecturer.FindAsync(id);

        if (lecturer == null)
        {
            return false;
        }

        _context.Lecturer.Remove(lecturer);

        await _context.SaveChangesAsync();

        return true;
    }

    public bool LecturerExists(int id)
    {
        var result = _context.Lecturer.Any(e => e.Id == id);
        return result;
    }

    #endregion

    #endregion 
}
