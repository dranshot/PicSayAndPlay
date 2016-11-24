using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using PicSayAndPlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "RegisterActivity", Theme = "@style/Base.Theme.Design")]
    public class RegisterActivity : Activity
    {
        private UserToRegister user;
        private Button registerBtn;
        private TextInputLayout firstNameLay;
        private TextInputLayout lastNameLay;
        private TextInputLayout nicknameLay;
        private TextInputLayout passwordLay;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            firstNameLay = FindViewById<TextInputLayout>(Resource.Id.firstnameRegisterLay);
            lastNameLay = FindViewById<TextInputLayout>(Resource.Id.lastnameRegisterLay);
            nicknameLay = FindViewById<TextInputLayout>(Resource.Id.nicknameRegisterLay);
            passwordLay = FindViewById<TextInputLayout>(Resource.Id.passwordRegisterLay);
            registerBtn = FindViewById<Button>(Resource.Id.sendRegisterBtn);

            user = new UserToRegister();

            registerBtn.Click += async (s, e) =>
            {
                registerBtn.Enabled = false;

                user.FirstName = firstNameLay.EditText.Text;
                user.LastName = lastNameLay.EditText.Text;
                user.NickName = nicknameLay.EditText.Text;
                user.Password = passwordLay.EditText.Text;
                user.Country = "Peru";
                user.Birthday = new DateTime(1997, 5, 12);
                user.Email = GetRandomString();

                var validation = ViewModels.RegisterViewModel.CheckInputs(user);

                if (!validation.Any(p => p.Value != ""))
                {
                    try
                    {
                        await Services.PicSayAndPlayService.RegisterUser(user);
                        Intent i = new Intent(this, typeof(Activities.WelcomeActivity));
                        i.AddFlags(ActivityFlags.ClearTop);
                        i.SetFlags(ActivityFlags.NewTask);
                        i.PutExtra("userName", $"{user.FirstName.ToUpperInvariant()} {user.LastName.ToUpperInvariant()}");
                        StartActivity(i);
                        this.Finish();
                    }
                    catch
                    {
                        Snackbar.Make(s as Android.Views.View, "Ups! Hubo un error", Snackbar.LengthLong).Show();
                    }
                }
                else
                {
                    ShowErrors(validation);
                }

                registerBtn.Enabled = true;
            };
        }

        private string GetRandomString()
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrst@.";
            var chars = Enumerable.Range(0, 15).Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        private void ShowErrors(List<KeyValuePair<ViewModels.RegisterField, string>> validation)
        {
            foreach (var error in validation)
            {
                switch (error.Key)
                {
                    case ViewModels.RegisterField.FirstName:
                        Helpers.Util.ShowError(firstNameLay, error.Value);
                        break;
                    case ViewModels.RegisterField.LastName:
                        Helpers.Util.ShowError(lastNameLay, error.Value);
                        break;
                    case ViewModels.RegisterField.NickName:
                        Helpers.Util.ShowError(nicknameLay, error.Value);
                        break;
                    case ViewModels.RegisterField.Password:
                        Helpers.Util.ShowError(passwordLay, error.Value);
                        break;
                }
            }
        }
    }
}