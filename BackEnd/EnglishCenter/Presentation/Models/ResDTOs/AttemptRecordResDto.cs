﻿namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class AttemptRecordResDto
    {
        public long? SubQueId { set; get; }
        public string? SelectedAnswer { set; get; }
        public bool? IsCorrect { set; get; }
        public AnswerToeicResDto? AnswerInfo { set; get; }
    }
}
