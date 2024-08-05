using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_2")]
public partial class ToeicPart2
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string ToeicId { get; set; } = null!;

    public int? NumNo { get; set; }

    [StringLength(5)]
    public string? CorrectAnswer { get; set; }

    public string? Audio { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart2s")]
    public virtual ToeicExam Toeic { get; set; } = null!;
}
