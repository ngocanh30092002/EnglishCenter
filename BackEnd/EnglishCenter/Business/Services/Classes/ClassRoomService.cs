using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Classes
{
    public class ClassRoomService : IClassRoomService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ClassRoomService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<Response> ChangeCapacityAsync(long id, int capacity)
        {
            var roomModel = _unit.ClassRooms.GetById(id);
            if (roomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            if (capacity < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Capacity must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassRooms.ChangeCapacityAsync(roomModel, capacity);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change capacity failed",
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

        public async Task<Response> ChangeLocationAsync(long id, string location)
        {
            var roomModel = _unit.ClassRooms.GetById(id);
            if (roomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassRooms.ChangeLocationAsync(roomModel, location);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change location failed",
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

        public async Task<Response> ChangeNameAsync(long id, string newName)
        {
            var roomModel = _unit.ClassRooms.GetById(id);
            if (roomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassRooms.ChangeNameAsync(roomModel, newName);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change name failed",
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

        public async Task<Response> CreateAsync(ClassRoomDto roomModel)
        {
            if (roomModel.Capacity < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Capacity must be greater than 0",
                    Success = false
                };
            }

            var roomEntity = _mapper.Map<ClassRoom>(roomModel);
            _unit.ClassRooms.Add(roomEntity);

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
            var roomModel = _unit.ClassRooms.GetById(id);
            if (roomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            _unit.ClassRooms.Remove(roomModel);
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
            var models = _unit.ClassRooms.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassRoomDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.ClassRooms.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ClassRoomDto>(model),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, ClassRoomDto roomModel)
        {
            var roomEntity = _unit.ClassRooms.GetById(id);
            if (roomEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (roomEntity.ClassRoomName != roomModel.ClassRoomName)
                {
                    var res = await ChangeNameAsync(id, roomModel.ClassRoomName);
                    if (!res.Success) return res;
                }

                if (roomEntity.Capacity != roomModel.Capacity)
                {
                    var res = await ChangeCapacityAsync(id, roomModel.Capacity);
                    if (!res.Success) return res;
                }

                if (roomEntity.Location != roomModel.Location)
                {
                    var res = await ChangeLocationAsync(id, roomModel.Location ?? "");
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
