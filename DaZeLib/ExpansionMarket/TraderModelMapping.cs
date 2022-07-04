using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DayZeLib
{
    public class TraderModelMapping
    {
        public string TradermapsPath;
        public BindingList<Tradermap> maps = new BindingList<Tradermap>();
        public bool isDirty { get; set; }

        public TraderModelMapping(string pathname)
        {
            TradermapsPath = pathname;
            maps = new BindingList<Tradermap>();
            if (!Directory.Exists(pathname))
                Directory.CreateDirectory(pathname);
            DirectoryInfo dinfo = new DirectoryInfo(pathname);
            FileInfo[] Files = dinfo.GetFiles("*.map");
            Console.WriteLine("Getting Trader maps....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                Console.WriteLine("Parsing " + file.Name);
                string[] tfile = File.ReadAllLines(file.FullName);
                foreach (string line in tfile)
                {
                    string[] tfilesplit = line.Split('|');
                    if (tfilesplit.Length == 1) continue;
                    Tradermap tmap = new Tradermap(tfilesplit);
                    tmap.Filename = file.FullName;
                    maps.Add(tmap);
                }

            }
        }
        public void savefiles(string saveTime = null)
        {
            DirectoryInfo dinfo = new DirectoryInfo(TradermapsPath);
            FileInfo[] Files = dinfo.GetFiles("*.map");
            if (saveTime != null)
            {
                Directory.CreateDirectory(TradermapsPath + "\\Backup\\" + saveTime);

                Console.WriteLine("Getting Trader maps....");
                Console.WriteLine(Files.Length.ToString() + " Found");
                foreach (FileInfo file in Files)
                {
                    if (File.Exists(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak"))
                        file.MoveTo(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak1");
                    else
                        file.MoveTo(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak");
                }
            }
            else
            {
                foreach (FileInfo file in Files)
                {
                    file.Delete();
                }
            }

            List<string> mappaths = new List<string>();
            foreach (Tradermap map in maps)
            {
                if (!mappaths.Contains(map.Filename))
                {
                    mappaths.Add(map.Filename);
                }
            }
            foreach(string path in mappaths)
            {
                StringBuilder sb = new StringBuilder();
                foreach(Tradermap tm in maps.Where(x => x .Filename == path))
                {
                    sb.Append(tm.NPCName + "." + tm.NPCTrade + "|" + tm.position.X.ToString("F6") + " " + tm.position.Y.ToString("F6") + " " + tm.position.Z.ToString("F6")
                        + "|" + tm.roattions.X.ToString("F6") + " " + tm.roattions.Y.ToString("F6") + " " + tm.roattions.Z.ToString("F6"));
                    if (tm.Attachments == null || tm.Attachments.Count == 0)
                        sb.Append(Environment.NewLine);
                    else
                    {
                        sb.Append("|");
                        int i = 0;
                        foreach(string s in tm.Attachments)
                        {
                            sb.Append(s);
                            if (i != tm.Attachments.Count - 1)
                                sb.Append(",");
                            i++;
                        }
                        sb.Append(Environment.NewLine);
                    }
                }
                File.WriteAllText(path, sb.ToString());
            }
        }
    }
    public class Tradermap
    {
        public string Filename { get; set; }
        public string NPCName { get; set; }
        public string NPCTrade { get; set; }

        public Vec3 position { get; set; }
        public Vec3 roattions { get; set; }
        public BindingList<string> Attachments { get; set; }

        public bool IsInAZone = false;

        public Tradermap()
        {

        }
        public Tradermap(string[] array)
        {
            Attachments = new BindingList<string>();
            if (array[0].StartsWith("//")){ return; }
            string[] Part1 = array[0].Split('.');
            NPCName = Part1[0];
            NPCTrade = Part1[1];
            position = new Vec3(array[1].Split(' '));
            roattions = new Vec3(array[2].Split(' '));
            if(array.Length == 4)
            {
                foreach(string s in array[3].Split(','))
                    Attachments.Add(s);
            }
        }
        public override string ToString()
        {
            return NPCName + " " + NPCTrade;
        }
    }


}
