using ImageLibGbaDS.Paleta;
using LibDeImagensGbaDs.Formatos.Indexado;
using System.Collections.Generic;
using System.Drawing;

namespace LibDeImagensGbaDs.Conversor
{
    public interface IConversorIndex
    {
        Bitmap ConvertaIndexado(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta);
        List<object> GerarIndeces(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta, Bitmap imagem);
    }
}
