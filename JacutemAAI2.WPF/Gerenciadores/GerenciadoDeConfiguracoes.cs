using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.Gerenciadores
{
    public static class GerenciadoDeConfiguracoes
    {
        public static string ObtenhaDiretorioRomDesmotanda() 
        {
            return Properties.Settings.Default.DiretorioRomDesmonstada;
        }

        public static void SetarDiretorioRomDesmotanda(string dir)
        {
            Properties.Settings.Default.DiretorioRomDesmonstada = dir;
        }

        public static string ObtenhaUltimoDiretorioSelecionado()
        {
            return Properties.Settings.Default.UltimaPastaFolderPicker;
        }

        public static void SetaUltimoDiretorioSelecionado(string dir)
        {
            Properties.Settings.Default.UltimaPastaFolderPicker = dir;
        }

        public static void SalvarConfiguracoes() 
        {
            Properties.Settings.Default.Save();
        }

       
    }
}
