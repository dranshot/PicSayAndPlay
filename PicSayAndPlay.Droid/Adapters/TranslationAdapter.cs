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
using PicSayAndPlay.Models;
using Plugin.CurrentActivity;
using Plugin.TextToSpeech;
using Android.Speech;
using Java.Util;

namespace PicSayAndPlay.Droid.Adapters
{
    public class TranslationAdapter : BaseAdapter<Translation>
    {
        public Translation SelectedItem { get; set; }
        private List<Translation> _translations;

        public TranslationAdapter(List<Translation> translations)
        {
            _translations = translations;
        }

        public override Translation this[int position]
        {
            get
            {
                return _translations[position];
            }
        }

        public override int Count
        {
            get
            {
                return _translations.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = LayoutInflater.From(CrossCurrentActivity.Current.Activity).Inflate(Resource.Layout.AdapterTranslatedWord, null, false);

            var OriginalTvw = view.FindViewById<TextView>(Resource.Id.originalWordTvw);
            var TranslatedTvw = view.FindViewById<TextView>(Resource.Id.translatedWordTvw);
            var SpeakButton = view.FindViewById<ImageButton>(Resource.Id.speakBtn);
            var RecordButton = view.FindViewById<ImageButton>(Resource.Id.recordBtn);

            var translation = _translations[position];
            OriginalTvw.Text = translation.OriginalWord;
            TranslatedTvw.Text = translation.TranslatedWord;

            //  Event handlers
            RecordButton.Click += (s, e) => { CrossTextToSpeech.Current.Speak(translation.OriginalWord); };
            SpeakButton.Click += delegate
            {
                //  Get speech recognizer pop-up
                Intent i = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                i.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                i.PutExtra(RecognizerIntent.ExtraLanguage, Locale.English);
                i.PutExtra(RecognizerIntent.ExtraPrompt, "Say it!");
                SelectedItem = translation;
                try
                {
                    CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 100);
                }
                catch
                {
                    Toast.MakeText(CrossCurrentActivity.Current.Activity, "Algo raro pasó :(", ToastLength.Long).Show();
                }
            };

            return view;
        }
    }
}