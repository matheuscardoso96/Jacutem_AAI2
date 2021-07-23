using System;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F4BPP : IFormatoIndexado
    {

        public const int Bpp = 4;
        public byte[] Indices { get; set; }
        public byte[] AlphaValues { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public F4BPP(int largura, int altura)
        {
            Largura = largura;
            Altura = altura;
        }

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int enderecoInicial = 0)
        {
            int totalBytes = ((Largura * Altura) * Bpp) / 8;
            byte[] rawIndexes = new byte[totalBytes];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, totalBytes);

            byte[] final = new byte[Largura * Altura];

            int contador = 0;
            for (int i = 0; i < rawIndexes.Length; i++)
            {
                int nibbleAlto = (rawIndexes[i] & 0xF0) >> 4;
                int nibbleBaixo = (rawIndexes[i] & 0x0F);
                final[contador] = (byte)nibbleBaixo;
                final[contador + 1] = (byte)nibbleAlto;
                contador+=2;

            }


           Indices = final;
        }

    }
}
