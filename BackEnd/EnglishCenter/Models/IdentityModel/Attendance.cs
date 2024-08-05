using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[PrimaryKey("AttendDate", "StuClassId")]
[Table("Attendance")]
public partial class Attendance
{
    [Key]
    [Column(TypeName = "datetime")]
    public DateTime AttendDate { get; set; }

    [Key]
    public int StuClassId { get; set; }

    public bool? IsAttended { get; set; }

    public bool? IsPermited { get; set; }

    public bool? IsLated { get; set; }

    public bool? IsLeaved { get; set; }

    [StringLength(5)]
    public string? StatusAssignment { get; set; }

    [StringLength(100)]
    public string? LinkAssignment { get; set; }

    [ForeignKey("StuClassId")]
    [InverseProperty("Attendances")]
    public virtual StudentInClass StuClass { get; set; } = null!;
}
