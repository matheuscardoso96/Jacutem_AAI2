using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace LibDeImagensGbaDs.Util
{
    public static class GeradorDeBitmap
    {

        public static void GerarBitmap(Bitmap processedBitmap, byte[] indices, Color[] paleta)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);
                int bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* ptrFirstPixel = (byte*)bitmapData.Scan0;
                int contadorIndices = 0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        currentLine[x] = paleta[indices[contadorIndices]].B;
                        currentLine[x + 1] = paleta[indices[contadorIndices]].G;
                        currentLine[x + 2] = paleta[indices[contadorIndices]].R;
                        currentLine[x + 3] = paleta[indices[contadorIndices]].A;
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }
        }
    }
}
