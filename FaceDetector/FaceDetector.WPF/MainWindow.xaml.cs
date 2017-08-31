using System.Configuration;
using System.Windows;
using FaceDetector.Library;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace FaceDetector.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FaceDetectorLibrary _faceDetectionLibrary;

        public MainWindow()
        {
            InitializeComponent();

            var id = ConfigurationManager.AppSettings["PrivateKeyId"];
            var url = ConfigurationManager.AppSettings["PrivateKeyUrl"];
            _faceDetectionLibrary = new FaceDetectorLibrary(id, url);
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new Microsoft.Win32.OpenFileDialog { Filter = "JPEG Image(*.jpg)|*.jpg" };

            bool? result = openDlg.ShowDialog(this);

            if (!(bool)result)
            {
                return;
            }

            _faceDetectionLibrary.LoadPicture(openDlg.FileName);
            Canvas.InvalidateVisual();

            Title = "Detecting...";
            var facesCount = await _faceDetectionLibrary.DetectFaces();
            Title = $"Detection Finished. {facesCount} face(s) detected";
            Canvas.InvalidateVisual();
        }


        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_faceDetectionLibrary == null)
            {
                return;
            }
            var skElement = sender as SKElement;
            if (skElement != null)
            {
                _faceDetectionLibrary.DrawPicture(e.Surface.Canvas);
            }
        }
    }
}
