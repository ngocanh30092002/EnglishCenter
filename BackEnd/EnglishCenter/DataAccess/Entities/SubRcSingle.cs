using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("Sub_RC_Single")]
    public class SubRcSingle
    {
        [Key]
        public long SubId { get; set; }

        public long PreQuesId { get; set; }

        public string Question { get; set; } = null!;

        [StringLength(300)]
        public string AnswerA { get; set; } = null!;

        [StringLength(300)]
        public string AnswerB { get; set; } = null!;

        [StringLength(300)]
        public string AnswerC { get; set; } = null!;

        [StringLength(300)]
        public string AnswerD { get; set; } = null!;

        public long? AnswerId { set; get; }

        [ForeignKey("AnswerId")]
        [InverseProperty("SubRcSingle")]
        public virtual AnswerRcSingle? Answer { set; get; }

        [ForeignKey("PreQuesId")]
        [InverseProperty("SubRcSingles")]
        public virtual QuesRcSingle PreQues { get; set; } = null!;

    }
}
