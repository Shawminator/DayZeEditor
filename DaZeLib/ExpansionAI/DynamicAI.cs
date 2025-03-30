using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class Spatial_Notifications
    {
        public int Version { get; set; }
        public BindingList<Spatial_Notification> notification { get; set; }

        [JsonIgnore]
        const int VERSION = 2;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public void DefaultSpatial_Notificationsettings(out Spatial_Notifications Data)
        {
            Data = new Spatial_Notifications();
            Data.Version = VERSION;
            Data.notification = new BindingList<Spatial_Notification>();
            Data.notification.Add(new Spatial_Notification("East", (decimal)0.0, (decimal)24.0, (decimal)1.5, 1, "East AI", new BindingList<string>() { "Enemies nearby", "AI Detected: East", "Example 3" }));
            Data.notification.Add(new Spatial_Notification("West", (decimal)0.0, (decimal)24.0, (decimal)2.5, 3, "West AI", new BindingList<string>() { "Enemies nearby", "AI Detected: West" }));
            Data.notification.Add(new Spatial_Notification("Guard", (decimal)0.0, (decimal)24.0, (decimal)4.5, 5, "Guard AI", new BindingList<string>() { "Guards detected", "Friendlies in the area, guns down." }));
            Data.notification.Add(new Spatial_Notification("Disabled", (decimal)0.0, (decimal)0.0, (decimal)10.0, 0, "Disabled", new BindingList<string>() { "Disabled" }));

        }
        public bool checkver()
        {
            if (Version != VERSION)
            {
                Version = VERSION;
                isDirty = true;
                return true;
            }
            return false;
        }
    }
    public class Spatial_Notification
    {
        public string Spatial_Name { get; set; }
        public decimal StartTime { get; set; }
        public decimal StopTime { get; set; }
        public decimal AgeTime { get; set; }
        public int MessageType { get; set; }
        public string MessageTitle { get; set; }
        public BindingList<string> MessageText { get; set; }
        public Spatial_Notification()
        {

        }
        public Spatial_Notification(string a, decimal b, decimal c, decimal d, int e, string f, BindingList<string> g)
        {
            Spatial_Name = a;
            StartTime = b;
            StopTime = c;
            AgeTime = d;
            MessageType = e;
            MessageTitle = f;
            MessageText = g;
        }
        public override string ToString()
        {
            return Spatial_Name;
        }

    }

    public class Spatial_Players
    {
        public int Version { get; set; }
        public BindingList<Spatial_Player> Group { get; set; }

        [JsonIgnore]
        const int VERSION = 1;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public bool checkver()
        {
            if (Version != VERSION)
            {
                Version = VERSION;
                isDirty = true;
                return true;
            }
            return false;
        }
    }

    public class Spatial_Player
    {
        public string UID { get; set; }
        public int Player_Birthday { get; set; }

        public override string ToString()
        {
            return UID;
        }
    }

    public class Spatial_Groups
    {
        public int Version { get; set; }
        public decimal Spatial_MinTimer { get; set; }
        public decimal Spatial_MaxTimer { get; set; }
        public int MinDistance { get; set; }
        public int MaxDistance { get; set; }
        public int HuntMode { get; set; }
        public int Points_Enabled { get; set; }
        public int Locations_Enabled { get; set; }
        public int Audio_Enabled { get; set; }
        public decimal EngageTimer { get; set; }
        public decimal CleanupTimer { get; set; }
        public int PlayerChecks { get; set; }
        public int MaxAI { get; set; }
        public int GroupDifficulty { get; set; }
        public int MinimumPlayerDistance { get; set; }
        public int MaxSoloPlayers { get; set; }
        public int MinimumAge { get; set; }
        public int ActiveHoursEnabled { get; set; }
        public decimal ActiveStartTime { get; set; }
        public decimal ActiveStopTime { get; set; }
        public string TargetBone { get; set; }
        public int MessageType { get; set; }
        public string MessageTitle { get; set; }
        public string MessageText { get; set; }
        public BindingList<string> LootWhitelist { get; set; }
        public int Spatial_InVehicle { get; set; }
        public int Spatial_IsBleeding { get; set; }
        public int Spatial_IsRestrained { get; set; }
        public int Spatial_IsUnconscious { get; set; }
        public int Spatial_IsInSafeZone { get; set; }
        public int Spatial_TPSafeZone { get; set; }
        public int Spatial_InOwnTerritory { get; set; }
        public BindingList<Spatial_Group> Group { get; set; }
        public BindingList<Spatial_Point> Point { get; set; }
        public BindingList<Spatial_Location> Location { get; set; }
        public BindingList<Spatial_Audio> Audio { get; set; }

        [JsonIgnore]
        const int VERSION = 20;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public Spatial_Groups()
        {
            LootWhitelist = new BindingList<string>();
            Group = new BindingList<Spatial_Group>();
            Point = new BindingList<Spatial_Point>();
            Location = new BindingList<Spatial_Location>();
            Audio = new BindingList<Spatial_Audio>();
        }
        public bool checkver()
        {
            if (Version != VERSION)
            {
                Version = VERSION;
                isDirty = true;
                return true;
            }
            return false;
        }
        public void setAllPoints()
        {
            foreach (Spatial_Point point in Point)
            {
                point.setSpatialPosition();
            }
            foreach (Spatial_Location point in Location)
            {
                point.setSpatialTriggerPosition();
                point.setSpatialSpawnPosition();
            }
            foreach (Spatial_Audio point in Audio)
            {
                point.setSpatialTriggerPosition();
                point.setSpatialSpawnPosition();
            }
        }
        public void getAllPoints()
        {
            foreach (Spatial_Point point in Point)
            {
                point.getSpatialPosition();
            }
            foreach (Spatial_Location point in Location)
            {
                point.getSpatialTriggerPosition();
                point.getSpatialSpawnPosition();
            }
            foreach (Spatial_Audio point in Audio)
            {
                point.getSpatialTriggerPosition();
                point.getSpatialSpawnPosition();
            }
        }

        public void AddnewPoint(DZE Importfile)
        {
            Spatial_Point newpoint = new Spatial_Point()
            {
                Spatial_Name = "New Point",
                Spatial_Safe = 0,
                Spatial_Radius = 100,
                Spatial_ZoneLoadout = new BindingList<string>() { "HumanLoadout.json" },
                Spatial_MinCount = 1,
                Spatial_MaxCount = 3,
                Spatial_HuntMode = 5,
                Spatial_Faction = "Mercenaries",
                Spatial_Lootable = 1,
                Spatial_Chance = (decimal)1.0,
                Spatial_MinAccuracy = (decimal)0.75,
                Spatial_MaxAccuracy = (decimal)0.95,
                Spatial_UnlimitedReload = 0
            };
            newpoint.ImportDZE(Importfile);
            Point.Add(newpoint);
            isDirty = true;
        }

        public void AddnewLocation(DZE Importfile)
        {
            Spatial_Location newlocation = new Spatial_Location()
            {
                Spatial_Name = "New Location",
                Spatial_TriggerRadius = 100,
                Spatial_ZoneLoadout = "HumanLoadout.json",
                Spatial_MinCount = 1,
                Spatial_MaxCount = 3,
                Spatial_HuntMode = 5,
                Spatial_Faction = "Mercenaries",
                Spatial_Lootable = 1,
                Spatial_Chance = (decimal)1.0,
                Spatial_MinAccuracy = (decimal)0.75,
                Spatial_MaxAccuracy = (decimal)0.95,
                Spatial_Timer = (decimal)20,
                Spatial_SpawnMode = 1,
                Spatial_UnlimitedReload = 0
            };
            newlocation.ImportDZE(Importfile, true);
            Location.Add(newlocation);
            isDirty = true;
        }

        public void AddNewAudio(DZE Importfile)
        {
            Spatial_Audio newaudio = new Spatial_Audio()
            {
                Spatial_Name = "New Audio",
                Spatial_TriggerRadius = 100,
                Spatial_ZoneLoadout = "HumanLoadout.json",
                Spatial_MinCount = 1,
                Spatial_MaxCount = 3,
                Spatial_HuntMode = 5,
                Spatial_Faction = "Mercenaries",
                Spatial_Lootable = 1,
                Spatial_Chance = (decimal)1.0,
                Spatial_MinAccuracy = (decimal)0.75,
                Spatial_MaxAccuracy = (decimal)0.95,
                Spatial_Timer = (decimal)20,
                Spatial_Sensitivity = (decimal)3.0,
                Spatial_SpawnMode = 1,
                Spatial_UnlimitedReload = 0
            };
            newaudio.ImportDZE(Importfile, true);
            Audio.Add(newaudio);
            isDirty = true;
        }
    }

    public class Spatial_Group
    {
        public int Spatial_MinCount { get; set; }
        public int Spatial_MaxCount { get; set; }
        public decimal Spatial_Weight { get; set; }
        public string Spatial_Loadout { get; set; }
        public string Spatial_Faction { get; set; }
        public string Spatial_Name { get; set; }
        public int Spatial_Lootable { get; set; }
        public decimal Spatial_Chance { get; set; }
        public decimal Spatial_MinAccuracy { get; set; }
        public decimal Spatial_MaxAccuracy { get; set; }
        public int Spatial_UnlimitedReload { get; set; }
        public override string ToString()
        {
            return Spatial_Name;
        }
    }

    public class Spatial_Point
    {
        public string Spatial_Name { get; set; }
        public int Spatial_Safe { get; set; }
        public decimal Spatial_Radius { get; set; }
        public BindingList<string> Spatial_ZoneLoadout { get; set; }
        public int Spatial_MinCount { get; set; }
        public int Spatial_MaxCount { get; set; }
        public int Spatial_HuntMode { get; set; }
        public string Spatial_Faction { get; set; }
        public int Spatial_Lootable { get; set; }
        public decimal Spatial_Chance { get; set; }
        public decimal Spatial_MinAccuracy { get; set; }
        public decimal Spatial_MaxAccuracy { get; set; }
        public int Spatial_UnlimitedReload { get; set; }
        public float[] Spatial_Position { get; set; }

        [JsonIgnore]
        public Vec3 _Spatial_Position { get; set; }
        public void setSpatialPosition()
        {
            Spatial_Position = _Spatial_Position.getfloatarray();
        }
        public void getSpatialPosition()
        {
            _Spatial_Position = new Vec3(Spatial_Position.ToArray());
        }
        public override string ToString()
        {
            return Spatial_Name;
        }
        public void ImportDZE(DZE Importfile)
        {
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if (eo.DisplayName == "GiftBox_Large_1")
                {
                    _Spatial_Position = new Vec3(eo.Position);
                }
            }
        }
        public void ImportMap(string[] fileContent)
        {
            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] == "") continue;
                string[] linesplit = fileContent[i].Split('|');
                string[] XYZ = linesplit[1].Split(' ');
                if (linesplit[0] == "GiftBox_Large_1")
                {
                    _Spatial_Position = new Vec3(XYZ);
                }
            }
        }

        public void ImportOpbjectSpawner(ObjectSpawnerArr newobjectspawner)
        {
            foreach (SpawnObjects so in newobjectspawner.Objects)
            {
                if (so.name == "GiftBox_Large_1")
                {
                    _Spatial_Position = new Vec3(so.pos);
                }
            }
        }
    }

    public class Spatial_Location
    {
        public string Spatial_Name { get; set; }
        public decimal Spatial_TriggerRadius { get; set; }
        public string Spatial_ZoneLoadout { get; set; }
        public int Spatial_MinCount { get; set; }
        public int Spatial_MaxCount { get; set; }
        public int Spatial_HuntMode { get; set; }
        public string Spatial_Faction { get; set; }
        public int Spatial_Lootable { get; set; }
        public decimal Spatial_Chance { get; set; }
        public decimal Spatial_MinAccuracy { get; set; }
        public decimal Spatial_MaxAccuracy { get; set; }
        public decimal Spatial_Timer { get; set; }
        public int Spatial_SpawnMode { get; set; }
        public int Spatial_UnlimitedReload { get; set; }
        public float[] Spatial_TriggerPosition { get; set; }
        public BindingList<float[]> Spatial_SpawnPosition { get; set; }

        [JsonIgnore]
        public Vec3 _Spatial_TriggerPosition { get; set; }
        [JsonIgnore]
        public BindingList<Vec3> _Spatial_SpawnPosition { get; set; }

        public void setSpatialTriggerPosition()
        {
            Spatial_TriggerPosition = _Spatial_TriggerPosition.getfloatarray();
        }
        public void getSpatialTriggerPosition()
        {
            _Spatial_TriggerPosition = new Vec3(Spatial_TriggerPosition.ToArray());
        }
        public void setSpatialSpawnPosition()
        {
            Spatial_SpawnPosition = new BindingList<float[]>();
            foreach (Vec3 v3 in _Spatial_SpawnPosition)
            {
                Spatial_SpawnPosition.Add(v3.getfloatarray());
            }
        }
        public void getSpatialSpawnPosition()
        {
            _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (float[] Array in Spatial_SpawnPosition)
            {
                _Spatial_SpawnPosition.Add(new Vec3(Array));
            }
        }
        public override string ToString()
        {
            return Spatial_Name;
        }

        public void ImportDZE(DZE Importfile, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if (eo.DisplayName == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(eo.Position);
                    }
                }
                else if (eo.DisplayName == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(eo.Position));
                }
            }
        }
        public void ImportMap(string[] fileContent, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] == "") continue;
                string[] linesplit = fileContent[i].Split('|');
                string[] XYZ = linesplit[1].Split(' ');
                if (linesplit[0] == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(XYZ);
                    }
                }
                else if (linesplit[0] == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(XYZ));
                }
            }
        }

        public void ImportOpbjectSpawner(ObjectSpawnerArr newobjectspawner, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (SpawnObjects so in newobjectspawner.Objects)
            {
                if (so.name == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(so.pos);
                    }
                }
                else if (so.name == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(so.pos));
                }
            }
        }
    }

    public class Spatial_Audio
    {
        public string Spatial_Name { get; set; }
        public decimal Spatial_TriggerRadius { get; set; }
        public string Spatial_ZoneLoadout { get; set; }
        public int Spatial_MinCount { get; set; }
        public int Spatial_MaxCount { get; set; }
        public int Spatial_HuntMode { get; set; }
        public string Spatial_Faction { get; set; }
        public int Spatial_Lootable { get; set; }
        public decimal Spatial_Chance { get; set; }
        public decimal Spatial_MinAccuracy { get; set; }
        public decimal Spatial_MaxAccuracy { get; set; }
        public decimal Spatial_Timer { get; set; }
        public decimal Spatial_Sensitivity { get; set; }
        public int Spatial_SpawnMode { get; set; }
        public int Spatial_UnlimitedReload { get; set; }
        public float[] Spatial_TriggerPosition { get; set; }
        public BindingList<float[]> Spatial_SpawnPosition { get; set; }

        [JsonIgnore]
        public Vec3 _Spatial_TriggerPosition { get; set; }
        [JsonIgnore]
        public BindingList<Vec3> _Spatial_SpawnPosition { get; set; }
        public void setSpatialTriggerPosition()
        {
            Spatial_TriggerPosition = _Spatial_TriggerPosition.getfloatarray();
        }
        public void getSpatialTriggerPosition()
        {
            _Spatial_TriggerPosition = new Vec3(Spatial_TriggerPosition.ToArray());
        }
        public void setSpatialSpawnPosition()
        {
            Spatial_SpawnPosition = new BindingList<float[]>();
            foreach (Vec3 v3 in _Spatial_SpawnPosition)
            {
                Spatial_SpawnPosition.Add(v3.getfloatarray());
            }
        }
        public void getSpatialSpawnPosition()
        {
            _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (float[] Array in Spatial_SpawnPosition)
            {
                _Spatial_SpawnPosition.Add(new Vec3(Array));
            }
        }
        public override string ToString()
        {
            return Spatial_Name;
        }
        public void ImportDZE(DZE Importfile, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if (eo.DisplayName == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(eo.Position);
                    }
                }
                else if (eo.DisplayName == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(eo.Position));
                }
            }
        }
        public void ImportMap(string[] fileContent, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] == "") continue;
                string[] linesplit = fileContent[i].Split('|');
                string[] XYZ = linesplit[1].Split(' ');
                if (linesplit[0] == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(XYZ);
                    }
                }
                else if (linesplit[0] == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(XYZ));
                }
            }
        }
        public void ImportOpbjectSpawner(ObjectSpawnerArr newobjectspawner, bool importTrigger)
        {
            if (_Spatial_SpawnPosition == null)
                _Spatial_SpawnPosition = new BindingList<Vec3>();
            foreach (SpawnObjects so in newobjectspawner.Objects)
            {
                if (so.name == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _Spatial_TriggerPosition = new Vec3(so.pos);
                    }
                }
                else if (so.name == "GiftBox_Small_1")
                {
                    _Spatial_SpawnPosition.Add(new Vec3(so.pos));
                }
            }
        }
    }

}
