using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("SubmissionTasks")]
    public class SubmissionTask
    {
        [Key]
        public long SubmissionId { set; get; }
        public string Title { set; get; } = null!;
        public string? Description { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public long LessonId { set; get; }

        [ForeignKey("LessonId")]
        [InverseProperty("SubmissionTasks")]
        public virtual Lesson Lesson { set; get; } = null!;

        [InverseProperty("SubmissionTask")]
        public virtual ICollection<SubmissionFile> SubmissionFiles { set; get; } = new List<SubmissionFile>();
    }
}
