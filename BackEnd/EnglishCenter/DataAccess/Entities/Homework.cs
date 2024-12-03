using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class Homework
    {
        [Key]
        public long HomeworkId { set; get; }

        public DateTime StartTime { set; get; } = DateTime.Now;

        public DateTime EndTime { set; get; }

        public int LateSubmitDays { set; get; } = 0;

        public TimeOnly Time { set; get; } = TimeOnly.MinValue;

        public TimeOnly ExpectedTime { set; get; } = TimeOnly.MinValue;

        [Range(0, 100)]
        [Required]
        public int AchievedPercentage { set; get; } = 0;

        [Column(TypeName = "nvarchar(200)")]
        public string Title { set; get; } = null!;

        [InverseProperty("Homework")]
        public virtual ICollection<HwSubmission> Submissions { set; get; } = new List<HwSubmission>();

        [InverseProperty("Homework")]
        public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();

        public long LessonId { set; get; }

        [ForeignKey("LessonId")]
        [InverseProperty("HomeworkTasks")]
        public virtual Lesson Lesson { set; get; } = null!;

        public string? Image { set; get; }

        public int Type { set; get; } = 1;
    }
}
