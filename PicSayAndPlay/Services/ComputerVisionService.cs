using Microsoft.ProjectOxford.Vision;

namespace PicSayAndPlay.Services
{
    public class ComputerVisionService
    {
        private static readonly string _subscriptionClient = "03dc5372c67944259a23ba42fb92ec11";

        public static VisionServiceClient Client
        {
            get
            {
                return new VisionServiceClient(_subscriptionClient);
            }
        }
    }
}