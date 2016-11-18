using System;

namespace PicSayAndPlay.ViewModels
{
    public enum Validation
    {
        Correct,
        UsernameEmpty,
        PasswordEmpty,
        AllEmpty
    }

    public class LoginViewModel
    {
        public static Validation CheckInputs(string username, string password)
        {
            var userValid = !String.IsNullOrEmpty(username);
            var passValid = !String.IsNullOrEmpty(password);

            if (userValid)
            {
                if (passValid)
                    return Validation.Correct;
                else
                    return Validation.PasswordEmpty;
            }
            else
            {
                if (passValid)
                    return Validation.UsernameEmpty;
                else
                    return Validation.AllEmpty;
            }
        }
    }
}