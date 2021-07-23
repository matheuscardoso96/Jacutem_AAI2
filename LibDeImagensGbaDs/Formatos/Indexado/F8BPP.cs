using System;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F8BPP: IFormatoIndexado
    {
        public const int Bpp = 8;
        public byte[] Indices { get; set; }
        public byte[] AlphaValues { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public F8BPP(int largura, int altura)
        {
            Largura = largura;
            Altura = altura;
        }
        public void ObtenhaIndicesPorPixel(byte[] arquivo,  int enderecoInicial = 0)
        {
            int totalBytes = ((Largura * Altura) * Bpp) / 8;
            byte[] rawIndexes = new byte[totalBytes];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, totalBytes);
            Indices = rawIndexes;
        }
    }
}
