using System.Collections;
using System.IO;

namespace ImageLibGbaDS.Formatos
{
    public class F2BPP : IFormatoIndexado
    {
        private const int _bpp = 2;
        public byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * altura) * _bpp) / 8;
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);

            BitArray bitArray = new BitArray(rawIndexes);
            byte[] final = new byte[largura * altura];
            
            int contador = 0;
            for (int i = 0; i < bitArray.Length; i += 2) 
            {
                int valor1 = bitArray[i] ? 1 : 0;
                int valor2 = bitArray[i + 1] ? 1 : 0;
                int valorFinal = (valor2 << 1) | valor1;
                final[contador] = (byte)valorFinal;
                contador++;
            }
                

            return final;
        }
    }
}
