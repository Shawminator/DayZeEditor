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
        public decimal BuySellTimer { get; set; }
        public decimal VehicleCleanupTimer { get; set; }
        public decimal SafezoneTimeout { get; set; }
        public string SafezoneRemoveAnimals { get; set; }
        public string SafezoneRemoveInfected { get; set; }
        public decimal TradingDistance { get; set; }
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

                if (variable.StartsWith("<BuySellTimer>"))
                {
                    BuySellTimer = Convert.ToDecimal(variable.Replace("<BuySellTimer>", ""));
                    Console.WriteLine("BuySellTimer " + BuySellTimer.ToString());

                }
                else if (variable.StartsWith("<VehicleCleanupTimer>"))
                {
                    VehicleCleanupTimer = Convert.ToDecimal(variable.Replace("<VehicleCleanupTimer>", ""));
                    Console.WriteLine("VehicleCleanupTimer " + VehicleCleanupTimer.ToString());
                }
                else if (variable.StartsWith("<SafezoneTimeout>"))
                {
                    SafezoneTimeout = Convert.ToDecimal(variable.Replace("<SafezoneTimeout>", ""));
                    Console.WriteLine("SafezoneTimeout " + SafezoneTimeout.ToString());
                }
                else if (variable.StartsWith("<SafezoneRemoveAnimals>"))
                {
                    SafezoneRemoveAnimals = variable.Replace("<SafezoneRemoveAnimals>", "").ToLower();
                    Console.WriteLine("SafezoneRemoveAnimals " + SafezoneRemoveAnimals.ToString());
                }
                else if (variable.StartsWith("<SafezoneRemoveInfected>"))
                {
                    SafezoneRemoveInfected = variable.Replace("<SafezoneRemoveInfected>", "").ToLower();
                    Console.WriteLine("SafezoneRemoveInfected " + SafezoneRemoveInfected.ToString());
                }
                else if (variable.StartsWith("<TradingDistance>"))
                {
                    TradingDistance = Convert.ToDecimal(variable.Replace("<TradingDistance>", ""));
                    Console.WriteLine("TradingDistance " + TradingDistance.ToString());
                }
            }
        }

        public bool SaveVariables(string SaveTime)
        {
            if (!isDirty) return false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<BuySellTimer> " + BuySellTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<VehicleCleanupTimer> " + VehicleCleanupTimer.ToString("0.0") + Environment.NewLine);
            sb.Append("<SafezoneTimeout> " + SafezoneTimeout.ToString("0.0") + Environment.NewLine);
            sb.Append("<SafezoneRemoveAnimals> " + SafezoneRemoveAnimals + Environment.NewLine);
            sb.Append("<SafezoneRemoveInfected> " + SafezoneRemoveInfected + Environment.NewLine);
            sb.Append("<TradingDistance> " + TradingDistance.ToString("0.0") + Environment.NewLine);
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
