using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[Table("Toeic_Part_7")]
public partial class ToeicPart7
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string? ToeicId { get; set; }

    public int? NumNo { get; set; }

    public int? Quantity { get; set; }

    [Column("ImageLink_1")]
    [StringLength(100)]
    public string? ImageLink1 { get; set; }

    [Column("ImageLink_2")]
    [StringLength(100)]
    public string? ImageLink2 { get; set; }

    [Column("ImageLink_3")]
    [StringLength(100)]
    public string? ImageLink3 { get; set; }

    [InverseProperty("Pre")]
    public virtual ICollection<SubQuestion7> SubQuestion7s { get; set; } = new List<SubQuestion7>();

    [ForeignKey("ToeicId")]
    [InverseProperty("ToeicPart7s")]
    public virtual ToeicExam? Toeic { get; set; }
}
