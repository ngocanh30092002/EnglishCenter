using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Answer_Toeic")]
    public class AnswerToeic
    {
        [Key]
        public long AnswerId { set; get; }
        public string? Question { set; get; }
        public string? AnswerA { set; get; }
        public string? AnswerB { set; get; }
        public string? AnswerC { set; get; }
        public string? AnswerD { set; get; }
        public string? Explanation { set; get; }

        [StringLength(1)]
        public string CorrectAnswer { set; get; } = null!;

        [InverseProperty("Answer")]
        public virtual SubToeic SubToeic { set; get; } = null!;
    }
}
