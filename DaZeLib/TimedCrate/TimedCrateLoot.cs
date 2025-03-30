using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class TimedCrateLoot
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public BindingList<Crate> Crates { get; set; }
    }

    public class Crate
    {
        public string cratetype { get; set; }
        public int numberOfItemsToSpawn { get; set; }
        public int IsRandom { get; set; }
        public BindingList<List> List { get; set; }
    }

    public class List
    {
        public string item { get; set; }
        public BindingList<string> attachments { get; set; }
        public int quantity { get; set; }
    }

}
