using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Sub_LC_Conversation")]
public partial class SubLcConversation
{
    [Key]
    public long SubId { get; set; }

    public long PreQuesId { get; set; }

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

    [ForeignKey("PreQuesId")]
    [InverseProperty("SubLcConversations")]
    public virtual QuesLcConversation PreQues { get; set; } = null!;
}
