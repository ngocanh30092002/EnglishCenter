using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Models.DTO
{
    public class UserBackgroundDtoModel
    {
        [Required]
        public string UserName { set; get; }

        public string? Description { set; get; }

        public List<string>? Roles { set; get; }
    }
}
