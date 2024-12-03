using System.ComponentModel.DataAnnotations;
using EnglishCenter.Presentation.Attribute;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.Presentation.Models
{
    public class RegisterModel
    {
        [Required]
        [Length(minimumLength: 0, maximumLength: 50)]
        public string FirstName { set; get; } = null!;

        [Required]
        [Length(minimumLength: 0, maximumLength: 50)]
        public string LastName { set; get; } = null!;

        [Required]
        [Length(minimumLength: 5, maximumLength: 50)]
        public string UserName { set; get; } = null!;

        [Required]
        public string Password { set; get; } = null!;

        [Compare("Password"), Required]
        public string ConfirmPassword { set; get; } = null!;

        [Required]
        public Gender Gender { set; get; }

        [MinAge(18)]
        public DateOnly? DateOfBirth { set; get; }

        [Phone]
        public string? PhoneNumber { set; get; }

        [MaxLength(200)]
        public string? Address { set; get; }

        [Required, EmailAddress]
        public string Email { set; get; } = null!;

        public string Role { set; get; } = AppRole.STUDENT;

        public IFormFile? Image { set; get; }
        public IFormFile? BackgroundImage { set; get; }
    }
}
