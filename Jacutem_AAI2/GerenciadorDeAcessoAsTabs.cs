using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2
{
    public class GerenciadorDeAcessoAsTabs
    {
        public bool ArquivosBinarios { get; set; }     
        public bool Imagens { get; set; }       
        public bool Textos { get; set; }       
        public bool Fontes { get; set; }
        public string ArquivosBinariosDir { get; } = "ROM_Desmontada";
        public string ImagenssDir { get; } = "__Binarios";
        public string TextosDir { get; } = @"__Binarios\jpn_spt";
        public string FontesDir { get; } = @"ROM_Desmontada\arm9.bin";

        public GerenciadorDeAcessoAsTabs()
        {
            ExcutarVerificacoes();
        }

        public void ExcutarVerificacoes()
        {
            VerificaAbaBinarios();
            VerificaAbaImagens();
            VerificaAbaTextos();
            VerificaAbaFontes();
        }


        private void VerificaAbaBinarios()
        {
            ArquivosBinarios = VerificaPastas(ArquivosBinariosDir);
        }

        private void VerificaAbaImagens()
        {
            Imagens = VerificaPastas(ImagenssDir);
            
        }

        private void VerificaAbaTextos()
        {            
            Textos = VerificaPastas(TextosDir);
        }

        private void VerificaAbaFontes()
        {
            Fontes = File.Exists(FontesDir);
        }

        private bool VerificaPastas(string dir)
        {
            if (Directory.Exists(dir))
            {

                if (Directory.GetFiles(dir).Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
    }
}
