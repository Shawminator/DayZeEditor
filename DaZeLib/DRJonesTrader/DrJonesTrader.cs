using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class DRJonesTraderConfig
    {
        public string Filename { get; set; }
        public string path { get; set; }
        public bool isDirty { get; set; }

        public DRJonesTraderCurrency CurrencyConfig;
        public BindingList<DRjonesTraders> m_Traders;
        public BindingList<DRJonesCategories> m_categories;
        public BindingList<DrjonesItems> DrjonesitemList;
        public BindingList<DrjonesFullTraderconfig> drjonesfullList { get; set; }

        public string traderfilename;
        public bool OpenNewFileForReading(string line_content, out FileStream fs,  out StreamReader reader1)
        {
            line_content = line_content.Replace("<OpenFile>", "");
            line_content = Helper.TrimComment(line_content);

            fs = new FileStream(path + "\\" + line_content, FileMode.Open, FileAccess.Read, FileShare.Read);
            reader1 = new StreamReader(fs);
            if (reader1 == null)
            {
                return false;
            }
            traderfilename = path + "\\" + line_content;
            return true;
        }
        public DRJonesTraderConfig(string filename)
        {
            Filename = filename;
            traderfilename = filename;
            path = Path.GetDirectoryName(filename);
            isDirty = false;
            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(fs);
            string line_content = "";
            Console.WriteLine("trader Config Found, Begin Parsing......");
            line_content = Helper.SearchForNextTermInFile(reader, "<CurrencyName>", "");
            line_content = line_content.Replace("<CurrencyName>", "");
            line_content = Helper.TrimComment(line_content);
            Console.WriteLine("Currency Found.... " + line_content);
            CurrencyConfig = new DRJonesTraderCurrency();
            CurrencyConfig.m_Trader_CurrencyName = line_content;
            CurrencyConfig.currencyList = new BindingList<DRJonesCurrency>();
            int currencyCounter = 0;

            line_content = "";
            while (currencyCounter <= 500 && !line_content.Contains("<Trader>"))
            {
                line_content = Helper.SearchForNextTermInFile(reader, "<Currency>", "<Trader>");
                line_content = line_content.Replace("<Currency>", "");
                line_content = Helper.TrimComment(line_content);
                if (line_content.Contains("<Trader>"))
                    break;

                string[] crys = line_content.Split(',');

                string currencyClassname = crys[0];
                currencyClassname = Helper.TrimSpaces(currencyClassname);

                string currencyValue = crys[1];
                currencyValue = Helper.TrimSpaces(currencyValue);

                DRJonesCurrency currency = new DRJonesCurrency();
                currency.m_Trader_CurrencyClassnames = currencyClassname;
                currency.m_Trader_CurrencyValues = Convert.ToInt32(currencyValue);
                CurrencyConfig.currencyList.Add(currency);
                Console.WriteLine("Currency classname Found...." + currencyClassname + " with value of " + currency.m_Trader_CurrencyValues);
                currencyCounter++;
            }

            bool traderInstanceDone = true;
            int traderCounter = 0;
            int categoryCounter = 0;
            m_categories = new BindingList<DRJonesCategories>();
            m_Traders = new BindingList<DRjonesTraders>();
            while (traderCounter <= 5000 && line_content != "<FileEnd>")
            {
                if (traderInstanceDone == false)
                    line_content = Helper.SearchForNextTermsInFile(reader, new string[] { "<Trader>", "<OpenFile>" }, "");
                else
                    traderInstanceDone = false;
                if (line_content.Contains("<OpenFile>"))
                {
                    return;
                }
                line_content = line_content.Replace("<Trader>", "");
                line_content = Helper.TrimComment(line_content);
                DRjonesTraders trader = new DRjonesTraders();
                trader.m_filename = traderfilename;
                Console.WriteLine("Trader Found.... " + line_content);
                trader.m_Trader_TraderNames = line_content;
                trader.traderID = traderCounter;
                m_Traders.Add(trader);
                line_content = "";
                while (categoryCounter <= 5000 && line_content != "<FileEnd>")
                {
                    line_content = Helper.TrimComment(Helper.SearchForNextTermsInFile(reader, new string[] { "<Category>", "<OpenFile>" }, "<Trader>"));

                    if (line_content.StartsWith("<OpenFile>"))
                    {
                        reader.Close();
                        reader.Dispose();
                        fs.Close();
                        fs.Dispose();
                        if (OpenNewFileForReading(line_content, out fs, out reader))
                            continue;
                        else
                            return;
                    }

                    if (line_content.Contains("<Trader>"))
                    {
                        traderInstanceDone = true;
                        break;
                    }

                    if (line_content == string.Empty)
                    {
                        line_content = "<FileEnd>";
                        break;
                    }
                    line_content = line_content.Replace("<Category>", "");
                    DRJonesCategories cat = new DRJonesCategories();
                    cat.m_Trader_Categorys = Helper.TrimComment(line_content);
                    cat.m_Trader_CategorysTraderKey = traderCounter;
                    cat.m_Trader_CategoryID = categoryCounter;
                    m_categories.Add(cat);
                    Console.WriteLine("Category Found.... " + cat.m_Trader_Categorys);
                    categoryCounter++;
                }
                traderCounter++;
            }
            reader.Close();
            reader.Dispose();
            fs.Close();
            fs.Dispose();
            Console.WriteLine("Parsing Items.... ");

            reader = new StreamReader(Filename);
            int itemCounter = 0;
            int char_count = 0;
            int traderID = -1;
            int categoryId = -1;
            DrjonesitemList = new BindingList<DrjonesItems>();
            line_content = "";
            while (itemCounter <= 10000 && char_count != -1 && line_content.Contains("<FileEnd>") == false)
            {
                line_content = reader.ReadLine();
                char_count = line_content.Length;
                line_content = Helper.TrimComment(line_content);
                if (line_content.Contains("<OpenFile>"))
                {
                    reader.Close();
                    reader.Dispose();
                    fs.Close();
                    fs.Dispose();
                    if (OpenNewFileForReading(line_content, out fs, out reader))
                        continue;
                    else
                        return;
                }
                if (line_content.Contains("<Trader>"))
                {
                    traderID++;
                    itemCounter = 0;
                    continue;
                }
                if (line_content.Contains("<Category>"))
                {
                    categoryId++;
                    itemCounter = 0;
                    continue;
                }
                if (!line_content.Contains(","))
                    continue;
                if (line_content.Contains("<Currency"))
                    continue;
                try
                {
                    string[] strs = line_content.Split(',');
                    string itemStr = strs[0];
                    itemStr = Helper.TrimSpaces(itemStr);
                    string qntStr = strs[1];
                    qntStr = Helper.TrimSpaces(qntStr);
                    string buyStr = strs[2];
                    buyStr = Helper.TrimSpaces(buyStr);
                    string sellStr = strs[3];
                    sellStr = Helper.TrimSpaces(sellStr);
                    DrjonesItems item = new DrjonesItems();
                    item.m_Trader_ItemsTraderId = traderID;
                    item.m_Trader_ItemsCategoryId = categoryId;
                    item.m_Trader_ItemsClassnames = itemStr;
                    item.m_Trader_ItemsQuantity = qntStr;
                    item.m_Trader_ItemsBuyValue = Convert.ToInt32(buyStr);
                    item.m_Trader_ItemsSellValue = Convert.ToInt32(sellStr);
                    DrjonesitemList.Add(item);
                    Console.WriteLine("Item Found.... " + item.m_Trader_ItemsClassnames + ", " + item.m_Trader_ItemsQuantity + ", " + item.m_Trader_ItemsSellValue + ", " + item.m_Trader_ItemsBuyValue);
                    itemCounter++;
                }
                catch
                {
                    MessageBox.Show("There is an error in the following line.\n" + line_content.ToString());
                }

                
            }
            reader.Close();
            reader.Dispose();
            fs.Close();
            fs.Dispose();
        }

        public bool saveTraderConfig(string saveTime)
        {
            if (!isDirty) return false;
            StringBuilder sb = new StringBuilder();
            sb.Append("/////////////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine);
            sb.Append("//                                                                                             //" + Environment.NewLine);
            sb.Append("//      Need Help? The Trader Mod has its own Channel on the DayZ Modders Discord Server       //" + Environment.NewLine);
            sb.Append("//             Only Singleline Comments work. Don't use Multiline Comments!                    //" + Environment.NewLine);
            sb.Append("//                                                                                             //" + Environment.NewLine);
            sb.Append("//                        /* THIS COMMENT WILL CRASH THE SERVER! */                            //" + Environment.NewLine);
            sb.Append("//                                  // THIS COMMENT WORKS!                                     //" + Environment.NewLine);
            sb.Append("//                                                                                             //" + Environment.NewLine);
            sb.Append("//                   Config Created by DayZ Loot Manager by Shawminator                        //" + Environment.NewLine);
            sb.Append("//                                                                                             //" + Environment.NewLine);
            sb.Append("/////////////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("<CurrencyName> " + CurrencyConfig.m_Trader_CurrencyName + Environment.NewLine);
            foreach(DRJonesCurrency currency in CurrencyConfig.currencyList)
            {
                sb.Append("\t<Currency> " + currency.m_Trader_CurrencyClassnames + ", " + currency.m_Trader_CurrencyValues.ToString() + Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            string TraderFilename = "";
            foreach(DrjonesFullTraderconfig trader in drjonesfullList)
            {
                if (TraderFilename != trader.m_traderpath && TraderFilename != "")
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("<OpenFile> " + trader.m_traderpath.Replace(path + "\\", "") + " // Links to another File; Must be right above <FileEnd>!" + Environment.NewLine);
                    sb.Append("<FileEnd> // This has to be on the End of this File and is very importand!");
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderFilename) + "\\Backup\\" + saveTime);
                    File.Copy(TraderFilename, Path.GetDirectoryName(TraderFilename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(TraderFilename), true);
                    File.WriteAllText(TraderFilename, sb.ToString());
                    TraderFilename = trader.m_traderpath;
                    sb = new StringBuilder();
                    sb.Append("/////////////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine);
                    sb.Append("//                                                                                             //" + Environment.NewLine);
                    sb.Append("//      Need Help? The Trader Mod has its own Channel on the DayZ Modders Discord Server       //" + Environment.NewLine);
                    sb.Append("//             Only Singleline Comments work. Don't use Multiline Comments!                    //" + Environment.NewLine);
                    sb.Append("//                                                                                             //" + Environment.NewLine);
                    sb.Append("//                        /* THIS COMMENT WILL CRASH THE SERVER! */                            //" + Environment.NewLine);
                    sb.Append("//                                  // THIS COMMENT WORKS!                                     //" + Environment.NewLine);
                    sb.Append("//                                                                                             //" + Environment.NewLine);
                    sb.Append("//           Config Additional file Created by DayZ Loot Manager by Shawminator                //" + Environment.NewLine);
                    sb.Append("//                                                                                             //" + Environment.NewLine);
                    sb.Append("/////////////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("<Trader> " + trader.Tradername + Environment.NewLine);
                    foreach (TraderCats cats in trader.cats)
                    {
                        sb.Append("\t<Category> " + cats.CatName + Environment.NewLine);
                        foreach (TraderItems items in cats.ItemList)
                        {
                            sb.Append("\t\t" + items.m_Trader_ItemsClassnames + ", " + items.m_Trader_ItemsQuantity + ", " + items.m_Trader_ItemsBuyValue.ToString() + ", " + items.m_Trader_ItemsSellValue.ToString() + Environment.NewLine);
                        }
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    TraderFilename = trader.m_traderpath;
                    sb.Append("<Trader> " + trader.Tradername + Environment.NewLine);
                    foreach (TraderCats cats in trader.cats)
                    {
                        sb.Append("\t<Category> " + cats.CatName + Environment.NewLine);
                        foreach (TraderItems items in cats.ItemList)
                        {
                            sb.Append("\t\t" + items.m_Trader_ItemsClassnames + ", " + items.m_Trader_ItemsQuantity + ", " + items.m_Trader_ItemsBuyValue.ToString() + ", " + items.m_Trader_ItemsSellValue.ToString() + Environment.NewLine);
                        }
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            sb.Append(Environment.NewLine);
            sb.Append("<FileEnd> // This has to be on the End of this File and is very importand!");
            Directory.CreateDirectory(Path.GetDirectoryName(TraderFilename) + "\\Backup\\" + saveTime);
            File.Copy(TraderFilename, Path.GetDirectoryName(TraderFilename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(TraderFilename), true);
            File.WriteAllText(TraderFilename, sb.ToString());
            isDirty = false;
            return true;
        }

        public List<DRJonesCategories> getCatsfromTraderID(int id)
        {
            List<DRJonesCategories> newlist = new List<DRJonesCategories>();
            foreach(DRJonesCategories cats in m_categories)
            {
                if (cats.m_Trader_CategorysTraderKey == id)
                    newlist.Add(cats);
            }
            return newlist;
        }
        public List<DrjonesItems> GetItemsFromCatID(int id)
        {
            return DrjonesitemList.Where(x => x.m_Trader_ItemsTraderId == id).ToList(); 
        }
        public void SetupFullTraderList()
        {
            drjonesfullList = new BindingList<DrjonesFullTraderconfig>();
            foreach(DRjonesTraders trader in m_Traders)
            {
                DrjonesFullTraderconfig newtrader = new DrjonesFullTraderconfig();
                newtrader.cats = new BindingList<TraderCats>();
                newtrader.Tradername = trader.m_Trader_TraderNames;
                newtrader.m_traderpath = trader.m_filename;
                List<DRJonesCategories> list = getCatsfromTraderID(trader.traderID);
                List<DrjonesItems> items = GetItemsFromCatID(trader.traderID);
                foreach(DRJonesCategories cat in list)
                {
                    TraderCats newcat = new TraderCats();
                    newcat.CatName = cat.m_Trader_Categorys;
                    newcat.ItemList = new BindingList<TraderItems>();
                    List<DrjonesItems> catitem = items.Where(x => x.m_Trader_ItemsCategoryId == cat.m_Trader_CategoryID).ToList();
                    foreach(DrjonesItems litems in catitem)
                    {
                        TraderItems newitem = new TraderItems();
                        newitem.m_Trader_ItemsClassnames = litems.m_Trader_ItemsClassnames;
                        newitem.m_Trader_ItemsQuantity = litems.m_Trader_ItemsQuantity;
                        newitem.m_Trader_ItemsSellValue = litems.m_Trader_ItemsSellValue;
                        newitem.m_Trader_ItemsBuyValue = litems.m_Trader_ItemsBuyValue;
                        newcat.ItemList.Add(newitem);
                    }
                    newtrader.cats.Add(newcat);
                }
                drjonesfullList.Add(newtrader);
            }
        }
        public bool removetrader(string name)
        {
            DrjonesFullTraderconfig removetrader = drjonesfullList.First(x => x.Tradername == name);
            if (removetrader != null)
            {
                drjonesfullList.Remove(removetrader);
                return true;
            }
            return false;
        }
        public void AddNewTrader(string userAnswer)
        {
            DrjonesFullTraderconfig newTrader = new DrjonesFullTraderconfig();
            newTrader.Tradername = userAnswer;
            newTrader.cats = new BindingList<TraderCats>();
            drjonesfullList.Add(newTrader);
        }
        public void removecurrency(string name)
        {
            DRJonesCurrency removeitem = CurrencyConfig.currencyList.First(x => x.m_Trader_CurrencyClassnames == name);
            if (removeitem != null)
            {
                CurrencyConfig.currencyList.Remove(removeitem);
                Console.WriteLine(removeitem + " removed from currency List....");
            }
        }
        public void addnewCurrency(string userAnswer)
        {
            DRJonesCurrency newcurrency = new DRJonesCurrency();
            newcurrency.m_Trader_CurrencyClassnames = userAnswer;
            newcurrency.m_Trader_CurrencyValues = 0;
            if (!CurrencyConfig.currencyList.Any(x => x.m_Trader_CurrencyClassnames == userAnswer))
            {
                CurrencyConfig.currencyList.Add(newcurrency);
                Console.WriteLine(userAnswer + " has been added to currency list....");
            }
        }
        public List<DrjonesItems> getItems(int id)
        {
            return DrjonesitemList.Where(x => x.m_Trader_ItemsCategoryId == id).ToList();
        }
    }
    public class DrjonesFullTraderconfig
    {
        public string Tradername { get; set; }
        public string m_traderpath { get; set; }
        public BindingList<TraderCats> cats {get;set;}
        public override string ToString()
        {
            return Tradername;
        }
        public void AddNewCat(string userAnswer)
        {
            TraderCats newcat = new TraderCats();
            newcat.CatName = userAnswer;
            newcat.ItemList = new BindingList<TraderItems>();
            if(!cats.Any(x => x.CatName == userAnswer))
                cats.Add(newcat);
        }
        public void removecatfromtrader(TraderCats currentCat)
        {
            if (cats.Any(x => x.CatName == currentCat.CatName))
                cats.Remove(currentCat);
        }
    }
    public class TraderCats
    {
        public string CatName { get; set; }
        public BindingList<TraderItems> ItemList { get; set; }
        public override string ToString()
        {
            return CatName;
        }
    }
    public class TraderItems
    {
        public string m_Trader_ItemsClassnames { get; set; }
        public string m_Trader_ItemsQuantity { get; set; }
        public int m_Trader_ItemsBuyValue { get; set; }
        public int m_Trader_ItemsSellValue { get; set; }
        public override string ToString()
        {
            return m_Trader_ItemsClassnames;
        }
    }

public class DRjonesTraders
    {
        public string m_filename { get; set; }
        public string m_Trader_TraderNames { get; set; }
        public int traderID { get; set; }
        public override string ToString()
        {
            return m_Trader_TraderNames;
        }
    }
    public class DRJonesCategories
    {
        public string m_Trader_Categorys { get; set; }
        public int m_Trader_CategoryID { get; set; }
        public int m_Trader_CategorysTraderKey { get; set; }
        public override string ToString()
        {
            return m_Trader_Categorys;
        }

    }
    public class DrjonesItems
    {
        public int m_Trader_ItemsTraderId { get; set; }
        public int m_Trader_ItemsCategoryId { get; set; }
        public string m_Trader_ItemsClassnames { get; set; }
        public string m_Trader_ItemsQuantity { get; set; }
        public int m_Trader_ItemsBuyValue { get; set; }
        public int m_Trader_ItemsSellValue { get; set; }

        public override string ToString()
        {
            return m_Trader_ItemsClassnames;
        }
    }
    public class DRJonesTraderCurrency
    {
        public string m_Trader_CurrencyName;
        public BindingList<DRJonesCurrency> currencyList;
        //Currecny layout as follows:
        //<CurrencyName> #tm_ruble              Currency Name (Only used for the Text in the upper right Corner of the Trader Menu)
        //  <Currency> MoneyRuble1,     1       Currency Classname, Currency Money Value
        //  <Currenct> MoneyRuble5,     5
        // etc....

    }
    public class DRJonesCurrency
    {
        public string m_Trader_CurrencyClassnames;                    // Currency Classname, 
        public int m_Trader_CurrencyValues;                           // Currency Money Value
        public override string ToString()
        {
            return m_Trader_CurrencyClassnames;
        }
    }
}
