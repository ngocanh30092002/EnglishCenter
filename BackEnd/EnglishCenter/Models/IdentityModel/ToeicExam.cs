using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class ToeicExam
{
    [Key]
    [StringLength(10)]
    public string ToeicId { get; set; } = null!;

    public int? SerialNum { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public int Year { get; set; }

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart1> ToeicPart1s { get; set; } = new List<ToeicPart1>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart2> ToeicPart2s { get; set; } = new List<ToeicPart2>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart3> ToeicPart3s { get; set; } = new List<ToeicPart3>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart4> ToeicPart4s { get; set; } = new List<ToeicPart4>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart5> ToeicPart5s { get; set; } = new List<ToeicPart5>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart6> ToeicPart6s { get; set; } = new List<ToeicPart6>();

    [InverseProperty("Toeic")]
    public virtual ICollection<ToeicPart7> ToeicPart7s { get; set; } = new List<ToeicPart7>();
}
