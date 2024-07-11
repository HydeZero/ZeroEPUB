using System.IO.Compression;
using System.Linq.Expressions;

namespace EpubLib
{
    public class EpubOpener
    {
        public string OpenEpub(string input, string output)
        {
            try
            {
                FileStream epub = File.OpenRead(input);
                using (var archive = new ZipArchive(epub))
                {
                    archive.ExtractToDirectory(output);
                }
            }
            catch (FileNotFoundException)
            {
                return "FileNotFound";
            }
            return "Ok";
        }
    }
}
