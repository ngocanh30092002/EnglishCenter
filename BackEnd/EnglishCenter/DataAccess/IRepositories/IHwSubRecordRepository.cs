using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IHwSubRecordRepository : IGenericRepository<HwSubRecord>
    {
        public Task<bool> ChangeSubmissionAsync(HwSubRecord hwSubRecord, long hwSubId);
        public Task<bool> ChangeHomeQuesAsync(HwSubRecord hwSubRecord, long homeQueId);
        public Task<bool> ChangeSubAsync(HwSubRecord hwSubRecord, long? subId);
        public Task<bool> ChangeSelectedAnswerAsync(HwSubRecord hwSubRecord, string selectedAnswer);
        public Task<bool> IsExistSubAsync(QuesTypeEnum type, long? subId);
        public Task<int> GetNumCorrectWithPartAsync(long submissionId, int partNum);
        public Task<List<QuesToeic>?> GetQuestionBySubmissionAsync(long submissionId);
        public Task<HomeworkScoreResDto?> GetScoreBySubmissionAsync(long submissionId);

    }
}
