using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class TraderPlusSafeZoneConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusSafeZoneConfig.json";
        [JsonIgnore]
        public const string m_Version = "2.5";

        public string Version { get; set; } //current version 2.3
        public int IsHideOutActive { get; set; }
        public int EnableAfkDisconnect { get; set; }
        public int KickAfterDelay { get; set; }
        public string MsgEnterZone { get; set; }
        public string MsgExitZone { get; set; }
        public string MsgOnLeavingZone { get; set; }
        public string MustRemoveArmband { get; set; }
        public BindingList<Safearealocation> SafeAreaLocation { get; set; }
        public int CleanUpTimer { get; set; }
        public BindingList<string> WhitelistEntities { get; set; }
        public BindingList<string> ObjectsToDelete { get; set; }
        public BindingList<string> SZSteamUIDs { get; set; }
        public BindingList<string> BlackListedItemInStash { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;


        public TraderPlusSafeZoneConfig()
        {
            Version = m_Version;
            IsHideOutActive = 0;
            EnableAfkDisconnect = 0;
            KickAfterDelay = 30;
            MsgEnterZone = "SafeZone Entered.";
            MsgExitZone = "You have left SafeZoe";
            MsgOnLeavingZone = "LEAVING ZONE IN:";
            MustRemoveArmband = "You must remove your armband in order to make your stash appear !";
            SafeAreaLocation = new BindingList<Safearealocation>();
            CleanUpTimer = 300;
            WhitelistEntities = new BindingList<string>();
            ObjectsToDelete = new BindingList<string>();
            SZSteamUIDs = new BindingList<string>();
            BlackListedItemInStash = new BindingList<string>();
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

    public class Safearealocation
    {
        public string SafeZoneStatut { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public int Countdown { get; set; }

        public override string ToString()
        {
            return SafeZoneStatut;
        }
    }
}
