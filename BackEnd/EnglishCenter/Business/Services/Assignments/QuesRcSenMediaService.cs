using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Assignments
{
    public class QuesRcSenMediaService : IQuesRcSenMediaService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _audioBasePath;
        private string _imageBasePath;

        public QuesRcSenMediaService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment) 
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _audioBasePath = Path.Combine("questions", "sen_media", "audio");
            _imageBasePath = Path.Combine("questions", "sen_media", "image");
        }
        public async Task<Response> ChangeAnswerAAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAnswerAAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeAnswerAsync(long quesId, long answerId)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isExistAnswer = _unit.AnswerRcMedia.IsExist(a => a.AnswerId == answerId);
            if (!isExistAnswer)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAnswerAsync(queModel, answerId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeAnswerBAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAnswerBAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeAnswerCAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAnswerCAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeAnswerDAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAnswerDAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeAudioAsync(long quesId, IFormFile? audioFile)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if(queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio ?? "");
            if(audioFile == null)
            {
                if (File.Exists(previousPath))
                {
                    File.Delete(previousPath);
                }

                var isSuccess = await _unit.QuesRcSenMedia.ChangeAudioAsync(queModel, null);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't change file audio",
                        Success = false
                    };
                }
            }
            else
            {
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
                var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(audioFile.FileName)}";

                var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeAudioAsync(queModel, Path.Combine(_audioBasePath, fileAudio));
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
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath);
            var fileImage = $"image_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image);

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeImageAsync(queModel, Path.Combine(_imageBasePath, fileImage));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file image",
                    Success = false
                };
            }

            if (File.Exists(previousPath))
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

        public async Task<Response> ChangeQuestionAsync(long quesId, string newQuestion)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeQuestionAsync(queModel, newQuestion);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly)
        {
            var queModel = _unit.QuesRcSenMedia.GetById(quesId);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSenMedia.ChangeTimeAsync(queModel, timeOnly);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change time",
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

        public async Task<Response> CreateAsync(QuesRcSenMediaDto queModel)
        {
            if(queModel.Image == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Image file is required",
                    Success = false
                };
            }

            var imgFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath);
            var fileImage = $"image_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Image.FileName)}";

            var result = await UploadHelper.UploadFileAsync(queModel.Image, imgFolderPath, fileImage);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            if (string.IsNullOrEmpty(queModel.Time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is required"
                };
            }
            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            var queEntity = new QuesRcSentenceMedia();

            if (queModel.Audio != null)
            {
                var audioFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
                var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Audio.FileName)}";

                result = await UploadHelper.UploadFileAsync(queModel.Audio, audioFolderPath, fileAudio);
                if (!string.IsNullOrEmpty(result))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = result,
                        Success = false
                    };
                }

                var durationAudio = await VideoHelper.GetDurationAsync(Path.Combine(audioFolderPath, fileAudio));
                queEntity.Audio = Path.Combine(_audioBasePath, fileAudio);
            }

            queEntity.Question = queModel.Question;
            queEntity.AnswerA = queModel.AnswerA;
            queEntity.AnswerB = queModel.AnswerB;
            queEntity.AnswerC = queModel.AnswerC;
            queEntity.AnswerD = queModel.AnswerD;
            queEntity.Time = timeOnly;
            queEntity.Image = Path.Combine(_imageBasePath, fileImage);

            if (queModel.AnswerId != null)
            {
                var answerModel = await _unit.AnswerRcMedia
                                            .Include(a => a.QuesRcSentenceMedia)
                                            .FirstOrDefaultAsync(a => a.AnswerId == queModel.AnswerId);
                if (answerModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any answers",
                        Success = false
                    };
                }

                if (answerModel.QuesRcSentenceMedia != null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "This answer is from another question",
                        Success = false
                    };
                }

                queEntity.AnswerId = queModel.AnswerId;
            }
            else
            {
                if (queModel.Answer != null)
                {
                    var answerModel = _mapper.Map<AnswerRcSentenceMedia>(queModel.Answer);
                    _unit.AnswerRcMedia.Add(answerModel);

                    queEntity.Answer = answerModel;
                }
            }

            _unit.QuesRcSenMedia.Add(queEntity);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long quesId)
        {
            var quesModel = _unit.QuesRcSenMedia.GetById(quesId);
            if (quesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Image);
            var audioPath = Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio ?? "");

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            if (File.Exists(audioPath))
            {
                File.Delete(audioPath);
            }

            if (quesModel.AnswerId.HasValue)
            {
                var answerModel = _unit.AnswerRcMedia.GetById((long)quesModel.AnswerId);
                _unit.AnswerRcMedia.Remove(answerModel);
            }

            _unit.QuesRcSenMedia.Remove(quesModel);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAllAsync()
        {
            var queModels = await _unit.QuesRcSenMedia
                                    .Include(q => q.Answer)
                                    .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcSenMediaResDto>>(queModels),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long quesId)
        {
            var queModel = await _unit.QuesRcSenMedia
                                    .Include(q => q.Answer)
                                    .FirstOrDefaultAsync(q => q.QuesId == quesId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesRcSenMediaResDto>(queModel),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long quesId, QuesRcSenMediaDto queModel)
        {
            var queEntity = _unit.QuesRcSenMedia.GetById(quesId);
            if (queEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            if (string.IsNullOrEmpty(queModel.Time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is required"
                };
            }
            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (queEntity.Question != queModel.Question)
                {
                    var response = await ChangeQuestionAsync(quesId, queModel.Question);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerA != queModel.AnswerA)
                {
                    var response = await ChangeAnswerAAsync(quesId, queModel.AnswerA);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerB != queModel.AnswerB)
                {
                    var response = await ChangeAnswerBAsync(quesId, queModel.AnswerB);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerC != queModel.AnswerC)
                {
                    var response = await ChangeAnswerCAsync(quesId, queModel.AnswerC);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerD != queModel.AnswerD)
                {
                    var response = await ChangeAnswerDAsync(quesId, queModel.AnswerD);
                    if (!response.Success) return response;
                }

                if (queModel.AnswerId.HasValue)
                {
                    var response = await ChangeAnswerAsync(quesId, (long)queModel.AnswerId);
                    if (!response.Success) return response;
                }

                if(queModel.Audio != null)
                {
                    var response = await ChangeAudioAsync(quesId, queModel.Audio);
                    if (!response.Success) return response;
                }

                var changeRes = await ChangeTimeAsync(quesId, timeOnly);
                if (!changeRes.Success) return changeRes;

                changeRes = await ChangeImageAsync(quesId, queModel.Image);
                if (!changeRes.Success) return changeRes;

                await _unit.CommitTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception err)
            {
                await _unit.RollBackTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = err.Message,
                    Success = false
                };
            }
        }
    }
}
