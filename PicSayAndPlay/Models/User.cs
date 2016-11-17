using System;
using System.Collections.Generic;

namespace PicSayAndPlay.Models
{
    public class User : UserToRegister
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Level { get; set; }
        public int TotalPoints { get; set; }
        public List<Translation> Queries { get; set; }
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