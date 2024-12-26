using System.Text.RegularExpressions;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Business.Services.Dictionary
{
    public class UserWordService : IUserWordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public UserWordService(IMapper mapper, IUnitOfWork unit, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<Response> ChangeAllFavoriteAsync(string userId, bool isFavorite)
        {
            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user model",
                    Success = false
                };
            }

            var models = _unit.UserWords.Find(w => w.UserId == userId);

            foreach (var model in models)
            {
                model.IsFavorite = isFavorite;
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeImageAsync(long id, IFormFile imageFile)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, wordModel.Image ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "dictionary");
            var fileName = $"{DateTime.Now.Ticks}_{imageFile.FileName}";
            var result = await UploadHelper.UploadFileAsync(imageFile, folderPath, fileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeImageAsync(wordModel, Path.Combine("dictionary", fileName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change image",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeIsFavoriteAsync(long id, bool isFavorite)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeIsFavoriteAsync(wordModel, isFavorite);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change favorite",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangePhoneticAsync(long id, string newPhonetic)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangePhoneticAsync(wordModel, newPhonetic);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change phonetic",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeTagAsync(long id, string tag)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeTagAsync(wordModel, tag);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change tag",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeTranslationAsync(long id, string newTranslation)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeTranslationAsync(wordModel, newTranslation);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change translation",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeTypeAsync(long id, int type)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeTypeAsync(wordModel, type);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change type",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeUserAsync(long id, string userId)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeUserAsync(wordModel, userId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change type",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeWordAsync(long id, string newWord)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserWords.ChangeWordAsync(wordModel, newWord);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change type",
                    Success = false
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> CreateAsync(UserWordDto wordModel)
        {
            var userModel = await _userManager.FindByIdAsync(wordModel.UserId ?? "");
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user",
                    Success = false
                };
            }

            var folderName = string.Empty;
            var fileName = string.Empty;

            if (wordModel.Image != null)
            {
                folderName = Path.Combine(_webHostEnvironment.WebRootPath, "dictionary");
                fileName = $"{DateTime.Now.Ticks}_{wordModel.Image.FileName}";

                var result = await UploadHelper.UploadFileAsync(wordModel.Image, folderName, fileName);
                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result,
                        Success = false
                    };
                }
            }

            if (wordModel.Type < 1 || wordModel.Type > 9)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Type is invalid",
                    Success = false
                };
            }

            if (!Regex.IsMatch(wordModel.Tag, @"^#[A-Z0-9]+$"))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Tag isn't valid with format #ABC",
                    Success = false
                };
            }

            var wordEntity = new UserWord()
            {
                UserId = userModel.Id,
                Word = wordModel.Word,
                Translation = wordModel.Translation,
                Image = wordModel.Image != null ? Path.Combine("dictionary", fileName) : null,
                Tag = wordModel.Tag,
                Type = wordModel.Type,
                Phonetic = wordModel.Phonetic ?? "",
                IsFavorite = false,
                UpdateDate = DateTime.Now
            };

            _unit.UserWords.Add(wordEntity);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var wordModel = _unit.UserWords.GetById(id);
            if (wordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            var previousFile = Path.Combine(_webHostEnvironment.WebRootPath, wordModel.Image ?? "");
            if (File.Exists(previousFile))
            {
                File.Delete(previousFile);
            }

            _unit.UserWords.Remove(wordModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetAllAsync()
        {
            var models = _unit.UserWords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserWordResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.UserWords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<UserWordResDto>(model),
                Success = true
            });
        }

        public Task<Response> GetByUserAsync(string userId)
        {
            var models = _unit.UserWords.Find(w => w.UserId == userId).ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserWordResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetByUserWithFavoriteAsync(string userId)
        {
            var models = _unit.UserWords.Find(w => w.UserId == userId && w.IsFavorite == true).ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserWordResDto>>(models),
                Success = true
            });
        }


        public Task<Response> GetWordTypeAsync()
        {
            var types = Enum.GetValues(typeof(WordTypeEnum))
                           .Cast<WordTypeEnum>()
                           .Select(type => new KeyValuePair<string, int>(type.ToString(), (int)type))
                           .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = types,
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, UserWordDto wordModel)
        {
            var wordEntity = _unit.UserWords.GetById(id);
            if (wordEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any user words",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (wordEntity.UserId != wordModel.UserId)
                {
                    var res = await ChangeUserAsync(id, wordModel.UserId ?? "");
                    if (!res.Success) return res;
                }

                if (wordEntity.Word != wordModel.Word)
                {
                    var res = await ChangeWordAsync(id, wordModel.Word);
                    if (!res.Success) return res;
                }

                if (wordEntity.Translation != wordModel.Translation)
                {
                    var res = await ChangeTranslationAsync(id, wordModel.Translation);
                    if (!res.Success) return res;
                }

                if (!string.IsNullOrEmpty(wordModel.Phonetic) && wordEntity.Phonetic != wordModel.Phonetic)
                {
                    var res = await ChangePhoneticAsync(id, wordModel.Phonetic);
                    if (!res.Success) return res;
                }

                if (wordModel.Image != null)
                {
                    var res = await ChangeImageAsync(id, wordModel.Image);
                    if (!res.Success) return res;
                }

                if (wordEntity.Tag != wordModel.Tag)
                {
                    var res = await ChangeTagAsync(id, wordModel.Tag);
                    if (!res.Success) return res;
                }

                if (wordEntity.Type != wordModel.Type)
                {
                    var res = await ChangeTypeAsync(id, wordModel.Type);
                    if (!res.Success) return res;
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
