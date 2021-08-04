using System;
using System.Collections;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F1BPP : IConversorDeProfundidadeDeCor
    {
        public const int Bpp = 1;
        public byte[] AlphaValues { get; set; }
        public byte[] Indices { get; set; }


        public void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho, int largura, int altura, int enderecoInicial = 0)
        {          
            byte[] rawIndexes = new byte[tamanho];
            Array.Copy(arquivo,enderecoInicial,rawIndexes, 0, tamanho);
            
            BitArray bitArray = new BitArray(rawIndexes);
            byte[] final = new byte[largura * altura];

            for (int i = 0; i < bitArray.Length; i++)
                final[i] = bitArray[i] == true ?(byte)1 : (byte)0;

            Indices = final;
        }

        public byte[] GereIndices(byte[] indices)
        {
            throw new NotImplementedException();
        }
    }
}
