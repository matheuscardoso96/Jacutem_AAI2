using LibDeImagensGbaDs.Conversor;
using LibDeImagensGbaDs.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FormatosNitro.Imagens
{
    public class Ncgr : NitroBase
    {
        public Char Char { get; set; }
        public Cpos Cpos { get; set; }
        public Ncer ArquivoNcer { get; set; }
        public Nscr ArquivoNscr { get; set; }
        public Nclr ArquivoNclr { get; set; }
        public string DirNcgr { get; set; }
        public List<Bitmap> Imagens { get; set; }
        public string LocalParaExportacao { get; set; }

        public delegate void Importar(string dirImg);
        public Importar TipoDeimportacao { get; set; }

        public Ncgr(BinaryReader br, Nclr nclr) : base(br)
        {
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);
            br.Close();
            CarregarImagens();


        }

        public Ncgr(BinaryReader br,Nclr nclr, Nscr nscr) :base(br)
        {
            ArquivoNscr = nscr;
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);        
            br.Close();
            CarregarImagemNCGRComNSCR();
        }

        public Ncgr(BinaryReader br, Nclr nclr, Ncer ncer) :base(br)
        {
            ArquivoNcer = ncer;
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);            
            br.Close();
        }

        public void Exportar() 
        {
            if (!Directory.Exists(LocalParaExportacao))
                Directory.CreateDirectory(LocalParaExportacao);

           // Imagem.Save($"{LocalParaExportacao}\\{Path.GetFileName(DirNcgr).Replace("ncgr","png")}" );
        }

        private void CarregarImagens()
        {
            
            EIndexFormat eIndexFormat = Char.IntensidadeDeBits == 3 ? EIndexFormat.F4BBP : EIndexFormat.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.Pltt.Paleta, EFormatoPaleta.BGR565, Char.QuatidadeDeTilesY * 8, Char.QuatidadeDeTilesX * 8, eIndexFormat, EModoDimensional.M1D));                    
            Imagens = new List<Bitmap>() { cdi.BinParaBmp(Char.Tiles, Char.Tiles.Length, 0) };
            Importar tipoDeimportacao = new Importar(ImportaNCGR);

        }

        private void CarregarImagemNCGRComNSCR()
        {
            EIndexFormat eIndexFormat = Char.IntensidadeDeBits == 3 ? EIndexFormat.F4BBP : EIndexFormat.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.Pltt.Paleta, EFormatoPaleta.BGR565, ArquivoNscr.Scrn.Altura, ArquivoNscr.Scrn.Largura, eIndexFormat, EModoDimensional.M1D , ArquivoNscr.Scrn.InfoTela.ToList(), true));
            Imagens = new List<Bitmap>() { cdi.BinParaBmp(Char.Tiles, Char.Tiles.Length, 0) };
            Importar tipoDeimportacao = new Importar(ImportaNCGRComNscr);

        }

        public void ExportarNCGRSprite1D(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {
          //  ArquivoNcer = new Ncer(dirNcer);

           

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            

           //// ConversorDeImagens cv = new ConversorDeImagens();
           // cv.Imagem = ImagemEmBytes;
           

           // foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
           // {
                
           //     if (item.TabelaDeOams.Count > 0)
           //     {
           //         cv.ExporteSprite1D(item.TabelaDeOams, fundoTransparente);
           //     }

                
                          
           // }

           

            
        }

        public void ExportarNCGRSprite2D(string dirNclr, string dirNcer, string dirSalvamento, bool fundoTransparente)
        {

          //  Ncer ncer = new Ncer(dirNcer);

          //  ArquivoNcer = new Ncer(dirNcer);

            if (!Directory.Exists(dirSalvamento))
            {
                Directory.CreateDirectory(dirSalvamento);
            }

            //ConversorDeImagens cv = new ConversorDeImagens();
            //cv.Imagem = ImagemEmBytes;


            //foreach (var item in ArquivoNcer.GrupoDeTabelasOam)
            //{
            //    if (item.TabelaDeOams.Count > 0)
            //    {
            //        cv.ExporteSprite2D(item.TabelaDeOams, fundoTransparente);
            //    }
                             
            //}

        }


        public void ImportaNCGR(string dirImg)
        {

           // ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.Pltt.Paleta, EFormatoPaleta.BGR565, Altura, Largura, Bpp, EModoDimensional.M1D));
          //  QuatidadeCores = ArquivoNclr.Pltt.Paleta.Length / 2;
           // cdi.BinParaBmp(ImagemEmBytes, ImagemEmBytes.Length, 0);
        }

        public void ImportaNCGRComNscr(string dirImg)
        {


            //ConversorDeImagens cv = new ConversorDeImagens();
            //cv.CoresPaleta = ArquivoNclr.Paletas[0];

            //if (Bpp == EIndexFormat.F4BBP)
            //{
            //    List<byte[]> imgEhTilemap = cv.Converta4bppNcgrTileMap(dirImg);
            //    InsiranoNcgrEhNcsr(imgEhTilemap, ArquivoNscr.Diretorio);

            //}
            //else
            //{
            //    List<byte[]> imgEhTilemap = cv.Converta8bppNcgrTileMap(dirImg);
            //    InsiranoNcgrEhNcsr(imgEhTilemap, ArquivoNscr.Diretorio);
            //}
        }

        //public void ImportaNCGRSprites(string dirImg, string dirNclr, List<Oam> oams)
        //{

        //    // Ncgr ncgr = new Ncgr(dirNcgr);
        //   // Nclr nclr = new Nclr(dirNclr);
        //   // ArquivoNcer = new Ncer(dirNcer);
        //  //  // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
        //  //  ConversorDeImagens cv = new ConversorDeImagens();
        //  //  cv.Imagem = ImagemEmBytes;
        //  ////  cv.Paleta = nclr.PaletaEmBytes;            
        //  //  InsiranoNcgr(cv.InsiraSpriteNgcr(dirImg, oams));

        //}

        //public void ImportaNCGRSpritesModo2(string dirImg, string dirNclr, List<Oam> oams)
        //{

        //    // Ncgr ncgr = new Ncgr(dirNcgr);
        // //   Nclr nclr = new Nclr(dirNclr);
        //    // ArquivoNcer = new Ncer(dirNcer);
        //    // ncer.Oams = ncer.Oams.Take(ncer.Oams.Count - menosElementos).ToList();            
        // //   ConversorDeImagens cv = new ConversorDeImagens();
        // //   cv.Imagem = ImagemEmBytes;
        // ////   cv.Paleta = nclr.PaletaEmBytes;
        // //   InsiranoNcgr(cv.InsiraSpriteNgcrModo2(dirImg, oams));

        //}

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
           // Array.Copy(Cabecalho, novoNcgr,Cabecalho.Length);
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

    public class Char 
    {
        public string Id { get; set; }
        public uint TamanhoChar { get; set; }
        public ushort QuatidadeDeTilesY { get; set; }
        public ushort QuatidadeDeTilesX { get; set; }
        public uint IntensidadeDeBits { get; set; }
        public ushort Desconhecido0 { get; set; }
        public ushort Desconhecido1 { get; set; }
        public uint FlagDeDimensao { get; set; }
        public uint TamanhoTiles { get; set; }
        public uint Desconhecido2 { get; set; }
        public byte[] Tiles { get; set; }

        public Char(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoChar = br.ReadUInt32();
            QuatidadeDeTilesY = br.ReadUInt16();
            QuatidadeDeTilesX = br.ReadUInt16();
            IntensidadeDeBits = br.ReadUInt32();
            Desconhecido0 = br.ReadUInt16();
            Desconhecido1 = br.ReadUInt16();
            FlagDeDimensao = br.ReadUInt32();
            TamanhoTiles = br.ReadUInt32();
            Desconhecido2 = br.ReadUInt32();
            Tiles = br.ReadBytes((int)TamanhoTiles);

        }

    }

    public class Cpos 
    {
        public string Id { get; set; }
        public uint TamanhoCpos { get; set; }
        public uint Padding { get; set; }
        public ushort TamanhoDeTile{ get; set; }
        public ushort QuatidadeDeTiles { get; set; }

        public Cpos(BinaryReader br)
        {
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoCpos = br.ReadUInt32();
            Padding = br.ReadUInt32();
            TamanhoDeTile = br.ReadUInt16();
            QuatidadeDeTiles = br.ReadUInt16();
        }

    }
}
