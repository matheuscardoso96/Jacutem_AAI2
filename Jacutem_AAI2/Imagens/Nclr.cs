using Jacutem_AAI2.Imagens.Enums;
using LibDeImagensGbaDs.Enums;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace Jacutem_AAI2.Imagens
{
    public class Nclr
    {
        public int TamanhoPaleta { get; set; }
        public List<List<Color>> Paletas { get; set; }
        public byte[] PaletasByte { get; set; }
        public string Diretorio { get; set; }
        public EIndexFormat BppPaleta { get; set; }

        public Nclr(string dir, EIndexFormat bpp)
        {
            BppPaleta = bpp;
            Diretorio = dir; 
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                br.BaseStream.Position = 0x20;
                TamanhoPaleta = br.ReadInt32();
                br.BaseStream.Position = 0x28;
             
                Paletas = new List<List<Color>>();
                ConvertaPaleta(br.ReadBytes(TamanhoPaleta));
            }
        }

        public void ConvertaPaleta(byte[] paleta)
        {
            List<Color> paletaa = new List<Color>();
            PaletasByte = paleta;

            using (BinaryReader br = new BinaryReader(new MemoryStream(paleta)))
            {
                int contador = 0;
                while (br.BaseStream.Position < paleta.Length)
                {
                    int bgr = br.ReadInt16();
                 

                    int r = (bgr & 31) * 255 / 31;
                    int g = (bgr >> 5 & 31) * 255 / 31;
                    int b = (bgr >> 10 & 31) * 255 / 31;
                    paletaa.Add(Color.FromArgb(r, g, b));
                    contador += 2;
                    if (BppPaleta == EIndexFormat.F4BBP && contador == 32 || BppPaleta == EIndexFormat.F8BBP && contador == 512)
                    {
     
                        Paletas.Add(paletaa);
                        paletaa = new List<Color>();
                      
                        contador = 0;
                    }
                    else if (br.BaseStream.Position == paleta.Length) 
                    {
                        Paletas.Add(paletaa);
                      
                    }
                        


                }
            }
        }
    }
}
