using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeClassAsync(Lesson lesson, string classId)
        {
            if (lesson == null) return false;

            var classModel = await context.Classes.FindAsync(classId);
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (classModel == null) return false;
            if (classModel.Status == (int)ClassEnum.End) return false;
            if (classModel.StartDate > currentDate || classModel.EndDate < currentDate) return false;

            var isExistClass = context.Lessons
                                      .Where(s => s.ClassId == classId &&
                                                  s.Date == lesson.Date &&
                                                  s.StartPeriod == lesson.StartPeriod &&
                                                  s.EndPeriod == lesson.EndPeriod)
                                      .Any();

            if (isExistClass) return false;


            lesson.ClassId = classId;

            return true;
        }

        public async Task<bool> ChangeClassRoomAsync(Lesson lesson, long classRoomId)
        {
            if (lesson == null) return false;

            var classRoomModel = await context.ClassRooms.FindAsync(classRoomId);
            if (classRoomModel == null) return false;

            var isExistClass = context.Lessons
                                    .Where(s => s.ClassId == lesson.ClassId &&
                                                s.Date == lesson.Date &&
                                                s.StartPeriod == lesson.StartPeriod &&
                                                s.EndPeriod == lesson.EndPeriod)
                                    .Any();

            if (isExistClass) return false;

            var isDuplicate = await IsDuplicateAsync(lesson.Date, classRoomId, lesson.StartPeriod, lesson.EndPeriod);
            if (isDuplicate) return false;

            lesson.ClassRoomId = classRoomId;
            return true;
        }

        public async Task<bool> ChangeDateAsync(Lesson lesson, DateOnly dateOnly)
        {
            if (lesson == null) return false;

            var isDuplicate = await IsDuplicateAsync(dateOnly, lesson.ClassRoomId, lesson.StartPeriod, lesson.EndPeriod);
            if (isDuplicate) return false;

            lesson.Date = dateOnly;

            return true;
        }

        public async Task<bool> ChangeEndPeriodAsync(Lesson lesson, int endPeriod)
        {
            if (lesson == null) return false;
            if (endPeriod < 0 || endPeriod > 13) return false;
            if (lesson.StartPeriod > endPeriod) return false;

            var isDuplicate = await IsDuplicateAsync(lesson.Date, lesson.ClassRoomId, lesson.StartPeriod, endPeriod, lesson.LessonId);
            if (isDuplicate) return false;

            lesson.EndPeriod = endPeriod;

            return true;
        }

        public async Task<bool> ChangeStartPeriodAsync(Lesson lesson, int startPeriod)
        {
            if (lesson == null) return false;
            if (startPeriod < 0 || startPeriod > 13) return false;
            if (lesson.EndPeriod < startPeriod) return false;

            var isDuplicate = await IsDuplicateAsync(lesson.Date, lesson.ClassRoomId, startPeriod, lesson.EndPeriod, lesson.LessonId);
            if (isDuplicate) return false;

            lesson.StartPeriod = startPeriod;

            return true;
        }

        public Task<bool> ChangeTopicAsync(Lesson lesson, string topic)
        {
            if (lesson == null) return Task.FromResult(false);

            lesson.Topic = topic;

            return Task.FromResult(true);
        }

        public async Task<bool> IsDuplicateAsync(DateOnly date, long classRoomId, int start, int end, long? lessonId = null)
        {
            var lessonInDate = new List<Lesson>();

            if (lessonId.HasValue)
            {
                lessonInDate = await context.Lessons
                                            .Where(l => l.Date == date && l.ClassRoomId == classRoomId && l.LessonId != lessonId.Value)
                                            .ToListAsync();
            }
            else
            {
                lessonInDate = await context.Lessons
                                            .Where(l => l.Date == date && l.ClassRoomId == classRoomId)
                                            .ToListAsync();
            }

            foreach (var lesson in lessonInDate)
            {
                if (lesson.StartPeriod <= start && start <= lesson.EndPeriod)
                {
                    return true;
                }
                if (lesson.StartPeriod <= end && end <= lesson.EndPeriod)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
