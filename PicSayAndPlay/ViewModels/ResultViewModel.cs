using DevKit.Xamarin.ImageKit;
using PicSayAndPlay.Models;
using PicSayAndPlay.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PicSayAndPlay.ViewModels
{
    public class ResultViewModel
    {
        public async Task<List<Translation>> GetWordsToShow(byte[] originalImage, int actualHeight, int actualWidth)
        {
            var format = DevKit.Xamarin.ImageKit.Abstractions.ImageFormat.JPG;

            var resizedImageArray = await CrossImageResizer.Current.ResizeImageAsync(
                originalImage,
                2000,
                (int)2000 * actualHeight / actualWidth,
                format);

            var result = await ComputerVisionService.Client.GetTagsAsync(new MemoryStream(resizedImageArray));
            var translations = await TranslationService.TranslateAsync(result);
            return translations;
        }
    }
}