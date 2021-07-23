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

        public Bitmap BinParaBmp(byte[] arquivo, int enderecoInicial) 
        {
            return Conversor.ConvertaParaBmp(arquivo,enderecoInicial);
        }

    }
}
