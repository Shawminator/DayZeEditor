using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class QuestPersistentServerData
    {
        [JsonIgnore]
        public const int m_currentConfigVersion = 1;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

        public int ConfigVersion { get; set; }
        public BindingList<Object> m_QuestMarketItems { get; set; }

        public QuestPersistentServerData()
        {
            ConfigVersion = m_currentConfigVersion;
            m_QuestMarketItems = new BindingList<object>();
        }

        public bool checkver()
        {
            if (ConfigVersion != m_currentConfigVersion)
            {
                ConfigVersion = m_currentConfigVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
    }

}
