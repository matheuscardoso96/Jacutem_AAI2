using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace JacutemAAI2.WPF.Ferramentas.Externas
{
    public static class NdsTool
    {
        public static string Message = "";
        private const string _ndsToolPath = "__Tools\\ndstool.exe";
        private const string _blzToolPath = "__Tools\\blz.exe";
        public static void ExportNdsFiles(string dirRom, string dirDestino)
        {

            Directory.CreateDirectory(dirDestino);

            if (ValidatePaths(dirDestino, dirRom))
            {
                string comando = $"/c {_ndsToolPath} -x \"{dirRom}\" -9 \"{dirDestino}\\arm9.bin\" -7 \"{dirDestino}\\arm7.bin\" -y9 \"{dirDestino}\\y9.bin\" -y7 \"{dirDestino}\\y7.bin\" -d \"{dirDestino}\\data\" -y \"{dirDestino}\\overlay\" -t \"{dirDestino}\\banner.bin\" -h \"{dirDestino}\\header.bin\"";
                CommandExecute(comando);
               
                if (ValidateNdsExtraction(dirDestino))
                {
                    DecompressArm9($"{dirDestino}\\arm9.bin");
                    Message = $"ROM desmontada para o seguinte diretório: {dirDestino}\\ROM_Desmontada, não mude esta pasta de diretório ou apague os arquivos internos.";
                }
            }

        }


        public static void CreateNewNdsFile(string exportedFilesPath, string fileName)
        {
            fileName = $"{fileName}{DateTime.Now:dd_MM_yyyy_HH_mm_ss}.nds";
            string command = $"/c {_ndsToolPath} -c {fileName} -9 \"{exportedFilesPath}\\arm9.bin\" -7 \"{exportedFilesPath}\\arm7.bin\" -y9 \"{exportedFilesPath}\\y9.bin\" -y7 \"{exportedFilesPath}\\y7.bin\" -d \"{exportedFilesPath}\\data\" -y \"{exportedFilesPath}\\overlay\" -t \"{exportedFilesPath}\\banner.bin\" -h \"{exportedFilesPath}\\header.bin\"";
            var validationErros = AssertFiles(exportedFilesPath);
            if (validationErros.Count > 0)
            {
                Message = $"Falha ao recriar ROM.\r\nFaltam os seguites arquivos necessários no diretório {exportedFilesPath}:\r\n{string.Join(",\r\n", validationErros)}";
            }
            else
            {
                CommandExecute(command);

                if (!File.Exists(fileName))
                    Message = "Ocorreu um erro na criação da nova Rom.";
                else 
                    Message = $"ROM criada: {fileName}";
            }

        }



        #region Validações

        private static bool ValidatePaths(string newDirPath, string ndsFilePath)
        {
            if (!Directory.Exists(newDirPath))
            {
                Message = "Não foi possível criar diretório para exportação.";
                return false;
            }

            if (!File.Exists(ndsFilePath))
            {
                Message = "Não foi possível encontrar caminho para a ROM.";
                return false;
            }


            return true;
        }

        private static bool ValidateNdsExtraction(string targetPath)
        {
            if (!File.Exists($"{targetPath}\\arm9.bin"))
            {
                Message = "Não foi possível exportar a rom, arquivo .nds inválido";
                return false;
            }


            return true;
        }

        private static List<string> AssertFiles(string targetPath)
        {
            var missing = new List<string>();
            foreach (var fileName in romFilesMap)
            {
                if (!File.Exists($"{targetPath}\\{fileName}"))
                {
                    missing.Add($"{targetPath}\\{fileName}");
                }
            }

            return missing;
        }

        #endregion

        private static void DecompressArm9(string arm9Path)
        {
            byte[] arm9WithHeader = File.ReadAllBytes(arm9Path);
            byte[] arm9WithoutHeader = new byte[arm9WithHeader.Length - 12];
            Array.Copy(arm9WithHeader, arm9WithoutHeader, arm9WithoutHeader.Length);
            File.WriteAllBytes(arm9Path, arm9WithoutHeader);
            string command = $"/c {_blzToolPath} -d \"{arm9Path}\"";
            CommandExecute(command);
        }

        private static void CommandExecute(string command)
        {
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", command);
            process.StartInfo = processInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        


        private static readonly IEnumerable<string> romFilesMap = new List<string>()
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
