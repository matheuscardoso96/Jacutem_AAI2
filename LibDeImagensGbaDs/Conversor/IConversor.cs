using System.Collections.Generic;
using System.Drawing;

namespace LibDeImagensGbaDs.Conversor
{
    public interface IConversor
    {
       Bitmap ConvertaParaBmp(byte[] arquivo, int tamanho, int enderecoInicial);
       List<object> ConvertaParaBin(Bitmap imagem);

    }
}
