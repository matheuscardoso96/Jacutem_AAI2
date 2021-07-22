using ImageLibGbaDS.Formatos;
using ImageLibGbaDS.Paleta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibGbaDS.Conversor
{
    public class ConversorDeImagem
    {
        private IFormatoIndexado _formatoIndexado;
        private IConversorFormatorIndexado _conversor;

        public ConversorDeImagem(IFormatoIndexado formatoIndexado, IConversorFormatorIndexado conversor)
        {
            _formatoIndexado = formatoIndexado;
            _conversor = conversor;
        }

        public Bitmap ConvertaFormatoIndexadoParaBmp(BinaryReader arquivo, int altura, int largura, int enderecoInicial) 
        {
            byte[] Indicies = _formatoIndexado.ObtenhaArrayDeIndices(arquivo, altura,largura, enderecoInicial);
            return null;
        }

    }
}
