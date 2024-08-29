using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class Class
{
    [Key]
    [StringLength(10)]
    public string ClassId { get; set; } = null!;

    [StringLength(10)]
    public string CourseId { get; set; } = null!;

    [StringLength(100)]
    public string? TeacherId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? RegisteredNum { get; set; }

    public int? MaxNum { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Classes")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Class")]
    public virtual ICollection<StuInClass> StuInClasses { get; set; } = new List<StuInClass>();
}
