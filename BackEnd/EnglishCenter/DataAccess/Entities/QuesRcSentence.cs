using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Ques_RC_Sentence")]
    public class QuesRcSentence
    {
        [Key]
        public long QuesId { set; get; }

        public string Question { set; get; } = null!;

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
        [InverseProperty("QuesRcSentence")]
        public virtual AnswerRcSentence? Answer { set; get; }

        [InverseProperty("QuesSentence")]
        public virtual ICollection<AssignQue> AssignQues { set; get; } = new List<AssignQue>();
        
        [InverseProperty("QuesSentence")]
        public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();
    }
}
