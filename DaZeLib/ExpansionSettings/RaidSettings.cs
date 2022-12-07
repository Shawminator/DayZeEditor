using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DayZeLib
{
    public class Schedule
    {
        public string Weekday { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int DurationMinutes { get; set; }

        public override string ToString()
        {
            return Weekday;
        }
    }

    public enum BaseRaidMode
    {
        [Description("No Expansion Build Raiding")]
        Expansion_BaseBuilding_elements_cant_be_raided = -1,
        [Description("All BaseBuilding Can Be Raided")]
        Every_basebuilding_elements_can_be_raided = 0,
        [Description("Only Door and Gates")]
        Only_doors_and_gates = 1,
        [Description("Only Doors Gates and Windows")]
        Only_doors_Gates_and_windows = 2
    }
    public class RaidSettings
    {
        const int CurrentVersion = 5;

        public int m_Version { get; set; }
        public int BaseBuildingRaidMode { get; set; }
        public decimal ExplosionTime { get; set; }
        public BindingList<string> ExplosiveDamageWhitelist { get; set; }
        public int EnableExplosiveWhitelist { get; set; }
        public decimal ExplosionDamageMultiplier { get; set; }
        public decimal ProjectileDamageMultiplier { get; set; }
        public int CanRaidSafes { get; set; }
        public int SafeRaidUseSchedule { get; set; }
        public decimal SafeExplosionDamageMultiplier { get; set; }
        public decimal SafeProjectileDamageMultiplier { get; set; }
        public BindingList<string> SafeRaidTools { get; set; }
        public int SafeRaidToolTimeSeconds { get; set; }
        public int SafeRaidToolCycles { get; set; }
        public decimal SafeRaidToolDamagePercent { get; set; }
        public int CanRaidBarbedWire { get; set; }
        public BindingList<string> BarbedWireRaidTools { get; set; }
        public int BarbedWireRaidToolTimeSeconds { get; set; }
        public int BarbedWireRaidToolCycles { get; set; }
        public decimal BarbedWireRaidToolDamagePercent { get; set; }
        public int CanRaidLocksOnWalls { get; set; }
        public int CanRaidLocksOnFences { get; set; }
        public int CanRaidLocksOnTents { get; set; }
        public BindingList<string> LockRaidTools { get; set; }
        public int LockOnWallRaidToolTimeSeconds { get; set; }
        public int LockOnFenceRaidToolTimeSeconds { get; set; }
        public int LockOnTentRaidToolTimeSeconds { get; set; }
        public int LockRaidToolCycles { get; set; }
        public decimal LockRaidToolDamagePercent { get; set; }
        public int CanRaidLocksOnContainers { get; set; }
        public BindingList<string> LockOnContainerRaidTools { get; set; }
        public int LockOnContainerRaidToolTimeSeconds { get; set; }
        public int LockOnContainerRaidToolCycles { get; set; }
        public decimal LockOnContainerRaidToolDamagePercent { get; set; }
        public BindingList<Schedule> Schedule { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public RaidSettings()
        {
            m_Version = CurrentVersion;
            ExplosiveDamageWhitelist = new BindingList<string>();
            SafeRaidTools = new BindingList<string>();
            BarbedWireRaidTools = new BindingList<string>();
            LockRaidTools = new BindingList<string>();
            LockOnContainerRaidTools = new BindingList<string>();
            Schedule = new BindingList<Schedule>();
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
