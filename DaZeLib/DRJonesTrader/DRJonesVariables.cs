using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class DRJonesVariables
    {
        public float BuySellTimer { get; set; }
        public float StatUpdateTimer { get; set; }
        public float FireBarrelUpdateTimer { get; set; }
        public float ZombieCleanupTimer { get; set; }
        public float VehicleCleanupTimer { get; set; }
        public float SafezoneTimeout { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }


        public DRJonesVariables(string filename)
        {
            Filename = filename;
            List<string> file = File.ReadAllLines(Filename).ToList();
            Console.WriteLine("Parsing " + Path.GetFileName(filename) + "....");
            isDirty = false;
            foreach (string line in file)
            {
                if (line.Equals("")) continue;
                if (line.StartsWith("<FileEnd>")) continue;
                string adminline = line.Replace("\t", "").Replace(" ", "");
                int index = adminline.IndexOf("/");
                string variable = adminline;
                if (index != -1)
                    variable = adminline.Remove(index, adminline.Length - index);

                if(variable.StartsWith("<BuySellTimer>"))
                {
                    BuySellTimer = Convert.ToSingle(variable.Replace("<BuySellTimer>", ""));
                    Console.WriteLine("BuySellTimer " + BuySellTimer.ToString());

                }
                else if (variable.StartsWith("<StatUpdateTimer>"))
                {
                    StatUpdateTimer = Convert.ToSingle(variable.Replace("<StatUpdateTimer>", ""));
                    Console.WriteLine("StatUpdateTimer " + StatUpdateTimer.ToString());
                }
                else if (variable.StartsWith("<FireBarrelUpdateTimer>"))
                {
                    FireBarrelUpdateTimer = Convert.ToSingle(variable.Replace("<FireBarrelUpdateTimer>", ""));
                    Console.WriteLine("FireBarrelUpdateTimer " + FireBarrelUpdateTimer.ToString());
                }
                else if (variable.StartsWith("<ZombieCleanupTimer>"))
                {
                    ZombieCleanupTimer = Convert.ToSingle(variable.Replace("<ZombieCleanupTimer>", ""));
                    Console.WriteLine("ZombieCleanupTimer " + ZombieCleanupTimer.ToString());
                }
                else if (variable.StartsWith("<VehicleCleanupTimer>"))
                {
                    VehicleCleanupTimer = Convert.ToSingle(variable.Replace("<VehicleCleanupTimer>", ""));
                    Console.WriteLine("VehicleCleanupTimer " + VehicleCleanupTimer.ToString());
                }
                else if (variable.StartsWith("<SafezoneTimeout>"))
                {
                    SafezoneTimeout = Convert.ToSingle(variable.Replace("<SafezoneTimeout>", ""));
                    Console.WriteLine("SafezoneTimeout " + SafezoneTimeout.ToString());
                }
            }
        }

        public bool SaveVariables(string SaveTime)
        {
            if (!isDirty) return false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<BuySellTimer> " + BuySellTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<StatUpdateTimer> " + StatUpdateTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<FireBarrelUpdateTimer> " + FireBarrelUpdateTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<ZombieCleanupTimer> " + ZombieCleanupTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<VehicleCleanupTimer> " + VehicleCleanupTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<SafezoneTimeout> " + SafezoneTimeout.ToString("0.0") + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("<FileEnd> // This has to be on the End of this File and is very importand!");
            Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime);
            File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileName(Filename), true);
            File.WriteAllText(Filename, sb.ToString());
            isDirty = false;
            return true;
        }
    }
}
