using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { set; get; }

        [Required]
        public string Password { set; get; }
    }
}
