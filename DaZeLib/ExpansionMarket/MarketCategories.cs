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
    public class MarketCategories
    {
        public BindingList<Categories> CatList { get; set; }
        public string MarketCatsPath {get;set;}

        public MarketCategories()
        {

            CatList = new BindingList<Categories>();
        }
        public MarketCategories(string Path)
        {
            MarketCatsPath = Path;
            CatList = new BindingList<Categories>();
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting Trader Zones....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("serializing " + file.Name);
                    Categories cat = JsonSerializer.Deserialize<Categories>(File.ReadAllText(file.FullName));
                    foreach (marketItem item in cat.Items)
                    {
                        if (item.ClassName.Any(char.IsUpper))
                        {
                            item.ClassName = item.ClassName.ToLower();
                            cat.isDirty = true;
                        }
                        for (int i = 0; i < item.Variants.Count; i++)
                        {
                            if (item.Variants[i].Any(char.IsUpper))
                            {
                                item.Variants[i] = item.Variants[i].ToLower();
                                cat.isDirty = true;
                            }
                        }
                        for (int i = 0; i < item.SpawnAttachments.Count; i++)
                        {
                            if (item.SpawnAttachments[i].Any(char.IsUpper))
                            {
                                item.SpawnAttachments[i] = item.SpawnAttachments[i].ToLower();
                                cat.isDirty = true;
                            }
                        }
                    }
                    cat.Filename = file.FullName;
                    cat.Items = new BindingList<marketItem>(new BindingList<marketItem>(cat.Items.OrderBy(x => x.ClassName).ToList()));
                    CatList.Add(cat);
                }
                catch
                {
                    MessageBox.Show("there is an error in the following file\n" + file.FullName); 

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
            return CatList.First(x => Path.GetFileNameWithoutExtension(x.Filename) == cat);
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
                if (cat.Items.Any(x => x.ClassName == className))
                    return Path.GetFileNameWithoutExtension(cat.Filename);
                if(cat.Items.Any(x => x.Variants.Contains(className)))
                    return Path.GetFileNameWithoutExtension(cat.Filename);
            }

            return null;
        }
    }
    public class Categories
    {
        public int m_Version { get; set; } //current version 4
        public string DisplayName { get; set; }
        public BindingList<marketItem> Items { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;


        public Categories(string filename)
        {
            m_Version = 4;
            DisplayName = filename;
            Items = new BindingList<marketItem>();
        }
        public Categories()
        {
            m_Version = 4;
            Items = new BindingList<marketItem>();
        }
        public override string ToString()
        {
            return DisplayName.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "");
        }
        public marketItem getitem(string name)
        {
            if (Items.Any(x => x.ClassName.ToLower() == name))
                return Items.First(x => x.ClassName.ToLower() == name);
            else if (Items.Any(x =>x.Variants.Contains(name)))
            {
                return Items.First(x => x.Variants.Contains(name));
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
        public int MaxStockThreshold { get; set; }
        public int MinStockThreshold { get; set; }
        public int PurchaseType { get; set; } 
        public BindingList<string> SpawnAttachments { get; set; }
        public BindingList<string> Variants { get; set; }

        public marketItem()
        {
            SpawnAttachments = new BindingList<string>();
            Variants = new BindingList<string>();
        }
        public override string ToString()
        {
            return ClassName;
        }
    }
}
