using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_LC_Image")]
public partial class QuesLcImage
{
    [Key]
    public long QuesId { get; set; }

    [StringLength(300)]
    public string Image { get; set; } = null!;

    [StringLength(300)]
    public string Audio { get; set; } = null!;

    public long? AnswerId { set; get; }

    [ForeignKey("AnswerId")]
    [InverseProperty("QuesLcImage")]
    public virtual AnswerLcImage? Answer { set; get; }

    [InverseProperty("QuesImage")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

}
