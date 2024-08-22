using System;
using System.Collections.Generic;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Database;

public partial class EnglishCenterContext : IdentityDbContext<User>
{
    public EnglishCenterContext()
    {
    }

    public EnglishCenterContext(DbContextOptions<EnglishCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<PreExamScore> PreExamScores { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentInClass> StudentInClasses { get; set; }

    public virtual DbSet<SubQuestion3> SubQuestion3s { get; set; }

    public virtual DbSet<SubQuestion4> SubQuestion4s { get; set; }

    public virtual DbSet<SubQuestion6> SubQuestion6s { get; set; }

    public virtual DbSet<SubQuestion7> SubQuestion7s { get; set; }

    public virtual DbSet<ToeicExam> ToeicExams { get; set; }

    public virtual DbSet<ToeicPart1> ToeicPart1s { get; set; }

    public virtual DbSet<ToeicPart2> ToeicPart2s { get; set; }

    public virtual DbSet<ToeicPart3> ToeicPart3s { get; set; }

    public virtual DbSet<ToeicPart4> ToeicPart4s { get; set; }

    public virtual DbSet<ToeicPart5> ToeicPart5s { get; set; }

    public virtual DbSet<ToeicPart6> ToeicPart6s { get; set; }

    public virtual DbSet<ToeicPart7> ToeicPart7s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:EnglishCenter");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasOne(d => d.StuClass).WithMany(p => p.Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_StudentInClass");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasOne(d => d.Course).WithMany(p => p.Classes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_Classes");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK_Course_ID");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Courses");

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Students");
        });

        modelBuilder.Entity<PreExamScore>(entity =>
        {
            entity.HasKey(e => e.PreScoreId).HasName("PK_PreExamScores_1");

            entity.HasOne(d => d.Course).WithMany(p => p.PreExamScores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreExamScores_Courses");

            entity.HasOne(d => d.User).WithMany(p => p.PreExamScores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreExamScores_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Student_ID");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_StudentUser");
        });

        modelBuilder.Entity<StudentInClass>(entity =>
        {
            entity.HasKey(e => e.StuClassId).HasName("PK_StudentInClass_1");

            entity.HasOne(d => d.PreScore).WithOne(p => p.StudentInClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentInClass_PreExamScores");
        });

        modelBuilder.Entity<SubQuestion3>(entity =>
        {
            entity.Property(e => e.SubId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Pre).WithMany(p => p.SubQuestion3s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubQuestion3_Toeic_Part_3");
        });

        modelBuilder.Entity<SubQuestion4>(entity =>
        {
            entity.Property(e => e.SubId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Pre).WithMany(p => p.SubQuestion4s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubQuestion4_Toeic_Part_4");
        });

        modelBuilder.Entity<SubQuestion6>(entity =>
        {
            entity.HasOne(d => d.Pre).WithMany(p => p.SubQuestion6s).HasConstraintName("FK_SubQuestion6_Toeic_Part_6");
        });

        modelBuilder.Entity<SubQuestion7>(entity =>
        {
            entity.HasOne(d => d.Pre).WithMany(p => p.SubQuestion7s).HasConstraintName("FK_SubQuestion7_Toeic_Part_7");
        });

        modelBuilder.Entity<ToeicPart1>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart1s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toeic_Part_1_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart2>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toeic_Part_2_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart3>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart3s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toeic_Part_3_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart4>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart4s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toeic_Part_4_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart5>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart5s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toeic_Part_5_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart6>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart6s).HasConstraintName("FK_Toeic_Part_6_ToeicExams");
        });

        modelBuilder.Entity<ToeicPart7>(entity =>
        {
            entity.HasOne(d => d.Toeic).WithMany(p => p.ToeicPart7s).HasConstraintName("FK_Toeic_Part_7_ToeicExams");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public static implicit operator EnglishCenterContext(UserManager<User> v)
    {
        throw new NotImplementedException();
    }
}
