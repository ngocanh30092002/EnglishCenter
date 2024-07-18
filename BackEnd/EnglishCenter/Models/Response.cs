namespace EnglishCenter.Models
{
    public class Response
    {
        public bool Success { set; get; } = false;
        public string Token { set; get; } = string.Empty;
        public string Message { set; get; } = string.Empty;
        public string RedirectLink { set; get; }  = string.Empty;
    }
}
