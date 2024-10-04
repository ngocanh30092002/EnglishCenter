using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Assign_Ques")]
[Index("QuesId", Name = "IX_Assign_Ques", IsUnique = true)]
public partial class AssignQue
{
    [Key]
    public long AssignQuesId { get; set; }

    [Required]
    public int Type { get; set; }

    [Column("Ques_Id")]
    public long? QuesId { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcAudio? QuesAudio { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcImage? QuesImage { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcDouble? QuesDouble { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcSingle? QuesSingle { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesRcTriple? QuesTriple { get; set; }

    [ForeignKey("QuesId")]
    [InverseProperty("AssignQue")]
    public virtual QuesLcConversation? QuesConversation { get; set; }

    public long? AssignmentId { set; get; }

    [ForeignKey("AssignmentId")]
    [InverseProperty("AssignQues")]
    public virtual Assignment? Assignment { set; get; }
}
