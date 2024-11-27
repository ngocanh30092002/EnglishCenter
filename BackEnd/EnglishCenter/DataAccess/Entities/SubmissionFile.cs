using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    [Table("SubmissionFiles")]
    public class SubmissionFile
    {
        [Key]
        public long SubmissionFileId { set; get; }
        public string? FilePath { set; get; }
        public string? LinkUrl { set; get; }
        public int Status { set; get; }
        public DateTime UploadAt { set; get; }
        public string UploadBy { set; get; } = null!;
        public long SubmissionTaskId { set; get; }

        [ForeignKey("SubmissionTaskId")]
        [InverseProperty("SubmissionFiles")]
        public SubmissionTask SubmissionTask { set; get; } = null!;

        public long EnrollId { set; get; }

        [ForeignKey("EnrollId")]
        [InverseProperty("SubmissionFiles")]
        public virtual Enrollment Enrollment { set; get; } = null!;
    }
}
