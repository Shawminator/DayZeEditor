using System.ComponentModel;
using System.IO;

namespace DayZeLib
{
    public class QuestObjectivesAIPatrol : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public ExpansionAIPatrol AISpawn { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }

        public QuestObjectivesAIPatrol()
        {

        }
        public override string ToString()
        {
            return ObjectiveText;
        }
        public override void SetVec3List()
        {
            AISpawn.Waypoints = new BindingList<float[]>();
            foreach (Vec3 point in AISpawn._waypoints)
            {
                AISpawn.Waypoints.Add(point.getfloatarray());
            }
        }
        public override void GetVec3List()
        {
            AISpawn._waypoints = new BindingList<Vec3>();
            foreach (float[] point in AISpawn.Waypoints)
            {
                AISpawn._waypoints.Add(new Vec3(point));
            }
        }
    }
}
