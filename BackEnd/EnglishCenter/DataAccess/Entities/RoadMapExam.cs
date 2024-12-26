using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("RoadMapExams")]
    public class RoadMapExam
    {
        [Key]
        public long RoadMapExamId { set; get; }

        public string Name { set; get; } = null!;

        public long? DirectionId { set; get; }

        [ForeignKey("DirectionId")]
        [InverseProperty("RoadMapExams")]
        public virtual ToeicDirection? ToeicDirection { set; get; }

        public int Point { set; get; } = 990;

        [Column("Time_Minutes")]
        public double TimeMinutes { set; get; } = 120;

        [Column("Completed_Num")]
        public int CompletedNum { set; get; } = 0;

        public long RoadMapId { set; get; }

        [ForeignKey("RoadMapId")]
        [InverseProperty("RoadMapExams")]
        public virtual RoadMap RoadMap { set; get; } = null!;

        [InverseProperty("RoadMapExam")]
        public virtual ICollection<UserAttempt> UserAttempts { set; get; } = new List<UserAttempt>();

        [InverseProperty("RoadMapExam")]
        public virtual ICollection<RandomQuesToeic> RandomQues { set; get; } = new List<RandomQuesToeic>();
    }
}
