using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageLibGbaDS.Formatos
{
    public interface IFormatoIndexado
    {
        byte[] ObtenhaArrayDeIndices(BinaryReader arquivo, int width, int height, int enderecoInicial);
    }
}
