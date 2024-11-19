using System.ComponentModel.DataAnnotations;
using EnglishCenter.Presentation.Attribute;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.Presentation.Models
{
    public class RegisterModel
    {
        [Required]
        [Length(minimumLength: 0, maximumLength: 50)]
        public string FirstName { set; get; }

        [Required]
        [Length(minimumLength: 0, maximumLength: 50)]
        public string LastName { set; get; }

        [Required]
        [Length(minimumLength: 5, maximumLength: 50)]
        public string UserName { set; get; }

        [Required]
        public string Password { set; get; }

        [Compare("Password"), Required]
        public string ConfirmPassword { set; get; }

        [Required]
        public Gender Gender { set; get; }

        [MinAge(18)]
        public DateOnly? DateOfBirth { set; get; }

        [Phone]
        public string? PhoneNumber { set; get; }

        [MaxLength(200)]
        public string? Address { set; get; }

        [Required, EmailAddress]
        public string Email { set; get; }
    }
}
