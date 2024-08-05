using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[PrimaryKey("SubId", "PreId")]
[Table("SubQuestion3")]
public partial class SubQuestion3
{
    [Key]
    public int SubId { get; set; }

    [Key]
    public int PreId { get; set; }

    public int? NumNo { get; set; }

    public string? Question { get; set; }

    [StringLength(5)]
    public string? CorrectAnswer { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    [ForeignKey("PreId")]
    [InverseProperty("SubQuestion3s")]
    public virtual ToeicPart3 Pre { get; set; } = null!;
}
