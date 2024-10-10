using System;
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
    public class QuesLcAudioService : IQuesLcAudioService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _audioBasePath;

        public QuesLcAudioService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _audioBasePath = Path.Combine("questions", "lc_audio");
        }

        public async Task<Response> ChangeAnswerAAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if(queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcAudios.ChangeAnswerAAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answerA",
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
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isExistAnswer = _unit.AnswerLcAudios.IsExist(a => a.AnswerId == answerId);
            if (!isExistAnswer)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcAudios.ChangeAnswerAsync(queModel, answerId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answer failed",
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
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcAudios.ChangeAnswerBAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answerB",
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
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcAudios.ChangeAnswerCAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answerC",
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

        public async Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile)
        {
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
            var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(audioFile.FileName)}";
            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio);
            
            var isChangeSuccess = await _unit.QuesLcAudios.ChangeAudioAsync(queModel, Path.Combine(_audioBasePath, fileAudio));
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

            isChangeSuccess = await _unit.QuesLcAudios.ChangeTimeAsync(queModel, TimeOnly.FromTimeSpan(durationAudio));
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

        public async Task<Response> ChangeQuestionAsync(long quesId, string newQues)
        {
            var queModel = _unit.QuesLcAudios.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcAudios.ChangeQuestionAsync(queModel, newQues);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change question",
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

        public async Task<Response> CreateAsync(QuesLcAudioDto queModel)
        {
            if (queModel.Audio == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Image file is required",
                    Success = false
                };
            }

            var audioFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, _audioBasePath);
            var fileAudio = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Audio.FileName)}";

            var result = await UploadHelper.UploadFileAsync(queModel.Audio, audioFolderPath, fileAudio);
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

            var queEntity = _mapper.Map<QuesLcAudio>(queModel);

            queEntity.Audio = Path.Combine(_audioBasePath, fileAudio);
            queEntity.Time = TimeOnly.FromTimeSpan(durationAudio);

            if (queModel.AnswerId != null)
            {
                var answerModel = await _unit.AnswerRcSingles
                                            .Include(a => a.SubRcSingle)
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

                if(answerModel.SubRcSingle != null)
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
                    var answerModel = _mapper.Map<AnswerLcAudio>(queModel.Answer);
                    _unit.AnswerLcAudios.Add(answerModel);

                    queEntity.Answer = answerModel;
                }
            }

            _unit.QuesLcAudios.Add(queEntity);

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
            var quesModel = _unit.QuesLcAudios.GetById(quesId);
            if (quesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var audioPath = Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio);

            if (File.Exists(audioPath))
            {
                File.Delete(audioPath);
            }

            if (quesModel.AnswerId.HasValue)
            {
                var answerModel = _unit.AnswerLcAudios.GetById((long)quesModel.AnswerId);
                _unit.AnswerLcAudios.Remove(answerModel);
            }

            _unit.QuesLcAudios.Remove(quesModel);

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
            var queModels = await _unit.QuesLcAudios
                                 .Include(q => q.Answer)
                                 .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesLcAudioResDto>>(queModels),
                Success = true
            };
        }

        public Task<Response> GetAsync(long quesId)
        {
            var queModel = _unit.QuesLcAudios
                                 .Include(q => q.Answer)
                                 .FirstOrDefault(q => q.QuesId == quesId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesLcAudioResDto>(queModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long quesId, QuesLcAudioDto queModel)
        {
            var queEntity = _unit.QuesLcAudios.GetById(quesId);
            if (queEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (queModel.Audio != null)
                {
                    var response = await ChangeAudioAsync(quesId, queModel.Audio);
                    if (!response.Success) return response;
                }

                if (queModel.AnswerId.HasValue && queEntity.AnswerId != queModel.AnswerId)
                {
                    var response = await ChangeAnswerAsync(quesId, queModel.AnswerId.Value);
                    if (!response.Success) return response;
                }

                if(queModel.Question != queEntity.Question)
                {
                    queEntity.Question = queModel.Question;
                }

                if (queModel.AnswerA != queEntity.AnswerA)
                {
                    queEntity.AnswerA = queModel.AnswerA;
                }

                if (queModel.AnswerB != queEntity.AnswerB)
                {
                    queEntity.AnswerB = queModel.AnswerB;
                }

                if (queModel.AnswerC != queEntity.AnswerC)
                {
                    queEntity.AnswerC = queModel.AnswerC;
                }

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
