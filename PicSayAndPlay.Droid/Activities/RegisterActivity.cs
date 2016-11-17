using Android.App;
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
    public class RegisterActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private UserToRegister user;
        private Button datePickerBtn;
        private Button registerBtn;
        private TextInputLayout firstNameLay;
        private TextInputLayout lastNameLay;
        private TextInputLayout nicknameLay;
        private TextInputLayout emailLay;
        private TextInputLayout passwordLay;
        private Spinner countryPicker;
        private TextInputLayout birthdayPicker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            firstNameLay = FindViewById<TextInputLayout>(Resource.Id.firstnameRegisterLay);
            lastNameLay = FindViewById<TextInputLayout>(Resource.Id.lastnameRegisterLay);
            nicknameLay = FindViewById<TextInputLayout>(Resource.Id.nicknameRegisterLay);
            emailLay = FindViewById<TextInputLayout>(Resource.Id.emailRegisterLay);
            passwordLay = FindViewById<TextInputLayout>(Resource.Id.passwordRegisterLay);
            countryPicker = FindViewById<Spinner>(Resource.Id.countrySpinner);

            birthdayPicker = FindViewById<TextInputLayout>(Resource.Id.datePickerLay);
            registerBtn = FindViewById<Button>(Resource.Id.sendRegisterBtn);

            user = new UserToRegister();

            SetCountryPicker();

            countryPicker.ItemSelected += delegate
            {
                user.Country = countryPicker.SelectedItem.ToString();
            };

            birthdayPicker.EditText.Click += (s, e) =>
            {
                var datePickerDialog = new DatePickerDialog(this, 0, this, 1997, 4, 12);

                //  Don't allow dates after today
                var now = DateTime.Now.Subtract(new DateTime(1970, 1, 1));
                datePickerDialog.DatePicker.MaxDate = (long)now.TotalMilliseconds;

                datePickerDialog.Show();
            };

            registerBtn.Click += async (s, e) =>
            {
                registerBtn.Enabled = false;

                user.FirstName = firstNameLay.EditText.Text;
                user.LastName = lastNameLay.EditText.Text;
                user.NickName = nicknameLay.EditText.Text;
                user.Email = emailLay.EditText.Text;
                user.Password = passwordLay.EditText.Text;
                var validation = ViewModels.RegisterViewModel.CheckInputs(user);

                if (validation.Count == 0)
                {
                    var success = await Services.PicSayAndPlayService.RegisterUser(user);
                    if (success)
                        Snackbar.Make(s as Android.Views.View, "Registro completo", Snackbar.LengthLong).Show();
                    else
                        Snackbar.Make(s as Android.Views.View, "Ups! Hubo un error", Snackbar.LengthLong).Show();
                }
                else
                {
                    ShowErrors(validation);
                }

                registerBtn.Enabled = true;
            };
        }

        private void ShowErrors(List<ViewModels.RegisterValidation> validation)
        {
            foreach (var error in validation)
            {
                switch (error.Type)
                {
                    case ViewModels.RegisterField.FirstName:
                        Helpers.Util.ShowError(firstNameLay, error.ErrorMessage);
                        break;
                    case ViewModels.RegisterField.LastName:
                        Helpers.Util.ShowError(lastNameLay, error.ErrorMessage);
                        break;
                    case ViewModels.RegisterField.NickName:
                        Helpers.Util.ShowError(nicknameLay, error.ErrorMessage);
                        break;
                    case ViewModels.RegisterField.Email:
                        Helpers.Util.ShowError(emailLay, error.ErrorMessage);
                        break;
                    case ViewModels.RegisterField.Password:
                        Helpers.Util.ShowError(passwordLay, error.ErrorMessage);
                        break;
                    case ViewModels.RegisterField.Birthday:
                        Helpers.Util.ShowError(birthdayPicker, error.ErrorMessage);
                        break;
                }
            }
        }

        private void SetCountryPicker()
        {
            countryPicker.Prompt = "Pais";
            var countries = Enum.GetValues(typeof(Country)).Cast<Country>().Select(p => p.ToString()).ToList();
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countries);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            countryPicker.Adapter = adapter;
        }

        #region IOnDateSetListener interface implementation

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            //  +1 because picker is zero based
            birthdayPicker.EditText.Text = $"Cumpleaños: {dayOfMonth}/{monthOfYear + 1}/{year}";
            user.Birthday = new DateTime(year, monthOfYear + 1, dayOfMonth);
        }

        #endregion IOnDateSetListener interface implementation
    }
}