using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

public class Attendance
{
    [Key]
    public long AttendanceId { set; get; }

    [ForeignKey("LessonId")]
    [InverseProperty("Attendances")]
    public long LessonId { set; get; }
    public virtual Lesson Lesson { set; get; } = null!;

    public long EnrollId { set; get; }
    [ForeignKey("EnrollId")]
    [InverseProperty("Attendances")]
    public Enrollment Enrollment { set; get; } = null!;
    public bool? IsAttended { set; get; }
    public bool? IsPermitted { set; get; }
    public bool? IsLate { set; get; }
    public bool? IsLeaved { set; get; }
}
