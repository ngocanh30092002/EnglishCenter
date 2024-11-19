using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ToeicAttemptDto
    {
        public string? UserId { set; get; }

        [Required]
        public long ToeicId { set; get; }
        public int? Listening_Score { set; get; }
        public int? Reading_Score { set; get; }
        public List<ToeicPracticeRecordDto>? PracticeRecords { set; get; }
    }
}
