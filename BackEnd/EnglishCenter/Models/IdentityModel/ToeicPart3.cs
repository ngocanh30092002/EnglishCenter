using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_3")]
public partial class ToeicPart3
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string ToeicId { get; set; } = null!;

    public int? NumNo { get; set; }

    public int? Quantity { get; set; }

    [StringLength(100)]
    public string? ImageLink { get; set; }

    public string? Audio { get; set; }

    [InverseProperty("Pre")]
    public virtual ICollection<SubQuestion3> SubQuestion3s { get; set; } = new List<SubQuestion3>();

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart3s")]
    public virtual ToeicExam Toeic { get; set; } = null!;
}
