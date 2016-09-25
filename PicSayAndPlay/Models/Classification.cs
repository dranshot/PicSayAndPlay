using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.Models
{
    public class Classification
    {
        public Country Country { get; set; }
        public List<Translation> AnsweredWords { get; set; } = new List<Translation>();
        public int PointsEarned { get; set; }
    }

}
