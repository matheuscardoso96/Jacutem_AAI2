using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ImageLibGbaDS.Paleta;
using System.Threading.Tasks;
using LibDeImagensGbaDs.Conversor;
using LibDeImagensGbaDs.Enums;

namespace Jacutem_AAI2.Imagens
{
    public class Btx
    {
        public List<TextureInfo> Texturas { get; set; }
        public string DirBtx { get; set; }
        public string LocalSalvamento  { get; set; }
        public Btx()
        {

        }

        public Btx(string dirBtx)
        {
            DirBtx = dirBtx.Split(',')[0];
            LocalSalvamento = dirBtx.Split(',')[1];

          //  string nomeRealImg = ObtenhaNomeNaListaDeExtracaoDescomprima(@"cpac_3d\_logs_keys\cpac_3d_1.txt", dirImg);
            byte[] imgTemp = File.ReadAllBytes(DirBtx);

            Texturas = new List<TextureInfo>();

            


            using (BinaryReader br = new BinaryReader(new MemoryStream(imgTemp)))
            {
                //

               // br.BaseStream.Position = 0x14;
                int idxTex = 0x14;
                br.BaseStream.Position = idxTex + 0xE;
                int offsetInfoTexturas = br.ReadInt16();

                br.BaseStream.Position = idxTex + 0x34;
                int PaleteInfoOffeset = br.ReadInt32();
                int PaleteOffeset = br.ReadInt32();

                br.BaseStream.Position = idxTex + 0x14;
                int OffsetBaseTextura = br.ReadInt32();
                br.BaseStream.Position = idxTex + offsetInfoTexturas + 1;
                int numObjetos = br.ReadByte();
                br.BaseStream.Position = idxTex + offsetInfoTexturas + 6;
                int tamanhoHeaderUkn = br.ReadInt16();
                offsetInfoTexturas += 4;
                br.BaseStream.Position = idxTex + offsetInfoTexturas + tamanhoHeaderUkn;

               
                List<PaleteInfo> paleteInfos = new List<PaleteInfo>();



                for (int i = 0; i < numObjetos; i++)
                {
                    ushort offsetTextura = br.ReadUInt16();
                    int paremetros = br.ReadUInt16() >> 4;
                    br.BaseStream.Position += 4;

                    TextureInfo texture = new TextureInfo();
                    texture.Offset = offsetTextura << 3;
                    texture.Indice = i;
                    texture.Largura = (8 << (paremetros & 7));
                    paremetros = paremetros >> 3;
                    texture.Altura = (8 << (paremetros & 7));
                    paremetros = paremetros >> 3;
                    texture.Formato = paremetros & 7;
                    Texturas.Add(texture);

                }



                foreach (var t in Texturas)
                {
                    t.NomeTextura = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
                }


                br.BaseStream.Position = idxTex + PaleteInfoOffeset + 1;

                int numPaletas = br.ReadByte();

                br.BaseStream.Position = idxTex + PaleteInfoOffeset + 6;
                int tamanhoSecaoPaletaInfo = br.ReadUInt16();
                PaleteInfoOffeset += 4;

                br.BaseStream.Position = idxTex + PaleteInfoOffeset + tamanhoSecaoPaletaInfo;



                for (int i = 0; i < numPaletas; i++)
                {
                    PaleteInfo paleteInfo = new PaleteInfo();
                    paleteInfo.Offset = br.ReadInt16() << 3;
                    paleteInfos.Add(paleteInfo);
                    br.BaseStream.Position += 2;
                }

                foreach (var p in paleteInfos)
                {
                    p.NomePaleta = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
                }

                int contador = 0;

                foreach (var textura in Texturas)
                {

                  //  ConversorDeImagens ci = new ConversorDeImagens();
                    Bitmap imagemFinal = new Bitmap(1,1);

                    if (textura.Formato == 1)
                    {

                        int tamanhoGrafico = (textura.Altura * textura.Largura);
                        br.BaseStream.Position = textura.Offset + OffsetBaseTextura + idxTex;
                        byte[] img = br.ReadBytes(tamanhoGrafico);
                        br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                        byte[] paleta = br.ReadBytes(0x200);
                      //  ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(paleta, EFormatoPaleta.BGR565, textura.Altura, textura.Largura, EIndexFormat.FA3I5, EModoDimensional.M2D,null, true));
                       // textura.Textura = cdi.BinParaBmp(img,img.Length, 0);
                        //textura.Bpp = Bpp.bpp8;
                    }

                    else  if (textura.Formato == 3)
                    {
                        int tamanhoGrafico = (textura.Altura * textura.Largura) / 2;
                        br.BaseStream.Position = textura.Offset + OffsetBaseTextura + idxTex;
                        byte[] img = br.ReadBytes(tamanhoGrafico);
                        br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.FirstOrDefault(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                        byte[] paleta = br.ReadBytes(0x20);
                      //  ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(paleta, EFormatoPaleta.BGR565, textura.Altura, textura.Largura, EIndexFormat.F4BBP, EModoDimensional.M2D));
                     //   textura.Textura = cdi.BinParaBmp(img, img.Length, 0);

                    }
                    //else if (textura.Formato == 666666)
                    //{
                    //    int tamanhoGrafico = (textura.Altura * textura.Largura);
                    //    br.BaseStream.Position = textura.Offset + OffsetBaseTextura + idxTex;
                    //    byte[] img = br.ReadBytes(tamanhoGrafico);                  
                    //    br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                    //    byte[] paleta = br.ReadBytes(0x200);
                    //    ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(paleta, EFormatoPaleta.BGR565, textura.Altura, textura.Largura, EIndexFormat.FA5I3, EModoDimensional.M2D, null, true));
                    //    textura.Textura = cdi.BinParaBmp(img, 0);
                    //    textura.Bpp = Bpp.bpp8;

                    //}
                    else if (textura.Formato == 6 || textura.Formato == 4)
                    {
                        int tamanhoGrafico = (textura.Altura * textura.Largura);
                        br.BaseStream.Position = textura.Offset + OffsetBaseTextura + idxTex;
                        byte[] img = br.ReadBytes(tamanhoGrafico);
                        br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                        byte[] paleta = br.ReadBytes(0x200);
                      //  ConversorDeImagem cdi = new ConversorDeImagem(new ConversorFormatoIndexado(paleta, EFormatoPaleta.BGR565, textura.Altura, textura.Largura, EIndexFormat.F8BBP, EModoDimensional.M2D));
                      //  textura.Textura = cdi.BinParaBmp(img, img.Length, 0);
                       // textura.Bpp = Bpp.bpp8;
                    }


                   

                    contador++;
                }



            }


        }

        public void Exportar() 
        {
            int contador = 0;
            if (!Directory.Exists(LocalSalvamento))
            {
                Directory.CreateDirectory(LocalSalvamento);
            }
            foreach (var textura in Texturas)
            {
                textura.Textura.Save($"{LocalSalvamento}{Path.GetFileName(DirBtx).Replace(".btx", "_")}{contador}.png");
                contador++;
            }
        }

        public void ImporteImagemParaBtx(string argumentos, string png)
        {

            string dirImg = argumentos.Split(',')[0];
            string dirSalva = argumentos.Split(',')[1];
            byte[] img = new byte[] { };
            int posicaoGrafico = 0;

            List<TextureInfo> textures = new List<TextureInfo>();
            List<PaleteInfo> paleteInfos = new List<PaleteInfo>();
            bool temSim = false;

            byte[] imgTemp = File.ReadAllBytes(dirImg);
           


            using (BinaryReader br = new BinaryReader(new MemoryStream(imgTemp)))
            {
                //

                int idxTex = 0x14;
                br.BaseStream.Position = idxTex + 0xE;
                int offsetInfoTexturas = br.ReadInt16();

                br.BaseStream.Position = idxTex + 0x34;
                int PaleteInfoOffeset = br.ReadInt32();
                int PaleteOffeset = br.ReadInt32();

                br.BaseStream.Position = idxTex + 0x14;
                int OffsetBaseTextura = br.ReadInt32();
                br.BaseStream.Position = idxTex + offsetInfoTexturas + 1;
                int numObjetos = br.ReadByte();
                br.BaseStream.Position = idxTex + offsetInfoTexturas + 6;
                int tamanhoHeaderUkn = br.ReadInt16();
                offsetInfoTexturas += 4;
                br.BaseStream.Position = idxTex + offsetInfoTexturas + tamanhoHeaderUkn;


                //Format: < 0, 8, 2, 4, 8, 2, 8, 16 >


                for (int i = 0; i < numObjetos; i++)
                {
                    ushort offsetTextura = br.ReadUInt16();
                    int paremetros = br.ReadUInt16() >> 4;
                    br.BaseStream.Position += 4;

                    TextureInfo texture = new TextureInfo();
                    texture.Indice = i;
                    texture.Offset = offsetTextura << 3;

                    texture.Largura = (8 << (paremetros & 7));
                    paremetros = paremetros >> 3;
                    texture.Altura = (8 << (paremetros & 7));
                    paremetros = paremetros >> 3;
                    texture.Formato = paremetros & 7;
                    //paremetros = paremetros >> 3;
                    // texture.PaleteId = textureFormat[paremetros & 1];
                    textures.Add(texture);

                }



                foreach (var t in textures)
                {
                    t.NomeTextura = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
                }


                br.BaseStream.Position = idxTex + PaleteInfoOffeset + 1;

                int numPaletas = br.ReadByte();

                br.BaseStream.Position = idxTex + PaleteInfoOffeset + 6;
                int tamanhoSecaoPaletaInfo = br.ReadUInt16();
                PaleteInfoOffeset += 4;

                br.BaseStream.Position = idxTex + PaleteInfoOffeset + tamanhoSecaoPaletaInfo;



                for (int i = 0; i < numPaletas; i++)
                {
                    PaleteInfo paleteInfo = new PaleteInfo();
                    paleteInfo.Offset = br.ReadInt16() << 3;
                    paleteInfos.Add(paleteInfo);
                    br.BaseStream.Position += 2;
                }

                foreach (var p in paleteInfos)
                {
                    p.NomePaleta = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");
                }


                int contador = 0;
                //  Path.GetFileName(png).Replace(".png", "_" + contador + ".png");

                string nomeTextura = Path.GetFileName(png).ToLower();
                

                foreach (var textura in textures)
                {
                    string nome = Path.GetFileName(dirImg).Replace(".btx", "") + "_" + contador + ".png";
                    if (nome.Contains(nomeTextura))
                    {
                        temSim = true;

                      //  ConversorDeImagens ci = new ConversorDeImagens();


                        if (textura.Formato == 3)
                        {
                            //br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                            //byte[] paleta = br.ReadBytes(0x20);
                            //ci.CoresPaleta = new List<Color>();
                            //ci.ConvertaPaleta(paleta);
                            //img = ci.Insira4bppBitmap(png);
                            //posicaoGrafico = textura.Offset + OffsetBaseTextura + idxTex;
                            // ci.Exporte4bppBitmap(img, textura.Largura, textura.Altura);

                        }
                        //else if (4)
                        //{
                        //    br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                        //    byte[] paleta = br.ReadBytes(0x200);
                        //    ci.CoresPaleta = new List<Color>();
                        //    ci.ConvertaPaleta(paleta);
                        //    img = ci.Insira8bppBitmap(png);
                        //    posicaoGrafico = textura.Offset + OffsetBaseTextura + idxTex;
                        //    // ci.Exporte8bppBitmap(img, textura.Largura, textura.Altura);
                        //}
                        else if (textura.Formato == 6 || textura.Formato == 4)
                        {
                            //br.BaseStream.Position = PaleteOffeset + idxTex + paleteInfos.First(x => x.NomePaleta.Contains(textura.NomeTextura + "_pl")).Offset;
                            //byte[] paleta = br.ReadBytes(0x200);
                            //ci.ConvertaPaleta(paleta);
                            //// ci.ExporteA5I3(img, textura.Largura, textura.Altura);

                        }
                        else if (textura.Formato == 7)
                        {

                            // ci.Exporte16bppDiretoBitmap(img, textura.Largura, textura.Altura);

                        }

                        

                        break;

                    }


                    contador++;
                }




            }



            if (temSim)
            {
                imgTemp = InsiraBinario(posicaoGrafico, imgTemp, img);


                File.WriteAllBytes(dirImg, imgTemp);
            }

            // 

        }

        private byte[] InsiraBinario(int offset, byte[] dir, byte[] binario)
        {
            MemoryStream ms = new MemoryStream(dir);
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.BaseStream.Position = offset;
                if (binario.Length + offset > bw.BaseStream.Length)
                {
                    throw new Exception(dir + "\n Esta imagem é maior que a original");
                }
                bw.Write(binario);


                return ms.ToArray();
            }
        }
    }

    public class TextureInfo
    {
        public int Indice { get; set; }
        public int Paremetros { get; set; }
        public int Offset { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }
        public int Formato { get; set; }
      //  public Bpp Bpp { get; set; }
        public int PaleteId { get; set; }
        public string NomeTextura { get; set; }
        public Bitmap Textura { get; set; }
        public List<Color> PaletaImg { get; set; }

        public override string ToString()
        {
            return Indice.ToString();
        }

    }

    public class PaleteInfo
    {
        public int Offset { get; set; }
        public string NomePaleta { get; set; }


    }

}
