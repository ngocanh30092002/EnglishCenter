using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Entities;

[Table("Ques_LC_Image")]
public partial class QuesLcImage
{
    [Key]
    public long QuesId { get; set; }

    [StringLength(300)]
    public string Image { get; set; } = null!;

    [StringLength(300)]
    public string Audio { get; set; } = null!;

    [StringLength(5)]
    public string CorrectAnswer { get; set; } = null!;

    [InverseProperty("QuesImage")]
    public virtual AssignQue? AssignQue { get; set; }
}
