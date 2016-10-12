using PicSayAndPlay.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PicSayAndPlay.Helpers
{
    public class TranslationManager
    {
        private User User { get; }

        public TranslationManager(User user)
        {
            User = user;
        }

        public async Task<bool> HandlePronunciation(string OriginalWord, string TranslatedWord, string UserInput, Stream Image = null)
        {
            if (!CheckPronunciation(TranslatedWord, UserInput))
                return false;

            var ImageUrl = GenerateImageUrl(Image);
            await SaveTranslation(OriginalWord, TranslatedWord, ImageUrl);
            return true;
        }

        #region HelperMethods

        private bool CheckPronunciation(string translatedWord, string userInput)
        {
            if (translatedWord.Equals(userInput))
                return true;
            return false;
        }

        private string GenerateImageUrl(Stream image)
        {
            //  Access to Azure blob storage to save image and return a url
            return "imageUrl";
        }

        private async Task SaveTranslation(string OriginalWord, string TranslatedWord, string ImageUrl)
        {
            var Translation = new Translation(OriginalWord, TranslatedWord, DateTime.Now, ImageUrl);
            //  User.Classification.AnsweredWords?.Add(Translation);
            //  await UsersService.SaveTranslation(User.FirstName, Translation);
        }

        #endregion HelperMethods
    }
}