using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class QuesLcConService : IQuesLcConService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISubLcConService _subService;
        private string _imageBasePath;
        private string _audioBasePath;

        public QuesLcConService(
            IUnitOfWork unit,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            ISubLcConService subService)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _subService = subService;
            _imageBasePath = Path.Combine("questions", "lc_conversation", "image");
            _audioBasePath = Path.Combine("questions", "lc_conversation", "audio");
        }

        public async Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile)
        {
            var queModel = _unit.QuesLcCons.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio ?? "");
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
            var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(audioFile.FileName)}";

            var isChangeSuccess = await _unit.QuesLcCons.ChangeAudioAsync(queModel, Path.Combine(_audioBasePath, fileAudio));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file audio",
                    Success = false
                };
            }

            if (File.Exists(previousPath))
            {
                File.Delete(previousPath);
            }

            var result = await UploadHelper.UploadFileAsync(audioFile, folderPath, fileAudio);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var durationAudio = await VideoHelper.GetDurationAsync(Path.Combine(folderPath, fileAudio));
            isChangeSuccess = await _unit.QuesLcCons.ChangeTimeAsync(queModel, TimeOnly.FromTimeSpan(durationAudio));

            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
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

        public async Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile)
        {
            var queModel = _unit.QuesLcCons.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image ?? "");
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath);
            var fileImage = $"image_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";

            var isChangeSuccess = await _unit.QuesLcCons.ChangeImageAsync(queModel, Path.Combine(_imageBasePath, fileImage));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file image",
                    Success = false
                };
            }

            if (queModel.Image?.Length > 0 && File.Exists(previousPath))
            {
                File.Delete(previousPath);
            }

            var result = await UploadHelper.UploadFileAsync(imageFile, folderPath, fileImage);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
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

        public async Task<Response> ChangeLevelAsync(long quesId, int level)
        {
            var queModel = _unit.QuesLcCons.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcCons.ChangeLevelAsync(queModel, level);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change level",
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

        public async Task<Response> ChangeQuantityAsync(long quesId, int quantity)
        {
            var queModel = _unit.QuesLcCons.Include(q => q.SubLcConversations).FirstOrDefault(q => q.QuesId == quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            if (quantity < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Quantity must be greater than 0",
                    Success = false
                };
            }

            if (queModel.SubLcConversations.Count > quantity)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are currently more questions than input values",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcCons.ChangeQuantityAsync(queModel, quantity);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change quantity",
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

        public async Task<Response> CreateAsync(QuesLcConDto queModel)
        {
            var queEntity = new QuesLcConversation();

            if (queModel.Image != null)
            {
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath);
                var fileImage = $"image_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Image.FileName)}";
                var result = await UploadHelper.UploadFileAsync(queModel.Image, folderPath, fileImage);
                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result,
                        Success = false
                    };
                }

                queEntity.Image = Path.Combine(_imageBasePath, fileImage);
            }

            if (queModel.Audio != null)
            {
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
                var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Audio.FileName)}";
                var result = await UploadHelper.UploadFileAsync(queModel.Audio, folderPath, fileAudio);
                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result,
                        Success = false
                    };
                }

                var durationVideo = await VideoHelper.GetDurationAsync(Path.Combine(folderPath, fileAudio));

                queEntity.Time = TimeOnly.FromTimeSpan(durationVideo);
                queEntity.Audio = Path.Combine(_audioBasePath, fileAudio);
            }
            else
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Audio is required",
                    Success = false
                };
            }

            if (queModel.Quantity <= 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Quantity must be greater than 0",
                    Success = false
                };
            }

            queEntity.Quantity = queModel.Quantity ?? 1;
            queEntity.Level = queModel.Level ?? 1;

            try
            {
                _unit.QuesLcCons.Add(queEntity);
                await _unit.CompleteAsync();

                if (queModel.SubLcCons != null && queModel.SubLcCons.Count != 0)
                {
                    foreach (var sub in queModel.SubLcCons)
                    {
                        sub.PreQuesId = queEntity.QuesId;

                        var createRes = await _subService.CreateAsync(sub);
                        if (!createRes.Success) return createRes;
                    }
                }

                await _unit.CommitTransAsync();
                await _unit.CompleteAsync();

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

        public async Task<Response> DeleteAsync(long quesId)
        {
            var queModel = _unit.QuesLcCons.Include(q => q.SubLcConversations).FirstOrDefault(q => q.QuesId == quesId);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            if (queModel.Image?.Length > 0)
            {
                var previousImagePath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image);
                if (File.Exists(previousImagePath))
                {
                    File.Delete(previousImagePath);
                }
            }

            var previousAudioPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio);
            if (File.Exists(previousAudioPath))
            {
                File.Delete(previousAudioPath);
            }

            var subIds = queModel.SubLcConversations.Select(s => s.SubId).ToList();

            foreach (var subId in subIds)
            {
                await _subService.DeleteAsync(subId);
            }

            _unit.QuesLcCons.Remove(queModel);
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
            var queModels = _unit.QuesLcCons.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesLcConResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long quesId)
        {
            var queModel = _unit.QuesLcCons.GetById(quesId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesLcConResDto>(queModel),
                Success = true
            });
        }

        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId)
        {
            var assignQues = _unit.AssignQues
                                  .Find(a => a.AssignmentId == assignmentId && a.Type == (int)QuesTypeEnum.Conversation)
                                  .Select(a => a.ConversationQuesId)
                                  .ToList();

            var queModels = _unit.QuesLcCons
                                .GetAll()
                                .Where(q => !assignQues.Contains(q.QuesId))
                                .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesLcConResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId)
        {
            var homeQues = _unit.HomeQues
                                  .Find(a => a.HomeworkId == homeworkId && a.Type == (int)QuesTypeEnum.Conversation)
                                  .Select(a => a.ConversationQuesId)
                                  .ToList();

            var queModels = _unit.QuesLcCons
                                .GetAll()
                                .Where(q => !homeQues.Contains(q.QuesId))
                                .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesLcConResDto>>(queModels),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long quesId, QuesLcConDto queModel)
        {
            await _unit.BeginTransAsync();

            try
            {
                if (queModel.Quantity.HasValue)
                {
                    var changeResponse = await ChangeQuantityAsync(quesId, queModel.Quantity.Value);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Image != null)
                {
                    var changeResponse = await ChangeImageAsync(quesId, queModel.Image);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Audio != null)
                {
                    var changeResponse = await ChangeAudioAsync(quesId, queModel.Audio);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Level.HasValue)
                {
                    var changeResponse = await ChangeLevelAsync(quesId, queModel.Level ?? 1);
                    if (!changeResponse.Success) return changeResponse;
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
    }
}
