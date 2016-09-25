using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.Models
{
    public class Translation
    {
        public string OriginalWord { get; }
        public string TranslatedWord { get; }
        public string ImageSource { get; }
        public DateTime QueryDate { get; set; }

        public Translation(string OriginalWord, string TranslatedWord, string ImageSource, DateTime QueryDate)
        {
            this.OriginalWord = OriginalWord;
            this.TranslatedWord = TranslatedWord;
            this.ImageSource = ImageSource;
            this.QueryDate = QueryDate;
        }

    }
}
