using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Formatos
{
    public class FA5I3 : IFormatoIndexado
    {
        private const int _bpp = 8;
        public byte[] AlphaValues { get; private set; }
        public byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * altura) * _bpp) / 8;
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);

            byte[] final = new byte[largura * altura];
            AlphaValues = new byte[final.Length];

            for (int i = 0; i < rawIndexes.Length; i++)
            {
                int valorNaPaleta = rawIndexes[i] & 7;
                int valorAlpha = rawIndexes[i] >> 3;
                valorAlpha = (valorAlpha * 255) / 31;

                AlphaValues[i] = (byte)valorAlpha;
                final[i] = (byte)valorNaPaleta;

            }


            return final;
        }
    }
}
