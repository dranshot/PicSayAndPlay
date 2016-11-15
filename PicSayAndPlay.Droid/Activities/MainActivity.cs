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

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "Pic, Say and Play", MainLauncher = false,
        Icon = "@drawable/icon", Theme = "@style/Base.Theme.Design")]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout drawer;
        private Button takePhotoBtn;
        private Button pickPhotoBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ab.SetDisplayHomeAsUpEnabled(true);

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            NavigationView navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            takePhotoBtn = FindViewById<Button>(Resource.Id.takePic);
            pickPhotoBtn = FindViewById<Button>(Resource.Id.pickPic);

            if (navView != null)
                SetUpDrawerContent(navView);

            //  EventHandlers
            takePhotoBtn.Click += GetPhotoToTranslate;
            pickPhotoBtn.Click += GetPhotoToTranslate;
        }

        private void SetUpDrawerContent(NavigationView navView)
        {
            navView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);
                drawer.CloseDrawers();
            };
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

        private async void GetPhotoToTranslate(object sender, EventArgs e)
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