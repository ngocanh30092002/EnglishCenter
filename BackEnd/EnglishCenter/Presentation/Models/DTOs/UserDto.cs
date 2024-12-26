using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class UserDto
    {
        public string UserId { set; get; } = null!;
        public string UserName { set; get; } = null!;

        [EmailAddress]
        public string UserEmail { set; get; } = null!;

        public string? DateOfBirth { set; get; }

        public string? PhoneNumber { set; get; }
        public string? Address { set; get; }
        public int EmailConfirm { set; get; }
        public int Locked { set; get; }
    }
}
