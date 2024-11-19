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
        public virtual Enrollment Enrollment { set; get; } = null!;

        public int Status { set; get; } = 1;

        public DateTime StartTime { set; get; } = DateTime.Now;

        public DateTime? EndTime { set; get; }

        public long? AssignmentId { set; get; }

        [ForeignKey("AssignmentId")]
        [InverseProperty("LearningProcesses")]
        public virtual Assignment? Assignment { set; get; }

        public long? ExamId { set; get; }

        [ForeignKey("ExamId")]
        [InverseProperty("LearningProcesses")]
        public virtual Examination? Examination { set; get; }

        [InverseProperty("LearningProcess")]
        public virtual ICollection<AssignmentRecord> AssignmentRecords { set; get; } = new List<AssignmentRecord>();
        
        [InverseProperty("LearningProcess")]
        public virtual ICollection<ToeicRecord> ToeicRecords { set; get; } = new List<ToeicRecord>();

    }
}
