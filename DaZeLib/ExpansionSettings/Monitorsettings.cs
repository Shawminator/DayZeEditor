using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class MonitoringSettings
    {
        public int m_Version { get; set; }
        public int Enabled { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public MonitoringSettings()
        {
            m_Version = 1;
            isDirty = true;
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);

        }
    }
}
