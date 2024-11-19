using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AssignRecordDto
    {
        [Required]
        public long ProcessId { set; get; }

        [Required]
        public long AssignQuesId { set; get; }

        public long? SubId { set; get; }

        public string? SelectedAnswer { set; get; }

        public bool IsCorrect { set; get; }
    }
}
