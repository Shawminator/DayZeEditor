using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public class TradersList
    {
        public const int CurrentVersion = 12;
        public BindingList<Traders> Traderlist { get; set; }
        public string TraderPath { get; set; }
        public bool SortedbyDisplayName { get; set; }
        public List<Traders> Markedfordelete { get; set; }

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
                    bool savefile = false;
                    Console.WriteLine("serializing " + file.Name);
                    Traders t = JsonSerializer.Deserialize<Traders>(File.ReadAllText(file.FullName));
                    if (System.IO.Path.GetFileNameWithoutExtension(file.FullName).Any(char.IsLower))
                    {
                        t.Filename = System.IO.Path.GetFileNameWithoutExtension(file.FullName).ToUpper();
                        savefile = true;
                    }
                    else
                    {
                        t.Filename = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
                    }
                    Console.WriteLine("Converting Stock Dictionary to list");
                    if (t.ConvertDictToList(marketCats))
                        savefile = true;
                    Traderlist.Add(t);
                    if (t.m_Version != CurrentVersion)
                    {
                        t.m_Version = CurrentVersion;
                        savefile = true;
                    }

                    if (savefile)
                    {
                        File.Delete(file.FullName);
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(t, options);
                        File.WriteAllText(TraderPath + "\\" + t.Filename + ".json", jsonString);
                    }
                }
                catch
                {
                    MessageBox.Show("there is An error in the following file\n" + file.FullName);

                }
            }
            List<string> duplist = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.Append("The below files are duplicated, please manually fix:" + Environment.NewLine);
            bool printmessage = false;
            foreach (Categories cat in marketCats.CatList)
            {
                foreach (marketItem item in cat.Items)
                {
                    if (duplist.Any(x => x.Split(',')[0] == item.ClassName))
                    {
                        printmessage = true;
                        sb.Append(duplist.First(x => x.Split(',')[0] == item.ClassName) + "," + cat.DisplayName + Environment.NewLine);
                    }
                    duplist.Add(item.ClassName + "," + cat.DisplayName);
                }
            }
            if (printmessage)
            {
                MessageBox.Show(sb.ToString());
            }
        }


        public void RemoveItemFromTrader(string removeitem)
        {
            foreach (Traders t in Traderlist)
            {
                if (t.ListItems.Any(x => x.ClassName == removeitem))
                {
                    t.removetraderitem(removeitem);
                }
            }
        }
        public void removelistfromtrader(List<string> toremovefromtraders)
        {
            foreach (string item in toremovefromtraders)
            {
                RemoveItemFromTrader(item);
            }
        }
        public Traders GetTraderFromName(string name)
        {
            return Traderlist.FirstOrDefault(x => x.Filename == name);
        }
        public void removeTrader(Traders removeitem)
        {
            if (Markedfordelete == null) Markedfordelete = new List<Traders>();
            Markedfordelete.Add(removeitem);
            Traderlist.Remove(removeitem);
        }
        public void AddNewTrader(string m_fileName)
        {
            Traders t = new Traders(m_fileName.ToUpper().Replace("_", " "));
            t.Filename = m_fileName.ToUpper().Replace(" ", "_");
            t.SortByDisplayName = SortedbyDisplayName;
            if (Traderlist.Any(x => x.Filename == t.Filename))
            {
                MessageBox.Show(t.Filename + " Allready in list of traders....");
                return;
            }
            else
            {
                Traderlist.Add(t);
                t.isDirty = true;
                MessageBox.Show(t.Filename + " added to list of  traders....");
            }
        }
        public void SortbyDisplayName()
        {
            var sortedListInstance = new BindingList<Traders>(Traderlist.OrderBy(x => x.DisplayName).ToList());
            foreach (Traders t in sortedListInstance)
            {
                t.SortByDisplayName = true;
            }
            Traderlist = sortedListInstance;
            SortedbyDisplayName = true;
        }
        public void SortByFilename()
        {
            var sortedListInstance = new BindingList<Traders>(Traderlist.OrderBy(x => x.Filename).ToList());
            foreach (Traders t in sortedListInstance)
            {
                t.SortByDisplayName = false;
            }
            Traderlist = sortedListInstance;
            SortedbyDisplayName = false;
        }
    }
    public class Traders
    {
        public int m_Version { get; set; }
        public string DisplayName { get; set; }
        public int MinRequiredReputation { get; set; }
        public int MaxRequiredReputation { get; set; }
        public string RequiredFaction { get; set; }
        public int RequiredCompletedQuestID { get; set; }
        public string TraderIcon { get; set; }
        public BindingList<string> Currencies { get; set; }
        public int DisplayCurrencyValue { get; set; }
        public string DisplayCurrencyName { get; set; }
        public BindingList<string> Categories { get; set; }
        public Dictionary<string, int> Items { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public BindingList<TradersItem> ListItems { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public bool SortByDisplayName { get; set; }

        public Traders()
        {
            m_Version = TradersList.CurrentVersion;
            DisplayName = "";
            TraderIcon = "";
            Currencies = new BindingList<string>();
            Items = new Dictionary<string, int>();
            ListItems = new BindingList<TradersItem>();
            Categories = new BindingList<string>();
            RequiredFaction = "";
            RequiredCompletedQuestID = -1;
            DisplayCurrencyValue = 1;
            DisplayCurrencyName = "";
        }
        public Traders(string filename)
        {
            m_Version = TradersList.CurrentVersion;
            DisplayName = filename;
            MinRequiredReputation = 0;
            MaxRequiredReputation = 2147483647;
            TraderIcon = "Questionmark";
            Currencies = new BindingList<string>();
            Items = new Dictionary<string, int>();
            ListItems = new BindingList<TradersItem>();
            Categories = new BindingList<string>();
            RequiredFaction = "";
            RequiredCompletedQuestID = -1;
            TraderIcon = "Trader";
            DisplayCurrencyName = "";
            DisplayCurrencyValue = 1;
        }
        public override string ToString()
        {
            if (SortByDisplayName)
                return DisplayName.Replace("#STR_EXPANSION_MARKET_TRADER_", "");
            else
                return Filename;
        }
        public bool ConvertDictToList(MarketCategories marketCats)
        {
            bool savefile = false;
            var initialList = new BindingList<TradersItem>();
            if (Categories.Count == 1 && Categories[0] == null)
                Categories = new BindingList<string>();
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i].Contains(":"))
                {
                    string[] test = Categories[i].Split(':');
                    if (System.IO.Path.GetFileNameWithoutExtension(test[0]).Any(char.IsLower))
                    {
                        Categories[i] = test[0].ToUpper() + ":" + test[1];
                        savefile = true;
                    }
                }
                else
                {
                    if (System.IO.Path.GetFileNameWithoutExtension(Categories[i]).Any(char.IsLower))
                    {
                        Categories[i] = Categories[i].ToUpper();
                        savefile = true;
                    }
                }
            }


            foreach (string cat in Categories)
            {
                string[] results = cat.Split(':');
                canBuyCansell cbs = canBuyCansell.CanBuyAndsell;
                if (results.Length == 2)
                {
                    cbs = (canBuyCansell)Convert.ToInt32(results[1]);
                }
                try
                {
                    List<marketItem> itemslist = marketCats.GetCatFromFileName(results[0]).Items.ToList();
                    foreach (marketItem item in itemslist)
                    {
                        TradersItem ti = new TradersItem() { ClassName = item.ClassName.ToLower(), buysell = cbs, CatName = results[0] };
                        if (item.SpawnAttachments.Count > 0)
                            ti.HasAttachemnts = true;
                        if (item.Variants.Count > 0)
                            ti.HasVarients = true;

                        if (!initialList.Any(x => x.ClassName == ti.ClassName))
                            initialList.Add(ti);

                    }
                }
                catch
                {
                    MessageBox.Show(Path.GetFileName(Filename) + " Conatins and category enrty for " + results[0] + " That doesn not exist in the market folder");
                }

            }
            foreach (KeyValuePair<string, int> item in Items)
            {
                if (initialList.Any(x => x.ClassName == item.Key))
                {
                    TradersItem eti = initialList.First(x => x.ClassName == item.Key.ToLower());
                    eti.buysell = (canBuyCansell)item.Value;
                }
                else
                {
                    string catname = marketCats.GetCatNameFromItemName(item.Key);
                    TradersItem ti = new TradersItem() { ClassName = item.Key.ToLower(), buysell = (canBuyCansell)item.Value, CatName = catname };
                    initialList.Add(ti);
                }

            }
            ListItems = new BindingList<TradersItem>(new BindingList<TradersItem>(initialList.OrderBy(x => x.ClassName).ToList()));
            return savefile;
        }
        public void SortList()
        {

        }
        public void ConvertToDict(MarketCategories MarketCats)
        {
            Items = new Dictionary<string, int>();
            List<string> CatNames = new List<string>();
            List<string> tItems = new List<string>();
            List<string> donetItems = new List<string>();
            List<string> cats = new List<string>();

            //get list of category names so when can check if all items from that cat are included, if so we put the catname in the trader cat list
            //any different buysell in that cat will be added seperatley to items list
            //also add all item name to a list for easy access when checking the above
            foreach (TradersItem TI in ListItems)
            {
                tItems.Add(TI.ClassName);
                if (!CatNames.Contains(TI.CatName))
                    CatNames.Add(TI.CatName);
            }
            tItems.Sort();  //sorting for easier reading


            foreach (string catname in CatNames)
            {
                List<marketItem> catitemlist = MarketCats.GetCatFromFileName(catname).Items.ToList();
                if (catitemlist.Count == 0) continue;
                List<string> TIlist = new List<string>();
                foreach (marketItem item in catitemlist)
                {
                    TIlist.Add(item.ClassName);
                }
                if (Helper.ContainsAllItems(tItems, TIlist))
                {
                    List<int> buysell = new List<int>();
                    foreach (String Bitem in TIlist)
                    {
                        TradersItem ti = ListItems.First(x => x.ClassName == Bitem);
                        buysell.Add((int)ti.buysell);
                    }
                    int maxRepeated = buysell.GroupBy(s => s).OrderByDescending(s => s.Count()).First().Key;
                    canBuyCansell cbs = (canBuyCansell)maxRepeated;
                    if (cbs == canBuyCansell.CanBuyAndsell)
                        cats.Add(catname);
                    else
                        cats.Add(catname + ":" + ((int)cbs).ToString());
                    foreach (string B2item in TIlist)
                    {
                        TradersItem ti = ListItems.First(x => x.ClassName == B2item);
                        marketItem item = MarketCats.getitemfromcategory(ti.ClassName);
                        foreach (string attach in item.SpawnAttachments)
                        {
                            if (!tItems.Contains(attach) && !Items.ContainsKey(attach))
                            {
                                bool additem = true;
                                foreach (string name in tItems)
                                {
                                    marketItem vitem = MarketCats.getitemfromcategory(attach);
                                    if (vitem != null && vitem.Variants.Contains(attach))
                                    {
                                        additem = false;
                                        break;
                                    }
                                }
                                if (additem)
                                    Items.Add(attach, (int)canBuyCansell.Attchment);
                            }
                        }
                        donetItems.Add(ti.ClassName);
                        if (ti.buysell != cbs)
                        {
                            Items.Add(ti.ClassName, (int)ti.buysell);
                        }
                    }
                }
            }
            foreach (string item in donetItems)
            {
                tItems.Remove(item);
            }
            foreach (string litem in tItems)
            {
                TradersItem TI = ListItems.First(x => x.ClassName == litem);
                marketItem item = MarketCats.getitemfromcategory(TI.ClassName);
                if (item.SpawnAttachments.Count > 0)
                {
                    foreach (string attachment in item.SpawnAttachments)
                    {
                        if (!tItems.Contains(attachment) && !Items.ContainsKey(attachment))
                        {
                            bool isvarient = false;
                            foreach (string titem in tItems)
                            {
                                if (MarketCats.getitemfromcategory(titem).Variants.Contains(attachment))
                                {
                                    if (tItems.Contains(MarketCats.getitemfromcategory(titem).ClassName))
                                    {
                                        isvarient = true;
                                        break;
                                    }
                                }
                            }
                            if (!isvarient)
                                Items.Add(attachment, 3);
                        }
                    }
                }
                if (cats.Contains(TI.CatName) && TI.buysell == canBuyCansell.CanBuyAndsell) { continue; }
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
            ti.ClassName = name.ToLower();
            ti.buysell = canBuyCansell.CanBuyAndsell;
            ti.CatName = Catname;
            if (!ListItems.Any(x => x.ClassName.ToLower() == ti.ClassName.ToLower()))
            {
                ListItems.Add(ti);
                isDirty = true;
            }
            else
            {
                TradersItem eti = ListItems.First(x => x.ClassName.ToLower() == ti.ClassName.ToLower());
                eti.buysell = ti.buysell;

            }
        }
        public void backupandDelete(string traderpath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string fullfilename = traderpath + "\\" + Filename + ".json";
            if (File.Exists(fullfilename))
            {
                Directory.CreateDirectory(traderpath + "\\Backup\\" + SaveTime);
                File.Copy(fullfilename, traderpath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(fullfilename);
            }
        }
    }
    public class TradersItem
    {
        public string ClassName { get; set; }
        public canBuyCansell buysell { get; set; }  //Items: 0 = Can only buy, 1 = can buy and sell, 2 = can only sell, 3 is attachment, not visiable in trader

        public string CatName { get; set; }
        public bool HasVarients { get; set; }
        public bool HasAttachemnts { get; set; }
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
