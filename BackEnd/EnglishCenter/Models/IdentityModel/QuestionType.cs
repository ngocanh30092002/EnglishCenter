using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Question_Type")]
public partial class QuestionType
{
    [Key]
    public int QuesTypeId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(50)]
    public string? TableName { get; set; }

    [InverseProperty("QuesType")]
    public virtual ICollection<AssignQue> AssignQues { get; set; } = new List<AssignQue>();
}
