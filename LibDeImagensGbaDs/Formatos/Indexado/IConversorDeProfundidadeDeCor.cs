
namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public interface IConversorDeProfundidadeDeCor
    {
        byte[] AlphaValues { get; set; }
        byte[] Indices { get; set; }
        void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho , int enderecoInicial, int largura, int altura);
        byte[] GereIndices(byte[] indices);
    }
}
