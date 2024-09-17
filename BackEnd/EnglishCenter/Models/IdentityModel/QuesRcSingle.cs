using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Ques_RC_Single")]
public partial class QuesRcSingle
{
    [Key]
    public long QuesId { get; set; }

    public string Question { get; set; } = null!;

    [StringLength(5)]
    public string CorrectAnswer { get; set; } = null!;

    [StringLength(300)]
    public string AnswerA { get; set; } = null!;

    [StringLength(300)]
    public string AnswerB { get; set; } = null!;

    [StringLength(300)]
    public string AnswerC { get; set; } = null!;

    [StringLength(300)]
    public string AnswerD { get; set; } = null!;

    [InverseProperty("Ques3")]
    public virtual AssignQue? AssignQue { get; set; }
}
