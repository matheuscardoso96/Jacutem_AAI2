using LibDeImagensGbaDs.Util;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using System.Drawing.Imaging;
using LibDeImagensGbaDs.Formatos.Indexado;
using System;

namespace LibDeImagensGbaDs.Conversor
{
    public class Index2D : IConversorIndex
    {
        public int Largura { get; set; }
        public int Altura { get; set; }
        public Index2D(int altura, int largura)
        {
            Largura = largura;
            Altura = altura;
        }

        public Bitmap ConvertaIndexado(IFormatoIndexado formatoIndexado, IPaleta paleta)
        {
            Bitmap final = new Bitmap(Largura, Altura, PixelFormat.Format32bppArgb);
            byte[] valoresAlpha = null;
            if (paleta.TemAlpha)
            {
                valoresAlpha = new byte[formatoIndexado.Indices.Length];
                Array.Copy(formatoIndexado.AlphaValues, 0, valoresAlpha, 0, valoresAlpha.Length);
            }

            ManipuladorDeImagem.GerarBitmap(final, formatoIndexado.Indices, paleta.Cores,paleta.TemAlpha, valoresAlpha);
            return final;

        }

        
    }
}
