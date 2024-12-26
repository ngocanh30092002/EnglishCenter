using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IUserWordService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByUserAsync(string userId);
        public Task<Response> GetByUserWithFavoriteAsync(string userId);
        public Task<Response> GetWordTypeAsync();
        public Task<Response> CreateAsync(UserWordDto wordModel);
        public Task<Response> UpdateAsync(long id, UserWordDto wordModel);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> ChangeUserAsync(long id, string userId);
        public Task<Response> ChangeWordAsync(long id, string newWord);
        public Task<Response> ChangeTranslationAsync(long id, string newTranslation);
        public Task<Response> ChangePhoneticAsync(long id, string newPhonetic);
        public Task<Response> ChangeImageAsync(long id, IFormFile imageFile);
        public Task<Response> ChangeTagAsync(long id, string tag);
        public Task<Response> ChangeIsFavoriteAsync(long id, bool isFavorite);
        public Task<Response> ChangeAllFavoriteAsync(string userId, bool isFavorite);
        public Task<Response> ChangeTypeAsync(long id, int type);
    }
}
