using FormatosNitro.Imagens.FNcer;
using FormatosNitro.Imagens.FNclr;
using FormatosNitro.Imagens.FNscr;
using LibDeImagensGbaDs.Conversor;
using LibDeImagensGbaDs.Enums;
using LibDeImagensGbaDs.Paleta;
using LibDeImagensGbaDs.Sprites;
using LibDeImagensGbaDs.TileMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FormatosNitro.Imagens.FNcgr
{
    public class Ncgr : NitroBase
    {
        public Char Char { get; private set; }
        public Cpos Cpos { get; private set; }
        public Ncer ArquivoNcer { get; private set; }
        public Nscr ArquivoNscr { get; private set; }
        public Nclr ArquivoNclr { get; private set; }
        public Bitmap ConvertedImage { get; private set; }
        public string ExportPath { get; set; }
        public List<string> AllErrors { get; set; } = new List<string>();

        public delegate void Importar(string dirImg);
        public delegate void Salvar(string dir, bool paletaFoiModificada);
        public delegate void Exportar();
        public Importar ImportarNgcr { get; set; }
        public Exportar ExportarImagem { get; set; }

        public Ncgr(BinaryReader br, Nclr nclr,string diretorio, string exportPath) : base(br, diretorio)
        {
            if (Errors.Count == 0 && nclr.Errors.Count == 0)
            {
                ExportPath = exportPath;
                ArquivoNclr = nclr;
                SetNgcrProperties(br, exportPath);
                CarregarImagens();
                ImportarNgcr = new Importar(ImportaNCGR);
                ExportarImagem = new Exportar(ExportarNcgrComOuSemNscr);
            }
            else
            {
                AllErrors.AddRange(Errors);
                AllErrors.AddRange(nclr.Errors);
            }

            br.Close();

        }

        public Ncgr(BinaryReader br,Nclr nclr, Nscr nscr, string diretorio, string exportPath) : base(br, diretorio)
        {
            if (Errors.Count == 0 && nclr.Errors.Count == 0 && nscr.Errors.Count == 0)
            {
                ArquivoNscr = nscr;
                ArquivoNclr = nclr;
                SetNgcrProperties(br, exportPath);             
                CarregarImagemNCGRComNSCR();
                ImportarNgcr = new Importar(ImportaNCGRComNscr);
                ExportarImagem = new Exportar(ExportarNcgrComOuSemNscr);
            }
            else
            {
                AllErrors.AddRange(Errors);
                AllErrors.AddRange(nclr.Errors);
                AllErrors.AddRange(nscr.Errors);
            }

            br.Close();

        }

        public Ncgr(BinaryReader br, Nclr nclr, Ncer ncer, string diretorio, string exportPath) : base(br, diretorio)
        {
            if (Errors.Count == 0 && nclr.Errors.Count == 0 && ncer.Errors.Count == 0)
            {
                ArquivoNcer = ncer;
                ArquivoNclr = nclr;
                SetNgcrProperties(br,exportPath);
                //CarregarImagemNCGRComNCER();
            }
            else
            {
                AllErrors.AddRange(Errors);
                AllErrors.AddRange(nclr.Errors);
                AllErrors.AddRange(ncer.Errors);
            }

            br.Close();

        }

        private void SetNgcrProperties(BinaryReader br, string exportPath)
        {
            ExportPath = exportPath;
            Char = new Char(br);
            if (SectionCount > 1)
                Cpos = new Cpos(br);
            
        }

        public void ExportarNcgrComOuSemNscr()
        {
            if (!Directory.Exists(ExportPath))
            {
                _ = Directory.CreateDirectory(ExportPath);
            }

            try
            {
                ConvertedImage.Save($"{ExportPath}{Path.GetFileName(NitroFilePath).Replace("ncgr", "png")}");
            }
            catch (Exception)
            {

                throw new Exception($"{NitroFilePath}");
            }

            
        }

        private void CarregarImagens()
        {
            ColorDepth depth = Char.IntensidadeDeBits == 3 ? ColorDepth.F4BBP : ColorDepth.F8BBP;
            BGR565 palette = new BGR565(ArquivoNclr.Pltt.Paleta);
            ConvertedImage = ImageConverter.RawIndexedToBitmap(Char.Tiles, Char.QuatidadeDeTilesX * 8, Char.QuatidadeDeTilesY * 8, palette, TileMode.Tiled, depth);
            ArquivoNclr.Colors = palette.Colors;
        }

        private void CarregarImagemNCGRComNSCR()
        {
            ColorDepth depth = Char.IntensidadeDeBits == 3 ? ColorDepth.F4BBP : ColorDepth.F8BBP;
            BGR565 palette = new BGR565(ArquivoNclr.Pltt.Paleta);
            ConvertedImage = ImageConverter.TileMappedToBitmap(Char.Tiles, ArquivoNscr.Scrn.InfoTela.ToList(), ArquivoNscr.Scrn.Largura, ArquivoNscr.Scrn.Altura, palette, depth);
            ArquivoNclr.Colors = palette.Colors;
        }

        public void LoadNGCRImageWithNcer(Ebk ebk)
        {
            ColorDepth depth = Char.IntensidadeDeBits == 3 ? ColorDepth.F4BBP : ColorDepth.F8BBP;
            TypeSprite typeSprite = new TypeSprite() { Oams = ebk.Oams, TileBoundary = (int)ArquivoNcer.Cebk.TileBoundary };
            TileMode tileMode = Char.FlagDeDimensao == 0 ? TileMode.Tiled : TileMode.NotTiled;
            ConvertedImage = ImageConverter.SpriteToBitmap(Char.Tiles, ArquivoNclr.Pltt.Paleta, typeSprite, tileMode, depth);    

        }



        public void ImportaNCGR(string dirImg)
        {
            ColorDepth depth = Char.IntensidadeDeBits == 3 ? ColorDepth.F4BBP : ColorDepth.F8BBP;
            BGR565 palette = new BGR565(ArquivoNclr.Pltt.Paleta);
            Bitmap imageToInsert = new Bitmap(dirImg);
            if (imageToInsert.Width > Char.QuatidadeDeTilesX * 8)
            {
                Errors.Add("A largura da imagem importada é maior a que original");
            }

            if (imageToInsert.Height > Char.QuatidadeDeTilesY * 8)
            {
                Errors.Add("A altura da imagem importada é maior que a original");
            }

            if (Errors.Count == 0)
            {
                Char.Tiles = ImageConverter.BitmapToRawIndexed(imageToInsert, palette, TileMode.Tiled, depth);
                CarregarImagens();
                ArquivoNclr.Colors = palette.Colors;
            }
           

           
        }

        public void ImportaNCGRComNscr(string dirImg)
        {
            ColorDepth depth = Char.IntensidadeDeBits == 3 ? ColorDepth.F4BBP : ColorDepth.F8BBP;
            BGR565 palette = new BGR565(ArquivoNclr.Pltt.Paleta);
            TileMapType tilemap = new TileMapType();
            Bitmap imageToInsert = new Bitmap(dirImg);
            if (imageToInsert.Width > ArquivoNscr.Scrn.Largura)
            {
                Errors.Add("A largura da imagem importada é maior que a original");
            }

            if (imageToInsert.Height > ArquivoNscr.Scrn.Altura)
            {
                Errors.Add("A altura da imagem importada é maior que a original");
            }

            if (Errors.Count == 0)
            {
                Char.Tiles = ImageConverter.BitmapToTileMapped(imageToInsert, palette, tilemap, depth);
                ArquivoNscr.Scrn.InfoTela = tilemap.Tilemap.ToArray();
                CarregarImagemNCGRComNSCR();
            }

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

            File.WriteAllBytes(base.NitroFilePath, novoNgcr.ToArray());

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
}
