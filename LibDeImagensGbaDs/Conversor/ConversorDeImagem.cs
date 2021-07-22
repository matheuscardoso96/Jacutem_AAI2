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
        private IConversorFormatoIndexado _conversor;

        public ConversorDeImagem(IFormatoIndexado formatoIndexado, IConversorFormatoIndexado conversor)
        {
            _formatoIndexado = formatoIndexado;
            _conversor = conversor;
        }

        public Bitmap ConvertaFormatoIndexadoParaBmp(BinaryReader arquivo, int altura, int largura, int enderecoInicial) 
        {
            byte[] Indicies = _formatoIndexado.ObtenhaArrayDeIndices(arquivo, altura,largura, enderecoInicial);
            return _conversor.ConvertaIndexadoParaBmp(Indicies, largura, altura);
        }

    }
}
