using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class AnswerRecord
    {
        [Key]
        public long AnswerRecordId { set; get; }

        public long LearningProcessId { set; get; }

        [ForeignKey("LearningProcessId")]
        [InverseProperty("AnswerRecords")]
        public virtual LearningProcess? LearningProcess { set; get; }

        public long AssignQuesId { set;get; }

        [ForeignKey("AssignQuesId")]
        [InverseProperty("AnswerRecords")]
        public virtual AssignQue? AssignQue { set; get; }

        public long? SubQueId { set; get; }

        [StringLength(1)]
        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; } = false;
    }
}
