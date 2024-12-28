namespace EnglishCenter.Presentation.Models.DTOs
{
    public class FbUserInfo
    {
        public string? Id { set; get; }
        public string? AccessToken { set; get; }
        public string? Birthday { set; get; }
        public string? Email { set; get; }
        public string? First_Name { set; get; }
        public string? Last_Name { set; get; }
        public string? Gender { set; get; }
        public Dictionary<string, object>? Location { get; set; }
        public Dictionary<string, object>? Picture { get; set; }
    }
}
