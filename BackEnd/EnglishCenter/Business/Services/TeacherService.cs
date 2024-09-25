using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unit;

        public TeacherService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public Response GetFullName(string userId)
        {
            var teacher = _unit.Students.GetById(userId);

            if(teacher == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers",
                    Success = false,
                };
            }

            var fullName = _unit.Teachers.GetFullName(teacher);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = fullName,
                Success = true,
            };
        }

        //public async Task<Response> AcceptedStudentAsync(long enrollId)
        //{
        //    var enrollmentModel = _unit.Enrollment.GetById(enrollId);
        //    if(enrollmentModel == null)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Can't find any enrollment",
        //            Success = false,
        //        };
        //    }


        //    var isSuccess = await _unit.Enrollment.ChangeStatusAsync(enrollmentModel, Presentation.Global.Enum.EnrollEnum.Accepted);
        //    if(!isSuccess)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Failed to approve students for class",
        //            Success = false,
        //        };
        //    }

        //    var createResponse = await _stuInClassService.CreateAsync(new Presentation.Models.DTOs.StuInClassDto() { ClassId = enrollmentModel.ClassId, UserId = enrollmentModel.UserId });

        //    if (!createResponse.Success)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Failed to approve students for class",
        //            Success = false,
        //        };
        //    }

        //    isSuccess = await _unit.Enrollment.ChangeStatusAsync(enrollmentModel, Presentation.Global.Enum.EnrollEnum.Approved);
        //    if(!isSuccess)
        //    {
        //        return new Response()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.BadRequest,
        //            Message = "Failed to approve students for class",
        //            Success = false,
        //        };
        //    }

        //    await _unit.CompleteAsync();
        //    return new Response()
        //    {
        //        StatusCode = System.Net.HttpStatusCode.OK,
        //        Success = true,
        //    };
        //}
    }
}
