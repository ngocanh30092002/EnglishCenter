using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[PrimaryKey("UserId", "CourseId")]
[Table("Enrollment")]
public partial class Enrollment
{
    [Key]
    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [Key]
    [StringLength(5)]
    public string CourseId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? EnrollDate { get; set; }

    public bool? IsCompleted { get; set; }

    public bool? IsPaid { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Enrollments")]
    public virtual Student User { get; set; } = null!;
}
