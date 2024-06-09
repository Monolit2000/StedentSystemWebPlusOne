using Students.Common.Models;

namespace Students.Interfaces;

public interface IDatabaseService
{

    #region Subject

    Task<List<Subject>> GetOllSubjectsAsync();

    Task<Subject?> GetSubjectAsync(int? id);

    Task<Subject?> GetSubjectWithStudentsAsync(int? id);

    Task CreateSubjectAsync(Subject subject, int[] subjectIdDst);

    Task UpdateSubjectAsync(Subject subject, int[] subjectIdDst);

    Task DeleteSubjectAsync(int id);

    bool SubjectExists(int id);

    List<Subject> GetChosenSubjects(int? id);

    List<Subject> GetAvailableSubjects(int? id);

    List<StudentSubject> GetStudentSubjects(int? id);


    #endregion

    #region ResearchWorker

    Task<ResearchWorker?> GetResearchWorkerAsync(int? Id);

    Task<IList<ResearchWorker>> GetOllResearchWorkerAsync();

    Task CreateResearchWorkerAsync(ResearchWorker researchWorker);

    Task UpdateResearchWorkerAsync(ResearchWorker researchWorker);

    Task DeleteResearchWorkerAsync(int? id);

    bool ResearchWorkerExists(int id);


    #endregion

    #region Major

    Task<Major?> GetMajorAsync(int? Id);

    Task<IList<Major>> GetOllMajorsAsync();

    Task<Major?> GetMajorWithStudentsAsync(int? id);

    Task<Major> CreateMajorAsync(int id, string name, int[] studentIdDst);

    Task UpdateMajorAsync(Major lectureHall, int[] studentIdDst);

    Task DeleteMajor(int id);

    bool MajorExists(int id);



    #endregion

    #region Student

    Student? DisplayStudent(int? id);

    Task<IList<Student>> GetOllStudentsAsync();

    Task<Student?> GetStudentAsync(int? id);

    Task<Student?> GetStudentWithSubjectsAsync(int? id);

    Task<bool> CreateStudentAsync(int id, string name, int age, string major, int[] subjectIdDst);

    Task<bool> DeleteStudentAsync(int? id);

    bool EditStudent(int id, string name, int age, string major, int[] subjectIdDst);

    bool StudentExists(int id);

    #endregion

    #region Lecturer

    Task<List<Lecturer>> GetOllLecturersAsync();

    Task<Lecturer?> GetLecturerAsync(int? id);

    Task<Lecturer?> GetLecturerWithSubjectsAsync(int? id);

    Task<Lecturer> CreateLecturerAsync(Lecturer subject, int[] subjectIdDst);

    Task UpdateLecturerAsync(Lecturer subject, int[] subjectIdDst);

    Task<bool> DeleteLecturerAsync(int id);

    bool LecturerExists(int id);


    #endregion
}
