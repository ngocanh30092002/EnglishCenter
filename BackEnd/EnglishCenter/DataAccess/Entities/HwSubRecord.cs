using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("HW_Sub_Records")]
    public class HwSubRecord
    {
        [Key]
        public long RecordId { set; get; }

        public long SubmissionId { set; get; }

        public long? HwQuesId { set; get; }

        public long? HwSubQuesId { set; get; }

        [StringLength(1)]
        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; } = false;

        [ForeignKey("SubmissionId")]
        [InverseProperty("SubRecords")]
        public virtual HwSubmission HwSubmission { set; get; } = null!;

        [ForeignKey("HwQuesId")]
        [InverseProperty("SubRecords")]
        public virtual HomeQue? HomeQue { set; get; }

        public long? SubToeicId { set; get; }

        [ForeignKey("SubToeicId")]
        [InverseProperty("SubRecords")]
        public virtual SubToeic? SubToeic { set; get; }
    }
}
