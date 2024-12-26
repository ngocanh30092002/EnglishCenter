namespace EnglishCenter.Presentation.Models.DTOs
{
    public class ClassRoomDto
    {
        public long Id { get; set; }
        public string ClassRoomName { set; get; } = null!;
        public int Capacity { set; get; }
        public string? Location { set; get; }
    }
}
