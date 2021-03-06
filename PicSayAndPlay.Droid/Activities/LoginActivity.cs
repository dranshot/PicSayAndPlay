using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using BlinkXamarinProject.AndroidApp.Helpers;
using PicSayAndPlay.Droid.Helpers;
using PicSayAndPlay.ViewModels;
using System;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "Pic Say & Play", Theme = "@style/Base.Theme.Design.Login")]
    public class LoginActivity : AppCompatActivity
    {
        private Button loginBtn;
        private Button registerBtn;
        private EditText usernameTxt;
        private EditText passwordTxt;
        private TextInputLayout userLayout;
        private TextInputLayout passLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            loginBtn = FindViewById<Button>(Resource.Id.LoginBtn);
            registerBtn = FindViewById<Button>(Resource.Id.RegisterBtn);
            usernameTxt = FindViewById<EditText>(Resource.Id.UsernameTxt);
            userLayout = FindViewById<TextInputLayout>(Resource.Id.UsernameTxtLayout);
            passwordTxt = FindViewById<EditText>(Resource.Id.PasswordTxt);
            passLayout = FindViewById<TextInputLayout>(Resource.Id.PasswordTxtLayout);

            loginBtn.Click += LoginBtn_Click;
            registerBtn.Click += delegate { StartActivity(typeof(RegisterActivity)); };
        }

        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            loginBtn.Enabled = registerBtn.Enabled = false;

            var areValidInputs = LoginViewModel.CheckInputs(usernameTxt.Text, passwordTxt.Text);
            ShowErrors(areValidInputs);

            if (areValidInputs != Validation.Correct)
            {
                loginBtn.Enabled = registerBtn.Enabled = true;
                return;
            }

            var userFound = await Services.PicSayAndPlayService.LoginUser(usernameTxt.Text, passwordTxt.Text);
            if (userFound == null)
            {
                Snackbar.Make(sender as Android.Views.View, "Datos incorrectos :(", Snackbar.LengthLong).Show();
                loginBtn.Enabled = registerBtn.Enabled = true;
                CleanInputs();
                loginBtn.RequestFocus();
                return;
            }

            loginBtn.Enabled = registerBtn.Enabled = true;

            var session = new UserSessionManager(this);
            session.CreateUserLoginSession(userFound.NickName, passwordTxt.Text);
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
            this.Finish();
        }

        private void CleanInputs()
        {
            usernameTxt.Text = "";
            passwordTxt.Text = "";
            usernameTxt.ClearFocus();
            passwordTxt.ClearFocus();
        }

        private void ShowErrors(Validation response)
        {
            switch (response)
            {
                case Validation.UsernameEmpty:
                    Util.ShowError(userLayout, "Debes ingresar tu nombre de usuario");
                    Util.ShowError(passLayout, "");
                    break;

                case Validation.PasswordEmpty:
                    Util.ShowError(userLayout, "");
                    Util.ShowError(passLayout, "Debes ingresar tu contraseņa");
                    break;

                case Validation.AllEmpty:
                    Util.ShowError(userLayout, "Debes llenar este campo");
                    Util.ShowError(passLayout, "Debes llenar este campo");
                    break;

                default:
                    Util.ShowError(userLayout, "");
                    Util.ShowError(passLayout, "");
                    break;
            }
        }

    }
}