using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class PreExamScore
{
    [Key]
    public int PreScoreId { get; set; }

    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [StringLength(5)]
    public string CourseId { get; set; } = null!;

    public int? EntrancePoint { get; set; }

    public int? MidtermPoint { get; set; }

    public int? FinalPoint { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("PreExamScores")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("PreScore")]
    public virtual StudentInClass? StudentInClass { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PreExamScores")]
    public virtual Student User { get; set; } = null!;
}
