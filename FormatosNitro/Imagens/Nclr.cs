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
        public Color[] Colors { get; set; }


        public Nclr(BinaryReader br, string diretorio) : base(br, diretorio)
        {
            if (Errors.Count == 0)
            {
                Pltt = new Pltt(br);
                if (SectionCount > 1)
                    Pcmp = new Pcmp(br);
            }
            
            
            br.Close();         

        }

        public void SalvarNclr()
        {
            MemoryStream novoNclr = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNclr))
            {
                base.EscreverPropiedades(bw);
                Pltt.EscreverPropiedades(bw);
                if (Pcmp != null)
                {
                    Pcmp.EscreverPropiedades(bw);
                }
                
            }

            File.WriteAllBytes(base.NitroFilePath, novoNclr.ToArray());
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

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoPcmp);
            bw.Write(QuatidadeDePaletas);
            bw.Write(Desconhecido);
            bw.Write(EnderecoIdPaletas);
            for (int i = 0; i < QuatidadeDePaletas; i++)
            {
                bw.Write(IdsDePaletas[i]);
            }


        }

    }
}
