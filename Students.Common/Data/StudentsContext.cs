using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Students.Common.Models;
using System.Drawing;
using System.Reflection;
using System.Security.Cryptography;

namespace Students.Common.Data;

public class StudentsContext : DbContext
{
    public StudentsContext(DbContextOptions<StudentsContext> options)
        : base(options)
    {
    }


    public DbSet<MajorStudent> MajorStudent { get; set;} = default!;
    public DbSet<Major> Major { get; set; } = default!;


    public DbSet<Subject> Subject { get; set; } = default!;
    public DbSet<Student> Student { get; set; } = default!;
    public DbSet<StudentSubject> StudentSubject { get; set; } = default!;


    public DbSet<Lecturer> Lecturer { get; set; }
    public DbSet<LecturerSubject> LecturerSubject { get; set; }

    public DbSet<ResearchWorker> ResearchWorker { get; set; } = default!;  


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region StudentSubject

        modelBuilder.Entity<StudentSubject>()
            .HasKey(ss => new { ss.StudentId, ss.SubjectId });

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.StudentId);

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Subject)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.SubjectId);

        #endregion


        #region MajorStudent
        modelBuilder.Entity<MajorStudent>()
            .HasKey(rs => new { rs.MajorId, rs.StudentId });

        modelBuilder.Entity<MajorStudent>()
           .HasOne(rs => rs.Major)
           .WithMany(r => r.MajorStudents)
           .HasForeignKey(ls => ls.MajorId);

        modelBuilder.Entity<MajorStudent>()
          .HasOne(rs => rs.Student)
          .WithMany(r => r.MajorStudents)
          .HasForeignKey(ls => ls.StudentId);

        #endregion


        #region  LecturerSubject

        modelBuilder.Entity<LecturerSubject>()
           .HasKey(rs => new { rs.LecturerId, rs.SubjectId });

        modelBuilder.Entity<LecturerSubject>()
           .HasOne(rs => rs.Lecturer)
           .WithMany(r => r.LecturerSubjects)
           .HasForeignKey(ls => ls.LecturerId);

        modelBuilder.Entity<LecturerSubject>()
          .HasOne(rs => rs.Subject)
          .WithMany(r => r.LecturerSubjects)
          .HasForeignKey(ls => ls.SubjectId);

        #endregion

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
