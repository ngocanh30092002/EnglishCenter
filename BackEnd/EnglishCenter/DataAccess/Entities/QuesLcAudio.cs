using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_LC_Audio")]
public class QuesLcAudio
{
    [Key]
    public long QuesId { get; set; }

    [StringLength(300)]
    public string Audio { get; set; } = null!;

    public string Question { set; get; } = null!;

    [StringLength(300)]
    public string AnswerA { get; set; } = null!;

    [StringLength(300)]
    public string AnswerB { get; set; } = null!;

    [StringLength(300)]
    public string AnswerC { get; set; } = null!;

    public long? AnswerId { set; get; }

    public TimeOnly Time { set; get; } = TimeOnly.MinValue;

    [ForeignKey("AnswerId")]
    [InverseProperty("QuesLcAudio")]
    public virtual AnswerLcAudio? Answer { set; get; }

    [InverseProperty("QuesAudio")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("QuesAudio")]
    public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();

}
