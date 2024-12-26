using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.DataAccess.Entities;

public class User : IdentityUser
{
    [InverseProperty("User")]
    public virtual Student? Student { get; set; }

    [InverseProperty("User")]
    public virtual Teacher? Teacher { set; get; }

    [InverseProperty("User")]
    public virtual ICollection<UserAttempt> UserAttempts { set; get; } = new List<UserAttempt>();

    [InverseProperty("Sender")]
    public virtual ICollection<ChatMessage> SentMessages { set; get; } = new List<ChatMessage>();

    [InverseProperty("Receiver")]
    public virtual ICollection<ChatMessage> ReceivedMessages { set; get; } = new List<ChatMessage>();

    [InverseProperty("User")]
    public virtual ICollection<UserWord> UserWords { set; get; } = new List<UserWord>();

    [InverseProperty("User")]
    public virtual ICollection<IssueReport> IssueReports { set; get; } = new List<IssueReport>();

    [InverseProperty("User")]
    public virtual ICollection<IssueResponse> IssueResponses { set; get; } = new List<IssueResponse>();
}
