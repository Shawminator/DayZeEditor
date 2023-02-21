using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TraderPlusInsuranceConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusInsuranceConfig.json";
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public const string m_Version = "2.5";

        public string Version { get; set; }
        public int[] AuthorizedIDInsurance { get; set; }
        public BindingList<Insurance> Insurances { get; set; }
        internal Insurance getinsurancebyCar(string vehicleName)
        {
            Insurance insurance = Insurances.Where(x => x.VehicleName == vehicleName).FirstOrDefault();
            return insurance;
        }
        public void setInsurers(TraderPlusGeneralConfig traderPlusGeneralConfig)
        {
            List<int> insurers = new List<int>();
            foreach(Trader t in traderPlusGeneralConfig.Traders)
            {
                if (t.isInsurer)
                    insurers.Add(t.Id);
            }
            AuthorizedIDInsurance = insurers.ToArray();
        }
        public bool CheckVersion()
        {
            if (Version != m_Version)
            {
                Version = m_Version;
                return false;
            }
            return true;
        }
    }

    public class Insurance
    {
        public string VehicleName { get; set; }
        public float InsurancePriceCoefficient { get; set; }
        public float CollateralMoneyCoefficient { get; set; }
    }
}
