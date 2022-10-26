using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class SocialMediaSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public BindingList<Newsfeedtext> NewsFeedTexts { get; set; }
        public BindingList<Newsfeedlink> NewsFeedLinks { get; set; }


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
    public class Newsfeedtext
    {
        public string m_Title { get; set; }
        public string m_Text { get; set; }

        public override string ToString()
        {
            return m_Title;
        }
    }

    public class Newsfeedlink
    {
        public string m_Label { get; set; }
        public string m_Icon { get; set; }
        public string m_URL { get; set; }

        public override string ToString()
        {
            return m_Label;
        }
    }
}
