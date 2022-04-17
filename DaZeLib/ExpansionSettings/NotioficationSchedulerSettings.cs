using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class NotificationSchedulerSettings
    {
        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int UTC { get; set; }
        public Notification[] Notifications { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public NotificationSchedulerSettings()
        {
            m_Version = 1;
            isDirty = true;
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

        public override string ToString()
        {
            return Title;
        }
    }
}
