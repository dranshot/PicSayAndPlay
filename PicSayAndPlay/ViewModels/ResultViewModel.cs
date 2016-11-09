using PicSayAndPlay.Models;
using PicSayAndPlay.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PicSayAndPlay.ViewModels
{
    public class ResultViewModel
    {
        public async Task<List<Translation>> GetWordsToShow(Stream fileStream)
        {
            var result = await ComputerVisionService.Client.GetTagsAsync(fileStream);
            var translations = await TranslationService.TranslateAsync(result);
            return translations;
        }
    }
}