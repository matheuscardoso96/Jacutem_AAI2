using System;
using System.Drawing;
using System.IO;

namespace LibDeImagensGbaDs.Paleta
{
    public class BGR565 : IPalette
    {
        public Color[] Colors { get; set; }
        public BGR565(byte[] palette)
        {
            Colors = new Color[palette.Length / 2];
            
            using (BinaryReader br = new BinaryReader(new MemoryStream(palette)))
            {
                int counter = 0;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    int bgr = br.ReadInt16();
                    int r = (bgr & 31) * 255 / 31;
                    int g = (bgr >> 5 & 31) * 255 / 31;
                    int b = (bgr >> 10 & 31) * 255 / 31;
                    Colors[counter] = Color.FromArgb(r, g, b);
                    counter++;
                }
            }

         
        }


        public byte GetNearColorIndex(Color c1)
        {
            int tolerance  = 10000;
            int index = 0;

            for (int x = 0; x < Colors.Length; x++)
            {
                Color c2 = Colors[x];

                int vl = ColorDiff(c1, c2);

                if (tolerance  > vl)
                {
                    tolerance  = vl;
                    index = x;
                }

            }

            return (byte)index;
        }

        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R) + (c1.G - c2.G) * (c1.G - c2.G) + (c1.B - c2.B) * (c1.B - c2.B) + (c1.A - c2.A) * (c1.A - c2.A));
        }
    }
}
