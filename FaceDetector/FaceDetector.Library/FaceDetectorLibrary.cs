using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using SkiaSharp;

namespace FaceDetector.Library
{
    public class FaceDetectorLibrary
    {
        private readonly FaceServiceClient _faceServiceClient;
        private Face[] _faces;
        private string _pictureFilePath;

        public FaceDetectorLibrary(string id, string url)
        {
            _faceServiceClient = new FaceServiceClient(id, url);
            _faces = new Face[0];
        }

        public void LoadPicture(string filePath)
        {
            _pictureFilePath = filePath;
            _faces = new Face[0];
        }

        public void DrawPicture(SKCanvas canvas)
        {
            if (string.IsNullOrEmpty(_pictureFilePath))
            {
                return;
            }

            canvas.Clear();

            using (var bitmap = SKBitmap.Decode(_pictureFilePath))
            {
                // draw directly on the bitmap
                using (var annotationCanvas = new SKCanvas(bitmap))
                using (var paint = new SKPaint())
                {
                    paint.StrokeWidth = 2;
                    //paint.Color = SKColors.Red;
                    paint.Style = SKPaintStyle.Stroke;

                    foreach (var face in _faces)
                    {
                        var faceRect = face.FaceRectangle;
                        paint.Color = face.FaceAttributes.Gender == "female" ? SKColors.Green : SKColors.Red;

                        var rect = SKRectI.Create(faceRect.Left, faceRect.Top, faceRect.Width, faceRect.Height);
                        annotationCanvas.DrawRect(rect, paint);
                    }
                }

                // Resize
                var width = canvas.DeviceClipBounds.Width;
                var height = canvas.DeviceClipBounds.Height;
                var bmpRect = SKRectI.Create(width, height).AspectFit(bitmap.Info.Size);
                using (var resized = new SKBitmap(bmpRect.Width, bmpRect.Height))
                {
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
                    _faces = await _faceServiceClient.DetectAsync(imageFileStream,
                        true,
                        true,
                        new[] {
                            FaceAttributeType.Gender,
                            FaceAttributeType.Age,
                            FaceAttributeType.Emotion
                        });
                }
            }
            catch
            {
                _faces = new Face[0];
                throw;
            }
            return _faces.Length;
        }
    }
}
