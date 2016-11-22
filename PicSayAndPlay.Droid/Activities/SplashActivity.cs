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
using BlinkXamarinProject.AndroidApp.Helpers;

namespace PicSayAndPlay.Droid.Activities
{
    [Activity(Label = "Pic, Say & Play",Theme = "@style/Base.Theme.Design.Splash", NoHistory =true,MainLauncher =true)]
    public class SplashActivity : AppCompatActivity
    {
        UserSessionManager session;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            session = new UserSessionManager(this);

            if (!session.IsUserLoggedIn)
                StartActivity(typeof(LoginActivity));
            else
                StartActivity(typeof(MainActivity));

        }
    }
}