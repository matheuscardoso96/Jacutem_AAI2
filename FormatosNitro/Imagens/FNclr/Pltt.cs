using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNclr
{
    public class Pltt
    {
        public string Id { get; set; }
        public uint TamanhoDoPltt { get; set; }
        public uint IntensidadeDeBits { get; set; }
        public int Padding { get; set; }
        public uint TamanhoPaleta { get; set; }
        public uint EnderecoPaleteData { get; set; }
        public byte[] Paleta { get; set; }

        public Pltt(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoDoPltt = br.ReadUInt32();
            IntensidadeDeBits = br.ReadUInt32();
            Padding = br.ReadInt32();
            TamanhoPaleta = br.ReadUInt32();
            EnderecoPaleteData = br.ReadUInt32();
            Paleta = br.ReadBytes((int)TamanhoPaleta);
        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(Paleta.Length + 24);
            bw.Write(IntensidadeDeBits);
            bw.Write(Padding);
            bw.Write(TamanhoPaleta);
            bw.Write(EnderecoPaleteData);
            bw.Write(Paleta);


        }

    }
}
