using System;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F8BPP: IConversorDeProfundidadeDeCor
    {
        public const int Bpp = 8;
        public byte[] Indices { get; set; }
        public byte[] AlphaValues { get; set; }

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho, int largura, int altura, int enderecoInicial = 0)
        {
            byte[] rawIndexes = new byte[largura * altura];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, tamanho);
            Indices = rawIndexes;
        }

        public byte[] GereIndices(byte[] indices)
        {
            return indices;
        }
    }
}
