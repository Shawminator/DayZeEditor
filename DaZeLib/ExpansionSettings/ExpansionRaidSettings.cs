using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DayZeLib
{
    public class ExpansionRaidSchedule
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

    public enum BaseBuildingRaidMode
    {
        ExpansionBaseBuildingElementscantberaided = -1,
        All = 0,
        DoorsGates,
        DoorsGatesWindows,
        DoorsGatesWindowsWalls,
    }
    public enum RaidLocksOnWallsEnum
    {
        Disabled = 0,
        Enabled,
        OnlyDoor,
        OnlyGate
    };
    public class ExpansionRaidSettings
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
        public int LockOnContainerRaidUseSchedule { get; set; }
        public BindingList<string> LockOnContainerRaidTools { get; set; }
        public int LockOnContainerRaidToolTimeSeconds { get; set; }
        public int LockOnContainerRaidToolCycles { get; set; }
        public decimal LockOnContainerRaidToolDamagePercent { get; set; }
        public BindingList<ExpansionRaidSchedule> Schedule { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionRaidSettings()
        {
            m_Version = CurrentVersion;
            ExplosionTime = 30;
            ExplosiveDamageWhitelist = new BindingList<string>() {
                "Expansion_C4_Explosion",
                "Expansion_RPG_Explosion",
                "Expansion_LAW_Explosion",
                "M79",
                "RGD5Grenade",
                "M67Grenade",
                "FlashGrenade",
                "Land_FuelStation_Feed",
                "Land_FuelStation_Feed_Enoch"
            };

            EnableExplosiveWhitelist = 0;
            ExplosionDamageMultiplier = 50;
            ProjectileDamageMultiplier = 1;

            CanRaidSafes = 1;
            SafeRaidUseSchedule = 1;
            CanRaidLocksOnContainers = 1;
            LockOnContainerRaidUseSchedule = 1;
            SafeExplosionDamageMultiplier = 17;
            SafeProjectileDamageMultiplier = 1;

            LockOnContainerRaidTools = new BindingList<string>() { "ExpansionPropaneTorch" };
            LockOnContainerRaidToolTimeSeconds = 10 * 60;
            LockOnContainerRaidToolCycles = 5;
            LockOnContainerRaidToolDamagePercent = 100;

            SafeRaidTools = new BindingList<string>() { "ExpansionPropaneTorch" };
            SafeRaidToolTimeSeconds = 10 * 60;
            SafeRaidToolCycles = 5;
            SafeRaidToolDamagePercent = 100;

            CanRaidBarbedWire = 1;

            BarbedWireRaidTools = new BindingList<string>() { "ExpansionBoltCutters" };
            BarbedWireRaidToolTimeSeconds = 5 * 60;
            BarbedWireRaidToolCycles = 5;
            BarbedWireRaidToolDamagePercent = 100;

            CanRaidLocksOnWalls = 0;
            CanRaidLocksOnFences = 0;
            CanRaidLocksOnTents = 0;

            LockOnWallRaidToolTimeSeconds = 15 * 60;
            LockOnFenceRaidToolTimeSeconds = 15 * 60;
            LockOnTentRaidToolTimeSeconds = 10 * 60;
            LockRaidToolCycles = 5;
            LockRaidToolDamagePercent = 100;

            BaseBuildingRaidMode = (int)DayZeLib.BaseBuildingRaidMode.All;
            Schedule = new BindingList<ExpansionRaidSchedule>();
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Sunday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Monday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Tuesday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Wednesday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Thursday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "friday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
            Schedule.Add(new ExpansionRaidSchedule() { Weekday = "Saturday", StartHour = 0, StartMinute = 0, DurationMinutes = 1440 });
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
