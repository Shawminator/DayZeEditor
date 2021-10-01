using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DayZeLib
{
    public class MissionSettingFiles
    {
        public int Enabled { get; set; }
        public float Weight { get; set; }
        public int MissionMaxTime { get; set; }
        public string MissionName { get; set; }
        public int Difficulty { get; set; }
        public int Objective { get; set; }
        public string Reward { get; set; }
        public int ShowNotification { get; set; }
        public float Height { get; set; }
        public float Speed { get; set; }
        public string Container { get; set; }
        public DropLocation DropLocation { get; set; }
        public BindingList<Empty> Loot { get;set;}
        public BindingList<Empty> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }


        [JsonIgnore]
        public string MissionPath { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public void SetIntValue(string mytype, int myvalue)
        {
            if (mytype == "MissionMaxTime")
                GetType().GetProperty(mytype).SetValue(this, (int)Helper.ConvertMinutesToSeconds(myvalue), null);
            else
                GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetFloatValue(string mytype, float myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class DropLocation
    {
        public float x { get; set; }
        public float z { get; set; }
        public string Name { get; set; }
        public float Radius { get; set; }
    }
    public class Empty{}
}
