using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public void savefiles(string saveTime)
        {
            DirectoryInfo dinfo = new DirectoryInfo(TradermapsPath);
            Directory.CreateDirectory(TradermapsPath + "\\Backup\\" + saveTime);
            FileInfo[] Files = dinfo.GetFiles("*.map");
            Console.WriteLine("Getting Trader maps....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                if (File.Exists(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak"))
                    file.MoveTo(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak1");
                else
                    file.MoveTo(TradermapsPath + "\\Backup\\" + saveTime + "\\" + file.Name + ".bak");
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
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vec3(string[] floats)
        {
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";
            X = Convert.ToSingle(floats[0], format);
            Y = Convert.ToSingle(floats[1], format);
            Z = Convert.ToSingle(floats[2], format);
        }
        public Vec3() { }
    }
    public class MapData
    {
        public bool FileExists { get; set; }
        public string FileName { get; set; }

        public MapData(string filename)
        {
            if (File.Exists(filename))
            {
                FileName = filename;
                FileExists = true;
            }
            else
                FileExists = false;
        }
        public void CreateNewData()
        {
            List<Vec3> points = new List<Vec3>();
            string[] lines = File.ReadAllLines(FileName);
            foreach (string line in lines)
            {
                //string line1 = Helper.TrimSpaces(line);
                //line1 = line1.Replace(" ", ",");
                //int firstindex = line1.IndexOf(",");
                //int lastindex = line1.LastIndexOf(',');
                //StringBuilder sb = new StringBuilder(line);
                //sb[firstindex] = '|';
                //sb[lastindex] = '|';
                //line1 = sb.ToString();
                //line1 = line1.Replace(",", "");
                string[] split = line.Split(' ');
                string[] newaary = new string[3];
                newaary[0] = split[0];
                newaary[1] = split[2];
                newaary[2] = split[1];
                points.Add(new Vec3(newaary));
            }
            using (FileStream writeStream = new FileStream(FileName + ".bin", FileMode.Create))
            using (BinaryWriter writeBinay = new BinaryWriter(writeStream))
            {
                writeBinay.Write((long)points.Count());
                foreach (Vec3 v in points)
                {
                    writeBinay.Write(v.X);
                    writeBinay.Write(v.Y);
                    writeBinay.Write(v.Z);
                }
            }
        }
        public float gethieght(float v1, float v2)
        {
            //CreateNewData(); only used to bin map file
            List<Vec3> points = new List<Vec3>();
            byte[] bytearray = File.ReadAllBytes(FileName);
            using (MemoryStream ms = new MemoryStream(bytearray))
            using (BinaryReader br = new BinaryReader(ms))
            {
                long count = br.ReadInt64();
                for (int i = 0; i < count; i++)
                {
                    Vec3 newvec = new Vec3();
                    newvec.X = br.ReadSingle();
                    newvec.Y = br.ReadSingle();
                    newvec.Z = br.ReadSingle();
                    points.Add(newvec);
                }
            }
            //points id a List<vec3> containing X,Y,Z values.

            List<Vec3> test = points
                .OrderBy(x => Math.Abs(v1 - x.X))
                .ToList();
            List<Vec3> test2 = test
                .FindAll(x => x.X == test[0].X)
                .OrderBy(x => Math.Abs(v2 - x.Y))
                .ToList();
            return test2[0].Z;

        }
    }
}
