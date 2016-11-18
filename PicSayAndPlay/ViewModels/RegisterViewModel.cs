using PicSayAndPlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.ViewModels
{
    public class RegisterViewModel
    {
        public static List<KeyValuePair<RegisterField, string>> CheckInputs(UserToRegister user)
        {
            var list = new List<KeyValuePair<RegisterField, string>>();
            list.Add(new KeyValuePair<RegisterField, string>(RegisterField.FirstName, user.FirstName == "" ? "Campo vacío" : ""));
            list.Add(new KeyValuePair<RegisterField, string>(RegisterField.LastName, user.LastName == "" ? "Campo vacío" : ""));
            list.Add(new KeyValuePair<RegisterField, string>(RegisterField.NickName, user.NickName == "" ? "Campo vacío" : ""));
            list.Add(new KeyValuePair<RegisterField, string>(RegisterField.Password, user.Password == "" ? "Campo vacío" : ""));

            return list;
        }
    }

    public class RegisterValidation
    {
        public RegisterField Type { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum RegisterField
    {
        FirstName,
        LastName,
        NickName,
        Email,
        Password,
        Country,
        Birthday
    }
}
