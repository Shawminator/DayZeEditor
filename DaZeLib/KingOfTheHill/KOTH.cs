using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public BindingList<object> lootSets { get; set; }
        public BindingList<MDCKOTHZones> zones { get; set; }
    }

    public class MDCKOTHZones
    {
        public string zoneName { get; set; }
        public float[] zonePosition { get; set; }
        public int zoneRadius { get; set; }
        public float baseCaptureTime { get; set; }
        public float playerTimeMultiplier { get; set; }
        public float timeDespawn { get; set; }
        public float timeLimit { get; set; }
        public float timeStart { get; set; }
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
        public BindingList<KOTHLootset> lootSets { get; set; }

        public override string ToString()
        {
            return zoneName;
        }
    }

    public class KOTHObject
    {
        public string classname { get; set; }
        public float[] position { get; set; }
        public float[] orientation { get; set; }
        public int absolutePlacement { get; set; }
        public int alignToTerrain { get; set; }
        public int placeOnSurface { get; set; }
    }



    public class MDCKOTHLoot
    {
        [JsonIgnore]
        public string FullFilename;
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KOTHLootset[] lootSets { get; set; }
    }

    public class KOTHLootset
    {
        public string name { get; set; }
        public BindingList<KOTHItem> items { get; set; }
    }

    public class KOTHItem
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public BindingList<KOTHItem> attachments { get; set; }
        public BindingList<KOTHItem> cargo { get; set; }
    }
}


