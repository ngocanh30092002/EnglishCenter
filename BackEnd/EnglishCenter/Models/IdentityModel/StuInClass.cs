using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("StuInClass")]
[Index("ScoreHisId", Name = "IX_StuInClass", IsUnique = true)]
public partial class StuInClass
{
    [Key]
    public long StuInClassId { get; set; }

    [StringLength(100)]
    public string? UserId { get; set; }

    [StringLength(10)]
    public string? ClassId { get; set; }

    public long? ScoreHisId { get; set; }

    [InverseProperty("StuClassIn")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("ClassId")]
    [InverseProperty("StuInClasses")]
    public virtual Class? Class { get; set; }

    [ForeignKey("ScoreHisId")]
    [InverseProperty("StuInClass")]
    public virtual ScoreHistory? ScoreHis { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("StuInClasses")]
    public virtual Student? User { get; set; }
}
