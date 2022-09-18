using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class NotificationSchedulerSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int UTC { get; set; }
        public int UseMissionTime { get; set; }
        public BindingList<Notification> Notifications { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public NotificationSchedulerSettings()
        {
            m_Version = CurrentVersion;
            Notifications = new BindingList<Notification>(); 
            isDirty = true;
        }
        public bool checknotificationcols()
        {
            bool returntype = false;
            foreach(Notification not in Notifications)
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

    public class Notification
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public Notification()
        {
            Color = "FFFFFFFF";
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
