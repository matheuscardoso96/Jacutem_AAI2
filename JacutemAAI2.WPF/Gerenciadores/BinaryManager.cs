using JacutemAAI2.WPF.Ferramentas.Internas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.Gerenciadores
{
    public static class BinaryManager
    {
        private static bool _isInialized = false;
        private static Dictionary<string, AAIBin> _binFiles;

        public static byte[] GetFile(string filePath)
        {
            if (_isInialized == false)
            {
                Initialize();
            }

            var containerName = Path.GetFileNameWithoutExtension(filePath);
            string[] parts = containerName.Split('_');
            containerName = $"{string.Join("_", parts.Skip(1).Take(parts.Length - 1))}.bin";
            int index = Convert.ToInt32(parts[0]);
           
            return _binFiles[containerName].GetFile(_binFiles[containerName].Entries[index]);
        }

        private static void Initialize()
        {
            _isInialized = true;
            _binFiles = new Dictionary<string, AAIBin>();
            foreach (var dir in _binaryFiles)
            {
                string fileName = Path.GetFileName(dir);
                _binFiles.Add(fileName, new AAIBin($"Rom_Desmontada\\{dir}"));
            }

        }

        private static readonly IEnumerable<string> _binaryFiles = new List<string>()
        {
          
            @"data\com\anmseq_chr.bin",
            @"data\com\bustanmseq_chr.bin",
            @"data\com\bustup.bin",
            @"data\com\cutdata.bin",
            @"data\com\cutobj.bin",
            @"data\com\font.bin",
            @"data\com\idcom.bin",
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
            @"data\com\movie.bin",
            @"data\com\modelexam.bin",
            @"data\com\modelitem.bin",
            @"data\com\opening.bin",
            @"data\com\save.bin",
            @"data\com\upcut.bin",
            @"data\jpn\cutobj_local.bin",
            @"data\jpn\idlocal.bin",
            @"data\jpn\logic_keyword_local.bin",
            @"data\jpn\modelitemlocal.bin",
            @"data\jpn\opening_local.bin",
            @"data\jpn\save_local.bin",
            @"data\jpn\speech.bin",
            @"data\jpn\spt.bin",
            @"data\jpn\title_local.bin",
            @"data\jpn\upcut_local.bin"};
    }
}
