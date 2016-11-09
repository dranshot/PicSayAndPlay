using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSayAndPlay.ViewModels
{

    public class MainViewModel
    {
        public static async Task<PictureResult> TakePhotoAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                return new PictureResult() { State = PhotoResult.CameraNotAvailable, Picture = null };

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                DefaultCamera = CameraDevice.Rear,
                SaveToAlbum = false
            });

            return CheckIfCanceled(file);
        }

        public static async Task<PictureResult> PickPhotoAsync()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                return new PictureResult() { State = PhotoResult.PickNotAvailable };

            var file = await CrossMedia.Current.PickPhotoAsync();

            return CheckIfCanceled(file);
        }

        private static PictureResult CheckIfCanceled(MediaFile file)
        {
            if (file == null)
                return new PictureResult() { State = PhotoResult.Canceled };
            else
                return new PictureResult() { State = PhotoResult.Success, Picture = file };
        }
    }

    public class PictureResult
    {
        public PhotoResult State { get; set; }
        public MediaFile Picture { get; set; } = null;
    }

    public enum PhotoResult
    {
        CameraNotAvailable,
        PickNotAvailable,
        Canceled,
        Success
    }
}
