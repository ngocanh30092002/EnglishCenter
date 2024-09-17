using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.Models.IdentityModel
{
    public class Homework
    {
        [Key]
        public long HomeworkId { set;get; }
        public long? AttendanceId { set; get; }
        public long? AssignmentId { set; get; }
        public DateTime? Deadline { set; get; }
        public long? AnswerSheetId { set; get; }

        [ForeignKey("AttendanceId")]
        [InverseProperty("HomeworkList")]
        public virtual Attendance? Attendance { set; get; }

        [ForeignKey("AssignmentId")]
        [InverseProperty("HomeworkList")]
        public virtual Assignment? Assignment { set; get; }

    }
}
