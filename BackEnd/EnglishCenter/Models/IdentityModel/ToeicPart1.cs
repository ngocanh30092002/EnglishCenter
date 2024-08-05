using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_1")]
public partial class ToeicPart1
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string ToeicId { get; set; } = null!;

    public int? NumNo { get; set; }

    [StringLength(100)]
    public string? ImageLink { get; set; }

    [StringLength(5)]
    public string? CorrectAnswer { get; set; }

    public string? Audio { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart1s")]
    public virtual ToeicExam Toeic { get; set; } = null!;
}
