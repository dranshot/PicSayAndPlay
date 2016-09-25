using System.Collections.Generic;

namespace PicSayAndPlay.Models
{
    public class Classification
    {
        public Country Country { get; set; }
        public List<Translation> AnsweredWords { get; set; } = new List<Translation>();
        public int PointsEarned { get; set; }
    }
}