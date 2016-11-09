using Android.App;
using Android.Content;
using Android.Speech;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using PicSayAndPlay.Models;
using Plugin.CurrentActivity;
using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;

namespace PicSayAndPlay.Droid
{
    public class TranslationAdapter : RecyclerView.Adapter
    {
        public List<Translation> Translations { get; set; }
        public RecyclerView RecyclerView { get; set; }
        public int SelectedItemPosition { get; set; }

        public TranslationAdapter(List<Translation> translations)
        {
            Translations = translations;
            RecyclerView = CrossCurrentActivity.Current.Activity
                .FindViewById<RecyclerView>(Resource.Id.recyclerView);
        }

        public override int ItemCount
        {
            get
            {
                return Translations.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = (TranslationViewHolder)holder;
            var translation = Translations[position];

            //  Fill up item info from list
            vh.OriginalTvw.Text = translation.OriginalWord;
            vh.TranslatedTvw.Text = translation.TranslatedWord;
            vh.RecordButton.Click += (s, e) => { CrossTextToSpeech.Current.Speak(translation.OriginalWord); };
            if (!vh.SpeakButton.HasOnClickListeners) //  To prevent multiple events
                vh.SpeakButton.Click += SpeakButton_Click;
        }

        private void SpeakButton_Click(object sender, EventArgs e)
        {
            //  Get speech recognizer pop-up
            Intent i = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            i.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            i.PutExtra(RecognizerIntent.ExtraLanguage, Locale.English);
            i.PutExtra(RecognizerIntent.ExtraPrompt, "Say it!");
            var view = (View)sender;
            SelectedItemPosition = RecyclerView.GetChildLayoutPosition((View)view.Parent.Parent);
            try
            {
                CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 100);
            }
            catch
            {
                Toast.MakeText(CrossCurrentActivity.Current.Activity, "Algo raro pasó :(", ToastLength.Long).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layout = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.AdapterTranslatedWord, parent, false);
            return new TranslationViewHolder(layout);
        }
    }
}