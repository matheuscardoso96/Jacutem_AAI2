using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNcer
{
    public class Uext 
    {
        public string Id { get; set; }
        public uint TamanhoUext { get; set; }
        public uint Padding { get; set; }
        public Uext(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoUext = br.ReadUInt32();
            Padding = br.ReadUInt32();
        }

        internal void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoUext);
            bw.Write(Padding);
        }
    }

   
}
