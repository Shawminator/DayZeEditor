using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum ExpansionAnnouncementType
    {
        CHAT = 0,
        NOTIFICATION,
        MUTEDNOTIFICATION
    };
    public class ExpansionNotificationSettings
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
        public int EnableKillFeedDiscordMsg { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionNotificationSettings()
        {
            m_Version = CurrentVersion;
            EnableNotification = 1;

            ShowPlayerJoinServer = 1;
            JoinMessageType = (int)ExpansionAnnouncementType.NOTIFICATION;
            ShowPlayerLeftServer = 1;
            LeftMessageType = (int)ExpansionAnnouncementType.NOTIFICATION;

            ShowAirdropStarted = 1;
            ShowAirdropClosingOn = 1;
            ShowAirdropDropped = 1;
            ShowAirdropEnded = 1;
            ShowPlayerAirdropStarted = 1;
            ShowPlayerAirdropClosingOn = 1;
            ShowPlayerAirdropDropped = 1;
            ShowTerritoryNotifications = 1;
            EnableKillFeed = 1;
            KillFeedMessageType = (int)ExpansionAnnouncementType.NOTIFICATION;

            //! These are not implemented, uncomment once done
            //ShowDistanceOnKillFeed = true;
            //ShowVictimOnKillFeed = true;
            //ShowKillerOnKillFeed = true;
            //ShowWeaponOnKillFeed = true;

            KillFeedFall = 1;
            KillFeedCarHitDriver = 1;
            KillFeedCarHitNoDriver = 1;
            KillFeedCarCrash = 1;
            KillFeedCarCrashCrew = 1;
            KillFeedHeliHitDriver = 1;
            KillFeedHeliHitNoDriver = 1;
            KillFeedHeliCrash = 1;
            KillFeedHeliCrashCrew = 1;
            KillFeedBoatHitDriver = 1;
            KillFeedBoatHitNoDriver = 1;
            KillFeedBoatCrash = 1;
            KillFeedBoatCrashCrew = 1;
            /*KillFeedPlaneHitDriver = true;
            KillFeedPlaneHitNoDriver = true;
            KillFeedBikeHitDriver = true;
            KillFeedBikeHitNoDriver = true;*/
            KillFeedBarbedWire = 1;
            KillFeedFire = 1;
            KillFeedWeaponExplosion = 1;
            KillFeedDehydration = 1;
            KillFeedStarvation = 1;
            KillFeedBleeding = 1;
            KillFeedStatusEffects = 1;
            KillFeedSuicide = 1;
            KillFeedWeapon = 1;
            KillFeedMeleeWeapon = 1;
            KillFeedBarehands = 1;
            KillFeedInfected = 1;
            KillFeedAnimal = 1;
            KillFeedKilledUnknown = 1;
            KillFeedDiedUnknown = 1;

            EnableKillFeedDiscordMsg = 0;
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
