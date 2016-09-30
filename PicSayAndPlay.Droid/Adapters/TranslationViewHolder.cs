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
using Android.Speech;
using Java.Util;
using Plugin.CurrentActivity;

namespace PicSayAndPlay.Droid
{
    public class TranslationViewHolder : RecyclerView.ViewHolder
    {
        public TextView OriginalTvw { get; set; }
        public TextView TranslatedTvw { get; set; }
        public ImageView ArrowIcon { get; set; }
        public ImageButton SpeakButton { get; set; }
        public ImageButton RecordButton { get; set; }

        public TranslationViewHolder(View itemView) : base(itemView)
        {
            OriginalTvw = itemView.FindViewById<TextView>(Resource.Id.originalWordTvw);
            TranslatedTvw = itemView.FindViewById<TextView>(Resource.Id.translatedWordTvw);
            ArrowIcon = itemView.FindViewById<ImageView>(Resource.Id.arrowIcon);
            SpeakButton = itemView.FindViewById<ImageButton>(Resource.Id.speakBtn);
            RecordButton = itemView.FindViewById<ImageButton>(Resource.Id.recordBtn);
        }
    }
}