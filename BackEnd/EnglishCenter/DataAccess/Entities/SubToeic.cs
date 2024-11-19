using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Sub_Toeic")]
    public class SubToeic
    {
        [Key]
        public long SubId { set; get; }

        public long QuesId { set; get; }

        [ForeignKey("QuesId")]
        [InverseProperty("SubToeicList")]
        public virtual QuesToeic QuesToeic { set; get; } = null!;
        public int QuesNo { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public long? AnswerId { set; get; }

        [ForeignKey("AnswerId")]
        [InverseProperty("SubToeic")]
        public virtual AnswerToeic? Answer { set; get; }

        [InverseProperty("SubToeic")]
        public virtual ICollection<ToeicRecord> ToeicRecords { set; get; } = new List<ToeicRecord>();

        [InverseProperty("SubToeic")]
        public virtual ICollection<ToeicPracticeRecord> ToeicPracticeRecords { set; get; } = new List<ToeicPracticeRecord>();
    }
}
