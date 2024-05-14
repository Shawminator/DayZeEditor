using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class MDCKOTHConfig
    {
        [JsonIgnore]
        public string FullFilename;
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int enabled { get; set; }
        public int loggingLevel { get; set; }
        public int useLocationText { get; set; }
        public int useMapMarker { get; set; }
        public int useNotifications { get; set; }
        public int reduceProgressOnAbandoned { get; set; }
        public int reduceProgressOnDeathFromOutside { get; set; }
        public int requireFlagConstruction { get; set; }
        public int estimateLocation { get; set; }
        public int celebrateWin { get; set; }
        public int punishLoss { get; set; }
        public decimal baseCaptureTime { get; set; }
        public decimal maxTimeBetweenEvents { get; set; }
        public decimal minTimeBetweenEvents { get; set; }
        public decimal playerTimeMultiplier { get; set; }
        public decimal timeDespawn { get; set; }
        public decimal timeLimit { get; set; }
        public decimal timeStart { get; set; }
        public decimal timeSpawn { get; set; }
        public decimal timeZoneCooldown { get; set; }
        public int minPlayerCount { get; set; }
        public int maxEnemyCount { get; set; }
        public int minEnemyCount { get; set; }
        public int maxEvents { get; set; }
        public int minimumDeaths { get; set; }
        public int minimumPlayers { get; set; }
        public int maximumPlayers { get; set; }
        public int rewardCount { get; set; }
        public string flagClassname { get; set; }
        public BindingList<string> enemies { get; set; }
        public string lootCrate { get; set; }
        public int crateLifeTime { get; set; }
        public BindingList<string> lootSets { get; set; }
        public BindingList<MDCKOTHZones> zones { get; set; }
    }

    public class MDCKOTHZones
    {
        public string zoneName { get; set; }
        public float[] zonePosition { get; set; }
        public int zoneRadius { get; set; }
        public decimal baseCaptureTime { get; set; }
        public decimal playerTimeMultiplier { get; set; }
        public decimal timeDespawn { get; set; }
        public decimal timeLimit { get; set; }
        public decimal timeStart { get; set; }
        public int maxEnemyCount { get; set; }
        public int minEnemyCount { get; set; }
        public int minimumDeaths { get; set; }
        public int minimumPlayers { get; set; }
        public int maximumPlayers { get; set; }
        public int rewardCount { get; set; }
        public string flagClassname { get; set; }
        public BindingList<KOTHObject> objects { get; set; }
        public BindingList<string> enemies { get; set; }
        public string lootCrate { get; set; }
        public int crateLifeTime { get; set; }
        public BindingList<String> lootSets { get; set; }

        public MDCKOTHZones()
        {
            zoneName = "";
            zonePosition = new float[] { 0, 0, 0 };
            zoneRadius = 50;
            baseCaptureTime = -1;
            playerTimeMultiplier = -1;
            timeDespawn = -1;
            timeLimit = -1;
            timeStart = -1;
            maxEnemyCount = -1;
            minEnemyCount = -1;
            minimumDeaths = -1;
            minimumPlayers = -1;
            maximumPlayers = -1;
            rewardCount = -1;
            flagClassname = "";
            lootCrate = "";
            crateLifeTime = -1;
            objects = new BindingList<KOTHObject>();
            enemies = new BindingList<string>();
            lootSets = new BindingList<string>();
        }

        public override string ToString()
        {
            return zoneName;
        }
    }

    public class KOTHObject
    {
        public string classname { get; set; }
        public decimal[] position { get; set; }
        public decimal[] orientation { get; set; }
        public int absolutePlacement { get; set; }
        public int alignToTerrain { get; set; }
        public int placeOnSurface { get; set; }

        public override string ToString()
        {
            return classname;
        }
    }



    public class MDCKOTHLoot
    {
        [JsonIgnore]
        public string FullFilename;
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<KOTHLootset> lootSets { get; set; }

        public KOTHLootset getlootsetbyname(string name)
        {
            return lootSets.FirstOrDefault(x => x.name == name);
        }
    }

    public class KOTHLootset
    {
        public string name { get; set; }
        public BindingList<KOTHItem> items { get; set; }

        public override string ToString()
        {
            return name;
        }
        public KOTHLootset Clone()
        {
            var clonedJson = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<KOTHLootset>(clonedJson);
        }
    }

    public class KOTHItem
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public BindingList<KOTHItem> attachments { get; set; }
        public BindingList<KOTHItem> cargo { get; set; }
    }
}


