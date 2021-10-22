using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class SpawnerBubaku
    {
        public BindingList<BubakLocations> BubakLocations { get; set; }

        [JsonIgnore]
        public bool isDirty;
    }
    public class BubakLocations
    {
        public string name { get; set; }
        public string triggerpos { get; set; }
        public string triggermins { get; set; }
        public string triggermaxs { get; set; }
        public int triggerdelay  { get; set; }
        public BindingList<string> spawnerpos { get; set; }
        public int bubaknum { get; set; }
        public BindingList<string> bubaci { get; set; }
    }
}