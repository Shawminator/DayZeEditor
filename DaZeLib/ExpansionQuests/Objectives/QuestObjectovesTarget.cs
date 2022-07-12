using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesTarget: QuestObjectivesBase
    {
        public float[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public Target Target { get; set; }

        public QuestObjectivesTarget()
        {
            Target = new Target();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Target
    {
        [JsonIgnore]
        string TargetName { get; set; }

        public int Amount { get; set; }
        public BindingList<string> ClassNames { get; set; }
        public int SpecialWeapon { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }

        public Target() 
        {
            ClassNames = new BindingList<string>();
            AllowedWeapons = new BindingList<string>();
        }
        public override string ToString()
        {
            return TargetName;
        }
    }

}
