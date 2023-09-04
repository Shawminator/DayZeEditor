using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class PartySettings
    {
        const int CurrentVersion = 6;

        public int m_Version { get; set; }//current version is 3
        public int EnableParties { get; set; }
        public int MaxMembersInParty { get; set; }
        public int UseWholeMapForInviteList { get; set; }
        public int ShowPartyMember3DMarkers { get; set; }
        public int ShowDistanceUnderPartyMembersMarkers { get; set; }
        public int ShowNameOnPartyMembersMarkers { get; set; }
        public int EnableQuickMarker { get; set; }
        public int ShowDistanceUnderQuickMarkers { get; set; }
        public int ShowNameOnQuickMarkers { get; set; }
        public int CanCreatePartyMarkers { get; set; }
        public int ShowPartyMemberHUD { get; set; }
        public int ShowHUDMemberBlood { get; set; }
        public int ShowHUDMemberStates { get; set; }
        public int ShowHUDMemberStance { get; set; }
        public int ShowPartyMemberMapMarkers { get; set; }
        public int ShowHUDMemberDistance { get; set; }
        public int ForcePartyToHaveTags { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public PartySettings()
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
}
}
