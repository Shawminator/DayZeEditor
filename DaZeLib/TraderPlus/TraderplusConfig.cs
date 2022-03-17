using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.IO;

namespace DayZeLib
{
    public enum EnableDefaultTraderStock
    {
        [Description("no auto filling during a server restart.")]
        No_Auto_Filling = 0,
        [Description("each product will be added to the stock based on the max stock value.")]
        Max_Stock_Filling = 1,
        [Description("each product will be added to the stock based on a random value between 0 and max stock value.")]
        Random_Filling_between_Min_Max = 2,
    }
    /// <summary>
    /// TraderPlusBankingConfig.json
    /// </summary>
    public class TraderPlusBankingConfig
    {
        public int IsCreditCarNeededForTransaction { get; set; }
        public float TransactionFees { get; set; }
        public int DefaultStartCurrency { get; set; }
        public int DefaultMaxCurrency { get; set; }
        public string TextUI { get; set; }
        public BindingList<string> CurrenciesAccepted { get; set; }

        [JsonIgnore]
        public const string fileName = "TraderPlusBankingConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public TraderPlusBankingConfig()
        {
            CurrenciesAccepted = new BindingList<string>();
        }
        public int getIntValue(string mytype)
        {
            return (int)GetType().GetProperty(mytype).GetValue(this);
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }

    }
    /// <summary>
    /// TraderPlusGarageConfig.json
    /// </summary>
    public class TraderPlusGarageConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusGarageConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int VehicleMustHaveLock { get; set; }
        public int SaveVehicleCargo { get; set; }
        public int SaveVehicleHealth { get; set; }
        public int SaveVehicleFuel { get; set; }
        public int ParkInCost { get; set; }
        public int ParkOutCost { get; set; }
        public int PayWithBankAccount { get; set; }
        public BindingList<string> WhitelistedObjects { get; set; }
        public BindingList<Npc> NPCs { get; set; }
        public string ParkingNotAvailable { get; set; }
        public string NotEnoughMoney { get; set; }
        public string NotRightToPark { get; set; }
        public string CarHasMember { get; set; }
        public string ParkInFail { get; set; }
        public string ParkInSuccess { get; set; }
        public string ParkOutFail { get; set; }
        public string ParkOutSuccess { get; set; }

    }
    public class Npc
    {
        public string ClassName { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        public float[] ParkingPosition { get; set; }
        public float[] ParkingOrientation { get; set; }
        public BindingList<string> Clothes { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }
    /// <summary>
    /// TraderPlusGeneralConfig.json
    /// </summary>
    public class TraderPlusGeneralConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusGeneralConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        [JsonIgnore]
        private bool _isDirty;

        public int ConvertTraderConfigToTraderPlus { get; set; }
        public int ConvertTraderConfigToTraderPlusWithStockBasedOnCE { get; set; }
        public int UseGarageToTradeCar { get; set; }
        public int DisableHeightFailSafeForReceiptDeployment { get; set; }
        public int EnableShowAllPrices { get; set; }
        public int EnableShowAllCheckBox { get; set; }
        public int EnableStockAllCategory { get; set; }
        public int IsReceiptTraderOnly { get; set; }
        public float LockPickChance { get; set; }
        public string LicenceKeyWord { get; set; }
        public BindingList<string> Licences { get; set; }
        public Acceptedstates AcceptedStates { get; set; }
        public int StoreOnlyToPristineState { get; set; }
        public BindingList<Currency> Currencies { get; set; }
        public BindingList<Trader> Traders { get; set; }
        public BindingList<Traderobject> TraderObjects { get; set; }

        public void SortTradersByIndex()
        {
            List<Trader> tradlist = new List<Trader>(Traders);
            var sortedListInstance = new BindingList<Trader>(tradlist.OrderBy(x => x.Id).ToList());
            Traders = sortedListInstance;
        }
        int tempindex;
        public void UpdateIndexes()
        {
            tempindex = 0;
            foreach (Trader t in Traders)
            {
                if (t.Id == -2)
                    t.isBanker = true;
                else
                {
                    t.Id = tempindex;
                    tempindex++;
                }
            }
        }
        public void getBankers()
        {
            foreach(Trader t in Traders)
            {
                if (t.Id == -2)
                    t.isBanker = true;
            }
        }
        public int getnextavialableID()
        {
            int tempindex = 0;
            foreach (Trader t in Traders)
            {
                if (t.Id == -2){}
                else
                {
                    tempindex++;
                }
            }
            return tempindex;
        }
        public void getallcurenciesclassnames()
        {
            foreach(Currency c in Currencies)
            {
                List<string> newlist = c.ClassName.Split(',').ToList();
                newlist.Sort();
                c.CurrenciesNames = new BindingList<string>();
                foreach(string s in newlist)
                {
                    c.CurrenciesNames.Add(s);
                }
            }
        }
        public void SortCurriences()
        {
            List<Currency> Currencylist = new List<Currency>(Currencies);
            var sortedListInstance = new BindingList<Currency>(Currencies.OrderByDescending(x => x.Value).ToList());
            Currencies = sortedListInstance;
        }
        public List<string> getlicenses()
        {
            List<string> newlist = new List<string>();
            foreach (string L in Licences)
            {
                newlist.Add(L);
            }
            return newlist;
        }
        public List<string> getcurrencies()
        {
            List<string> newlist = new List<string>();
            foreach(Currency c in Currencies)
            {
                foreach(string currencyname in c.CurrenciesNames)
                {
                    newlist.Add(currencyname);
                }
            }
            return newlist.OrderBy(q => q).ToList();
        }

        public void SaveCurrencies()
        {
            foreach(Currency c in Currencies)
            {
                c.ClassName = string.Join(",", c.CurrenciesNames);
            }
        }
    }

    public class Acceptedstates
    {
        public int AcceptWorn { get; set; }
        public int AcceptDamaged { get; set; }
        public int AcceptBadlyDamaged { get; set; }
    }

    public class Currency
    {
        public string ClassName { get; set; }
        public int Value { get; set; }
        
        [JsonIgnore]
        public BindingList<string> CurrenciesNames { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Trader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Role { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        public BindingList<string> Clothes { get; set; }


        [JsonIgnore]
        public bool isBanker { get; set; }

        public override string ToString()
        {
            return Role;
        }
    }

    public class Traderobject
    {
        public string ObjectName { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }

        public override string ToString()
        {
            return ObjectName;
        }
    }
    /// <summary>
    /// TraderPlusIDsConfig.json
    /// </summary>
    public class TraderPlusIDsConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusIDsConfig.json";
        [JsonIgnore]
        public bool isDirty;

        public BindingList<Id> IDs { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }

        public void setupIndex()
        {
            for(int i = 0; i < IDs.Count; i++)
            {
                IDs[i].Index = i;
            }
        }
        public Id getTraderbyID(int id)
        {
            return IDs.Where(x => x.Index == id).First();
        }
    }

    public class Id
    {
        public int EnableStockAllCategoryForID { get; set; }
        public BindingList<string> Categories { get; set; }
        public BindingList<string> LicencesRequired { get; set; }
        public BindingList<string> CurrenciesAccepted { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

    }

    /// <summary>
    /// TraderPlusPriceConfig.json
    /// </summary>
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
            foreach(Tradercategory tcat in TraderCategories)
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
                string product =  item.Classname + "," + 
                                  ((float)item.Coefficient / 100).ToString() + "," + 
                                  item.MaxStock.ToString() + "," +
                                  item.TradeQuantity.ToString() + "," +
                                  item.BuyPrice.ToString() + "," +
                                  item.Sellprice.ToString();
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
                string[] itemsplit = item.Split(',');
                ItemProducts itemProduct = new ItemProducts();
                itemProduct.Classname = itemsplit[0];
                itemProduct.Coefficient = (int)(Convert.ToSingle(itemsplit[1])*100);
                itemProduct.MaxStock = Convert.ToInt32(itemsplit[2]);
                itemProduct.TradeQuantity = Convert.ToSingle(itemsplit[3]);
                itemProduct.BuyPrice = Convert.ToInt32(itemsplit[4]);
                itemProduct.Sellprice = Convert.ToInt32(itemsplit[5]);
                if (itemsplit.Length == 7)
                {
                    itemProduct.destockCoefficent = (int)(Convert.ToSingle(itemsplit[6])*100);
                    itemProduct.UseDestockCoeff = true;
                }
                itemProducts.Add(itemProduct);
            }
            List<ItemProducts> CatList = new List<ItemProducts>(itemProducts);
            var sortedListInstance = new BindingList<ItemProducts>(CatList.OrderBy(x => x.Classname).ToList());
            itemProducts = sortedListInstance;
        }

    }
    public class ItemProducts
    {
        public string Classname { get; set; }
        public int Coefficient { get; set; }
        public int MaxStock { get; set; }
        public float TradeQuantity { get; set; }
        public int BuyPrice { get; set; }
        public int Sellprice { get; set; }
        public int destockCoefficent { get; set; }
        public bool UseDestockCoeff { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }
    /// <summary>
    /// TraderPlusSafeZoneConfig.json
    /// </summary>
    public class TraderPlusSafeZoneConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusSafeZoneConfig.json";
        

        public int EnableNameTag { get; set; }
        public int EnableAfkDisconnect { get; set; }
        public int KickAfterDelay { get; set; }
        public string MsgEnterZone { get; set; }
        public string MsgExitZone { get; set; }
        public string MsgOnLeavingZone { get; set; }
        public BindingList<Safearealocation> SafeAreaLocation { get; set; }
        public int CleanUpTimer { get; set; }
        public BindingList<string> ObjectsToDelete { get; set; }
        public BindingList<string> SZSteamUIDs { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;
    }

    public class Safearealocation
    {
        public string SafeZoneStatut { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public int Countdown { get; set; }

        public override string ToString()
        {
            return SafeZoneStatut;
        }
    }

    /// <summary>
    /// TraderPlusVehiclesConfig.json
    /// </summary>
    public class TraderPlusVehiclesConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusVehiclesConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public BindingList<Vehiclespart> VehiclesParts { get; set; }

        public TraderPlusVehiclesConfig()
        {
            VehiclesParts = new BindingList<Vehiclespart>();
        }
    }

    public class Vehiclespart
    {
        public string VehicleName { get; set; }
        public int Height { get; set; }
        public BindingList<string> VehicleParts { get; set; }

        public Vehiclespart()
        {
            VehicleParts = new BindingList<string>();
        }

        public override string ToString()
        {
            return VehicleName;
        }
    }
    /// <summary>
    /// TraderPlusStock.json, one for each trader ID
    /// </summary>
    public class TraderPlusStock
    {
        [JsonIgnore]
        public string fileName { get; set; }
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;


        public BindingList<string> TraderPlusItems { get; set; }

        public TraderPlusStock()
        {
            TraderPlusItems = new BindingList<string>();
        }

        public override string ToString()
        {
            return fileName;
        }
    }

}
