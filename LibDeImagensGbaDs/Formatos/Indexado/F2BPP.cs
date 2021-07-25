using System;
using System.Collections;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F2BPP : IFormatoIndexado
    {
        public const int Bpp = 2;
        public byte[] AlphaValues { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public byte[] Indices { get; set; }
        public F2BPP(int largura, int altura)
        {
            Largura = largura;
            Altura = altura;
        }

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho, int enderecoInicial = 0)
        {
           // int totalBytes = ((Largura * Altura) * Bpp) / 8;
            byte[] rawIndexes = new byte[tamanho];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, tamanho);

            BitArray bitArray = new BitArray(rawIndexes);
            byte[] final = new byte[Largura * Altura];
            
            int contador = 0;
            for (int i = 0; i < bitArray.Length; i += 2) 
            {
                int valor1 = bitArray[i] ? 1 : 0;
                int valor2 = bitArray[i + 1] ? 1 : 0;
                int valorFinal = (valor2 << 1) | valor1;
                final[contador] = (byte)valorFinal;
                contador++;
            }
                

            Indices = final;
        }
    }
}
