using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models
{
    public class MailContent
    {
        [Required]
        public string Subject { set; get; }

        [EmailAddress]
        public string To { set; get; }

        [EmailAddress]
        public string From { set; get; }

        [Required]
        public string Body { set; get; }
    }
}
