namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ChatMessageResDto
    {
        public long MessageId { set; get; }
        public string SenderId { set; get; }
        public string ReceiverId { set; get; }
        public string? Message { set; get; }
        public string? SendAt { set; get; }
        public ChatFileResDto? File { set; get; }
        public bool IsRead { set; get; }
        public bool IsDelete { set; get; }
        public bool IsOwnSender { set; get; }
    }
}
