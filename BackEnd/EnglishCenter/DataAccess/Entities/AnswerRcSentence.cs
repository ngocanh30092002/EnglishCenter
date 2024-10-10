using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Answer_RC_Sentence")]
    public class AnswerRcSentence
    {
        [Key]
        public long AnswerId { set; get; }
        public string Question { set; get; } = null!;
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;
        public string? Explanation { set; get; }

        [StringLength(1)]
        public string CorrectAnswer { set; get; } = null!;

        [InverseProperty("Answer")]
        public virtual QuesRcSentence QuesRcSentence { set; get; } = null!;
    }
}
