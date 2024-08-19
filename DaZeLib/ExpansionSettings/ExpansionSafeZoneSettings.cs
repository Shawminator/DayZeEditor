using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionSafeZoneSettings
    {
        const int CurrentVersion = 11;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int FrameRateCheckSafeZoneInMs { get; set; }
        public BindingList<ExpansionSafeZoneCircle> CircleZones { get; set; }
        public BindingList<ExpansionSafeZonePolygon> PolygonZones { get; set; }
        public BindingList<ExpansionSafeZoneCylinder> CylinderZones { get; set; }
        public int ActorsPerTick { get; set; }
        public int DisablePlayerCollision { get; set; }
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

        public ExpansionSafeZoneSettings()
        {
            m_Version = CurrentVersion;
            Enabled = 1;
            DisablePlayerCollision = 0;
            DisableVehicleDamageInSafeZone = 1;
            FrameRateCheckSafeZoneInMs = 0;
            ActorsPerTick = 5;
            EnableForceSZCleanup = 1;
            ItemLifetimeInSafeZone = 15 * 60;  //! 15 Minutes
            EnableForceSZCleanupVehicles = 1;
            VehicleLifetimeInSafeZone = 60 * 60;  //! 60 Minutes
            CircleZones = new BindingList<ExpansionSafeZoneCircle>();
            PolygonZones = new BindingList<ExpansionSafeZonePolygon>();
            CylinderZones = new BindingList<ExpansionSafeZoneCylinder>();
            ForceSZCleanup_ExcludedItems = new BindingList<string>() { "CarCoverBase", "ExpansionVehicleCover" };
            DefaultChernarusSafeZones();
        }
        void DefaultChernarusSafeZones()
        {
            //! Krasnostav Trader Camp
            PolygonZones.Add(new ExpansionSafeZonePolygon()
            {
                Positions = new BindingList<float[]>()
                {
                    new float[] { 12288.9f, 142.4f, 12804.4f },
                    new float[] { 12068.4f, 139.8f, 12923.4f },
                    new float[] { 11680.6f, 141.1f, 12650.6f },
                    new float[] { 11805.3f, 146.3f, 12258.9f },
                    new float[] { 12327.7f, 140.0f, 12453.8f }
                }
            });
            //! Green Mountain Trader Camp
            CircleZones.Add(new ExpansionSafeZoneCircle()
            {
                Center = new float[] { 3728.27f, 403f, 6003.6f },
                Radius = 500
            });
            //! Kamenka Trader Camp
            CircleZones.Add(new ExpansionSafeZoneCircle()
            {
                Center = new float[] { 1143.14f, 6.9f, 2423.27f },
                Radius = 700
            });
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
            foreach (ExpansionSafeZoneCircle CZ in CircleZones)
            {
                CZ.CircleSafeZoneName = "Circle Zone " + i.ToString();
                i++;
            }
        }
        public void SetPolygonNames()
        {
            int i = 0;
            foreach (ExpansionSafeZonePolygon PZ in PolygonZones)
            {
                PZ.polygonSafeZoneName = "Polygon Zone " + i.ToString();
                i++;
            }
        }
        public void SetCylinderNames()
        {
            int i = 0;
            foreach (ExpansionSafeZoneCylinder PZ in CylinderZones)
            {
                PZ.CylinderSafeZoneName = "Cylinder Zone " + i.ToString();
                i++;
            }
        }
        public void Convertpolygonarrays()
        {
            foreach (ExpansionSafeZonePolygon PZ in PolygonZones)
            {
                PZ.Polygonpoints = new BindingList<Polygonpoints>();
                for (int i = 0; i < PZ.Positions.Count; i++)
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
            foreach (ExpansionSafeZonePolygon PZ in PolygonZones)
            {
                PZ.Positions = new BindingList<float[]>();
                foreach (Polygonpoints PGP in PZ.Polygonpoints)
                {
                    PZ.Positions.Add(PGP.points);
                }
            }
        }
        public void RemoveCircleZone(ExpansionSafeZoneCircle currentZone)
        {
            CircleZones.Remove(currentZone);
            isDirty = true;
        }
        public void RemovePolygonZone(ExpansionSafeZonePolygon currentpolygonZones)
        {
            PolygonZones.Remove(currentpolygonZones);
            isDirty = true;
        }
        public void AddNewCircleZones()
        {
            CircleZones.Add(new ExpansionSafeZoneCircle() { Center = new float[] { 0, 0, 0 }, Radius = 100 });
            SetCircleNames();
            isDirty = true;
        }

        public void RemovecylinderZone(ExpansionSafeZoneCylinder currentcylenderzones)
        {
            CylinderZones.Remove(currentcylenderzones);
            isDirty = true;
        }
    }
    // Zone types 
    // 1 = Circle
    // 2 = polygon
    // 3 = Cylinder
    public class ExpansionSafeZoneCircle
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
    public class ExpansionSafeZonePolygon
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
    public class ExpansionSafeZoneCylinder
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
