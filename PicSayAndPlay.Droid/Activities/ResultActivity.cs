using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Widget;
using Java.Util;
using PicSayAndPlay.Helpers;
using PicSayAndPlay.Services;
using Plugin.TextToSpeech;
using System;
using System.IO;
using Uri = Android.Net.Uri;

namespace PicSayAndPlay.Droid
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {
        private ProgressDialog dialog;
        private ImageView imageView;
        private TextView resultText;
        private ImageButton pronounceBtn;
        private ImageButton practiceBtn;
        private Uri imageUri;
        private string word;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result);

            //  Get picture's path from extra
            var path = Intent.GetStringExtra("Image");
            imageUri = Uri.Parse(path);

            imageView = FindViewById<ImageView>(Resource.Id.AnalyzedImage);
            resultText = FindViewById<TextView>(Resource.Id.ResultTxt);
            pronounceBtn = FindViewById<ImageButton>(Resource.Id.pronounceBtn);
            practiceBtn = FindViewById<ImageButton>(Resource.Id.speakBtn);

            imageView.SetImageURI(imageUri);

            //  Resize image and send it
            dialog = ProgressDialog.Show(this, "Un momento", "Analizando imagen...");
            byte[] resizedImageArray = ResizeImage();
            var result = await ComputerVisionService.Client.DescribeAsync(new MemoryStream(resizedImageArray));
            dialog.Dismiss();

            word = result?.Description.Tags[0];
            resultText.Text = word;

            pronounceBtn.Click += (s, e) => { CrossTextToSpeech.Current.Speak(word); };
            practiceBtn.Click += PracticeBtn_Click;
        }

        private void PracticeBtn_Click(object sender, EventArgs e)
        {
            //  Get speech recognizer pop-up
            Intent i = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            i.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            i.PutExtra(RecognizerIntent.ExtraLanguage, Locale.Default);
            i.PutExtra(RecognizerIntent.ExtraPrompt, "Say it!");
            try
            {
                StartActivityForResult(i, 100);
            }
            catch (ActivityNotFoundException)
            {
                Toast.MakeText(this.ApplicationContext, "Algo malo pasó", ToastLength.Long).Show();
            }
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

                            /* TODO: Delegate this to TranslationManager */
                            if (word.ToLower().Equals(result[0].ToLower()))
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