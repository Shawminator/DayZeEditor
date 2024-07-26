using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesAICamp : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public decimal InfectedDeletionRadius { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }
        public BindingList<ExpansionAIPatrol> AISpawns { get; set; }

        public override void SetVec3List()
        {
            foreach (ExpansionAIPatrol aispawn in AISpawns)
            {
                aispawn.Waypoints = new BindingList<float[]>();
                foreach (Vec3 point in aispawn._waypoints)
                {
                    aispawn.Waypoints.Add(point.getfloatarray());
                }
            }
        }
        public override void GetVec3List()
        {
            foreach (ExpansionAIPatrol aispawn in AISpawns)
            {
                aispawn._waypoints = new BindingList<Vec3>();
                foreach (float[] point in aispawn.Waypoints)
                {
                    aispawn._waypoints.Add(new Vec3(point));
                }
            }
        }

        public QuestObjectivesAICamp()
        {
            AISpawns = new BindingList<ExpansionAIPatrol>();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }



}
