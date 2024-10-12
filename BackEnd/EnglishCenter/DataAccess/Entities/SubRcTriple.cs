using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Sub_RC_Triple")]
public partial class SubRcTriple
{
    [Key]
    public long SubId { get; set; }

    public long PreQuesId { get; set; }

    public string Question { get; set; } = null!;

    [StringLength(300)]
    public string AnswerA { get; set; } = null!;

    [StringLength(300)]
    public string AnswerB { get; set; } = null!;

    [StringLength(300)]
    public string AnswerC { get; set; } = null!;

    [StringLength(300)]
    public string AnswerD { get; set; } = null!;

    public int NoNum { set; get; } = 1;

    public long? AnswerId { set; get; }

    [ForeignKey("AnswerId")]
    [InverseProperty("SubRcTriple")]
    public virtual AnswerRcTriple? Answer { set; get; }

    [ForeignKey("PreQuesId")]
    [InverseProperty("SubRcTriples")]
    public virtual QuesRcTriple PreQues { get; set; } = null!;
}
