using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionTerritorySettings
    {
        const int CurrentVersion = 4;
        public int m_Version { get; set; }
        public int EnableTerritories { get; set; }
        public int UseWholeMapForInviteList { get; set; }
        public decimal TerritorySize { get; set; }
        public decimal TerritoryPerimeterSize { get; set; }
        public int MaxMembersInTerritory { get; set; }
        public int MaxTerritoryPerPlayer { get; set; }
        public decimal TerritoryInviteAcceptRadius { get; set; }
        public int AuthenticateCodeLockIfTerritoryMember { get; set; }
        public int InviteCooldown { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionTerritorySettings()
        {
            m_Version = CurrentVersion;
            EnableTerritories = 1;
            UseWholeMapForInviteList = 1;
            TerritorySize = (decimal)150.0;
            TerritoryPerimeterSize = (decimal)150.0;
            MaxMembersInTerritory = 10;
            MaxTerritoryPerPlayer = 1;
            TerritoryInviteAcceptRadius = (decimal)150.0;
            AuthenticateCodeLockIfTerritoryMember = 0; //added in version 3
            InviteCooldown = 0; //added in version 4
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
        public void SetdeciamlValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
