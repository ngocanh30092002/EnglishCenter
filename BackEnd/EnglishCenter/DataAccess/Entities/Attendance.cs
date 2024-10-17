using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities;

public class Attendance
{
    [Key]
    public long AttendanceId { set; get; }

    [Required]
    public DateOnly Date { set; get; }
    public bool? IsAttended { set; get; }
    public bool? IsPermitted { set; get; }
    public bool? IsLate { set; get; }
    public bool? IsLeaved { set; get; }
}
