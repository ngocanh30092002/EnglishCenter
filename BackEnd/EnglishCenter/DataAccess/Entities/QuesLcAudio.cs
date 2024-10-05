using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_LC_Audio")]
public partial class QuesLcAudio
{
    [Key]
    public long QuesId { get; set; }

    [StringLength(300)]
    public string Audio { get; set; } = null!;

    public long? AnswerId { set; get; }

    [ForeignKey("AnswerId")]
    [InverseProperty("QuesLcAudio")]
    public virtual AnswerLcAudio? Answer { set; get; }

    [InverseProperty("QuesAudio")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();
}
