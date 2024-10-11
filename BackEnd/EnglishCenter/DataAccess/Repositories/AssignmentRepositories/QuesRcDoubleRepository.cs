using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesRcDoubleRepository : GenericRepository<QuesRcDouble>, IQuesRcDoubleRepository
    {
        public QuesRcDoubleRepository(EnglishCenterContext context) : base(context)
        {

        }

        public override QuesRcDouble GetById(long id)
        {
            var model = context.QuesRcDoubles
                            .Include(s => s.SubRcDoubles)
                            .ThenInclude(s => s.Answer)
                            .FirstOrDefault(q => q.QuesId == id);

            return model;
        }

        public override IEnumerable<QuesRcDouble> GetAll()
        {
            var models = context.QuesRcDoubles
                                .Include(s => s.SubRcDoubles)
                                .ThenInclude(s => s.Answer)
                                .ToList();

            return models;
        }

        public Task<bool> ChangeImage1Async(QuesRcDouble model, string imageUrl)
        {
            if(model == null) return Task.FromResult(false);

            model.Image1 = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage2Async(QuesRcDouble model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image2 = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantityAsync(QuesRcDouble model, int quantity)
        {
            if (model == null) return Task.FromResult(false);

            model.Quantity = quantity;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(QuesRcDouble model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;

            return Task.FromResult(true);

        }
    }
}
