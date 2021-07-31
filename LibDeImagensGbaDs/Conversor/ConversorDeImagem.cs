using System.Collections.Generic;
using System.Drawing;

namespace LibDeImagensGbaDs.Conversor
{
    public class ConversorDeImagem
    {
        public IConversor Conversor { get; set; }
        public ConversorDeImagem(IConversor conversor)
        {
            Conversor = conversor;
        }

        public Bitmap BinParaBmp(byte[] arquivo,int tamanho, int enderecoInicial) 
        {
            return Conversor.ConvertaParaBmp(arquivo,tamanho,enderecoInicial);
        }

        public List<object> BmpParaBin(Bitmap imagem)
        {
            return Conversor.ConvertaParaBin(imagem);
        }

    }
}
