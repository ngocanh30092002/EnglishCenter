using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_RC_Double")]
public partial class QuesRcDouble
{
    [Key]
    public long QuesId { get; set; }

    public int Quantity { get; set; }

    [Column("Image_1")]
    [StringLength(300)]
    public string? Image1 { get; set; }

    [Column("Image_2")]
    [StringLength(300)]
    public string? Image2 { get; set; }

    [InverseProperty("QuesDouble")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubRcDouble> SubRcDoubles { get; set; } = new List<SubRcDouble>();
}
