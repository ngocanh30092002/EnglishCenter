using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Ques_RC_Sentence_Media")]
    public class QuesRcSentenceMedia
    {
        [Key]
        public long QuesId { set; get; }

        public string Question { set; get; } = null!;

        public string Image { set; get; } = null!;

        public string? Audio { set; get; }

        [StringLength(300)]
        public string AnswerA { get; set; } = null!;

        [StringLength(300)]
        public string AnswerB { get; set; } = null!;

        [StringLength(300)]
        public string AnswerC { get; set; } = null!;

        [StringLength(300)]
        public string AnswerD { get; set; } = null!;

        public long? AnswerId { set; get; }

        public TimeOnly Time { set; get; } = TimeOnly.MinValue;

        [ForeignKey("AnswerId")]
        [InverseProperty("QuesRcSentenceMedia")]
        public virtual AnswerRcSentenceMedia? Answer { set; get; }

        [InverseProperty("QuesSentenceMedia")]
        public virtual ICollection<AssignQue> AssignQues { set; get; } = new List<AssignQue>();

        [InverseProperty("QuesSentenceMedia")]
        public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();
    }
}
