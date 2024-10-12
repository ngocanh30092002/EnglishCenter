using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_RC_Triple")]
public partial class QuesRcTriple
{
    [Key]
    public long QuesId { get; set; }

    public int Quantity { get; set; }

    [Column("Image_1")]
    [StringLength(300)]
    public string Image1 { get; set; } = null!;

    [Column("Image_2")]
    [StringLength(300)]
    public string Image2 { get; set; } = null!;

    [Column("Image_3")]
    [StringLength(300)]
    public string Image3 { get; set; } = null!;

    public TimeOnly? Time { set; get; }

    [InverseProperty("QuesTriple")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubRcTriple> SubRcTriples { get; set; } = new List<SubRcTriple>();
}
