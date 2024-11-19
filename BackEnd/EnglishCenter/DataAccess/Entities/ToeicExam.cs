using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Exams")]
    public class ToeicExam
    {
        [Key]
        public long ToeicId { set; get; }

        [StringLength(50)]
        public string Name { set; get; } = null!;

        public int Code { set; get; }

        public int Year { set; get; }

        public long? DirectionId { set; get; }

        [Column("Completed_Num")]
        public int CompletedNum { set; get; } = 0;

        public int Point { set; get; } = 990;

        [Column("Time_Minutes")]
        public int TimeMinutes { set; get; } = 120;

        [ForeignKey("DirectionId")]
        [InverseProperty("ToeicExams")]
        public virtual ToeicDirection? ToeicDirection { set; get; }

        [InverseProperty("ToeicExam")]
        public virtual ICollection<QuesToeic> QuesToeic { set; get; } = new List<QuesToeic>();

        [InverseProperty("ToeicExam")]
        public virtual ICollection<ToeicAttempt> ToeicAttempts { set; get; } = new List<ToeicAttempt>();
    }
}
