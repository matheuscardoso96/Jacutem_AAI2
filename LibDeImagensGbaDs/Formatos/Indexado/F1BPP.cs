﻿using System;
using System.Collections;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class F1BPP : IFormatoIndexado
    {
        public const int Bpp = 1;
        public byte[] AlphaValues { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public byte[] Indices { get; set; }

        public F1BPP(int largura, int altura)
        {
            Largura = largura;
            Altura = altura;
        }

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho,int enderecoInicial = 0)
        {          
            byte[] rawIndexes = new byte[tamanho];
            Array.Copy(arquivo,enderecoInicial,rawIndexes, 0, tamanho);
            
            BitArray bitArray = new BitArray(rawIndexes);
            byte[] final = new byte[Largura * Altura];

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
