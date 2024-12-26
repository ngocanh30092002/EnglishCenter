using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Attempt_Records")]
    public class AttemptRecord
    {
        [Key]
        public long RecordId { set; get; }

        public long SubQueId { set; get; }

        [ForeignKey("SubQueId")]
        [InverseProperty("AttemptRecords")]
        public virtual SubToeic SubToeic { set; get; } = null!;

        [StringLength(1)]
        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; } = false;

        public long AttemptId { set; get; }

        [ForeignKey("AttemptId")]
        [InverseProperty("AttemptRecords")]
        public virtual UserAttempt UserAttempt { set; get; } = null!;
    }
}
