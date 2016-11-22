using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using PicSayAndPlay.ViewModels;
using Plugin.Media;
using System;
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.Design.Widget;
using BlinkXamarinProject.AndroidApp.Helpers;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "Pic, Say & Play", MainLauncher = false,
        Icon = "@drawable/icon", Theme = "@style/Base.Theme.Design.Main")]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout drawer;
        private Button takePhotoBtn;
        private Button pickPhotoBtn;
        private TextView usernameTvw;
        private string nickname;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            SetUpToolbar();

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            takePhotoBtn = FindViewById<Button>(Resource.Id.takePic);
            pickPhotoBtn = FindViewById<Button>(Resource.Id.pickPic);
            usernameTvw = FindViewById<TextView>(Resource.Id.usernameTvw);
            NavigationView navView = FindViewById<NavigationView>(Resource.Id.nav_view);


            if (navView != null)
                SetUpDrawerContent(navView);

            SetUserInfo();

            //  EventHandlers
            takePhotoBtn.Click += GetPhotoToTranslateAsync;
            pickPhotoBtn.Click += GetPhotoToTranslateAsync;
        }

        private void SetUpToolbar()
        {
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ab.SetDisplayHomeAsUpEnabled(true);
            ab.SetDisplayShowTitleEnabled(false);
        }

        private void SetUpDrawerContent(NavigationView navView)
        {
            navView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_exit_to_app:
                        var session = new UserSessionManager(this);
                        session.LogoutUser();
                        this.Finish();
                        break;
                }

                drawer.CloseDrawers();
            };
            navView.SetCheckedItem(Resource.Id.nav_home);
        }

        private void SetUserInfo()
        {
            var session = new UserSessionManager(this);
            var userDetails = session.GetUserDetails();

            userDetails.TryGetValue(UserSessionManager.KEY_NICKNAME, out nickname);
            usernameTvw.Text = nickname;
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawer.OpenDrawer((int)Android.Views.GravityFlags.Left);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private async void GetPhotoToTranslateAsync(object sender, EventArgs e)
        {
            PictureResult result;
            await CrossMedia.Current.Initialize();

            if (sender.Equals(takePhotoBtn))
                result = await MainViewModel.TakePhotoAsync();
            else
                result = await MainViewModel.PickPhotoAsync();

            switch (result.State)
            {
                case PhotoResult.CameraNotAvailable:
                    Toast.MakeText(this.ApplicationContext, "Cámara no disponible", ToastLength.Short).Show();
                    return;
                case PhotoResult.PickNotAvailable:
                    Toast.MakeText(this.ApplicationContext, "Galería no disponible", ToastLength.Short).Show();
                    return;
                case PhotoResult.Canceled:
                    return;
            }

            NavigateToResult(result.Picture);
        }

        private void NavigateToResult(Plugin.Media.Abstractions.MediaFile file)
        {
            Intent i = new Intent(this, typeof(ResultActivity));
            i.PutExtra("Image", file.Path);
            StartActivity(i);
        }
    }
}