using System;

namespace TonyM.Modules
{
    public class GlobalMethod
    {
        public static double Timestamp()
        {
            DateTime tBase = new(2018, 06, 14); //Champion !!!!
            DateTime tNow = DateTime.Now;
            TimeSpan tCal = tNow - tBase;
            double timestamp = Math.Round(tCal.TotalSeconds);
            return timestamp;
        }

        public static void SoundAlert()
        {
            for (int i = 0; i < 3; i++)
                Console.Beep();
        }
    }
}
