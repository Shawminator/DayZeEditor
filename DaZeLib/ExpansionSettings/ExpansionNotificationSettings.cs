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
        const int CurrentVersion = 5;

        public int m_Version { get; set; }
        public bool EnableNotification{ get; set; }
        public bool ShowPlayerJoinServer{ get; set; }
        public ExpansionAnnouncementType JoinMessageType{ get; set; }
        public bool ShowPlayerLeftServer{ get; set; }
        public ExpansionAnnouncementType LeftMessageType{ get; set; }

        public bool ShowAirdropStarted{ get; set; }
        public bool ShowAirdropClosingOn{ get; set; }
        public bool ShowAirdropDropped{ get; set; }
        public bool ShowAirdropEnded{ get; set; }

        public bool ShowPlayerAirdropStarted{ get; set; }
        public bool ShowPlayerAirdropClosingOn{ get; set; }
        public bool ShowPlayerAirdropDropped{ get; set; }

        public bool ShowAIMissionStarted{ get; set; }
        public bool ShowAIMissionAction{ get; set; }
        public bool ShowAIMissionEnded{ get; set; }

        public bool ShowTerritoryNotifications{ get; set; }                //! Show the notifications when entering or leaving territory.

        public bool EnableKillFeed{ get; set; }
        public ExpansionAnnouncementType KillFeedMessageType{ get; set; }
        public bool KillFeedFall{ get; set; }
        public bool KillFeedCarHitDriver{ get; set; }
        public bool KillFeedCarHitNoDriver{ get; set; }
        public bool KillFeedCarCrash{ get; set; }
        public bool KillFeedCarCrashCrew{ get; set; }

        public bool KillFeedHeliHitDriver{ get; set; }
        public bool KillFeedHeliHitNoDriver{ get; set; }
        public bool KillFeedHeliCrash{ get; set; }
        public bool KillFeedHeliCrashCrew{ get; set; }
        public bool KillFeedBoatHitDriver{ get; set; }
        public bool KillFeedBoatHitNoDriver{ get; set; }
        public bool KillFeedBoatCrash{ get; set; }
        public bool KillFeedBoatCrashCrew{ get; set; }
        /*bool KillFeedPlaneHitDriver{ get; set; }
        bool KillFeedPlaneHitNoDriver{ get; set; }
        bool KillFeedBikeHitDriver{ get; set; }
        bool KillFeedBikeHitNoDriver{ get; set; }*/

        public bool KillFeedBarbedWire{ get; set; }
        public bool KillFeedFire{ get; set; }
        public bool KillFeedWeaponExplosion{ get; set; }
        public bool KillFeedDehydration{ get; set; }
        public bool KillFeedStarvation{ get; set; }
        public bool KillFeedBleeding{ get; set; }
        public bool KillFeedStatusEffects{ get; set; }
        public bool KillFeedSuicide{ get; set; }
        public bool KillFeedWeapon{ get; set; }
        public bool KillFeedMeleeWeapon{ get; set; }
        public bool KillFeedBarehands{ get; set; }
        public bool KillFeedInfected{ get; set; }
        public bool KillFeedAnimal{ get; set; }
        public bool KillFeedAI{ get; set; }
        public bool KillFeedDrowned { get; set; }
        public bool KillFeedKilledUnknown{ get; set; }
        public bool KillFeedDiedUnknown{ get; set; }

        public bool EnableKillFeedDiscordMsg { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionNotificationSettings()
        {
            m_Version = CurrentVersion;
            EnableNotification = true;

            ShowPlayerJoinServer = true;
            JoinMessageType = ExpansionAnnouncementType.NOTIFICATION;
            ShowPlayerLeftServer = true;
            LeftMessageType = ExpansionAnnouncementType.NOTIFICATION;

            ShowAirdropStarted = true;
            ShowAirdropClosingOn = true;
            ShowAirdropDropped = true;
            ShowAirdropEnded = true;
            ShowPlayerAirdropStarted = true;
            ShowPlayerAirdropClosingOn = true;
            ShowPlayerAirdropDropped = true;
            ShowTerritoryNotifications = true;
            EnableKillFeed = true;
            KillFeedMessageType = ExpansionAnnouncementType.NOTIFICATION;

            //! These are not implemented, uncomment once done
            //ShowDistanceOnKillFeed = true;
            //ShowVictimOnKillFeed = true;
            //ShowKillerOnKillFeed = true;
            //ShowWeaponOnKillFeed = true;

            KillFeedFall = true;
            KillFeedCarHitDriver = true;
            KillFeedCarHitNoDriver = true;
            KillFeedCarCrash = true;
            KillFeedCarCrashCrew = true;
            KillFeedHeliHitDriver = true;
            KillFeedHeliHitNoDriver = true;
            KillFeedHeliCrash = true;
            KillFeedHeliCrashCrew = true;
            KillFeedBoatHitDriver = true;
            KillFeedBoatHitNoDriver = true;
            KillFeedBoatCrash = true;
            KillFeedBoatCrashCrew = true;
            /*KillFeedPlaneHitDriver = true;
            KillFeedPlaneHitNoDriver = true;
            KillFeedBikeHitDriver = true;
            KillFeedBikeHitNoDriver = true;*/
            KillFeedBarbedWire = true;
            KillFeedFire = true;
            KillFeedWeaponExplosion = true;
            KillFeedDehydration = true;
            KillFeedStarvation = true;
            KillFeedBleeding = true;
            KillFeedStatusEffects = true;
            KillFeedSuicide = true;
            KillFeedWeapon = true;
            KillFeedMeleeWeapon = true;
            KillFeedBarehands = true;
            KillFeedInfected = true;
            KillFeedAnimal = true;
            KillFeedDrowned = true;
            KillFeedKilledUnknown = true;
            KillFeedDiedUnknown = true;

            EnableKillFeedDiscordMsg = false;
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

        public void SetBoolValue(string mytype, bool myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
