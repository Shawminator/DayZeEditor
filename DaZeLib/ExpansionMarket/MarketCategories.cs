﻿using System;
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

    public class MarketCategories
    {
        public const int CurrentVersion = 12;
        public BindingList<Categories> CatList { get; set; }
        public bool SortedbyDisplayName { get; private set; }
        public string MarketCatsPath { get; set; }
        public List<Categories> Markedfordelete { get; set; }

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
            FileInfo[] Files = dinfo.GetFiles("*.json", SearchOption.AllDirectories);
            Console.WriteLine("Getting MarketCategories");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    bool savefile = false;
                    Console.WriteLine("serializing " + file.Name);
                    Categories cat = JsonSerializer.Deserialize<Categories>(File.ReadAllText(file.FullName));
                    if (cat.Icon == null)
                    {
                        cat.Icon = "Deliver";
                        savefile = true;
                    }
                    if (cat.Color == null)
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
                        if (item.ClassName != item.ClassName.ToLower())
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
                    }
                   
                    cat.Filename = System.IO.Path.GetFileNameWithoutExtension(file.FullName);

                    string folder = System.IO.Path.GetDirectoryName(file.FullName).Replace(Path, "");
                    if(folder != "")
                    {
                        folder = folder.Substring(1);
                    }
                    cat.Folder = folder;
                    cat.Items = new BindingList<marketItem>(new BindingList<marketItem>(cat.Items.OrderBy(x => x.ClassName).ToList()));
                    CatList.Add(cat);
                    if (savefile)
                    {
                        File.Delete(file.FullName);
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(cat, options);
                        File.WriteAllText(MarketCatsPath + "\\" + cat.Filename + ".json", jsonString);
                    }
                }
                catch (Exception ex)
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
            if (item == null)
            {
                return null;
            }
            foreach (Categories cat in CatList)
            {
                if (cat.Items.Any(x => x.ClassName == item.ClassName))
                    return cat;
            }
            return null;
        }
        public void RemoveCat(Categories catfordelete)
        {
            if (Markedfordelete == null) Markedfordelete = new List<Categories>();
            Markedfordelete.Add(catfordelete);
            CatList.Remove(catfordelete);

        }
        public void CreateNewCat(string catName)
        {
            string foldername = "";
            string filename = catName;
            if(catName.Contains('\\'))
            {
                string[] strings = catName.Split('\\');
                for (int i = 0; i < strings.Length -1; i++)
                {
                    foldername += strings[i];
                    if(i < strings.Length - 2)
                    {
                        foldername += "\\";
                    }
                }
                filename = strings.Last();
            }
            Categories NewCat = new Categories(filename.Replace("_", " "));
            NewCat.Filename = filename.Replace(" ", "_");
            NewCat.Folder = foldername;
            NewCat.SortByDisplayName = SortedbyDisplayName;
            if (CatList.Any(x => x.Filename == NewCat.Filename))
            {
                MessageBox.Show(NewCat.Filename + " Allready in list of catogories....");
                return;
            }
            else
            {
                CatList.Add(NewCat);
                NewCat.isDirty = true;
                MessageBox.Show(NewCat.Filename + " added to list of  catogories....");
            }
        }
        public List<TraderListItem> getallCats()
        {
            List<TraderListItem> returnlist = new List<TraderListItem>();
            foreach (Categories cat in CatList)
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
            foreach (Categories cat in CatList)
            {
                if (extact)
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
            return CatList.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Filename) == cat);
        }
        public List<Categories> GetexchangeCats()
        {
            return CatList.Where(x => x.IsExchange == 1).ToList();
        }
        public bool isincludedinavarient(string item)
        {
            foreach (Categories cat in CatList)
            {
                foreach (marketItem mitem in cat.Items)
                {
                    if (mitem.Variants.Count > 0)
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
                if (cat.Items.Any(x => x.Variants.Contains(className.ToLower())))
                    return Path.GetFileNameWithoutExtension(cat.Filename);
            }

            return null;
        }

        public void SortbyDisplayName()
        {
            var sortedListInstance = new BindingList<Categories>(CatList.OrderBy(x => x.Folder).ThenBy(y => y.DisplayName).ToList());
            foreach (Categories t in sortedListInstance)
            {
                t.SortByDisplayName = true;
            }
            CatList = sortedListInstance;
            SortedbyDisplayName = false;
        }
        public void Sortbyfilename()
        {
            var sortedListInstance = new BindingList<Categories>(CatList.OrderBy(x => x.Folder).ThenBy(y => y.Filename).ToList());
            foreach (Categories t in sortedListInstance)
            {
                t.SortByDisplayName = false;
            }
            CatList = sortedListInstance;
            SortedbyDisplayName = false;
        }
        public void SortItemlistbyitemName(bool AtoZ)
        {
            foreach (Categories t in CatList)
            {
                t.SortbyitemName(AtoZ);
            }
        }
        public void SortItemlistbyitemPrice(bool MintoMax)
        {
            foreach (Categories t in CatList)
            {
                t.SortByprice(MintoMax);
            }
        }

        public void RemoveMarkedCats()
        {
            throw new NotImplementedException();
        }
    }
    public class Categories
    {
        public int m_Version { get; set; } //current version 9
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public decimal InitStockPercent { get; set; }
        public int IsExchange { get; set; }
        public BindingList<marketItem> Items { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public string Folder { get; set; }
        [JsonIgnore]
        public bool SortByDisplayName { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

        public Categories(string filename)
        {
            m_Version = MarketCategories.CurrentVersion;
            DisplayName = filename;
            Icon = "Deliver";
            Color = "FBFCFEFF";
            InitStockPercent = (decimal)75.0;
            Items = new BindingList<marketItem>();
        }
        public Categories()
        {
            m_Version = MarketCategories.CurrentVersion;
            DisplayName = "";
            Icon = "Deliver";
            Color = "FBFCFEFF";
            InitStockPercent = (decimal)75.0;
            Items = new BindingList<marketItem>();
        }
        public override string ToString()
        {
            if (SortByDisplayName)
                return DisplayName.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "");
            else
                return Filename;
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
            foreach (marketItem mi in Items)
            {
                items.Add(mi.ClassName);
            }
            return items;
        }
        public void backupandDelete(string marketpath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string Fullfilename = marketpath + "\\";
            if(Folder != "")
            {
                Fullfilename += Folder + "\\";
            }
            Fullfilename += Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                if (!Directory.Exists(marketpath + "\\Backup\\" + SaveTime))
                    Directory.CreateDirectory(marketpath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, marketpath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }

        public void SortbyitemName(bool atoZ)
        {
            if (atoZ)
            {
                var sortedListInstance = new BindingList<marketItem>(Items.OrderBy(x => x.ClassName).ToList());
                Items = sortedListInstance;
            }
            else if (!atoZ)
            {
                var sortedListInstance = new BindingList<marketItem>(Items.OrderByDescending(x => x.ClassName).ToList());
                Items = sortedListInstance;
            }

        }

        public void SortByprice(bool mintoMax)
        {
            if (mintoMax)
            {
                var sortedListInstance = new BindingList<marketItem>(Items.OrderBy(x => x.MaxPriceThreshold).ThenBy(x => x.ClassName).ToList());
                Items = sortedListInstance;
            }
            else if (!mintoMax)
            {
                var sortedListInstance = new BindingList<marketItem>(Items.OrderByDescending(x => x.MaxPriceThreshold).ThenBy(x => x.ClassName).ToList());
                Items = sortedListInstance;
            }
        }

        internal string getfullname()
        {
            string name = "";
            if(Folder != "")
            {
                name += Folder + "\\";
            }
            name += Filename;

            return name;
        }
    }
    public class marketItem
    {
        public string ClassName { get; set; }
        public int MaxPriceThreshold { get; set; }
        public int MinPriceThreshold { get; set; }
        public decimal SellPricePercent { get; set; }
        public int MaxStockThreshold { get; set; }
        public int MinStockThreshold { get; set; }
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
