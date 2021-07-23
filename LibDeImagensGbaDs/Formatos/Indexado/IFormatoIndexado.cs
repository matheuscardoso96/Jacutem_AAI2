
namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public interface IFormatoIndexado
    {
        byte[] AlphaValues { get; set; }
        byte[] Indices { get; set; }
        void ObtenhaIndicesPorPixel(byte[] arquivo, int enderecoInicial);
    }
}
