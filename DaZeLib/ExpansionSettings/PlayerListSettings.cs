using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class PlayerListSettings
    {
        public int m_Version { get; set; } //current version is 0
        public int EnablePlayerList { get; set; }
        public int EnableTooltip { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public PlayerListSettings()
        {
            m_Version = 0;
            isDirty = true;
        }
    }
}
