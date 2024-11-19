using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Toeic_Records")]
    public class ToeicRecord
    {
        [Key]
        public long RecordId { set; get; }

        public long LearningProcessId { set; get; }

        [ForeignKey("LearningProcessId")]
        [InverseProperty("ToeicRecords")]
        public virtual LearningProcess? LearningProcess { set; get; }

        public long SubQueId { set; get; }

        [ForeignKey("SubQueId")]
        [InverseProperty("ToeicRecords")]
        public virtual SubToeic SubToeic { set; get; } = null!;

        [StringLength(1)]
        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; } = false;
    }
}
