using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class MDCKOTH
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
        public float baseCaptureTime { get; set; }
        public float maxTimeBetweenEvents { get; set; }
        public float minTimeBetweenEvents { get; set; }
        public float playerTimeMultiplier { get; set; }
        public float timeDespawn { get; set; }
        public float timeLimit { get; set; }
        public float timeStart { get; set; }
        public float timeSpawn { get; set; }
        public float timeZoneCooldown { get; set; }
        public int minPlayerCount { get; set; }
        public int maxEnemyCount { get; set; }
        public int minEnemyCount { get; set; }
        public int maxEvents { get; set; }
        public int minimumDeaths { get; set; }
        public int minimumPlayers { get; set; }
        public int maximumPlayers { get; set; }
        public int rewardCount { get; set; }
        public string flagClassname { get; set; }
        public string[] enemies { get; set; }
        public string lootCrate { get; set; }
        public int crateLifeTime { get; set; }
        public object[] lootSets { get; set; }
        public Zone[] zones { get; set; }
    }

    public class Zone
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
        public KOTHObject[] objects { get; set; }
        public object[] enemies { get; set; }
        public string lootCrate { get; set; }
        public int crateLifeTime { get; set; }
        public object[] lootSets { get; set; }
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



    public class KingOfTheHillConfig
    {
        [JsonIgnore]
        public string FullFilename;
        [JsonIgnore]
        public bool isDirty { get; set; }


        public string m_ModVersion { get; set; }
        public decimal m_CaptureTime { get; set; }
        public decimal m_UpdateInterval { get; set; }
        public decimal m_ServerStartDelay { get; set; }
        public decimal m_HillEventInterval { get; set; }
        public decimal m_EventCleanupTime { get; set; }
        public decimal m_EventPreStart { get; set; }
        public string m_EventPreStartMessage { get; set; }
        public string m_EventCapturedMessage { get; set; }
        public string m_EventDespawnedMessage { get; set; }
        public string m_EventStartMessage { get; set; }
        public int m_DoLogsToCF { get; set; }
        public int m_PlayerPopulationToStartEvents { get; set; }
        public int m_MaxEvents { get; set; }
        public string m_FlagName { get; set; }
        public BindingList<M_Hilllocations> m_HillLocations { get; set; }
        public BindingList<M_Rewardpools> m_RewardPools { get; set; }
        public BindingList<string> m_Creatures { get; set; }
    }

    public class M_Hilllocations
    {
        public string Name { get; set; }
        public decimal Radius { get; set; }
        public float[] Position { get; set; }
        public int AISpawnCount { get; set; }
        public string ObjectListName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    public class M_Rewardpools
    {
        public string RewardContainerName { get; set; }
        public BindingList<M_Rewards> m_Rewards { get; set; }

        public override string ToString()
        {
            return RewardContainerName;
        }
    }

    public class M_Rewards
    {
        public string ItemName { get; set; }
        public BindingList<string> Attachments { get; set; }

        public override string ToString()
        {
            return ItemName;
        }
    }
}
