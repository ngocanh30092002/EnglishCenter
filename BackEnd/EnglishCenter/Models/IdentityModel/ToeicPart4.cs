using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_4")]
public partial class ToeicPart4
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
    public virtual ICollection<SubQuestion4> SubQuestion4s { get; set; } = new List<SubQuestion4>();

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart4s")]
    public virtual ToeicExam Toeic { get; set; } = null!;
}
