using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class CourseContent
    {
        [Key]
        public long ContentId { set; get; }

        [Column(TypeName = "nvarchar(200)")]
        public string Title { set; get; }

        [Column(TypeName = "nvarchar(200)")]
        public string Content { set; get; }

        public int NoNum { set; get; }

        public string CourseId { get; set; } = null!;

        public int Type { set; get; } = 1;

        [ForeignKey("CourseId")]
        [InverseProperty("CourseContents")]
        public virtual Course Course { set; get; }

        [InverseProperty("CourseContent")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
