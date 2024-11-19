using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesRcSingleRepository : GenericRepository<QuesRcSingle>, IQuesRcSingleRepository
    {
        public QuesRcSingleRepository(EnglishCenterContext context) : base(context)
        {
        }

        public override QuesRcSingle GetById(long id)
        {
            var model = context.QuesRcSingles
                            .Include(s => s.SubRcSingles)
                            .ThenInclude(s => s.Answer)
                            .FirstOrDefault(q => q.QuesId == id);

            return model;
        }

        public override IEnumerable<QuesRcSingle> GetAll()
        {
            var models = context.QuesRcSingles
                                .Include(s => s.SubRcSingles)
                                .ThenInclude(s => s.Answer)
                                .ToList();

            return models;
        }

        public Task<bool> ChangeImageAsync(QuesRcSingle model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantityAsync(QuesRcSingle model, int quantity)
        {
            if (model == null) return Task.FromResult(false);
            if (quantity <= 0) return Task.FromResult(false);
            if (model.SubRcSingles.Count > quantity) return Task.FromResult(false);

            model.Quantity = quantity;

            return Task.FromResult(true); 
        }

        public Task<bool> ChangeTimeAsync(QuesRcSingle model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;

            return Task.FromResult(true);
        }
    }
}
