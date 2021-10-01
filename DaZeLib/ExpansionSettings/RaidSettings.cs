using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DayZeLib
{
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
        public int m_Version { get; set; } //current version is 0
        public float ExplosionTime { get; set; }
        public BindingList<string> ExplosiveDamageWhitelist { get; set; }
        public int EnableExplosiveWhitelist { get; set; }
        public float ExplosionDamageMultiplier { get; set; }
        public float ProjectileDamageMultiplier { get; set; }
        public int CanRaidSafes { get; set; }
        public float SafeExplosionDamageMultiplier { get; set; }
        public float SafeProjectileDamageMultiplier { get; set; }
        public BindingList<string> SafeRaidTools { get; set; }
        public int SafeRaidToolTimeSeconds { get; set; }
        public int SafeRaidToolCycles { get; set; }
        public float SafeRaidToolDamagePercent { get; set; }
        public int CanRaidBarbedWire { get; set; }
        public BindingList<string> BarbedWireRaidTools { get; set; }
        public int BarbedWireRaidToolTimeSeconds { get; set; }
        public int BarbedWireRaidToolCycles { get; set; }
        public float BarbedWireRaidToolDamagePercent { get; set; }
        public int CanRaidLocksOnWalls { get; set; }
        public int CanRaidLocksOnFences { get; set; }
        public int CanRaidLocksOnTents { get; set; }
        public BindingList<string> LockRaidTools { get; set; }
        public int LockOnWallRaidToolTimeSeconds { get; set; }
        public int LockOnFenceRaidToolTimeSeconds { get; set; }
        public int LockOnTentRaidToolTimeSeconds { get; set; }
        public int LockRaidToolCycles { get; set; }
        public float LockRaidToolDamagePercent { get; set; }
        public int BaseBuildingRaidMode { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

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
