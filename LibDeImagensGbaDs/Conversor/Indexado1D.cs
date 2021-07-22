﻿using ImageLibGbaDS.Paleta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Conversor
{
    public class Indexado1D : IConversorFormatoIndexado
    {
        public IFormatoPaleta FormatoPaleta { get ; set; }

        public Indexado1D(IFormatoPaleta formatoPaleta)
        {
            FormatoPaleta = formatoPaleta;
        }

        public Bitmap ConvertaIndexadoParaBmp(byte[] indices, int altura, int largura)
        {
            Bitmap final = new Bitmap(largura, altura, PixelFormat.Format32bppArgb);
            ProcessUsingLockbitsAndUnsafe(final, indices);
            return final;

        }

        private void ProcessUsingLockbitsAndUnsafe(Bitmap processedBitmap, byte[] indices)
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
                        currentLine[x] = FormatoPaleta.Paleta[indices[contadorIndices]].B;
                        currentLine[x + 1] = FormatoPaleta.Paleta[indices[contadorIndices]].G;
                        currentLine[x + 2] = FormatoPaleta.Paleta[indices[contadorIndices]].R;
                        currentLine[x + 3] = FormatoPaleta.Paleta[indices[contadorIndices]].A;
                        contadorIndices++;
                    }
                }

                processedBitmap.UnlockBits(bitmapData);
            }
        }
    }
}
