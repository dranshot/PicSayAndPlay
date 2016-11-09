using Android.Support.Design.Widget;
using System;

namespace PicSayAndPlay.Droid.Helpers
{
    public class Util
    {
        public static void ShowError(TextInputLayout inputLayout, string message)
        {
            inputLayout.Error = message;

            if (String.IsNullOrEmpty(message))
                inputLayout.ErrorEnabled = false;
            else
                inputLayout.ErrorEnabled = true;
        }
    }
}