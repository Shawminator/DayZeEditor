using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ECMItemRulesConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<Cantbedamagedoutsidepvp> CantBeDamagedOutsidePvP { get; set; }
        public BindingList<string> CantDoDamageOutsidePvP { get; set; }
        public BindingList<string> CantBeDamagedAtAll { get; set; }
        public BindingList<string> CantBeUnpinnedOutsidePvP { get; set; }
    }

    public class Cantbedamagedoutsidepvp
    {
        public string itemName { get; set; }
        public int EnableDamageOnGround { get; set; }
    }

}
