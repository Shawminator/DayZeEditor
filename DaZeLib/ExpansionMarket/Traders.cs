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
using DayZeLib;

namespace DayZeLib
{
    public class TradersList
    {
        public BindingList<Traders> Traderlist { get; set; }
        public string TraderPath { get; set; }

        public TradersList()
        {
            Traderlist = new BindingList<Traders>();
        }
        public TradersList(string Path, MarketCategories marketCats)
        {
            TraderPath = Path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            Traderlist = new BindingList<Traders>();
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting Traders....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("serializing " + file.Name);
                    Traders t = JsonSerializer.Deserialize<Traders>(File.ReadAllText(file.FullName));
                    t.Filename = file.FullName;
                    Console.WriteLine("Converting Stock Dictionary to list");
                    t.ConvertDictToList(marketCats);
                    Traderlist.Add(t);
                }
                catch
                {
                MessageBox.Show("there is An error in the following file\n" + file.FullName);

                }
            }
        }
        public void RemoveItemFromTrader(string removeitem)
        {
            foreach (Traders t in Traderlist)
            {
                if(t.ListItems.Any(x => x.ClassName == removeitem))
                {
                    t.removetraderitem(removeitem);
                }
            }
        }
        public void removelistfromtrader(List<string> toremovefromtraders)
        {
           foreach(string item in toremovefromtraders)
            {
                RemoveItemFromTrader(item);
            }
        }
        public Traders GetTraderFromName(string name)
        {
            return Traderlist.FirstOrDefault(x => x.TraderName == name);
        }
        public void removeTrader(Traders removeitem)
        {
            removeitem.backupandDelete();
            Traderlist.Remove(removeitem);
        }
        public void AddNewTrader(string tradername)
        {
            Traders t = new Traders(tradername);
            t.Filename = TraderPath + "\\" + tradername + ".json";
            if (Traderlist.Any(x => x.TraderName == tradername))
            {
                MessageBox.Show(tradername = " Allready in list of traders....");
                return;
            }
            else
            {
                Traderlist.Add(t);
                MessageBox.Show(tradername = " added to list of  traders....");
            }
        }
        public void CheckCategories(MarketCategories marketCats)
        {
            
            
        }
    }
    public class Traders
    {
        public int m_Version { get; set; } //current Version 7
        public string TraderName { get; set; }
        public string DisplayName { get; set; }
        public string TraderIcon { get; set; }
        public BindingList<string> Currencies { get; set; }
        public BindingList<string> Categories { get; set; }
        public Dictionary<string, int> Items { get; set; }

 
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public BindingList<TradersItem> ListItems { get; set; }
        [JsonIgnore]
        public bool isDirty = false;


        public Traders(string filename)
        {
            m_Version = 5;
            TraderName = filename;
            DisplayName = filename;
            Currencies = new BindingList<string>();
            Items = new Dictionary<string, int>();
            ListItems = new BindingList<TradersItem>();
        }
        public override string ToString()
        {
            return TraderName;
        }
        public void ConvertDictToList(MarketCategories marketCats)
        {
            var initialList = new BindingList<TradersItem>();
            foreach (string cat in Categories)
            {
                List<marketItem> itemslist = marketCats.GetCatFromFileName(cat).Items.ToList();
                foreach (marketItem item in itemslist)
                {
                    TradersItem ti = new TradersItem() { ClassName = item.ClassName.ToLower(), buysell = canBuyCansell.CanBuyAndsell, CatName = cat};
                    if(!initialList.Any(x => x.ClassName == ti.ClassName))
                        initialList.Add(ti);
                }
            }
            foreach (KeyValuePair<string, int> item in Items)
            {
                if(item.Value == 3) { continue; }
                if(initialList.Any(x => x.ClassName == item.Key))
                {
                    TradersItem eti = initialList.First(x => x.ClassName == item.Key);
                    eti.buysell = (canBuyCansell)item.Value;
                }
                else
                {
                    string catname = marketCats.GetCatNameFromItemName(item.Key);
                    TradersItem ti = new TradersItem() { ClassName = item.Key, buysell = (canBuyCansell)item.Value, CatName = catname };
                    initialList.Add(ti);
                }
                
            }
            ListItems = new BindingList<TradersItem>(new BindingList<TradersItem>(initialList.OrderBy(x => x.ClassName).ToList()));
        }
        public void SortList()
        {
            
        }
        public void ConvertToDict(MarketCategories MarketCats)
        {
            Items = new Dictionary<string, int>();
            List<string> CatNames = new List<string>();
            List<string> tItems = new List<string>();
            List<string> cats = new List<string>();
            foreach (TradersItem TI in ListItems)
            {
                tItems.Add(TI.ClassName);
                if (!CatNames.Contains(TI.CatName))
                    CatNames.Add(TI.CatName);
            }
            tItems.Sort();
            foreach (string catname in CatNames)
            {
                List<marketItem> catitemlist = MarketCats.GetCatFromFileName(catname).Items.ToList();
                List<string> TIlist = new List<string>();
                foreach (marketItem item in catitemlist)
                {
                    TIlist.Add(item.ClassName);
                }
                if (Helper.ContainsAllItems(tItems, TIlist))
                {
                    cats.Add(catname);
                }
            }
            foreach (TradersItem TI in ListItems)
            {
                marketItem item = MarketCats.getitemfromcategory(TI.ClassName);
                if(item.SpawnAttachments.Count > 0)
                {
                    foreach (string attachment in item.SpawnAttachments)
                    {
                        if (!tItems.Contains(attachment) && !Items.ContainsKey(attachment))
                        {
                            bool isvarient = false;
                            foreach(string titem in tItems)
                            {
                                if(MarketCats.getitemfromcategory(titem).Variants.Contains(attachment))
                                {
                                    if (tItems.Contains(MarketCats.getitemfromcategory(titem).ClassName))
                                    {
                                        isvarient = true;
                                        break;
                                    }
                                }
                            }
                            if(!isvarient)
                                Items.Add(attachment, 3);
                        }
                    }
                }
                if(cats.Contains(TI.CatName) && TI.buysell == canBuyCansell.CanBuyAndsell) { continue; }
                Items.Add(TI.ClassName, (int)TI.buysell);
            }
            Categories = new BindingList<string>(cats.ToList());
        }
        public void removetraderitem(string name)
        {
            var itemToRemove = ListItems.SingleOrDefault(x => x.ClassName == name);
            if (itemToRemove != null)
            {
                ListItems.Remove(itemToRemove);
                isDirty = true;
            }
        }
        public void AdditemtoTrader(string name, string Catname)
        {
            TradersItem ti = new TradersItem();
            ti.ClassName = name;
            ti.buysell = canBuyCansell.CanBuyAndsell;
            ti.CatName = Catname;
            if (!ListItems.Any(x => x.ClassName == ti.ClassName))
            {
                ListItems.Add(ti);
                isDirty = true;
            }
            else
            {
                TradersItem eti = ListItems.First(x => x.ClassName == ti.ClassName);
                eti.buysell = ti.buysell;

            }
        }
        public void backupandDelete()
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (File.Exists(Filename))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".bak");
                File.Delete(Filename);
            }
        }
    }
    public class TradersItem
    {
        public string ClassName { get; set; }
        public canBuyCansell buysell { get; set; }  //Items: 0 = Can only buy, 1 = can buy and sell, 2 = can only sell

        public string CatName { get; set; }
        public bool isvarient { get; set; }
        public override string ToString()
        {
            return ClassName;
        }
    }
    public enum canBuyCansell
    {
        CanOnlyBuy = 0,
        CanBuyAndsell = 1,
        CanOnlySell = 2,
        Attchment = 3
   }

    public class TraderListItem
    {
        public string Classname { get; set; }

        public override string ToString()
        {
            return Classname.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "");
        }
    }
}
