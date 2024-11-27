using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IUserWordRepository : IGenericRepository<UserWord>
    {
        public Task<bool> ChangePhoneticAsync(UserWord userWord, string newPhonetic);
        public Task<bool> ChangeUserAsync(UserWord userWord, string userId);
        public Task<bool> ChangeWordAsync(UserWord userWord, string newWord);
        public Task<bool> ChangeTranslationAsync(UserWord userWord, string newTranslation);
        public Task<bool> ChangeImageAsync(UserWord userWord, string imgUrl);
        public Task<bool> ChangeTagAsync(UserWord userWord, string tag);
        public Task<bool> ChangeIsFavoriteAsync(UserWord userWord, bool isFavorite);
        public Task<bool> ChangeTypeAsync(UserWord userWord, int type);
    }
}
