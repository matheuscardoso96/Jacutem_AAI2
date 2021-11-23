using System.Drawing;
//using ImageLibGbaDS;

namespace FormatosNitro.Imagens.FBtx
{
    public class PaletteInfo
    {
        public int Offset { get; set; }
        public string PaletteName { get; set; }
        public Color[] Palette { get; set; }
        public byte[] PaletteBytes { get; set; }

    }

}
