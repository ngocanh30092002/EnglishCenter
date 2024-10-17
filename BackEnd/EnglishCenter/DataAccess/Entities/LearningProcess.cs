using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class LearningProcess
    {
        [Key]
        public long ProcessId { set; get; }

        [Required]
        public long EnrollId { set; get; }

        [ForeignKey("EnrollId")]
        [InverseProperty("LearningProcesses")]
        public virtual Enrollment? Enrollment { set; get; }

        public int Status { set; get; } = 1;

        public DateTime StartTime { set; get; } = DateTime.Now;

        public DateTime? EndTime { set; get; }

        public long AssignmentId { set; get; }

        [ForeignKey("AssignmentId")]
        [InverseProperty("LearningProcesses")]
        public virtual Assignment Assignment { set; get; } = null!;

        [InverseProperty("LearningProcess")]
        public virtual ICollection<AnswerRecord> AnswerRecords { set; get; } = new List<AnswerRecord>();
    }
}
