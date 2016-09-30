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
using Android.Support.V7.Widget;
using PicSayAndPlay.Models;
using Plugin.TextToSpeech;
using Plugin.CurrentActivity;
using Android.Speech;
using Java.Util;

namespace PicSayAndPlay.Droid
{
    public class TranslationAdapter : RecyclerView.Adapter
    {
        public List<Translation> Translations { get; set; }
        public RecyclerView RecyclerView { get; set; }
        public Activity Activity { get; set; }
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

            //  Fill up item info from list
            vh.OriginalTvw.Text = Translations[position].OriginalWord;
            vh.TranslatedTvw.Text = Translations[position].TranslatedWord;
            vh.RecordButton.Click += (s, e) => { CrossTextToSpeech.Current.Speak(vh.OriginalTvw.Text); };
            if(!vh.SpeakButton.HasOnClickListeners) //  To prevent multiple events
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
            var id = RecyclerView.GetChildLayoutPosition((View)view.Parent.Parent);
            SelectedItemPosition = id;
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