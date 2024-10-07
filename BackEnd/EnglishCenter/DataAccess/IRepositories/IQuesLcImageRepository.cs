﻿using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesLcImageRepository : IGenericRepository<QuesLcImage>
    {
        Task<bool> ChangeImageAsync(QuesLcImage queModel, string imagePath);
        Task<bool> ChangeAudioAsync(QuesLcImage queModel, string audioPath);
        Task<bool> ChangeAnswerAsync(QuesLcImage queModel, long answerId);
    }
}
