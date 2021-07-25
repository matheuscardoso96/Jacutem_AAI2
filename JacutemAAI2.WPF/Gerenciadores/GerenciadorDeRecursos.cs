using System.IO;

namespace JacutemAAI2.WPF.Gerenciadores
{
    public class GerenciadorDeRecursos
    {
        public static void VerficarFicarPastasDoPrograma()
        {
            if (!Directory.Exists(Properties.Settings.Default.PastaBinarios))
                Directory.CreateDirectory(Properties.Settings.Default.PastaBinarios);

            if (!Directory.Exists(Properties.Settings.Default.PastaInfoBinarios))
                Directory.CreateDirectory(Properties.Settings.Default.PastaInfoBinarios);

            if (!Directory.Exists(Properties.Settings.Default.PastaImagens))
                Directory.CreateDirectory(Properties.Settings.Default.PastaImagens);

            if (!Directory.Exists(Properties.Settings.Default.PastaImagensEditadas))
                Directory.CreateDirectory(Properties.Settings.Default.PastaImagensEditadas);

            if (!Directory.Exists(Properties.Settings.Default.PastaTextos))
                Directory.CreateDirectory(Properties.Settings.Default.PastaTextos);

            if (!Directory.Exists(Properties.Settings.Default.PastaScriptsOriginais))
                Directory.CreateDirectory(Properties.Settings.Default.PastaScriptsOriginais);

            if (!Directory.Exists(Properties.Settings.Default.PastaScriptsTraduzidos))
                Directory.CreateDirectory(Properties.Settings.Default.PastaScriptsTraduzidos);

            if (!Directory.Exists(Properties.Settings.Default.PastaTabelas))
                Directory.CreateDirectory(Properties.Settings.Default.PastaTabelas);

            if (!Directory.Exists(Properties.Settings.Default.PastaTools))
                Directory.CreateDirectory(Properties.Settings.Default.PastaTools);

            VerificaArquivos();

        }

        private static void VerificaArquivos()
        {
            if (!File.Exists($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstool}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstool}", Properties.Resources.ndstool);

            if (!File.Exists($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibgcc_s_seh_1}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibgcc_s_seh_1}", Properties.Resources.libgcc_s_seh_1);

            if (!File.Exists($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibstdc_6}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibstdc_6}", Properties.Resources.libstdc___6);

            if (!File.Exists($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibwinpthread_1}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolNdstoolLibwinpthread_1}", Properties.Resources.libwinpthread_1);

            if (!File.Exists($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolBlz}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTools}\\{Properties.Settings.Default.ToolBlz}", Properties.Resources.blz);

            if (!File.Exists($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosInfoScript}"))
                File.WriteAllText($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosInfoScript}", Properties.Resources._infoScripts);

            if (!File.Exists($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2}", Properties.Resources.aai2);

            if (!File.Exists($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Botoes}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Botoes}", Properties.Resources.aai2_botoes);

            if (!File.Exists($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Descricoes}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_Descricoes}", Properties.Resources.aai2_descricoes);

            if (!File.Exists($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_TabelaTag}"))
                File.WriteAllBytes($"{Properties.Settings.Default.PastaTabelas}\\{Properties.Settings.Default.TextosTabelaAAI2_TabelaTag}", Properties.Resources.tabelaTag);



        }
    }
}
