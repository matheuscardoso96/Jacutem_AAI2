using LibDeImagensGbaDs.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.Images
{
    public static class SpriteHelpers
    {
        public static Bitmap MarkSelectOam(Bitmap image, Oam oam)
        {
            Bitmap copy = image.Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
            using (Graphics g = Graphics.FromImage(copy))
            {
                Rectangle rect = new Rectangle((int)oam.X, (int)oam.Y,(int)oam.Width, (int)oam.Height);
                Pen pen = new Pen(Color.Red, 2);
                g.DrawRectangle(pen, rect);
            }
            copy.MakeTransparent();
            return copy;
        }
    }
}
