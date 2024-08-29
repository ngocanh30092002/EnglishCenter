using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Assign_Ques")]
[Index("QuesId", Name = "IX_Assign_Ques", IsUnique = true)]
public partial class AssignQue
{
    [Key]
    public long AssignQuesId { get; set; }

    public int? QuesTypeId { get; set; }

    [StringLength(15)]
    public string? AssignmentId { get; set; }

    [Column("Ques_Id")]
    public long? QuesId { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcAudio? Ques { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcImage? Ques1 { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcDouble? Ques2 { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcSingle? Ques3 { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcTriple? Ques4 { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcConversation? QuesNavigation { get; set; }

    [ForeignKey("QuesTypeId")]
    [InverseProperty("AssignQues")]
    public virtual QuestionType? QuesType { get; set; }
}
