using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.Repositories.HomeRepositories
{
    public class IssueReportRepository : GenericRepository<IssueReport>, IIssueReportRepository
    {
        public IssueReportRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeDesAsync(IssueReport issueReport, string newDes)
        {
            if (issueReport == null) return Task.FromResult(false);

            issueReport.Description = newDes;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatusAsync(IssueReport issueReport, int status)
        {
            if (issueReport == null) return Task.FromResult(false);

            if (!Enum.IsDefined(typeof(IssueStatusEnum), status)) return Task.FromResult(false);

            issueReport.Status = status;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTitleAsync(IssueReport issueReport, string newTitle)
        {
            if (issueReport == null) return Task.FromResult(false);

            issueReport.Title = newTitle;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTypeAsync(IssueReport issueReport, int type)
        {
            if (issueReport == null) return Task.FromResult(false);

            if (!Enum.IsDefined(typeof(IssueTypeEnum), type)) return Task.FromResult(false);

            issueReport.Type = type;

            return Task.FromResult(true);
        }
    }
}
