using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TraderPlusGarageConfig
    {

        [JsonIgnore]
        public const string m_version = "2.5";
        [JsonIgnore]
        public const string fileName = "TraderPlusGarageConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public string Version { get; set; }
        public int UseGarageOnlyToTrade { get; set; }
        public int VehicleMustHaveLock { get; set; }
        public int SaveVehicleCargo { get; set; }
        public int SaveVehicleHealth { get; set; }
        public int SaveVehicleFuel { get; set; }
        public int SavedVehicleInGarageForTradeInHour { get; set; }
        public int MaxVehicleStored { get; set; }
        public int ParkInCost { get; set; }
        public int ParkOutCost { get; set; }
        public int PayWithBankAccount { get; set; }
        public BindingList<string> WhitelistedObjects { get; set; }
        public BindingList<Npc> NPCs { get; set; }
        public string ParkingNotAvailable { get; set; }
        public string NotEnoughMoney { get; set; }
        public string NotRightToPark { get; set; }
        public string CarHasMember { get; set; }
        public string ParkInFail { get; set; }
        public string ParkInSuccess { get; set; }
        public string ParkOutFail { get; set; }
        public string ParkOutSuccess { get; set; }
        public string MaxVehicleStoredReached { get; set; }
        public string TradeVehicleWarning { get; set; }
        public string TradeVehicleHasBeenDeleted { get; set; }

        public bool CheckVersion()
        {
            if (Version != m_version)
            {
                Version = m_version;
                return false;
            }
            return true;
        }

    }
    public class Npc
    {
        public string ClassName { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        public float[] ParkingPosition { get; set; }
        public float[] ParkingOrientation { get; set; }
        public BindingList<string> Clothes { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }
}
