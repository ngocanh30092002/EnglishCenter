﻿namespace EnglishCenter.Presentation.Models.DTOs
{
    public class AttemptRecordDto
    {
        public string? UserId { set; get; }
        public long SubId { set; get; }
        public long AttemptId { set; get; }
        public string? SelectedAnswer { set; get; }

    }
}
