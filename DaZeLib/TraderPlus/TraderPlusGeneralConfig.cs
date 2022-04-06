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
        [JsonIgnore]
        public const string m_Version = "2.2";
        [JsonIgnore]
        public int tempindex;

        public string Version { get; set; } //version "2.2"
        public int ConvertTraderConfigToTraderPlus { get; set; }
        public int ConvertTraderConfigToTraderPlusWithStockBasedOnCE { get; set; }
        public int UseGarageToTradeCar { get; set; }
        public int DisableHeightFailSafeForReceiptDeployment { get; set; }
        public int EnableShowAllPrices { get; set; }
        public int EnableShowAllCheckBox { get; set; }
        //public int EnableStockAllCategory { get; set; } //removed in 2.1
        public int IsReceiptTraderOnly { get; set; }
        public int IsReceiptSaveLock { get; set; }
        public int IsReceiptSaveAttachment { get; set; }
        public int IsReceiptSaveCargo { get; set; }
        public float LockPickChance { get; set; }
        public string LicenceKeyWord { get; set; }
        public BindingList<string> Licences { get; set; }
        public Acceptedstates AcceptedStates { get; set; }
        public int StoreOnlyToPristineState { get; set; }
        public BindingList<Currency> Currencies { get; set; }
        public BindingList<Trader> Traders { get; set; }
        public BindingList<Traderobject> TraderObjects { get; set; }

        public bool CheckVersion()
        {
            if (Version != m_Version)
            {
                Version = m_Version;
                return false;
            }
            return true;
        }
        public void SortTradersByIndex()
        {
            List<Trader> tradlist = new List<Trader>(Traders);
            var sortedListInstance = new BindingList<Trader>(tradlist.OrderBy(x => x.Id).ToList());
            Traders = sortedListInstance;
        }
        public void UpdateIndexes(TraderPlusIDsConfig traderPlusIDsConfig)
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
        public void getInsurers(TraderPlusInsuranceConfig traderPlusInsuranceConfig)
        {
            foreach(Trader t in Traders)
            {
                if (traderPlusInsuranceConfig.AuthorizedIDInsurance.Contains(t.Id))
                    t.isInsurer = true;
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
        public void GetCategoriesbyID(TraderPlusIDsConfig traderPlusIDsConfig)
        {
            foreach(Trader t in Traders)
            {
                if (t.isBanker) continue;
                t.TraderCategoryList = traderPlusIDsConfig.getTraderbyID(t.Id);
            }
        }
        public void SaveIDS(TraderPlusIDsConfig traderPlusIDsConfig)
        {
            traderPlusIDsConfig.IDs = new BindingList<IDs>();
            foreach(Trader t in Traders)
            {
                if (t.isBanker) continue;
                traderPlusIDsConfig.IDs.Add(t.TraderCategoryList);
            }
            traderPlusIDsConfig.isDirty = true;
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
        public IDs TraderCategoryList;
        [JsonIgnore]
        public bool isBanker { get; set; }
        [JsonIgnore]
        public bool isInsurer { get; set; }

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
