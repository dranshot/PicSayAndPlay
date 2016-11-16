using System;
using System.Collections.Generic;

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
        public UserToRegister User { get; set; }
        public DateTime Date { get; set; }
        public string OriginalWord { get; set; }
        public string TranslatedWord { get; set; }
        public string ImageUrl { get; set; }
        public Puntuation Puntuation { get; set; }
    }

    public class UserInformation
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Level { get; set; }
        public string Country { get; set; }
        public int TotalPoints { get; set; }
        public List<Query> Queries { get; set; }
    }

    public class UserToRegister
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
    }
}