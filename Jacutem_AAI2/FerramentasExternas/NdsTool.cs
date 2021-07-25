using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2.FerramentasExternas
{
    public static class NdsTool
    {
        public static string Mensagem = "";
        public static void DesmontarArquivoNds(string dirRom, string dirDestino)
        {
            Directory.CreateDirectory(dirDestino);

            if (ValidacoesDeDiretorios(dirDestino, dirRom))
            {
                string comando = $"/c _Tools\\ndstool.exe -x \"{dirRom}\" -9 \"{dirDestino}\\arm9.bin\" -7 \"{dirDestino}\\arm7.bin\" -y9 \"{dirDestino}\\y9.bin\" -y7 \"{dirDestino}\\y7.bin\" -d \"{dirDestino}\\data\" -y \"{dirDestino}\\overlay\" -t \"{dirDestino}\\banner.bin\" -h \"{dirDestino}\\header.bin\"";
                ExecutarComando(comando);
               
                if (ValidacoesDeExportacao(dirDestino))
                {
                    DescomprimirArm9($"{dirDestino}\\arm9.bin");
                    Mensagem = "ROM desmontada para o seguinte diretório: ROM_Desmontada, não mude esta pasta de diretório ou apague os arquivos internos.";
                }
            }

                        

        }

     

        public static void NovoArquivoNds(string dirRomDesmontada, string nomeDaNovaRom)
        {
            nomeDaNovaRom = $"{nomeDaNovaRom}{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.nds";
            string comando = $"/c _Tools\\ndstool.exe -c {nomeDaNovaRom} -9 \"{dirRomDesmontada}\\arm9.bin\" -7 \"{dirRomDesmontada}\\arm7.bin\" -y9 \"{dirRomDesmontada}\\y9.bin\" -y7 \"{dirRomDesmontada}\\y7.bin\" -d \"{dirRomDesmontada}\\data\" -y \"{dirRomDesmontada}\\overlay\" -t \"{dirRomDesmontada}\\banner.bin\" -h \"{dirRomDesmontada}\\header.bin\"";
            var validacaoDeDiretorio = ValideSeDiretorioPodeSerUsadoParaCriarRom(dirRomDesmontada);
            if (validacaoDeDiretorio.Count == 0)
            {
                ExecutarComando(comando);

                if (!File.Exists(nomeDaNovaRom)) 
                {
                   Mensagem = "Ocorreu um erro na criação da nova Rom.";
                   return; 
                }
                    

                Mensagem = $"ROM criada: {nomeDaNovaRom}";
                return;

            }

            Mensagem = $"Falha ao recriar ROM.\r\nFaltam os seguites arquivos necessários no diretório {dirRomDesmontada}:\r\n{string.Join(",\r\n", validacaoDeDiretorio)}";

        }

        private static bool ValidacoesDeDiretorios(string dirCriado, string dirRom)
        {
            if (!Directory.Exists(dirCriado))
            {
                Mensagem = "Não foi possível criar diretório para exportação.";
                return false;
            }

            if (!File.Exists(dirRom))
            {
                Mensagem = "Não foi possível encontrar caminho para a ROM.";
                return false;
            }


            return true;
        }

        #region Validações

        private static bool ValidacoesDeExportacao(string dirDestino)
        {
            if (!File.Exists($"{dirDestino}\\arm9.bin"))
            {
                Mensagem = "A operação não foi possível exportar a rom, arquivo .nds inválido";
                return false;
            }


            return true;
        }

        private static List<string> ValideSeDiretorioPodeSerUsadoParaCriarRom(string dirDestino)
        {
            var faltando = new List<string>();
            foreach (var arquivo in _arquivosNecessarioParaCriarRom)
            {
                if (!File.Exists($"{dirDestino}\\{arquivo}"))
                {
                    faltando.Add($"{dirDestino}\\{arquivo}");
                }
            }

            return faltando;
        }

        #endregion

        private static void DescomprimirArm9(string dirArm9)
        {
            byte[] arm9ComHeader = File.ReadAllBytes(dirArm9);
            byte[] arm9SemHeader = new byte[arm9ComHeader.Length - 12];
            Array.Copy(arm9ComHeader, arm9SemHeader, arm9SemHeader.Length);
            File.WriteAllBytes(dirArm9, arm9SemHeader);
            string comando = $"/c _Tools\\blz.exe -d \"{dirArm9}\"";
            ExecutarComando(comando);
        }

        private static void ExecutarComando(string comando)
        {
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", comando);
            process.StartInfo = processInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        


        private static IEnumerable<string> _arquivosNecessarioParaCriarRom = new List<string>()
        {
            @"arm7.bin",
            @"arm9.bin",
            @"banner.bin",
            @"header.bin",
            @"y7.bin",
            @"y9.bin",
            @"data\com\anmseq_chr.bin",
            @"data\com\bustanmseq_chr.bin",
            @"data\com\bustup.bin",
            @"data\com\cutdata.bin",
            @"data\com\cutobj.bin",
            @"data\com\font.bin",
            @"data\com\idcom.bin",
            @"data\com\kenji2_sound.sdat",
            @"data\com\logic_keyword.bin",
            @"data\com\logicbg.bin",
            @"data\com\logicin.bin",
            @"data\com\logicmat.bin",
            @"data\com\logo.bin",
            @"data\com\mapbg.bin",
            @"data\com\mapchar.bin",
            @"data\com\mapdata.bin",
            @"data\com\mapefc.bin",
            @"data\com\mapobj.bin",
            @"data\com\mapobjset.bin",
            @"data\com\mgm_0_00.mods",
            @"data\com\mgm_0_01.mods",
            @"data\com\mgm_0_02.mods",
            @"data\com\mgm_0_03.mods",
            @"data\com\mgm_0_04.mods",
            @"data\com\mgm_1_00.mods",
            @"data\com\mgm_1_01.mods",
            @"data\com\mgm_1_02.mods",
            @"data\com\mgm_1_03.mods",
            @"data\com\mgm_1_04.mods",
            @"data\com\mgm_2_00.mods",
            @"data\com\mgm_2_01.mods",
            @"data\com\mgm_2_02.mods",
            @"data\com\mgm_2_03.mods",
            @"data\com\mgm_2_04.mods",
            @"data\com\modelexam.bin",
            @"data\com\modelitem.bin",
            @"data\com\movie.bin",
            @"data\com\opening.bin",
            @"data\com\save.bin",
            @"data\com\upcut.bin",
            @"data\data\com\system\serial.bin",
            @"data\jpn\cutobj_local.bin",
            @"data\jpn\idlocal.bin",
            @"data\jpn\logic_keyword_local.bin",
            @"data\jpn\modelitemlocal.bin",
            @"data\jpn\opening_local.bin",
            @"data\jpn\save_local.bin",
            @"data\jpn\speech.bin",
            @"data\jpn\spt.bin",
            @"data\jpn\title_local.bin",
            @"data\jpn\upcut_local.bin",
            @"overlay\overlay_0000.bin",
            @"overlay\overlay_0001.bin",
            @"overlay\overlay_0002.bin",
            @"overlay\overlay_0003.bin",
            @"overlay\overlay_0004.bin",
            @"overlay\overlay_0005.bin",
            @"overlay\overlay_0006.bin",
            @"overlay\overlay_0007.bin",
            @"overlay\overlay_0008.bin",
            @"overlay\overlay_0009.bin",
            @"overlay\overlay_0010.bin",
            @"overlay\overlay_0011.bin",
            @"overlay\overlay_0012.bin",
            @"overlay\overlay_0013.bin",
            @"overlay\overlay_0014.bin",
            @"overlay\overlay_0015.bin",
            @"overlay\overlay_0016.bin",
            @"overlay\overlay_0017.bin",
            @"overlay\overlay_0018.bin",
            @"overlay\overlay_0019.bin",
            @"overlay\overlay_0020.bin",
            @"overlay\overlay_0021.bin",
            @"overlay\overlay_0022.bin",
            @"overlay\overlay_0023.bin",
            @"overlay\overlay_0024.bin",
            @"overlay\overlay_0025.bin",
            @"overlay\overlay_0026.bin",
            @"overlay\overlay_0027.bin",
            @"overlay\overlay_0028.bin",
            @"overlay\overlay_0029.bin",
            @"overlay\overlay_0030.bin",
            @"overlay\overlay_0031.bin",
            @"overlay\overlay_0032.bin",
            @"overlay\overlay_0033.bin",
            @"overlay\overlay_0034.bin",
            @"overlay\overlay_0035.bin",
            @"overlay\overlay_0036.bin",
            @"overlay\overlay_0037.bin",
            @"overlay\overlay_0038.bin",
            @"overlay\overlay_0039.bin",
            @"overlay\overlay_0040.bin",
            @"overlay\overlay_0041.bin",
            @"overlay\overlay_0042.bin",
            @"overlay\overlay_0043.bin",
            @"overlay\overlay_0044.bin",
            @"overlay\overlay_0045.bin"};

    }
}
