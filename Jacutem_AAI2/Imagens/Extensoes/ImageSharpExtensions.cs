//https://gist.github.com/zwcloud/2b52825bcc4408d4d5e076409adaf44a
using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Jacutem_AAI2.Imagens.Extensoes
{
    public static class ImageSharpExtensions
    {
        public static Byte[] ToArray<TPixel>(this Image<TPixel> image, IImageFormat imageFormat) where TPixel : unmanaged, IPixel<TPixel>
        {
            Span<byte> a = new byte[10];
            a[1] = 1;
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }

        public static System.Drawing.Bitmap ToBitmap<TPixel>(this Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);
                image.Save(memoryStream, imageEncoder);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return new System.Drawing.Bitmap(memoryStream);
            }
        }

        public static Image<TPixel> ToImageSharpImage<TPixel>(this System.Drawing.Bitmap bitmap) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return Image.Load<TPixel>(memoryStream);
            }
        }
    }
}
