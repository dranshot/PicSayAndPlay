using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Widget;
using Microsoft.ProjectOxford.Vision.Contract;
using PicSayAndPlay.Helpers;
using PicSayAndPlay.Models;
using PicSayAndPlay.Services;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private RecyclerView recyclerView;
        private List<Translation> translations;
        private AnalysisResult result;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result);

            //  Get picture's path from extra
            imageUri = Uri.Parse(Intent.GetStringExtra("Image"));

            imageView = FindViewById<ImageView>(Resource.Id.AnalyzedImage);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            Plugin.TextToSpeech.CrossTextToSpeech.Current.Init();


            ShowDialog();
            
            byte[] resizedImageArray = ResizeImage(imageUri);
            result = await ComputerVisionService.Client.GetTagsAsync(new MemoryStream(resizedImageArray));
            translations = await TranslationService.TranslateAsync(result);

            dialog.Dismiss();


            //  Set results
            Picasso.With(this.ApplicationContext).Load("file:" + imageUri).Into(imageView);
            recyclerView.SetAdapter(new TranslationAdapter(translations));
            recyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
        }

        private void ShowDialog()
        {
            dialog = new ProgressDialog(this);
            dialog.Indeterminate = true;
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
                            var result = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                            var position = ((TranslationAdapter)recyclerView.GetAdapter()).SelectedItemPosition;

                            /* TODO: Delegate this to TranslationManager */
                            if (translations[position].OriginalWord.ToLower().Equals(result[0].ToLower()))
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
                                    $"Incorrecto :(. Pronunciaste {result[0]}",
                                    ToastLength.Long).Show();
                            }

                            /* TODO: Change activity */
                        }
                    }
                    break;
            }
        }

        private byte[] ResizeImage(Uri imageUri)
        {
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                new FileStream(imageUri.Path, FileMode.Open).CopyTo(ms);
                imageData = ms.ToArray();
            }
            byte[] resizedImage = Resizer.ResizeImageAndroid(imageData);
            return resizedImage;
        }
    }
}