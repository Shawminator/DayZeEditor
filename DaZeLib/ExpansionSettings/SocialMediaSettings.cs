using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class SocialMediaSettings
    {
        const int CurrentVersion = 0;

        public int m_Version { get; set; }
        public string Discord { get; set; }
        public string Homepage { get; set; }
        public string Forums { get; set; }
        public string YouTube { get; set; }
        public string Steam { get; set; }
        public string Twitter { get; set; }
        public string Guilded { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public SocialMediaSettings()
        {
            m_Version = CurrentVersion;
            isDirty = true;
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

        public void SetStringValue(string mytype, string myString)
        {
            GetType().GetProperty(mytype).SetValue(this, myString, null);
        }
    }
}
