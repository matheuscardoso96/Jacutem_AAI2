using System.Collections;
using System.IO;

namespace ImageLibGbaDS.Formatos
{
    public class F1BPP : IFormatoIndexado
    {
        private const int _bpp = 1;
        public byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * altura) * _bpp ) / 8;           
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);
            
            BitArray bitArray = new BitArray(rawIndexes);
            byte[] final = new byte[largura * altura];

            for (int i = 0; i < bitArray.Length; i++)
                final[i] = bitArray[i] == true ?(byte)1 : (byte)0;

            return final;
        }
    }
}
