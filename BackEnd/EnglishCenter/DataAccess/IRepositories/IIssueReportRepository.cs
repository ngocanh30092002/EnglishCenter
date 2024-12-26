using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IIssueReportRepository : IGenericRepository<IssueReport>
    {
        public Task<bool> ChangeTitleAsync(IssueReport issueReport, string newTitle);
        public Task<bool> ChangeDesAsync(IssueReport issueReport, string newDes);
        public Task<bool> ChangeTypeAsync(IssueReport issueReport, int type);
        public Task<bool> ChangeStatusAsync(IssueReport issueReport, int status);
    }
}
