using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Enrollment")]
public partial class Enrollment
{
    [Key]
    public long EnrollId { get; set; }

    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [StringLength(10)]
    public string ClassId { get; set; } = null!;

    public DateOnly? EnrollDate { get; set; }

    public int? StatusId { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Enrollments")]
    public virtual Class Class { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Enrollments")]
    public virtual EnrollStatus? Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Enrollments")]
    public virtual Student User { get; set; } = null!;
}
