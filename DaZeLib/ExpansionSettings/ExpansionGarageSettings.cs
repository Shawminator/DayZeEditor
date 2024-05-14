using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum ExpansionGarageMode
    {
        Territory = 0,
        Personal
    }
    public enum ExpansionGarageStoreMode
    {
        Personal = 0,
        TerritoryShared,
        TerritoryTyrannical
    }
    public enum ExpansionGarageRetrieveMode
    {
        Personal = 0,
        TerritoryShared,
        TerritoryTyrannical
    }
    public enum ExpansionGarageGroupStoreMode
    {
        [Description("Group members can only store vehicles of other group members")]
        StoreOnly = 0,
        [Description("Group members can only retrieve vehicles of other group members")]
        RetrieveOnly,
        [Description("Group members can store and retrieve vehicles of other group members.")]
        StoreAndRetrieve
    }

    public class ExpansionGarageSettings
    {
        const int CurrentVersion = 6;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int AllowStoringDEVehicles { get; set; }
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
        public int ParkingMeterEnableFlavor { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionGarageSettings()
        {
            m_Version = CurrentVersion;
            Enabled = 1;
            AllowStoringDEVehicles = 0;

            GarageMode = (int)ExpansionGarageMode.Personal;

            GarageStoreMode = 0;
            GarageRetrieveMode = 0;
            MaxStorableVehicles = 2;

            VehicleSearchRadius = (decimal)20.0;
            MaxDistanceFromStoredPosition = (decimal)150.0;
            CanStoreWithCargo = 1;
            NeedKeyToStore = 1;

            EntityWhitelist = new BindingList<string>() { "ExpansionParkingMeter" };

            EnableGroupFeatures = 0;
            GroupStoreMode = 2;

            EnableMarketFeatures = 0;
            StorePricePercent = (decimal)5.0;
            StaticStorePrice = 0;

            MaxStorableTier1 = 2;
            MaxStorableTier2 = 4;
            MaxStorableTier3 = 6;
            MaxRangeTier1 = (decimal)20.0;
            MaxRangeTier2 = (decimal)30.0;
            MaxRangeTier3 = (decimal)40.0;
            ParkingMeterEnableFlavor = 0;
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
    }

}
