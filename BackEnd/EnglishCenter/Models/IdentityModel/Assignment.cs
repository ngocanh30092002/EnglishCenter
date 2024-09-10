using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models;

[Table("Assignment")]
public partial class Assignment
{
    [Key]
    [StringLength(15)]
    public string AssignmentId { get; set; } = null!;

    [StringLength(10)]
    public string? CourseId { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    public TimeOnly? Time { get; set; }

    [InverseProperty("Assignment")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ForeignKey("CourseId")]
    [InverseProperty("Assignments")]
    public virtual Course? Course { get; set; }
}
