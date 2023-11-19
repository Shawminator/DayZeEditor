using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionPlayerListSettings
    {
        const int CurrentVersion = 0;

        public int m_Version { get; set; } //current version is 0
        public int EnablePlayerList { get; set; }
        public int EnableTooltip { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionPlayerListSettings()
        {
            m_Version = CurrentVersion;
            EnablePlayerList = 1;
            EnableTooltip = 1;
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
    }
}
