﻿using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAssignmentRecordService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByProcessIdAsync(long processId);
        public Task<Response> GetResultAsync(long processId);
        public Task<Response> ChangeProcessAsync(long id, long processId);
        public Task<Response> ChangeAssignQuesAsync(long id, long assignQueId);
        public Task<Response> ChangeSelectedAnswerAsync(long id, string selectedAnswer);
        public Task<Response> ChangeSubAsync(long id, long? subId);
        public Task<Response> CreateAsync(AssignRecordDto model);
        public Task<Response> UpdateAsync(long id, AssignRecordDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
