using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ExamInfoResDto
    {
        public long ProcessId { set; get; }
        public ExaminationDto? Examination { set; get; }
        public ToeicScoreResDto? ToeicScore { set; get; }
    }
}
