using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
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
        private RecyclerView recyclerView;
        private List<Translation> translations;
        private Bitmap bitmap;
        private ResultViewModel vm;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result);

            //  Get picture's path from extra
            imageUri = Uri.Parse(Intent.GetStringExtra("Image"));

            imageView = FindViewById<ImageView>(Resource.Id.AnalyzedImage);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            vm = new ResultViewModel();
            bitmap = Helpers.BitmapHelper.GetAndRotateBitmap(imageUri.Path);

            try
            {
                ShowDialog();
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                    translations = await vm.GetWordsToShow(stream.ToArray(), bitmap.Height, bitmap.Width);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(this.ApplicationContext, "Hubo algún error :(", ToastLength.Long).Show();
                Log.WriteLine(LogPriority.Error, e.GetType().ToString(), e.InnerException.ToString());
                this.Finish();
            }
            finally
            {
                dialog.Dismiss();
            }

            //  Set results
            imageView.SetImageBitmap(bitmap);
            recyclerView.SetAdapter(new TranslationAdapter(translations));
            recyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
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
    }
}