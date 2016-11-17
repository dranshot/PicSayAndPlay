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
        public static List<RegisterValidation> CheckInputs(UserToRegister user)
        {
            var list = new List<RegisterValidation>();
            if (String.IsNullOrEmpty(user.FirstName))
                list.Add(new RegisterValidation() { Type = RegisterField.FirstName, ErrorMessage = "Ingresa tu nombre" });
            if (String.IsNullOrEmpty(user.LastName))
                list.Add(new RegisterValidation() { Type = RegisterField.LastName, ErrorMessage = "Ingresa tu apellido" });
            if (String.IsNullOrEmpty(user.NickName))
                list.Add(new RegisterValidation() { Type = RegisterField.NickName, ErrorMessage = "Ingresa un nombre de usuario" });
            if (String.IsNullOrEmpty(user.Email))
                list.Add(new RegisterValidation() { Type = RegisterField.Email, ErrorMessage = "Ingresa un email" });
            if (String.IsNullOrEmpty(user.Password))
                list.Add(new RegisterValidation() { Type = RegisterField.Password, ErrorMessage = "Ingresa una contraseña" });
            if (String.IsNullOrEmpty(user.Country))
                list.Add(new RegisterValidation() { Type = RegisterField.Country, ErrorMessage = "Selecciona un país" });
            if (user.Birthday == null)
                list.Add(new RegisterValidation() { Type = RegisterField.Birthday, ErrorMessage = "Ingresa una fecha válida" });
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
