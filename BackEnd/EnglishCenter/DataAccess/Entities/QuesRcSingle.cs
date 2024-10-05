using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_RC_Single")]
public partial class QuesRcSingle
{
    [Key]
    public long QuesId { get; set; }

    public int Quantity { set; get; }

    [Column("Image")]
    [StringLength(300)]
    public string? Image { get; set; }

    [InverseProperty("QuesSingle")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubRcSingle> SubRcSingles { get; set; } = new List<SubRcSingle>();
}
