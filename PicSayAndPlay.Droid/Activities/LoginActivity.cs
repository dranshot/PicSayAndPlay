using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "Pic Say & Play", Theme = "@style/Base.Theme.Design.Login", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        private Button loginBtn;
        private EditText usernameTxt;
        private EditText passwordTxt;
        private TextInputLayout userLayout;
        private TextInputLayout passLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            loginBtn = FindViewById<Button>(Resource.Id.LoginBtn);
            usernameTxt = FindViewById<EditText>(Resource.Id.UsernameTxt);
            userLayout = FindViewById<TextInputLayout>(Resource.Id.UsernameTxtLayout);
            passwordTxt = FindViewById<EditText>(Resource.Id.PasswordTxt);
            passLayout = FindViewById<TextInputLayout>(Resource.Id.PasswordTxtLayout);

            loginBtn.Click += LoginBtn_Click;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            /* TODO: Check login */

            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        private bool ValidateInputs()
        {
            var areValidInputs = true;
            string userError = null;

            if (String.IsNullOrEmpty(usernameTxt.Text))
            {
                areValidInputs = false;
                userError = "Debes llenar el campo";
            }
            ShowError(userLayout, userError);

            string passError = null;
            if (String.IsNullOrEmpty(passwordTxt.Text))
            {
                areValidInputs = false;
                passError = "Debes llenar el campo";
            }
            ShowError(passLayout, passError);

            return areValidInputs;
        }

        private void ShowError(TextInputLayout inputLayout, string message)
        {
            inputLayout.Error = message;

            if (String.IsNullOrEmpty(message))
                inputLayout.ErrorEnabled = false;
            else
                inputLayout.ErrorEnabled = true;
        }
    }
}