using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class Homework
    {
        [Key]
        public long HomeworkId { set; get; }

        [StringLength(10)]
        public string ClassId { set; get; } = null!;

        [ForeignKey("ClassId")]
        [InverseProperty("HomeworkTasks")]
        public virtual Class Class { set; get; } = null!;
        
        public DateTime StartTime { set; get; } = DateTime.Now;
        
        public DateTime EndTime { set; get; }

        public int LateSubmitDays { set; get; } = 0;
    }
}
