using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class SafeZoneSettings
    {
        const int CurrentVersion = 10;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int FrameRateCheckSafeZoneInMs { get; set; }
        public BindingList<CircleZones> CircleZones { get; set; }
        public BindingList<PolygonZones> PolygonZones { get; set; }
        public BindingList<CylinderZones> CylinderZones { get; set; }
        public int ActorsPerTick { get; set; }
        public int DisableVehicleDamageInSafeZone { get; set; }
        public int EnableForceSZCleanup { get; set; }
        public decimal ItemLifetimeInSafeZone { get; set; }
        public int EnableForceSZCleanupVehicles { get; set; }
        public decimal VehicleLifetimeInSafeZone { get; set; }
        public BindingList<string> ForceSZCleanup_ExcludedItems { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public SafeZoneSettings()
        {
            m_Version = CurrentVersion;
            CircleZones = new BindingList<CircleZones>();
            PolygonZones = new BindingList<PolygonZones>();
            CylinderZones = new BindingList<CylinderZones>();
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
        public void SetCircleNames()
        {
            int i = 0;
            foreach (CircleZones CZ in CircleZones)
            {
                CZ.CircleSafeZoneName = "Circle Zone " + i.ToString();
                i++;
            }
        }
        public void SetPolygonNames()
        {
            int i = 0;
            foreach (PolygonZones PZ in PolygonZones)
            {
                PZ.polygonSafeZoneName = "Polygon Zone " + i.ToString();
                i++;
            }
        }
        public void SetCylinderNames()
        {
            int i = 0;
            foreach (CylinderZones PZ in CylinderZones)
            {
                PZ.CylinderSafeZoneName = "Cylinder Zone " + i.ToString();
                i++;
            }
        }
        public void Convertpolygonarrays()
        {
            foreach (PolygonZones PZ in PolygonZones)
            {
                PZ.Polygonpoints = new BindingList<Polygonpoints>();
                for(int i = 0; i < PZ.Positions.Count; i++ )
                {
                    Polygonpoints pgp = new Polygonpoints();
                    pgp.name = "point " + i.ToString();
                    pgp.points = PZ.Positions[i];
                    PZ.Polygonpoints.Add(pgp);
                }
            }
        }
        public void convertpointstoarray()
        {
            foreach (PolygonZones PZ in PolygonZones)
            {
                PZ.Positions = new BindingList<float[]>();
                foreach(Polygonpoints PGP in PZ.Polygonpoints)
                {
                    PZ.Positions.Add(PGP.points);
                }
            }
        }
        public void RemoveCircleZone(CircleZones currentZone)
        {
            CircleZones.Remove(currentZone);
            isDirty = true;
        }
        public void RemovePolygonZone(PolygonZones currentpolygonZones)
        {
            PolygonZones.Remove(currentpolygonZones);
            isDirty = true;
        }
        public void AddNewCircleZones()
        {
            CircleZones.Add(new CircleZones() {Center = new float[] {0,0,0 }, Radius = 100 });
            SetCircleNames();
            isDirty = true;
        }

        public void RemovecylinderZone(CylinderZones currentcylenderzones)
        {
            CylinderZones.Remove(currentcylenderzones);
            isDirty = true;
        }
    }
    // Zone types 
    // 1 = Circle
    // 2 = polygon
    // 3 = Cylinder
    public class CircleZones
    {
        public float[] Center { get; set; }
        public float Radius { get; set; }

        [JsonIgnore]
        public string CircleSafeZoneName { get; set; }

        public override string ToString()
        {
            return CircleSafeZoneName;
        }
    }
    public class PolygonZones
    {
        public BindingList<float[]> Positions { get; set; }
        public float RadiusPolygon { get; set; }

        [JsonIgnore]
        public string polygonSafeZoneName { get; set; }
        [JsonIgnore]
        public BindingList<Polygonpoints> Polygonpoints { get; set; }

        public override string ToString()
        {
            return polygonSafeZoneName;
        }
        public void SetPointnames()
        {
            int i = 0;
            foreach (Polygonpoints CZ in Polygonpoints)
            {
                CZ.name = "Point " + i.ToString();
                i++;
            }
        }
        public void removepoints(Polygonpoints currentpolygonpoint)
        {
            Polygonpoints.Remove(currentpolygonpoint);
        }
    }
    public class Polygonpoints
    {
        public string name { get; set; }
        public float[] points { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class CylinderZones
    {
        public float[] Center { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }

        [JsonIgnore]
        public string CylinderSafeZoneName { get; set; }

        public override string ToString()
        {
            return CylinderSafeZoneName;
        }
    }
}
