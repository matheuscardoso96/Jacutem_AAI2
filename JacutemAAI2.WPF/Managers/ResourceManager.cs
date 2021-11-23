using System.IO;
using System.Text;

namespace JacutemAAI2.WPF.Managers
{
    public class ResourceManager
    {
        public static void VerfyDirectories()
        {
            CreateDir(Properties.Settings.Default.PastaBinarios);
            CreateDir(Properties.Settings.Default.PastaInfoBinarios);
            CreateDir(Properties.Settings.Default.PastaImagens);
            CreateDir(Properties.Settings.Default.PastaImagensEditadas);
            CreateDir(Properties.Settings.Default.PastaTextos);
            CreateDir(Properties.Settings.Default.PastaScriptsOriginais);
            CreateDir(Properties.Settings.Default.PastaScriptsTraduzidos);
            CreateDir(Properties.Settings.Default.PastaTabelas);
            CreateDir(Properties.Settings.Default.PastaTools);
            VerificyResources();

        }

        private static void VerificyResources()
        {
            WriteResource($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstool}", Properties.Resources.ndstool);
            WriteResource($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibgcc_s_seh_1}", Properties.Resources.libgcc_s_seh_1);
            WriteResource($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibstdc_6}", Properties.Resources.libstdc___6);
            WriteResource($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibwinpthread_1}", Properties.Resources.libwinpthread_1);
            WriteResource($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolBlz}", Properties.Resources.blz);
            WriteResource($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosInfoScript}", Encoding.UTF8.GetBytes(Properties.Resources._infoScripts));
            WriteResource($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2}", Properties.Resources.aai2);
            WriteResource($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Botoes}", Properties.Resources.aai2_botoes);
            WriteResource($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Descricoes}", Properties.Resources.aai2_descricoes);
            WriteResource($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_TabelaTag}", Properties.Resources.tabelaTag);

        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }
        }


        private static void WriteResource(string path, byte[] resource)
        {
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path,resource);
            }
        }
    }
}
