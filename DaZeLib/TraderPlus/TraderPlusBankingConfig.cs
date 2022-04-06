using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TraderPlusBankingConfig
    {
        public string Version { get; set; }  //version  "2.2"
        public int BankingLogs { get; set; }
        public int IsCreditCarNeededForTransaction { get; set; }
        public float TransactionFees { get; set; }
        public int DefaultStartCurrency { get; set; }
        public int DefaultMaxCurrency { get; set; }
        public BindingList<string> CurrenciesAccepted { get; set; }
        public string TheAmountHasBeenTransferedToTheAccount { get; set; }

        [JsonIgnore]
        public const string m_version = "2.2";
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
        public bool CheckVersion()
        {
            if(Version != m_version)
            {
                Version = m_version;
                return false;
            }
            return true;
        }
    }
}
