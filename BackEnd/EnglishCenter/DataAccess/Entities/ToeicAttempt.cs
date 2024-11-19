using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Attempts")]
    public class ToeicAttempt
    {
        [Key]
        public long AttemptId { set; get; }

        [StringLength(100)]
        public string UserId { set; get; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("ToeicAttempts")]
        public virtual User User { get; set; } = null!;

        public long ToeicId { set; get; }
        [ForeignKey("ToeicId")]
        [InverseProperty("ToeicAttempts")]
        public virtual ToeicExam ToeicExam { get; set; } = null!;

        public int? ListeningScore { set; get; } = 0;

        public int? ReadingScore { set; get; } = 0;

        public DateTime Date { set; get; }

        public bool IsSubmitted { set; get; } = false;

        [InverseProperty("ToeicAttempt")]
        public ICollection<ToeicPracticeRecord> ToeicPracticeRecords { set; get; } = new List<ToeicPracticeRecord>();
    }
}
