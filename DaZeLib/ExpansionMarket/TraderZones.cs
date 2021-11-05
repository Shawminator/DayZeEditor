using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class TraderZones
    {
        public BindingList<Zones> ZoneList { get; set; }
        public string ZonesPath { get; set; }


        public TraderZones(string Path)
        {
            ZoneList = new BindingList<Zones>();
            ZonesPath = Path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting Trader Zones....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                Console.WriteLine("serializing " + file.Name);
                Zones z = JsonSerializer.Deserialize<Zones>(File.ReadAllText(file.FullName));
                Console.WriteLine("Converting trader Item Dictionary to list");
                z.ConvertDicttoList();
                z.Filename = file.FullName;
                ZoneList.Add(z);
            }
        }
        public void ConvertallListstoDict()
        {
            foreach (Zones z in ZoneList)
            {
                z.ConvertlisttoDict();
            }
        }
        public void NewTraderZone(string zonename, int mapsize)
        {
            Zones z = new Zones(zonename);
            z.Position = new float[] { mapsize/2, 0, mapsize/2 };
            z.isDirty = true;
            z.Filename = ZonesPath + "\\" + z.m_ZoneName + ".json";
            if (ZoneList.Any(x => x.m_ZoneName == zonename))
            {
                MessageBox.Show(zonename = " Allready in list of Zones....");
                return;
            }
            else
            {
                ZoneList.Add(z);
                MessageBox.Show(zonename = " added to list of Zones....");
            }
        }
        public void removeZone(Zones removeitem)
        {
            removeitem.backupAndDelete();
            ZoneList.Remove(removeitem);
        }
        public Zones GetZoneZoneName(string removeitem)
        {
            return ZoneList.First(x => x.m_ZoneName == removeitem);
        }
    }
    public class Zones
    {
        public int m_Version { get; set; }  //current version 5
        public string m_ZoneName { get; set; }
        public string m_DisplayName { get; set; }
        public float[] Position { get; set; }
        public float Radius { get; set; }
        public float BuyPricePercent { get; set; }
        public float SellPricePercent { get; set; }
        public Dictionary<string, int> Stock { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public BindingList<StockItem> StockItems {get;set;}
        [JsonIgnore]
        public bool isDirty = false;

        public Zones (string filename)
        {
            m_Version = 5;
            m_ZoneName = filename;
            m_DisplayName = filename;
            Position = new float[] { 0, 0, 0 };
            Radius = 1000;
            BuyPricePercent = 100;
            SellPricePercent = -1;
            Stock = new Dictionary<string, int>();
            StockItems = new BindingList<StockItem>();
        }
        public override string ToString()
        {
            return m_ZoneName;
        }
        public void ConvertDicttoList()
        {
            var intialList = new BindingList<StockItem>();
            foreach (KeyValuePair<string, int> dcit in Stock)
            {
                StockItem si = new StockItem();
                si.Classname = dcit.Key;
                si.StockValue = dcit.Value;
                intialList.Add(si);
            }
            StockItems = new BindingList<StockItem>(new BindingList<StockItem>(intialList.OrderBy(x => x.Classname).ToList()));
        }
        public void ConvertlisttoDict()
        {
            Stock = new Dictionary<string, int>();
            foreach(StockItem si in StockItems)
            {
                Stock.Add(si.Classname, si.StockValue);
            }
        }
        public void backupAndDelete()
        {
            if (File.Exists(Filename))
            {
                string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".bak", true);
                File.Delete(Filename);
            }
        }
    }
    public class StockItem
    {
        public string Classname { get; set; }
        public int StockValue { get; set; }
        //public int ZoneBuyPrice { get; set; }
        //public int ZoneSellPrice { get; set; }
        //public int StockCheker { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }
}
