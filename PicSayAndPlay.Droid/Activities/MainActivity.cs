using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using PicSayAndPlay.ViewModels;
using Plugin.Media;
using System;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "Pic, Say and Play", MainLauncher = false,
        Icon = "@drawable/icon", Theme = "@style/Base.Theme.Design")]
    public class MainActivity : AppCompatActivity
    {
        private Button takePhotoBtn;
        private Button pickPhotoBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            takePhotoBtn = FindViewById<Button>(Resource.Id.takePic);
            pickPhotoBtn = FindViewById<Button>(Resource.Id.pickPic);

            //  EventHandlers
            takePhotoBtn.Click += GetPhotoToTranslate;
            pickPhotoBtn.Click += GetPhotoToTranslate;
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