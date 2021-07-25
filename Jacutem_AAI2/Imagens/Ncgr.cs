using Jacutem_AAI2.Imagens.Enums;
using LibDeImagensGbaDs.Conversor;
using LibDeImagensGbaDs.Enums;
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
        public EIndexFormat Bpp { get; set; }
        public int TamanhoGrafico { get; set; }
        public string DirNcgr { get; set; }
        public List<Color> CoresConvertidas { get; set; }
        public Ncer ArquivoNcer { get; set; }
        public Nscr ArquivoNscr { get; set; }
        public Nclr ArquivoNclr { get; set; }
        public string Tipo { get; set; }
        public Bitmap Imagem { get; set; }
        public string LocalParaExportacao { get; set; }
        public bool EhSprite { get; set; } = false;


        public Ncgr(string args, string tipo)
        {
            string[] argumentos = args.Split(','); 
            DirNcgr = argumentos[0];
            Tipo = tipo;
            LocalParaExportacao = argumentos[argumentos.Length - 1];

            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(argumentos[0]))))
            {
                Cabecalho = br.ReadBytes(0x30);
                br.BaseStream.Position = 0x18;
                Altura = br.ReadInt16() * 8;
                Largura = br.ReadInt16() * 8;
                int bpp = br.ReadByte();
                if (bpp == 0x3)
                    Bpp = EIndexFormat.F4BBP;
                else
                    Bpp = EIndexFormat.F8BBP;

                br.BaseStream.Position = 0x28;
                TamanhoGrafico = br.ReadInt32();
                br.BaseStream.Position = 0x30;
                ImagemEmBytes = br.ReadBytes(TamanhoGrafico);
            }
            
            ArquivoNclr = new Nclr(argumentos[1], Bpp);

            if (ArquivoNclr.Diretorio.Contains("0125_idcom.nclr"))
            {
                Largura = 48;
                Altura = 24;
            }

            switch (tipo)
            {
                case "BG Sem Tile Map":
                   Imagem = ExportarNCGR();
                    break;
                case "BG Com Tile Map":
                    ArquivoNscr = new Nscr(argumentos[2]);
                    Largura = ArquivoNscr.Largura;
                    Altura = ArquivoNscr.Altura;
                    Imagem = ExportarNCGRTileMap();                
                    break;
                case "Sprites Modo Tile":
                    ArquivoNcer = new Ncer(argumentos[2]);
                    EhSprite = true;
                    break;
                case "Sprites Modo 2D":
                    ArquivoNcer = new Ncer(argumentos[2]);
                    EhSprite = true;
                    break;
                default: break;
            }
        }

        public void Exportar() 
        {
            if (!Directory.Exists(LocalParaExportacao))
                Directory.CreateDirectory(LocalParaExportacao);

            Imagem.Save($"{LocalParaExportacao}\\{Path.GetFileName(DirNcgr).Replace("ncgr","png")}" );
        }

        private Bitmap ExportarNCGR()
        {
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.PaletasByte, EFormatoPaleta.BGR565, Altura, Largura, Bpp, EModoDimensional.M1D));                                          
            return cdi.BinParaBmp(ImagemEmBytes, ImagemEmBytes.Length, 0);

        }

        private Bitmap ExportarNCGRTileMap()
        {    

            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.PaletasByte, EFormatoPaleta.BGR565, Altura, Largura, Bpp, EModoDimensional.M1D, ArquivoNscr.TileMap));
            return cdi.BinParaBmp(ImagemEmBytes, ImagemEmBytes.Length, 0);

        }

        public void ExportarNCGRSprite1D(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {
            ArquivoNcer = new Ncer(dirNcer);

           

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
           

            foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
            {
                
                if (item.TabelaDeOams.Count > 0)
                {
                    cv.ExporteSprite1D(item.TabelaDeOams, fundoTransparente);
                }

                
                          
            }

           

            
        }

        public void ExportarNCGRSprite2D(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {

            Ncer ncer = new Ncer(dirNcer);

            ArquivoNcer = new Ncer(dirNcer);

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;


            foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
            {
                if (item.TabelaDeOams.Count > 0)
                {
                    cv.ExporteSprite2D(item.TabelaDeOams, fundoTransparente);
                }
                             
            }

        }


        public void ImportaNCGRSemMap(string dirImg)
        {

            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = ArquivoNclr.Paletas[0];

            if (Bpp == EIndexFormat.F4BBP)
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

        public void ImportaNCGRComMap(string dirImg)
        {


            ConversorDeImagens cv = new ConversorDeImagens();
            cv.CoresPaleta = ArquivoNclr.Paletas[0];

            if (Bpp == EIndexFormat.F4BBP)
            {
                List<byte[]> imgEhTilemap = cv.Converta4bppNcgrTileMap(dirImg);
                InsiranoNcgrEhNcsr(imgEhTilemap, ArquivoNscr.Diretorio);

            }
            else
            {
                List<byte[]> imgEhTilemap = cv.Converta8bppNcgrTileMap(dirImg);
                InsiranoNcgrEhNcsr(imgEhTilemap, ArquivoNscr.Diretorio);
            }
        }

        public void ImportaNCGRSprites(string dirImg, string dirNclr, List<Oam> oams)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
           // Nclr nclr = new Nclr(dirNclr);
           // ArquivoNcer = new Ncer(dirNcer);
            // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
          //  cv.Paleta = nclr.PaletaEmBytes;            
            InsiranoNcgr(cv.InsiraSpriteNgcr(dirImg, oams));

        }

        public void ImportaNCGRSpritesModo2(string dirImg, string dirNclr, List<Oam> oams)
        {

            // Ncgr ncgr = new Ncgr(dirNcgr);
         //   Nclr nclr = new Nclr(dirNclr);
            // ArquivoNcer = new Ncer(dirNcer);
            // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
            ConversorDeImagens cv = new ConversorDeImagens();
            cv.Imagem = ImagemEmBytes;
         //   cv.Paleta = nclr.PaletaEmBytes;
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
