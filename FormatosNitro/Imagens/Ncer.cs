using LibDeImagensGbaDs.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Ncer : NitroBase
    {
        public Cebk Cebk { get; set; }
        public Labl Labl { get; set; }
        public Uext Uext { get; set; }

        public Ncer(BinaryReader br, string diretorio) : base(br, diretorio)
        {
            if (Errors.Count == 0)
            {
                Cebk = new Cebk(br);
                if (SectionCount > 1)
                {
                    Labl = new Labl(br, Cebk.QuatidadeEntradasDeBeks);
                    Uext = new Uext(br);
                }
            }
            
            br.Close();
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

        public void SalvarNcer()
        {
            MemoryStream novoNcer = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNcer))
            {
                EscreverPropiedades(bw);
                Cebk.EscreverPropiedades(bw);
                Labl.EscreverPropiedades(bw);
                Uext.EscreverPropiedades(bw);
            }

            File.WriteAllBytes(NitroFilePath, novoNcer.ToArray());
        }
    }

    public class Cebk
    {
        public string Id { get; set; }
        public uint TamanhoCebk { get; set; }
        public ushort QuatidadeEntradasDeBeks{ get; set; }
        public ushort TamanhoEntradaBek{ get; set; }
        public uint EnderecoBeks { get; set; }
        public uint TileBoundary { get; set; } //TileBoundary * 64
        public uint EnderecoDeParticaoDeDados { get; set; }
        public ulong Padding { get; set; }

        public List<Ebk> Ebks { get; set; }

        public Cebk(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoCebk = br.ReadUInt32();
            QuatidadeEntradasDeBeks = br.ReadUInt16();
            TamanhoEntradaBek = br.ReadUInt16();
            EnderecoBeks = br.ReadUInt32();
            TileBoundary = br.ReadUInt32();
            EnderecoDeParticaoDeDados = br.ReadUInt32();
            Padding = br.ReadUInt64();
            br.BaseStream.Position = EnderecoBeks + 8 + 16; //16 tamanho do cabeçalho + 8 padding
            LerEbks(br);
        }

        private void LerEbks(BinaryReader br) 
        {
            Ebks = new List<Ebk>();
            for (int i = 0; i < QuatidadeEntradasDeBeks; i++)
            {
                Ebk ebk = new Ebk();
                ebk.Id = i;
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

            foreach (Ebk ebk in Ebks)
            {
                ebk.Oams = new List<Oam>();

                for (int i = 0; i < ebk.QuantidadeDeCelulas; i++)
                {
                    ebk.Oams.Add(new Oam(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                }

                ebk.Oams.Reverse();
            }
    

        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoCebk);
            bw.Write(QuatidadeEntradasDeBeks);
            bw.Write(TamanhoEntradaBek);
            bw.Write(EnderecoBeks);
            bw.Write(TileBoundary);
            bw.Write(EnderecoDeParticaoDeDados);
            bw.Write(Padding);
            foreach (var ebk in Ebks)
            {
                bw.Write(ebk.QuantidadeDeCelulas);
                bw.Write(ebk.InfoSomenteLeituraCelula);
                bw.Write(ebk.EnderecoCelula);
                //bw.Write(ebk.TamanhoParticao);

                if (TamanhoEntradaBek == 1)
                {
                    bw.Write(ebk.LarguraMaxima);
                    bw.Write(ebk.AlturaMaxima);
                    bw.Write(ebk.LarguraMinima);
                    bw.Write(ebk.AlturaMinima);
                }
               
            }

            foreach (var ebk in Ebks)
            {
                foreach (var item in ebk.Oams)
                {
                    bw.Write(item.OBJ0Attributes);
                    bw.Write(item.OBJ1Attributes);
                    bw.Write(item.OBJ2Attributes);
                }

            }

        }

        
    }

    public class Ebk
    {
        public int Id { get; set; }
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

        public override string ToString()
        {
            return $"{Id}";
        }
    }

    public class Labl
    {
        public string Id { get; set; }
        public uint TamanhoLabl { get; set; }
        public List<uint> Enderecos { get; set; }
        public List<string> Labls { get; set; }

        public Labl(BinaryReader br, int quantidadeDeBanks)
        {
            byte IsZero = br.ReadByte();

            if (IsZero == 0)
            {
                while (IsZero == 0)
                {
                   IsZero = br.ReadByte();
                }
            }

            br.BaseStream.Position--;

            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (Id != "LBAL")
            {
                throw new Exception("Cade o LABL?");
            }

            TamanhoLabl = br.ReadUInt32();

            Enderecos = new List<uint>();
            uint offset = br.ReadUInt32();
            while (offset < br.BaseStream.Length)
            {
                Enderecos.Add(offset);
                offset = br.ReadUInt32();
            }

            br.BaseStream.Position -= 4;
            Labls = new List<string>();

            long enderecoBase = br.BaseStream.Position;

            for (int i = 0; i < Enderecos.Count; i++)
            {
                br.BaseStream.Position = enderecoBase + Enderecos[i];
                StringBuilder label = new StringBuilder();
                while (true)
                {
                    char[] letra = Encoding.ASCII.GetChars(br.ReadBytes(1));
                    if (letra[0] == '\0')
                        break;
                    
                    label.Append(letra);
                }

                Labls.Add(label.ToString());
            }

        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoLabl);
            foreach (var index in Enderecos)
            {
                bw.Write(index);
            }
            foreach (var label in Labls)
            {
                bw.Write(Encoding.ASCII.GetBytes(label));
                bw.Write((byte)0);
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

        internal void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoUext);
            bw.Write(Padding);
        }
    }

   
}
