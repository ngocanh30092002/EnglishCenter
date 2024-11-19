using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class QuesRcSingleService : IQuesRcSingleService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ISubRcSingleService _subService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _imageBasePath;

        public QuesRcSingleService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, ISubRcSingleService subService)
        {
            _unit = unit;
            _mapper = mapper;
            _subService = subService;
            _webHostEnvironment = webHostEnvironment;
            _imageBasePath = Path.Combine("questions", "rc_single", "image");
        }

        public async Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile)
        {
            var queModel = _unit.QuesRcSingles.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath);
            var fileImage = $"image_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";

            var isChangeSuccess = await _unit.QuesRcSingles.ChangeImageAsync(queModel, Path.Combine(_imageBasePath, fileImage));
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

        public async Task<Response> ChangeQuantityAsync(long quesId, int quantity)
        {
            var queModel = _unit.QuesRcSingles.Include(q => q.SubRcSingles).FirstOrDefault(q => q.QuesId == quesId);

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

            if (queModel.SubRcSingles.Count > quantity)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are currently more questions than input values",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSingles.ChangeQuantityAsync(queModel, quantity);
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

        public async Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly)
        {
            var queModel = _unit.QuesRcSingles.GetById(quesId);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSingles.ChangeTimeAsync(queModel, timeOnly);
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

        public async Task<Response> CreateAsync(QuesRcSingleDto queModel)
        {
            var queEntity = new QuesRcSingle();

            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            if (queModel.Image == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Image is required",
                    Success = false
                };
            }

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

            if (queModel.Quantity <= 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Quantity must be greater than 0",
                    Success = false
                };
            }

            queEntity.Time = timeOnly;
            queEntity.Image = Path.Combine(_imageBasePath, fileImage);
            queEntity.Quantity = queModel.Quantity ?? 1;

            _unit.QuesRcSingles.Add(queEntity);
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
            var queModel = _unit.QuesRcSingles.Include(q => q.SubRcSingles).FirstOrDefault(q => q.QuesId == quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousImagePath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image);
            if (File.Exists(previousImagePath))
            {
                File.Delete(previousImagePath);
            }

            var subIds = queModel.SubRcSingles.Select(s => s.SubId).ToList();

            foreach (var subId in subIds)
            {
                await _subService.DeleteAsync(subId);
            }

            _unit.QuesRcSingles.Remove(queModel);
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
            var queModels = _unit.QuesRcSingles.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcSingleResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long quesId)
        {
            var queModel = _unit.QuesRcSingles.GetById(quesId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesRcSingleResDto>(queModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long quesId, QuesRcSingleDto queModel)
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

                if (!string.IsNullOrEmpty(queModel.Time))
                {
                    if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Time is not in correct format"
                        };
                    }

                    var changeResponse = await ChangeTimeAsync(quesId, timeOnly);
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
