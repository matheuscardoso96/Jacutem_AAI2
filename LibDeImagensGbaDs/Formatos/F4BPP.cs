using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Formatos
{
    public class F4BPP : IFormatoIndexado
    {

        private const int _bpp = 4;
        public byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * altura) * _bpp) / 8;
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);

            byte[] final = new byte[largura * altura];

            int contador = 0;
            for (int i = 0; i < rawIndexes.Length; i++)
            {
                int nibbleAlto = (rawIndexes[i] & 0xF0) >> 4;
                int nibbleBaixo = (rawIndexes[i] & 0x0F);
                final[contador] = (byte)nibbleBaixo;
                final[contador + 1] = (byte)nibbleAlto;
                contador+=2;

            }


            return final;
        }

    }
}
