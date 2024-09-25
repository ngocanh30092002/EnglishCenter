using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class EnrollStatusRepository : GenericRepository<EnrollStatus> ,IEnrollStatusRepository
    {
        public EnrollStatusRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<bool> UpdateAsync(int enrollStatusId, EnrollStatus model)
        {
            var enrollStatus = await context.EnrollStatuses.FindAsync(enrollStatusId);

            if (enrollStatus == null) return false;

            enrollStatus.Name = model.Name;

            return true;
        }
    }
}
