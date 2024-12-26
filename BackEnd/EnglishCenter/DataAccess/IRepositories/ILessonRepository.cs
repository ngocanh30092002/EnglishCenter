using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        public Task<bool> ChangeClassAsync(Lesson lesson, string classId);
        public Task<bool> ChangeDateAsync(Lesson lesson, DateOnly dateOnly);
        public Task<bool> ChangeStartPeriodAsync(Lesson lesson, int startPeriod);
        public Task<bool> ChangeEndPeriodAsync(Lesson lesson, int endPeriod);
        public Task<bool> ChangeTopicAsync(Lesson lesson, string topic);
        public Task<bool> ChangeClassRoomAsync(Lesson lesson, long classRoomId);
        public Task<bool> IsDuplicateTeacherAsync(DateOnly date, int start, int end, string teacherId, long? lessonId = null);
        public Task<bool> IsDuplicateAsync(DateOnly date, long classRoomId, int start, int end, long? lessonId = null);
    }
}
