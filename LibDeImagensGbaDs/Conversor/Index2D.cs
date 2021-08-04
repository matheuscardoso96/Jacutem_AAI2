using LibDeImagensGbaDs.Util;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using System.Drawing.Imaging;
using LibDeImagensGbaDs.Formatos.Indexado;
using System;
using System.Collections.Generic;
using LibDeImagensGbaDs.Sprites;

namespace LibDeImagensGbaDs.Conversor
{
    public class Index2D
    {
        public int Largura { get; set; }
        public int Altura { get; set; }
        public Index2D(int altura, int largura)
        {
            Largura = largura;
            Altura = altura;
        }

        public Index2D(List<Oam> valoresOam)
        {

        }

        public Bitmap ConvertaIndexado(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta)
        {
            Bitmap final = new Bitmap(Largura, Altura, PixelFormat.Format32bppArgb);
            byte[] valoresAlpha = null;
            if (paleta.TemAlpha)
            {
                valoresAlpha = new byte[formatoIndexado.Indices.Length];
                Array.Copy(formatoIndexado.AlphaValues, 0, valoresAlpha, 0, valoresAlpha.Length);
            }

            ManipuladorDeImagem.GerarBitmap(final, formatoIndexado.Indices, paleta.Cores, paleta.TemAlpha, valoresAlpha);
            return final;

        }

        public List<object> GerarIndeces(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta, Bitmap imagem)
        {
            List<byte> indices = new List<byte>();
            Color[] cores = ManipuladorDeImagem.ObtenhaCoresDeImagem(imagem);

            foreach (var cor in cores)
                indices.Add(paleta.ObtenhaIndexCorMaisProxima(cor));

            List<object> final = new List<object>() { formatoIndexado.GereIndices(indices.ToArray()) };         
            return final;
        }
    }
}
