using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("StudentInClass")]
[Index("PreScoreId", Name = "IX_StudentInClass", IsUnique = true)]
public partial class StudentInClass
{
    [Key]
    public int StuClassId { get; set; }

    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [StringLength(5)]
    public string ClassId { get; set; } = null!;

    public int PreScoreId { get; set; }

    [InverseProperty("StuClass")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("PreScoreId")]
    [InverseProperty("StudentInClass")]
    public virtual PreExamScore PreScore { get; set; } = null!;
}
