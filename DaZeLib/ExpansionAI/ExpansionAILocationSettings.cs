using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ExpansionAILocationSettings
    {
        [JsonIgnore]
        const int CurrentVersion = 1;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public BindingList<ExpansionAIRoamingLocation> RoamingLocations { get; set; }
        public BindingList<string> ExcludedRoamingBuildings {  get; set; }
        public BindingList<ExpansionAINoGoAreaConfig> NoGoAreas {  get; set; }

        public ExpansionAILocationSettings()
        {
            RoamingLocations = new BindingList<ExpansionAIRoamingLocation>();
            ExcludedRoamingBuildings = new BindingList<string>();
            NoGoAreas = new BindingList<ExpansionAINoGoAreaConfig>();
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

        public void GetVec3Points()
        {
            foreach(ExpansionAIRoamingLocation location in RoamingLocations)
            {
                location._Position = new Vec3(location.Position);
            }
            foreach(ExpansionAINoGoAreaConfig area in NoGoAreas) 
            {
                area._Position = new Vec3(area.Position);
            }
        }

        public void SetAILocationsPoints()
        {
            foreach (ExpansionAIRoamingLocation location in RoamingLocations)
            {
                location.Position = location._Position.getfloatarray();
            }
            foreach (ExpansionAINoGoAreaConfig area in NoGoAreas)
            {
                area.Position = area._Position.getfloatarray();
            }
        }
    }
    public class ExpansionAIRoamingLocation
    {
        public string Name { get; set; }
        public float[] Position { get;set; }
        public decimal Radius {  get; set; }
        public string Type { get; set; }
        public bool Enabled { get; set; }

        [JsonIgnore]
        public Vec3 _Position { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    public class ExpansionAINoGoAreaConfig
    {
        public string Name { get; set; }
        public float[] Position { get;set; }
        public float Radius { get; set; }
        public float Height { get; set; }

        [JsonIgnore]
        public Vec3 _Position { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
