using LibDeImagensGbaDs.Formatos.Indexado;
using System.Drawing;
using System.Drawing.Imaging;

namespace LibDeImagensGbaDs.Util
{
    public static class ManipuladorDeImagem
    {

        public static void GerarBitmap(Bitmap processedBitmap, byte[] indices, Color[] paleta, bool temAlpha = false, byte[] valoresAlpha = null)
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
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        currentLine[x] = paleta[indices[contadorIndices]].B;
                        currentLine[x + 1] = paleta[indices[contadorIndices]].G;
                        currentLine[x + 2] = paleta[indices[contadorIndices]].R;
                        currentLine[x + 3] = temAlpha? valoresAlpha[contadorIndices]: paleta[indices[contadorIndices]].A;
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }
        }


        public static Color[] ObtenhaCoresDeImagem(Bitmap processedBitmap)
        {
            Color[] cores = new Color[processedBitmap.Width * processedBitmap.Height];
            
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
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        cores[contadorIndices] = Color.FromArgb(currentLine[x + 3], currentLine[x + 2], currentLine[x + 1], currentLine[x]);
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }

            return cores;
        }
    }
}
