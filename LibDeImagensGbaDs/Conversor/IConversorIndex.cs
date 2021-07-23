using ImageLibGbaDS.Paleta;
using LibDeImagensGbaDs.Formatos.Indexado;
using System.Drawing;

namespace LibDeImagensGbaDs.Conversor
{
    public interface IConversorIndex
    {
        Bitmap ConvertaIndexado(IFormatoIndexado formatoIndexado, IPaleta paleta);
    }
}
