using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_RC_Single")]
public class QuesRcSingle
{
    [Key]
    public long QuesId { get; set; }

    public int Quantity { set; get; }

    [Column("Image")]
    [StringLength(300)]
    public string Image { get; set; } = null!;

    public TimeOnly Time { set; get; } = TimeOnly.MinValue;

    [InverseProperty("QuesSingle")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();
    
    [InverseProperty("QuesSingle")]
    public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubRcSingle> SubRcSingles { get; set; } = new List<SubRcSingle>();
}
