using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Formatos
{
    public class F8BPP
    {
        private const int _bpp = 8;
        public byte[] GetIndexesFromFile(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * altura) * _bpp) / 8;
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);         

            return rawIndexes;
        }
    }
}
