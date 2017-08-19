using System;
using Plugin.Media;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using FaceDetector.Library;

namespace FaceDetector.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        private FaceDetectoryLibrary _faceDetectionLibrary;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            var mediaFile = await CrossMedia.Current.PickPhotoAsync();

            if (mediaFile == null)
            {
                return;
            }

            _faceDetectionLibrary = new FaceDetectoryLibrary(mediaFile.Path);
            canvas.InvalidateSurface();

            Title = "Detecting...";
            var facesCount = await _faceDetectionLibrary.DetectFaces();
            Title = $"Detection Finished. {facesCount} face(s) detected";
            canvas.InvalidateSurface();
        }

        private void SKCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_faceDetectionLibrary == null)
            {
                return;
            }
            var skElement = sender as SKCanvasView;
            if (skElement != null)
            {
                _faceDetectionLibrary.DrawPicture(e.Surface.Canvas);
            }
        }
    }
}
