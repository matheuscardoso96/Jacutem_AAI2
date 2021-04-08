using System.IO;

namespace Jacutem_AAI2.Imagens
{
    public class Nscr
    {
        public byte[] TileMap { get; set; }
        public int TamanhoTileMap { get; set; }

        public Nscr(string dir)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                br.BaseStream.Position = 0x20;
                TamanhoTileMap = br.ReadInt32();
                br.BaseStream.Position = 0x24;
                TileMap = br.ReadBytes(TamanhoTileMap);
            }
        }
    }
}
