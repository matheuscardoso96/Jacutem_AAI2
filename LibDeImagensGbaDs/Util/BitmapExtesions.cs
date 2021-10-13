using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LibDeImagensGbaDs.Util
{
    public static class BitmapExtesions
    {

        public static void PoulateBitmap(this Bitmap processedBitmap, byte[] indexes, Color[] paleta, byte[] alphaValues = null)
        {
            bool temAlpha = alphaValues != null;
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
                        currentLine[x] = paleta[indexes[contadorIndices]].B;
                        currentLine[x + 1] = paleta[indexes[contadorIndices]].G;
                        currentLine[x + 2] = paleta[indexes[contadorIndices]].R;
                        currentLine[x + 3] = temAlpha? alphaValues[contadorIndices]: paleta[indexes[contadorIndices]].A;
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }
        }


        public static Color[] GetColors(this Bitmap processedBitmap, bool hasAlpha = false)
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

                        if (hasAlpha)
                        {
                            cores[contadorIndices] = Color.FromArgb(currentLine[x + 3], currentLine[x + 2], currentLine[x + 1], currentLine[x]);
                        }
                        else
                        {
                            cores[contadorIndices] = Color.FromArgb(currentLine[x + 2], currentLine[x + 1], currentLine[x]);
                        }
                            
                       
                        
                        
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }

            return cores;
        }

        public static List<Bitmap> SlitIntoTiles(this Bitmap image, int tileWi, int tileHe)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            for (int y = 0; y < image.Height; y += tileHe)
            {
                for (int x = 0; x < image.Width; x += tileWi)
                {
                    Rectangle rec = new Rectangle(x, y, tileWi, tileHe);
                    tiles.Add(image.Clone(rec, image.PixelFormat));

                }
            }

            //using (Graphics g = Graphics.FromImage(image))
            //{

            //}


            return tiles;
        }

        public static Bitmap ConvertTo32Bpp(this Bitmap image)
        {
            Bitmap imagem32Bpp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppRgb);

            using (Graphics graphics = Graphics.FromImage(imagem32Bpp))
            {
                graphics.DrawImage(image, new Point(0, 0));
            }

            return imagem32Bpp;

        }
    }
}
