using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class TerritorySettings
    {
        const int CurrentVersion = 0;
        public int m_Version { get; set; }
        public int EnableTerritories { get; set; }
        public int UseWholeMapForInviteList { get; set; }
        public float TerritorySize { get; set; }
        public float TerritoryPerimeterSize { get; set; }
        public int MaxMembersInTerritory { get; set; }
        public int MaxTerritoryPerPlayer { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public TerritorySettings()
        {
            m_Version = CurrentVersion;
            isDirty = true;
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
        public void SetFloatValue(string mytype, float myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
