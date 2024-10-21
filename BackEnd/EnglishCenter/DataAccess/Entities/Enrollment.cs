using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Enrollment")]
[Index("ScoreHisId", Name = "IX_Enrollment", IsUnique = true)]
public class Enrollment
{
    [Key]
    public long EnrollId { get; set; }

    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [StringLength(10)]
    public string ClassId { get; set; } = null!;

    public DateOnly? EnrollDate { get; set; }

    public int? StatusId { get; set; }

    public long? ScoreHisId { get; set; }

    public DateTime? UpdateTime { set; get; }

    [ForeignKey("ClassId")]
    [InverseProperty("Enrollments")]
    public virtual Class Class { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Enrollments")]
    public virtual EnrollStatus? Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Enrollments")]
    public virtual Student User { get; set; } = null!;

    [ForeignKey("ScoreHisId")]
    [InverseProperty("Enrollment")]
    public virtual ScoreHistory? ScoreHis { get; set; }

    [InverseProperty("Enrollment")]
    public virtual ICollection<LearningProcess> LearningProcesses { set; get; } = new List<LearningProcess>();

    [InverseProperty("Enrollment")]
    public virtual ICollection<HwSubmission> Submissions { set; get; } = new List<HwSubmission>();
}
