﻿using System;
using System.Collections;

namespace LibDeImagensGbaDs.Formats.Indexed
{
    public class F1BPP : IIndexedFormat
    {
        public const int Bpp = 1;
        public byte[] AlphaValues { get; set; }


        public byte[] DecompressIndexes(byte[] arquivo, ref byte[] alphaValues)
        {          
            
            BitArray bitArray = new BitArray(arquivo);
            byte[] final = new byte[arquivo.Length * 8];

            for (int i = 0; i < bitArray.Length; i++)
                final[i] = bitArray[i] == true ?(byte)1 : (byte)0;

            return final;
        }

        public byte[] CompressIndexes(byte[] indices)
        {
            throw new NotImplementedException();
        }
    }
}
