using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

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
