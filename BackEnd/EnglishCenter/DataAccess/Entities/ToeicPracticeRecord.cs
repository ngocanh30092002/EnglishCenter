using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Practice_Records")]
    public class ToeicPracticeRecord
    {
        [Key]
        public long RecordId { set; get; }

        public long SubQueId { set; get; }

        [ForeignKey("SubQueId")]
        [InverseProperty("ToeicPracticeRecords")]
        public virtual SubToeic SubToeic { set; get; } = null!;

        [StringLength(1)]
        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; } = false;

        public long AttemptId { set; get; }

        [ForeignKey("AttemptId")]
        [InverseProperty("ToeicPracticeRecords")]
        public virtual ToeicAttempt ToeicAttempt { set; get; } = null!;
    }
}
