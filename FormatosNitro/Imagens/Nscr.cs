
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Nscr : NitroBase
    {
        public Scrn Scrn { get; set; }
       
        public Nscr(string dir, BinaryReader br) : base(br)
        {
            Scrn = new Scrn(br);
            br.Close();

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
       
    }
}
