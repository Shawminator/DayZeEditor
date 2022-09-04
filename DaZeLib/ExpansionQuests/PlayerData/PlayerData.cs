using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public enum ExpansionQuestState
    {
        NONE = 0,
        STARTED = 1,
        CAN_TURNIN = 2,
        COMPLETED = 3
    };
    public class QuestPlayerData
    {
        public string Filename { get; set; }
        public bool isDirty { get; set; }

        const int CONFIGVERSION = 1;
        public int ExpansionQuestPersistentQuestDataCount;
        public BindingList<ExpansionQuestPersistentQuestData> QuestDatas;

       public QuestPlayerData(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (br.ReadInt32() != CONFIGVERSION) return;
                QuestDatas = new BindingList<ExpansionQuestPersistentQuestData>();
                ExpansionQuestPersistentQuestDataCount = br.ReadInt32();
                for(int i = 0; i < ExpansionQuestPersistentQuestDataCount; i ++)
                {
                    QuestDatas.Add(new ExpansionQuestPersistentQuestData(br));
                }
                long pos = br.BaseStream.Position;
            }
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }

    public class ExpansionQuestPersistentQuestData
    {
        public int QuestID = -1;
        public ExpansionQuestState State = ExpansionQuestState.NONE;
        public int Timestamp = -1;
        public int QuestObjectivesCount;
        public BindingList<ExpansionQuestObjectiveData> QuestObjectives;

        public Quests AssignedQuestinfo;

        public ExpansionQuestPersistentQuestData(BinaryReader br)
        {
            QuestID = br.ReadInt32();
            State = (ExpansionQuestState)br.ReadInt32();
            Timestamp = br.ReadInt32();
            QuestObjectivesCount = br.ReadInt32();
            QuestObjectives = new BindingList<ExpansionQuestObjectiveData>();
            for(int i = 0; i < QuestObjectivesCount; i++)
            {
                QuestObjectives.Add(new ExpansionQuestObjectiveData(br));
            }
        }
        public override string ToString()
        {
            return "Quest ID : " + QuestID;
        }
    }
    public class ExpansionQuestObjectiveData
    {
        public int ObjectiveIndex = -1;
        public QuExpansionQuestObjectiveTypeestType ObjectiveType = QuExpansionQuestObjectiveTypeestType.NONE;
        public bool IsCompleted = false;
        public bool IsActive = false;
        public int ObjectiveAmount = -1;
        public int ObjectiveCount = -1;
        public float[] ObjectivePosition;

        public bool ActionState = false;
        public int TimeLimit = -1;

        public QuestObjectivesBase AssignedObjective;

        public ExpansionQuestObjectiveData(BinaryReader br)
        {
            ObjectiveIndex = br.ReadInt32();
            ObjectiveType = (QuExpansionQuestObjectiveTypeestType)br.ReadInt32();
            IsCompleted = br.ReadInt32() == 1 ? true : false;
            IsActive = br.ReadInt32() == 1 ? true : false;
            ObjectiveAmount = br.ReadInt32();
            ObjectiveCount = br.ReadInt32();
            int loop = br.ReadInt32();
            ObjectivePosition = new float[loop];
            for (int i = 0; i < loop; i++)
            {
                ObjectivePosition[i] = br.ReadSingle();
            }
            ActionState = br.ReadInt32() == 1 ? true : false;
            TimeLimit = br.ReadInt32();

        }
        public override string ToString()
        {
            if (AssignedObjective == null)
                return "This Querst is Broken, please remove from Player Data......";
            else
                return AssignedObjective.ToString();
        }
    }
}
