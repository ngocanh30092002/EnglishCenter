using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class Examination
    {
        [Key]
        public long ExamId { set; get; }

        public long ContentId { set; get; }

        [ForeignKey("ContentId")]
        [InverseProperty("Examination")]
        public virtual CourseContent CourseContent { set; get; } = null!;

        public long ToeicId { set; get; }

        public string Title { set; get; } = null!;

        public TimeOnly Time { set; get; } = TimeOnly.MinValue;

        public string? Description { set; get; }

        [InverseProperty("Examination")]
        public virtual ICollection<LearningProcess> LearningProcesses { set; get; } = new List<LearningProcess>();
    }
}
