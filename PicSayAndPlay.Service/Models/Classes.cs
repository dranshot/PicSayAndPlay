using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicSayAndPlay.Service.Models
{
    public class Country
    {
        public string Name { get; set; }
    }

    public class Level
    {
        public string Value { get; set; }
        public int PointsRequired { get; set; }
    }

    public class Puntuation
    {
        public int NumberOfChars { get; set; }
        public int Points { get; set; }
    }

    public class Query
    {
        public User User { get; set; }
        public DateTime Date { get; set; }
        public string OriginalWord { get; set; }
        public string TranslatedWord { get; set; }
        public string ImageUrl { get; set; }
        public Puntuation Puntuation { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public Level Level { get; set; }
        public Country Country { get; set; }
    }
}