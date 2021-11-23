using System.Drawing;
//using ImageLibGbaDS;
using LibDeImagensGbaDs.Enums;

namespace FormatosNitro.Imagens.FBtx
{
    public class TextureInfo
    {
        public int Index { get; set; }
        public int Params { get; set; }
        public int Offset { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Format { get; set; }
        public ColorDepth Bpp { get; set; }
        public int PaletteIndex { get; set; }
        public int ColorCount { get; set; }
        public string TextureName { get; set; }
        public Bitmap TextureImage { get; set; }
        public override string ToString()
        {
            return TextureName;
        }

    }

}
