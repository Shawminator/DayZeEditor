using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TraderPlusVehiclesConfig
    {
        [JsonIgnore]
        public const string fileName = "TraderPlusVehiclesConfig.json";
        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public const string m_Version = "2.5";

        public string Version { get; set; } //current version 2.3
        public BindingList<Vehiclespart> VehiclesParts { get; set; }

        public TraderPlusVehiclesConfig()
        {
            VehiclesParts = new BindingList<Vehiclespart>();
        }
        public bool getInsurance(TraderPlusInsuranceConfig traderPlusInsuranceConfig)
        {
            bool needtosave = false;
            foreach(Vehiclespart vehicle in VehiclesParts)
            {
                Insurance insurance = traderPlusInsuranceConfig.getinsurancebyCar(vehicle.VehicleName);
                if (insurance == null)
                {
                    insurance = new Insurance()
                    {
                        VehicleName = vehicle.VehicleName,
                        InsurancePriceCoefficient = 0.5f,
                        CollateralMoneyCoefficient = 0.5f
                    };
                    needtosave = true;
                    traderPlusInsuranceConfig.isDirty = true;
                }
                vehicle.Insurance = insurance;
            }
            return needtosave;
        }
        public void setInsurances(TraderPlusInsuranceConfig traderPlusInsuranceConfig)
        {
            traderPlusInsuranceConfig.Insurances = new BindingList<Insurance>();
            foreach (Vehiclespart vehile in VehiclesParts)
            {
                traderPlusInsuranceConfig.Insurances.Add(vehile.Insurance);
            }
            traderPlusInsuranceConfig.isDirty = true;
        }
        public bool CheckVersion()
        {
            if (Version != m_Version)
            {
                Version = m_Version;
                return false;
            }
            return true;
        }
    }

    public class Vehiclespart
    {
        public string VehicleName { get; set; }
        public int Height { get; set; }
        public BindingList<string> VehicleParts { get; set; }

        [JsonIgnore]
        public Insurance Insurance;

        public Vehiclespart()
        {
            VehicleParts = new BindingList<string>();
        }
        public override string ToString()
        {
            return VehicleName;
        }
    }
}
