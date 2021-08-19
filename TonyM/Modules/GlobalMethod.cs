using System;
using System.IO;

namespace TonyM.Modules
{
    public class GlobalMethod
    {
        public static void SoundAlert()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Beep();
            }
        }

        public static double Timestamp()
        {
            DateTime tBase = new(2018, 06, 14); //Champion !!!
            DateTime tNow = DateTime.Now;
            TimeSpan tCal = tNow - tBase;
            double timestamp = Math.Round(tCal.TotalSeconds);
            return timestamp;
        }

        public static void CreateDropFile(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
                Directory.Delete(path);
        }

        public static void DisplayOldDrop(string pathAndFile)
        {
            Console.WriteLine("-- HISTORIQUE DES DROPS --");
            string oldDrop = File.ReadAllText(pathAndFile);
            Console.WriteLine(oldDrop);
        }
    }
}
