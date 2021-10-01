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
        public int m_Version { get; set; } //Currently Version 3
        public int Enabled { get; set; }
        public float FrameRateCheckSafeZoneInMs { get; set; }
        public BindingList<CircleZones> CircleZones { get; set; }
        public BindingList<PolygonZones> PolygonZones { get; set; }
        public int DisableVehicleDamageInSafeZone { get; set; }
        public int EnableForceSZCleanup { get; set; }
        public int ForceSZCleanupInterval { get; set; }
        public float ItemLifetimeInSafeZone{get;set;}

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

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
                if (PZ.CenterPolygon == null)
                    PZ.CenterPolygon = new float[] {0,0,0};
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
            CircleZones.Add(new CircleZones() {Type = 0, Center = new float[] {0,0,0 }, Radius = 100 });
            SetCircleNames();
            isDirty = true;
        }
    }
    // Zone types 
    // 1 = Circle
    // 2 = polygon
    public class CircleZones
    {
        public int Type { get; set; }
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
        public int Type { get; set; }
        public BindingList<float[]> Positions { get; set; } 
        public float[] CenterPolygon { get; set; }
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
}
