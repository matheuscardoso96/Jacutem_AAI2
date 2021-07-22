
using System.Collections.Generic;
using System.IO;

namespace Jacutem_AAI2.Imagens
{
    public class Nscr
    {
        public List<ushort> TileMap = new List<ushort>();
        public int TamanhoTileMap { get; set; }
        public string Diretorio { get; set; }

        public Nscr(string dir)
        {
            Diretorio = dir;
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                br.BaseStream.Position = 0x20;
                TamanhoTileMap = br.ReadInt32();
                br.BaseStream.Position = 0x24;
                int contador = 0;
                while (contador < TamanhoTileMap) 
                {
                    TileMap.Add(br.ReadUInt16());
                    contador += 2; 
                }
                    
            }
        }
    }
}
