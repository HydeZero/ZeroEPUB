using Avalonia.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            string[] allfiles = Directory.GetFiles($"{homeDir}/.zeroepub/${ebook}");
            string contentFile = "";
            foreach (var file in allfiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.Name == "content.opf")
                {
                    contentFile = info.FullName;
                    break;
                }
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(contentFile);

            XmlNodeList items = doc.SelectNodes("//manifest/item[@media-type='application/xhtml+xml']");

            List<string> chapters = new List<string>();

            return "silly";
        }
    }
}