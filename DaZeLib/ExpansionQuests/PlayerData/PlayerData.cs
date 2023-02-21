using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public enum ExpansionQuestState
    {
        NONE = 0,
        STARTED = 1,
        CAN_TURNIN = 2,
        COMPLETED = 3
    };
    public class QuestPersistantDataPlayersList
    {
        private string m_questPlayerDataPath;
        public BindingList<QuestPlayerData> QuestPlayerDataList { get; private set; }
        public List<QuestPlayerData> Markedfordelete { get; set; }

        public QuestPersistantDataPlayersList(string questPlayerDataPath)
        {
            m_questPlayerDataPath = questPlayerDataPath;
            QuestPlayerDataList = new BindingList<QuestPlayerData>();
            DirectoryInfo d = new DirectoryInfo(m_questPlayerDataPath);
            FileInfo[] Files = d.GetFiles();
            foreach (FileInfo file in Files)
            {
                try
                {
                    QuestPlayerData QuestPlayerData = new QuestPlayerData(file.FullName);
                    QuestPlayerData.Filename = Path.GetFileNameWithoutExtension(file.Name); ;
                    QuestPlayerData.isDirty = false;
                    QuestPlayerDataList.Add(QuestPlayerData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                }
            }
        }
        public void deletePlayerData(QuestPlayerData Playerdata)
        {
            if (Markedfordelete == null) Markedfordelete = new List<QuestPlayerData>();
            Markedfordelete.Add(Playerdata);
            QuestPlayerDataList.Remove(Playerdata);
        }

    }


    public class QuestPlayerData
    {
        public string Filename { get; set; }
        public bool isDirty { get; set; }

        const int CONFIGVERSION = 2;
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
                    if(i == 5)
                    {
                        string stop = "";
                    }
                    QuestDatas.Add(new ExpansionQuestPersistentQuestData(br));
                }
                long pos = br.BaseStream.Position;
            }
        }
        public override string ToString()
        {
            return Filename;
        }

        public void backupandDelete(string questplayerdataPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string Fullfilename = questplayerdataPath + "\\" + Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(questplayerdataPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, questplayerdataPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }

        public void SaveFIle(string path)
        {
            using (FileStream fs = new FileStream(path + "//" + Filename + ".bin", FileMode.Open, FileAccess.ReadWrite))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(CONFIGVERSION);
                bw.Write(ExpansionQuestPersistentQuestDataCount);
                for (int i = 0; i < ExpansionQuestPersistentQuestDataCount; i++)
                {
                    QuestDatas[i].WriteBin(bw);
                }
            }
        }
    }

    public class ExpansionQuestPersistentQuestData
    {
        public int QuestID = -1;
        public ExpansionQuestState State = ExpansionQuestState.NONE;
        public int Timestamp = -1;
        public int QuestObjectivesCount;
        public BindingList<ExpansionQuestObjectiveData> QuestObjectives;
        public int LastUpdateTime;
        public int CompletionCount;

        public Quests AssignedQuestinfo;

        public ExpansionQuestPersistentQuestData(BinaryReader br)
        {
            long pos = br.BaseStream.Position;
            QuestID = br.ReadInt32();
            State = (ExpansionQuestState)br.ReadInt32();
            Timestamp = br.ReadInt32();
            QuestObjectivesCount = br.ReadInt32();
            QuestObjectives = new BindingList<ExpansionQuestObjectiveData>();
            for(int i = 0; i < QuestObjectivesCount; i++)
            {
                QuestObjectives.Add(new ExpansionQuestObjectiveData(br));
            }
            LastUpdateTime = br.ReadInt32();
            CompletionCount = br.ReadInt32();
        }
        public override string ToString()
        {
            return "Quest ID : " + QuestID;
        }

        internal void WriteBin(BinaryWriter bw)
        {
            bw.Write(QuestID);
            bw.Write((int)State);
            bw.Write(Timestamp);
            bw.Write(QuestObjectivesCount);
            for (int i = 0; i < QuestObjectivesCount; i++)
            {
                QuestObjectives[i].WriteBin(bw);
            }
            bw.Write(LastUpdateTime);
            bw.Write(CompletionCount);
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

        public int deliveryCount = 0;
        public BindingList<ExpansionQuestDeliveryObjectiveData> ExpansionQuestDeliveryObjectiveData;


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
            if(ObjectiveType == QuExpansionQuestObjectiveTypeestType.COLLECT || ObjectiveType == QuExpansionQuestObjectiveTypeestType.DELIVERY)
            {
                deliveryCount = br.ReadInt32();
                if(deliveryCount > 0)
                {
                    ExpansionQuestDeliveryObjectiveData = new BindingList<ExpansionQuestDeliveryObjectiveData>();
                    for (int j = 0; j < deliveryCount; j++)
                    {
                        ExpansionQuestDeliveryObjectiveData.Add(new ExpansionQuestDeliveryObjectiveData()
                        {
                            Index = br.ReadInt32(),
                            Count = br.ReadInt32()
                        }) ;
                    }
                }

            }

            long pos = br.BaseStream.Position;
        }
        public override string ToString()
        {
            if (AssignedObjective == null)
                return "This Querst is Broken, please remove from Player Data......";
            else
                return AssignedObjective.ToString();
        }

        internal void WriteBin(BinaryWriter bw)
        {
            bw.Write(ObjectiveIndex);
            bw.Write((int)ObjectiveType);
            bw.Write(IsCompleted ? 1 : 0);
            bw.Write(IsActive ? 1 : 0);
            bw.Write(ObjectiveAmount);
            bw.Write(ObjectiveCount);
            bw.Write(3);
            for (int i = 0; i < 3; i++)
            {
                bw.Write(ObjectivePosition[i]);
            }
            bw.Write(ActionState ? 1 : 0);
            bw.Write(TimeLimit);
        }
    }

    public class ExpansionQuestDeliveryObjectiveData
    {
        public int Index;
        public int Count;
    }
}
