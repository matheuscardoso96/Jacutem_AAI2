using JacutemAAI2.WPF.Imagens.Enums;
using LibDeImagensGbaDs.Enums;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Jacutem_AAI2.Imagens
{
    public class Nclr
    {
        public CabecalhoNclr CabecalhoNclr { get; set; }
        public Pltt Pltt { get; set; }
        public Pcmp Pcmp { get; set; }     
        public List<byte[]> Paletas { get; set; }
        public string Diretorio { get; set; }
        public EIndexFormat BppPaleta { get; set; }

        public Nclr(string dir, EIndexFormat bpp)
        {
            BppPaleta = bpp;
            Diretorio = dir;
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                CabecalhoNclr = new CabecalhoNclr(br);
                Pltt = new Pltt(br);
                if (CabecalhoNclr.QuantidadeDeSecoes > 1)
                {
                    Pcmp = new Pcmp(br);
                }
                            
                //ConvertaPaleta(Pltt);
            }
        }

        public void ConvertaPaleta(Pltt pltt)
        {
            
        
            using (BinaryReader br = new BinaryReader(new MemoryStream(pltt.Paleta)))
            {          

               
               Paletas.Add(br.ReadBytes((int)(pltt.TamanhoPaleta)));
                
            }
        }
    }

    public class CabecalhoNclr
    {
        public string Id { get; set; }
        public ushort OrdemDosBytes { get; set; }
        public ushort Versao { get; set; }
        public uint TamanhoDoArquivo { get; set; }
        public ushort EnderecoPLTT { get; set; }
        public ushort QuantidadeDeSecoes { get; set; }

        public CabecalhoNclr(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            OrdemDosBytes = br.ReadUInt16();
            Versao = br.ReadUInt16();
            TamanhoDoArquivo = br.ReadUInt32();
            EnderecoPLTT = br.ReadUInt16();
            QuantidadeDeSecoes = br.ReadUInt16();
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
