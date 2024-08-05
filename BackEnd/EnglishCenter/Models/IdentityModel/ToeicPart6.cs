using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_6")]
public partial class ToeicPart6
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string? ToeicId { get; set; }

    public int? NumNo { get; set; }

    public int? Quantity { get; set; }

    [StringLength(100)]
    public string? ImageLink { get; set; }

    [InverseProperty("Pre")]
    public virtual ICollection<SubQuestion6> SubQuestion6s { get; set; } = new List<SubQuestion6>();

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart6s")]
    public virtual ToeicExam? Toeic { get; set; }
}
