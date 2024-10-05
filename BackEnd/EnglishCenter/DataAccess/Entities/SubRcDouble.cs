using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Sub_RC_Double")]
public partial class SubRcDouble
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
    [InverseProperty("SubRcDouble")]
    public virtual AnswerRcDouble? Answer { set; get; }

    [ForeignKey("PreQuesId")]
    [InverseProperty("SubRcDoubles")]
    public virtual QuesRcDouble PreQues { get; set; } = null!;
}
