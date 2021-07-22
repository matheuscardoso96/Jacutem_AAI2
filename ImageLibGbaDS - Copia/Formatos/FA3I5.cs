using System.IO;

namespace ImageLibGbaDS.Formatos
{
    public class FA3I5 : IFormatoIndexado
    {
        private const int _bpp = 8;
        public byte[] AlphaValues { get; private set; }

        public byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int largura, int altura, int enderecoInicial = 0)
        {
            int totalBytes = ((largura * largura) * _bpp) / 8;
            arquivo.BaseStream.Position = enderecoInicial;
            byte[] rawIndexes = arquivo.ReadBytes(totalBytes);

            byte[] final = new byte[largura * largura];
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


            return final;
        }
    }
}
