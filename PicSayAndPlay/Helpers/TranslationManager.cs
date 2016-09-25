using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicSayAndPlay.Models;

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
            AsignPoints();
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
            var Translation = new Translation(OriginalWord, TranslatedWord, ImageUrl, DateTime.Now);
            //  User.Classification.AnsweredWords?.Add(Translation);
            //  await UsersService.SaveTranslation(User.FirstName, Translation);
        }

        private void AsignPoints()
        {
            User.Classification.PointsEarned += 20;
        }

        #endregion HelperMethods
    }
}
