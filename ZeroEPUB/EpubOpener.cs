using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    string processedDir = $"{homeDir}/.zeroepub/{input.Split('\\')[input.Split('\\').Length - 1].Split('.').First()}";
                    DirectoryInfo extractionDir = new(processedDir);
                    if (extractionDir.Exists)
                    {
                        extractionDir.Delete(true);
                    }
                    archive.ExtractToDirectory(processedDir); // this is very inefficient
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
            string[] allfiles = Directory.GetFiles($"{homeDir}/.zeroepub/{ebook.Split('\\')[ebook.Split('\\').Length - 1].Split('.').First()}", "*.*", System.IO.SearchOption.AllDirectories);
            string contentFile = "";
            string parentDir = "";
            foreach (var file in allfiles)
            {
                FileInfo info = new FileInfo(file);
                if (info.Name == "content.opf")
                {
                    contentFile = info.Name;
                    parentDir = info.DirectoryName;
                    break;
                }
            }

            Debug.WriteLine(Path.Combine(parentDir, contentFile));


            XElement doc = XElement.Load(File.Open(Path.Combine(parentDir, contentFile), FileMode.OpenOrCreate, FileAccess.Read));
            // Dont touch this if it works
            var chapters = doc.Elements("item").SelectMany(a => (string)a.Attribute("media-type") == "application/xhtml-xml");

            var chapterHref = new List<string>();

            foreach (var chapt in chapters)
            {
                chapterHref.Add(chapt.);
            }

            return File.ReadAllText($"{parentDir}/{chapterHref[chapter]}");
        }
    }
}