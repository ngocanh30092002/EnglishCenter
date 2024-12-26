using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class ClassRoomRepository : GenericRepository<ClassRoom>, IClassRoomRepository
    {
        public ClassRoomRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeCapacityAsync(ClassRoom room, int capacity)
        {
            if (room == null) return Task.FromResult(false);

            if (capacity < 0) return Task.FromResult(false);

            room.Capacity = capacity;

            return Task.FromResult(true);
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
