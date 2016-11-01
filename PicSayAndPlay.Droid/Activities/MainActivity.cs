using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
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
            Plugin.Media.Abstractions.MediaFile file = null;
            await CrossMedia.Current.Initialize();

            if (sender.Equals(takePhotoBtn))
            {
                /*  Take photo clicked */

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    /* TODO: Change to snackbar*/
                    Toast.MakeText(this.ApplicationContext, "La cámara no está disponible :(", ToastLength.Long).Show();
                    return;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    SaveToAlbum = false
                });
            }
            else
            {
                /* Pick foto clicked */

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    /* TODO: Change to snackbar*/
                    Toast.MakeText(this.ApplicationContext, "No puedo ir a la galería :(", ToastLength.Long).Show();
                    return;
                }

                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file == null)
                return;

            NavigateToResult(file);
        }

        private void NavigateToResult(Plugin.Media.Abstractions.MediaFile file)
        {
            Intent i = new Intent(this, typeof(ResultActivity));
            i.PutExtra("Image", file.Path);
            StartActivity(i);
        }
    }
}