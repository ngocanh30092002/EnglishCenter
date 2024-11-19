using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class QuesToeicService : IQuesToeicService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ISubToeicService _subService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuesToeicService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, ISubToeicService subService)
        {
            _unit = unit;
            _mapper = mapper;
            _subService = subService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response> ChangeAudioAsync(long id, IFormFile? audioFile)
        {
            var queModel = await _unit.QuesToeic
                                .Include(q => q.ToeicExam)
                                .FirstOrDefaultAsync(q => q.QuesId == id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var preAudioPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio ?? "");
            var audioPath = string.Empty;
            var fileAudioName = string.Empty;

            if (audioFile != null)
            {
                fileAudioName = $"Toeic_{queModel.ToeicExam.Code}_{queModel.ToeicExam.Year}_{DateTime.Now.Ticks}{Path.GetExtension(audioFile.FileName)}";
                audioPath = Path.Combine("toeic", queModel.ToeicExam.Year.ToString(), queModel.ToeicExam.Code.ToString(), "Part_" + queModel.Part);

                var result = await UploadHelper.UploadFileAsync(audioFile, Path.Combine(_webHostEnvironment.WebRootPath, audioPath), fileAudioName);
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

            if (File.Exists(preAudioPath))
            {
                File.Delete(preAudioPath);
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeAudioAsync(queModel, Path.Combine(audioPath, fileAudioName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change audio file failed",
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

        public async Task<Response> ChangeGroupAsync(long id, bool isGroup)
        {
            var queModel = _unit.QuesToeic.GetById(id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeGroupAsync(queModel, isGroup);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change group failed",
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

        public async Task<Response> ChangeImage1Async(long id, IFormFile? imageFile)
        {
            var queModel = await _unit.QuesToeic
                               .Include(q => q.ToeicExam)
                               .FirstOrDefaultAsync(q => q.QuesId == id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var preImagePath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image_1 ?? "");
            var imagePath = string.Empty;
            var fileImageName = string.Empty;

            if (imageFile != null)
            {
                fileImageName = $"Toeic_{queModel.ToeicExam.Code}_{queModel.ToeicExam.Year}_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
                imagePath = Path.Combine("toeic", queModel.ToeicExam.Year.ToString(), queModel.ToeicExam.Code.ToString(), "Part_" + queModel.Part);

                var result = await UploadHelper.UploadFileAsync(imageFile, Path.Combine(_webHostEnvironment.WebRootPath, imagePath), fileImageName);
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

            if (File.Exists(preImagePath))
            {
                File.Delete(preImagePath);
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeImage1Async(queModel, Path.Combine(imagePath, fileImageName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change image file failed",
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

        public async Task<Response> ChangeImage2Async(long id, IFormFile? imageFile)
        {
            var queModel = await _unit.QuesToeic
                                       .Include(q => q.ToeicExam)
                                       .FirstOrDefaultAsync(q => q.QuesId == id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var preImagePath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image_2 ?? "");
            var imagePath = string.Empty;
            var fileImageName = string.Empty;

            if (imageFile != null)
            {
                fileImageName = $"Toeic_{queModel.ToeicExam.Code}_{queModel.ToeicExam.Year}_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
                imagePath = Path.Combine("toeic", queModel.ToeicExam.Year.ToString(), queModel.ToeicExam.Code.ToString(), "Part_" + queModel.Part);

                var result = await UploadHelper.UploadFileAsync(imageFile, Path.Combine(_webHostEnvironment.WebRootPath, imagePath), fileImageName);
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

            if (File.Exists(preImagePath))
            {
                File.Delete(preImagePath);
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeImage2Async(queModel, Path.Combine(imagePath, fileImageName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change image file failed",
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

        public async Task<Response> ChangeImage3Async(long id, IFormFile? imageFile)
        {
            var queModel = await _unit.QuesToeic
                                      .Include(q => q.ToeicExam)
                                      .FirstOrDefaultAsync(q => q.QuesId == id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var preImagePath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image_3 ?? "");
            var imagePath = string.Empty;
            var fileImageName = string.Empty;

            if (imageFile != null)
            {
                fileImageName = $"Toeic_{queModel.ToeicExam.Code}_{queModel.ToeicExam.Year}_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
                imagePath = Path.Combine("toeic", queModel.ToeicExam.Year.ToString(), queModel.ToeicExam.Code.ToString(), "Part_" + queModel.Part);

                var result = await UploadHelper.UploadFileAsync(imageFile, Path.Combine(_webHostEnvironment.WebRootPath, imagePath), fileImageName);
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

            if (File.Exists(preImagePath))
            {
                File.Delete(preImagePath);
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeImage3Async(queModel, Path.Combine(imagePath, fileImageName));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change image file failed",
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

        public async Task<Response> ChangeNoNumAsync(long id, int noNum)
        {
            var queModel = _unit.QuesToeic.GetById(id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesToeic.ChangeNoNumAsync(queModel, noNum);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change no num failed",
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

        public async Task<Response> CreateAsync(QuesToeicDto model)
        {
            var toeicModel = _unit.ToeicExams.GetById(model.ToeicId);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examinations",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(PartEnum), model.Part))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid part",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part1 && (model.Audio == null || model.Image_1 == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 1 requires image and audio files.",
                    Success = false
                };
            }

            if ((model.Part == (int)PartEnum.Part2 || model.Part == (int)PartEnum.Part3 || model.Part == (int)PartEnum.Part4) && model.Audio == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = $"Part {model.Part} requires audio file",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part6 && model.Image_1 != null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = $"Part {model.Part} requires image file",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part7 && (model.Image_1 == null && model.Image_2 == null && model.Image_3 == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = $"Part {model.Part} requires image file",
                    Success = false
                };
            }

            var isFull = await IsFullQuesAsync(toeicModel, model.Part);
            if (isFull)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't add more question in this part",
                    Success = false
                };
            }

            var queModel = new QuesToeic();
            queModel.ToeicId = model.ToeicId;
            queModel.Part = model.Part;
            queModel.IsGroup = model.IsGroup;
            queModel.NoNum = await NextNoNumAsync(model.ToeicId, model.Part);

            await _unit.BeginTransAsync();
            try
            {
                _unit.QuesToeic.Add(queModel);
                await _unit.CompleteAsync();

                var response = await ChangeAudioAsync(queModel.QuesId, model.Audio);
                if (!response.Success) return response;

                response = await ChangeImage1Async(queModel.QuesId, model.Image_1);
                if (!response.Success) return response;

                response = await ChangeImage2Async(queModel.QuesId, model.Image_2);
                if (!response.Success) return response;

                response = await ChangeImage3Async(queModel.QuesId, model.Image_3);
                if (!response.Success) return response;

                if (model.NoNum.HasValue && model.NoNum.Value != queModel.NoNum)
                {
                    response = await ChangeNoNumAsync(queModel.QuesId, model.NoNum.Value);
                    if (!response.Success) return response;
                }

                if (model.SubToeicDtos != null && model.SubToeicDtos.Count > 0)
                {
                    foreach (var sub in model.SubToeicDtos)
                    {
                        sub.QuesId = queModel.QuesId;

                        var createRes = await _subService.CreateAsync(sub);
                        if (!createRes.Success) return createRes;
                    }
                }

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

        public async Task<Response> DeleteAsync(long id)
        {
            var queModel = _unit.QuesToeic
                                .Include(s => s.SubToeicList)
                                .FirstOrDefault(s => s.QuesId == id);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var response = await ChangeAudioAsync(id, null);
            if (!response.Success) return response;

            response = await ChangeImage1Async(id, null);
            if (!response.Success) return response;

            response = await ChangeImage2Async(id, null);
            if (!response.Success) return response;

            response = await ChangeImage3Async(id, null);
            if (!response.Success) return response;

            if (queModel.SubToeicList.Count > 0)
            {
                var subIds = queModel.SubToeicList.Select(s => s.SubId).ToList();

                foreach (var subId in subIds)
                {
                    var deleteRes = await _subService.DeleteAsync(subId);
                    if (!deleteRes.Success) return deleteRes;
                }
            }

            _unit.QuesToeic.Remove(queModel);
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
            var queModels = _unit.QuesToeic.GetAll().OrderBy(q => q.NoNum);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var queModel = _unit.QuesToeic.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesToeicResDto>(queModel),
                Success = true
            });
        }

        public async Task<Response> GetByToeicAsync(long toeicId)
        {
            var queModels = _unit.QuesToeic.Find(q => q.ToeicId == toeicId).OrderBy(q => q.NoNum);

            foreach (var que in queModels)
            {
                await _unit.QuesToeic.LoadSubQuesAsync(que);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(queModels),
                Success = true
            };
        }

        public Task<Response> GetTotalNumberSentences(long toeicId)
        {
            var subToeic = _unit.SubToeic
                                 .Include(s => s.QuesToeic)
                                 .Where(s => s.QuesToeic.ToeicId == toeicId)
                                 .ToList();

            var numQuesToeic = new NumQuesToeicResDto()
            {
                Part1 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part1).Count(),
                Part2 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part2).Count(),
                Part3 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part3).Count(),
                Part4 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part4).Count(),
                Part5 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part5).Count(),
                Part6 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part6).Count(),
                Part7 = subToeic.Where(s => s.QuesToeic.Part == (int)PartEnum.Part7).Count(),
            };

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numQuesToeic,
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, QuesToeicDto model)
        {
            var queModel = _unit.QuesToeic.GetById(id);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                var response = await ChangeAudioAsync(id, model.Audio);
                if (!response.Success) return response;

                response = await ChangeImage1Async(id, model.Image_1);
                if (!response.Success) return response;

                response = await ChangeImage2Async(id, model.Image_2);
                if (!response.Success) return response;

                response = await ChangeImage3Async(id, model.Image_3);
                if (!response.Success) return response;

                if (queModel.IsGroup != model.IsGroup)
                {
                    response = await ChangeGroupAsync(id, model.IsGroup);
                    if (!response.Success) return response;
                }

                if (model.NoNum.HasValue && queModel.NoNum != model.NoNum)
                {
                    response = await ChangeNoNumAsync(id, model.NoNum.Value);
                    if (!response.Success) return response;
                }

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

        private Task<bool> IsFullQuesAsync(ToeicExam toeicModel, int part)
        {
            var numQues = _unit.QuesToeic.Find(q => q.ToeicId == toeicModel.ToeicId && q.Part == part).Count();

            switch (part)
            {
                case (int)PartEnum.Part1:
                    return Task.FromResult(numQues >= 6);
                case (int)PartEnum.Part2:
                    return Task.FromResult(numQues >= 25);
                case (int)PartEnum.Part3:
                    return Task.FromResult(numQues >= 13);
                case (int)PartEnum.Part4:
                    return Task.FromResult(numQues >= 10);
                case (int)PartEnum.Part5:
                    return Task.FromResult(numQues >= 30);
                case (int)PartEnum.Part6:
                    return Task.FromResult(numQues >= 4);
                case (int)PartEnum.Part7:
                    return Task.FromResult(numQues >= 15);
                default:
                    throw new NotImplementedException();
            }
        }

        public Task<int> NextNoNumAsync(long toeicId, int part)
        {
            var toeicModel = _unit.ToeicExams.GetById(toeicId);
            if (toeicModel == null) return Task.FromResult(-1);

            var numbers = _unit.QuesToeic
                               .Find(q => q.ToeicId == toeicId && q.Part == part)
                               .OrderBy(q => q.NoNum)
                               .Select(q => q.NoNum)
                               .ToArray();

            int startNum = 0;
            int endNum = 0;

            switch (part)
            {
                case (int)PartEnum.Part1:
                    startNum = 1;
                    endNum = 6;
                    break;
                case (int)PartEnum.Part2:
                    startNum = 7;
                    endNum = 31;
                    break;
                case (int)PartEnum.Part3:
                    startNum = 32;
                    endNum = 44;
                    break;
                case (int)PartEnum.Part4:
                    startNum = 45;
                    endNum = 54;
                    break;
                case (int)PartEnum.Part5:
                    startNum = 55;
                    endNum = 84;
                    break;
                case (int)PartEnum.Part6:
                    startNum = 85;
                    endNum = 88;
                    break;
                case (int)PartEnum.Part7:
                    startNum = 89;
                    endNum = 103;
                    break;
            }

            int result = -1;
            for (int i = startNum; i <= endNum; i++)
            {
                if (i - startNum >= numbers.Length)
                {
                    result = i;
                    break;
                }

                if (numbers[i - startNum] != i)
                {
                    result = i;
                    break;
                }
            }

            return Task.FromResult(result);
        }
    }
}
