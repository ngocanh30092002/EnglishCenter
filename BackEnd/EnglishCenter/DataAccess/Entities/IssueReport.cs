using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class IssueReport
    {
        [Key]
        public long IssueId { set; get; }
        public string Title { set; get; } = null!;
        public string? Description { set; get; }
        public int Type { set; get; }
        public int Status { set; get; }
        public DateTime CreatedAt { set; get; }

        [Required]
        [StringLength(100)]
        public string UserId { set; get; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("IssueReports")]
        public virtual User User { set; get; } = null!;

        [InverseProperty("IssueReport")]
        public virtual ICollection<IssueResponse> IssueResponses { set; get; } = new List<IssueResponse>();
    }
}
