using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class SFLConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int EnableDebug { get; set; }//bool
        public decimal Rarity { get; set; }//float
        public int InitialCooldown { get; set; }
        public int XPGain { get; set; }
        public int SoundEnabled { get; set; }//bool
        public int DisableNotifications { get; set; }//bool
        public string NotificationHeading { get; set; }
        public string NotificationText { get; set; }
        public string NotificationText2 { get; set; }
        public decimal MaxHealthCoef { get; set; }//float
        public BindingList<SFBuildingCategory> SFLBuildings { get; set; }
        public BindingList<SFLootCategory> SFLLootCategory { get; set; }
        public BindingList<SFProxyCategory> SFLProxyCategory { get; set; }

        public SFLConfig()
        {
            SFLBuildings = new BindingList<SFBuildingCategory>();
            SFLLootCategory = new BindingList<SFLootCategory>();
            SFLProxyCategory = new BindingList<SFProxyCategory>();
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetDecimalValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetBoolValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetTextValue(string mytype, string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class SFBuildingCategory
    {
        public string name { get; set; }
        public BindingList<string> buildings { get; set; }

        public SFBuildingCategory()
        {
            buildings = new BindingList<string>();            
        }
        public void SetTextValue(string mytype, string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public override string ToString()
        {
            return name;
        }
    }

    public class SFLootCategory
    {
        public string name { get; set; }
        public decimal rarity { get; set; }//float
        public BindingList<string> loot { get; set; }

        public SFLootCategory()
        {
            loot = new BindingList<string>();
        }
        public void SetDecimalValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetTextValue(string mytype, string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public override string ToString()
        {
            return name;
        }
    }

    public class SFProxyCategory
    {
        public string name { get; set; }
        public BindingList<string> proxies { get; set; }

        public SFProxyCategory()
        {
            proxies = new BindingList<string>();
        }
        public void SetTextValue(string mytype, string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public override string ToString()
        {
            return name;
        }
    }
}
