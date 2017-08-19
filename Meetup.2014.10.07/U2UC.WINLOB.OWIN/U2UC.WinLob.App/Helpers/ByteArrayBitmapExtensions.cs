namespace U2UC.WinLob.App
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Extension methods to transform string and byte array to bitmap.
    /// </summary>
    internal static class ByteArrayBitmapExtensions
    {
        public static BitmapImage AsBitmapImage(this byte[] byteArray)
        {
            if (byteArray != null)
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    stream.WriteAsync(byteArray.AsBuffer()).GetResults(); // I made this one synchronous on the UI thread; this is not a best practice.
                    var image = new BitmapImage();
                    stream.Seek(0);
                    image.SetSource(stream);
                    return image;
                }
            }

            return null;
        }

        public static BitmapImage AsBitmapImage(this string base64string)
        {
            byte[] buffer = Convert.FromBase64String(base64string);
            return buffer.AsBitmapImage();
        }
    }
}
