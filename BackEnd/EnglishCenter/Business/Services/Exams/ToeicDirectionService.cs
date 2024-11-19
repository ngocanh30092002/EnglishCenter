using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Exams
{
    public class ToeicDirectionService : IToeicDirectionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _basePath;

        public ToeicDirectionService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _basePath = Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction");
        }

        public async Task<Response> ChangeAudioAsync(long id, IFormFile audioFile, int part)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            var audioFileName = $"audio_{DateTime.Now.Ticks}{Path.GetExtension(audioFile.FileName)}";
            var result = await UploadHelper.UploadFileAsync(audioFile, _basePath, audioFileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var isChangeSuccess = false;

            switch (part)
            {
                case 1:
                    isChangeSuccess = await _unit.ToeicDirections.ChangeAudio1Async(toeicModel, Path.Combine("toeic_direction", audioFileName));
                    break;
                case 2:
                    isChangeSuccess = await _unit.ToeicDirections.ChangeAudio2Async(toeicModel, Path.Combine("toeic_direction", audioFileName));
                    break;
                case 3:
                    isChangeSuccess = await _unit.ToeicDirections.ChangeAudio3Async(toeicModel, Path.Combine("toeic_direction", audioFileName));
                    break;
                case 4:
                    isChangeSuccess = await _unit.ToeicDirections.ChangeAudio4Async(toeicModel, Path.Combine("toeic_direction", audioFileName));
                    break;
            }


            if (!isChangeSuccess)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change audio failed",
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

        public async Task<Response> ChangeImageAsync(long id, IFormFile imageFile)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            var imageFileName = $"image_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
            var result = await UploadHelper.UploadFileAsync(imageFile, _basePath, imageFileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicDirections.ChangeImageAsync(toeicModel, Path.Combine("toeic_direction", imageFileName));
            if (!isChangeSuccess)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change image failed",
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

        public async Task<Response> ChangeIntroduceListeningAsync(long id, string introduce)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicDirections.ChangeIntroduceListeningAsync(toeicModel, introduce);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change introduce failed",
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

        public async Task<Response> ChangeIntroduceReadingAsync(long id, string introduce)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicDirections.ChangeIntroduceReadingAsync(toeicModel, introduce);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change introduce failed",
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

        public async Task<Response> ChangePartAsync(long id, string value, int part)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            var isChangeSuccess = false;

            switch (part)
            {
                case 1:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart1Async(toeicModel, value);
                    break;
                case 2:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart2Async(toeicModel, value);
                    break;
                case 3:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart3Async(toeicModel, value);
                    break;
                case 4:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart4Async(toeicModel, value);
                    break;
                case 5:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart5Async(toeicModel, value);
                    break;
                case 6:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart6Async(toeicModel, value);
                    break;
                case 7:
                    isChangeSuccess = await _unit.ToeicDirections.ChangePart7Async(toeicModel, value);
                    break;
            }


            if (!isChangeSuccess)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change part failed",
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

        public async Task<Response> CreateAsync(ToeicDirectionDto model)
        {

            await _unit.BeginTransAsync();
            try
            {
                var entityModel = _mapper.Map<ToeicDirection>(model);
                _unit.ToeicDirections.Add(entityModel);
                await _unit.CompleteAsync();


                var response = await ChangeAudioAsync(entityModel.Id, model.Audio1, 1);
                if (!response.Success) return response;

                response = await ChangeAudioAsync(entityModel.Id, model.Audio2, 2);
                if (!response.Success) return response;

                response = await ChangeAudioAsync(entityModel.Id, model.Audio3, 3);
                if (!response.Success) return response;

                response = await ChangeAudioAsync(entityModel.Id, model.Audio4, 4);
                if (!response.Success) return response;

                response = await ChangeImageAsync(entityModel.Id, model.imageFile);
                if (!response.Success) return response;

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

        public async Task<Response> DeleteAsync(long id)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction", toeicModel.Image ?? ""));
            File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction", toeicModel.Audio1 ?? ""));
            File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction", toeicModel.Audio2 ?? ""));
            File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction", toeicModel.Audio3 ?? ""));
            File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "toeic_direction", toeicModel.Audio4 ?? ""));

            _unit.ToeicDirections.Remove(toeicModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetAsync(long id)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicDirectionResDto>(toeicModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, ToeicDirectionDto model)
        {
            var toeicModel = _unit.ToeicDirections.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic directions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.imageFile != null)
                {
                    var response = await ChangeImageAsync(toeicModel.Id, model.imageFile);
                    if (!response.Success) return response;
                }

                if (model.Audio1 != null)
                {
                    var response = await ChangeAudioAsync(toeicModel.Id, model.Audio1, 1);
                    if (!response.Success) return response;
                }

                if (model.Audio2 != null)
                {
                    var response = await ChangeAudioAsync(toeicModel.Id, model.Audio2, 2);
                    if (!response.Success) return response;
                }

                if (model.Audio3 != null)
                {
                    var response = await ChangeAudioAsync(toeicModel.Id, model.Audio3, 3);
                    if (!response.Success) return response;
                }

                if (model.Audio4 != null)
                {
                    var response = await ChangeAudioAsync(toeicModel.Id, model.Audio4, 4);
                    if (!response.Success) return response;
                }

                var responsePart = await ChangePartAsync(toeicModel.Id, model.Part_1, 1);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_2, 2);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_3, 3);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_4, 4);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_5, 5);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_6, 6);
                if (!responsePart.Success) return responsePart;

                responsePart = await ChangePartAsync(toeicModel.Id, model.Part_7, 7);
                if (!responsePart.Success) return responsePart;

                var responseIntroduce = await ChangeIntroduceListeningAsync(toeicModel.Id, model.Introduce_Listening);
                if (!responseIntroduce.Success) return responsePart;

                responseIntroduce = await ChangeIntroduceReadingAsync(toeicModel.Id, model.Introduce_Reading);
                if (!responseIntroduce.Success) return responsePart;

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
