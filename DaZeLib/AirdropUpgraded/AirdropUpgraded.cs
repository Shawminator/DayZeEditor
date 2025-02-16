using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class AirdropUpgraded
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public Controls Controls { get; set; }
        public Map Map { get; set; }
        public Aircraft Aircraft { get; set; }
        public Messages Messages { get; set; }
        public Container Container { get; set; }
        public BindingList<Location> Locations { get; set; }
        public BindingList<Dropzone> DropZones { get; set; }
        public BindingList<Droptype> DropTypes { get; set; }

        public AirdropUpgraded()
        {
            Locations = new BindingList<Location>();
            DropZones = new BindingList<Dropzone>();
            DropTypes = new BindingList<Droptype>();
        }
    }

    public class Controls
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public int Interval { get; set; }
        public int Variance { get; set; }
        public BindingList<int> FlightHours { get;set; }
        public int Mode { get; set; }
        public int AD_LogManager { get; set; }
        public int AD_LogAircraft { get; set; }
        public int AD_LogContainer { get; set; }
        public int MinimumPlayers { get; set; }
        public int MaxBackupDays { get; set; }
        public int MaxLogDays { get; set; }
        public int SmokeTrails { get; set; }
    }

    public class Map
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Offset { get; set; }
    }

    public class Aircraft
    {
        public int AirSpeedKIAS { get; set; }
        public int StartAltMSL { get; set; }
        public int DropAGL { get; set; }
        public int DropOffset { get; set; }
        public int DropAccuracy { get; set; }
        public decimal TerrainFollowing { get; set; }
    }

    public class Messages
    {
        public int Mode { get; set; }
        public int Duration { get; set; }
        public int Proximity { get; set; }
        public int ImperialUnits { get; set; }
        public int TitlePostfixMode { get; set; }
        public string Dispatched_S { get; set; }
        public string Dispatched_C { get; set; }
        public string Proximity_S { get; set; }
        public string Proximity_C { get; set; }
        public string Released_S { get; set; }
        public string Released_C { get; set; }

    }

    public class Container
    {
        public decimal TriggerAGL { get; set; }
        public decimal FallRate { get; set; }
        public decimal StandUpTimer { get; set; }
        public decimal SpawnMin { get; set; }
        public decimal SpawnMax { get; set; }
        public decimal SpawnOffset { get; set; }
        public decimal WindStrength { get; set; }
        public int Lifespan { get; set; }
    }

    public class Location
    {
        public string Title { get; set; }
        public BindingList<string> Zombies { get; set; }

        public Location()
        {
            Zombies = new BindingList<string>();
        }

        public override string ToString()
        {
            return Title;
        }
    }

    public class Dropzone
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string DropType { get; set; }
        public decimal X { get; set; }
        public decimal Z { get; set; }
        public int Zombies { get; set; }
        public int Radius { get; set; }
        public int DropAccuracy { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public class Droptype
    {
        public string Title { get; set; }
        public string Container { get; set; }
        public int Quantity { get; set; }
        public decimal AddFlare { get; set; }
        public decimal SpawnMin { get; set; }
        public decimal SpawnMax { get; set; }
        public decimal SpawnOffset { get; set; }
        public ItemCondition ItemCondition { get; set; }
        public int Lifespan { get; set; }   
        public BindingList<string> Items { get; set; }

        public Droptype()
        {
            ItemCondition = new ItemCondition();
            Items = new BindingList<string>();
        }

        public override string ToString()
        {
            return Title;
        }
    }
    public class ItemCondition
    {
        public int MinCondition { get; set; }
        public int MaxCondition { get; set; }
        public int Samples { get; set; }
    }
    public class VPP_Map
    {
        public int ExportMap { get; set; }
        public int TitleMode { get; set; }
        public int[] MapColor { get; set; }
        public string MapIcon { get; set; }
        public int Isactive { get; set; }
        public int Is3DActive { get; set; }

        public VPP_Map()
        {
            MapColor = new int[] { 0, 0, 0 };
        }
    }
}
