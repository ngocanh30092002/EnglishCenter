using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class IssueResponse
    {
        [Key]
        public long IssueResId { set; get; }

        [StringLength(100)]
        public string UserId { set; get; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("IssueResponses")]
        public User User { set; get; } = null!;

        public long IssueId { set; get; }
        [ForeignKey("IssueId")]
        [InverseProperty("IssueResponses")]
        public IssueReport IssueReport { set; get; } = null!;

        public string Message { set; get; } = null!;
        public DateTime CreatedAt { set; get; }
    }
}
