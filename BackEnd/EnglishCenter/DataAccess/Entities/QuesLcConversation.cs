using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_LC_Conversation")]
public partial class QuesLcConversation
{
    [Key]
    public long QuesId { get; set; }

    public int Quantity { get; set; }

    [StringLength(300)]
    public string Image { get; set; } = null!;

    [StringLength(300)]
    public string Audio { get; set; } = null!;

    [InverseProperty("QuesConversation")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();

    [InverseProperty("PreQues")]
    public virtual ICollection<SubLcConversation> SubLcConversations { get; set; } = new List<SubLcConversation>();
}
