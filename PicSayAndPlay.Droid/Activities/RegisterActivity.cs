using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "RegisterActivity", Theme = "@style/Base.Theme.Design")]
    public class RegisterActivity : Activity, DatePickerDialog.IOnDateSetListener
    {
        private Button datePickerBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            datePickerBtn = FindViewById<Button>(Resource.Id.datepickerBtn);

            datePickerBtn.Click += (s, e) =>
            {
                var datePickerDialog = new DatePickerDialog(this, 0, this, 1997, 5, 12);

                //  Don't allow dates after today
                var now = DateTime.Now.Subtract(new DateTime(1970, 1, 1));
                datePickerDialog.DatePicker.MaxDate = (long)now.TotalMilliseconds;

                datePickerDialog.Show();
            };
        }

        #region IOnDateSetListener interface implementation

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            Console.WriteLine($"Date picked {dayOfMonth}/{monthOfYear}/{year}");
        }

        #endregion IOnDateSetListener interface implementation
    }
}