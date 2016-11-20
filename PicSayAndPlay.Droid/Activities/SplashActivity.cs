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

namespace PicSayAndPlay.Droid.Activities
{
    [Activity(Label = "SplashActivity",Theme = "@style/Base.Theme.Design.Splash", NoHistory =true,MainLauncher =true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent i = new Intent(this, typeof(LoginActivity));
            StartActivity(i);            
        }
    }
}