using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AnswerRecordDto
    {
        [Required]
        public long ProcessId { set; get; }
        
        [Required]
        public long AssignQuesId { set; get; }
        
        public long? SubId { set; get; }
        
        [Required]
        public string SelectedAnswer { set; get; } = null!;
        
        [Required]
        public bool IsCorrect { set; get; }
    }
}
