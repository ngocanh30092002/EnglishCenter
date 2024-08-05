using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class Class
{
    [Key]
    [StringLength(5)]
    public string ClassId { get; set; } = null!;

    [StringLength(5)]
    public string CourseId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    public int? RegisteredNum { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Classes")]
    public virtual Course Course { get; set; } = null!;
}
