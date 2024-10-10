using Xabe.FFmpeg;

namespace EnglishCenter.Presentation.Helpers
{
    public static class VideoHelper
    {
        private static string? _ffmpegPath;
        public static void Initialize(IConfiguration configuration)
        {
            _ffmpegPath = configuration["FFMPEG:Path"];
        }

        public static async Task<TimeSpan> GetDurationAsync(string audioPath)
        {
            FFmpeg.SetExecutablesPath(_ffmpegPath);
            var mediaInfo = await FFmpeg.GetMediaInfo(audioPath);
            
            return mediaInfo.Duration;
        }
    }
}
