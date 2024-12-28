using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class ClassRoomRepository : GenericRepository<ClassRoom>, IClassRoomRepository
    {
        public ClassRoomRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeCapacityAsync(ClassRoom room, int capacity)
        {
            if (room == null) return false;

            if (capacity < 0) return false;

            var classes = await context.Lessons
                                 .Include(l => l.Class)
                                 .Where(l => l.ClassRoomId == room.ClassRoomId)
                                 .Select(l => l.Class)
                                 .ToListAsync();

            var isValidCapacity = classes.Any(c => c.RegisteredNum > capacity);
            if (isValidCapacity) return false;

            room.Capacity = capacity;

            return true;
        }

        public Task<bool> ChangeLocationAsync(ClassRoom room, string location)
        {
            if (room == null) return Task.FromResult(false);
            room.Location = location;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeNameAsync(ClassRoom room, string newName)
        {
            if (room == null) return Task.FromResult(false);

            room.ClassRoomName = newName;

            return Task.FromResult(true);
        }
    }
}
