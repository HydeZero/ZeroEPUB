using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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


            XmlReader reader = XmlReader.Create(File.Open(Path.Combine(parentDir, contentFile), FileMode.Open, FileAccess.Read));
            // Dont touch this if it works
            XElement doc = XElement.Load(reader);

            XmlNameTable nameTable = reader.NameTable;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("package", "http://www.idpf.org/2007/opf");

            IEnumerable<XElement> chapters = doc.XPathSelectElements("/package:manifest", namespaceManager);

            var chapterHref = new List<string>();

            foreach (var chapt in chapters.Elements().ToArray())
            {
                try
                {
                    chapterHref.Add(chapt.Attribute("href").Value);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return File.ReadAllText($"{parentDir}/{chapterHref[chapter]}");
        }
    }
}