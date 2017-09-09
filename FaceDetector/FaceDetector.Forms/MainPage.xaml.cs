using System;
using FaceDetector.Library;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SkiaSharp.Views.Forms;

namespace FaceDetector.Forms
{
    public partial class MainPage
    {
        private FaceDetectorLibrary _faceDetectorLibrary;

        public MainPage()
        {
            InitializeComponent();
        }

        private FaceDetectorLibrary FaceDetectorLibrary
        {
            get
            {
                if (_faceDetectorLibrary == null)
                {
                    var id = AppSettings.PrivateKeyId;
                    var url = AppSettings.PrivateKeyUrl;
                    _faceDetectorLibrary = new FaceDetectorLibrary(id, url);
                }
                return _faceDetectorLibrary;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                MediaFile mediaFile = null;

                if (sender == BtnPickPhoto)
                {
                    mediaFile = await CrossMedia.Current.PickPhotoAsync();
                }
                else if (sender == BtnTakePhoto)
                {
                    mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                }

                if (mediaFile == null)
                {
                    return;
                }

                FaceDetectorLibrary.LoadPicture(mediaFile.Path);
                SkCanvas.InvalidateSurface();

                LabelResult.Text = "Detecting...";
                var facesCount = await FaceDetectorLibrary.DetectFaces();
                LabelResult.Text = $"Detection Finished. {facesCount} face(s) detected";
                SkCanvas.InvalidateSurface();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void SKCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (sender is SKCanvasView)
            {
                FaceDetectorLibrary.DrawPicture(e.Surface.Canvas);
            }
        }
    }
}
