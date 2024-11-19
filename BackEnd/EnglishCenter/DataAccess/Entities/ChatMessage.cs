using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCenter.DataAccess.Entities
{
    public class ChatMessage
    {
        [Key]
        public long MessageId { set; get; }

        [StringLength(100)]
        public string SenderId { set; get; } = null!;

        [ForeignKey("SenderId")]
        [InverseProperty("SentMessages")]
        public virtual User Sender { set; get; } = null!;

        [StringLength(100)]
        public string ReceiverId { set; get; } = null!;

        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceivedMessages")]
        public virtual User Receiver { set; get; } = null!;

        public string? Message { set; get; }

        public DateTime SendAt { set; get; }

        public long? FileId { set; get; }

        [ForeignKey("FileId")]
        [InverseProperty("ChatMessage")]
        public virtual ChatFile? ChatFile { set; get; }

        public bool IsRead { set; get; }
        public bool IsDelete { set; get; } = false;
    }
}
