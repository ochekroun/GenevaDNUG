using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FaceDetector.Library;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace FaceDetector.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
            canvas.InvalidateVisual();

            Title = "Detecting...";
            var facesCount = await _faceDetectionLibrary.DetectFaces();
            Title = $"Detection Finished. {facesCount} face(s) detected";
            canvas.InvalidateVisual();
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
