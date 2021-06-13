using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2.Arquivos
{
    public static class IntegracaoComNdsTool
    {
        public static string DesmontarArquivoNds(string dirRom, string dirDestino)
        {

           bool criouDiretorio = CriarDiretorioEVerificarSucesso(dirDestino);

            if (criouDiretorio)
            {
                string comando = $"/c _Tools\\ndstool.exe -x \"{dirRom}\" -9 \"{dirDestino}\\arm9.bin\" -7 \"{dirDestino}\\arm7.bin\" -y9 \"{dirDestino}\\y9.bin\" -y7 \"{dirDestino}\\y7.bin\" -d \"{dirDestino}\\data\" -y \"{dirDestino}\\overlay\" -t \"{dirDestino}\\banner.bin\" -h \"{dirDestino}\\header.bin\"";

                if (File.Exists(dirRom))
                {
                    ExecutarNDSTool(comando);
                    if (Directory.GetFiles(dirDestino).Length == 0)
                        return "Ocorreu um erro na execução da NDSTool.exe, verifique as permissões de sistema.";

                    return "ROM desmontada para o seguinte diretório: ROM_Desmontada, não mude esta pasta de diretório ou apague os arquivos internos.";
                }
                else
                    return $"Diretório de {Path.GetFileName(dirRom)} não encontrado.";
            }

            return "Não foi possivel criar diretório de exportação.";

        }


        private static bool CriarDiretorioEVerificarSucesso(string dirDestino)
        {
            if (!Directory.Exists(dirDestino))
            {
                Directory.CreateDirectory(dirDestino);
            }

            if (Directory.Exists(dirDestino))
                return true;

            return false;
        }

        public static string NovoArquivoNds(string dirRomDesmontada,string nomeDaNovaRom)
        {
            string comando = $"/c _Tools\\ndstool.exe -c {nomeDaNovaRom} -9 \"{dirRomDesmontada}\\arm9.bin\" -7 \"{dirRomDesmontada}\\arm7.bin\" -y9 \"{dirRomDesmontada}\\y9.bin\" -y7 \"{dirRomDesmontada}\\y7.bin\" -d \"{dirRomDesmontada}\\data\" -y \"{dirRomDesmontada}\\overlay\" -t \"{dirRomDesmontada}\\banner.bin\" -h \"{dirRomDesmontada}\\header.bin\"";

            if (!File.Exists($"{dirRomDesmontada}"))
            {
                return "Diretório inválido";
            }

            if (Directory.Exists(dirRomDesmontada))
            {
                ExecutarNDSTool(comando);
                return "ROM criada: AAI2_novo.nds";
            }
            return $"Diretório {dirRomDesmontada} não encontrado.";


        }

        private static void ExecutarNDSTool(string comando)
        {
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", comando);
            process.StartInfo = processInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

    }
}
