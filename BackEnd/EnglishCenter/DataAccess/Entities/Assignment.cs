﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

[Table("Assignment")]
public class Assignment
{
    [Key]
    public long AssignmentId { get; set; }

    [Required]
    public int NoNum { set; get; }

    [Column(TypeName = "nvarchar(200)")]
    public string? Title { set; get; }

    public TimeOnly Time { set; get; } = TimeOnly.MinValue;

    public TimeOnly ExpectedTime { set; get; } = TimeOnly.MinValue;

    public long CourseContentId { set; get; }

    [Range(0, 100)]
    [Required]
    public int AchievedPercentage { set; get; } = 0;

    public bool CanViewResult { set; get; } = true;

    [ForeignKey("CourseContentId")]
    [InverseProperty("Assignments")]
    public virtual CourseContent CourseContent { set; get; } = null!;

    [InverseProperty("Assignment")]
    public virtual ICollection<AssignQue> AssignQues { set; get; } = new List<AssignQue>();

    [InverseProperty("Assignment")]
    public virtual ICollection<LearningProcess> LearningProcesses { set; get; } = new List<LearningProcess>();
}
