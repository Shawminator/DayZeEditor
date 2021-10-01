using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class PartySettings
    {
        public int m_Version { get; set; }//current version is 2
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

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
    }
}
}
