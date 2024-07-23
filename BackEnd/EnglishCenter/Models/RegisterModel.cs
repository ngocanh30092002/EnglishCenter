using System.ComponentModel.DataAnnotations;
using EnglishCenter.Global.Enum;

namespace EnglishCenter.Models
{
    public class RegisterModel
    {
        [Required]
        [Length(minimumLength: 0, maximumLength: 20 )]
        public string FirstName { set; get; }

        [Required]
        [Length(minimumLength:0, maximumLength: 20)]
        public string LastName { set; get; }

        [Required]
        [Length(minimumLength:5 , maximumLength: 20)]
        public string UserName { set; get; }

        [Required]
        public string Password { set; get; }    

        [Required]
        public Gender Gender { set; get; }

        [Required]
        public DateTime DateOfBirth { set; get; }

        [Required, Phone]
        public string PhoneNumber {  set; get; }
        
        [Required]
        [MaxLength(30)]
        public string Address { set; get; }
        
        [Required,EmailAddress]        
        public string Email { set; get; }
    }
}
