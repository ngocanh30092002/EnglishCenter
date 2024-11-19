using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("HW_Submission")]
    public class HwSubmission
    {
        [Key]
        public long SubmissionId { set; get; }

        public long HomeworkId { set; get; }

        public DateTime Date { set; get; } = DateTime.Now;

        public int SubmitStatus { set; get; }

        public string? FeedBack { set; get; }

        public bool IsPass { set; get; }

        public long EnrollId { set; get; }

        [ForeignKey("EnrollId")]
        [InverseProperty("Submissions")]
        public virtual Enrollment Enrollment { set; get; } = null!;

        [ForeignKey("HomeworkId")]
        [InverseProperty("Submissions")]
        public virtual Homework Homework { set; get; } = null!;

        [InverseProperty("HwSubmission")]
        public virtual ICollection<HwSubRecord> SubRecords { set; get; } = new List<HwSubRecord>(); 
    }
}
