using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ECMExpansionCircleMarkerConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public decimal ScheduleZoneCheckInterval { get; set; }
        public decimal PvPExitCountdown { get; set; }
        public int PvPeverywhere { get; set; }
        public int ReflectDamage { get; set; }
        public int PvPForceFirstPerson { get; set; }
        public int BlockChemGasGrenadeOutsidePvP { get; set; }
        public int ForceFirstPersonInVehicle { get; set; }
        public int ThirdPersonOnlyDriverAndCoDriver { get; set; }
        public int DisablePlayerZoneCheckInterval { get; set; }
        public decimal PlayerZoneCheckInterval { get; set; }
        public int DisablePlayerMinMoveDistance { get; set; }
        public decimal PlayerMinMoveDistance { get; set; }
        public int EnablePvPZoneCreationWhileLockpickingVehicle { get; set; }
        public int EnableLockpickingBroadcast { get; set; }
        public decimal LockpickingBroadcastRadius { get; set; }
        public int AllowAIToDoDamageEverywhere { get; set; }
        public int OnlyAllowAIToDoDamageIfPlayerIsPvP { get; set; }
        public int AllowDamageToAIEverywhere { get; set; }
        public int AllowDamageToAIOnlyIfPlayerHasPvPStatus { get; set; }
        public int DisableExpansionGroupsFriendlyFire { get; set; }
        public int DisableExpansionGroupsFriendlyFireItemDamage { get; set; }
        public string PvPIconPath { get; set; }
        public decimal m_PvPIconX { get; set; }
        public decimal m_PvPIconY { get; set; }
        public int EnableTerritoryFlagPvPZones { get; set; }
        public decimal TerritoryFlagPvPZoneCheckInterval { get; set; }
        public decimal TerritoryFlagPvPRadius { get; set; }
        public BindingList<Customzone> CustomZones { get; set; }
        public AIRDROP_ZONES AIRDROP_ZONES { get; set; }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);

        }
        public void SetDecimalValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);

        }
        public void SetBoolValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);

        }
        public void SetTextValue(string mytype,string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);

        }
    }

    public class AIRDROP_ZONES
    {
        public decimal Radius { get; set; }
        public int drawCircle { get; set; }
        public int isPvPZone { get; set; }
        public int zoneAlpha { get; set; }
        public int zoneRed { get; set; }
        public int zoneGreen { get; set; }
        public int zoneBlue { get; set; }
    }

    public class Customzone
    {
        public string zoneName { get; set; }
        public decimal x { get; set; }
        public decimal z { get; set; }
        public decimal zoneRadius { get; set; }
        public int zoneAlpha { get; set; }
        public int zoneRed { get; set; }
        public int zoneGreen { get; set; }
        public int zoneBlue { get; set; }
        public int drawCircle { get; set; }
        public int isPvPZone { get; set; }
        public Zoneschedule ZoneSchedule { get; set; }
        public int EnableCustomMessages { get; set; }
        public string CustomTitle { get; set; }
        public string CustomMessageEnter { get; set; }
        public string CustomMessageExit { get; set; }
        public string CustomIcon { get; set; }
        public BindingList<string> OnlyAllowedWeapons { get; set; }
    }
}
