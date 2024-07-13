using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class InediaMovementAdmins_Config
    {
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public string Filename { get; set; }

        public BindingList<string> AdminsSteamIds { get; set; }

    }
}
