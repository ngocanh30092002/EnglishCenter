
using System.ComponentModel.DataAnnotations;
using EnglishCenter.Presentation.Attribute;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class UserInfoDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [MinAge(18)]
        public DateOnly? DateOfBirth { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        public string? Address { get; set; }

        public string? UserName { set; get; }
    }
}
