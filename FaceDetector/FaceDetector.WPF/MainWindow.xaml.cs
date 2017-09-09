using System;
using System.Configuration;
using System.Windows;
using FaceDetector.Library;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using Microsoft.Win32;

namespace FaceDetector.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FaceDetectorLibrary _faceDetectorLibrary;

        public MainWindow()
        {
            InitializeComponent();
        }

        private FaceDetectorLibrary FaceDetectorLibrary
        {
            get
            {
                if (_faceDetectorLibrary == null)
                {
                    var id = ConfigurationManager.AppSettings["PrivateKeyId"];
                    var url = ConfigurationManager.AppSettings["PrivateKeyUrl"];
                    _faceDetectorLibrary = new FaceDetectorLibrary(id, url);
                }
                return _faceDetectorLibrary;
            }
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openDlg = new OpenFileDialog { Filter = "JPEG Image(*.jpg)|*.jpg" };
                bool? result = openDlg.ShowDialog(this);

                if (!result.Value)
                {
                    return;
                }

                FaceDetectorLibrary.LoadPicture(openDlg.FileName);
                Canvas.InvalidateVisual();

                Title = "Detecting...";
                var facesCount = await FaceDetectorLibrary.DetectFaces();
                Title = $"Detection Finished. {facesCount} face(s) detected";
                Canvas.InvalidateVisual();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (sender is SKElement)
            {
                FaceDetectorLibrary.DrawPicture(e.Surface.Canvas);
            }
        }
    }
}
