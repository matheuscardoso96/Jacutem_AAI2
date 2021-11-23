using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNcgr
{
    public class Cpos
    {
        public string Id { get; set; }
        public uint TamanhoCpos { get; set; }
        public uint Padding { get; set; }
        public ushort TamanhoDeTile{ get; set; }
        public ushort QuatidadeDeTiles { get; set; }

        public Cpos(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoCpos = br.ReadUInt32();
            Padding = br.ReadUInt32();
            TamanhoDeTile = br.ReadUInt16();
            QuatidadeDeTiles = br.ReadUInt16();
        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoCpos);
            bw.Write(Padding);
            bw.Write(TamanhoDeTile);
            bw.Write(QuatidadeDeTiles);
        }
    }
}
