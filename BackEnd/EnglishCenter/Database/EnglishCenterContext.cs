﻿using System;
using System.Collections.Generic;
using EnglishCenter.Models;
using EnglishCenter.Models.IdentityModel;
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

    public virtual DbSet<Homework> Homework { get; set; }

    public virtual DbSet<AssignQue> AssignQues { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<CourseContent> CourseContents { set; get; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<EnrollStatus> EnrollStatuses { get; set; }

    public virtual DbSet<Notification> Notifications { set; get; }

    public virtual DbSet<NotiStudent> NotiStudents { set; get; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<QuesLcAudio> QuesLcAudios { get; set; }

    public virtual DbSet<QuesLcConversation> QuesLcConversations { get; set; }

    public virtual DbSet<QuesLcImage> QuesLcImages { get; set; }

    public virtual DbSet<QuesRcDouble> QuesRcDoubles { get; set; }

    public virtual DbSet<QuesRcSingle> QuesRcSingles { get; set; }

    public virtual DbSet<QuesRcTriple> QuesRcTriples { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<ScoreHistory> ScoreHistories { get; set; }

    public virtual DbSet<StuInClass> StuInClasses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SubLcConversation> SubLcConversations { get; set; }

    public virtual DbSet<SubRcDouble> SubRcDoubles { get; set; }

    public virtual DbSet<SubRcTriple> SubRcTriples { get; set; }

    public virtual DbSet<Group> Groups { set; get; }

    public virtual DbSet<ScheduleEvent> ScheduleEvents { set; get; }

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

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.HasOne(c => c.Course).WithMany(c => c.CourseContents).HasConstraintName("FK_CourseContent_Courses");
        });

        modelBuilder.Entity<AssignQue>(entity =>
        {
            entity.HasOne(d => d.Ques).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_LC_Audio");

            entity.HasOne(d => d.QuesNavigation).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_LC_Conversation");

            entity.HasOne(d => d.Ques1).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_LC_Image");

            entity.HasOne(d => d.Ques2).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_RC_Double");

            entity.HasOne(d => d.Ques3).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_RC_Single");

            entity.HasOne(d => d.Ques4).WithOne(p => p.AssignQue).HasConstraintName("FK_Assign_Ques_Ques_RC_Triple");

            entity.HasOne(d => d.QuesType).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Question_Type");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Assignment");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasOne(d => d.CourseContent).WithMany(p => p.Assignments).HasConstraintName("FK_Assignment_CourseContent");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasOne(d => d.StuInClass)
                .WithMany(p => p.Attendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_StuInClass");
        });

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.HasOne(a => a.Attendance)
                  .WithMany(a => a.HomeworkList)
                  .HasConstraintName("FK_Homework_Attendance");

            entity.HasOne(a => a.Assignment)
                  .WithMany(a => a.HomeworkList)
                  .HasConstraintName("FK_Homework_Assignment");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasOne(d => d.Course).WithMany(p => p.Classes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Classes_Courses");
        });

        modelBuilder.Entity<EnrollStatus>(entity =>
        {
            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.EnrollId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Class).WithMany(p => p.Enrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Classes");

            entity.HasOne(d => d.Status).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_EnrollStatus");

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments).HasConstraintName("FK_Enrollment_Students");
        });

        modelBuilder.Entity<NotiStudent>(entity =>
        {
            entity.Property(e => e.NotiStuId).ValueGeneratedOnAdd();

            entity.HasOne(n => n.Student).WithMany(s => s.NotiStudents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NotiStudent_Students");

            entity.HasOne(n => n.Notification).WithMany(n => n.NotiStudents)
                .HasConstraintName("FK_NotiStudent_Notifications");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.Property(e => e.QuesTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ScoreHistory>(entity =>
        {
            entity.HasKey(e => e.ScoreHisId).HasName("PK_Table_1");
        });

        modelBuilder.Entity<StuInClass>(entity =>
        {
            entity.HasOne(d => d.Class).WithMany(p => p.StuInClasses).HasConstraintName("FK_StuInClass_Classes");

            entity.HasOne(d => d.ScoreHis).WithOne(p => p.StuInClass).HasConstraintName("FK_StuInClass_ScoreHistory");

            entity.HasOne(d => d.User).WithMany(p => p.StuInClasses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_StuInClass_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasOne(d => d.User).WithOne(p => p.Student).HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasMany(g => g.Students)
                  .WithMany(s => s.Groups)
                  .UsingEntity<Dictionary<string, object>>(
                    "GroupStudent",
                    j => j
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("UserId"),
                    j => j
                        .HasOne<Group>()
                        .WithMany()
                        .HasForeignKey("GroupId"),
                    j =>
                    {
                        j.HasKey("GroupId", "UserId");
                        j.ToTable("GroupStudent");
                    });
        });

        modelBuilder.Entity<SubLcConversation>(entity =>
        {
            entity.HasOne(d => d.PreQues).WithMany(p => p.SubLcConversations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_LC_Conversation_Ques_LC_Conversation");
        });

        modelBuilder.Entity<SubRcDouble>(entity =>
        {
            entity.HasOne(d => d.PreQues).WithMany(p => p.SubRcDoubles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_RC_Double_Ques_RC_Double");
        });

        modelBuilder.Entity<SubRcTriple>(entity =>
        {
            entity.HasOne(d => d.PreQues).WithMany(p => p.SubRcTriples)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_RC_Triple_Ques_RC_Triple");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
