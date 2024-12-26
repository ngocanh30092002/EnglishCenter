using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("User_Attempts")]
    public class UserAttempt
    {
        [Key]
        public long AttemptId { set; get; }

        [StringLength(100)]
        public string UserId { set; get; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("UserAttempts")]
        public virtual User User { get; set; } = null!;

        public long? ToeicId { set; get; }
        [ForeignKey("ToeicId")]
        [InverseProperty("UserAttempts")]
        public virtual ToeicExam? ToeicExam { get; set; }

        public long? RoadMapExamId { set; get; }

        [ForeignKey("RoadMapExamId")]
        [InverseProperty("UserAttempts")]
        public virtual RoadMapExam? RoadMapExam { get; set; }

        public int? ListeningScore { set; get; } = 0;

        public int? ReadingScore { set; get; } = 0;

        public DateTime Date { set; get; }

        public bool IsSubmitted { set; get; } = false;

        [InverseProperty("UserAttempt")]
        public ICollection<AttemptRecord> AttemptRecords { set; get; } = new List<AttemptRecord>();
    }
}
