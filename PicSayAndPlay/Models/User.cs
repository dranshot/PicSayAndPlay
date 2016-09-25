using System;

namespace PicSayAndPlay.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Classification Classification { get; set; }
    }
}