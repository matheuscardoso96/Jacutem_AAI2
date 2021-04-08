using Jacutem_AAI2.Imagens.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Jacutem_AAI2.Imagens
{
    public class Oam
    {
        public ushort _atributosOBJ0 { get; set; }
        public ushort _atributosOBJ1 { get; set; }
        public ushort _atributosOBJ2 { get; set; }
        public uint Y { get; set; }
        public uint X { get; set; }
        public uint Largura { get; set; }
        public uint Altura { get; set; }
        public uint TileId { get; set; }
        public uint Prioridade { get; set; }
        public uint NumeroDaPaleta { get; set; }
        public bool RotateOrScaling { get; set; }
        public bool Mozaico { get; set; }
        public bool HorizontalFlip { get; set; }
        public bool VerticalFlip { get; set; }
        public OBJShape OBJShape { get; set; }
        public OBJMode OBJMode { get; set; }
        public Bpp Bpp { get; set; }
        public Bitmap Imagem { get; set; }
        public List<Color> Paleta{ get; set; }
        public Rectangle Retangulo { get; set; }


        public Oam(ushort atributosOBJ0, ushort atributosOBJ1, ushort atributosOBJ2)
        {
            _atributosOBJ0 = atributosOBJ0;
            _atributosOBJ1 = atributosOBJ1;
            _atributosOBJ2 = atributosOBJ2;
            ObtenhaAtribuosObj0(atributosOBJ0);
            ObtenhaAtribuosObj1(atributosOBJ1);
            ObtenhaAtribuosObj2(atributosOBJ2);
            Retangulo = new Rectangle((int) X, (int) Y, (int) Largura, (int) Altura);


        }

        public Oam(Oam oam, int resX, int resY)
        {
            _atributosOBJ0 = oam._atributosOBJ0;
            _atributosOBJ1 = oam._atributosOBJ1;
            _atributosOBJ2 = oam._atributosOBJ2;
            ObtenhaAtribuosObj0(oam._atributosOBJ0);
            ObtenhaAtribuosObj1(oam._atributosOBJ1);
            ObtenhaAtribuosObj2(oam._atributosOBJ2);
            Imagem = new Bitmap(resX, resY);
            using (Graphics gg = Graphics.FromImage(Imagem))
            {
                SolidBrush sb = new SolidBrush(oam.Paleta[0]);
                Rectangle rect = new Rectangle(0, 0, resX, resY);
                gg.FillRectangle(sb, rect);
                gg.Dispose();
                sb.Dispose();
                Retangulo = rect;
            }
            Paleta = oam.Paleta;

        }

        private void ObtenhaAtribuosObj0(ushort atb0)
        {
            Y = (uint)(atb0 & 0XFF);
           // Y += 128;

              if (Y >= 128)
                Y -= 128;
              else
                 Y += 128;

          //  if (Y >= 256)
           // {
             //   Y = Y - 256;
            //}

            atb0 = (ushort)(atb0 >> 8);
            RotateOrScaling = ((atb0 & 1) == 0) ? false : true;
            atb0 = (ushort)(atb0 >> 1);

            if (RotateOrScaling)
            {

            }
            else
                atb0 = (ushort)(atb0 >> 1);

            OBJMode = (OBJMode)(atb0 & 3);
            atb0 = (ushort)(atb0 >> 2);
            Mozaico = ((atb0 & 1) == 0) ? false : true;
            atb0 = (ushort)(atb0 >> 1);
            Bpp = (Bpp)(atb0 & 1);
            atb0 = (ushort)(atb0 >> 1);
            OBJShape = (OBJShape)(atb0 & 3);

        }

        private void ObtenhaAtribuosObj1(ushort atb1)
        {
            X = (uint)(atb1 & 0X1FF);
           // X += 256;

         //   if (X >= 512)
          // {
            //    X = X - 512;
            //}
            if (X >= 256)
            {
                X -= 256;
            }
            else
            {
                X += 256;
            }

            atb1 = (ushort)(atb1 >> 9);


            if (RotateOrScaling)
            {
                atb1 = (ushort)(atb1 >> 5);
            }
            else
            {
                atb1 = (ushort)(atb1 >> 3);
                HorizontalFlip = ((atb1 & 1) == 0) ? false : true;
                atb1 = (ushort)(atb1 >> 1);
                VerticalFlip = ((atb1 & 1) == 0) ? false : true;
                atb1 = (ushort)(atb1 >> 1);
            }

            uint size = (uint)atb1 & 3;

            switch (OBJShape)
            {
                case OBJShape.Square:
                    Largura = (uint)(8 << (int)size);
                    Altura = (uint)(8 << (int)size);

                    break;
                case OBJShape.Horizontal:
                    if (size == 0)
                    {
                        Largura = 16;
                        Altura = 8;

                    }
                    else if (size == 1)
                    {
                        Largura = 32;
                        Altura = 8;
                    }
                    else if (size == 2)
                    {
                        Largura = 32;
                        Altura = 16;
                    }
                    else if (size == 3)
                    {
                        Largura = 64;
                        Altura = 32;
                    }
                    break;
                case OBJShape.Vertical:
                    if (size == 0)
                    {
                        Largura = 8;
                        Altura = 16;

                    }
                    else if (size == 1)
                    {
                        Largura = 8;
                        Altura = 32;
                    }
                    else if (size == 2)
                    {
                        Largura = 16;
                        Altura = 32;
                    }
                    else if (size == 3)
                    {
                        Largura = 32;
                        Altura = 64;
                    }
                    break;
                case OBJShape.Prohibited:
                    break;
                default:
                    break;
            }



        }

        private void ObtenhaAtribuosObj2(ushort atb2)
        {
            TileId = (uint)(atb2 & 0X3FF);
            atb2 = (ushort)(atb2 >> 10);
            Prioridade = (uint)(atb2 & 3);
            atb2 = (ushort)(atb2 >> 2);
            NumeroDaPaleta = (uint)(atb2 & 0xF);
        }
    }
}
