using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class LogSettings
    {
        const int CurrentVersion = 6;

        public int m_Version { get; set; }
        public int Safezone { get; set; }
        public int AdminTools { get; set; }
        public int ExplosionDamageSystem { get; set; }
        public int VehicleCarKey { get; set; }
        public int VehicleTowing { get; set; }
        public int VehicleLockPicking { get; set; }
        public int VehicleDestroyed { get; set; }
        public int VehicleAttachments { get; set; }
        public int VehicleEnter { get; set; }
        public int VehicleLeave { get; set; }
        public int VehicleDeleted { get; set; }
        public int VehicleEngine { get; set; }
        public int BaseBuildingRaiding { get; set; }
        public int CodeLockRaiding { get; set; }
        public int Territory { get; set; }
        public int Killfeed { get; set; }
        public int SpawnSelection { get; set; }
        public int Party { get; set; }
        public int MissionAirdrop { get; set; }
        public int Chat { get; set; }
        public int Market { get; set; }
        public int ATM { get; set; }
        public int AIGeneral { get; set; }
        public int AIPatrol { get; set; }
        public int AIObjectPatrol { get; set; }
        public int LogToScripts { get; set; }
        public int LogToADM { get; set; }
        public int Hardline { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public LogSettings()
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
