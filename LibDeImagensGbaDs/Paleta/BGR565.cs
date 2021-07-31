using System;
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


        public byte ObtenhaIndexCorMaisProxima(Color c1)
        {
            int eOmenor = 10000;
            int index = 0;

            for (int x = 0; x < Cores.Length; x++)
            {
                Color c2 = Cores[x];

                int vl = DiferencaDeCores(c1, c2);

                if (eOmenor > vl)
                {
                    eOmenor = vl;
                    index = x;
                }

            }

            return (byte)index;
        }

        private int DiferencaDeCores(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R) + (c1.G - c2.G) * (c1.G - c2.G) + (c1.B - c2.B) * (c1.B - c2.B) + (c1.A - c2.A) * (c1.A - c2.A));
        }
    }
}
