using LibDeImagensGbaDs.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace LibDeImagensGbaDs.Sprites
{
    public class Oam
    {
        public ushort OBJ0Attributes { get; set; }
        public ushort OBJ1Attributes { get; set; }
        public ushort OBJ2Attributes { get; set; }
        public uint Y { get; set; }
        public uint X { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint TileId { get; set; }
        public uint Priority { get; set; }
        public uint PaletteId { get; set; }
        public bool RotateOrScaling { get; set; }
        public bool Mozaic { get; set; }
        public bool HorizontalFlip { get; set; }
        public bool VerticalFlip { get; set; }
        public OBJShape OBJShape { get; set; }
        public OBJMode OBJMode { get; set; }
        public Bpp Bpp { get; set; }


        public Oam(ushort atributosOBJ0, ushort atributosOBJ1, ushort atributosOBJ2)
        {
            OBJ0Attributes = atributosOBJ0;
            OBJ1Attributes = atributosOBJ1;
            OBJ2Attributes = atributosOBJ2;
            GetObj0Attributes(atributosOBJ0);
            GetObj1Attributes(atributosOBJ1);
            GetObj2Attributes(atributosOBJ2);


        }

        public Oam(Oam oam, int resX, int resY)
        {
            OBJ0Attributes = oam.OBJ0Attributes;
            OBJ1Attributes = oam.OBJ1Attributes;
            OBJ2Attributes = oam.OBJ2Attributes;
            GetObj0Attributes(oam.OBJ0Attributes);
            GetObj1Attributes(oam.OBJ1Attributes);
            GetObj2Attributes(oam.OBJ2Attributes);
            //Imagem = new Bitmap(resX, resY);
            //using (Graphics gg = Graphics.FromImage(Imagem))
            //{
            //    SolidBrush sb = new SolidBrush(oam.Paleta[0]);
            //    Rectangle rect = new Rectangle(0, 0, resX, resY);
            //    gg.FillRectangle(sb, rect);
            //    gg.Dispose();
            //    sb.Dispose();
            //    Rectangle = rect;
            //}
            //Paleta = oam.Paleta;

        }

        private void GetObj0Attributes(ushort atb0)
        {
            Y = (uint)(atb0 & 0XFF);

              if (Y >= 128)
                Y -= 128;
              else
                 Y += 128;

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
            Mozaic = ((atb0 & 1) == 0) ? false : true;
            atb0 = (ushort)(atb0 >> 1);
            Bpp = (Bpp)(atb0 & 1);
            atb0 = (ushort)(atb0 >> 1);
            OBJShape = (OBJShape)(atb0 & 3);

        }

        private void GetObj1Attributes(ushort atb1)
        {
            X = (uint)(atb1 & 0X1FF);

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
                    Width = (uint)(8 << (int)size);
                    Height = (uint)(8 << (int)size);

                    break;
                case OBJShape.Horizontal:
                    if (size == 0)
                    {
                        Width = 16;
                        Height = 8;

                    }
                    else if (size == 1)
                    {
                        Width = 32;
                        Height = 8;
                    }
                    else if (size == 2)
                    {
                        Width = 32;
                        Height = 16;
                    }
                    else if (size == 3)
                    {
                        Width = 64;
                        Height = 32;
                    }
                    break;
                case OBJShape.Vertical:
                    if (size == 0)
                    {
                        Width = 8;
                        Height = 16;

                    }
                    else if (size == 1)
                    {
                        Width = 8;
                        Height = 32;
                    }
                    else if (size == 2)
                    {
                        Width = 16;
                        Height = 32;
                    }
                    else if (size == 3)
                    {
                        Width = 32;
                        Height = 64;
                    }
                    break;
                case OBJShape.Prohibited:
                    break;
                default:
                    break;
            }



        }

        private void GetObj2Attributes(ushort atb2)
        {
            TileId = (uint)(atb2 & 0X3FF);
            atb2 = (ushort)(atb2 >> 10);
            Priority = (uint)(atb2 & 3);
            atb2 = (ushort)(atb2 >> 2);
            PaletteId = (uint)(atb2 & 0xF);
        }

        public override string ToString()
        {
            return $"{Width} x {Height}";
        }
    }
}
