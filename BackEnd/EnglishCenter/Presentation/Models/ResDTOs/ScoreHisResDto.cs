using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Presentation.Models.ResDTOs
{
    public class ScoreHisResDto
    {
        public UserInfoDto? UserInfo { get; set; }
        public UserBackgroundDto? UserBackground { get; set; }
        public long ScoreHisId { set; get; }
        public int Entrance_Point { set; get; }
        public int Midterm_Point { set; get; }
        public int Final_Point { set; get; }
    }
}
