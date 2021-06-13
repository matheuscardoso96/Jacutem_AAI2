using Jacutem_AAI2.Imagens.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Jacutem_AAI2.Imagens
{
    public class Ncgr
    {
        public byte[] Cabecalho { get; set; }
        public byte[] ImagemEmBytes { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public Bpp Bpp { get; set; }
        public int TamanhoGrafico { get; set; }
        public string DirNcgr { get; set; }
        public List<Color> CoresConvertidas { get; set; }
        public Ncer ArquivoNcer { get; set; }

        public Ncgr(string dir)
        {
            DirNcgr = dir;

            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                Cabecalho = br.ReadBytes(0x30);
                br.BaseStream.Position = 0x18;
                Altura = br.ReadInt16() * 8;
                Largura = br.ReadInt16() * 8;
                int bpp = br.ReadByte();
                if (bpp == 0x3)
                {
                    Bpp = Bpp.bpp4;
                }
                else
                {
                    Bpp = Bpp.bpp8;
                }
                br.BaseStream.Position = 0x28;
                TamanhoGrafico = br.ReadInt32();
                br.BaseStream.Position = 0x30;
                ImagemEmBytes = br.ReadBytes(TamanhoGrafico);
            }
        }

        public Ncgr(string dir, string dirNcer)
        {
            DirNcgr = dir;

            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                br.BaseStream.Position = 0x18;
                Altura = br.ReadInt16() * 8;
                Largura = br.ReadInt16() * 8;
                int bpp = br.ReadByte();
                if (bpp == 0x3)
                {
                    Bpp = Bpp.bpp4;
                }
                else
                {
                    Bpp = Bpp.bpp8;
                }
                br.BaseStream.Position = 0x28;
                TamanhoGrafico = br.ReadInt32();
                br.BaseStream.Position = 0x30;
                ImagemEmBytes = br.ReadBytes(TamanhoGrafico);
            }

            ArquivoNcer = new Ncer(dirNcer);
        }

        public Bitmap ExportarNCGR(string dirNclr)
        {

            Nclr nclr = new Nclr(dirNclr);

           

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = new List<Color>();
            cv.ConvertaPaleta(nclr.PaletaEmBytes);

            CoresConvertidas = cv.CoresPaleta;

            if (Bpp == Bpp.bpp4)
            {
                if (dirNclr.Contains("0125_idcom.nclr"))
                {
                    Largura = 48;
                    Altura = 24;
                }
                return cv.Exporte4bppTileRaw(Largura, Altura, ImagemEmBytes);
               // img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
                //img.Dispose();
            }
            else
            {
                return cv.Exporte8bppTileRaw(Largura, Altura, ImagemEmBytes);
               // img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
               // img.Dispose();
            }
        }

        public Bitmap ExportarNCGRTile(string dirNclr, string dirNscr)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);
            Nscr nscr = new Nscr(dirNscr);


            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = new List<Color>();
            cv.ConvertaPaleta(nclr.PaletaEmBytes);
            cv.TileMap = nscr.TileMap;

            CoresConvertidas = cv.CoresPaleta;

            if (Bpp == Bpp.bpp4)
            {
                return cv.Exporte4bppTile(Largura, Altura, ImagemEmBytes);
             //   img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
              //  img.Dispose();
            }
            else
            {
                return cv.Exporte8bppTile(Largura, Altura, ImagemEmBytes);
               // img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
              //  img.Dispose();
            }
        }

        public void ExportarNCGRSprites(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);
            ArquivoNcer = new Ncer(dirNcer);
            // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();

           

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
            cv.Paleta = nclr.PaletaEmBytes;
           

            foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
            {
                
                if (item.TabelaDeOams.Count > 0)
                {
                    cv.ExporteSprite(item.TabelaDeOams, fundoTransparente);
                }

                
                          
            }

           

            
        }

        public void ExportarNCGRSpriteModo2(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);
            Ncer ncer = new Ncer(dirNcer);

            ArquivoNcer = new Ncer(dirNcer);

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
            cv.Paleta = nclr.PaletaEmBytes;
            // int oo = 0;

            foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
            {
                if (item.TabelaDeOams.Count > 0)
                {
                    cv.ExporteSpriteModo2(item.TabelaDeOams, fundoTransparente);
                }
                             
            }



            /*    
                cv.CoresPaleta = new List<Color>();
                cv.ConvertaPaleta(nclr.PaletaEmBytes);
               // cv.TileMap = nscr.TileMap;

                if (Bpp == Bpp.bpp4)
                {
                    Bitmap img = cv.Exporte4bppTile(Largura, Altura, ImagemEmBytes);
                    img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
                    img.Dispose();
                }
                else
                {
                    Bitmap img = cv.Exporte8bppTile(Largura, Altura, ImagemEmBytes);
                    img.Save(dirSalvamento + "\\" + Path.GetFileName(DirNcgr.Replace("ncgr", "png")));
                    img.Dispose();
                }
                */
        }


        public void ImportaNCGRSemMap(string dirImg, string dirNclr)
        {

            //   Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = new List<Color>();
            cv.ConvertaPaleta(nclr.PaletaEmBytes);

            if (Bpp == Bpp.bpp4)
            {
                byte[] img = cv.Converta4bppRawNcgr(dirImg);
                InsiranoNcgr(img);
            }
            else
            {
                byte[] img = cv.Converta8bppRawNcgr(dirImg);
                InsiranoNcgr(img);
            }
        }

        public void ImportaNCGRComMap(string dirImg, string dirNclr, string dirNscr)
        {

            Nclr nclr = new Nclr(dirNclr);

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = new List<Color>();
            cv.ConvertaPaleta(nclr.PaletaEmBytes);

            if (Bpp == Bpp.bpp4)
            {
                List<byte[]> imgEhTilemap = cv.Converta4bppNcgrTileMap(dirImg);
                InsiranoNcgrEhNcsr(imgEhTilemap, dirNscr);
               // InsiraBytes(img);
            }
            else
            {
                List<byte[]> imgEhTilemap = cv.Converta8bppNcgrTileMap(dirImg);
                InsiranoNcgrEhNcsr(imgEhTilemap, dirNscr);
            }
        }

        public void ImportaNCGRSprites(string dirImg, string dirNclr, List<Oam> oams)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);
           // ArquivoNcer = new Ncer(dirNcer);
            // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
            cv.Paleta = nclr.PaletaEmBytes;            
            InsiranoNcgr(cv.InsiraSpriteNgcr(dirImg, oams));

        }

        public void ImportaNCGRSpritesModo2(string dirImg, string dirNclr, List<Oam> oams)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
            Nclr nclr = new Nclr(dirNclr);
            // ArquivoNcer = new Ncer(dirNcer);
            // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
            cv.Paleta = nclr.PaletaEmBytes;
            InsiranoNcgr(cv.InsiraSpriteNgcrModo2(dirImg, oams));

        }

        private void InsiranoNcgr(byte[] imgConv)
        {
            byte[] imgOg = File.ReadAllBytes(DirNcgr);
            MemoryStream ms = new MemoryStream(imgOg);
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.BaseStream.Position = 0x30;
                bw.Write(imgConv);
                imgOg = ms.ToArray();
            }

            File.WriteAllBytes(DirNcgr, imgOg);
        }

        private void InsiranoNcgrEhNcsr(List<byte[]> imgEhTilemap, string dirNcsr)
        {
            byte[] imgOg = File.ReadAllBytes(DirNcgr);
            MemoryStream ms = new MemoryStream(imgOg);
            GerarNovoNgcrParaOamAdicionado(DirNcgr, imgEhTilemap[1]);
          

            MemoryStream nscr = new MemoryStream(File.ReadAllBytes(dirNcsr));

            using (BinaryWriter bw = new BinaryWriter(nscr))
            {
                bw.BaseStream.Position = 0x24;
                bw.Write(imgEhTilemap[0]);
                File.WriteAllBytes(dirNcsr, nscr.ToArray());
               
            }

            
        }

        public void GerarNovoNgcrParaOamAdicionado(string dirSalvar, byte[] bytesSalvar)
        {
            byte[] novoNcgr = new byte[bytesSalvar.Length + 0x30];
            Array.Copy(Cabecalho, novoNcgr,Cabecalho.Length);
            Array.Copy(bytesSalvar,0, novoNcgr, 0x30,bytesSalvar.Length);
            BitArray myBA = new BitArray(BitConverter.GetBytes((int)bytesSalvar.Length));
            myBA.CopyTo(novoNcgr, 0x28);
            myBA = new BitArray(BitConverter.GetBytes((int)bytesSalvar.Length + 0x20));
            myBA.CopyTo(novoNcgr, 0x14);
            myBA = new BitArray(BitConverter.GetBytes((int)bytesSalvar.Length + 0x30));
            myBA.CopyTo(novoNcgr, 0x8);
            File.WriteAllBytes(dirSalvar, novoNcgr);
        }
    }
}
