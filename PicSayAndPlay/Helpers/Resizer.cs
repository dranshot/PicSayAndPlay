using Android.Graphics;
using System.IO;

namespace PicSayAndPlay.Helpers
{
    public class Resizer
    {
        public static byte[] ResizeImageAndroid(byte[] imageData, float width = 1000, float height = 1000)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}