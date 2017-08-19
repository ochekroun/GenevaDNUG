using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using SkiaSharp;

namespace FaceDetector.Library
{
    public class FaceDetectoryLibrary
    {
        private readonly FaceServiceClient _faceServiceClient;
        private FaceRectangle[] _faceRectangles;
        private string _pictureFilePath;

        public FaceDetectoryLibrary(string pictureFilePath)
        {
            _faceServiceClient = new FaceServiceClient(PrivateKey.ID, PrivateKey.URL);
            _pictureFilePath = pictureFilePath;
        }

        public void DrawPicture(SKCanvas canvas)
        {
            canvas.Clear();
            if (string.IsNullOrEmpty(_pictureFilePath))
            {
                return;
            }

            using (var bitmap = SKBitmap.Decode(_pictureFilePath))
            {
                var width = canvas.DeviceClipBounds.Width;
                var height = canvas.DeviceClipBounds.Height;
                var bmpRect = SKRectI.Create(width, height).AspectFit(bitmap.Info.Size);

                // draw directly on the bitmap
                using (var annotationCanvas = new SKCanvas(bitmap))
                using (var paint = new SKPaint())
                using (var resized = new SKBitmap(bmpRect.Width, bmpRect.Height))
                {
                    paint.StrokeWidth = 2;
                    paint.Color = SKColors.Red;
                    paint.Style = SKPaintStyle.Stroke;

                    if (_faceRectangles != null)
                    {
                        foreach (var faceRect in _faceRectangles)
                        {
                            var rect = SKRectI.Create(faceRect.Left, faceRect.Top, faceRect.Width, faceRect.Height);
                            annotationCanvas.DrawRect(rect, paint);
                        }
                    }
                    bitmap.Resize(resized, SKBitmapResizeMethod.Box);
                    canvas.DrawBitmap(resized, bmpRect);
                }
            }
        }

        public async Task<int> DetectFaces()
        {
            try
            {

                using (Stream imageFileStream = File.OpenRead(_pictureFilePath))
                {
                    var faces = await _faceServiceClient.DetectAsync(imageFileStream);
                    var faceRects = faces.Select(face => face.FaceRectangle);
                    _faceRectangles = faceRects.ToArray();
                }
            }
            catch (Exception)
            {
                _faceRectangles = new FaceRectangle[0];
            }
            return _faceRectangles.Length;
        }

    }
}
