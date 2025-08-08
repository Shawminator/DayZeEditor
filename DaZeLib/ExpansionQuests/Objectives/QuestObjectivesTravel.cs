namespace DayZeLib
{
    public class QuestObjectivesTravel : QuestObjectivesBase
    {
        public override int ConfigVersion { get; set; }
        public override int ID { get; set; }
        public override int ObjectiveType { get; set; }
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public decimal[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int TriggerOnEnter { get; set; }
        public int TriggerOnExit { get; set; }

        public QuestObjectivesTravel() { }
        public override string ToString()
        {
            return ObjectiveText;
        }

    }

}
