using LibDeImagensGbaDs.Sprites;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNcer
{
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

   
}
