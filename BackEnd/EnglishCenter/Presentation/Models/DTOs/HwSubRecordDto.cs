using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HwSubRecordDto
    {
        [Required]
        public long SubmissionId { set; get; }

        [Required]
        public long HwQuesId { set; get; }

        public long? HwSubQuesId { set; get; }

        public string? SelectedAnswer { set; get; }
    }
}
