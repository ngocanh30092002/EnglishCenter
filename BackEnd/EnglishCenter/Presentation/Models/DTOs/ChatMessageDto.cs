using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ChatMessageDto
    {
        public string? SenderId { set; get; }

        [StringLength(100)]
        public string ReceiverId { set; get; } = null!;

        public string? Message { set; get; }

        public IFormFile? file { set; get; }

        public long? FileId { set; get; }
    }
}
