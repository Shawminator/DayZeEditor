using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class NotificationSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; }
        public int EnableNotification { get; set; }
        public int ShowPlayerJoinServer { get; set; }
        public int JoinMessageType { get; set; }
        public int ShowPlayerLeftServer { get; set; }
        public int LeftMessageType { get; set; }
        public int ShowAirdropStarted { get; set; }
        public int ShowAirdropClosingOn { get; set; }
        public int ShowAirdropDropped { get; set; }
        public int ShowAirdropEnded { get; set; }
        public int ShowPlayerAirdropStarted { get; set; }
        public int ShowPlayerAirdropClosingOn { get; set; }
        public int ShowPlayerAirdropDropped { get; set; }
        public int ShowTerritoryNotifications { get; set; }
        public int EnableKillFeed { get; set; }
        public int KillFeedMessageType { get; set; }
        public int KillFeedFall { get; set; }
        public int KillFeedCarHitDriver { get; set; }
        public int KillFeedCarHitNoDriver { get; set; }
        public int KillFeedCarCrash { get; set; }
        public int KillFeedCarCrashCrew { get; set; }
        public int KillFeedHeliHitDriver { get; set; }
        public int KillFeedHeliHitNoDriver { get; set; }
        public int KillFeedHeliCrash { get; set; }
        public int KillFeedHeliCrashCrew { get; set; }
        public int KillFeedBoatHitDriver { get; set; }
        public int KillFeedBoatHitNoDriver { get; set; }
        public int KillFeedBoatCrash { get; set; }
        public int KillFeedBoatCrashCrew { get; set; }
        public int KillFeedBarbedWire { get; set; }
        public int KillFeedFire { get; set; }
        public int KillFeedWeaponExplosion { get; set; }
        public int KillFeedDehydration { get; set; }
        public int KillFeedStarvation { get; set; }
        public int KillFeedBleeding { get; set; }
        public int KillFeedStatusEffects { get; set; }
        public int KillFeedSuicide { get; set; }
        public int KillFeedWeapon { get; set; }
        public int KillFeedMeleeWeapon { get; set; }
        public int KillFeedBarehands { get; set; }
        public int KillFeedInfected { get; set; }
        public int KillFeedAnimal { get; set; }
        public int KillFeedKilledUnknown { get; set; }
        public int KillFeedDiedUnknown { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public NotificationSettings()
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
