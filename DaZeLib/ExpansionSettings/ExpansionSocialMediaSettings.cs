using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class ExpansionSocialMediaSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public BindingList<ExpansionNewsFeedTextSetting> NewsFeedTexts { get; set; }
        public BindingList<ExpansionNewsFeedLinkSetting> NewsFeedLinks { get; set; }


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionSocialMediaSettings()
        {
            m_Version = CurrentVersion;
            DefaultNewsFeedTexts();
            DefaultNewsFeedLinks();
        }
        public void DefaultNewsFeedTexts()
        {
            NewsFeedTexts = new BindingList<ExpansionNewsFeedTextSetting>()
            {
                new ExpansionNewsFeedTextSetting()
                {
                    m_Title = "CHANGE ME",
                    m_Text = "THIS IS A PLACEHOLDER TEXT"
                }
            };
        }

        public void DefaultNewsFeedLinks()
        {
            NewsFeedLinks = new BindingList<ExpansionNewsFeedLinkSetting>()
            {
                new ExpansionNewsFeedLinkSetting()
                {
                    m_Label = "Discord",
                    m_Icon =  "set:expansion_iconset image:icon_discord",
                    m_URL = "https://www.google.com/"
                },
                new ExpansionNewsFeedLinkSetting()
                {
                    m_Label = "Twitter",
                    m_Icon =  "set:expansion_iconset image:icon_twitter",
                    m_URL = "https://www.google.com/"
                }
            };
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
    public class ExpansionNewsFeedTextSetting
    {
        public string m_Title { get; set; }
        public string m_Text { get; set; }

        public override string ToString()
        {
            return m_Title;
        }
    }

    public class ExpansionNewsFeedLinkSetting
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
