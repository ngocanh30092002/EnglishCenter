using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("SubQuestion7")]
public partial class SubQuestion7
{
    [Key]
    public int SubId { get; set; }

    public int? PreId { get; set; }

    public int? NumNo { get; set; }

    public string? Question { get; set; }

    [StringLength(5)]
    public string? CorrectAnswer { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    [ForeignKey("PreId")]
    [InverseProperty("SubQuestion7s")]
    public virtual ToeicPart7? Pre { get; set; }
}
