using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Answer_LC_Audio")]

    public class AnswerLcAudio
    {
        [Key]
        public long AnswerId { set; get; }
        public string AnswerA { set; get; } = null!;
        public string AnswerB { set; get; } = null!;
        public string AnswerC { set; get; } = null!;

        [StringLength(1)]
        public string CorrectAnswer { set; get; } = null!;

        [InverseProperty("Answer")]
        public virtual QuesLcAudio QuesLcAudio { set; get; } = null!;
    }
}
