using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using PicSayAndPlay.Models;
using PicSayAndPlay.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using Uri = Android.Net.Uri;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "ResultActivity", Theme = "@style/Base.Theme.Design",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.KeyboardHidden)]
    public class ResultActivity : AppCompatActivity
    {
        private ProgressDialog dialog;
        private ImageView imageView;
        private Uri imageUri;
        private List<Translation> translations;
        private Bitmap bitmap;
        private ResultViewModel vm;
        private ListView resultsListView;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result);

            //  Get picture's path from extra
            imageUri = Uri.Parse(Intent.GetStringExtra("Image"));

            imageView = FindViewById<ImageView>(Resource.Id.AnalyzedImage);
            resultsListView = FindViewById<ListView>(Resource.Id.listView);
            vm = new ResultViewModel();
            
            try
            {
                ShowDialog();
                bitmap = Helpers.BitmapHelper.GetAndRotateBitmap(imageUri.Path);
                bitmap = Bitmap.CreateScaledBitmap(bitmap, 2000, (int)(2000 * bitmap.Height / bitmap.Width), false);
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    translations = await vm.GetWordsToShow(stream);
                }
                //  Set results
                ShowPicture();

                resultsListView.Adapter = new Adapters.TranslationAdapter(translations);
            }
            catch (Exception e)
            {
                Toast.MakeText(this.ApplicationContext, "Hubo algún error :(", ToastLength.Long).Show();
                Console.WriteLine(e.Message);
                this.Finish();
            }
            finally
            {
                dialog.Dismiss();
            }
        }

        private void ShowPicture()
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = 8;
            Bitmap bitmap = BitmapFactory.DecodeFile(imageUri.Path, options);
            imageView.SetImageBitmap(bitmap);
        }

        private void ShowDialog()
        {
            dialog = new ProgressDialog(this);
            dialog.SetCancelable(false);
            dialog.SetMessage("Analizando imagen");
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            switch (requestCode)
            {
                case 100:
                    {
                        if (resultCode == Result.Ok && data != null)
                        {
                            var userInput = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults)[0];
                            var selectedItem = ((Adapters.TranslationAdapter)resultsListView.Adapter).SelectedItem;

                            /* TODO: Delegate this to TranslationManager */
                            if (selectedItem.OriginalWord.ToLower().Equals(userInput.ToLower()))
                            {
                                Toast.MakeText(
                                    this.ApplicationContext,
                                    "Correcto!",
                                    ToastLength.Long).Show();
                            }
                            else
                            {
                                Toast.MakeText(
                                    this.ApplicationContext,
                                    $"Incorrecto :(. Pronunciaste {userInput}",
                                    ToastLength.Long).Show();
                            }

                            /* TODO: Change activity */
                        }
                    }
                    break;
            }
        }
    }
}