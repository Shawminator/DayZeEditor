using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public class TraderZones
    {
        public const int CurrentVersion = 6;

        public BindingList<Zones> ZoneList { get; set; }
        public string ZonesPath { get; set; }

        public TraderZones()
        {
            ZoneList = new BindingList<Zones>();
        }
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
                bool savefile = false;
                Console.WriteLine("serializing " + file.Name);
                Zones z = JsonSerializer.Deserialize<Zones>(File.ReadAllText(file.FullName));
                Console.WriteLine("Converting trader Item Dictionary to list");
                z.ConvertDicttoList();
                z.Filename = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
                ZoneList.Add(z);
                if (savefile)
                {
                    File.Delete(file.FullName);
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(z, options);
                    File.WriteAllText(ZonesPath + "\\" + z.Filename + ".json", jsonString);
                }
            }
        }
        public void ConvertallListstoDict()
        {
            foreach (Zones z in ZoneList)
            {
                z.ConvertlisttoDict();
            }
        }
        public void NewTraderZone(string zonename, int mapsize, bool showmessage = true)
        {
            Zones z = new Zones(zonename.Replace("_", " "));
            z.Position = new float[] { mapsize / 2, 0, mapsize / 2 };
            z.Radius = mapsize / 2;
            z.isDirty = true;
            z.Filename = zonename.Replace(" ", "_");
            if (ZoneList.Any(x => x.Filename == z.Filename))
            {
                MessageBox.Show(zonename = " Allready in list of Zones....");
                return;
            }
            else
            {
                ZoneList.Add(z);
                if (showmessage == true)
                {
                    z.isDirty = true;
                    MessageBox.Show(zonename = " added to list of Zones....");
                }
            }
        }
        public void removeZone(Zones removeitem)
        {
            removeitem.backupAndDelete(ZonesPath);
            ZoneList.Remove(removeitem);
        }
        public Zones GetZoneZoneName(string removeitem)
        {
            return ZoneList.First(x => x.m_DisplayName == removeitem);
        }
    }
    public class Zones
    {
        public int m_Version { get; set; }  //current version 5
        public string m_DisplayName { get; set; }
        public float[] Position { get; set; }
        public decimal Radius { get; set; }
        public decimal BuyPricePercent { get; set; }
        public decimal SellPricePercent { get; set; }
        public Dictionary<string, int> Stock { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public BindingList<StockItem> StockItems { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

        public Zones()
        {
            m_Version = TraderZones.CurrentVersion;
            m_DisplayName = "World Zone";
            Position = new float[] { 0, 0, 0 };
            Radius = 1000;
            BuyPricePercent = 100;
            SellPricePercent = -1;
            Stock = new Dictionary<string, int>();
            StockItems = new BindingList<StockItem>();
        }

        public Zones(string filename)
        {
            m_Version = TraderZones.CurrentVersion;
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
            return m_DisplayName;
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
            foreach (StockItem si in StockItems)
            {
                Stock.Add(si.Classname, si.StockValue);
            }
        }
        public void backupAndDelete(string ZonesPath)
        {
            string fullfilename = ZonesPath + "\\" + Filename + ".json";
            if (File.Exists(fullfilename))
            {
                string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
                Directory.CreateDirectory(ZonesPath + "\\Backup\\" + SaveTime);
                File.Copy(fullfilename, ZonesPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak", true);
                File.Delete(fullfilename);
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
