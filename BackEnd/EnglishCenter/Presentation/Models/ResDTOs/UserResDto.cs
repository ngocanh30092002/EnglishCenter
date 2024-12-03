namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class UserResDto
    {
        public string? UserImage { set; get; }
        public string? UserId { set; get; }
        public string? UserName { set; get; }
        public string? UserEmail { set; get; }
        public string? PhoneNumber { set; get; }
        public int EmailConfirm { set; get; }
        public int Lock { set; get; }
        public string? DateOfBirth { set; get; }
        public string? Address { set; get; }
        public List<string> Roles { set; get; }

    }
}
