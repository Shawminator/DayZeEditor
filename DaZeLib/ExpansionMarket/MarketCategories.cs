using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class MarketCategories
    {
        public int CurrentVersion = 9;
        public BindingList<Categories> CatList { get; set; }
        public string MarketCatsPath {get;set;}

        public MarketCategories()
        {

            CatList = new BindingList<Categories>();
        }
        public MarketCategories(string Path, bool createfolder = true)
        {
            MarketCatsPath = Path;
            CatList = new BindingList<Categories>();
            if (createfolder)
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting MarketCategories");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    bool savefile = false;
                    Console.WriteLine("serializing " + file.Name);
                    Categories cat = JsonSerializer.Deserialize<Categories>(File.ReadAllText(file.FullName));
                    if(cat.Icon == null)
                    {
                        cat.Icon = "Deliver";
                        savefile = true;
                    }
                    if(cat.Color == null)
                    {
                        cat.Color = "FBFCFEFF";
                        savefile = true;
                    }
                    if (cat.m_Version != CurrentVersion)
                    {
                        cat.m_Version = CurrentVersion;
                        savefile = true;
                    }
                    foreach (marketItem item in cat.Items)
                    {
                        if(item.ClassName != item.ClassName.ToLower())
                        {
                            item.ClassName = item.ClassName.ToLower();
                            savefile = true;
                        }
                        for (int i = 0; i < item.Variants.Count; i++)
                        {
                            if (item.Variants[i] != item.Variants[i].ToLower())
                            {
                                item.Variants[i] = item.Variants[i].ToLower();
                                savefile = true;
                            }
                        }
                        for (int j = 0; j < item.SpawnAttachments.Count; j++)
                        {
                            if (item.SpawnAttachments[j] != item.SpawnAttachments[j].ToLower())
                            {
                                item.SpawnAttachments[j] = item.SpawnAttachments[j].ToLower();
                                savefile = true;
                            }
                        }
                        if (item.MaxPriceThreshold < item.MinPriceThreshold)
                            MessageBox.Show(cat.DisplayName + Environment.NewLine + item.ClassName + " Has a max price lower than the min price." + Environment.NewLine + "Please fix......");
                        if (item.MaxStockThreshold < item.MinStockThreshold)
                            MessageBox.Show(cat.DisplayName + Environment.NewLine + item.ClassName + " Has a max stock lower than the min Stock." + Environment.NewLine + "Please fix......");
                    }
                    cat.Filename = file.FullName;
                    cat.Items = new BindingList<marketItem>(new BindingList<marketItem>(cat.Items.OrderBy(x => x.ClassName).ToList()));
                    CatList.Add(cat);
                    if (savefile)
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(cat, options);
                        File.WriteAllText(cat.Filename, jsonString);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message); 

                }

            }
           
        }
        public marketItem getitemfromcategory(string Itemname)
        {
            marketItem item = null;
            foreach (Categories cat in CatList)
            {
                item = cat.getitem(Itemname);
                if (item != null)
                    return item;
            }
            return item;
        }
        public Categories GetCatFromDisplayName(String name)
        {
            return CatList.FirstOrDefault(x => x.DisplayName.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "") == name);
        }
        public Categories GetCat(marketItem item)
        {
            if(item == null) { 
                return null; }
            foreach (Categories cat in CatList)
            {
                if (cat.Items.Any(x => x.ClassName == item.ClassName))
                    return cat;
            }
            return null;
        }
        public void RemoveCat(Categories catfordelete)
        {
            catfordelete.backupandDelete();
            CatList.Remove(catfordelete);
            
        }
        public void CreateNewCat(string catName)
        {
            Categories NewCat = new Categories(catName.ToUpper().Replace("_", " "));
            NewCat.isDirty = true;
            NewCat.Filename = MarketCatsPath + "\\" + catName + ".json";
            if (CatList.Any(x => x.DisplayName == catName))
            {
                MessageBox.Show(catName = " Allready in list of catogories....");
                return;
            }
            else
            {
                CatList.Add(NewCat);
                MessageBox.Show(catName = " added to list of  catogories....");
            }
        }
        public List<TraderListItem> getallCats()
        {
            List<TraderListItem> returnlist = new List<TraderListItem>();
            foreach(Categories cat in CatList)
            {
                TraderListItem tli = new TraderListItem();
                tli.Classname = cat.DisplayName;
                returnlist.Add(tli);
               
            }
            return returnlist;
        }
        public List<marketItem> searchforitems(string searchterm, bool extact = false)
        {
            List<marketItem> items = new List<marketItem>();
            foreach(Categories cat in CatList)
            {
                if(extact)
                    items.AddRange(cat.Items.Where(x => x.ClassName.Equals(searchterm)));
                else
                    items.AddRange(cat.Items.Where(x => x.ClassName.Contains(searchterm)));
            }
            return items;
        }
        public void removeitemfromcat(marketItem item)
        {
            foreach (Categories cat in CatList)
            {
                if (cat.Items.Any(x => x.ClassName == item.ClassName))
                {
                    marketItem _item = cat.Items.First(x => x.ClassName == item.ClassName);
                    cat.Items.Remove(_item);
                    cat.isDirty = true;
                }
            }
        }
        public Categories GetCatFromFileName(string cat)
        {
            return CatList.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Filename.ToLower()) == cat.ToLower());
        }
        public bool isincludedinavarient(string item)
        {
            foreach (Categories cat in CatList)
            {
                foreach(marketItem mitem in cat.Items)
                {
                    if(mitem.Variants.Count > 0)
                    {
                        if (mitem.Variants.Contains(item))
                            return true;
                    }
                }
            }
            return false;
        }
        public void AddToCat(marketItem v, string displayName)
        {
            Categories cat = GetCatFromDisplayName(displayName);
            cat.Items.Add(v);
            cat.isDirty = true;
        }
        public string GetCatNameFromItemName(string className)
        {
            foreach (Categories cat in CatList)
            {
                if (cat.Items.Any(x => x.ClassName == className.ToLower()))
                    return Path.GetFileNameWithoutExtension(cat.Filename);
                if(cat.Items.Any(x => x.Variants.Contains(className.ToLower())))
                    return Path.GetFileNameWithoutExtension(cat.Filename);
            }

            return null;
        }
    }
    public class Categories
    {
        public int m_Version { get; set; } //current version 9
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public float InitStockPercent { get; set; }
        public BindingList<marketItem> Items { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;


        public Categories(string filename)
        {
            m_Version = 9;
            DisplayName = filename;
            Icon = "deliver";
            Color = "FBFCFEFF";
            InitStockPercent = 75.0f;
            Items = new BindingList<marketItem>();
        }
        public Categories()
        {
            m_Version = 9;
            DisplayName = "";
            Icon = "deliver";
            Color = "FBFCFEFF";
            InitStockPercent = 75.0f;
            Items = new BindingList<marketItem>();
        }
        public override string ToString()
        {
            return DisplayName.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "");
        }
        public marketItem getitem(string name)
        {
            if (Items.Any(x => x.ClassName.ToLower() == name.ToLower()))
            {
                return Items.First(x => x.ClassName.ToLower() == name.ToLower());
            }
            else if (Items.Any(x => x.Variants.Contains(name.ToLower())))
            {
                return Items.First(x => x.Variants.Contains(name.ToLower()));
            }
            else
                return null;
        }
        public List<string> getallItemsasString()
        {
            List<string> items = new List<string>();
            foreach(marketItem mi in Items)
            {
                items.Add(mi.ClassName);
            }
            return items;
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
    public class marketItem
    {
        public string ClassName { get; set; }
        public int MaxPriceThreshold { get; set; }
        public int MinPriceThreshold { get; set; }
        public int SellPricePercent { get; set; } //Added in Version 7
        public int MaxStockThreshold { get; set; }
        public int MinStockThreshold { get; set; }
        //public int PurchaseType { get; set; }  removed in Version 6
        public int QuantityPercent { get; set; }
        public BindingList<string> SpawnAttachments { get; set; }
        public BindingList<string> Variants { get; set; }

        public marketItem()
        {
            SpawnAttachments = new BindingList<string>();
            Variants = new BindingList<string>();
            SellPricePercent = -1;
            QuantityPercent = -1;
        }
        public override string ToString()
        {
            return ClassName;
        }
    }
}
