﻿using System;

namespace LibDeImagensGbaDs.Formatos.Indexado
{
    public class FA3I5 : IFormatoIndexado
    {
        public const int Bpp = 8;
        public byte[] Indices { get; set; }
        public byte[] AlphaValues { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public FA3I5(int largura, int altura)
        {
            Largura = largura;
            Altura = altura;
        }
       

        public void ObtenhaIndicesPorPixel(byte[] arquivo, int tamanho, int enderecoInicial = 0)
        {
            byte[] rawIndexes = new byte[tamanho];
            Array.Copy(arquivo, enderecoInicial, rawIndexes, 0, tamanho);

            byte[] final = new byte[Largura * Altura];
            AlphaValues = new byte[final.Length];

            for (int i = 0; i < rawIndexes.Length; i++)
            {
                int valorNaPaleta = rawIndexes[i] & 0x1F;
                int valorAlpha = rawIndexes[i] >> 5;
                valorAlpha = (valorAlpha * 4) + (valorAlpha / 2);
                valorAlpha = (valorAlpha * 255) / 31;
                AlphaValues[i] = (byte)valorAlpha;
                final[i] = (byte)valorNaPaleta;

            }


            Indices = final;
        }

        public byte[] GereIndices(byte[] indices)
        {
            throw new NotImplementedException();
        }
    }
}
