using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class Course
{
    [Key]
    [StringLength(5)]
    public string CourseId { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Fee { get; set; }

    public int EntryPoint { get; set; }

    public int StandardPoint { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    [InverseProperty("Course")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Course")]
    public virtual ICollection<PreExamScore> PreExamScores { get; set; } = new List<PreExamScore>();
}
