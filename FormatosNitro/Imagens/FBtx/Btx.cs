using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using LibDeImagensGbaDs.Conversor;
using LibDeImagensGbaDs.Enums;
using LibDeImagensGbaDs.Paleta;

namespace FormatosNitro.Imagens.FBtx
{
    public class Btx
    {
        public List<string> Errors = new List<string>();
        public List<TextureInfo> TextureInfos { get; set; }
        public List<PaletteInfo> PaletteInfos { get; set; }
        public string BtxPath { get; set; }
        public string ExportPath { get; set; }
        public int PalettesOffset { get; set; }
        public int TextureInfosBaseOffset { get; set; }
        public int BaseOffsetTextures { get; set; }
        public byte[] BtxBytes { get; set; }

        public Btx(BinaryReader br, string dirBtx)
        {
            BtxPath = dirBtx.Split(',')[0];
            ExportPath = $"{dirBtx.Split(',')[1]}{Path.GetFileName(BtxPath).Replace(".btx", "")}\\";
            if (File.Exists(BtxPath))
            {
                BtxBytes = File.ReadAllBytes(BtxPath);

                TextureInfosBaseOffset = 0x14;
                br.BaseStream.Position = TextureInfosBaseOffset + 0xE;
                int textInfoOffset = br.ReadInt16();

                br.BaseStream.Position = TextureInfosBaseOffset + 0x34;
                int paletteInfosOffset = br.ReadInt32();
                PalettesOffset = br.ReadInt32();

                br.BaseStream.Position = TextureInfosBaseOffset + 0x14;
                BaseOffsetTextures = br.ReadInt32();
                br.BaseStream.Position = TextureInfosBaseOffset + textInfoOffset + 1;
                int objectCount = br.ReadByte();
                br.BaseStream.Position = TextureInfosBaseOffset + textInfoOffset + 6;
                int tamanhoHeaderUkn = br.ReadInt16();
                textInfoOffset += 4;
                TextureInfos = GetTextureInfos(br, TextureInfosBaseOffset + textInfoOffset + tamanhoHeaderUkn, objectCount);

                br.BaseStream.Position = TextureInfosBaseOffset + paletteInfosOffset + 1;

                int paletteCount = br.ReadByte();
                br.BaseStream.Position = TextureInfosBaseOffset + paletteInfosOffset + 6;
                int paletteInfoSectionSize = br.ReadUInt16();
                paletteInfosOffset += 4;
                PaletteInfos = GetPalleteInfos(br, TextureInfosBaseOffset + paletteInfosOffset + paletteInfoSectionSize, paletteCount);

                LoadTextures(br, TextureInfos);
                br.Close();

            }
            else
            {
                Errors.Add($"Não foi possível encontrar {BtxPath}");
            }

        }

        private List<PaletteInfo> GetPalleteInfos(BinaryReader br, int offset, int paletteCount)
        {
            br.BaseStream.Position = offset;
            List<PaletteInfo> paleteInfos = new List<PaletteInfo>();
            for (int i = 0; i < paletteCount; i++)
            {
                PaletteInfo paleteInfo = new PaletteInfo();
                paleteInfo.Offset = br.ReadInt16() << 3;
                paleteInfos.Add(paleteInfo);
                br.BaseStream.Position += 2;
            }

            foreach (var p in paleteInfos)
            {
                p.PaletteName = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
            }

            return paleteInfos;
        }

        private List<TextureInfo> GetTextureInfos(BinaryReader br, int offset, int objectCount)
        {
            br.BaseStream.Position = offset;
            var txtInfo = new List<TextureInfo>();
            for (int i = 0; i < objectCount; i++)
            {
                ushort textureOffset = br.ReadUInt16();
                int paramss = br.ReadUInt16() >> 4;
                br.BaseStream.Position += 4;

                TextureInfo textureInfo = new TextureInfo();
                textureInfo.Offset = textureOffset << 3;
                textureInfo.Index = i;
                textureInfo.Width = 8 << (paramss & 7);
                paramss >>= 3;
                textureInfo.Height = 8 << (paramss & 7);
                paramss >>= 3;
                textureInfo.Format = paramss & 7;
                txtInfo.Add(textureInfo);

            }

            foreach (var info in txtInfo)
            {
                info.TextureName = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
            }

            return txtInfo;
        }

        public void LoadTextures(BinaryReader br, List<TextureInfo> textureInfos)
        {

            foreach (var textura in textureInfos)
            {
                int texturesByteSize = 0;
                switch (textura.Format)
                {
                    case 1:
                        texturesByteSize = textura.Height * textura.Width;
                        GetImage(textura, texturesByteSize, 0x200, ColorDepth.FA5I3, br);
                        break;
                    case 3:
                        texturesByteSize = textura.Height * textura.Width / 2;
                        GetImage(textura, texturesByteSize, 0x20, ColorDepth.F4BBP, br);
                        break;
                    case 4:
                    case 6:
                        texturesByteSize = textura.Height * textura.Width;
                        GetImage(textura, texturesByteSize, 0x200, ColorDepth.F8BBP, br);
                        break;
                    default:
                        break;
                }

            }
        }

        private void GetImage(TextureInfo info, int textureSize, int paletteSize, ColorDepth colorDepth, BinaryReader br)
        {
            br.BaseStream.Position = info.Offset + BaseOffsetTextures + TextureInfosBaseOffset;
            byte[] img = br.ReadBytes(textureSize);
            var pInfoIndex = PaletteInfos.FindIndex(x => x.PaletteName.Contains(info.TextureName + "_pl"));
            br.BaseStream.Position = PalettesOffset + TextureInfosBaseOffset + PaletteInfos[pInfoIndex].Offset;
            byte[] palette = br.ReadBytes(paletteSize);
            PaletteInfos[pInfoIndex].PaletteBytes = palette;
            info.ColorCount = paletteSize / 2;
            BGR565 bGR565 = new BGR565(palette);
            PaletteInfos[pInfoIndex].Palette = bGR565.Colors;
            info.TextureImage = ImageConverter.RawIndexedToBitmap(img, info.Width, info.Height, bGR565, TileMode.NotTiled, colorDepth);
            info.Bpp = colorDepth;
            info.PaletteIndex = pInfoIndex;
        }

        public void ExportAllTextures()
        {
            if (!Directory.Exists(ExportPath))
            {
                _ = Directory.CreateDirectory(ExportPath);
            }
            foreach (var txInfo in TextureInfos)
            {
                txInfo.TextureImage.Save($"{ExportPath}{txInfo.TextureName}.png");
            }
        }

        public void ExportOneTexture(TextureInfo textureInfo)
        {
            if (!Directory.Exists(ExportPath))
            {
                Directory.CreateDirectory(ExportPath);
            }

            TextureInfos[textureInfo.Index].TextureImage.Save($"{ExportPath}{textureInfo.TextureName}.png");
        }

        public void ImportOneTexture(string png)
        {
            MemoryStream btx = new MemoryStream(BtxBytes);

            using (BinaryWriter bw = new BinaryWriter(btx))
            {
                string texureName = Path.GetFileName(png).Replace(".png", "");
                var txtInfo = TextureInfos.Find(t => t.TextureName.Contains(texureName));
                if (txtInfo != null)
                {
                    var temp = new Bitmap(png);
                    ValidateTexure(temp, txtInfo);
                    if (Errors.Count == 0)
                    {
                        ConvertAndInsert(txtInfo, bw, temp);
                        BtxBytes = btx.ToArray();
                    }

                }
                else
                {
                    Errors.Add($"{texureName} não encontrado.");
                }

            }

        }

        public void ImportMultipleTextures(string[] pngsPaths)
        {
            MemoryStream btx = new MemoryStream(BtxBytes);

            using (BinaryWriter bw = new BinaryWriter(btx))
            {
                foreach (var png in pngsPaths)
                {
                    string texureName = Path.GetFileName(png).Replace(".png", "");
                    var txtInfo = TextureInfos.Find(t => t.TextureName.Contains(texureName));
                    if (txtInfo != null)
                    {
                        Bitmap temp = new Bitmap(png);
                        ValidateTexure(temp, txtInfo);
                        if (Errors.Count == 0)
                        {
                            ConvertAndInsert(txtInfo, bw, temp);
                        }

                        temp.Dispose();

                    }
                    else
                    {
                        Errors.Add($"{texureName} não encontrado.");
                    }
                }

                BtxBytes = btx.ToArray();

            }

        }

        private void ConvertAndInsert(TextureInfo info, BinaryWriter bw, Bitmap png)
        {
            var palInfo = PaletteInfos.First(x => x.PaletteName.Contains(info.TextureName + "_pl"));
            BGR565 bGR565 = new BGR565(palInfo.PaletteBytes);
            byte[] img = new byte[0];
            bw.BaseStream.Position = info.Offset + BaseOffsetTextures + TextureInfosBaseOffset;

            switch (info.Format)
            {
                case 1:
                    //img = ImageConverter.BitmapToRawIndexed(new Bitmap(png), bGR565, TileMode.NotTiled, ColorDepth.F4BBP);
                    break;
                case 3:
                    img = ImageConverter.BitmapToRawIndexed(new Bitmap(png), bGR565, TileMode.NotTiled, ColorDepth.F4BBP);
                    break;
                case 4:
                case 6:
                    img = ImageConverter.BitmapToRawIndexed(new Bitmap(png), bGR565, TileMode.NotTiled, ColorDepth.F8BBP);
                    break;
                default:
                    break;
            }

            bw.Write(img);
            ReloadTextures();
        }

        private void ReloadTextures()
        {
            BinaryReader br = new BinaryReader(new MemoryStream(BtxBytes));
            LoadTextures(br, TextureInfos);
            br.Close();
        }

        public void SaveToFile()
        {
            File.WriteAllBytes(BtxPath, BtxBytes);
        }

        private void ValidateTexure(Bitmap png, TextureInfo textureInfo)
        {
            if (png.Width > textureInfo.Width)
            {
                Errors.Add("A textura importada possui largura maior que a original.");
            }

            if (png.Height > textureInfo.Height)
            {
                Errors.Add("A textura importada possui altura maior que a original.");
            }

            if (png.Width < textureInfo.Width)
            {
                Errors.Add("A textura importada possui largura menor que a original.");
            }

            if (png.Height < textureInfo.Height)
            {
                Errors.Add("A textura importada possui altura menor que a original.");
            }

        }
    }

}
