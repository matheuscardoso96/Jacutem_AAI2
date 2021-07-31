using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.Imagens.MapaDeArquivos
{
    public class SpritesModo2D : MapaDeArquivos
    {
        public SpritesModo2D()
        {
            Tipo = "Sprites Modo 2D";
            Lista = new Dictionary<string, string>()
            {
                ["0002_opening.ncgr"] = @"__Binarios\com_opening\0002_opening.ncgr,__Binarios\com_opening\0003_opening.nclr,__Binarios\com_opening\0001_opening.ncer,__Imagens\com_opening\",
                ["0006_opening.ncgr"] = @"__Binarios\com_opening\0006_opening.ncgr,__Binarios\com_opening\0007_opening.nclr,__Binarios\com_opening\0005_opening.ncer,__Imagens\com_opening\",
                ["0018_opening.ncgr"] = @"__Binarios\com_opening\0018_opening.ncgr,__Binarios\com_opening\0019_opening.nclr,__Binarios\com_opening\0017_opening.ncer,__Imagens\com_opening\",
                ["0022_opening.ncgr"] = @"__Binarios\com_opening\0022_opening.ncgr,__Binarios\com_opening\0023_opening.nclr,__Binarios\com_opening\0021_opening.ncer,__Imagens\com_opening\",
                ["0026_opening.ncgr"] = @"__Binarios\com_opening\0026_opening.ncgr,__Binarios\com_opening\0027_opening.nclr,__Binarios\com_opening\0025_opening.ncer,__Imagens\com_opening\",
                ["0013_opening_local.ncgr"] = @"__Binarios\jpn_opening_local\0013_opening_local.ncgr,__Binarios\jpn_opening_local\0014_opening_local.nclr,__Binarios\jpn_opening_local\0012_opening_local.ncer,__Imagens\jpn_opening_local\",
                ["0002_idcom.ncgr"] = @"__Binarios\com_idcom\0002_idcom.ncgr,__Binarios\com_idcom\0003_idcom.nclr,__Binarios\com_idcom\0001_idcom.ncer,__Imagens\com_idcom\",
                ["0001_logicin.ncgr"] = @"__Binarios\com_logicin\0001_logicin.ncgr,__Binarios\com_logicin\0000_logicin.nclr,__Binarios\com_logicin\0002_logicin.ncer,__Imagens\com_logicin\",
                ["0005_logicin.ncgr"] = @"__Binarios\com_logicin\0005_logicin.ncgr,__Binarios\com_logicin\0004_logicin.nclr,__Binarios\com_logicin\0006_logicin.ncer,__Imagens\com_logicin\",
                ["0002_mapobj.ncgr"] = @"__Binarios\com_mapobj\0002_mapobj.ncgr,__Binarios\com_mapobj\0003_mapobj.nclr,__Binarios\com_mapobj\0000_mapobj.ncer,__Imagens\com_mapobj\",
                ["0010_mapobj.ncgr"] = @"__Binarios\com_mapobj\0010_mapobj.ncgr,__Binarios\com_mapobj\0011_mapobj.nclr,__Binarios\com_mapobj\0008_mapobj.ncer,__Imagens\com_mapobj\",
                ["0014_mapobj.ncgr"] = @"__Binarios\com_mapobj\0014_mapobj.ncgr,__Binarios\com_mapobj\0015_mapobj.nclr,__Binarios\com_mapobj\0012_mapobj.ncer,__Imagens\com_mapobj\",
                ["0018_mapobj.ncgr"] = @"__Binarios\com_mapobj\0018_mapobj.ncgr,__Binarios\com_mapobj\0019_mapobj.nclr,__Binarios\com_mapobj\0016_mapobj.ncer,__Imagens\com_mapobj\",
                ["0022_mapobj.ncgr"] = @"__Binarios\com_mapobj\0022_mapobj.ncgr,__Binarios\com_mapobj\0023_mapobj.nclr,__Binarios\com_mapobj\0020_mapobj.ncer,__Imagens\com_mapobj\",
                ["0026_mapobj.ncgr"] = @"__Binarios\com_mapobj\0026_mapobj.ncgr,__Binarios\com_mapobj\0027_mapobj.nclr,__Binarios\com_mapobj\0024_mapobj.ncer,__Imagens\com_mapobj\",
                ["0030_mapobj.ncgr"] = @"__Binarios\com_mapobj\0030_mapobj.ncgr,__Binarios\com_mapobj\0031_mapobj.nclr,__Binarios\com_mapobj\0028_mapobj.ncer,__Imagens\com_mapobj\",
                ["0034_mapobj.ncgr"] = @"__Binarios\com_mapobj\0034_mapobj.ncgr,__Binarios\com_mapobj\0035_mapobj.nclr,__Binarios\com_mapobj\0032_mapobj.ncer,__Imagens\com_mapobj\",
                ["0038_mapobj.ncgr"] = @"__Binarios\com_mapobj\0038_mapobj.ncgr,__Binarios\com_mapobj\0039_mapobj.nclr,__Binarios\com_mapobj\0036_mapobj.ncer,__Imagens\com_mapobj\",
                ["0042_mapobj.ncgr"] = @"__Binarios\com_mapobj\0042_mapobj.ncgr,__Binarios\com_mapobj\0043_mapobj.nclr,__Binarios\com_mapobj\0040_mapobj.ncer,__Imagens\com_mapobj\",
                ["0046_mapobj.ncgr"] = @"__Binarios\com_mapobj\0046_mapobj.ncgr,__Binarios\com_mapobj\0047_mapobj.nclr,__Binarios\com_mapobj\0044_mapobj.ncer,__Imagens\com_mapobj\",
                ["0050_mapobj.ncgr"] = @"__Binarios\com_mapobj\0050_mapobj.ncgr,__Binarios\com_mapobj\0051_mapobj.nclr,__Binarios\com_mapobj\0048_mapobj.ncer,__Imagens\com_mapobj\",
                ["0054_mapobj.ncgr"] = @"__Binarios\com_mapobj\0054_mapobj.ncgr,__Binarios\com_mapobj\0055_mapobj.nclr,__Binarios\com_mapobj\0052_mapobj.ncer,__Imagens\com_mapobj\",
                ["0058_mapobj.ncgr"] = @"__Binarios\com_mapobj\0058_mapobj.ncgr,__Binarios\com_mapobj\0059_mapobj.nclr,__Binarios\com_mapobj\0056_mapobj.ncer,__Imagens\com_mapobj\",
                ["0062_mapobj.ncgr"] = @"__Binarios\com_mapobj\0062_mapobj.ncgr,__Binarios\com_mapobj\0063_mapobj.nclr,__Binarios\com_mapobj\0060_mapobj.ncer,__Imagens\com_mapobj\",
                ["0066_mapobj.ncgr"] = @"__Binarios\com_mapobj\0066_mapobj.ncgr,__Binarios\com_mapobj\0067_mapobj.nclr,__Binarios\com_mapobj\0064_mapobj.ncer,__Imagens\com_mapobj\",
                ["0070_mapobj.ncgr"] = @"__Binarios\com_mapobj\0070_mapobj.ncgr,__Binarios\com_mapobj\0071_mapobj.nclr,__Binarios\com_mapobj\0068_mapobj.ncer,__Imagens\com_mapobj\",
                ["0074_mapobj.ncgr"] = @"__Binarios\com_mapobj\0074_mapobj.ncgr,__Binarios\com_mapobj\0075_mapobj.nclr,__Binarios\com_mapobj\0072_mapobj.ncer,__Imagens\com_mapobj\",
                ["0078_mapobj.ncgr"] = @"__Binarios\com_mapobj\0078_mapobj.ncgr,__Binarios\com_mapobj\0079_mapobj.nclr,__Binarios\com_mapobj\0076_mapobj.ncer,__Imagens\com_mapobj\",
                ["0082_mapobj.ncgr"] = @"__Binarios\com_mapobj\0082_mapobj.ncgr,__Binarios\com_mapobj\0083_mapobj.nclr,__Binarios\com_mapobj\0080_mapobj.ncer,__Imagens\com_mapobj\",
                ["0086_mapobj.ncgr"] = @"__Binarios\com_mapobj\0086_mapobj.ncgr,__Binarios\com_mapobj\0087_mapobj.nclr,__Binarios\com_mapobj\0084_mapobj.ncer,__Imagens\com_mapobj\",
                ["0090_mapobj.ncgr"] = @"__Binarios\com_mapobj\0090_mapobj.ncgr,__Binarios\com_mapobj\0091_mapobj.nclr,__Binarios\com_mapobj\0088_mapobj.ncer,__Imagens\com_mapobj\",
                ["0094_mapobj.ncgr"] = @"__Binarios\com_mapobj\0094_mapobj.ncgr,__Binarios\com_mapobj\0095_mapobj.nclr,__Binarios\com_mapobj\0092_mapobj.ncer,__Imagens\com_mapobj\",
                ["0098_mapobj.ncgr"] = @"__Binarios\com_mapobj\0098_mapobj.ncgr,__Binarios\com_mapobj\0099_mapobj.nclr,__Binarios\com_mapobj\0096_mapobj.ncer,__Imagens\com_mapobj\",
                ["0102_mapobj.ncgr"] = @"__Binarios\com_mapobj\0102_mapobj.ncgr,__Binarios\com_mapobj\0103_mapobj.nclr,__Binarios\com_mapobj\0100_mapobj.ncer,__Imagens\com_mapobj\",
                ["0106_mapobj.ncgr"] = @"__Binarios\com_mapobj\0106_mapobj.ncgr,__Binarios\com_mapobj\0107_mapobj.nclr,__Binarios\com_mapobj\0104_mapobj.ncer,__Imagens\com_mapobj\",
                ["0110_mapobj.ncgr"] = @"__Binarios\com_mapobj\0110_mapobj.ncgr,__Binarios\com_mapobj\0111_mapobj.nclr,__Binarios\com_mapobj\0108_mapobj.ncer,__Imagens\com_mapobj\",
                ["0114_mapobj.ncgr"] = @"__Binarios\com_mapobj\0114_mapobj.ncgr,__Binarios\com_mapobj\0115_mapobj.nclr,__Binarios\com_mapobj\0112_mapobj.ncer,__Imagens\com_mapobj\",
                ["0118_mapobj.ncgr"] = @"__Binarios\com_mapobj\0118_mapobj.ncgr,__Binarios\com_mapobj\0119_mapobj.nclr,__Binarios\com_mapobj\0116_mapobj.ncer,__Imagens\com_mapobj\",
                ["0122_mapobj.ncgr"] = @"__Binarios\com_mapobj\0122_mapobj.ncgr,__Binarios\com_mapobj\0123_mapobj.nclr,__Binarios\com_mapobj\0120_mapobj.ncer,__Imagens\com_mapobj\",
                ["0126_mapobj.ncgr"] = @"__Binarios\com_mapobj\0126_mapobj.ncgr,__Binarios\com_mapobj\0127_mapobj.nclr,__Binarios\com_mapobj\0124_mapobj.ncer,__Imagens\com_mapobj\",
                ["0130_mapobj.ncgr"] = @"__Binarios\com_mapobj\0130_mapobj.ncgr,__Binarios\com_mapobj\0131_mapobj.nclr,__Binarios\com_mapobj\0128_mapobj.ncer,__Imagens\com_mapobj\",
                ["0134_mapobj.ncgr"] = @"__Binarios\com_mapobj\0134_mapobj.ncgr,__Binarios\com_mapobj\0135_mapobj.nclr,__Binarios\com_mapobj\0132_mapobj.ncer,__Imagens\com_mapobj\",
                ["0138_mapobj.ncgr"] = @"__Binarios\com_mapobj\0138_mapobj.ncgr,__Binarios\com_mapobj\0139_mapobj.nclr,__Binarios\com_mapobj\0136_mapobj.ncer,__Imagens\com_mapobj\",
                ["0142_mapobj.ncgr"] = @"__Binarios\com_mapobj\0142_mapobj.ncgr,__Binarios\com_mapobj\0143_mapobj.nclr,__Binarios\com_mapobj\0140_mapobj.ncer,__Imagens\com_mapobj\",
                ["0146_mapobj.ncgr"] = @"__Binarios\com_mapobj\0146_mapobj.ncgr,__Binarios\com_mapobj\0147_mapobj.nclr,__Binarios\com_mapobj\0144_mapobj.ncer,__Imagens\com_mapobj\",
                ["0150_mapobj.ncgr"] = @"__Binarios\com_mapobj\0150_mapobj.ncgr,__Binarios\com_mapobj\0151_mapobj.nclr,__Binarios\com_mapobj\0148_mapobj.ncer,__Imagens\com_mapobj\",
                ["0154_mapobj.ncgr"] = @"__Binarios\com_mapobj\0154_mapobj.ncgr,__Binarios\com_mapobj\0155_mapobj.nclr,__Binarios\com_mapobj\0152_mapobj.ncer,__Imagens\com_mapobj\",
                ["0158_mapobj.ncgr"] = @"__Binarios\com_mapobj\0158_mapobj.ncgr,__Binarios\com_mapobj\0159_mapobj.nclr,__Binarios\com_mapobj\0156_mapobj.ncer,__Imagens\com_mapobj\",
                ["0162_mapobj.ncgr"] = @"__Binarios\com_mapobj\0162_mapobj.ncgr,__Binarios\com_mapobj\0163_mapobj.nclr,__Binarios\com_mapobj\0160_mapobj.ncer,__Imagens\com_mapobj\",
                ["0166_mapobj.ncgr"] = @"__Binarios\com_mapobj\0166_mapobj.ncgr,__Binarios\com_mapobj\0167_mapobj.nclr,__Binarios\com_mapobj\0164_mapobj.ncer,__Imagens\com_mapobj\",
                ["0170_mapobj.ncgr"] = @"__Binarios\com_mapobj\0170_mapobj.ncgr,__Binarios\com_mapobj\0171_mapobj.nclr,__Binarios\com_mapobj\0168_mapobj.ncer,__Imagens\com_mapobj\",
                ["0174_mapobj.ncgr"] = @"__Binarios\com_mapobj\0174_mapobj.ncgr,__Binarios\com_mapobj\0175_mapobj.nclr,__Binarios\com_mapobj\0172_mapobj.ncer,__Imagens\com_mapobj\",
                ["0178_mapobj.ncgr"] = @"__Binarios\com_mapobj\0178_mapobj.ncgr,__Binarios\com_mapobj\0179_mapobj.nclr,__Binarios\com_mapobj\0176_mapobj.ncer,__Imagens\com_mapobj\",
                ["0182_mapobj.ncgr"] = @"__Binarios\com_mapobj\0182_mapobj.ncgr,__Binarios\com_mapobj\0183_mapobj.nclr,__Binarios\com_mapobj\0180_mapobj.ncer,__Imagens\com_mapobj\",
                ["0186_mapobj.ncgr"] = @"__Binarios\com_mapobj\0186_mapobj.ncgr,__Binarios\com_mapobj\0187_mapobj.nclr,__Binarios\com_mapobj\0184_mapobj.ncer,__Imagens\com_mapobj\"

            };
        }
    }
}
