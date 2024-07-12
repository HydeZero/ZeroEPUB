using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
                    DirectoryInfo extractionDir = new($"{homeDir}/.zeroepub/{input.Split('\\')[input.Split('\\').Length - 1]}");
                    if (extractionDir.Exists)
                    {
                        extractionDir.Delete(true);
                    }
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
            string[] allfiles = Directory.GetFiles($"{homeDir}/.zeroepub/{ebook}", "*.*", System.IO.SearchOption.AllDirectories);
            string contentFile = "";
            string parentDir = "";
            foreach (var file in allfiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.Name == "content.opf")
                {
                    contentFile = info.FullName;
                    parentDir = info.DirectoryName;
                    break;
                }
            }

            XElement doc = XElement.Load(contentFile);

            var chapters = doc.Descendants("manifest").SelectMany(a => a.Elements("item")).Where(b => (string)b.Attribute("media-type") == "application/xhtml-xml");

            List<Dictionary<string, string>> chapterList = chapters.Select(element => new Dictionary<string, string>());


            var chapterDict = chapters.Select(b => new
            {
                href = (string)b.Attribute("href"),
                id = (string)b.Attribute("id"),
                media_type = (string)b.Attribute("media-type")
            }).ToDictionary(x => "id", x => new { x.id, x.href });

            List<string> chapterHref = new List<string>();

            foreach (var chapt in chapters)
            {
                chapterHref.Add((string)chapt.Attribute("href"));
            }

            return File.ReadAllText($"{parentDir}/{chapterHref[chapter]}");
        }
    }
}