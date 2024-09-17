using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishCenter.Models.IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class Attendance
{
    [Key]
    public long AttendanceId { set; get; }

    [Required]
    public DateOnly Date { set; get; }
    public bool? IsAttended { set; get; }
    public bool? IsPermitted { set;get; }
    public bool? IsLate { set; get; }
    public bool? IsLeaved { set; get; }

    public long StuInClassId { get; set; }

    [ForeignKey("StuInClassId")]
    [InverseProperty("Attendances")]
    public virtual StuInClass StuInClass { set; get; }

    [InverseProperty("Attendance")]
    public virtual ICollection<Homework> HomeworkList { set; get; } = new List<Homework>();
}
