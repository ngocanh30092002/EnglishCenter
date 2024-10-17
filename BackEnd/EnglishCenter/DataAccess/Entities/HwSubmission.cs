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

        public int Status { set; get; }

        public string? FeedBack { set; get; }

        [ForeignKey("HomeworkId")]
        [InverseProperty("Submissions")]
        public virtual Homework Homework { set; get; } = null!;

        [InverseProperty("HwSubmission")]
        public virtual ICollection<HwSubRecord> SubRecords { set; get; } = new List<HwSubRecord>(); 
    }
}
