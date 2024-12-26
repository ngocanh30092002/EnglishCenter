using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IClassRoomService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeNameAsync(long id, string newName);
        public Task<Response> ChangeCapacityAsync(long id, int capacity);
        public Task<Response> ChangeLocationAsync(long id, string location);
        public Task<Response> CreateAsync(ClassRoomDto roomModel);
        public Task<Response> UpdateAsync(long id, ClassRoomDto roomModel);
        public Task<Response> DeleteAsync(long id);
    }
}
