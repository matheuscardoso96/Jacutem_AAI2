using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Nclr : NitroBase
    {
        public Pltt Pltt { get; set; }
        public Pcmp Pcmp { get; set; }
        public List<byte[]> Paletas { get; set; }
        public string Diretorio { get; set; }


        public Nclr(string dir, BinaryReader br) : base(br)
        {
            Diretorio = dir;
            Pltt = new Pltt(br);
            if (QuantidadeDeSecoes > 1)
                Pcmp = new Pcmp(br);
            
            br.Close();

        }
    }


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

    }

    public class Pcmp
    {
        public string Id { get; set; }
        public uint TamanhoPcmp { get; set; }
        public ushort QuatidadeDePaletas { get; set; }
        public ushort Desconhecido { get; set; }
        public uint EnderecoIdPaletas { get; set; }
        public ushort[] IdsDePaletas { get; set; }

        public Pcmp(BinaryReader br)
        {

            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoPcmp = br.ReadUInt32();
            QuatidadeDePaletas = br.ReadUInt16();
            Desconhecido = br.ReadUInt16();
            EnderecoIdPaletas = br.ReadUInt16();
            IdsDePaletas = new ushort[QuatidadeDePaletas];
            for (int i = 0; i < QuatidadeDePaletas; i++)
            {
                IdsDePaletas[i] = br.ReadUInt16();
            }
        }

    }
}
