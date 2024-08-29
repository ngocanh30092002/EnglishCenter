using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

[PrimaryKey("AttendDate", "StuClassInId")]
[Table("Attendance")]
public partial class Attendance
{
    [Key]
    public DateOnly AttendDate { get; set; }

    [Key]
    public long StuClassInId { get; set; }

    public int? LessionNum { get; set; }

    public bool? IsAttended { get; set; }

    public bool? IsPermited { get; set; }

    public bool? IsLated { get; set; }

    public bool? IsLeaved { get; set; }

    public int? StatusAssignment { get; set; }

    [StringLength(15)]
    public string? AssignmentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Deadline { get; set; }

    [InverseProperty("Attendance")]
    public virtual ICollection<AnswerSheet> AnswerSheets { get; set; } = new List<AnswerSheet>();

    [ForeignKey("AssignmentId")]
    [InverseProperty("Attendances")]
    public virtual Assignment? Assignment { get; set; }

    [ForeignKey("StuClassInId")]
    [InverseProperty("Attendances")]
    public virtual StuInClass StuClassIn { get; set; } = null!;
}
