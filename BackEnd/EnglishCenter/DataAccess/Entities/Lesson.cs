using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Lessons")]
    public class Lesson
    {
        [Key]
        public long LessonId { set; get; }

        [StringLength(10)]
        public string ClassId { set; get; } = null!;

        [ForeignKey("ClassId")]
        [InverseProperty("Lessons")]
        public virtual Class Class { set; get; } = null!;

        public DateOnly Date { set; get; }

        [Range(1, 12)]
        public int StartPeriod { set; get; }

        [Range(1, 12)]
        public int EndPeriod { set; get; }

        public string? Topic { set; get; }

        public long ClassRoomId { set; get; }

        [ForeignKey("ClassRoomId")]
        [InverseProperty("Lessons")]
        public ClassRoom ClassRoom { set; get; } = null!;

        [InverseProperty("Lesson")]
        public virtual ICollection<ClassMaterial> ClassMaterials { set; get; } = new List<ClassMaterial>();

        [InverseProperty("Lesson")]
        public virtual ICollection<Homework> HomeworkTasks { set; get; } = new List<Homework>();

        [InverseProperty("Lesson")]
        public virtual ICollection<SubmissionTask> SubmissionTasks { set; get; } = new List<SubmissionTask>();
    }
}
