using System;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F4BPP : IConversorDeProfundidadeDeCor
    {

        public const int Bpp = 4;
        public byte[] Indices { get; set; }
        public byte[] AlphaValues { get; set; }

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int largura, int altura, int tamanho, int enderecoInicial = 0)
        {
            byte[] rawIndexes = new byte[tamanho];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, tamanho);

            byte[] final = new byte[largura * altura];

            int contador = 0;
            for (int i = 0; i < rawIndexes.Length; i++)
            {
                int nibbleAlto = (rawIndexes[i] & 0xF0) >> 4;
                int nibbleBaixo = rawIndexes[i] & 0x0F;
                final[contador] = (byte)nibbleBaixo;
                final[contador + 1] = (byte)nibbleAlto;
                contador+=2;

            }


           Indices = final;
        }

        public byte[] GereIndices(byte[] indices)
        {

            byte[] listaDeIndicesFinal = new byte[indices.Length / 2];

            int contador = 0;

            for (int i = 0; i < indices.Length; i+=2)
            {
                byte indice =(byte)((indices[i + 1] << 4)  + indices[i]);
                listaDeIndicesFinal[contador] = indice;
                contador++;
            }

            return listaDeIndicesFinal;

        }

    }
}
