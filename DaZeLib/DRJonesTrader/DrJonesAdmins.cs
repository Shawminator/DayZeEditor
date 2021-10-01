using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class DrJonesAdmins
    {
        public BindingList<string> Admins { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }

        public DrJonesAdmins(string filename)
        {
            Filename = filename;
            List<string> file = File.ReadAllLines(Filename).ToList();
            Console.WriteLine("Parsing " + Path.GetFileName(filename) + "....");
            Admins = new BindingList<string>();
            isDirty = false;
            foreach(string line in file)
            {
                if (line.Equals("") || line.StartsWith("//")) continue;
                if (line.StartsWith("<FileEnd>")) continue;

                string adminline = line.Replace("\t", "").Replace(" ", "");
                int index = adminline.IndexOf("/");
                if (index != -1)
                {
                    Admins.Add(adminline.Remove(index, adminline.Length - index));
                    Console.WriteLine("Admin Found.... " + adminline.Remove(index, adminline.Length - index));
                }
                else
                {
                    Admins.Add(adminline);
                    Console.WriteLine("Admin Found.... " + adminline);
                }
            }
        }
        public bool AddnewAdmin(string id)
        {
            if (Admins.Contains(id))
                return false;
            else
            {
                Admins.Add(id);
                isDirty = true;
                return true;
            }
        }
        public void removeAdmin(string id)
        {
            Admins.Remove(id);
            isDirty = true;
        }
        public bool savefile(string SaveTime)
        {
            if (!isDirty) return false;
            StringBuilder sb = new StringBuilder();
            sb.Append("///////////////////////////////////////////////////////////" + Environment.NewLine);
            sb.Append("//                                                       //" + Environment.NewLine);
            sb.Append("//      Put the Steam64 IDs for your Admins in here      //" + Environment.NewLine);
            sb.Append("//      Make a new Line for every ID!                    //" + Environment.NewLine);
            sb.Append("//                                                       //" + Environment.NewLine);
            sb.Append("//  Config Created by DayZ Loot Manager by Shawminator   //" + Environment.NewLine);
            sb.Append("//                                                       //" + Environment.NewLine);
            sb.Append("///////////////////////////////////////////////////////////" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            int i = 1;
            foreach(string admin in Admins)
            {
                sb.Append(admin + " // Admin No" + i.ToString() + Environment.NewLine);
            }
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
