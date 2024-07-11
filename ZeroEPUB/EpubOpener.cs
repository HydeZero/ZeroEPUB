using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroEPUB
{
    internal class EpubOpener
    {
        string homeDir = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        // This is basically a wrapper for a zip opener lol
        public string OpenEpub(string input)
        {
            try
            {
                FileStream epub = File.OpenRead(input);

                using (var archive = new ZipArchive(epub))
                {
                    archive.ExtractToDirectory($"{homeDir}/.zeroepub/{input.Split('\\')[input.Split('\\').Length - 1]}"); // this is very inefficient
                }
            }
            catch (FileNotFoundException)
            {
                return "FileNotFound";
            }
            return "ok";
        }

        public string GetContents(string ebook, int chapter)
        {
            FileStream contents = File.OpenRead($"{homeDir}/.zeroepub/${ebook}/content.opf");

            return "silly";
        }
    }
}