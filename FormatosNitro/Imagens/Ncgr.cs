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
        public Char Char { get; private set; }
        public Cpos Cpos { get; private set; }
        public Ncer ArquivoNcer { get; private set; }
        public Nscr ArquivoNscr { get; private set; }
        public Nclr ArquivoNclr { get; private set; }
        public List<Bitmap> Imagens { get; private set; }

        public delegate void Importar(string dirImg);
        public delegate void Salvar(string dir, bool paletaFoiModificada);
        public delegate void Exportar(string dir);
        public Importar ImportarNgcr { get; set; }
        public Exportar ExportarImagem { get; set; }

        public Ncgr(BinaryReader br, Nclr nclr,string diretorio) : base(br, diretorio)
        {
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);
            br.Close();
            CarregarImagens();
            ImportarNgcr = new Importar(ImportaNCGR);
            ExportarImagem = new Exportar(ExportarNcgrComOuSemNscr);


        }

        public Ncgr(BinaryReader br,Nclr nclr, Nscr nscr, string diretorio) : base(br, diretorio)
        {
            ArquivoNscr = nscr;
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);        
            br.Close();
            CarregarImagemNCGRComNSCR();
            ImportarNgcr = new Importar(ImportaNCGRComNscr);
            ExportarImagem = new Exportar(ExportarNcgrComOuSemNscr);
        }

        public Ncgr(BinaryReader br, Nclr nclr, Ncer ncer, string diretorio) : base(br, diretorio)
        {
            ArquivoNcer = ncer;
            ArquivoNclr = nclr;
            Char = new Char(br);
            if (QuantidadeDeSecoes > 1)
                Cpos = new Cpos(br);            
            br.Close();
        }

        public void ExportarNcgrComOuSemNscr(string dir) 
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
           Imagens.First().Save($"{dir}{Path.GetFileName(Diretorio).Replace("ncgr","png")}");
        }

        private void CarregarImagens()
        {
            
            ProfundidaDeCor eIndexFormat = Char.IntensidadeDeBits == 3 ? ProfundidaDeCor.F4BBP : ProfundidaDeCor.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(
                ArquivoNclr.Pltt.Paleta, 
                EFormatoPaleta.NintendoDS, 
                Char.QuatidadeDeTilesY * 8, 
                Char.QuatidadeDeTilesX * 8, 
                eIndexFormat, EModoDimensional.M1D));                    
            Imagens = new List<Bitmap>() { cdi.BinParaBmp(Char.Tiles, Char.Tiles.Length, 0) };
            

        }

        private void CarregarImagemNCGRComNSCR()
        {
            ProfundidaDeCor eIndexFormat = Char.IntensidadeDeBits == 3 ? ProfundidaDeCor.F4BBP : ProfundidaDeCor.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(
                ArquivoNclr.Pltt.Paleta, 
                EFormatoPaleta.NintendoDS, 
                ArquivoNscr.Scrn.Altura,
                ArquivoNscr.Scrn.Largura, 
                eIndexFormat, EModoDimensional.M1D ,
                ArquivoNscr.Scrn.InfoTela.ToList()
                ));
            
            Imagens = new List<Bitmap>() { cdi.BinParaBmp(Char.Tiles, Char.Tiles.Length, 0) };
           

        }




        public void ImportaNCGR(string dirImg)
        {

            ProfundidaDeCor eIndexFormat = Char.IntensidadeDeBits == 3 ? ProfundidaDeCor.F4BBP : ProfundidaDeCor.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(ArquivoNclr.Pltt.Paleta, EFormatoPaleta.NintendoDS, Char.QuatidadeDeTilesY * 8, Char.QuatidadeDeTilesX * 8, eIndexFormat, EModoDimensional.M1D));
            byte[] imagem = (byte[])cdi.BmpParaBin(new Bitmap(dirImg))[0];
            Char.Tiles = imagem;
            CarregarImagens();
        }

        public void ImportaNCGRComNscr(string dirImg)
        {
            ProfundidaDeCor eIndexFormat = Char.IntensidadeDeBits == 3 ? ProfundidaDeCor.F4BBP : ProfundidaDeCor.F8BBP;
            ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(
                ArquivoNclr.Pltt.Paleta,
                EFormatoPaleta.NintendoDS,
                ArquivoNscr.Scrn.Altura,
                ArquivoNscr.Scrn.Largura,
                eIndexFormat, EModoDimensional.M1D,
                ArquivoNscr.Scrn.InfoTela.ToList()
                ));

            byte[] imagem = (byte[])cdi.BmpParaBin(new Bitmap(dirImg))[0];
            Char.Tiles = imagem;
            CarregarImagens();

        }

      

        public void SalvarNCGR(bool paletaFoiModificada) 
        {
            MemoryStream novoNgcr = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNgcr))
            {
                base.EscreverPropiedades(bw);
                Char.EscreverPropiedades(bw);
                if (Cpos != null)
                {
                    Cpos.EscreverPropiedades(bw);
                }
                
            }

            File.WriteAllBytes(base.Diretorio, novoNgcr.ToArray());

            if (paletaFoiModificada)
            {
                ArquivoNclr.SalvarNclr();
            }

            if (ArquivoNscr != null)
            {
                ArquivoNscr.SalvarNscr();
            }

            if (ArquivoNcer != null)
            {
                ArquivoNcer.SalvarNcer();
            }

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
        public uint TamanhoTilesEmBytes { get; set; }
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
            TamanhoTilesEmBytes = br.ReadUInt32();
            Desconhecido2 = br.ReadUInt32();
            Tiles = br.ReadBytes((int)TamanhoTilesEmBytes);

        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(32 + TamanhoTilesEmBytes);
            bw.Write(QuatidadeDeTilesY);
            bw.Write(QuatidadeDeTilesX);
            bw.Write(IntensidadeDeBits);
            bw.Write(Desconhecido0);
            bw.Write(Desconhecido1);
            bw.Write(FlagDeDimensao);
            bw.Write(TamanhoTilesEmBytes);
            bw.Write(Desconhecido2);
            bw.Write(Tiles);
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

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoCpos);
            bw.Write(Padding);
            bw.Write(TamanhoDeTile);
            bw.Write(QuatidadeDeTiles);
           
        }
    }
}
