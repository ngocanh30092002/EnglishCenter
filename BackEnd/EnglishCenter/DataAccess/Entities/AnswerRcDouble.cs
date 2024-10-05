using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Answer_RC_Double")]
    public class AnswerRcDouble
    {
        [Key]
        public long AnswerId { set; get; }
        public string Question { set; get; } = null!;
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;
        public string AnswerD { set; get; } = null!;

        [StringLength(1)]
        public string CorrectAnswer { set; get; } = null!;

        [InverseProperty("Answer")]
        public virtual SubRcDouble SubRcDouble { set; get; } = null!;
    }
}
