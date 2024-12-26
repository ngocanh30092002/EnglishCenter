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
    public class QuesRcTripleService : IQuesRcTripleService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ISubRcTripleService _subService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imageBasePath1;
        private readonly string _imageBasePath2;
        private readonly string _imageBasePath3;

        public QuesRcTripleService(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment webHostEnvironment, ISubRcTripleService subService)
        {
            _unit = unit;
            _mapper = mapper;
            _subService = subService;
            _webHostEnvironment = webHostEnvironment;
            _imageBasePath1 = Path.Combine("questions", "rc-triple", "image1");
            _imageBasePath2 = Path.Combine("questions", "rc-triple", "image2");
            _imageBasePath3 = Path.Combine("questions", "rc-triple", "image3");
        }
        public async Task<Response> ChangeImage1Async(long quesId, IFormFile imageFile)
        {
            var queModel = _unit.QuesRcTriples.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image1);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath1);
            var fileName = $"image_1_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeImage1Async(queModel, Path.Combine(_imageBasePath1, fileName));
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

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeImage2Async(long quesId, IFormFile imageFile)
        {
            var queModel = _unit.QuesRcTriples.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image2);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath2);
            var fileName = $"image_2_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeImage2Async(queModel, Path.Combine(_imageBasePath2, fileName));
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

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeImage3Async(long quesId, IFormFile imageFile)
        {
            var queModel = _unit.QuesRcTriples.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousPath = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image3);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath3);
            var fileName = $"image_3_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeImage3Async(queModel, Path.Combine(_imageBasePath3, fileName));
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
            var queModel = _unit.QuesRcTriples.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeLevelAsync(queModel, level);
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
            var queModel = _unit.QuesRcTriples.Include(q => q.SubRcTriples).FirstOrDefault(q => q.QuesId == quesId);
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

            if (queModel.SubRcTriples.Count > quantity)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "There are currently more questions than input values",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeQuantityAsync(queModel, quantity);
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
            var queModel = _unit.QuesRcTriples.GetById(quesId);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcTriples.ChangeTimeAsync(queModel, timeOnly);
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

        public async Task<Response> CreateAsync(QuesRcTripleDto queModel)
        {
            if (queModel.Image1 == null || queModel.Image2 == null || queModel.Image3 == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Image is required",
                    Success = false
                };
            }

            var queEntity = new QuesRcTriple();

            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            var folderPath1 = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath1);
            var fileImage1 = $"image_1_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Image1.FileName)}";

            var result = await UploadHelper.UploadFileAsync(queModel.Image1, folderPath1, fileImage1);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var folderPath2 = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath2);
            var fileImage2 = $"image_2_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Image2.FileName)}";

            result = await UploadHelper.UploadFileAsync(queModel.Image2, folderPath2, fileImage2);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            var folderPath3 = Path.Combine(_webHostEnvironment.WebRootPath, _imageBasePath3);
            var fileImage3 = $"image_3_{DateTime.Now.Ticks}{Path.GetExtension(queModel.Image3.FileName)}";

            result = await UploadHelper.UploadFileAsync(queModel.Image3, folderPath3, fileImage3);
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
            queEntity.Image1 = Path.Combine(_imageBasePath1, fileImage1);
            queEntity.Image2 = Path.Combine(_imageBasePath2, fileImage2);
            queEntity.Image3 = Path.Combine(_imageBasePath3, fileImage3);
            queEntity.Quantity = queModel.Quantity ?? 1;
            queEntity.Level = queModel.Level ?? 1;

            try
            {
                _unit.QuesRcTriples.Add(queEntity);
                await _unit.CompleteAsync();

                if (queModel.SubRcTripleDtos != null && queModel.SubRcTripleDtos.Count != 0)
                {
                    foreach (var sub in queModel.SubRcTripleDtos)
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
            var queModel = _unit.QuesRcTriples.Include(q => q.SubRcTriples).FirstOrDefault(q => q.QuesId == quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var previousImagePath1 = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image1);
            if (File.Exists(previousImagePath1))
            {
                File.Delete(previousImagePath1);
            }

            var previousImagePath2 = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image2);
            if (File.Exists(previousImagePath2))
            {
                File.Delete(previousImagePath2);
            }

            var previousImagePath3 = Path.Combine(_webHostEnvironment.WebRootPath, queModel.Image3);
            if (File.Exists(previousImagePath3))
            {
                File.Delete(previousImagePath3);
            }

            var subIds = queModel.SubRcTriples.Select(s => s.SubId).ToList();
            foreach (var subId in subIds)
            {
                await _subService.DeleteAsync(subId);
            }

            _unit.QuesRcTriples.Remove(queModel);
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
            var queModels = _unit.QuesRcTriples.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcTripleResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long quesId)
        {
            var queModel = _unit.QuesRcTriples.GetById(quesId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesRcTripleResDto>(queModel),
                Success = true
            });
        }

        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId)
        {
            var assignQues = _unit.AssignQues
                                  .Find(a => a.AssignmentId == assignmentId && a.Type == (int)QuesTypeEnum.Triple)
                                  .Select(a => a.TripleQuesId)
                                  .ToList();

            var queModels = _unit.QuesRcTriples
                                .GetAll()
                                .Where(q => !assignQues.Contains(q.QuesId))
                                .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcTripleResDto>>(queModels),
                Success = true
            });
        }

        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId)
        {
            var homeQues = _unit.HomeQues
                                .Find(a => a.HomeworkId == homeworkId && a.Type == (int)QuesTypeEnum.Triple)
                                .Select(a => a.TripleQuesId)
                                .ToList();

            var queModels = _unit.QuesRcTriples
                                .GetAll()
                                .Where(q => !homeQues.Contains(q.QuesId))
                                .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcTripleResDto>>(queModels),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long quesId, QuesRcTripleDto queModel)
        {
            await _unit.BeginTransAsync();

            try
            {
                if (queModel.Quantity.HasValue)
                {
                    var changeResponse = await ChangeQuantityAsync(quesId, queModel.Quantity.Value);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Image1 != null)
                {
                    var changeResponse = await ChangeImage1Async(quesId, queModel.Image1);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Image2 != null)
                {
                    var changeResponse = await ChangeImage2Async(quesId, queModel.Image2);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Image3 != null)
                {
                    var changeResponse = await ChangeImage3Async(quesId, queModel.Image3);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queModel.Level.HasValue)
                {
                    var changeResponse = await ChangeLevelAsync(quesId, queModel.Level.Value);
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
