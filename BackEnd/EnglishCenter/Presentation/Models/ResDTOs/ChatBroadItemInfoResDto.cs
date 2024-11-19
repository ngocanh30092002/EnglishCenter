namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ChatBroadItemInfoResDto
    {
        public string UserId { set; get; }
        public string UserName { set; get; }
        public string Image { set; get; }
        public string LastMessage { set; get; }
        public string LastTime { set; get; }
        public bool IsRead { set; get; }
        public bool IsDelete { set; get; }
    }
}
