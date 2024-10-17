using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class LearningProcessDto
    {
        public long? EnrollId { set; get; }

        public long? AssignmentId { set; get; }

        public int Status { set; get; } = 1;

        public string StartTime { set; get; } = DateTime.Now.ToString();
    
        public string? EndTime { set; get; }

        public List<AnswerRecordDto>? Answers { set; get; }
    }
}
