using EnglishCenter.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Database;

public class EnglishCenterContext : IdentityDbContext<User>
{
    public EnglishCenterContext()
    {
    }

    public EnglishCenterContext(DbContextOptions<EnglishCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Homework> Homework { set; get; }

    public virtual DbSet<HwSubmission> HwSubmissions { set; get; }

    public virtual DbSet<HomeQue> HomeQues { set; get; }

    public virtual DbSet<HwSubRecord> HwSubRecords { set; get; }

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

    public virtual DbSet<LearningProcess> LearningProcesses { set; get; }

    public virtual DbSet<AssignmentRecord> AssignmentRecords { set; get; }

    public virtual DbSet<ToeicConversion> ToeicConversion { set; get; }

    public virtual DbSet<ToeicRecord> ToeicRecords { set; get; }

    public virtual DbSet<Examination> Examinations { set; get; }

    public virtual DbSet<ToeicExam> ToeicExams { set; get; }

    public virtual DbSet<QuesToeic> QuesToeic { set; get; }

    public virtual DbSet<SubToeic> SubToeic { set; get; }

    public virtual DbSet<AnswerToeic> AnswerToeic { set; get; }

    public virtual DbSet<ToeicDirection> Directions { set; get; }

    public virtual DbSet<ToeicPracticeRecord> ToeicPracticeRecords { set; get; }

    public virtual DbSet<ToeicAttempt> ToeicAttempts { set; get; }

    public virtual DbSet<ChatMessage> ChatMessages { set; get; }

    public virtual DbSet<ChatFile> ChatFiles { set; get; }

    public virtual DbSet<ClassSchedule> ClassSchedules { set; get; }

    public virtual DbSet<Lesson> Lessons { set; get; }

    public virtual DbSet<ClassRoom> ClassRooms { set; get; }

    public virtual DbSet<Period> Periods { set; get; }

    public virtual DbSet<ClassMaterial> ClassMaterials { set; get; }

    public virtual DbSet<SubmissionTask> SubmissionTask { set; get; }

    public virtual DbSet<SubmissionFile> SubmissionFiles { set; get; }

    public virtual DbSet<UserWord> UserWords { set; get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Name=ConnectionStrings:EnglishCenter");
    }

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

        modelBuilder.Entity<UserWord>(entity =>
        {
            entity.HasOne(u => u.User).WithMany(u => u.UserWords).HasConstraintName("FK_UserWord_User");
        });

        modelBuilder.Entity<ClassMaterial>(entity =>
        {
            entity.HasOne(c => c.Class).WithMany(c => c.ClassMaterials).HasConstraintName("FK_ClassMaterials_Classes");
            entity.HasOne(c => c.Lesson).WithMany(c => c.ClassMaterials).HasConstraintName("FK_ClassMaterials_Lessons");
        });

        modelBuilder.Entity<SubmissionFile>(entity =>
        {
            entity.HasOne(s => s.SubmissionTask).WithMany(l => l.SubmissionFiles).HasConstraintName("FK_SubmissionTasks_SubmissionFiles");
        });

        modelBuilder.Entity<SubmissionTask>(entity =>
        {
            entity.HasOne(s => s.Lesson).WithMany(l => l.SubmissionTasks).HasConstraintName("FK_SubmissionTask_Lessons");
        });

        modelBuilder.Entity<ClassSchedule>(entity =>
        {
            entity.HasOne(c => c.Class).WithMany(c => c.ClassSchedules).HasConstraintName("FK_ClassSchedule_Class");
            entity.HasOne(c => c.ClassRoom).WithMany(c => c.ClassSchedules).HasConstraintName("FK_ClassSchedule_ClassRoom");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasOne(c => c.Class).WithMany(c => c.Lessons).HasConstraintName("FK_Lessons_Class");
            entity.HasOne(c => c.ClassRoom).WithMany(c => c.Lessons).HasConstraintName("FK_Lessons_ClassRoom");

        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.Property(e => e.PeriodId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(c => c.ChatFile).WithOne(f => f.ChatMessage)
                .HasConstraintName("FK_ChatMessage_ChatFile");

            entity.HasOne(c => c.Sender).WithMany(u => u.SentMessages)
                .HasConstraintName("FK_ChatMessage_Sender")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.Receiver).WithMany(u => u.ReceivedMessages)
                .HasConstraintName("FK_ChatMessage_Receiver");
        });

        modelBuilder.Entity<ToeicExam>(entity =>
        {
            entity.HasOne(q => q.ToeicDirection).WithMany(t => t.ToeicExams).HasConstraintName("FK_Toeic_Exam_Toeic_Directions");
        });

        modelBuilder.Entity<QuesToeic>(entity =>
        {
            entity.HasOne(q => q.ToeicExam).WithMany(t => t.QuesToeic).HasConstraintName("FK_Ques_Toeic_Toeic_Exam");
        });

        modelBuilder.Entity<SubToeic>(entity =>
        {
            entity.HasOne(q => q.QuesToeic).WithMany(t => t.SubToeicList).HasConstraintName("FK_Sub_Toeic_Ques_Toeic");
        });

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.Property(c => c.Type).HasDefaultValue(1);

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

        modelBuilder.Entity<LearningProcess>(entity =>
        {
            entity.HasOne(p => p.Enrollment).WithMany(e => e.LearningProcesses).HasConstraintName("FK_LearningProcess_Enrollment");

            entity.HasOne(p => p.Assignment).WithMany(e => e.LearningProcesses).HasConstraintName("FK_LearningProcess_Assignment");

            entity.HasOne(p => p.Examination).WithMany(e => e.LearningProcesses).HasConstraintName("FK_LearningProcess_Examination");
        });

        modelBuilder.Entity<AssignmentRecord>(entity =>
        {
            entity.HasOne(p => p.LearningProcess).WithMany(e => e.AssignmentRecords).HasConstraintName("FK_AssignmentRecord_LearningProcess");

            entity.HasOne(p => p.AssignQue)
                .WithMany(e => e.AssignmentRecords)
                .HasConstraintName("FK_AssignmentRecords_AssignQue")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ToeicRecord>(entity =>
        {
            entity.HasOne(p => p.LearningProcess).WithMany(e => e.ToeicRecords).HasConstraintName("FK_ToeicRecord_LearningProcess");
            entity.HasOne(p => p.SubToeic).WithMany(e => e.ToeicRecords).HasConstraintName("FK_ToeicRecord_SubToeic");
        });

        modelBuilder.Entity<ToeicPracticeRecord>(entity =>
        {
            entity.HasOne(r => r.SubToeic).WithMany(e => e.ToeicPracticeRecords).HasConstraintName("FK_ToeicPracticeRecord_SubToeic");
            entity.HasOne(r => r.ToeicAttempt)
                  .WithMany(e => e.ToeicPracticeRecords)
                  .HasConstraintName("FK_ToeicPracticeRecords_Attempt")
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ToeicAttempt>(entity =>
        {
            entity.HasOne(a => a.User)
                   .WithMany(u => u.ToeicAttempts)
                   .HasConstraintName("FK_ToeicAttempted_User");
            entity.HasOne(a => a.ToeicExam)
                  .WithMany(u => u.ToeicAttempts)
                  .HasConstraintName("FK_ToeicAttempted_ToeicExams");
        });

        modelBuilder.Entity<Examination>(entity =>
        {
            entity.HasOne(p => p.CourseContent).WithOne(e => e.Examination)
                .HasConstraintName("FK_Examination_CourseContent")
                .HasForeignKey<Examination>(e => e.ContentId);
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
            entity.HasOne(d => d.Lesson).WithMany(c => c.HomeworkTasks).HasConstraintName("FK_Homework_Lessons");
            entity.HasMany(d => d.Submissions).WithOne(c => c.Homework).HasConstraintName("FK_HwSubmission_Homework");
        });

        modelBuilder.Entity<HwSubmission>(entity =>
        {
            entity.HasOne(s => s.Enrollment)
                .WithMany(e => e.Submissions)
                .HasConstraintName("FK_HW_Submission_Enrollment")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HwSubRecord>(entity =>
        {
            entity.HasOne(d => d.HwSubmission).WithMany(c => c.SubRecords).HasConstraintName("FK_HwSubRecord_HwSubmission");

            entity.HasOne(p => p.HomeQue)
              .WithMany(e => e.SubRecords)
              .HasConstraintName("FK_Hw_Sub_Record_HomeQue")
              .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<HomeQue>(entity =>
        {
            entity.HasOne(q => q.QuesImage).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_LC_Image");
            entity.HasOne(q => q.QuesAudio).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_LC_Audio");
            entity.HasOne(q => q.QuesConversation).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_LC_Conversation");
            entity.HasOne(q => q.QuesSentence).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_RC_Sentence");
            entity.HasOne(q => q.QuesSingle).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_RC_Single");
            entity.HasOne(q => q.QuesDouble).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_RC_Double");
            entity.HasOne(q => q.QuesTriple).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Ques_RC_Triple");
            entity.HasOne(q => q.Homework).WithMany(q => q.HomeQues).HasConstraintName("FK_Home_Ques_Homework");
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
    }
}
