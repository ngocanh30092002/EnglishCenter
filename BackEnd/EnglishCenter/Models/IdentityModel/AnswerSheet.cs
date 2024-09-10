using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models;

[Table("AnswerSheet")]
public partial class AnswerSheet
{
    [Key]
    public long AnswerSheetId { get; set; }

    public DateOnly? AttendDate { get; set; }

    public long? StuInClassId { get; set; }

    public TimeOnly? Time { get; set; }

    public int? FalseNum { get; set; }

    public int? CorrectNum { get; set; }

    [StringLength(500)]
    public string? AnswerString { get; set; }

    [ForeignKey("AttendDate, StuInClassId")]
    [InverseProperty("AnswerSheets")]
    public virtual Attendance? Attendance { get; set; }
}
