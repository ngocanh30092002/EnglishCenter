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
    public class QuesLcImageService : IQuesLcImageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private string _imageBasePath;
        private string _audioBasePath;

        public QuesLcImageService(IUnitOfWork unit, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _imageBasePath = Path.Combine("questions", "lc_image", "image");
            _audioBasePath = Path.Combine("questions", "lc_image", "audio");
        }

        public async Task<Response> ChangeAnswerAsync(long quesId, long answerId)
        {
            var queModel = _unit.QuesLcImage.GetById(quesId);
            if(queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isExistAnswer = _unit.AnswerLcImage.IsExist(a => a.AnswerId == answerId);
            if (!isExistAnswer)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesLcImage.ChangeAnswerAsync(queModel, answerId);
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

        public async Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile)
        {
            var queModel = _unit.QuesLcImage.GetById(quesId);
            if(queModel == null)
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

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Audio);
            if (File.Exists(previousPath))
            {
                File.Delete(previousPath);
            }

            var isChangeSuccess = await _unit.QuesLcImage.ChangeAudioAsync(queModel, Path.Combine(_audioBasePath, fileAudio));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file audio",
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
            var queModel = _unit.QuesLcImage.GetById(quesId);
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

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image);
            if (File.Exists(previousPath))
            {
                File.Delete(previousPath);
            }

            var isChangeSuccess = await _unit.QuesLcImage.ChangeImageAsync(queModel, Path.Combine(_imageBasePath, fileImage));
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change file image",
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

        public async Task<Response> CreateAsync(QuesLcImageDto queModel)
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


            var queEntity = new QuesLcImage();
            queEntity.Image = Path.Combine(_imageBasePath, fileImage);
            queEntity.Audio = Path.Combine(_audioBasePath, fileAudio);

            if(queModel.AnswerId != null)
            {
                var isExistAnswer = _unit.AnswerLcImage.IsExist(a => a.AnswerId == queModel.AnswerId);
                if(!isExistAnswer)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any answers",
                        Success = false
                    };
                }

                queEntity.AnswerId = queModel.AnswerId;
            }
            else
            {
                if(queModel.Answer != null)
                {
                    var answerModel = _mapper.Map<AnswerLcImage>(queModel.Answer);
                    _unit.AnswerLcImage.Add(answerModel);

                    queEntity.Answer = answerModel;
                }
            }

            _unit.QuesLcImage.Add(queEntity);
            
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
            var quesModel = _unit.QuesLcImage.GetById(quesId);
            if(quesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Image);
            var audioPath = Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio);

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
                var answerModel = _unit.AnswerLcImage.GetById((long) quesModel.AnswerId);
                _unit.AnswerLcImage.Remove(answerModel);
            }

            _unit.QuesLcImage.Remove(quesModel);
            
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
            var queModels = await _unit.QuesLcImage
                                 .Include(q => q.Answer)
                                 .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesLcImageResDto>>(queModels),
                Success = true
            };
        }

        public Task<Response> GetAsync(long quesId)
        {
            var queModel = _unit.QuesLcImage
                                .Include(q => q.Answer)
                                .FirstOrDefault(q => q.QuesId == quesId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesLcImageResDto>(queModel),
                Success = true
            });


        }

        public async Task<Response> UpdateAsync(long quesId, QuesLcImageDto queModel)
        {
            var queEntity = _unit.QuesLcImage.GetById(quesId);
            if(queEntity == null)
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
                if (queModel.Image != null)
                {
                    var response = await ChangeImageAsync(quesId, queModel.Image);
                    if (!response.Success) return response;
                }

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


                await _unit.CommitTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch(Exception err)
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
