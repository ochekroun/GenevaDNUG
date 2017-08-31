using System;
using Plugin.Media;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using FaceDetector.Library;
using Plugin.Media.Abstractions;

namespace FaceDetector.XamarinForms
{
    public partial class MainPage : ContentPage
    {
        private FaceDetectorLibrary _faceDetectoryLibrary;

        public MainPage()
        {
            InitializeComponent();
        }

        private FaceDetectorLibrary FaceDetectorLibrary
        {
            get
            {
                if(_faceDetectoryLibrary == null)
                {
                    var id = AppSettings.PrivateKeyId;
                    var url = AppSettings.PrivateKeyUrl;
                    _faceDetectoryLibrary = new FaceDetectorLibrary(id, url);
                }
                return _faceDetectoryLibrary;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            MediaFile mediaFile = null;

            if (sender == BtnPickPhoto)
            {
                mediaFile = await CrossMedia.Current.PickPhotoAsync();
            }
            else if(sender == BtnTakePhoto)
            {
                mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            }

            if (mediaFile == null)
            {
                return;
            }

            FaceDetectorLibrary.LoadPicture(mediaFile.Path);
            SKCanvas.InvalidateSurface();

            LabelResult.Text = "Detecting...";
            var facesCount = await FaceDetectorLibrary.DetectFaces();
            LabelResult.Text = $"Detection Finished. {facesCount} face(s) detected";
            SKCanvas.InvalidateSurface();
        }

        private void SKCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var skElement = sender as SKCanvasView;
            if (skElement != null)
            {
                FaceDetectorLibrary.DrawPicture(e.Surface.Canvas);
            }
        }
    }
}
