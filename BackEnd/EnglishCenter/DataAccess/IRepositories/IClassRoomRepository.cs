using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IClassRoomRepository : IGenericRepository<ClassRoom>
    {
        public Task<bool> ChangeNameAsync(ClassRoom room, string newName);
        public Task<bool> ChangeCapacityAsync(ClassRoom room, int capacity);
        public Task<bool> ChangeLocationAsync(ClassRoom room, string location);
    }
}
