using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesAIPatrol: QuestObjectivesBase
    {
        public Aipatrol AIPatrol { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set; }

        public QuestObjectivesAIPatrol()
        {
            AIPatrol = new Aipatrol();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Aipatrol
    {
        public int NPCUnits { get; set; }
        public BindingList<float[]> Waypoints { get; set; }
        public string NPCSpeed { get; set; }
        public string NPCMode { get; set; }
        public string NPCFaction { get; set; }
        public string NPCLoadoutFile { get; set; }
        public BindingList<string> ClassNames { get; set; }
        public int SpecialWeapon { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }

        public Aipatrol()
        {
            Waypoints = new BindingList<float[]>();
            ClassNames = new BindingList<string>();
            AllowedWeapons = new BindingList<string>();
        }
        public override string ToString()
        {
            return NPCFaction;
        }
    }

}
