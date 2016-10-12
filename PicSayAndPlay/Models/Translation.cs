using System;

namespace PicSayAndPlay.Models
{
    public class Translation
    {
        public string OriginalWord { get; }
        public string TranslatedWord { get; }
        public string ImageSource { get; set; }
        public DateTime QueryDate { get; }
        public bool IsWellPronounced { get; set; }

        public Translation(string OriginalWord, string TranslatedWord, DateTime QueryDate, string ImageSource = "")
        {
            this.OriginalWord = OriginalWord;
            this.TranslatedWord = TranslatedWord;
            this.ImageSource = ImageSource;
            this.QueryDate = QueryDate;
        }
    }
}