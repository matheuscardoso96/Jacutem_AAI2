﻿
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Nscr : NitroBase
    {
        public Scrn Scrn { get; set; }
       
        public Nscr(BinaryReader br, string diretorio) : base(br, diretorio)
        {
            if (Errors.Count == 0)
            {
                Scrn = new Scrn(br);
            }
            
            br.Close();

        }

        public void SalvarNscr() 
        {
            MemoryStream novoNscr = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNscr))
            {
                base.EscreverPropiedades(bw);
                Scrn.EscreverPropiedades(bw);
               
            }

            File.WriteAllBytes(base.NitroFilePath, novoNscr.ToArray());
        }
    }

    public class Scrn
    {
        public string Id { get; set; }
        public uint TamanhoDoScrn { get; set; }
        public ushort Largura { get; set; }
        public ushort Altura { get; set; }       
        public int Padding { get; set; }
        public uint TamanhoInfosTela { get; set; }
        public ushort[] InfoTela { get; set; }

        public Scrn(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoDoScrn = br.ReadUInt32();
            Largura = br.ReadUInt16();
            Altura = br.ReadUInt16();
            Padding = br.ReadInt32();
            TamanhoInfosTela = br.ReadUInt32();
            InfoTela = new ushort[TamanhoInfosTela / 2];
            int contador = 0;
            for (int i = 0; i < TamanhoInfosTela; i+=2)
            {
                InfoTela[contador] = br.ReadUInt16();
                contador++;
            }
            
            
        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(Id.Length + InfoTela.Length * 2 + 16 );
            bw.Write(Largura);
            bw.Write(Altura);
            bw.Write(Padding);
            bw.Write(InfoTela.Length * 2);
            foreach (var info in InfoTela)
                bw.Write(info);
                   
           
        }

    }
}
