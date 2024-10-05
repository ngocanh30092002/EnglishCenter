using System;
using System.Collections.Generic;
using EnglishCenter.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Database;

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

    public virtual DbSet<Teacher> Teachers { set; get; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<EnrollStatus> EnrollStatuses { get; set; }

    public virtual DbSet<Notification> Notifications { set; get; }

    public virtual DbSet<NotiStudent> NotiStudents { set; get; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<QuesLcAudio> QuesLcAudios { get; set; }

    public virtual DbSet<AnswerLcAudio> AnswerLcAudios { set; get; }

    public virtual DbSet<QuesLcConversation> QuesLcConversations { get; set; }

    public virtual DbSet<QuesLcImage> QuesLcImages { get; set; }

    public virtual DbSet<AnswerLcImage> AnswerLcImages { set; get; }

    public virtual DbSet<QuesRcDouble> QuesRcDoubles { get; set; }

    public virtual DbSet<QuesRcSentence> QuesRcSentences { set; get; }

    public virtual DbSet<QuesRcSingle> QuesRcSingles { get; set; }

    public virtual DbSet<QuesRcTriple> QuesRcTriples { get; set; }

    public virtual DbSet<ScoreHistory> ScoreHistories { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SubLcConversation> SubLcConversations { get; set; }

    public virtual DbSet<AnswerLcConversation> AnswerLcConversations { get; set; }

    public virtual DbSet<SubRcSingle> SubRcSingles { set; get; }

    public virtual DbSet<AnswerRcSingle> AnswerRcSingles { set; get; }

    public virtual DbSet<AnswerRcSentence> AnswerRcSentences { set; get; }

    public virtual DbSet<AnswerRcDouble> AnswerRcDoubles { set; get; }

    public virtual DbSet<AnswerRcTriple> AnswerRcTriples { set; get; }

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
            entity.HasOne(d => d.QuesAudio).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_LC_Audio");

            entity.HasOne(d => d.QuesConversation).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_LC_Conversation");

            entity.HasOne(d => d.QuesImage).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_LC_Image");

            entity.HasOne(d => d.QuesDouble).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_RC_Double");

            entity.HasOne(d => d.QuesSentence).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_RC_Sentence");

            entity.HasOne(d => d.QuesSingle).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_RC_Single");

            entity.HasOne(d => d.QuesTriple).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Ques_RC_Triple");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignQues).HasConstraintName("FK_Assign_Ques_Assignment");
        });

        modelBuilder.Entity<QuesLcImage>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.QuesLcImage).HasConstraintName("FK_Ques_LC_Answer_Image");
        });

        modelBuilder.Entity<QuesLcAudio>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.QuesLcAudio).HasConstraintName("FK_Ques_LC_Answer_Audio");
        });

        modelBuilder.Entity<SubLcConversation>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.SubLcConversation).HasConstraintName("FK_Sub_Ques_LC_Answer_Conversation");
        });

        modelBuilder.Entity<QuesRcSentence>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.QuesRcSentence).HasConstraintName("FK_Ques_RC_Answer_Sentence");
        });

        modelBuilder.Entity<SubRcSingle>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.SubRcSingle).HasConstraintName("FK_Sub_Ques_RC_Answer_Single");
        });

        modelBuilder.Entity<SubRcDouble>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.SubRcDouble).HasConstraintName("FK_Sub_Ques_RC_Answer_Double");
        });

        modelBuilder.Entity<SubRcTriple>(entity =>
        {
            entity.HasOne(d => d.Answer).WithOne(d => d.SubRcTriple).HasConstraintName("FK_Sub_Ques_RC_Answer_Triple");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasOne(d => d.CourseContent).WithMany(p => p.Assignments).HasConstraintName("FK_Assignment_CourseContent");
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

            entity.HasOne(t => t.Teacher).WithMany(c => c.Classes)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Classes_Teachers");
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

            entity.HasOne(e => e.ScoreHis).WithOne(s => s.Enrollment).HasConstraintName("FK_Enrollment_ScoreHis");

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

        modelBuilder.Entity<ScoreHistory>(entity =>
        {
            entity.HasKey(e => e.ScoreHisId).HasName("PK_Table_1");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasOne(d => d.User).WithOne(p => p.Student).HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasOne(d => d.User).WithOne(p => p.Teacher)
            .HasConstraintName("FK_Teachers_Users");
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

        modelBuilder.Entity<SubRcSingle>(entity =>
        {
            entity.HasOne(d => d.PreQues).WithMany(p => p.SubRcSingles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub_RC_Double_Ques_RC_Single");
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
