using System.Drawing;
using System.IO;

namespace ImageLibGbaDS.Paleta
{
    public class BGR565 : IPaleta
    {
        public Color[] Cores { get; set; }
        public bool TemAlpha { get; set; }
        public BGR565(byte[] bytesDaPaleta, bool temAlpha)
        {
            TemAlpha = temAlpha;
            Cores = new Color[bytesDaPaleta.Length / 2];
            
            using (BinaryReader br = new BinaryReader(new MemoryStream(bytesDaPaleta)))
            {
                int contador = 0;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    int bgr = br.ReadInt16();
                    int r = (bgr & 31) * 255 / 31;
                    int g = (bgr >> 5 & 31) * 255 / 31;
                    int b = (bgr >> 10 & 31) * 255 / 31;
                    Cores[contador] = Color.FromArgb(r, g, b);
                    contador++;
                }
            }

         
        }
    }
}
