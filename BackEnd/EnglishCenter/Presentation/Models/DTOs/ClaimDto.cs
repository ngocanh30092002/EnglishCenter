using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ClaimDto
    {
        public ClaimDto() { }
        public ClaimDto(string claimName, string claimValue)
        {
            ClaimName = claimName;
            ClaimValue = claimValue;
        }

        [Required]
        public string ClaimName { set; get; } = null!;

        [Required]
        public string ClaimValue { set; get; } = null!;
    }
}
