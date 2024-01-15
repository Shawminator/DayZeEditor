using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            string zipfile = args[0];
            Console.WriteLine("Sleeping for 2 seconds to make sure Application is fully closed");
            Console.WriteLine("Update File : " + zipfile);
            System.Threading.Thread.Sleep(2000);
            string outputpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Directory.Delete(outputpath + "\\lib", true);
            using (FileStream fs = File.OpenRead(zipfile))
            {
                using (ZipArchive zip = new ZipArchive(fs))
                {
                    ExtractToDirectory(zip, outputpath, true);
                }
            }
            File.Delete(zipfile);
            System.Diagnostics.Process.Start("DayZeEditor.exe");
        }
        public static void ExtractToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                if (file.Name == ("Updater.exe")) continue;
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));
                Console.WriteLine("Extracting : " + completeFileName);
                if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Trying to extract file outside of destination directory");
                }

                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }

    }
}
