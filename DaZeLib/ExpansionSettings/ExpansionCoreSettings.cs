using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionCoreSettings
    {
        const int CurrentVersion = 9;

        public int m_Version { get; set; }
        public int ServerUpdateRateLimit { get; set; }
        public int ForceExactCEItemLifetime { get; set; }
        public int EnableInventoryCargoTidy { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionCoreSettings()
        {
            m_Version = CurrentVersion;
            ServerUpdateRateLimit = 0;
            ForceExactCEItemLifetime = 0;
            EnableInventoryCargoTidy = 0;
        }

        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
