using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Paleta
{
    public class BGR565 : IFormatoPaleta
    {
        public Color[] Paleta { get; set; }
        public BGR565(byte[] bytesDaPaleta)
        {
            Paleta = new Color[bytesDaPaleta.Length / 2];
            
            using (BinaryReader br = new BinaryReader(new MemoryStream(bytesDaPaleta)))
            {
                int contador = 0;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    int bgr = br.ReadInt16();
                    int r = (bgr & 31) * 255 / 31;
                    int g = (bgr >> 5 & 31) * 255 / 31;
                    int b = (bgr >> 10 & 31) * 255 / 31;
                    Paleta[contador] = Color.FromArgb(r, g, b);
                    contador++;
                }
            }

         
        }
    }
}
