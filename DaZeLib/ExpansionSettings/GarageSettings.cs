using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public enum GarageMode
    {
        Territory_Mode,
        Personal_Mode
    }
    public enum GarageStoreMode
    {
        Personal_Mode,
        Shared_Mode,
        Tyrannical_Mode
    }
    public enum GarageRetrieveMode
    {
        Personal_Mode,
        Shared_Mode,
        Tyrannical_Mode
    }
    public enum GroupStoreMode
    {
        [Description("Group members can only store vehicles of other group members")]
        Only_Store_other_Memebers_Vehicles,
        [Description("Group members can only retrieve vehicles of other group members")]
        Only_retrieve_other_memebers_vehicles,
        [Description("Group members can store and retrieve vehicles of other group members.")]
        Both_of_the_above
    }

    public class GarageSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int GarageMode { get; set; }
        public int GarageStoreMode { get; set; }
        public int GarageRetrieveMode { get; set; }
        public int MaxStorableVehicles { get; set; }
        public decimal VehicleSearchRadius { get; set; }
        public decimal MaxDistanceFromStoredPosition { get; set; }
        public int CanStoreWithCargo { get; set; }
        public int UseVirtualStorageForCargo { get; set; }
        public int NeedKeyToStore { get; set; }
        public BindingList<string> EntityWhitelist { get; set; }
        public int EnableGroupFeatures { get; set; }
        public int GroupStoreMode { get; set; }
        public int EnableMarketFeatures { get; set; }
        public decimal StorePricePercent { get; set; }
        public int StaticStorePrice { get; set; }
        public int MaxStorableTier1 { get; set; }
        public int MaxStorableTier2 { get; set; }
        public int MaxStorableTier3 { get; set; }
        public decimal MaxRangeTier1 { get; set; }
        public decimal MaxRangeTier2 { get; set; }
        public decimal MaxRangeTier3 { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

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
    }

}
