using System.Text.RegularExpressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeworkRepositories
{
    public class HwSubRecordRepository : GenericRepository<HwSubRecord>, IHwSubRecordRepository
    {
        private readonly IHomeQuesRepository _homeQuesRepo;

        public HwSubRecordRepository(EnglishCenterContext context, IHomeQuesRepository homeQuesRepo) : base(context)
        {
            _homeQuesRepo = homeQuesRepo;
        }

        public async Task<bool> ChangeHomeQuesAsync(HwSubRecord hwSubRecord, long homeQueId)
        {
            if(hwSubRecord == null) return false;

            var homeQueModel = await context.HomeQues.FindAsync(homeQueId);
            if (homeQueModel == null) return false;

            if(hwSubRecord.HwSubQuesId.HasValue)
            {
                var isExistSub = await IsExistSubAsync((QuesTypeEnum)homeQueModel.Type, hwSubRecord.HwSubQuesId.Value);
                if(!isExistSub) return false;
            }

            hwSubRecord.HwQuesId = homeQueId;
            hwSubRecord.IsCorrect = await _homeQuesRepo.IsCorrectAnswerAsync(homeQueModel, hwSubRecord.SelectedAnswer, hwSubRecord.HwSubQuesId);

            return true;
        }

        public async Task<bool> ChangeSelectedAnswerAsync(HwSubRecord hwSubRecord, string selectedAnswer)
        {
            if (hwSubRecord == null) return false;

            string pattern = "^[ABCDabcd]+$";
            if (!Regex.IsMatch(selectedAnswer, pattern)) return false;

            var homeQueModel = await context.HomeQues.FindAsync(hwSubRecord.HwQuesId);
            if (homeQueModel == null) return false;

            hwSubRecord.SelectedAnswer = selectedAnswer.ToUpper();
            hwSubRecord.IsCorrect = await _homeQuesRepo.IsCorrectAnswerAsync(homeQueModel, selectedAnswer, hwSubRecord.HwSubQuesId);
            return true;
        }

        public async Task<bool> ChangeSubAsync(HwSubRecord hwSubRecord, long? subId)
        {
            if (hwSubRecord == null) return false;

            var homeQueModel = await context.HomeQues.FindAsync(hwSubRecord.HwQuesId);
            if (homeQueModel == null) return false;

            var isExist = await IsExistSubAsync((QuesTypeEnum)homeQueModel.Type, subId);
            if(isExist == false) return false;

            hwSubRecord.HwSubQuesId = subId;
            hwSubRecord.IsCorrect = await _homeQuesRepo.IsCorrectAnswerAsync(homeQueModel, hwSubRecord.SelectedAnswer, hwSubRecord.HwSubQuesId);
            
            return true;
        }

        public async Task<bool> ChangeSubmissionAsync(HwSubRecord hwSubRecord, long hwSubId)
        {
            if (hwSubRecord == null) return false;

            var isExistProcess = await context.HwSubmissions.AnyAsync(s => s.SubmissionId == hwSubId);
            if (!isExistProcess) return false;

            hwSubRecord.SubmissionId = hwSubId;

            return true;
        }

        public async Task<bool> IsExistSubAsync(QuesTypeEnum type, long? subId)
        {
            var isExist = false;
            switch (type)
            {
                case QuesTypeEnum.Conversation:
                    isExist = await context.SubLcConversations.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Single:
                    isExist = await context.SubRcSingles.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Double:
                    isExist = await context.SubRcDoubles.AnyAsync(s => s.SubId == subId);
                    break;
                case QuesTypeEnum.Triple:
                    isExist = await context.SubRcTriples.AnyAsync(s => s.SubId == subId);
                    break;
                default:
                    if (subId.HasValue) return false;
                    isExist = true;
                    break;
            }

            return isExist;
        }
    }
}
