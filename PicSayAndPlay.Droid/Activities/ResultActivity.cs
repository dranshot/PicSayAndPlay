using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Widget;
using Microsoft.ProjectOxford.Vision.Contract;
using PicSayAndPlay.Helpers;
using PicSayAndPlay.Models;
using PicSayAndPlay.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uri = Android.Net.Uri;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {
        private ProgressDialog dialog;
        private ImageView imageView;
        private Uri imageUri;
        private RecyclerView recyclerView;
        private List<Translation> list;
        private AnalysisResult result;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result);

            //  Get picture's path from extra
            var path = Intent.GetStringExtra("Image");
            imageUri = Uri.Parse(path);

            imageView = FindViewById<ImageView>(Resource.Id.AnalyzedImage);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            imageView.SetImageURI(imageUri);

            //  Resize image and send it
            dialog = ProgressDialog.Show(this, "Un momento", "Analizando imagen...");

            byte[] resizedImageArray = ResizeImage();
            try
            {
                result = await ComputerVisionService.Client.DescribeAsync(new MemoryStream(resizedImageArray));
            }
            catch (Exception e)
            {
                Log.WriteLine(LogPriority.Error, "ComputerVision", "Error trying to describe image" + e.Message);
            }
            var words = result.Description.Tags.ToList();

            //  Translate list
            var translations = new List<string>();
            foreach (var word in words)
            {
                try
                {
                    var trans = await TranslationService.Translate(word);
                    translations.Add(trans);
                }
                catch (Exception e)
                {
                    Log.WriteLine(LogPriority.Error, "Translation", "Error trying to translate" + e.Message);
                }
            }

            dialog.Dismiss();

            //  Translate list of words
            list = new List<Translation>();
            for (int i = 0; i < words.Count; i++)
            {
                list.Add(new Translation(words[i], translations[i], "", DateTime.Now));
            }

            //  Set results
            recyclerView.SetAdapter(new TranslationAdapter(list));
            recyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
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
                            if (list[position].OriginalWord.ToLower().Equals(result[0].ToLower()))
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

        private byte[] ResizeImage()
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