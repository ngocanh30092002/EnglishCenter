using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    //public class StuInClassRepository : GenericRepository<StuInClass>, IStuInClassRepository
    //{
    //    public StuInClassRepository(EnglishCenterContext context) : base(context)
    //    {

    //    }

    //    public async Task<bool> ChangeClassAsync(StuInClass model, string classId)
    //    {
    //        if (model == null) return false;

    //        var isExistClass = await context.Classes.AnyAsync(c => c.ClassId == classId);
    //        if (!isExistClass) return false;

    //        model.ClassId = classId;
    //        return true;
    //    }

    //    public async Task<bool> ChangeScoreHisAsync(StuInClass model, long scoreId)
    //    {
    //        if (model == null) return false;

    //        var isExistClass = await context.ScoreHistories.AnyAsync(s => s.ScoreHisId == scoreId);
    //        if (!isExistClass) return false;

    //        model.ScoreHisId = scoreId;
    //        return true;
    //    }

    //    public Task<bool> ChangeStatusAsync(StuInClass model, StuInClassEnum status)
    //    {
    //        if (model == null) return Task.FromResult(false);

    //        model.Status = (int)status;

    //        return Task.FromResult(true);
    //    }

    //    public async Task<bool> ChangeUserIdAsync(StuInClass model, string userId)
    //    {
    //        if (model == null) return false;

    //        var isExistUser = await context.Students.FindAsync(userId);
    //        if (isExistUser == null) return false;

    //        string courseId = string.Empty;
    //        if (model.Class == null)
    //        {
    //            var classModel = await context.Classes.FindAsync(model.ClassId);

    //            courseId = classModel!.CourseId;
    //        }
    //        else
    //        {
    //            courseId = model.Class.CourseId;
    //        }

    //        var isStillLearning = context.StuInClasses.Include(s => s.Class)
    //                                                .Where(s => s.UserId == userId && 
    //                                                            s.Status != (int)StuInClassEnum.Completed &&
    //                                                            s.Class!.CourseId == courseId)
    //                                                .Any();

    //        if (isStillLearning) return false;

    //        model.UserId = userId;
    //        return true;
    //    }

    //    public async Task<Response> UpdateAsync(long stuId, StuInClassDto model)
    //    {
    //        var stuInClassModel = await context.StuInClasses.FindAsync(stuId);
    //        if(stuInClassModel == null)
    //        {
    //            return new Response()
    //            {
    //                StatusCode = System.Net.HttpStatusCode.BadRequest,
    //                Message = "Can't find any StuInClass",
    //                Success = false
    //            };
    //        }

    //        var isChangeSuccess = await this.ChangeClassAsync(stuInClassModel, model.ClassId ?? "");
    //        if (isChangeSuccess)
    //        {
    //            return new Response()
    //            {
    //                StatusCode = System.Net.HttpStatusCode.BadRequest,
    //                Message = "Can't change class",
    //                Success = false
    //            };
    //        }

    //        isChangeSuccess = await this.ChangeUserIdAsync(stuInClassModel, model.UserId ?? "");
    //        if (isChangeSuccess)
    //        {
    //            return new Response()
    //            {
    //                StatusCode = System.Net.HttpStatusCode.BadRequest,
    //                Message = "Can't change user",
    //                Success = false
    //            };
    //        }

    //        isChangeSuccess = await this.ChangeScoreHisAsync(stuInClassModel, model.ScoreHisId ?? 0);
    //        if (isChangeSuccess)
    //        {
    //            return new Response()
    //            {
    //                StatusCode = System.Net.HttpStatusCode.BadRequest,
    //                Message = "Can't change score history",
    //                Success = false
    //            };
    //        }

    //        isChangeSuccess = await this.ChangeStatusAsync(stuInClassModel, (StuInClassEnum) model.Status);
    //        if (isChangeSuccess)
    //        {
    //            return new Response()
    //            {
    //                StatusCode = System.Net.HttpStatusCode.BadRequest,
    //                Message = "Can't change status",
    //                Success = false
    //            };
    //        }

    //        return new Response()
    //        {
    //            StatusCode = System.Net.HttpStatusCode.OK,
    //            Message = "",
    //            Success = true
    //        };
    //    }
    //}
}
