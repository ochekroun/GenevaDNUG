namespace FaceDetector.XamarinForms
{
    public static class AppSettings
    {
        public static readonly string PrivateKeyId;
        public static readonly string PrivateKeyUrl;
        static AppSettings()
        {
            PrivateKeyId = "668ab4016405405e8ed00cce4c95a096";
            PrivateKeyUrl = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/";
        }
    }
}
