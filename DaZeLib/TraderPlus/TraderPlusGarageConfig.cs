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

        public TraderPlusGarageConfig()
        {
            Version = m_version;
            UseGarageOnlyToTrade = 1;
            VehicleMustHaveLock = 1;
            SaveVehicleCargo = 1;
            SaveVehicleHealth = 1;
            SaveVehicleFuel = 1;
            SavedVehicleInGarageForTradeInHour = 1;
            MaxVehicleStored = 5;
            ParkInCost = 0;
            ParkOutCost = 0;
            PayWithBankAccount = 0;
            WhitelistedObjects = new BindingList<string>();
            NPCs = new BindingList<Npc>();
            ParkingNotAvailable = "The parking area is blocked!";
            NotEnoughMoney = "You don't have enough money";
            NotRightToPark = "You don't have the right to park this vehicle!";
            CarHasMember = "Your vehicle still has a player inside!";
            ParkInFail = "Your vehicle was not able to be stored correctly";
            ParkInSuccess = "Your vehicle was successfully stored";
            ParkOutFail = "Your vehicle was not able to retrieved!";
            ParkOutSuccess = "Youre vehicle was retrieved successfully!";
            MaxVehicleStoredReached = "Max number of vehicle reached!";
            TradeVehicleWarning = "The vehicle has been stored for %1 hour; passed this point it will be lost so trade it or get it fast";
            TradeVehicleHasBeenDeleted = "The vehicle stored for trade reached his inactivity and has been deleted!";
        }

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
