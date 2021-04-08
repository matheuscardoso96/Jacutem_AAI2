using Jacutem_AAI2.Imagens.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;


namespace Jacutem_AAI2.Imagens
{
    public class ConversorDeImagens
    {

        public byte[] TileMap { get; set; }
        public byte[] Imagem { get; set; }
        public byte[] Paleta { get; set; }
        public List<Color> CoresPaleta { get; set; }

        public ConversorDeImagens(string dirPaleta, string dirImagem, string dirTilemap, string bpp)
        {

            TileMap = File.ReadAllBytes(dirTilemap);
            if (dirImagem.Contains("-"))
            {
                Imagem = File.ReadAllBytes(dirImagem.Split('-').First() + ".bin");
            }
            else
            {
                Imagem = File.ReadAllBytes(dirImagem);
            }

            Paleta = File.ReadAllBytes(dirPaleta);
            CoresPaleta = new List<Color>();
            ConvertaPaleta(Paleta);
        }

        public ConversorDeImagens(string dirPaleta, string dirImagem, string bpp)
        {
            if (dirImagem.Contains("-"))
            {
                Imagem = File.ReadAllBytes(dirImagem.Split('-').First() + ".bin");
            }
            else
            {
                Imagem = File.ReadAllBytes(dirImagem);
            }
            Paleta = File.ReadAllBytes(dirPaleta);
            CoresPaleta = new List<Color>();
            ConvertaPaleta(Paleta);
        }

        public ConversorDeImagens(string dirPaleta, string dirImagem)
        {
            if (dirImagem.Contains("-"))
            {
                Imagem = File.ReadAllBytes(dirImagem.Split('-').First() + ".bin");
            }
            else
            {
                Imagem = File.ReadAllBytes(dirImagem);
            }

            Paleta = File.ReadAllBytes(dirPaleta);

        }
        public ConversorDeImagens()
        {

        }

        #region Exportar

        public Bitmap ExporteA3I5(byte[] imagem, int largura, int altura)
        {

            Bitmap img = new Bitmap(largura, altura);

            int complemento = 1;



            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    for (int y = 0; y < altura; y++)
                    {
                        for (int x = 0; x < largura; x += complemento)
                        {

                            int valorPixelComAlphaEPaleta = br.ReadByte();
                            int valorNaPaleta = valorPixelComAlphaEPaleta & 0x1F;
                            int valorAlpha = valorPixelComAlphaEPaleta >> 5;
                            valorAlpha = (valorAlpha * 4) + (valorAlpha / 2);

                            valorAlpha = (valorAlpha * 255) / 31;

                            Color corComAlhpa = Color.FromArgb(valorAlpha, CoresPaleta[valorNaPaleta]);

                            img.SetPixel(x, y, corComAlhpa);



                        }
                    }

                }

            }

            return img;
        }

        public Bitmap ExporteA5I3(byte[] imagem, int largura, int altura)
        {

            Bitmap img = new Bitmap(largura, altura);

            int complemento = 1;



            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    for (int y = 0; y < altura; y++)
                    {
                        for (int x = 0; x < largura; x += complemento)
                        {

                            int valorPixelComAlphaEPaleta = br.ReadByte();
                            int valorNaPaleta = valorPixelComAlphaEPaleta & 7;
                            int valorAlpha = valorPixelComAlphaEPaleta >> 3;

                            valorAlpha = (valorAlpha * 255) / 31;

                            Color corComAlhpa = Color.FromArgb(valorAlpha, CoresPaleta[valorNaPaleta]);

                            img.SetPixel(x, y, corComAlhpa);

                          

                        }
                    }


                }

            }

            return img;
        }

        public Bitmap Exporte1bppBitmap(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            CoresPaleta = new List<Color>();
            CoresPaleta.Add(Color.FromArgb(0, 0, 0));
            CoresPaleta.Add(Color.FromArgb(255, 255, 255));
            int contador = 0;

            var bits = new BitArray(imagem);

            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    int valor = bits[contador] ? 1 : 0;
                    imagemFinal.SetPixel(x, y, CoresPaleta[valor]);
                    contador++;
                }
            }

            return imagemFinal;
        }

        public Bitmap Exporte2bppBitmap(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            CoresPaleta = new List<Color>();
            CoresPaleta.Add(Color.FromArgb(0, 0, 0));
            CoresPaleta.Add(Color.FromArgb(255, 255, 255));
            CoresPaleta.Add(Color.FromArgb(169, 169, 169));
            CoresPaleta.Add(Color.FromArgb(100, 100, 100));
            int contador = 0;

            var bits = new BitArray(imagem);

            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    int valor1 = bits[contador] ? 1 : 0;
                    int valor2 = bits[contador + 1] ? 1 : 0;
                    int valorFinal = (valor2 << 1) | valor1;
                    imagemFinal.SetPixel(x, y, CoresPaleta[valorFinal]);
                    contador+=2;
                }
            }

            return imagemFinal;
        }

        public Bitmap Exporte4bppTile(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            List<Bitmap> tiles = ConvertaBytesParaTile(imagem, "4");
            imagemFinal = LeiaTileMapERetorneImagem(TileMap, tiles, imagemFinal);
            DisposeListaBmp(tiles);
            return imagemFinal;
        }

        public Bitmap Exporte4bppTileRaw(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            List<Bitmap> tiles = ConvertaBytesParaTile(imagem, "4");
            imagemFinal = ExporteImagemSemTileMap(tiles, imagemFinal);
            DisposeListaBmp(tiles);
            return imagemFinal;

        }

        public Bitmap Exporte4bppBitmap(byte[] imagem, int largura, int altura)
        {
            Bitmap img = new Bitmap(largura, altura);

            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    for (int y = 0; y < altura; y++)
                    {
                        for (int x = 0; x < largura; x += 2)
                        {

                            int valorNaPaleta = br.ReadByte();
                            int nibbleAlto = (valorNaPaleta & 0xF0) >> 4;
                            int nibbleBaixo = (valorNaPaleta & 0x0F);
                            img.SetPixel(x, y, CoresPaleta[nibbleBaixo]);
                            img.SetPixel(x + 1, y, CoresPaleta[nibbleAlto]);

                        }
                    }


                    // tiles.Add(tile);
                    // tile.Dispose();

                }

            }

            return img;
        }

        public Bitmap Exporte8bppTile(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            List<Bitmap> tiles = ConvertaBytesParaTile(imagem, "8");
            imagemFinal = LeiaTileMapERetorneImagem(TileMap, tiles, imagemFinal);
            DisposeListaBmp(tiles);
            return imagemFinal;

        }

        public Bitmap Exporte8bppTileRaw(int largura, int altura, byte[] imagem)
        {
            Bitmap imagemFinal = new Bitmap(largura, altura);
            List<Bitmap> tiles = ConvertaBytesParaTile(imagem, "8");
            imagemFinal = ExporteImagemSemTileMap(tiles, imagemFinal);
            tiles.Clear();
            return imagemFinal;

        }

        public Bitmap Exporte8bppBitmap(byte[] imagem, int largura, int altura)
        {
            Bitmap img = new Bitmap(largura, altura);

            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    for (int y = 0; y < altura; y++)
                    {
                        for (int x = 0; x < largura; x++)
                        {


                            int valorNaPaleta = br.ReadByte();
                            img.SetPixel(x, y, CoresPaleta[valorNaPaleta]);

                            /* else
                             {
                                 int valorNaPaleta = br.ReadByte();
                                 int nibbleAlto = (valorNaPaleta & 0xF0) >> 4;
                                 int nibbleBaixo = (valorNaPaleta & 0x0F);
                                 tiles.SetPixel(x, y, CoresPaleta[nibbleBaixo]);
                                 tiles.SetPixel(x + 1, y, CoresPaleta[nibbleAlto]);
                             }*/

                        }
                    }


                    // tiles.Add(tile);
                    // tile.Dispose();

                }

            }

            return img;
        }


        public void ExporteSprite(List<Oam> valoresOam, bool fundoTransparente)
        {
            UtilizeTabelaOamEExporteSprite(valoresOam, "", "aai2", fundoTransparente);
        }


        public void ExporteSpriteModo2(List<Oam> valoresOam, bool fundoTransparente)
        {
            // List<Oam> valoresOam = ObtenhaTabelaOam(dirArquivoOam, idxTabela);
            // valoresOam = valoresOam.Take(valoresOam.Count - menosElementos).ToList();


            UtilizeTabelaOamEExporteSpriteModo2(valoresOam, fundoTransparente);
        }

        public List<Bitmap> ConvertaBytesParaTile(byte[] imagem, string bpp)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            int complemento = 1;

            if (bpp.Contains("4"))
            {
                complemento = 2;
            }

            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    Bitmap tile = new Bitmap(8, 8);
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x += complemento)
                        {
                            if (bpp.Contains("8"))
                            {
                                int valorNaPaleta = br.ReadByte();
                                tile.SetPixel(x, y, CoresPaleta[valorNaPaleta]);
                            }
                            else
                            {
                                int valorNaPaleta = br.ReadByte();
                                int nibbleAlto = (valorNaPaleta & 0xF0) >> 4;
                                int nibbleBaixo = (valorNaPaleta & 0x0F);
                                tile.SetPixel(x, y, CoresPaleta[nibbleBaixo]);
                                tile.SetPixel(x + 1, y, CoresPaleta[nibbleAlto]);
                            }

                        }
                    }

                    tiles.Add(tile);
                    // tile.Dispose();

                }

            }

            return tiles;
        }

        public Bitmap ConvertaBytesParaTileModo2(byte[] imagem, string bpp, int largura, int altura)
        {
            Bitmap tiles = new Bitmap(largura, altura);

            int complemento = 1;

            if (bpp.Contains("4"))
            {
                complemento = 2;
            }

            using (BinaryReader br = new BinaryReader(new MemoryStream(imagem)))
            {
                while (br.BaseStream.Position < imagem.Length)
                {
                    for (int y = 0; y < altura; y++)
                    {
                        for (int x = 0; x < largura; x += complemento)
                        {
                            if (bpp.Contains("8"))
                            {
                                int valorNaPaleta = br.ReadByte();
                                tiles.SetPixel(x, y, CoresPaleta[valorNaPaleta]);
                            }
                            else
                            {
                                int valorNaPaleta = br.ReadByte();
                                int nibbleAlto = (valorNaPaleta & 0xF0) >> 4;
                                int nibbleBaixo = (valorNaPaleta & 0x0F);
                                tiles.SetPixel(x, y, CoresPaleta[nibbleBaixo]);
                                tiles.SetPixel(x + 1, y, CoresPaleta[nibbleAlto]);
                            }

                        }
                    }


                    // tiles.Add(tile);
                    // tile.Dispose();

                }

            }

            return tiles;
        }

        public Bitmap LeiaTileMapERetorneImagem(byte[] tileMap, List<Bitmap> tiles, Bitmap imagemFinal)
        {
            int contadorTileMap = 0;
            if (TileMap.Length == 0x600)
            {
                imagemFinal.Dispose();
                imagemFinal = new Bitmap(256, 192);
            }
            else if (TileMap.Length == 0x800)
            {
                imagemFinal.Dispose();
                imagemFinal = new Bitmap(256, 256);
            }
            Graphics g = Graphics.FromImage(imagemFinal);
            List<int> tilesMapLido = new List<int>();

            using (BinaryReader br = new BinaryReader(new MemoryStream(tileMap)))
            {
                while (br.BaseStream.Position < tileMap.Length)
                {
                    tilesMapLido.Add(br.ReadInt16());
                }
            }

            for (int y = 0; y < imagemFinal.Height; y += 8)
            {
                for (int x = 0; x < imagemFinal.Width; x += 8)
                {
                    /*   if (tilesMapLido[contadorTileMap] < tiles.Count)
                       {
                           g.DrawImage(tiles[tilesMapLido[contadorTileMap]], x, y);
                       }
                       else
                       {
                           g.DrawImage(tiles[0], x, y);
                       }
                    */

                    int valor = tilesMapLido[contadorTileMap];
                    int tileNum = valor & 0x3FF;

                    Bitmap tile = new Bitmap(tiles[tileNum]);

                    if (valor > tiles.Count())
                    {

                    }

                    valor = valor >> 10;
                    int horizotal = valor & 1;
                    valor = valor >> 1;
                    int vertical = valor & 1;

                    if (horizotal == 1)
                    {

                        tile.RotateFlip(RotateFlipType.Rotate180FlipY);
                        //tile.Save("teste.png");
                    }

                    if (vertical == 1)
                    {
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        // tile.Save("teste.png");
                    }

                    g.DrawImage(tile, x, y);
                    contadorTileMap++;
                    tile.Dispose();
                }
            }

            g.Dispose();
            Bitmap final = new Bitmap(imagemFinal);
            imagemFinal.Dispose();
            DisposeListaBmp(tiles);
            return final;

        }

        public Bitmap ExporteImagemSemTileMap(List<Bitmap> tiles, Bitmap imagemFinal)
        {

            Graphics g = Graphics.FromImage(imagemFinal);
            List<int> tilesMapLido = new List<int>();

            if (tiles.Count > 0)
            {
                int contadorTiles = 0;

                for (int y = 0; y < imagemFinal.Height; y += 8)
                {
                    for (int x = 0; x < imagemFinal.Width; x += 8)
                    {
                        g.DrawImage(tiles[contadorTiles], x, y);
                        tiles[contadorTiles].Dispose();
                        contadorTiles++;

                    }
                }
            }



            g.Dispose();

            Bitmap final = new Bitmap(imagemFinal);
            imagemFinal.Dispose();
            return final;

        }

        #endregion

        #region Importar

        public byte[] ConvertaPara1bppBitmap(Bitmap imagemEditada)
        {
                       

            CoresPaleta = new List<Color>();
            CoresPaleta.Add(Color.FromArgb(0, 0, 0));
            CoresPaleta.Add(Color.FromArgb(255, 255, 255));
            int contador = 0;

            var bits = new BitArray(imagemEditada.Width * imagemEditada.Height);

            for (int y = 0; y < imagemEditada.Height; y++)
            {
                for (int x = 0; x < imagemEditada.Width; x++)
                {
                   // int valor = bits[contador] ? 1 : 0;
                    int valor = ObtenhaIndexCorMaisProxima(imagemEditada.GetPixel(x, y), CoresPaleta);
                    bits[contador] = valor == 1 ? true : false;
                    contador++;
                }
            }

            byte[] imagembytes = new byte[bits.Count / 8];
            bits.CopyTo(imagembytes,0);

            return imagembytes;
        }

        public byte[] ConvertaPara2bppBitmap(Bitmap imagemEditada)
        {
            CoresPaleta = new List<Color>();
            CoresPaleta.Add(Color.FromArgb(0, 0, 0));
            CoresPaleta.Add(Color.FromArgb(255, 255, 255));
            CoresPaleta.Add(Color.FromArgb(169, 169, 169));
            CoresPaleta.Add(Color.FromArgb(100, 100, 100));
            int contador = 0;

            var bits = new BitArray((imagemEditada.Width * imagemEditada.Height) * 2);

            for (int y = 0; y < imagemEditada.Height; y++)
            {
                for (int x = 0; x < imagemEditada.Width; x++)
                {

                    int valor = ObtenhaIndexCorMaisProxima(imagemEditada.GetPixel(x, y), CoresPaleta);
                    int v1 = valor & 1;
                    int v2 = valor >> 1;
                    bits[contador] = v1 == 1 ? true : false;
                    bits[contador + 1] = v2 == 1 ? true : false;
                    contador +=2;
                }
            }

            byte[] imagembytes = new byte[bits.Count / 8];
            bits.CopyTo(imagembytes, 0);

            return imagembytes;
        }

        public void Insira4bppTile(string dirImg, string imgEmBytes, string dirTileMap)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));
            var unicas = ObtenhaTilesUnicas(tiles);


            byte[] novoTilemapFinal = CriarNovoTileMap(tiles, unicas);
            List<byte> imagembytes = new List<byte>();

            foreach (var tile in unicas)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x += 2)
                    {
                        int valor1 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        int valor2 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x + 1, y), CoresPaleta) << 4;
                        valor2 = valor2 + valor1;
                        imagembytes.Add((byte)valor2);
                    }
                }


            }

            byte[] imagemFinal = InsirarTilesBytes(imagembytes.ToArray());

            File.WriteAllBytes(imgEmBytes, imagemFinal);
            File.WriteAllBytes(dirTileMap, novoTilemapFinal);
        }

        public void Insira4bppRaw(string dirImg, string imgEmBytes)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));

            List<byte> imagembytes = new List<byte>();

            foreach (var tile in tiles)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x += 2)
                    {

                        int valor1 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        int valor2 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x + 1, y), CoresPaleta) << 4;
                        valor2 = valor2 + valor1;
                        imagembytes.Add((byte)valor2);
                    }
                }


            }

            byte[] imagemFinal = InsirarTilesBytes(imagembytes.ToArray());
            File.WriteAllBytes(imgEmBytes, imagemFinal);

        }

        public byte[] Insira4bppBitmap(string dirImg)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));

            Bitmap imagemEditada = new Bitmap(dirImg);

            List<byte> imagembytes = new List<byte>();

            for (int y = 0; y < imagemEditada.Height; y++)
            {
                for (int x = 0; x < imagemEditada.Width; x += 2)
                {

                    int valor1 = ObtenhaIndexCorMaisProxima(imagemEditada.GetPixel(x, y), CoresPaleta);
                    int valor2 = ObtenhaIndexCorMaisProxima(imagemEditada.GetPixel(x + 1, y), CoresPaleta) << 4;
                    valor2 = valor2 + valor1;
                    imagembytes.Add((byte)valor2);
                }
            }


            imagemEditada.Dispose();

            return imagembytes.ToArray();


        }

        public void Insira8bppTile(string dirImg, string imgEmBytes, string dirTileMap)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));
            var unicas = ObtenhaTilesUnicas(tiles);


            byte[] novoTilemapFinal = CriarNovoTileMap(tiles, unicas);
            List<byte> imagembytes = new List<byte>();

            foreach (var tile in unicas)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x++)
                    {
                        int valor = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        imagembytes.Add((byte)valor);
                    }
                }


            }

            byte[] imagemFinal = InsirarTilesBytes(imagembytes.ToArray());

            File.WriteAllBytes(imgEmBytes, imagemFinal);
            File.WriteAllBytes(dirTileMap, novoTilemapFinal);
        }

        public void Insira8bppRaw(string dirImg, string imgEmBytes)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));

            List<byte> imagembytes = new List<byte>();

            foreach (var tile in tiles)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x++)
                    {
                        int valor = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        imagembytes.Add((byte)valor);
                    }
                }


            }

            byte[] imagemFinal = InsirarTilesBytes(imagembytes.ToArray());
            File.WriteAllBytes(imgEmBytes, imagemFinal);

        }

       

        public byte[] Insira8bppBitmap(string dirImg)
        {
            Bitmap imagemEditada = new Bitmap(dirImg);

            List<byte> imagembytes = new List<byte>();


            for (int y = 0; y < imagemEditada.Height; y++)
            {
                for (int x = 0; x < imagemEditada.Width; x++)
                {
                    int valor = ObtenhaIndexCorMaisProxima(imagemEditada.GetPixel(x, y), CoresPaleta);
                    imagembytes.Add((byte)valor);
                }
            }



            imagemEditada.Dispose();
            return imagembytes.ToArray();


        }

        public byte[] Converta4bppRawNcgr(string dirImg)
        {
            Bitmap imagemEditada = new Bitmap(dirImg);
          

            var tiles = ObtenhaTilesDeImagem(imagemEditada);
            imagemEditada.Dispose();
            List<byte> imagembytes = new List<byte>();


            foreach (var tile in tiles)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x += 2)
                    {

                        int valor1 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        int valor2 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x + 1, y), CoresPaleta) << 4;
                        valor2 = valor2 + valor1;
                        imagembytes.Add((byte)valor2);
                    }
                }


            }

            return imagembytes.ToArray();

        }


        public byte[] Converta8bppRawNcgr(string dirImg)
        {
            Bitmap imagemEditada = new Bitmap(dirImg);
          
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));
            imagemEditada.Dispose();

            List<byte> imagembytes = new List<byte>();

           

            foreach (var tile in tiles)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x++)
                    {
                        int valor = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        imagembytes.Add((byte)valor);
                    }
                }


            }

            return imagembytes.ToArray();


        }

        public List<byte[]> Converta4bppNcgrTileMap(string dirImg)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));
            var unicas = ObtenhaTilesUnicas(tiles);

            List<byte[]> imagemETilemap = new List<byte[]>();

            byte[] novoTilemapFinal = CriarNovoTileMap(tiles, unicas);

            imagemETilemap.Add(novoTilemapFinal);

            List<byte> imagembytes = new List<byte>();

            foreach (var tile in unicas)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x += 2)
                    {
                        int valor1 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        int valor2 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x + 1, y), CoresPaleta) << 4;
                        valor2 = valor2 + valor1;
                        imagembytes.Add((byte)valor2);
                    }
                }


            }

            imagemETilemap.Add(imagembytes.ToArray());

            return imagemETilemap;


        }

        public List<byte[]> Converta8bppNcgrTileMap(string dirImg)
        {
            var tiles = ObtenhaTilesDeImagem(new Bitmap(dirImg));
            var unicas = ObtenhaTilesUnicas(tiles);

            List<byte[]> imagemETilemap = new List<byte[]>();

            byte[] novoTilemapFinal = CriarNovoTileMap(tiles, unicas);

            imagemETilemap.Add(novoTilemapFinal);

            List<byte> imagembytes = new List<byte>();

            foreach (var tile in unicas)
            {
                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x++)
                    {
                        int valor = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                        imagembytes.Add((byte)valor);
                    }
                }


            }

            imagemETilemap.Add(imagembytes.ToArray());

            return imagemETilemap;
        }

        public byte[] InsiraSpriteNgcr(string dirImg, List<Oam> valoresOam)
        {
            
            
            Bitmap imagemASerCortada = new Bitmap(dirImg);

            if (imagemASerCortada.Height != 256)
            {
                throw new Exception("A imagem possui mais ou menos que 256px de altura.");
            }

            if (imagemASerCortada.Width != 512)
            {
                throw new Exception("A imagem possui mais ou menos que 512px de largura.");
            }

            foreach (var oam in valoresOam)
            {
                int x = (int)oam.X;
                int y = (int)oam.Y;

               

                Rectangle rect = new Rectangle(x, y, (int)oam.Largura, (int)oam.Altura);
                Bitmap pedaco = imagemASerCortada.Clone(rect, imagemASerCortada.PixelFormat);
    
                if (oam.HorizontalFlip && oam.VerticalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                }
                else if (oam.HorizontalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                else if (oam.VerticalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                byte[] pedacoEmbytes = LeiaImagemERetorneBytes(pedaco, oam.Bpp, (int)oam.NumeroDaPaleta, false);

                int offsePedacoImg = 0;


               offsePedacoImg = (int)(oam.TileId * 0x80);

                // pedaco.Save("teste.png");

                Imagem = InsiraBytesImgOam(pedacoEmbytes, Imagem, offsePedacoImg);

                pedaco.Dispose();
            }

            imagemASerCortada.Dispose();

           

            return Imagem;
        }


        public byte[] InsiraSpriteNgcrModo2(string dirImg, List<Oam> valoresOam)
        {


            Bitmap imagemASerCortada = new Bitmap(dirImg);


            foreach (var oam in valoresOam)
            {
                int x = (int)oam.X;
                int y = (int)oam.Y;



                Rectangle rect = new Rectangle(x, y, (int)oam.Largura, (int)oam.Altura);
                Bitmap pedaco = imagemASerCortada.Clone(rect, imagemASerCortada.PixelFormat);
                pedaco.Save("oam.png");
                if (oam.HorizontalFlip && oam.VerticalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                }
                else if (oam.HorizontalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                else if (oam.VerticalFlip)
                {
                    pedaco.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                byte[] pedacoEmbytes = LeiaImagemERetorneBytesModo2(pedaco, oam.Bpp, (int)oam.NumeroDaPaleta, false);

                int offsePedacoImg = 0;


                offsePedacoImg = (int)(oam.TileId * 0x80);

                // pedaco.Save("teste.png");

                Imagem = InsiraBytesImgOam(pedacoEmbytes, Imagem, offsePedacoImg);

                pedaco.Dispose();
            }

            imagemASerCortada.Dispose();



            return Imagem;
        }


        private byte[] InsiraBytesImgOam(byte[] bytesAserInserido, byte[] bytesImg, int offset)
        {
            byte[] final = bytesImg;
            MemoryStream mm = new MemoryStream(bytesImg);

            using (BinaryWriter bw = new BinaryWriter(mm))
            {
                bw.BaseStream.Position = offset;
                bw.Write(bytesAserInserido);

                final = mm.ToArray();
            }

            return final;
        }

        public byte[] LeiaImagemERetorneBytes(Bitmap parteImg, Bpp bpp, int idPaleta,bool fundoTransparente)
        {


            List<byte> imgEmBytes = new List<byte>();

            ConvertaPaletaOam(Paleta, bpp, idPaleta, fundoTransparente);

            List<Bitmap> tiles = ObtenhaTilesDeImagem(parteImg);


            foreach (var tile in tiles)
            {
                int complemento = 1;

                if (bpp == Bpp.bpp4)
                {
                    complemento += 1;
                }

                for (int y = 0; y < tile.Height; y++)
                {
                    for (int x = 0; x < tile.Width; x += complemento)
                    {
                        if (bpp == Bpp.bpp4)
                        {
                            int valor1 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                            int valor2 = ObtenhaIndexCorMaisProxima(tile.GetPixel(x + 1, y), CoresPaleta) << 4;
                            valor2 = valor2 + valor1;
                            imgEmBytes.Add((byte)valor2);
                        }
                        else
                        {
                            int valor = ObtenhaIndexCorMaisProxima(tile.GetPixel(x, y), CoresPaleta);
                            imgEmBytes.Add((byte)valor);
                        }
                    }
                }

                tile.Dispose();
            }



            return imgEmBytes.ToArray();
        }

        public byte[] LeiaImagemERetorneBytesModo2(Bitmap parteImg, Bpp bpp, int idPaleta, bool fundoTransparente)
        {


            List<byte> imgEmBytes = new List<byte>();

            ConvertaPaletaOam(Paleta, bpp, idPaleta, fundoTransparente);

           


           
                int complemento = 1;

                if (bpp == Bpp.bpp4)
                {
                    complemento += 1;
                }

                for (int y = 0; y < parteImg.Height; y++)
                {
                    for (int x = 0; x < parteImg.Width; x += complemento)
                    {
                        if (bpp == Bpp.bpp4)
                        {
                            int valor1 = ObtenhaIndexCorMaisProxima(parteImg.GetPixel(x, y), CoresPaleta);
                            int valor2 = ObtenhaIndexCorMaisProxima(parteImg.GetPixel(x + 1, y), CoresPaleta) << 4;
                            valor2 = valor2 + valor1;
                            imgEmBytes.Add((byte)valor2);
                        }
                        else
                        {
                            int valor = ObtenhaIndexCorMaisProxima(parteImg.GetPixel(x, y), CoresPaleta);
                            imgEmBytes.Add((byte)valor);
                        }
                    }
                }
           


            return imgEmBytes.ToArray();
        }

        public List<Bitmap> ObtenhaTilesDeImagem(Bitmap img)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            for (int y = 0; y < img.Height; y += 8)
            {
                for (int x = 0; x < img.Width; x += 8)
                {
                    Rectangle rec = new Rectangle(x, y, 8, 8);
                    tiles.Add(img.Clone(rec, img.PixelFormat));

                }
            }

            img.Dispose();

            return tiles;
        }

        private byte[] CriarNovoTileMap(List<Bitmap> tiles, List<Bitmap> unicas)
        {
            List<ushort> tilemap = new List<ushort>();

            foreach (var tile in tiles)
            {
                int contador2 = 0;

                foreach (var item in unicas)
                {

                    if (CompareTiles(tile, item))
                    {
                        break;
                    }

                    contador2++;
                }

                tilemap.Add((ushort)contador2);
            }

            byte[] novoTileMapFinal = new byte[tilemap.Count * 2];
            MemoryStream ntmf = new MemoryStream(novoTileMapFinal);
            using (BinaryWriter br = new BinaryWriter(ntmf))
            {
                foreach (var item in tilemap)
                {
                    br.Write(item);
                }

                novoTileMapFinal = ntmf.ToArray();
            }

            return novoTileMapFinal;
        }

        public bool CompareTiles(Bitmap bmp1, Bitmap bmp2)
        {
            List<bool> pixels = new List<bool>();
            List<Color> cores1 = new List<Color>();
            List<Color> cores2 = new List<Color>();

            for (int y1 = 0; y1 < 8; y1++)
            {
                for (int x1 = 0; x1 < 8; x1++)
                {
                    Color c1 = bmp1.GetPixel(x1, y1);
                    cores1.Add(c1);
                }
            }

            for (int y1 = 0; y1 < 8; y1++)
            {
                for (int x1 = 0; x1 < 8; x1++)
                {
                    Color c1 = bmp2.GetPixel(x1, y1);
                    cores2.Add(c1);
                }
            }

            for (int i = 0; i < cores1.Count; i++)
            {
                pixels.Add(CoresSaoIguais(cores1[i], cores2[i]));
            }

            bool resultado = true;

            foreach (var item in pixels)
            {
                if (item == false)
                {
                    resultado = false;
                    break;
                }
            }




            return resultado;
        }

        public List<Bitmap> ObtenhaTilesUnicas(List<Bitmap> tilesImg)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            foreach (var item in tilesImg)
            {
                bool existe = false;

                foreach (var tl in tiles)
                {
                    if (CompareTiles(item, tl))
                    {
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                {
                    tiles.Add(new Bitmap(item));
                }


            }


            return tiles;
        }



        private bool CoresSaoIguais(Color color1, Color color2)
        {
            return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }

        public int ObtenhaIndexCorMaisProxima(Color c1, List<Color> paleta)
        {
            int eOmenor = 10000;
            Color c2;
            Color corSimilar = Color.FromArgb(0, 0, 0, 0);

            for (int x = 0; x < paleta.Count; x++)
            {
                c2 = paleta[x];

                int vl = DiferencaDeCores(c1, c2);

                if (eOmenor > vl)
                {
                    eOmenor = vl;
                    corSimilar = paleta[x];
                }

            }

            return paleta.IndexOf(corSimilar);
        }

        private int DiferencaDeCores(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R) + (c1.G - c2.G) * (c1.G - c2.G) + (c1.B - c2.B) * (c1.B - c2.B) + (c1.A - c2.A) * (c1.A - c2.A));
        }

        private byte[] InsirarTilesBytes(byte[] nTiles)
        {
            byte[] tiles = new byte[nTiles.Length];
            MemoryStream newTiles = new MemoryStream(tiles);
            using (BinaryWriter bw = new BinaryWriter(newTiles))
            {
                bw.Write(nTiles);
                tiles = newTiles.ToArray();
            }

            return tiles;
        }

        #endregion

        #region Auxiliares

        public void ConvertaPaleta(byte[] paleta)
        {

            using (BinaryReader br = new BinaryReader(new MemoryStream(paleta)))
            {

                while (br.BaseStream.Position < paleta.Length)
                {
                    int bgr = br.ReadInt16();

                    int r = (bgr & 31) * 255 / 31;
                    int g = (bgr >> 5 & 31) * 255 / 31;
                    int b = (bgr >> 10 & 31) * 255 / 31;

                    CoresPaleta.Add(Color.FromArgb(r, g, b));
                }
            }
        }



        private List<Oam> ObtenhaTabelaOam(string arquivoTblOam, int idxTabela)
        {

            List<Oam> valoresOam = new List<Oam>();

            using (BinaryReader br = new BinaryReader(File.Open(arquivoTblOam, FileMode.Open)))
            {
                br.BaseStream.Position = idxTabela;


                while (true)
                {
                    ushort atb1 = br.ReadUInt16();



                    if (atb1 == 0xFFFF)
                    {
                        break;
                    }
                    ushort atb2 = br.ReadUInt16();
                    ushort atb3 = br.ReadUInt16();

                    valoresOam.Add(new Oam(atb1, atb2, atb3));

                    br.BaseStream.Position += 2;

                }
            }

            return valoresOam;
        }

        private void UtilizeTabelaOamEExporteSprite(List<Oam> valoresOam, string localDoGrafico, string aai, bool fundoTransparente)
        {




            int contador = 0;
            string bpp = "" + valoresOam.First().Bpp;



            using (BinaryReader br = new BinaryReader(new MemoryStream(Imagem)))
            {
                //  int oo = 0;
                foreach (var infoOam in valoresOam)
                {
                    ConvertaPaletaOam(Paleta, infoOam.Bpp, (int)infoOam.NumeroDaPaleta, fundoTransparente);
                    

                    infoOam.Paleta = CoresPaleta;

                    byte[] imgTemp = new byte[] { };
                    int totalBpp = 0;

                    if (infoOam.Bpp == Bpp.bpp4)
                    {
                        imgTemp = new byte[(infoOam.Largura * infoOam.Altura) / 2];
                        totalBpp = 128;
                    }
                    else
                    {
                        imgTemp = new byte[infoOam.Largura * infoOam.Altura];
                        totalBpp = 128;
                    }
                    Array.Copy(Imagem, infoOam.TileId * totalBpp, imgTemp, 0, imgTemp.Length);
                    List<Bitmap> tiles = ConvertaBytesParaTile(imgTemp, bpp);

                    if (infoOam.Bpp == Bpp.bpp8)
                    {

                        // List<Bitmap> tilesOam = tiles.Take((int)((infoOam.Largura / 8) * (infoOam.Altura / 8))).ToList();
                        Bitmap pedacao = new Bitmap((int)infoOam.Largura, (int)infoOam.Altura);
                        Bitmap tile = new Bitmap(ExporteImagemSemTileMap((tiles), pedacao));
                        pedacao.Dispose();

                        if (infoOam.HorizontalFlip && infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (infoOam.HorizontalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else if (infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }





                        int x = (int)infoOam.X;
                        int y = (int)infoOam.Y;
                        infoOam.Imagem = tile;
                        infoOam.Retangulo = new Rectangle((int)infoOam.X, (int)infoOam.Y, tile.Width, tile.Height);
                        // Rectangle rectangle = new Rectangle(x, (int)infoOam.Y, tile.Width, tile.Height);
                        // g.DrawImage(tile, rectangle);

                        //tile.Dispose();


                    }
                    else
                    {
                        infoOam.Paleta = CoresPaleta;

                        int x = (int)infoOam.X;
                        int y = (int)infoOam.Y;


                        // List<Bitmap> tilesOam = tiles.Skip((int)infoOam.TileId * 4).Take((int)((infoOam.Largura / 8) * (infoOam.Altura / 8))).ToList();
                        Bitmap pedacao = new Bitmap((int)infoOam.Largura, (int)infoOam.Altura);

                        Bitmap tile = new Bitmap(ExporteImagemSemTileMap((tiles), pedacao));
                        pedacao.Dispose();


                        if (infoOam.HorizontalFlip && infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (infoOam.HorizontalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else if (infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }

                        infoOam.Imagem = tile;
                        infoOam.Retangulo = new Rectangle((int)infoOam.X, (int)infoOam.Y, tile.Width,tile.Height);

                        // tile.Save("8bpp1\\" + oo + ".png");
                        // oo++;

                        //   Rectangle rectangle = new Rectangle(x, y, tile.Width, tile.Height);
                        //    g.DrawImage(tile, rectangle);

                        // tile.Dispose();
                        // DisposeListaBmp(tilesOam);
                        contador++;
                    }

                    DisposeListaBmp(tiles);
                }


            }


            valoresOam.Reverse();


        }

        private void UtilizeTabelaOamEExporteSpriteModo2(List<Oam> valoresOam, bool fundoTransparente)
        {


           
            string bpp = "" + valoresOam.First().Bpp;
            //  ConvertaPaletaOam(Paleta, valoresOam.First().Bpp, (int)valoresOam.First().NumeroDaPaleta);


            using (BinaryReader br = new BinaryReader(new MemoryStream(Imagem)))
            {


                //  int oo = 0;
                foreach (var infoOam in valoresOam)
                {
                    ConvertaPaletaOam(Paleta, infoOam.Bpp, (int)infoOam.NumeroDaPaleta,fundoTransparente);
                    infoOam.Paleta = CoresPaleta;
                    

                    if (infoOam.Bpp == Bpp.bpp8)
                    {

                        // List<Bitmap> tilesOam = tiles.Skip((int)infoOam.TileId * 2).Take((int)((infoOam.Largura / 8) * (infoOam.Altura / 8))).ToList();
                        //   Bitmap pedacao = new Bitmap((int)infoOam.Largura, (int)infoOam.Altura);
                        byte[] imgTemp = new byte[infoOam.Largura * infoOam.Altura];
                        Array.Copy(Imagem, infoOam.TileId * 0x80, imgTemp, 0, imgTemp.Length);

                        Bitmap tile = ConvertaBytesParaTileModo2(imgTemp, bpp, (int)infoOam.Largura, (int)infoOam.Altura);
                        int x = (int)infoOam.X;
                        int y = (int)infoOam.Y;
                        if (infoOam.HorizontalFlip && infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (infoOam.HorizontalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else if (infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }

                        infoOam.Imagem = tile;
                        infoOam.Retangulo = new Rectangle((int)infoOam.X, (int)infoOam.Y, tile.Width, tile.Height);

                    }
                    else
                    {

                        int x = (int)infoOam.X;
                        int y = (int)infoOam.Y;


                        byte[] imgTemp = new byte[(infoOam.Largura * infoOam.Altura) / 2];
                        int offsetDosTilesOam = (int)infoOam.TileId * 0x80;
                        Array.Copy(Imagem, offsetDosTilesOam, imgTemp, 0, imgTemp.Length);

                        Bitmap tile = ConvertaBytesParaTileModo2(imgTemp, bpp, (int)infoOam.Largura, (int)infoOam.Altura);


                        if (infoOam.HorizontalFlip && infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (infoOam.HorizontalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else if (infoOam.VerticalFlip)
                        {
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }

                        infoOam.Imagem = tile;
                        infoOam.Retangulo = new Rectangle((int)infoOam.X, (int)infoOam.Y, tile.Width, tile.Height);
                       

                    }


                }


            }




        }

        private void ConvertaPaletaOam(byte[] paleta, Bpp bpp, int idPaleta, bool fundoTransparente)
        {

            CoresPaleta = new List<Color>();

            using (BinaryReader br = new BinaryReader(new MemoryStream(paleta)))
            {
                int contador = 0;

                if (bpp == Bpp.bpp4)
                {
                    int inicio = idPaleta * 32;
                    br.BaseStream.Position = inicio;

                    if (inicio >= br.BaseStream.Length)
                    {
                        br.BaseStream.Position = br.BaseStream.Length - 32;
                    }

                    while (contador < 32)
                    {
                        
                        int bgr = br.ReadInt16();

                        int r = (bgr & 31) * 255 / 31;
                        int g = (bgr >> 5 & 31) * 255 / 31;
                        int b = (bgr >> 10 & 31) * 255 / 31;

                        CoresPaleta.Add(Color.FromArgb(r, g, b));
                        contador += 2;
                    }
                }
                else
                {
                    while (br.BaseStream.Position < paleta.Length)
                    {
                        int bgr = br.ReadInt16();

                        int r = (bgr & 31) * 255 / 31;
                        int g = (bgr >> 5 & 31) * 255 / 31;
                        int b = (bgr >> 10 & 31) * 255 / 31;

                        CoresPaleta.Add(Color.FromArgb(r, g, b));
                    }
                }


            }

            if (fundoTransparente)
            {
                CoresPaleta[0] = Color.FromArgb(0, 0, 0, 0);
            }

           
        }

        public void DisposeListaBmp(List<Bitmap> bitmaps)
        {
            foreach (var item in bitmaps)
            {
                item.Dispose();
            }
        }


        #endregion

    }
}
