using LibDeImagensGbaDs.Util;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using System.Drawing.Imaging;


namespace ImageLibGbaDS.Conversor
{
    public class Indexado2D : IConversorFormatoIndexado
    {
        public IFormatoPaleta FormatoPaleta { get; set; }

        public Indexado2D(IFormatoPaleta formatoPaleta)
        {
            FormatoPaleta = formatoPaleta;
        }

        public Bitmap ConvertaIndexadoParaBmp(byte[] indices, int altura, int largura)
        {
            Bitmap final = new Bitmap(largura, altura, PixelFormat.Format32bppArgb);
            GeradorDeBitmap.GerarBitmap(final, indices, FormatoPaleta.Paleta);
            return final;

        }

        
    }
}
