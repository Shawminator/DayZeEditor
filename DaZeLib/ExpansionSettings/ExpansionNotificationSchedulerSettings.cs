using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class ExpansionNotificationSchedulerSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int UTC { get; set; }
        public int UseMissionTime { get; set; }
        public BindingList<ExpansionNotificationSchedule> Notifications { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionNotificationSchedulerSettings()
        {
            m_Version = CurrentVersion;
            Enabled = 0;
            UTC = 0;
            UseMissionTime = 0;
            Notifications = new BindingList<ExpansionNotificationSchedule>()
            {
                new ExpansionNotificationSchedule()
                {
                    Hour = 22,
                    Minute = 00,
                    Second = 0,
                    Title = "Notification Schedule Test 1",
                    Text = "Lorem ipsum dolor sit amet",
                    Icon = "Info",
                    Color = "FFFFFFFF"
                },
                new ExpansionNotificationSchedule()
                {
                    Hour = 22,
                    Minute = 01,
                    Second = 0,
                    Title = "Notification Schedule Test 2",
                    Text = "Lorem ipsum dolor sit amet",
                    Icon = "Info",
                    Color = "FFFFFFFF"
                }
            };
        }
        public bool checknotificationcols()
        {
            bool returntype = false;
            foreach(ExpansionNotificationSchedule not in Notifications)
            {
                if (not.Color == "")
                {
                    not.Color = "FFFFFFFF";
                    isDirty = true;
                    returntype = true;
                }
            }
            return returntype;
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

    public class ExpansionNotificationSchedule
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public ExpansionNotificationSchedule()
        {
            Color = "FFFFFFFF";
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
