using FormatosNitro.Imagens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDeImagensGbaDs.Conversor;
using System.IO;

namespace JacutemAAI2.WPF.Imagens
{
    public static class GerenciadorConversaoImagens
    {
        public static Ncgr CarregarNcgr(string argumentosImg) 
        {
            string[] argSplit = argumentosImg.Split(',');
            
            BinaryReader leitorNclr = new BinaryReader(File.OpenRead(argSplit[1]));
            Nclr nclr = new Nclr(leitorNclr);
            
            BinaryReader leitorNgcr = new BinaryReader(File.OpenRead(argSplit[0]));         
            Ncgr ncgr;

         
            if (argumentosImg.Contains(".nscr"))
            {
                BinaryReader leitorNscr = new BinaryReader(File.OpenRead(argSplit[2]));
                Nscr nscr = new Nscr(leitorNscr);
                ncgr = new Ncgr(leitorNgcr, nclr, nscr);

            }
            else if (argumentosImg.Contains(".ncer"))
            {
                BinaryReader leitorNscer = new BinaryReader(File.OpenRead(argSplit[2]));
                Ncer ncer = new Ncer(leitorNscer);
                ncgr = new Ncgr(leitorNgcr, nclr, ncer);
            }
            else
            {
                ncgr = new Ncgr(leitorNgcr, nclr);
               
            }
            
            return ncgr;
        }
    }
}
