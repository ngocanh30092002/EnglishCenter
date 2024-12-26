using System.Text.RegularExpressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.DataAccess.Repositories.DictionaryRepositories
{
    public class UserWordRepository : GenericRepository<UserWord>, IUserWordRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public UserWordRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public Task<bool> ChangeImageAsync(UserWord userWord, string imgUrl)
        {
            if (userWord == null) return Task.FromResult(false);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imgUrl);
            if (!File.Exists(filePath)) return Task.FromResult(false);

            userWord.Image = imgUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIsFavoriteAsync(UserWord userWord, bool isFavorite)
        {
            if (userWord == null) return Task.FromResult(false);

            userWord.IsFavorite = isFavorite;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePhoneticAsync(UserWord userWord, string newPhonetic)
        {
            if (userWord == null) return Task.FromResult(false);

            userWord.Phonetic = newPhonetic;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTagAsync(UserWord userWord, string tag)
        {
            if (userWord == null) return Task.FromResult(false);

            bool isValid = Regex.IsMatch(tag, @"^#[A-Z0-9]+$");
            if (!isValid) return Task.FromResult(false);

            userWord.Tag = tag;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTranslationAsync(UserWord userWord, string newTranslation)
        {
            if (userWord == null) return Task.FromResult(false);

            userWord.Translation = newTranslation;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTypeAsync(UserWord userWord, int type)
        {
            if (userWord == null) return Task.FromResult(false);
            if (type < 1 || type > 9) return Task.FromResult(false);

            userWord.Type = type;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeUserAsync(UserWord userWord, string userId)
        {
            if (userWord == null) return false;

            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null) return false;

            userWord.UserId = userId;

            return true;
        }

        public Task<bool> ChangeWordAsync(UserWord userWord, string newWord)
        {
            if (userWord == null) return Task.FromResult(false);

            userWord.Word = newWord;

            return Task.FromResult(true);
        }
    }
}
