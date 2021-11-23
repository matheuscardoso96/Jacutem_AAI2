using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNcer
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

   
}
