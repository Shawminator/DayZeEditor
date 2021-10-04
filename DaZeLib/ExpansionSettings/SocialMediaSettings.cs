using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class SocialMediaSettings
    {
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
            m_Version = 0;
            isDirty = true;
        }

        public void SetStringValue(string mytype, string myString)
        {
            GetType().GetProperty(mytype).SetValue(this, myString, null);
        }
    }
}
