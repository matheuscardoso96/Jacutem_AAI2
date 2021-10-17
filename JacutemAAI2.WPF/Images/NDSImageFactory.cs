using FormatosNitro.Imagens;
using System.IO;

namespace JacutemAAI2.WPF.Images
{
    public static class NDSImageFactory
    {
        public static Ncgr LoadNgcr(string argumentosImg)
        {
            string[] argSplit = argumentosImg.Split(',');
            
            BinaryReader leitorNclr = new BinaryReader(File.OpenRead(argSplit[1]));
            Nclr nclr = new Nclr(leitorNclr, argSplit[1]);
            
            BinaryReader leitorNgcr = new BinaryReader(File.OpenRead(argSplit[0]));         
            Ncgr ncgr;
          

            if (argumentosImg.Contains(".nscr"))
            {
                BinaryReader leitorNscr = new BinaryReader(File.OpenRead(argSplit[2]));
                Nscr nscr = new Nscr(leitorNscr, argSplit[2]);
                ncgr = new Ncgr(leitorNgcr, nclr, nscr, argSplit[0],argSplit[3]);

            }
            else if (argumentosImg.Contains(".ncer"))
            {
                BinaryReader leitorNscer = new BinaryReader(File.OpenRead(argSplit[2]));
                Ncer ncer = new Ncer(leitorNscer, argSplit[2]);
                ncgr = new Ncgr(leitorNgcr, nclr, ncer, argSplit[0],argSplit[3]);
            }
            else
            {
                ncgr = new Ncgr(leitorNgcr, nclr, argSplit[0], argSplit[2]);

            }

            return ncgr;
        }


        public static Btx LoadBtx(string argumentosImg)
        {
            return new Btx(argumentosImg);
        }
    }
}
