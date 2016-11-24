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
using Android.Views.Animations;
using Android.Support.V7.App;

namespace PicSayAndPlay.Droid.Activities
{
    [Activity(Label = "WelcomeActivity", NoHistory = true, Theme = "@style/Base.Theme.Design")]
    public class WelcomeActivity : AppCompatActivity
    {
        TextView welcomeTvw;
        Button continueBtn;
        Animation animationFadeIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Welcome);

            var username = Intent.GetStringExtra("userName");

            welcomeTvw = FindViewById<TextView>(Resource.Id.welcomeTvw);
            continueBtn = FindViewById<Button>(Resource.Id.welcomeContinueBtn);
            welcomeTvw.Text = $"¡Bienvenido, {username}!";

            animationFadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.fade_in);
            animationFadeIn.AnimationEnd += WelcomeTextAnimationEvent;

            //  Start welcome text animation
            welcomeTvw.Visibility = ViewStates.Visible;
            welcomeTvw.StartAnimation(animationFadeIn);
        }

        private void WelcomeTextAnimationEvent(object sender, Animation.AnimationEndEventArgs e)
        {
            //  Detach event not to be reproduced in the next animation
            animationFadeIn.AnimationEnd -= WelcomeTextAnimationEvent;

            //  Wait to reproduce the next animation
            System.Threading.Thread.Sleep(500);

            //  Clear animation
            animationFadeIn.Cancel();

            //  Detach animation to welcome text
            welcomeTvw.Animation = null;

            //  Apply animation to the button
            animationFadeIn.Duration = 800;
            continueBtn.Visibility = ViewStates.Visible;
            continueBtn.Animation = animationFadeIn;
            continueBtn.Animation.StartNow();

            //  Apply event to the button
            continueBtn.Click += delegate
            {
                StartActivity(typeof(LoginActivity));
            };
        }
    }
}