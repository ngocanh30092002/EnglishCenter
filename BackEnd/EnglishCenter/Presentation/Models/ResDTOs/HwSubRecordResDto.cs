namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class HwSubRecordResDto
    {
        public long? HwQuesId { set; get; }
        public long? HwSubQuesId { set; get; }
        public long? SubQueId { set; get; }
        public string? SelectedAnswer { set; get; }
        public bool? IsCorrect { set; get; }
        public object? AnswerInfo { set; get; }
    }
}
