using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNcgr
{
    public class Char
    {
        public string Id { get; set; }
        public uint TamanhoChar { get; set; }
        public ushort QuatidadeDeTilesY { get; set; }
        public ushort QuatidadeDeTilesX { get; set; }
        public uint IntensidadeDeBits { get; set; }
        public ushort Desconhecido0 { get; set; }
        public ushort Desconhecido1 { get; set; }
        public uint FlagDeDimensao { get; set; }
        public uint TamanhoTilesEmBytes { get; set; }
        public uint Desconhecido2 { get; set; }
        public byte[] Tiles { get; set; }

        public Char(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoChar = br.ReadUInt32();
            QuatidadeDeTilesY = br.ReadUInt16();
            QuatidadeDeTilesX = br.ReadUInt16();
            IntensidadeDeBits = br.ReadUInt32();
            Desconhecido0 = br.ReadUInt16();
            Desconhecido1 = br.ReadUInt16();
            FlagDeDimensao = br.ReadUInt32();
            TamanhoTilesEmBytes = br.ReadUInt32();
            Desconhecido2 = br.ReadUInt32();
            Tiles = br.ReadBytes((int)TamanhoTilesEmBytes);

        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(32 + TamanhoTilesEmBytes);
            bw.Write(QuatidadeDeTilesY);
            bw.Write(QuatidadeDeTilesX);
            bw.Write(IntensidadeDeBits);
            bw.Write(Desconhecido0);
            bw.Write(Desconhecido1);
            bw.Write(FlagDeDimensao);
            bw.Write(TamanhoTilesEmBytes);
            bw.Write(Desconhecido2);
            bw.Write(Tiles);
        }

    }
}
