using ImageLibGbaDS.Formatos;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using System.IO;

namespace ImageLibGbaDS.Conversor
{
    public interface IConversorFormatoIndexado
    {
        IFormatoPaleta FormatoPaleta { get; set;}
        Bitmap ConvertaIndexadoParaBmp(byte[] indices, int largura, int altura);
    }
}
