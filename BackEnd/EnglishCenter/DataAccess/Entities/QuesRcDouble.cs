﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_RC_Double")]
public class QuesRcDouble
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

    public TimeOnly Time { set; get; } = TimeOnly.MinValue;

    public int Level { set; get; } = 1;

    [InverseProperty("QuesDouble")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("QuesDouble")]
    public virtual ICollection<HomeQue> HomeQues { set; get; } = new List<HomeQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubRcDouble> SubRcDoubles { get; set; } = new List<SubRcDouble>();
}
