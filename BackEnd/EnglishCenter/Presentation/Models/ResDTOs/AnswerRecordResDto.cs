namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class AnswerRecordResDto
    {
        public long? AssignQuesId { set; get; }
        public long? SubQueId { set; get; }
        public string? SelectedAnswer { set; get; }
        public bool? IsCorrect { set; get; }
    }
}
