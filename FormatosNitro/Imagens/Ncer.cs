using LibDeImagensGbaDs.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Ncer : NitroBase
    {
        Cebk Cebk { get; set; }
        Labl Labl { get; set; }
        public Uext Uext { get; set; }

        public Ncer(BinaryReader br) : base(br)
        {
            Cebk = new Cebk(br);
            if (base.QuantidadeDeSecoes > 1)
            {
                Labl = new Labl(br, Cebk.QuatidadeEntradasDeBeks);
                Uext = new Uext(br);
            }
           
            

            
        }

        public void SalvarNcer(string diretorio)
        {
           

           
        }

        private void AtualizarPosicoesXYdeValoresOam()
        {
           
        }

        public void SalvarNcerdicionando(string diretorio)
        {

        }

        private int TamanhoQtdDeEntradas()
        {
            

            return 0;
        }
    }

    public class Cebk 
    {
        public string Id { get; set; }
        public uint TamanhoCebk { get; set; }
        public ushort QuatidadeEntradasDeBeks{ get; set; }
        public ushort TamanhoEntradaBek{ get; set; }
        public uint EnderecoBenks { get; set; }
        public uint LimiteAreaBek { get; set; }
        public uint EnderecoDeParitcaoDeDados { get; set; }
        public ulong Padding { get; set; }

        public List<Ebk> Ebks { get; set; }
        

        public Cebk(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoCebk = br.ReadUInt32();
            QuatidadeEntradasDeBeks = br.ReadUInt16();
            TamanhoEntradaBek = br.ReadUInt16();
            EnderecoBenks = br.ReadUInt32();
            LimiteAreaBek = br.ReadUInt32();
            EnderecoDeParitcaoDeDados = br.ReadUInt32();
            Padding = br.ReadUInt64();
            br.BaseStream.Position = EnderecoBenks + 8 + 16; //16 tamanho do cabeçalho + 8 padding
            LerEbks(br);
            

        }

        private void LerEbks(BinaryReader br) 
        {
            Ebks = new List<Ebk>();
            for (int i = 0; i < QuatidadeEntradasDeBeks; i++)
            {
                Ebk ebk = new Ebk();
                ebk.QuantidadeDeCelulas = br.ReadUInt16();
                ebk.InfoSomenteLeituraCelula = br.ReadUInt16();
                ebk.EnderecoCelula = br.ReadUInt32();

                if (TamanhoEntradaBek == 1)
                {
                    ebk.LarguraMaxima = br.ReadInt16();
                    ebk.AlturaMaxima = br.ReadInt16();
                    ebk.LarguraMinima = br.ReadInt16();
                    ebk.AlturaMinima = br.ReadInt16();

                }

                Ebks.Add(ebk);
               
            }

            long enderecoBase = br.BaseStream.Position;

            foreach (Ebk ebk  in Ebks)
            {
                ebk.Oams = new List<Oam>();

                for (int i = 0; i < ebk.QuantidadeDeCelulas; i++)
                {
                    ebk.Oams.Add(new Oam(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                }
            }

        }

    }

    public class Ebk 
    {
        public ushort QuantidadeDeCelulas { get; set; }
        public ushort InfoSomenteLeituraCelula { get; set; }
        public uint EnderecoCelula { get; set; }
        public uint EnderecoParticao { get; set; }
        public uint TamanhoParticao { get; set; }
        public short LarguraMaxima { get; set; }
        public short AlturaMaxima { get; set; }
        public short LarguraMinima { get; set; }
        public short AlturaMinima { get; set; }
        public List<Oam> Oams { get; set; }
    }

    public class Labl 
    {
        public string Id { get; set; }
        public uint TamanhoLabl { get; set; }
        public List<uint> Enderecos { get; set; }
        public List<string> Labls { get; set; }

        public Labl(BinaryReader br, int quantidadeDeBanks)
        {
            
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (Id != "LABL")
            {
                throw new Exception("Cade o LABL?");
            }
            
            TamanhoLabl = br.ReadUInt32();

            Enderecos = new List<uint>();

            for (int i = 0; i < quantidadeDeBanks; i++)
            {
                Enderecos.Add(br.ReadUInt32());
            }

            Labls = new List<string>();

            long enderecoBase = br.BaseStream.Position;

            for (int i = 0; i < quantidadeDeBanks; i++)
            {
                br.BaseStream.Position = enderecoBase + Enderecos[i];
                StringBuilder label = new StringBuilder();
                while (true)
                {
                    char[] letra = Encoding.ASCII.GetChars(br.ReadBytes(1));
                    if (letra[0] == '0')
                        break;
                    
                    label.Append(letra);
                }

                Labls.Add(label.ToString());
            }

        }
    }

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
    }

   
}
