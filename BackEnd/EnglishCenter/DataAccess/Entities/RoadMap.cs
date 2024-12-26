using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("RoadMaps")]
    public class RoadMap
    {
        [Key]
        public long RoadMapId { set; get; }
        public string Name { set; get; } = null!;

        [StringLength(10)]
        public string CourseId { set; get; } = null!;

        [ForeignKey("CourseId")]
        [InverseProperty("RoadMaps")]
        public virtual Course Course { set; get; } = null!;

        public int Order { set; get; } = 1;

        [InverseProperty("RoadMap")]
        public virtual ICollection<RoadMapExam> RoadMapExams { set; get; } = new List<RoadMapExam>();
    }
}
