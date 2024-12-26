using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class HwSubRecordDto
    {
        [Required]
        public long SubmissionId { set; get; }

        public long? HwQuesId { set; get; }

        public long? HwSubQuesId { set; get; }

        public string? SelectedAnswer { set; get; }
        public long? SubToeicId { set; get; }
    }
}
