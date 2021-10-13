using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using LibDeImagensGbaDs.Util;

namespace JacutemAAI2.WPF.Ferramentas.Internas
{
    public static class PaletteVisualGenerator
    {
        public static BitmapImage CreateImage(Color[] colors)
        {
            List<Bitmap> cls = new List<Bitmap>();
            Bitmap final = new Bitmap(128,128);
            for (int i = 0; i < colors.Length; i++)
            {
                byte[] indexes = new byte[64];
                Bitmap c = new Bitmap(8,8);
                Color[] color = new Color[] { colors[i]};
                c.PoulateBitmap(indexes, color);
                cls.Add(c);
            }

            using (Graphics g = Graphics.FromImage(final))
            {
                int count = 0;
                for (int y = 0; y < final.Height; y+=8)
                {
                    for (int x = 0; x < final.Width; x+=8)
                    {
                        if (count < cls.Count)
                        {
                            g.DrawImage(cls[count], x, y);
                            count++;
                        }
                        
                    }
                }
            }
            cls = null;
            return final.ToImageSource();


        }
    }
}
