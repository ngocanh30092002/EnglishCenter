namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class StudentResDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { set; get; }
        public string? Address { get; set; }
        public string? UserName { set; get; }
        public string? Description { set; get; }
        public List<string>? Roles { set; get; }
        public string? Image { set; get; }
        public string? BackgroundImage { set; get; }
    }
}
