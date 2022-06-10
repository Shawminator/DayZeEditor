using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class TraderPlusPriceConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusPriceConfig.json";

        public int EnableAutoCalculation { get; set; }
        public int EnableAutoDestockAtRestart { get; set; }
        public int EnableDefaultTraderStock { get; set; }
        public BindingList<Tradercategory> TraderCategories { get; set; }

        [JsonIgnore]
        public bool _isDirty;
        [JsonIgnore]
        public bool isDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        [JsonIgnore]
        public string FullFilename { get; set; }

        public TraderPlusPriceConfig()
        {
            TraderCategories = new BindingList<Tradercategory>();
        }

        public void SetProducts()
        {
            foreach (Tradercategory tc in TraderCategories)
            {
                tc.setproducts();
            }
        }
        public void getproducts()
        {
            foreach (Tradercategory tc in TraderCategories)
            {
                tc.getproducts();
            }
        }
        public void Sortcategories()
        {
            List<Tradercategory> CatList = new List<Tradercategory>(TraderCategories);
            var sortedListInstance = new BindingList<Tradercategory>(CatList.OrderBy(x => x.CategoryName).ToList());
            TraderCategories = sortedListInstance;
        }
        public List<Tradercategory> getallCats()
        {
            List<Tradercategory> newlist = new List<Tradercategory>();
            foreach (Tradercategory tcat in TraderCategories)
            {
                newlist.Add(tcat);
            }
            return newlist;
        }
    }

    public class Tradercategory
    {
        public string CategoryName { get; set; }
        public BindingList<string> Products { get; set; }

        [JsonIgnore]
        public BindingList<ItemProducts> itemProducts { get; set; }

        public Tradercategory()
        {
            Products = new BindingList<string>();
        }
        public override string ToString()
        {
            return CategoryName;
        }

        internal void setproducts()
        {
            Products = new BindingList<string>();
            foreach (ItemProducts item in itemProducts)
            {
                string product = item.Classname + ",";
                product += ((float)item.Coefficient / 100).ToString() + ",";
                product += item.MaxStock.ToString() + ",";

                if (item.MaxStock > 0 && item.MaxStock < 1)
                    product += item.MaxStock.ToString("F2") + ",";
                else
                    product += item.MaxStock.ToString("F0") + ",";

                product += item.TradeQuantity.ToString() + ",";
                product += item.BuyPrice.ToString() + ",";
                if(item.Sellprice > 0 && item.Sellprice < 1)
                    product += item.Sellprice.ToString("F2");
                else
                    product += item.Sellprice.ToString("F0");
                if (item.UseDestockCoeff)
                    product += "," + ((float)item.destockCoefficent / 100).ToString();
                Products.Add(product);
            }

        }
        internal void getproducts()
        {
            itemProducts = new BindingList<ItemProducts>();
            foreach (string item in Products)
            {
                try
                {
                    string[] itemsplit = item.Split(',');
                    ItemProducts itemProduct = new ItemProducts();
                    itemProduct.Classname = itemsplit[0];
                    itemProduct.Coefficient = (int)(Convert.ToSingle(itemsplit[1]) * 100);
                    if (itemProduct.Coefficient == -100)
                        itemProduct.Coefficient = 100;
                    itemProduct.MaxStock = Convert.ToInt32(itemsplit[2]);
                    itemProduct.TradeQuantity = Convert.ToDecimal(itemsplit[3]);
                    itemProduct.BuyPrice = Convert.ToInt32(itemsplit[4]);
                    itemProduct.Sellprice = Convert.ToSingle(itemsplit[5]);
                    if (itemsplit.Length >= 7)
                    {
                        float f;
                        if (float.TryParse(itemsplit[6], out f))
                        {
                            itemProduct.destockCoefficent = (int)(f * 100);
                            itemProduct.UseDestockCoeff = true;
                        }
                    }
                    itemProducts.Add(itemProduct);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(CategoryName + ex.Message + "\n" + item + "\nAll Values will be set to defualt values,\n Please fix asap");
                    MessageBox.Show(ex.Message + "\n" + item + "\nAll Values will be set to defualt values,\n Please fix asap");
                    string[] itemsplit = item.Split(',');
                    ItemProducts itemProduct = new ItemProducts()
                    {
                        Classname = itemsplit[0],
                        Coefficient = 100,
                        MaxStock = -1,
                        TradeQuantity = -1,
                        BuyPrice = 2,
                        Sellprice = 1,
                        UseDestockCoeff = false,
                        destockCoefficent = 0
                    };
                    itemProducts.Add(itemProduct);

                }

            }
            List<ItemProducts> CatList = new List<ItemProducts>(itemProducts);
            var sortedListInstance = new BindingList<ItemProducts>(CatList.OrderBy(x => x.Classname).ToList());
            itemProducts = sortedListInstance;
        }

        public void removeItemProduct(ItemProducts item)
        {
            itemProducts.Remove(item);
        }

        public void AdditemProduct(ItemProducts item)
        {
            itemProducts.Add(item);
        }
    }
    public class ItemProducts
    {
        public string Classname { get; set; }
        public int Coefficient { get; set; }
        public int MaxStock { get; set; }
        public decimal TradeQuantity { get; set; }
        public int BuyPrice { get; set; }
        public float Sellprice { get; set; }
        public int destockCoefficent { get; set; }
        public bool UseDestockCoeff { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }
}
