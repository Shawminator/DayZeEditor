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
        public string Version { get; set; }  //version  "2.5"
        public int BankingLogs { get; set; }
        public int IsCreditCarNeededForTransaction { get; set; }
        public float TransactionFees { get; set; }
        public int DefaultStartCurrency { get; set; }
        public int DefaultMaxCurrency { get; set; }
        public BindingList<string> CurrenciesAccepted { get; set; }
        public string TheAmountHasBeenTransferedToTheAccount { get; set; }
        public string TheAmountErrorTransferAccount { get; set; }

        [JsonIgnore]
        public const string m_version = "2.5";
        [JsonIgnore]
        public const string fileName = "TraderPlusBankingConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public TraderPlusBankingConfig()
        {
            Version = m_version;
            BankingLogs = 1;
            IsCreditCarNeededForTransaction = 0;
            TransactionFees = 0;
            DefaultStartCurrency = 0;
            DefaultMaxCurrency = 1000000;
            CurrenciesAccepted = new BindingList<string>();
            TheAmountErrorTransferAccount = "You have successfully transferred the amount to another player.";
            TheAmountErrorTransferAccount = "The transaction could not be completed. Check you have the right details.";
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
