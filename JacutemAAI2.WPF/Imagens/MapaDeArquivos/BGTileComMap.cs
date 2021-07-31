using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.Imagens.MapaDeArquivos
{
    public class BGTileComMap : MapaDeArquivos
    {
        
        public BGTileComMap()
        {
            Tipo = "BG Com Tile Map";
            Lista = new Dictionary<string, string>() 
            {
                ["0000_title_local.ncgr"] = @"__Binarios\jpn_title_local\0000_title_local.ncgr,__Binarios\jpn_title_local\0001_title_local.nclr,__Binarios\jpn_title_local\0002_title_local.nscr,__Imagens\jpn_title_local\",
                ["0063_idcom.ncgr"] = @"__Binarios\com_idcom\0063_idcom.ncgr,__Binarios\com_idcom\0064_idcom.nclr,__Binarios\com_idcom\0065_idcom.nscr,__Imagens\com_idcom\",
                ["0066_idcom.ncgr"] = @"__Binarios\com_idcom\0066_idcom.ncgr,__Binarios\com_idcom\0067_idcom.nclr,__Binarios\com_idcom\0068_idcom.nscr,__Imagens\com_idcom\",
                ["0069_idcom.ncgr"] = @"__Binarios\com_idcom\0069_idcom.ncgr,__Binarios\com_idcom\0070_idcom.nclr,__Binarios\com_idcom\0071_idcom.nscr,__Imagens\com_idcom\",
                ["0072_idcom.ncgr"] = @"__Binarios\com_idcom\0072_idcom.ncgr,__Binarios\com_idcom\0073_idcom.nclr,__Binarios\com_idcom\0074_idcom.nscr,__Imagens\com_idcom\",
                ["0121_idcom.ncgr"] = @"__Binarios\com_idcom\0121_idcom.ncgr,__Binarios\com_idcom\0122_idcom.nclr,__Binarios\com_idcom\0123_idcom.nscr,__Imagens\com_idcom\",
                ["0124_idcom.ncgr"] = @"__Binarios\com_idcom\0124_idcom.ncgr,__Binarios\com_idcom\0125_idcom.nclr,__Binarios\com_idcom\0126_idcom.nscr,__Imagens\com_idcom\",
                ["0124_idcom.ncgr"] = @"__Binarios\com_idcom\0124_idcom.ncgr,__Binarios\com_idcom\0125_idcom.nclr,__Binarios\com_idcom\0127_idcom.nscr,__Imagens\com_idcom\",
                ["0124_idcom.ncgr"] = @"__Binarios\com_idcom\0124_idcom.ncgr,__Binarios\com_idcom\0125_idcom.nclr,__Binarios\com_idcom\0127_idcom.nscr,__Imagens\com_idcom\",
                ["0970_idcom.ncgr"] = @"__Binarios\com_idcom\0970_idcom.ncgr,__Binarios\com_idcom\0971_idcom.nclr,__Binarios\com_idcom\0972_idcom.nscr,__Imagens\com_idcom\",
                ["0970_idcom.ncgr"] = @"__Binarios\com_idcom\0970_idcom.ncgr,__Binarios\com_idcom\0971_idcom.nclr,__Binarios\com_idcom\0973_idcom.nscr,__Imagens\com_idcom\",
                ["0974_idcom.ncgr"] = @"__Binarios\com_idcom\0974_idcom.ncgr,__Binarios\com_idcom\0975_idcom.nclr,__Binarios\com_idcom\0976_idcom.nscr,__Imagens\com_idcom\",
                ["0974_idcom.ncgr"] = @"__Binarios\com_idcom\0974_idcom.ncgr,__Binarios\com_idcom\0975_idcom.nclr,__Binarios\com_idcom\0977_idcom.nscr,__Imagens\com_idcom\",
                ["0974_idcom.ncgr"] = @"__Binarios\com_idcom\0974_idcom.ncgr,__Binarios\com_idcom\0975_idcom.nclr,__Binarios\com_idcom\0978_idcom.nscr,__Imagens\com_idcom\",
                ["0974_idcom.ncgr"] = @"__Binarios\com_idcom\0974_idcom.ncgr,__Binarios\com_idcom\0975_idcom.nclr,__Binarios\com_idcom\0979_idcom.nscr,__Imagens\com_idcom\",
                ["0980_idcom.ncgr"] = @"__Binarios\com_idcom\0980_idcom.ncgr,__Binarios\com_idcom\0981_idcom.nclr,__Binarios\com_idcom\0982_idcom.nscr,__Imagens\com_idcom\",
                ["0980_idcom.ncgr"] = @"__Binarios\com_idcom\0980_idcom.ncgr,__Binarios\com_idcom\0981_idcom.nclr,__Binarios\com_idcom\0983_idcom.nscr,__Imagens\com_idcom\",
                ["1043_idcom.ncgr"] = @"__Binarios\com_idcom\1043_idcom.ncgr,__Binarios\com_idcom\1044_idcom.nclr,__Binarios\com_idcom\1045_idcom.nscr,__Imagens\com_idcom\",
                ["1043_idcom.ncgr"] = @"__Binarios\com_idcom\1043_idcom.ncgr,__Binarios\com_idcom\1044_idcom.nclr,__Binarios\com_idcom\1046_idcom.nscr,__Imagens\com_idcom\",
                ["1043_idcom.ncgr"] = @"__Binarios\com_idcom\1043_idcom.ncgr,__Binarios\com_idcom\1044_idcom.nclr,__Binarios\com_idcom\1047_idcom.nscr,__Imagens\com_idcom\",
                ["1043_idcom.ncgr"] = @"__Binarios\com_idcom\1043_idcom.ncgr,__Binarios\com_idcom\1044_idcom.nclr,__Binarios\com_idcom\1048_idcom.nscr,__Imagens\com_idcom\",
                ["1064_idcom.ncgr"] = @"__Binarios\com_idcom\1064_idcom.ncgr,__Binarios\com_idcom\1065_idcom.nclr,__Binarios\com_idcom\1066_idcom.nscr,__Imagens\com_idcom\",
                ["0000_logicmat.ncgr"] = @"__Binarios\com_logicmat\0000_logicmat.ncgr,__Binarios\com_logicmat\0001_logicmat.nclr,__Binarios\com_logicmat\0002_logicmat.nscr,__Imagens\com_logicmat\",
                ["0003_logicmat.ncgr"] = @"__Binarios\com_logicmat\0003_logicmat.ncgr,__Binarios\com_logicmat\0001_logicmat.nclr,__Binarios\com_logicmat\0004_logicmat.nscr,__Imagens\com_logicmat\",
                ["0000_logo.ncgr"] = @"__Binarios\com_logo\0000_logo.ncgr,__Binarios\com_logo\0001_logo.nclr,__Binarios\com_logo\0002_logo.nscr,__Imagens\com_logo\",
                ["0003_logo.ncgr"] = @"__Binarios\com_logo\0003_logo.ncgr,__Binarios\com_logo\0004_logo.nclr,__Binarios\com_logo\0005_logo.nscr,__Imagens\com_logo\",
                ["0001_save.ncgr"] = @"__Binarios\com_save\0001_save.ncgr,__Binarios\com_save\0002_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0005_save.ncgr"] = @"__Binarios\com_save\0005_save.ncgr,__Binarios\com_save\0006_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0007_save.ncgr"] = @"__Binarios\com_save\0007_save.ncgr,__Binarios\com_save\0008_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0009_save.ncgr"] = @"__Binarios\com_save\0009_save.ncgr,__Binarios\com_save\0010_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0011_save.ncgr"] = @"__Binarios\com_save\0011_save.ncgr,__Binarios\com_save\0010_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0013_save.ncgr"] = @"__Binarios\com_save\0013_save.ncgr,__Binarios\com_save\0014_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0015_save.ncgr"] = @"__Binarios\com_save\0015_save.ncgr,__Binarios\com_save\0016_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0017_save.ncgr"] = @"__Binarios\com_save\0017_save.ncgr,__Binarios\com_save\0018_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0019_save.ncgr"] = @"__Binarios\com_save\0019_save.ncgr,__Binarios\com_save\0020_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0021_save.ncgr"] = @"__Binarios\com_save\0021_save.ncgr,__Binarios\com_save\0022_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0023_save.ncgr"] = @"__Binarios\com_save\0023_save.ncgr,__Binarios\com_save\0024_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0025_save.ncgr"] = @"__Binarios\com_save\0025_save.ncgr,__Binarios\com_save\0026_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0027_save.ncgr"] = @"__Binarios\com_save\0027_save.ncgr,__Binarios\com_save\0028_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0029_save.ncgr"] = @"__Binarios\com_save\0029_save.ncgr,__Binarios\com_save\0030_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0031_save.ncgr"] = @"__Binarios\com_save\0031_save.ncgr,__Binarios\com_save\0032_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0033_save.ncgr"] = @"__Binarios\com_save\0033_save.ncgr,__Binarios\com_save\0034_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0035_save.ncgr"] = @"__Binarios\com_save\0035_save.ncgr,__Binarios\com_save\0036_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0037_save.ncgr"] = @"__Binarios\com_save\0037_save.ncgr,__Binarios\com_save\0038_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\",
                ["0039_save.ncgr"] = @"__Binarios\com_save\0039_save.ncgr,__Binarios\com_save\0040_save.nclr,__Binarios\com_save\0000_save.nscr,__Imagens\com_save\"
            };
        }
    }
}
