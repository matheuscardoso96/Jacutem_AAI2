using LibDeImagensGbaDs.Formatos.Indexado;
using System.Collections.Generic;
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
                        if (bytesPerPixel == 4)
                        {
                            cores[contadorIndices] = Color.FromArgb(currentLine[x + 3], currentLine[x + 2], currentLine[x + 1], currentLine[x]);
                        }
                        else if (bytesPerPixel == 3)
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

        public static List<Bitmap> DividaImagemEmTiles(Bitmap img)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            for (int y = 0; y < img.Height; y += 8)
            {
                for (int x = 0; x < img.Width; x += 8)
                {
                    Rectangle rec = new Rectangle(x, y, 8, 8);
                    tiles.Add(img.Clone(rec, img.PixelFormat));

                }
            }

            img.Dispose();

            return tiles;
        }

        public static Bitmap MudarPixelFormatPra32Bpp(Bitmap imagem)
        {
            Bitmap imagem32Bpp = new Bitmap(imagem.Width, imagem.Height, PixelFormat.Format32bppRgb);

            using (Graphics graphics = Graphics.FromImage(imagem32Bpp))
            {
                graphics.DrawImage(imagem, new Point(0, 0));
            }

            return imagem32Bpp;

        }
    }
}
