using System;
using System.IO;

namespace TonyM.Models
{
    public static class DropFile
    {
        public static readonly string FileName = "drops.txt";
        public static readonly string Folder = "drop";
        public static string PathAndFile
        {
            get
            {
                string pathAndFile = Path.Combine(Folder, FileName);
                return pathAndFile;
            }
        }

        public static void Delete()
        {
            if (File.Exists(PathAndFile))
                File.Delete(PathAndFile);
        }

        public static void Display()
        {
            Console.WriteLine("\n== HISTORIQUE DES DROPS ==");
            string oldDrop = File.ReadAllText(PathAndFile);
            Console.WriteLine(oldDrop);
        }
    }
}
