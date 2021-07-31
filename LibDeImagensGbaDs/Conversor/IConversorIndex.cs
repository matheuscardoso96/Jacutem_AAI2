using ImageLibGbaDS.Paleta;
using LibDeImagensGbaDs.Formatos.Indexado;
using System.Collections.Generic;
using System.Drawing;

namespace LibDeImagensGbaDs.Conversor
{
    public interface IConversorIndex
    {
        Bitmap ConvertaIndexado(IFormatoIndexado formatoIndexado, IPaleta paleta);
        List<object> GerarIndeces(IFormatoIndexado formatoIndexado, IPaleta paleta, Bitmap imagem);
    }
}
