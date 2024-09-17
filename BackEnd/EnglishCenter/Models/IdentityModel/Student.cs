using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishCenter.Models.IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Models;

public partial class Student
{
    [Key]
    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public int? Gender { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(200)]
    public string? Image { get; set; }

    [StringLength(200)]
    public string? BackgroundImage { set; get; }

    [StringLength(200)]
    public string? UserName { set; get; }

    [StringLength(300)]
    public string? Description { set; get; }

    public DateOnly? DateOfBirth { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("User")]
    public virtual ICollection<StuInClass> StuInClasses { get; set; } = new List<StuInClass>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [InverseProperty("Student")]
    public virtual ICollection<NotiStudent> NotiStudents { set; get; } = new List<NotiStudent>();

    [InverseProperty("Student")]
    public virtual ICollection<ScheduleEvent> ScheduleEvents { get; set; } = new List<ScheduleEvent>();
}
