using Cyotek.Windows.Forms;
using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{

    public partial class ExpansionQuests : DarkForm
    {
        public Project currentproject { get; internal set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public BindingList<string> Factions { get; private set; }
        public float[] CurrentWapypoint { get; private set; }
        public treasurehunitemvarients LootVarients { get; private set; }

        private bool useraction;

        public string QuestsSettingsPath { get; set; }
        public string QuestObjectivesPath { get; set; }
        public string QuestNPCPath { get; set; }
        public string QuestsPath { get; set; }
        public string QuestPlayerDataPath { get; set; }
        public string QuestPersistantServerDataPath { get; set; }
        public string AILoadoutsPath { get; set; }

        public NPCEmotes NPCEmotes { get; set; }
        public NPCEmotes NPCEmotes1 { get; set; }
        public NPCEmotes NPCEmotes2 { get; set; }
        public NPCEmotes NPCEmotes3 { get; set; }
        public NPCEmotes NPCEmotes4 { get; set; }
        public QuestSettings QuestSettings { get; set; }
        public QuestObjectives QuestObjectives { get; set; }
        public ExpansioQuestList QuestsList { get; set; }
        public QuestNPCLists QuestNPCs { get; set; }
        public QuestPersistantDataPlayersList QuestPlayerDataList { get; set; }
        public QuestPersistentServerData QuestPersistentServerData { get; set; }
        public BindingList<AILoadouts> LoadoutList { get; set; }


        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            var CurrentItemWidth = (int)this.CreateGraphics().MeasureString(lb.Items[lb.Items.Count - 1].ToString(), lb.Font, TextRenderer.MeasureText(lb.Items[lb.Items.Count - 1].ToString(), new Font("Arial", 20.0F))).Width;
            lb.HorizontalExtent = CurrentItemWidth+5;
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                myBrush = Brushes.White;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 63, 65)), e.Bounds);
            }
            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds);
            e.DrawFocusRectangle();
        }
        public ExpansionQuests()
        {
            InitializeComponent();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton7.Checked = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            if (tabControl1.SelectedIndex == 3)
                toolStripButton1.Checked = true;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            if (tabControl1.SelectedIndex == 4)
                toolStripButton4.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton7.Checked = false;
            toolStripButton1.Checked = false;
            toolStripButton4.Checked = false;
        }
        private void ExpansionQuests_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            Factions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt").ToList());
            SetupFactionsDropDownBoxes();

            bool needtosave = false;

            LoadoutList = new BindingList<AILoadouts>();
            AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
                    AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
                    AILoadouts.Filename = file.FullName;
                    AILoadouts.Setname();
                    AILoadouts.isDirty = false;
                    LoadoutList.Add(AILoadouts);
                }
                catch { }
            }
            ObjectivesAICampNPCLoadoutFileCB.DisplayMember = "DisplayName";
            ObjectivesAICampNPCLoadoutFileCB.ValueMember = "Value";
            ObjectivesAICampNPCLoadoutFileCB.DataSource = LoadoutList;

            ObjectivesAIVIPNPCLoadoutFileCB.DisplayMember = "DisplayName";
            ObjectivesAIVIPNPCLoadoutFileCB.ValueMember = "Value";
            ObjectivesAIVIPNPCLoadoutFileCB.DataSource = LoadoutList;

            ObjectivesAIPatrolNPCLoadoutFileCB.DisplayMember = "DisplayName";
            ObjectivesAIPatrolNPCLoadoutFileCB.ValueMember = "Value";
            ObjectivesAIPatrolNPCLoadoutFileCB.DataSource = LoadoutList;


            QuestsSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings\\QuestSettings.json";
            if (!File.Exists(QuestsSettingsPath))
            {
                QuestSettings = new QuestSettings();
                needtosave = true;
            }
            else
            {
                QuestSettings = JsonSerializer.Deserialize<QuestSettings>(File.ReadAllText(QuestsSettingsPath));
                QuestSettings.isDirty = false;
                if (QuestSettings.checkver())
                {
                    QuestSettings.isDirty = true;
                    needtosave = true;
                }
            }
            QuestSettings.Filename = QuestsSettingsPath;
            setupQuestsettings();

            QuestPersistantServerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\PersistentServerData.json";
            if (!File.Exists(QuestPersistantServerDataPath))
            {
                QuestPersistentServerData = new QuestPersistentServerData();
                needtosave = true;
            }
            else
            {
                QuestPersistentServerData = JsonSerializer.Deserialize<QuestPersistentServerData>(File.ReadAllText(QuestPersistantServerDataPath));
                QuestPersistentServerData.isDirty = false;
                if (QuestPersistentServerData.checkver())
                {
                    QuestPersistentServerData.isDirty = true;
                    needtosave = true;
                }
            }

            QuestObjectivesPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Objectives";
            QuestObjectives = new QuestObjectives(QuestObjectivesPath);
            if (QuestObjectives.Objectives.Any(x => x.isDirty == true))
                needtosave = true;
            setupobjectives();

            QuestNPCPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\NPCs";
            QuestNPCs = new QuestNPCLists(QuestNPCPath);
            if (QuestNPCs.NPCList.Any(x => x.isDirty == true))
                needtosave = true;
            setupNPCs();

            QuestsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Quests";
            QuestsList = new ExpansioQuestList(QuestsPath);
            QuestsList.setobjectiveenums();
            QuestsList.GetNPCLists(QuestNPCs);
            QuestsList.GetPreQuests();
            if (QuestsList.QuestList.Any(x => x.isDirty == true))
                needtosave = true;
            setupquests();
              
            SetupSharedLists();


            QuestPlayerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\PlayerData";
            if (!Directory.Exists(QuestPlayerDataPath))
            {
                Directory.CreateDirectory(QuestPlayerDataPath);
            }
            QuestPlayerDataList = new QuestPersistantDataPlayersList(QuestPlayerDataPath);
            setupplayerdata();

            tabControl1.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
            }
        }

        private void SetupFactionsDropDownBoxes()
        {
            useraction = false;
            ObjectivesAICampNPCFactionCB.DataSource = new BindingList<string>(Factions);
            ObjectivesAIPatrolNPCFactionCB.DataSource = new BindingList<string>(Factions);
            QuestNPCFactionLB.DataSource = new BindingList<string>(Factions);


            useraction = true;
        }

        private void ExpansionQuests_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (QuestSettings.isDirty)
            {
                needtosave = true;
            }
            foreach (ExpansionQuestNPCs npcs in QuestNPCs.NPCList)
            {
                if (npcs.isDirty)
                    needtosave = true;
            }
            foreach (Quests Quest in QuestsList.QuestList)
            {
                if (Quest.isDirty)
                    needtosave = true;
            }
            foreach (QuestObjectivesBase obj in QuestObjectives.Objectives)
            {
                if (obj.isDirty)
                    needtosave = true;
            }
            foreach(QuestPlayerData qpd in QuestPlayerDataList.QuestPlayerDataList)
            {
                if (qpd.isDirty)
                    needtosave = true;
            }
            if (QuestNPCs.Markedfordelete != null)
            {
                needtosave = true;
            }
            if (QuestsList.Markedfordelete != null)
            {
                needtosave = true;
            }
            if (QuestObjectives.Markedfordelete != null)
            {
                needtosave = true;
            }
            if (QuestPlayerDataList.Markedfordelete != null)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes or finilized deletetions., do you wish to save/Finalise delete", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    savefiles();
                }
            }
        }

        private object ReadJson<T>(string filename)
        {
            try
            {
                Console.WriteLine("serializing " + filename);
                return JsonSerializer.Deserialize<T>(File.ReadAllText(filename));
            }
            catch (Exception ex)
            {
                MessageBox.Show("there is an error in the following file\n" + filename + Environment.NewLine + ex.InnerException.Message);
                return null;
            }
        }
        private void SetupSharedLists()
        {
            useraction = false;

            //QuestNPCs.setupquestlists(QuestsList);

            comboBox1.DisplayMember = "DisplayName";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = Enum.GetValues(typeof(ExpansionQuestState));

            comboBox2.DisplayMember = "DisplayName";
            comboBox2.ValueMember = "Value";
            comboBox2.DataSource = Enum.GetValues(typeof(QuExpansionQuestObjectiveTypeestType));

            


            QuestNPCListLB.SelectedIndex = -1;
            if (QuestNPCListLB.Items.Count > 0)
                QuestNPCListLB.SelectedIndex = 0;

            QuestsListLB.SelectedIndex = -1;
            if (QuestsListLB.Items.Count > 0)
                QuestsListLB.SelectedIndex = 0;

            useraction = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles(bool updated = false)
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (QuestSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(QuestSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(QuestSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(QuestSettings.Filename, Path.GetDirectoryName(QuestSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(QuestSettings.Filename) + ".bak", true);
                }
                QuestSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(QuestSettings, options);
                File.WriteAllText(QuestSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(QuestSettings.Filename));
            }
            foreach(ExpansionQuestNPCs npcs in QuestNPCs.NPCList)
            {
                if (!npcs.isDirty) continue;
                npcs.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(npcs, options);
                if (currentproject.Createbackups && File.Exists(QuestNPCPath + "\\" + npcs.Filename + ".json"))
                {
                    Directory.CreateDirectory(QuestNPCPath + "\\Backup\\" + SaveTime);
                    File.Copy(QuestNPCPath + "\\" + npcs.Filename + ".json", QuestNPCPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(npcs.Filename) + ".bak", true);
                }
                File.WriteAllText(QuestNPCPath + "\\" + npcs.Filename + ".json", jsonString);
                midifiedfiles.Add(Path.GetFileName(npcs.Filename));
            }
            foreach (Quests Quest in QuestsList.QuestList)
            {
                if (!Quest.isDirty) continue;
                Quest.isDirty = false;
                Quest.SetNPCList();
                Quest.SetPreQuests();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(Quest, options);
                if (currentproject.Createbackups && File.Exists(QuestsPath + "\\" + Quest.Filename + ".json"))
                {
                    Directory.CreateDirectory(QuestsPath + "\\Backup\\" + SaveTime);
                    File.Copy(QuestsPath + "\\" + Quest.Filename + ".json", QuestsPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Quest.Filename) + ".bak", true);
                }
                File.WriteAllText(QuestsPath + "\\" + Quest.Filename + ".json", jsonString);
                midifiedfiles.Add(Path.GetFileName(Quest.Filename));
            }
            foreach (QuestObjectivesBase obj in QuestObjectives.Objectives)
            {
                if (!obj.isDirty) continue;
                obj.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = "";
                string filepath = QuestObjectivesPath + "\\" + obj.getfoldernames()[(int)obj._ObjectiveTypeEnum];
                switch (obj._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        QuestObjectivesTarget target = obj as QuestObjectivesTarget;
                        jsonString = JsonSerializer.Serialize(target, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        QuestObjectivesTravel travel = obj as QuestObjectivesTravel;
                        jsonString = JsonSerializer.Serialize(travel, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        QuestObjectivesCollection collection = obj as QuestObjectivesCollection;
                        jsonString = JsonSerializer.Serialize(collection, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        QuestObjectivesDelivery Delivery = obj as QuestObjectivesDelivery;
                        jsonString = JsonSerializer.Serialize(Delivery, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        QuestObjectivesTreasureHunt TreasureHunt = obj as QuestObjectivesTreasureHunt;
                        jsonString = JsonSerializer.Serialize(TreasureHunt, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        QuestObjectivesAIPatrol AIPatrol = obj as QuestObjectivesAIPatrol;
                        jsonString = JsonSerializer.Serialize(AIPatrol, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        QuestObjectivesAICamp AICamp = obj as QuestObjectivesAICamp;
                        jsonString = JsonSerializer.Serialize(AICamp, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        QuestObjectivesAIVIP AIVIP = obj as QuestObjectivesAIVIP;
                        jsonString = JsonSerializer.Serialize(AIVIP, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        QuestObjectivesAction Action = obj as QuestObjectivesAction;
                        jsonString = JsonSerializer.Serialize(Action, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        QuestObjectivesCrafting Crafting = obj as QuestObjectivesCrafting;
                        jsonString = JsonSerializer.Serialize(Crafting, options);
                        break;
                    default:
                        break;
                }
                if (currentproject.Createbackups && File.Exists(filepath + "\\" + obj.Filename + ".json"))
                {
                    Directory.CreateDirectory(filepath + "\\Backup\\" + SaveTime);
                    File.Copy(filepath + "\\" + obj.Filename + ".json", filepath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(obj.Filename) + ".bak", true);
                }
                File.WriteAllText(filepath + "\\" + obj.Filename + ".json", jsonString);
                midifiedfiles.Add(Path.GetFileName(obj.Filename));
            }

            foreach(QuestPlayerData qpd in QuestPlayerDataList.QuestPlayerDataList)
            {
                if(qpd.isDirty)
                {
                    qpd.isDirty = false;
                    if (currentproject.Createbackups && File.Exists(QuestPlayerDataPath + "\\" + qpd.Filename + ".bin"))
                    {
                        Directory.CreateDirectory(QuestsPath + "\\Backup\\" + SaveTime);
                        File.Copy(QuestPlayerDataPath + "\\" + qpd.Filename + ".bin", QuestPlayerDataPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(qpd.Filename) + ".bak", true);
                    }
                    qpd.SaveFIle(QuestPlayerDataPath);
                    midifiedfiles.Add(Path.GetFileName(qpd.Filename));
                }
            }

            string message = "The Following Files were saved....\n";
            if (updated)
            {
                message = "The following files were either Created or Updated...\n";
            }
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                if (i == 5)
                {
                    message += l + "\n";
                    i = 0;
                }
                else
                {
                    message += l + ", ";
                    i++;
                }

            }
            if (midifiedfiles.Count == 0)
                message = "";
            if (QuestNPCs.Markedfordelete != null)
            {
                message += "The following Quest NPC files were Removed\n";
                i = 0;
                foreach (ExpansionQuestNPCs del in QuestNPCs.Markedfordelete)
                {
                    del.backupandDelete(QuestNPCPath);
                    midifiedfiles.Add(del.Filename);
                    if (i == 5)
                    {
                        message += del.Filename + "\n";
                        i = 0;
                    }
                    else
                    {
                        message += del.Filename + ", ";
                        i++;
                    }
                }
                QuestNPCs.Markedfordelete = null;
            }
            if (QuestsList.Markedfordelete != null)
            {
                message += "The following Quest files were Removed\n";
                i = 0;
                foreach (Quests del in QuestsList.Markedfordelete)
                {
                    del.backupandDelete(QuestsPath);
                    midifiedfiles.Add(del.Filename);
                    if (i == 5)
                    {
                        message += del.Filename + "\n";
                        i = 0;
                    }
                    else
                    {
                        message += del.Filename + ", ";
                        i++;
                    }
                }
                QuestsList.Markedfordelete = null;
            }
            if (QuestObjectives.Markedfordelete != null)
            {
                message += "The following Quest Objective files were Removed\n";
                i = 0;
                foreach (QuestObjectivesBase del in QuestObjectives.Markedfordelete)
                {
                    del.backupandDelete(QuestObjectivesPath);
                    midifiedfiles.Add(del.Filename);
                    if (i == 5)
                    {
                        message += del.Filename + "\n";
                        i = 0;
                    }
                    else
                    {
                        message += del.Filename + ", ";
                        i++;
                    }
                }
                QuestObjectives.Markedfordelete = null;
            }
            if (QuestPlayerDataList.Markedfordelete != null)
            {
                message += "The following Quest Player Data files were Removed\n";
                i = 0;
                foreach (QuestPlayerData del in QuestPlayerDataList.Markedfordelete)
                {
                    del.backupandDelete(QuestPlayerDataPath);
                    midifiedfiles.Add(del.Filename);
                    if (i == 5)
                    {
                        message += del.Filename + "\n";
                        i = 0;
                    }
                    else
                    {
                        message += del.Filename + ", ";
                        i++;
                    }
                }
                QuestPlayerDataList.Markedfordelete = null;
            }

            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\expansionMod\\settings");
                    break;
                case 2:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\NPCs");
                    break;
                case 1:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Quests");
                    break;
                case 3:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Objectives");
                    break;
            }
        }

        #region questsettings
        private void setupQuestsettings()
        {
            useraction = false;

            string[] boolignorenames = new string[] { "m_Version", "WeeklyQuestResetHour", "WeeklyQuestResteMinute", "DailyQuestResetHour", "DailyQuestResetMinute", "GroupQuestMode" };
            List<string> questbools = Helper.GetPropertiesNameOfClass<int>(QuestSettings, boolignorenames);
            QuestBoolsLB.DisplayMember = "DisplayName";
            QuestBoolsLB.ValueMember = "Value";
            QuestBoolsLB.DataSource = questbools;

            string[] stringignorenames = new string[] { "Filename" };
            List<string> queststrings = Helper.GetPropertiesNameOfClass<string>(QuestSettings, stringignorenames);
            QuestStringsLB.DisplayMember = "DisplayName";
            QuestStringsLB.ValueMember = "Value";
            QuestStringsLB.DataSource = queststrings;

            string[] intIgnoreNames = new string[] { "m_Version", "EnableQuests", "EnableQuestLogTab", "CreateQuestNPCMarkers", "UseUTCTime",};
            List<string> questints = Helper.GetPropertiesNameOfClass<int>(QuestSettings, intIgnoreNames);
            QuestIntsLB.DisplayMember = "DisplayName";
            QuestIntsLB.ValueMember = "Value";
            QuestIntsLB.DataSource = questints;


            useraction = true;
        }
        private void QuestBoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestBoolsLB.SelectedItems.Count < 1) return;
            useraction = false;
            QuestBoolsCB.Checked = (int)Helper.GetPropValue(QuestSettings, QuestBoolsLB.GetItemText(QuestBoolsLB.SelectedItem)) == 1 ? true : false;
            groupBox8.Text = "bool Info";
            switch (QuestBoolsLB.GetItemText(QuestBoolsLB.SelectedItem))
            {
                case "EnableQuests":
                    InfoLabel.Text = "Boolean.\n\nEnable\\disable the quest system.";
                    break;
                case "EnableQuestLogTab":
                    InfoLabel.Text = "Boolean.\n\nOnly used if the Expansion - Book mod is loaded next to the quest mod. Enable\\disable the book quest log tab.";
                    break;
                case "CreateQuestNPCMarkers":
                    InfoLabel.Text = "Boolean.\n\nOnly used if the Expansion-Navigation mod is loaded next to the quest mod. Set server map markers on the quest NPC spawn locations. NOT WORKING AT THIS POINT. SUBJECT TO CHANGE!";
                    break;
                case "UseUTCTime":
                    InfoLabel.Text = "Boolean.\n\nUse UTC server time or not for all quest cooldown and reset times.";
                    break;
            }

            useraction = true;
        }
        private void QuestStringsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestStringsLB.SelectedItems.Count < 1) return;
            useraction = false;
            QuestStringTB.Text = Helper.GetPropValue(QuestSettings, QuestStringsLB.GetItemText(QuestStringsLB.SelectedItem)).ToString();
            useraction = true;
        }
        private void QuestIntsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestIntsLB.SelectedItems.Count < 1) return;
            useraction = false;
            QuestIntsNUD.Value = (int)Helper.GetPropValue(QuestSettings, QuestIntsLB.GetItemText(QuestIntsLB.SelectedItem));
            groupBox8.Text = "int Info";
            switch (QuestIntsLB.GetItemText(QuestIntsLB.SelectedItem))
            {
                case "GroupQuestMode":
                    InfoLabel.Text = "GroupQuestMode\nInteger.\n\n0 - Only group owners can accept and turn-in group quests.\n\n1 - Only group owner can turn-in group quest but all group members can accept them.\n\n2 - All group members can accept and turn-in group quests.";
                    break;
                case "DailyQuestResetMinute":
                    InfoLabel.Text = "Integer.\n\nMinute at when the quest reset will happend for all daily quests.";
                    break;
                case "DailyQuestResetHour":
                    InfoLabel.Text = "Integer.\n\nHour at when the quest reset will happend for all daily quests.";
                    break;
                case "WeeklyQuestResteMinute":
                    InfoLabel.Text = "Integer.\n\nMinute at when the quest reset will happend for all weekly quests.";
                    break;
                case "WeeklyQuestResetHour":
                    InfoLabel.Text = "Integer.\n\nHour at when the quest reset will happend for all weekly quests.";
                    break;
            }
            useraction = true;
        }
        private void QuestBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(QuestSettings, QuestBoolsLB.GetItemText(QuestBoolsLB.SelectedItem), QuestBoolsCB.Checked);
            QuestSettings.isDirty = true;
        }
        private void QuestStringTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetStringValue(QuestSettings, QuestStringsLB.GetItemText(QuestStringsLB.SelectedItem), QuestStringTB.Text);
            QuestSettings.isDirty = true;
        }
        private void QuestIntsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(QuestSettings, QuestIntsLB.GetItemText(QuestIntsLB.SelectedItem), (int)QuestIntsNUD.Value);
            QuestSettings.isDirty = true;
        }
        #endregion questsettings
        #region npcs
        public ExpansionQuestNPCs currentQuestNPC { get; set; }
        public void setupNPCs()
        {
            useraction = false;

            QuestNPCsClassNameCB.DisplayMember = "Name";
            QuestNPCsClassNameCB.ValueMember = "Value";
            QuestNPCsClassNameCB.DataSource = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\QuestNPCs.txt").ToList();
            
            NPCEmotes = new NPCEmotes(Application.StartupPath + "\\TraderNPCs\\Emotes.txt");
            NPCEmotes1 = new NPCEmotes(Application.StartupPath + "\\TraderNPCs\\Emotes.txt");
            NPCEmotes2 = new NPCEmotes(Application.StartupPath + "\\TraderNPCs\\Emotes.txt");
            NPCEmotes3 = new NPCEmotes(Application.StartupPath + "\\TraderNPCs\\Emotes.txt");
            NPCEmotes4 = new NPCEmotes(Application.StartupPath + "\\TraderNPCs\\Emotes.txt");
            questsNPCsNPCEmoteIDCB.DisplayMember = "DisplayName";
            questsNPCsNPCEmoteIDCB.ValueMember = "Value";
            questsNPCsNPCEmoteIDCB.DataSource = NPCEmotes.Emotes;
            NPCInteractionEmoteIDCB.DisplayMember = "DisplayName";
            NPCInteractionEmoteIDCB.ValueMember = "Value";
            NPCInteractionEmoteIDCB.DataSource = NPCEmotes1.Emotes;
            NPCQuestCancelEmoteIDCB.DisplayMember = "DisplayName";
            NPCQuestCancelEmoteIDCB.ValueMember = "Value";
            NPCQuestCancelEmoteIDCB.DataSource = NPCEmotes2.Emotes;
            NPCQuestStartEmoteIDCB.DisplayMember = "DisplayName";
            NPCQuestStartEmoteIDCB.ValueMember = "Value";
            NPCQuestStartEmoteIDCB.DataSource = NPCEmotes3.Emotes;
            NPCQuestCompleteEmoteIDCB.DisplayMember = "DisplayName";
            NPCQuestCompleteEmoteIDCB.ValueMember = "Value";
            NPCQuestCompleteEmoteIDCB.DataSource = NPCEmotes4.Emotes;
            List<string> loadouts = new List<string>();
            loadouts.Add("");
            string AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                loadouts.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
            QuestNPCsLoadoutsCB.DisplayMember = "DisplayName";
            QuestNPCsLoadoutsCB.ValueMember = "Value";
            QuestNPCsLoadoutsCB.DataSource = loadouts;



            QuestNPCListLB.DisplayMember = "DisplayName";
            QuestNPCListLB.ValueMember = "Value";
            QuestNPCListLB.DataSource = QuestNPCs.NPCList;




            useraction = true;
        }
        private void QuestNPCListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestNPCListLB.SelectedItems.Count < 1) return;
            currentQuestNPC = QuestNPCListLB.SelectedItem as ExpansionQuestNPCs;
            useraction = false;
            QuestNPCFilenameTB.Text = currentQuestNPC.Filename;
            QuestNPCConfigVersionNUD.Value = currentQuestNPC.ConfigVersion;
            QuestNPCIDNUD.Value = currentQuestNPC.ID;
            QuestNPCsClassNameCB.SelectedIndex = QuestNPCsClassNameCB.FindStringExact(currentQuestNPC.ClassName);
            QuestNPCIsAICB.Checked = currentQuestNPC.IsAI == 1 ? true : false;
            QuestNPCsPOSXNUD.Value = (decimal)currentQuestNPC.Position[0];
            QuestNPCsPOSYNUD.Value = (decimal)currentQuestNPC.Position[1];
            QuestNPCsPOSZNUD.Value = (decimal)currentQuestNPC.Position[2];
            QuestNPCsOXNUD.Value = (decimal)currentQuestNPC.Orientation[0];
            QuestNPCsOYNUD.Value = (decimal)currentQuestNPC.Orientation[1];
            QuestNPCsOZNUD.Value = (decimal)currentQuestNPC.Orientation[2];
            QuestsNPCsNameTB.Text = currentQuestNPC.NPCName;
            QuestsNPCsDefaultNPCTextTB.Text = currentQuestNPC.DefaultNPCText;
            questsNPCsNPCEmoteIDCB.SelectedValue = currentQuestNPC.NPCEmoteID;
            NPCInteractionEmoteIDCB.SelectedValue = currentQuestNPC.NPCInteractionEmoteID;
            NPCQuestCancelEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestCancelEmoteID;
            NPCQuestStartEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestStartEmoteID;
            NPCQuestCompleteEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestCompleteEmoteID;

            QuestNPCIsEmoteStaticCB.Checked = currentQuestNPC.NPCEmoteIsStatic == 1 ? true : false;
            QuestNPCsLoadoutsCB.SelectedIndex = QuestNPCsLoadoutsCB.FindStringExact(currentQuestNPC.NPCLoadoutFile);
            QuestNPCsIsStaticCB.Checked = currentQuestNPC.IsStatic == 1 ? true:false;

            QuestNPCWaypointsLB.DisplayMember = "DisplayName";
            QuestNPCWaypointsLB.ValueMember = "Value";
            QuestNPCWaypointsLB.DataSource = currentQuestNPC.Waypoints;


            useraction = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            QuestNPCs.CreateNewNPC();
            QuestNPCListLB.SelectedIndex = -1;
            SetupSharedLists();
            if (QuestNPCListLB.Items.Count == 0)
                QuestNPCListLB.SelectedIndex = QuestNPCListLB.Items.Count - 1;
            else
                QuestNPCListLB.SelectedIndex = 0;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this NPC, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                QuestNPCs.RemoveNPC(currentQuestNPC);
                QuestsList.RemoveNPCFromQuests(currentQuestNPC);
                SetupSharedLists();
                if (QuestNPCListLB.Items.Count == 0)
                    QuestNPCListLB.SelectedIndex = -1;
                else
                    QuestNPCListLB.SelectedIndex = 0;
            }
        }
        private void QuestNPCsClassNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.ClassName = QuestNPCsClassNameCB.GetItemText(QuestNPCsClassNameCB.SelectedItem);
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsLoadoutsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.NPCLoadoutFile = QuestNPCsLoadoutsCB.GetItemText(QuestNPCsLoadoutsCB.SelectedItem);
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCFactionLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.NPCFaction = QuestNPCFactionLB.GetItemText(QuestNPCFactionLB.SelectedItem);
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCWaypointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            float[] waypoint = QuestNPCWaypointsLB.SelectedItem as float[];
            useraction = false;
            QuestNPCWaypointXNUD.Value = (decimal)waypoint[0];
            QuestNPCWaypointYNUD.Value = (decimal)waypoint[1];
            QuestNPCWaypointZNUD.Value = (decimal)waypoint[2];
            useraction = true;
        }
        private void QuestsNPCsNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.NPCName = QuestsNPCsNameTB.Text;
            currentQuestNPC.isDirty = true;
            QuestNPCListLB.Refresh();
        }
        private void QuestsNPCsDefaultNPCTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.DefaultNPCText = QuestsNPCsDefaultNPCTextTB.Text;
            currentQuestNPC.isDirty = true;
        }
        private void questsNPCsNPCEmoteIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Emote emote = questsNPCsNPCEmoteIDCB.SelectedItem as Emote;
            currentQuestNPC.NPCEmoteID = emote.Value;
            currentQuestNPC.isDirty = true;
        }
        private void NPCInteractionEmoteIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Emote emote = NPCInteractionEmoteIDCB.SelectedItem as Emote;
            currentQuestNPC.NPCInteractionEmoteID = emote.Value;
            currentQuestNPC.isDirty = true;
        }
        private void NPCQuestCancelEmoteIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Emote emote = NPCQuestCancelEmoteIDCB.SelectedItem as Emote;
            currentQuestNPC.NPCQuestCancelEmoteID = emote.Value;
            currentQuestNPC.isDirty = true;
        }
        private void NPCQuestStartEmoteIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Emote emote = NPCQuestStartEmoteIDCB.SelectedItem as Emote;
            currentQuestNPC.NPCQuestStartEmoteID = emote.Value;
            currentQuestNPC.isDirty = true;
        }
        private void NPCQuestCompleteEmoteIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Emote emote = NPCQuestCompleteEmoteIDCB.SelectedItem as Emote;
            currentQuestNPC.NPCQuestCompleteEmoteID = emote.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCIsAICB_CheckedChanged(object sender, EventArgs e)
        {
            groupBox6.Visible = QuestNPCIsAICB.Checked;
            if (!useraction) return;
            currentQuestNPC.IsAI = QuestNPCIsAICB.Checked == true ? 1 : 0;
            currentQuestNPC.Waypoints = new BindingList<float[]>();
            QuestNPCWaypointsLB.DisplayMember = "DisplayName";
            QuestNPCWaypointsLB.ValueMember = "Value";
            QuestNPCWaypointsLB.DataSource = currentQuestNPC.Waypoints;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsIsStaticCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.IsStatic = QuestNPCsIsStaticCB.Checked == true ? 1 : 0;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCIsEmoteStaticCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.NPCEmoteIsStatic = QuestNPCIsEmoteStaticCB.Checked == true ? 1 : 0;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Position[0] = (float)QuestNPCsPOSXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Position[1] = (float)QuestNPCsPOSYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Position[2] = (float)QuestNPCsPOSZNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Orientation[0] = (float)QuestNPCsOXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Orientation[1] = (float)QuestNPCsOYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Orientation[2] = (float)QuestNPCsOZNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            currentQuestNPC.Waypoints.Add(new float[] { 0, 0, 0 });
            currentQuestNPC.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            currentQuestNPC.Waypoints.Remove(QuestNPCWaypointsLB.SelectedItem as float[]);
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            float[] waypoint = QuestNPCWaypointsLB.SelectedItem as float[];
            waypoint[0] = (float)QuestNPCWaypointXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            float[] waypoint = QuestNPCWaypointsLB.SelectedItem as float[];
            waypoint[1] = (float)QuestNPCWaypointYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            float[] waypoint = QuestNPCWaypointsLB.SelectedItem as float[];
            waypoint[2] = (float)QuestNPCWaypointZNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    currentQuestNPC.Waypoints = new BindingList<float[]>();
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');

                        currentQuestNPC.Waypoints.Add(new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) });
                    }
                    QuestNPCWaypointsLB.SelectedIndex = -1;
                    QuestNPCWaypointsLB.SelectedIndex = QuestNPCWaypointsLB.Items.Count - 1;
                    QuestNPCWaypointsLB.Invalidate();
                    currentQuestNPC.isDirty = true;
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine(currentQuestNPC.NPCName + "|" + currentQuestNPC.Position[0].ToString("F6") + " " + currentQuestNPC.Position[1].ToString("F6") + " " + currentQuestNPC.Position[2].ToString("F6") + "|0.0 0.0 0.0");
            foreach (float[] vec3 in currentQuestNPC.Waypoints)
            {
                SB.AppendLine(currentQuestNPC.NPCName + "|" + vec3[0].ToString("F6") + " " + vec3[1].ToString("F6") + " " + vec3[2].ToString("F6") + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        currentQuestNPC.Waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        int i = 0;
                        if (i == 0)
                        {
                            currentQuestNPC.Position = eo.Position;
                            QuestNPCsPOSXNUD.Value = (decimal)currentQuestNPC.Position[0];
                            QuestNPCsPOSYNUD.Value = (decimal)currentQuestNPC.Position[1];
                            QuestNPCsPOSZNUD.Value = (decimal)currentQuestNPC.Position[2];
                        }
                        else
                        {
                            currentQuestNPC.Waypoints.Add(eo.Position);
                        }
                    }
                    QuestNPCWaypointsLB.SelectedIndex = -1;
                    QuestNPCWaypointsLB.SelectedIndex = QuestNPCWaypointsLB.Items.Count - 1;
                    QuestNPCWaypointsLB.Refresh();
                    currentQuestNPC.isDirty = true;
                }
            }
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in currentQuestNPC.Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = currentQuestNPC.NPCName,
                    DisplayName = currentQuestNPC.NPCName,
                    Position = array,
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        #endregion npc
        #region quests
        public Quests CurrentQuest { get; private set; }


        public void setupquests()
        {
            useraction = false;

            Quests emptyquest = new Quests()
            {
                ID = -1,
                Title = "NONE"
            };
            BindingList<Quests> questlist = new BindingList<Quests>();
            questlist.Add(emptyquest);
            foreach (Quests q in QuestsList.QuestList)
            {
                questlist.Add(q);
            }
            questlist = new BindingList<Quests>(questlist.OrderBy(x => x.ID).ToList());


            QuestFollowupQuestCB.DisplayMember = "DisplayName";
            QuestFollowupQuestCB.ValueMember = "Value";
            QuestFollowupQuestCB.DataSource = questlist;

            QuestTypeCB.DataSource = Enum.GetValues(typeof(ExpansionQuestsQuestType));

            QuestsListLB.DisplayMember = "DisplayName";
            QuestsListLB.ValueMember = "Value";
            QuestsListLB.DataSource = QuestsList.QuestList;

            useraction = true; 
        }
        private void QuestsListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestsListLB.SelectedItems.Count < 1) return;
            CurrentQuest = QuestsListLB.SelectedItem as Quests;
            useraction = false;

            QuestFileNameTB.Text = CurrentQuest.Filename;
            QuestConfigVersionNUD.Value = CurrentQuest.ConfigVersion;
            QuestIDNUD.Value = CurrentQuest.ID;
            QuestTypeCB.SelectedItem = (ExpansionQuestsQuestType)CurrentQuest.Type;
            QuestTitleTB.Text = CurrentQuest.Title;
            if (CurrentQuest.Descriptions.Count == 0)
            {
                CurrentQuest.Descriptions = new BindingList<string>(new string[] { "", "", "" });
            }
            QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
            QuestDescription2TB.Text = CurrentQuest.Descriptions[1];
            QuestDescription3TB.Text = CurrentQuest.Descriptions[2];
            useraction = false;
            QuestObjectiveTextTB.Text = CurrentQuest.ObjectiveText;

            QuestFollowupQuestCB.SelectedItem = QuestFollowupQuestCB.Items.Cast<Quests>().FirstOrDefault(z => z.ID == CurrentQuest.FollowUpQuest);
            QuestIsAchivementCB.Checked = CurrentQuest.IsAchivement == 1 ? true : false;
            QuestRepeatableCB.Checked = CurrentQuest.Repeatable == 1 ? true : false;
            QuestIsDailyQuestCB.Checked = CurrentQuest.IsDailyQuest == 1 ? true : false;
            QuestIsWeeklyQuestCB.Checked = CurrentQuest.IsWeeklyQuest == 1 ? true : false;
            QuestCancelQuestOnPlayerDeathCB.Checked = CurrentQuest.CancelQuestOnPlayerDeath == 1 ? true : false;
            questAutocompleteCB.Checked = CurrentQuest.Autocomplete == 1 ? true : false;
            QuestIsGroupQuestCB.Checked = CurrentQuest.IsGroupQuest == 1 ? true : false;
            QuestObjectSetFileNameTB.Text = CurrentQuest.ObjectSetFileName;

            for( int i = 0; i < CurrentQuest.Objectives.Count; i++)
            {
                QuestObjectivesBase checkobj = QuestObjectives.CheckObjectiveExists(CurrentQuest.Objectives[i]);
                if (checkobj == null)
                    MessageBox.Show("Type " + CurrentQuest.Objectives[i].ObjectiveType.ToString() + ",Objective " + CurrentQuest.Objectives[i].ID.ToString() + " doesn not exist in the list of objectives, please check");
                else
                {
                    if(CurrentQuest.Objectives[i].ConfigVersion != checkobj.ConfigVersion)
                    {
                        CurrentQuest.Objectives[i].ConfigVersion = checkobj.ConfigVersion;
                        CurrentQuest.isDirty = true;
                    }

                }
            }
            QuestNeedToSelectRewardCB.Checked = CurrentQuest.NeedToSelectReward == 1 ? true : false;
            QuestRewardsForGroupOwnerOnlyCB.Checked = CurrentQuest.RewardsForGroupOwnerOnly == 1 ? true : false;
            QuestReputationRewardNUD.Value = CurrentQuest.ReputationReward;
            QuestReputationRequirmentNUD.Value = CurrentQuest.ReputationRequirement;

            QuestPreQuestIDsLB.DisplayMember = "DisplayName";
            QuestPreQuestIDsLB.ValueMember = "Value";
            QuestPreQuestIDsLB.DataSource = CurrentQuest.PreQuests;

            QuestGiverIDsLB.DisplayMember = "DisplayName";
            QuestGiverIDsLB.ValueMember = "Value";
            QuestGiverIDsLB.DataSource = CurrentQuest.QuestGivers;

            QuestQuestTurnInIDsLB.DisplayMember = "DisplayName";
            QuestQuestTurnInIDsLB.ValueMember = "Value";
            QuestQuestTurnInIDsLB.DataSource = CurrentQuest.QuestTurnIns;

            QuestObjectivesLB.DisplayMember = "DisplayName";
            QuestObjectivesLB.ValueMember = "Value";
            QuestObjectivesLB.DataSource = CurrentQuest.Objectives;

            QuestQuestItemsLB.DisplayMember = "DisplayName";
            QuestQuestItemsLB.ValueMember = "Value";
            QuestQuestItemsLB.DataSource = CurrentQuest.QuestItems;

            QuestRewardsLB.DisplayMember = "DisplayName";
            QuestRewardsLB.ValueMember = "Value";
            QuestRewardsLB.DataSource = CurrentQuest.Rewards;

            QuestColour.Invalidate();

            useraction = true;
        }
        private void QuestTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            ExpansionQuestsQuestType QuestType = (ExpansionQuestsQuestType)QuestTypeCB.SelectedItem;
            CurrentQuest.Type = (int)QuestType;
            CurrentQuest.isDirty = true;
        }
        private void QuestRewardsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            useraction = false;
            QuestRewardsAmountNUD.Value = qr.Amount;

            QuestRewrdsAttchemntsLB.DisplayMember = "DisplayName";
            QuestRewrdsAttchemntsLB.ValueMember = "Value";
            QuestRewrdsAttchemntsLB.DataSource = qr.Attachments;
            useraction = true;
        }
        private void QuestRewardsAmountNUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            qr.Amount = (int)QuestRewardsAmountNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!qr.Attachments.Contains(l))
                    {
                        qr.Attachments.Add(l);
                        CurrentQuest.isDirty = true;
                    }
                }
                QuestRewardsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        private void darkButton35_Click(object sender, EventArgs e)
        {
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            qr.Attachments.Remove(QuestRewrdsAttchemntsLB.GetItemText(QuestRewrdsAttchemntsLB.SelectedItem));
            CurrentQuest.isDirty = true;
            QuestRewrdsAttchemntsLB.Refresh();
            if (QuestRewrdsAttchemntsLB.Items.Count == 0)
                QuestRewrdsAttchemntsLB.SelectedIndex = -1;
            else
                QuestRewrdsAttchemntsLB.SelectedIndex = 0;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            List<int> AllQuestIDs = QuestsList.GetAllQuestIDS();

            AddNewQuestID form = new AddNewQuestID
            {
                NumberofquestsIDs = AllQuestIDs
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                QuestsList.CreateNewQuest(form.SelectedID);
            }
            QuestsListLB.SelectedIndex = -1;
            if (QuestsListLB.Items.Count == 0)
                QuestsListLB.SelectedIndex = QuestsListLB.Items.Count - 1;
            else
                QuestsListLB.SelectedIndex = 0;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Quest, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                QuestsList.RemoveQuest(CurrentQuest);
                if (QuestsListLB.Items.Count == 0)
                    QuestsListLB.SelectedIndex = -1;
                else
                    QuestsListLB.SelectedIndex = 0;
            }
        }
        private void QuestTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.Title = QuestTitleTB.Text;
            CurrentQuest.isDirty = true;

        }
        private void QuestDescription1TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if(CurrentQuest.Descriptions.Count == 3)
                CurrentQuest.Descriptions[0] = QuestDescription1TB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestDescription2TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if(CurrentQuest.Descriptions.Count == 3)
                CurrentQuest.Descriptions[1] = QuestDescription2TB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestDescription3TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if(CurrentQuest.Descriptions.Count == 3)
                CurrentQuest.Descriptions[2] = QuestDescription3TB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestObjectiveTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.ObjectiveText = QuestObjectiveTextTB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestFollowupQuestCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!useraction) return;

            Quests quest = QuestFollowupQuestCB.SelectedItem as Quests;
            CurrentQuest.FollowUpQuest = quest.ID;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsAchivementCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsAchivement = QuestIsAchivementCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestRepeatableCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.Repeatable = QuestRepeatableCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsDailyQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsDailyQuest = QuestIsDailyQuestCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsWeeklyQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsWeeklyQuest = QuestIsWeeklyQuestCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestCancelQuestOnPlayerDeathCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.CancelQuestOnPlayerDeath = QuestCancelQuestOnPlayerDeathCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void questAutocompleteCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.Autocomplete = questAutocompleteCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsGroupQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsGroupQuest = QuestIsGroupQuestCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestObjectSetFileNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.ObjectSetFileName = QuestObjectSetFileNameTB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestNeedToSelectRewardCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.NeedToSelectReward = QuestNeedToSelectRewardCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestRewardsForGroupOwnerOnlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.RewardsForGroupOwnerOnly = QuestRewardsForGroupOwnerOnlyCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestHumanityRewardCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            //CurrentQuest.HumanityReward = QuestHumanityRewardCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    QuestReward newrawrd = new QuestReward()
                    {
                        ClassName = l,
                        Amount = 1,
                        Attachments = new BindingList<string>()
                    };
                    CurrentQuest.Rewards.Add(newrawrd);
                    CurrentQuest.isDirty = true;
                }
                QuestRewardsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            CurrentQuest.Rewards.Remove(qr);
            CurrentQuest.isDirty = true;
            QuestRewardsLB.Refresh();
            if (QuestRewardsLB.Items.Count == 0)
                QuestRewardsLB.SelectedIndex = -1;
            else
                QuestRewardsLB.SelectedIndex = 0;
        }
        private void QuestQuestItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestQuestItemsLB.SelectedItems.Count < 1) return;
            Questitem qr = QuestQuestItemsLB.SelectedItem as Questitem;
            useraction = false;
            QuestQuestItemsAmountNUD.Value = qr.Amount;
            useraction = true;
        }
        private void QuestQuestItemsAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestQuestItemsLB.SelectedItems.Count < 1) return;
            Questitem qr = QuestQuestItemsLB.SelectedItem as Questitem;
            qr.Amount = (int)QuestQuestItemsAmountNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Questitem newrawrd = new Questitem()
                    {
                        ClassName = l,
                        Amount = 1
                    };
                    if (!CurrentQuest.QuestItems.Any(x => x.ClassName == newrawrd.ClassName))
                    {
                        CurrentQuest.QuestItems.Add(newrawrd);
                        CurrentQuest.isDirty = true;
                    }
                }
                QuestRewardsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton27_Click(object sender, EventArgs e)
        {
            if (QuestQuestItemsLB.SelectedItems.Count < 1) return;
            Questitem qr = QuestQuestItemsLB.SelectedItem as Questitem;
            CurrentQuest.QuestItems.Remove(qr);
            CurrentQuest.isDirty = true;
            QuestQuestItemsLB.Refresh();
            if (QuestQuestItemsLB.Items.Count == 0)
                QuestQuestItemsLB.SelectedIndex = -1;
            else
                QuestQuestItemsLB.SelectedIndex = 0;
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            AddObjectives form = new AddObjectives
            {
                QuestObjectives = QuestObjectives.Objectives
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<QuestObjectivesBase> selectedqauests = form.selectedquests;
                foreach (QuestObjectivesBase obj in selectedqauests)
                {
                    QuestObjectivesBase questbase = new QuestObjectivesBase()
                    {
                        Filename = obj.Filename,
                        ConfigVersion = obj.ConfigVersion,
                        ID = obj.ID,
                        _ObjectiveTypeEnum = obj._ObjectiveTypeEnum,
                        ObjectiveType = obj.ObjectiveType,

                    };
                    CurrentQuest.Objectives.Add(questbase);
                }
                CurrentQuest.isDirty = true;
            }
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            if (QuestObjectivesLB.SelectedItems.Count < 1) return;
            QuestObjectivesBase objbase = QuestObjectivesLB.SelectedItem as QuestObjectivesBase;
            CurrentQuest.Objectives.Remove(objbase);
            QuestObjectivesLB.Refresh();
            if (QuestObjectivesLB.Items.Count == 0)
                QuestObjectivesLB.SelectedIndex = -1;
            else
                QuestObjectivesLB.SelectedIndex = 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestReputationRequirmentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.ReputationRequirement = (int)QuestReputationRequirmentNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void QuestReputationRewardNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.ReputationReward = (int)QuestReputationRewardNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void darkButton77_Click(object sender, EventArgs e)
        {
            AddQuestQuest form = new AddQuestQuest
            {
                ExpansioQuestList = QuestsList
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<Quests> SelectedQuests = form.SelectedQuests;
                foreach (Quests obj in SelectedQuests)
                {
                    if (!CurrentQuest.PreQuests.Any(x => x.ID == obj.ID))
                        CurrentQuest.PreQuests.Add(obj);
                }
                CurrentQuest.isDirty = true;
            }
        }
        private void darkButton76_Click(object sender, EventArgs e)
        {
            if (QuestPreQuestIDsLB.SelectedItems.Count < 1) return;
            Quests qr = QuestPreQuestIDsLB.SelectedItem as Quests;
            CurrentQuest.PreQuests.Remove(qr);
            CurrentQuest.isDirty = true;
            QuestPreQuestIDsLB.Refresh();
            if (QuestPreQuestIDsLB.Items.Count == 0)
                QuestPreQuestIDsLB.SelectedIndex = -1;
            else
                QuestPreQuestIDsLB.SelectedIndex = 0;
        }
        private void darkButton75_Click(object sender, EventArgs e)
        {
            AddQuestNPC form = new AddQuestNPC
            {
                QuestNPCLists = QuestNPCs
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<ExpansionQuestNPCs> SelectedNPCs = form.SelectedNPCs;
                foreach (ExpansionQuestNPCs obj in SelectedNPCs)
                {
                    if(!CurrentQuest.QuestGivers.Any(x => x.ID == obj.ID))
                        CurrentQuest.QuestGivers.Add(obj);
                }
                CurrentQuest.isDirty = true;
            }
        }
        private void darkButton74_Click(object sender, EventArgs e)
        {
            if (QuestGiverIDsLB.SelectedItems.Count < 1) return;
            ExpansionQuestNPCs qr = QuestGiverIDsLB.SelectedItem as ExpansionQuestNPCs;
            CurrentQuest.QuestGivers.Remove(qr);
            CurrentQuest.isDirty = true;
            QuestGiverIDsLB.Refresh();
            if (QuestGiverIDsLB.Items.Count == 0)
                QuestGiverIDsLB.SelectedIndex = -1;
            else
                QuestGiverIDsLB.SelectedIndex = 0;
        }
        private void darkButton73_Click(object sender, EventArgs e)
        {
            AddQuestNPC form = new AddQuestNPC
            {
                QuestNPCLists = QuestNPCs
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<ExpansionQuestNPCs> SelectedNPCs = form.SelectedNPCs;
                foreach (ExpansionQuestNPCs obj in SelectedNPCs)
                {
                    if (!CurrentQuest.QuestTurnIns.Any(x => x.ID == obj.ID))
                        CurrentQuest.QuestTurnIns.Add(obj);
                }
                CurrentQuest.isDirty = true;
            }
        }
        private void darkButton72_Click(object sender, EventArgs e)
        {
            if (QuestQuestTurnInIDsLB.SelectedItems.Count < 1) return;
            ExpansionQuestNPCs qr = QuestQuestTurnInIDsLB.SelectedItem as ExpansionQuestNPCs;
            CurrentQuest.QuestTurnIns.Remove(qr);
            CurrentQuest.isDirty = true;
            QuestQuestTurnInIDsLB.Refresh();
            if (QuestQuestTurnInIDsLB.Items.Count == 0)
                QuestQuestTurnInIDsLB.SelectedIndex = -1;
            else
                QuestQuestTurnInIDsLB.SelectedIndex = 0;
        }
        private void QuestColour_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(CurrentQuest.QuestColor);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                CurrentQuest.QuestColor = cpick.Color.ToArgb();
                QuestColour.Invalidate();
                CurrentQuest.isDirty = true;
            }
        }
        private void QuestColour_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentQuest == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(CurrentQuest.QuestColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        #endregion quests
        #region objectives
        public QuestObjectivesBase CurrentTreeNodeTag;
        private void setupobjectives()
        {
            QuestObjectivesObjectiveTypeCB.DataSource = Enum.GetValues(typeof(QuExpansionQuestObjectiveTypeestType));
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode("Objectives")
            {
                Tag = "Parent"
            };
            TreeNode ObjectivesAction = new TreeNode("Action")
            {
                Tag = "Action"
            };
            TreeNode ObjectivesAICamp = new TreeNode("AICamp")
            {
                Tag = "AICamp"
            };
            TreeNode ObjectivesAIPatrol = new TreeNode("AIPatrol")
            {
                Tag = "AIPatrol"
            };
            TreeNode ObjectivesAIVIP = new TreeNode("AIVIP")
            {
                Tag = "AIVIP"
            };
            TreeNode ObjectivesCollection = new TreeNode("Collection")
            {
                Tag = "Collection"
            };
            TreeNode ObjectivesCrafting = new TreeNode("Crafting")
            {
                Tag = "Crafting"
            };
            TreeNode ObjectivesDelivery = new TreeNode("Delivery")
            {
                Tag = "Delivery"
            };
            TreeNode ObjectivesTarget = new TreeNode("Target")
            {
                Tag = "Target"
            };
            TreeNode ObjectivesTravel = new TreeNode("Travel")
            {
                Tag = "Travel"
            };
            TreeNode ObjectivesTreasureHunt = new TreeNode("TreasureHunt")
            {
                Tag = "TreasureHunt"
            };
            for(int i = 0; i < QuestObjectives.Objectives.Count; i++)
            {
                switch (QuestObjectives.Objectives[i]._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        TreeNode newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTarget>(QuestObjectivesPath + "\\Target\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTarget;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TARGET;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTravel>(QuestObjectivesPath + "\\Travel\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTravel;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TRAVEL;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCollection>(QuestObjectivesPath + "\\Collection\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCollection;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.COLLECT;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCrafting>(QuestObjectivesPath + "\\Crafting\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCrafting;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.CRAFTING;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCrafting.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesDelivery>(QuestObjectivesPath + "\\Delivery\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesDelivery;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.DELIVERY;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTreasureHunt>(QuestObjectivesPath + "\\TreasureHunt\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTreasureHunt;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TREASUREHUNT;
                        QuestObjectivesTreasureHunt qoth = QuestObjectives.Objectives[i] as QuestObjectivesTreasureHunt;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIPatrol>(QuestObjectivesPath + "\\AIPatrol\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIPatrol;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIPATROL;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAICamp>(QuestObjectivesPath + "\\AICamp\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAICamp;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AICAMP;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIVIP>(QuestObjectivesPath + "\\AIVIP\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIVIP;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIVIP;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAction>(QuestObjectivesPath + "\\Action\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAction;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.ACTION;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAction.Nodes.Add(newnode);
                        break;
                    default:
                        break;
                }
            }
            root.Nodes.Add(ObjectivesAction);
            root.Nodes.Add(ObjectivesAICamp);
            root.Nodes.Add(ObjectivesAIPatrol);
            root.Nodes.Add(ObjectivesAIVIP);
            root.Nodes.Add(ObjectivesCollection);
            root.Nodes.Add(ObjectivesCrafting);
            root.Nodes.Add(ObjectivesDelivery);
            root.Nodes.Add(ObjectivesTarget);
            root.Nodes.Add(ObjectivesTravel);
            root.Nodes.Add(ObjectivesTreasureHunt);
            treeViewMS1.Nodes.Add(root);
        }
        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            treeViewMS1.SelectedNode = e.Node;
            if (e.Node.Tag is string)
            {
                foreach (ToolStripMenuItem TSMI in contextMenuStrip1.Items)
                {
                    TSMI.Visible = false;
                }
                switch (e.Node.Tag.ToString())
                {
                    case "Action":
                        if (e.Button == MouseButtons.Right)
                    {
                        addNewActionObjectiveToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                    break;
                    case "AICamp":
                        if (e.Button == MouseButtons.Right)
                    {
                        addNewAICampObjectiveToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                    break;
                    case "AIPatrol":
                        if (e.Button == MouseButtons.Right)
                    {
                        addNewAIPatrolObjectiveToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                    break;
                    case "AIVIP":
                        if (e.Button == MouseButtons.Right)
                    {
                        addNewAiVIPObjectiveToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                    break;
                    case "Collection":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewCollectionObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Crafting":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewCraftingObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Delivery":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewDeliveryObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Target":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewTargetObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Travel":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewTravelObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "TreasureHunt":
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewTreasureHuntObjectiveToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                }
            }
            else
            {
                CurrentTreeNodeTag = e.Node.Tag as QuestObjectivesBase;
                QuestObjectivesFilenameTB.Text = CurrentTreeNodeTag.Filename;
                QuestObjectivesConfigVersionNUD.Value = CurrentTreeNodeTag.ConfigVersion;
                QuestsObjectivesIDNUD.Value = CurrentTreeNodeTag.ID;
                QuestObjectivesObjectiveTypeCB.SelectedItem = (QuExpansionQuestObjectiveTypeestType)CurrentTreeNodeTag.ObjectiveType;
                foreach (GroupBox gpBox in flowLayoutPanel3.Controls.OfType<GroupBox>())
                {
                    if (gpBox.Name == "QuestObjectivesBaseInfoGB") continue;
                    gpBox.Visible = false;
                }
                switch (CurrentTreeNodeTag._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        ObjectivesTargetGB.Visible = true;
                        setupObjectiveTarget(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        ObjectivesTravelGB.Visible = true;
                        SetupObjectiveTravel(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        ObjectivesCollectionGB.Visible = true;
                        setupObjectiveCollection(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        ObjectivesCraftingGB.Visible = true;
                        setupobjectivecrafting(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        ObejctiovesDeliveryGB.Visible = true;
                        setupobjectiveDelivery(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        ObjectivesTresureHuntGB.Visible = true;
                        SetupobjectiveTreasueHunt(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        ObjectivesAIVIPGB.Visible = true;
                        SetupObjectiveAIVIP(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        ObjectivesAIPatrolGB.Visible = true;
                        SetupobjectiveAIPatrol(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        ObjectivesAICampGB.Visible = true;
                        SetupObjectiveAICamp(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        ObjectiveActionGB.Visible = true;
                        SetupObjectiveAction(e);
                        break;
                    default:
                        break;
                }
                if (e.Button == MouseButtons.Right)
                {
                    deleteObjectiveToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }

            }
            useraction = true;
        }
        private void QuestObjectivesObjectiveTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            switch (CurrentTreeNodeTag._ObjectiveTypeEnum)
            {
                case QuExpansionQuestObjectiveTypeestType.TARGET:
                    QuestObjectivesTarget t = CurrentTreeNodeTag as QuestObjectivesTarget;
                    t.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    t.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                    QuestObjectivesTravel tr = CurrentTreeNodeTag as QuestObjectivesTravel;
                    tr.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    tr.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.COLLECT:
                    QuestObjectivesCollection coll = CurrentTreeNodeTag as QuestObjectivesCollection;
                    coll.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    coll.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                    QuestObjectivesDelivery del = CurrentTreeNodeTag as QuestObjectivesDelivery;
                    del.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    del.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                    QuestObjectivesTreasureHunt th = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                    th.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    th.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                    QuestObjectivesAIPatrol aip = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                    aip.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    aip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AICAMP:
                    QuestObjectivesAICamp aic = CurrentTreeNodeTag as QuestObjectivesAICamp;
                    aic.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    aic.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIVIP:
                    QuestObjectivesAIVIP aivip = CurrentTreeNodeTag as QuestObjectivesAIVIP;
                    aivip.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    aivip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.ACTION:
                    QuestObjectivesAction a = CurrentTreeNodeTag as QuestObjectivesAction;
                    a.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    a.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                    QuestObjectivesCrafting c = CurrentTreeNodeTag as QuestObjectivesCrafting;
                    c.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
                    c.isDirty = true;
                    break;
                default:
                    break;
            }

        }
        private void QuestObjectivesTimeLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            switch (CurrentTreeNodeTag._ObjectiveTypeEnum)
            {
                case QuExpansionQuestObjectiveTypeestType.TARGET:
                    QuestObjectivesTarget t = CurrentTreeNodeTag as QuestObjectivesTarget;
                    t.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    t.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                    QuestObjectivesTravel tr = CurrentTreeNodeTag as QuestObjectivesTravel;
                    tr.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    tr.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.COLLECT:
                    QuestObjectivesCollection coll = CurrentTreeNodeTag as QuestObjectivesCollection;
                    coll.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    coll.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                    QuestObjectivesDelivery del = CurrentTreeNodeTag as QuestObjectivesDelivery;
                    del.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    del.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                    QuestObjectivesTreasureHunt th = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                    th.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    th.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                    QuestObjectivesAIPatrol aip = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                    aip.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    aip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AICAMP:
                    QuestObjectivesAICamp aic = CurrentTreeNodeTag as QuestObjectivesAICamp;
                    aic.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    aic.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIVIP:
                    QuestObjectivesAIVIP aivip = CurrentTreeNodeTag as QuestObjectivesAIVIP;
                    aivip.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    aivip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.ACTION:
                    QuestObjectivesAction a = CurrentTreeNodeTag as QuestObjectivesAction;
                    a.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    a.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                    QuestObjectivesCrafting c = CurrentTreeNodeTag as QuestObjectivesCrafting;
                    c.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
                    c.isDirty = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Objective actions
        /// </summary>
        private void addNewActionObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.ACTION);
            QuestObjectivesAction newactionobjective = new QuestObjectivesAction()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_A_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.ACTION,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.ACTION,
                ObjectiveText = "New Action Objective",
                TimeLimit = -1,
                ActionNames = new BindingList<string>(),
                AllowedClassNames = new BindingList<string>(),
                ExcludedClassNames = new BindingList<string>(),
                ExecutionAmount = 1,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newactionobjective);
            TreeNode newnode = new TreeNode(newactionobjective.Filename)
            {
                Tag = newactionobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewAICampObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.AICAMP);
            QuestObjectivesAICamp newAICampobjective = new QuestObjectivesAICamp()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_AIC_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AICAMP,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AICAMP,
                ObjectiveText = "New AICamp Objective",
                TimeLimit = -1,
                AICamp = new Aicamp()
                {
                    Positions = new BindingList<float[]>(),
                    NPCSpeed = "JOG",
                    NPCMode = "HALT",
                    NPCFaction = "West",
                    NPCLoadoutFile = "BanditLoadout",
                    NPCAccuracyMin = 0,
                    NPCAccuracyMax = 0,
                    ClassNames = new BindingList<string>(),
                    SpecialWeapon = 0,
                    AllowedWeapons = new BindingList<string>()
                },
                MinDistRadius = 50,
                MaxDistRadius = 150,
                DespawnRadius = 880,
                CanLootAI = 1,
                InfectedDeletionRadius = 500,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newAICampobjective);
            TreeNode newnode = new TreeNode(newAICampobjective.Filename)
            {
                Tag = newAICampobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewAIPatrolObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.AIPATROL);
            QuestObjectivesAIPatrol newAIPatrolobjective = new QuestObjectivesAIPatrol()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_AIP_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AIPATROL,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIPATROL,
                ObjectiveText = "New AIPatrol Objective",
                TimeLimit = -1,
                AIPatrol = new AIPatrol()
                {
                    NPCUnits = 4,
                    Waypoints = new BindingList<float[]>(),
                    NPCSpeed = "JOG",
                    NPCMode = "HALT",
                    NPCFaction = "West",
                    NPCFormation = "RANDOM",
                    NPCLoadoutFile = "BanditLoadout",
                    NPCAccuracyMin = 0,
                    NPCAccuracyMax = 0,
                    ClassNames = new BindingList<string>(),
                    SpecialWeapon = 0,
                    AllowedWeapons = new BindingList<string>()
                },
                MinDistRadius = 50,
                MaxDistRadius = 150,
                DespawnRadius = 880,
                CanLootAI = 1,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newAIPatrolobjective);
            TreeNode newnode = new TreeNode(newAIPatrolobjective.Filename)
            {
                Tag = newAIPatrolobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewAiVIPObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.AIVIP);
            QuestObjectivesAIVIP newAIVIPobjective = new QuestObjectivesAIVIP()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_AIVIP_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AIVIP,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIVIP,
                ObjectiveText = "New AIVIP Objective",
                TimeLimit = -1,
                Position = new float[] { 0,0,0},
                MaxDistance = -1,
                AIVIP = new AIVIP()
                {
                    NPCLoadoutFile = "BanditLoadout",
                },
                MarkerName = "",
                ShowDistance = 0,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newAIVIPobjective);
            TreeNode newnode = new TreeNode(newAIVIPobjective.Filename)
            {
                Tag = newAIVIPobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewCollectionObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.COLLECT);
            QuestObjectivesCollection newCollectionobjective = new QuestObjectivesCollection()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_C_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.COLLECT,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.COLLECT,
                ObjectiveText = "New Collection Objective",
                TimeLimit = -1,
                MaxDistance = 0,
                MarkerName = "",
                ShowDistance = 0,
                Collections = new BindingList<Collections>(),
                NeedAnyCollection = 0,
                AddItemsToNearbyMarketZone = 0,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newCollectionobjective);
            TreeNode newnode = new TreeNode(newCollectionobjective.Filename)
            {
                Tag = newCollectionobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewCraftingObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.CRAFTING);
            QuestObjectivesCrafting newCraftingobjective = new QuestObjectivesCrafting()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_CR_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.CRAFTING,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.CRAFTING,
                ObjectiveText = "New Crafting Objective",
                TimeLimit = -1,
                ItemNames = new BindingList<string>(),
                ExecutionAmount = 1,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newCraftingobjective);
            TreeNode newnode = new TreeNode(newCraftingobjective.Filename)
            {
                Tag = newCraftingobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewDeliveryObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.DELIVERY);
            QuestObjectivesDelivery newdeliveryobjective = new QuestObjectivesDelivery()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_D_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.DELIVERY,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.DELIVERY,
                ObjectiveText = "New Delivery Objective",
                TimeLimit = -1,
                Collections = new BindingList<Delivery>(),
                MaxDistance = 50,
                MarkerName = "",
                ShowDistance = 0,
                AddItemsToNearbyMarketZone = 0,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newdeliveryobjective);
            TreeNode newnode = new TreeNode(newdeliveryobjective.Filename)
            {
                Tag = newdeliveryobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewTargetObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.TARGET);
            QuestObjectivesTarget newTargetobjective = new QuestObjectivesTarget()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_TA_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TARGET,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TARGET,
                ObjectiveText = "New Target Objective",
                TimeLimit = -1,
                Position = new float[] { 0,0,0},
                MaxDistance = -1,
                Target = new Target()
                {
                    Amount = 0,
                    ClassNames = new BindingList<string>(),
                    CountSelfKill = 0,
                    CountAIPlayers = 0,
                    SpecialWeapon = 0,
                    AllowedWeapons = new BindingList<string>(),
                    ExcludedClassNames = new BindingList<string>()
                },
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newTargetobjective);
            TreeNode newnode = new TreeNode(newTargetobjective.Filename)
            {
                Tag = newTargetobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewTravelObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.TRAVEL);
            QuestObjectivesTravel newTravelobjective = new QuestObjectivesTravel()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_T_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TRAVEL,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TRAVEL,
                ObjectiveText = "New Travel Objective",
                TimeLimit = -1,
                Position = new float[] { 0, 0, 0 },
                MaxDistance = -1,
                MarkerName = "",
                ShowDistance = 0,
                TriggerOnEnter = 0,
                TriggerOnExit = 0,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newTravelobjective);
            TreeNode newnode = new TreeNode(newTravelobjective.Filename)
            {
                Tag = newTravelobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void addNewTreasureHuntObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newid = QuestObjectives.GetNextQuestID(QuExpansionQuestObjectiveTypeestType.TREASUREHUNT);
            QuestObjectivesTreasureHunt newTreasureHuntobjective = new QuestObjectivesTreasureHunt()
            {
                ConfigVersion = QuestObjectivesBase.GetconfigVersion,
                ID = newid,
                Filename = "Objective_TH_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TREASUREHUNT,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TREASUREHUNT,
                ObjectiveText = "New TreasureHunt Objective",
                TimeLimit = -1,
                ShowDistance = 0,
                ContainerName = "ExpansionQuestSeaChest",
                DigInStash = 1,
                MarkerName = "???",
                MarkerVisibility = 4,
                Positions = new BindingList<float[]>(),
                Loot = new BindingList<TreasureHuntItems>(),
                LootitemsAmount = 2,
                MaxDistance= 10,
                isDirty = true
            };
            QuestObjectives.Objectives.Add(newTreasureHuntobjective);
            TreeNode newnode = new TreeNode(newTreasureHuntobjective.Filename)
            {
                Tag = newTreasureHuntobjective
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
        }
        private void deleteObjectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTreeNodeTag = treeViewMS1.SelectedNode.Tag as QuestObjectivesBase;
            QuestObjectives.deleteObjective(CurrentTreeNodeTag);
            treeViewMS1.SelectedNode.Parent.Nodes.Remove(treeViewMS1.SelectedNode);
        }

        /// <summary>
        /// Action Objectives
        /// </summary>
        /// <param name="e"></param>
        private void SetupObjectiveAction(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAction CurrentAction = e.Node.Tag as QuestObjectivesAction;
            useraction = false;

            ObjectivesActionsExecutionAmountNUD.Value = CurrentAction.ExecutionAmount;

            QuestObjectivesObjectiveTextTB.Text = CurrentAction.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentAction.TimeLimit;
            ObjectivesActionnamesLB.DisplayMember = "DisplayName";
            ObjectivesActionnamesLB.ValueMember = "Value";
            ObjectivesActionnamesLB.DataSource = CurrentAction.ActionNames;

            ObjectivesActionAllowedNamesLB.DisplayMember = "DisplayName";
            ObjectivesActionAllowedNamesLB.ValueMember = "Value";
            ObjectivesActionAllowedNamesLB.DataSource = CurrentAction.AllowedClassNames;

            ObjectivesActionExcludedNamesLB.DisplayMember = "DisplayName";
            ObjectivesActionExcludedNamesLB.ValueMember = "Value";
            ObjectivesActionExcludedNamesLB.DataSource = CurrentAction.ExcludedClassNames;

            useraction = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentAction.ActionNames.Add(l);
                }
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void darkButton38_Click(object sender, EventArgs e)
        {
            if (ObjectivesActionnamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            for (int i = 0; i < ObjectivesActionnamesLB.SelectedItems.Count; i++)
            {
                CurrentAction.ActionNames.Remove(ObjectivesActionnamesLB.GetItemText(ObjectivesActionnamesLB.SelectedItems[0]));
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentAction.AllowedClassNames.Add(l);
                }
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            if (ObjectivesActionAllowedNamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            for (int i = 0; i < ObjectivesActionAllowedNamesLB.SelectedItems.Count; i++)
            {
                CurrentAction.AllowedClassNames.Remove(ObjectivesActionAllowedNamesLB.GetItemText(ObjectivesActionAllowedNamesLB.SelectedItems[0]));
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentAction.ExcludedClassNames.Add(l);
                }
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (ObjectivesActionExcludedNamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            for (int i = 0; i < ObjectivesActionExcludedNamesLB.SelectedItems.Count; i++)
            {
                CurrentAction.ExcludedClassNames.Remove(ObjectivesActionExcludedNamesLB.GetItemText(ObjectivesActionExcludedNamesLB.SelectedItems[0]));
            }
            CurrentTreeNodeTag.isDirty = true;
        }
        private void ObjectivesActionsExecutionAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            CurrentAction.ExecutionAmount = (int)ObjectivesActionsExecutionAmountNUD.Value;
            CurrentAction.isDirty = true;
        }
        /// <summary>
        /// AI Camp
        /// </summary>
        private void SetupObjectiveAICamp(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesAICamp CurrentAICamp = e.Node.Tag as QuestObjectivesAICamp;
            QuestObjectivesObjectiveTextTB.Text = CurrentAICamp.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentAICamp.TimeLimit;
            ObjectiovesAICampNPCSpeedCB.SelectedIndex = ObjectiovesAICampNPCSpeedCB.FindStringExact(CurrentAICamp.AICamp.NPCSpeed);
            ObjectivesAICampNPCModeCB.SelectedIndex = ObjectivesAICampNPCModeCB.FindStringExact(CurrentAICamp.AICamp.NPCMode);
            ObjectivesAICampNPCFactionCB.SelectedIndex = ObjectivesAICampNPCFactionCB.FindStringExact(CurrentAICamp.AICamp.NPCFaction);
            ObjectivesAICampNPCLoadoutFileCB.SelectedIndex = ObjectivesAICampNPCLoadoutFileCB.FindStringExact(CurrentAICamp.AICamp.NPCLoadoutFile);
            ObjectivesAICampNPCAccuracyMaxNUD.Value = CurrentAICamp.AICamp.NPCAccuracyMax;
            ObjectivesAICampNPCAccuracyMinNUD.Value = CurrentAICamp.AICamp.NPCAccuracyMin;
            ObjectivesAICampSpecialWeaponCB.Checked = CurrentAICamp.AICamp.SpecialWeapon == 1 ? true : false;
            ObjectivesAICampMinDistRadiusNUD.Value = CurrentAICamp.MinDistRadius;
            ObjectivesAICampMaxDistRadiusNUD.Value = CurrentAICamp.MaxDistRadius;
            ObjectivesAICampDespawnRadiusNUD.Value = CurrentAICamp.DespawnRadius;
            ObjectiovesAICampCanLootAICB.Checked = CurrentAICamp.CanLootAI == 1 ? true : false;
            QuestObjectovesInfectedDeletionRadiusNUD.Value = CurrentAICamp.InfectedDeletionRadius;

            ObjectivesAICampPositionsLB.DisplayMember = "DisplayName";
            ObjectivesAICampPositionsLB.ValueMember = "Value";
            ObjectivesAICampPositionsLB.DataSource = CurrentAICamp.AICamp.Positions;

            ObjectivesAiCampClassnamesLB.DisplayMember = "DisplayName";
            ObjectivesAiCampClassnamesLB.ValueMember = "Value";
            ObjectivesAiCampClassnamesLB.DataSource = CurrentAICamp.AICamp.ClassNames;

            ObjectivesAICampAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesAICampAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesAICampAllowedWeaponsLB.DataSource = CurrentAICamp.AICamp.AllowedWeapons;
            useraction = true;
        }
        private void ObjectivesAICampPositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesAICampPositionsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = ObjectivesAICampPositionsLB.SelectedItem as float[];
            useraction = false;
            numericUpDown9.Value = (decimal)CurrentWapypoint[0];
            numericUpDown11.Value = (decimal)CurrentWapypoint[1];
            numericUpDown12.Value = (decimal)CurrentWapypoint[2];
            useraction = true;

        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.Positions.Add(new float[] { 0, 0, 0 });
            CurrentAICam.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;

            CurrentAICam.AICamp.Positions.Remove(CurrentWapypoint);
            CurrentAICam.isDirty = true;
            ObjectivesAICampPositionsLB.Refresh();
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentAICam.AICamp.Positions.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        float[] newfloatarray = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                        CurrentAICam.AICamp.Positions.Add(newfloatarray);

                    }
                    ObjectivesAICampPositionsLB.SelectedIndex = -1;
                    ObjectivesAICampPositionsLB.SelectedIndex = ObjectivesAICampPositionsLB.Items.Count - 1;
                    ObjectivesAICampPositionsLB.Refresh();
                    CurrentAICam.isDirty = true;
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            StringBuilder SB = new StringBuilder();
            foreach (float[] array in CurrentAICam.AICamp.Positions)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentAICam.AICamp.Positions.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        float[] newfloatarray = new float[] { Convert.ToSingle(eo.Position[0]), Convert.ToSingle(eo.Position[1]), Convert.ToSingle(eo.Position[2]) };
                        CurrentAICam.AICamp.Positions.Add(newfloatarray);
                    }
                    ObjectivesAICampPositionsLB.SelectedIndex = -1;
                    ObjectivesAICampPositionsLB.SelectedIndex = ObjectivesAICampPositionsLB.Items.Count - 1;
                    ObjectivesAICampPositionsLB.Refresh();
                    CurrentAICam.isDirty = true;
                }
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in CurrentAICam.AICamp.Positions)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = array,
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentWapypoint[0] = (float)numericUpDown9.Value;
            CurrentAICam.isDirty = true;
        }
        private void QuestObjectovesInfectedDeletionRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.InfectedDeletionRadius = (int)QuestObjectovesInfectedDeletionRadiusNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentWapypoint[1] = (float)numericUpDown11.Value;
            CurrentAICam.isDirty = true;
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentWapypoint[2] = (float)numericUpDown12.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectiovesAICampNPCSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCSpeed = ObjectiovesAICampNPCSpeedCB.GetItemText(ObjectiovesAICampNPCSpeedCB.SelectedItem);
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampNPCModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCMode = ObjectivesAICampNPCModeCB.GetItemText(ObjectivesAICampNPCModeCB.SelectedItem);
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampNPCFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCFaction = ObjectivesAICampNPCFactionCB.GetItemText(ObjectivesAICampNPCFactionCB.SelectedItem);
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampNPCLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCLoadoutFile = ObjectivesAICampNPCLoadoutFileCB.GetItemText(ObjectivesAICampNPCLoadoutFileCB.SelectedItem);
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampNPCAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCAccuracyMin = ObjectivesAICampNPCAccuracyMinNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampNPCAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.NPCAccuracyMax = ObjectivesAICampNPCAccuracyMaxNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if(!CurrentAICam.AICamp.ClassNames.Contains(l))
                    CurrentAICam.AICamp.ClassNames.Add(l);
                }
            }
            CurrentAICam.isDirty = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            if (ObjectivesAiCampClassnamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            for (int i = 0; i < ObjectivesAiCampClassnamesLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AICamp.ClassNames.Remove(ObjectivesAiCampClassnamesLB.GetItemText(ObjectivesAiCampClassnamesLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampSpecialWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.AICamp.SpecialWeapon = ObjectivesAICampSpecialWeaponCB.Checked == true ? 1 : 0;
            CurrentAICam.isDirty = true;
        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
                foreach (string l in addedtypes)
                {
                    if (!CurrentAICam.AICamp.AllowedWeapons.Contains(l))
                    {
                        CurrentAICam.AICamp.AllowedWeapons.Add(l);
                        CurrentAICam.isDirty = true;
                    }
                }
                ObjectivesAICampAllowedWeaponsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            if (ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count < 1) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            for (int i = 0; i < ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AICamp.AllowedWeapons.Remove(ObjectivesAICampAllowedWeaponsLB.GetItemText(ObjectivesAICampAllowedWeaponsLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.MinDistRadius = ObjectivesAICampMinDistRadiusNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.MaxDistRadius = ObjectivesAICampMaxDistRadiusNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.DespawnRadius = ObjectivesAICampDespawnRadiusNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectiovesAICampCanLootAICB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.CanLootAI = ObjectiovesAICampCanLootAICB.Checked == true ? 1:0;
            CurrentAICam.isDirty = true;
        }
        /// <summary>
        /// AI Patrol
        /// </summary>
        private void SetupobjectiveAIPatrol(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesAIPatrol CurrentAIPatrol = e.Node.Tag as QuestObjectivesAIPatrol;
            ObjectivesAIPatrolNPCUnitsNUD.Value = CurrentAIPatrol.AIPatrol.NPCUnits;
            ObjectiovesAIPatrolNPCSpeedCB.SelectedIndex = ObjectiovesAIPatrolNPCSpeedCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCSpeed);
            ObjectivesAIPatrolNPCModeCB.SelectedIndex = ObjectivesAIPatrolNPCModeCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCMode);
            ObjectivesAIPatrolNPCFactionCB.SelectedIndex = ObjectivesAIPatrolNPCFactionCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCFaction);
            ObjectivesAIPatrolNPCFormationCB.SelectedIndex = ObjectivesAIPatrolNPCFormationCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCFormation);
            ObjectivesAIPatrolNPCLoadoutFileCB.SelectedIndex = ObjectivesAIPatrolNPCLoadoutFileCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCLoadoutFile);
            ObjectivesAIPatrolNPCAccuracyMaxNUD.Value = CurrentAIPatrol.AIPatrol.NPCAccuracyMax;
            ObjectivesAIPatrolNPCAccuracyMinNUD.Value = CurrentAIPatrol.AIPatrol.NPCAccuracyMin;
            ObjectivesAIPatrolSpecialWeaponCB.Checked = CurrentAIPatrol.AIPatrol.SpecialWeapon == 1 ? true : false;
            ObjectivesAIPatrolMinDistRadiusNUD.Value = CurrentAIPatrol.MinDistRadius;
            ObjectivesAIPatrolMaxDistRadiusNUD.Value = CurrentAIPatrol.MaxDistRadius;
            ObjectivesAIPatrolDespawnRadiusNUD.Value = CurrentAIPatrol.DespawnRadius;
            ObjectiovesAIPatrolCanLootAICB.Checked = CurrentAIPatrol.CanLootAI == 1 ? true : false;

            ObjectivesAIPatrolWaypointsLB.DisplayMember = "DisplayName";
            ObjectivesAIPatrolWaypointsLB.ValueMember = "Value";
            ObjectivesAIPatrolWaypointsLB.DataSource = CurrentAIPatrol.AIPatrol.Waypoints;

            ObjectivesAiPatrolClassnamesLB.DisplayMember = "DisplayName";
            ObjectivesAiPatrolClassnamesLB.ValueMember = "Value";
            ObjectivesAiPatrolClassnamesLB.DataSource = CurrentAIPatrol.AIPatrol.ClassNames;

            ObjectivesAIPatrolAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesAIPatrolAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesAIPatrolAllowedWeaponsLB.DataSource = CurrentAIPatrol.AIPatrol.AllowedWeapons;
            useraction = true;
        }
        private void ObjectivesAIPatrolWaypointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesAIPatrolWaypointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = ObjectivesAIPatrolWaypointsLB.SelectedItem as float[];
            useraction = false;
            numericUpDown20.Value = (decimal)CurrentWapypoint[0];
            numericUpDown21.Value = (decimal)CurrentWapypoint[1];
            numericUpDown22.Value = (decimal)CurrentWapypoint[2];
            useraction = true;
        }
        private void darkButton48_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.Waypoints.Add(new float[] { 0, 0, 0 });
            CurrentAIPatrol.isDirty = true;
        }
        private void darkButton47_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.Waypoints.Remove(CurrentWapypoint);
            CurrentAIPatrol.isDirty = true;
            ObjectivesAIPatrolWaypointsLB.Refresh();
        }
        private void darkButton46_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentAIPatrol.AIPatrol.Waypoints.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        float[] newfloatarray = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                        CurrentAIPatrol.AIPatrol.Waypoints.Add(newfloatarray);

                    }
                    ObjectivesAIPatrolWaypointsLB.SelectedIndex = -1;
                    ObjectivesAIPatrolWaypointsLB.SelectedIndex = ObjectivesAIPatrolWaypointsLB.Items.Count - 1;
                    ObjectivesAIPatrolWaypointsLB.Refresh();
                    CurrentAIPatrol.isDirty = true;
                }
            }
        }
        private void darkButton45_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            StringBuilder SB = new StringBuilder();
            foreach (float[] array in CurrentAIPatrol.AIPatrol.Waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }
        private void darkButton44_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentAIPatrol.AIPatrol.Waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        float[] newfloatarray = new float[] { Convert.ToSingle(eo.Position[0]), Convert.ToSingle(eo.Position[1]), Convert.ToSingle(eo.Position[2]) };
                        CurrentAIPatrol.AIPatrol.Waypoints.Add(newfloatarray);
                    }
                    ObjectivesAIPatrolWaypointsLB.SelectedIndex = -1;
                    ObjectivesAIPatrolWaypointsLB.SelectedIndex = ObjectivesAIPatrolWaypointsLB.Items.Count - 1;
                    ObjectivesAIPatrolWaypointsLB.Refresh();
                    CurrentAIPatrol.isDirty = true;
                }
            }
        }
        private void darkButton43_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in CurrentAIPatrol.AIPatrol.Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = array,
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentWapypoint[2] = (float)numericUpDown20.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentWapypoint[2] = (float)numericUpDown21.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentWapypoint[2] = (float)numericUpDown22.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCUnitsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCUnits = (int)ObjectivesAIPatrolNPCUnitsNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectiovesAIPatrolNPCSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCSpeed = ObjectiovesAIPatrolNPCSpeedCB.GetItemText(ObjectiovesAIPatrolNPCSpeedCB.SelectedItem);
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCMode = ObjectivesAIPatrolNPCModeCB.GetItemText(ObjectivesAIPatrolNPCModeCB.SelectedItem);
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCFaction = ObjectivesAIPatrolNPCFactionCB.GetItemText(ObjectivesAIPatrolNPCFactionCB.SelectedItem);
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCFormation = ObjectivesAIPatrolNPCFormationCB.GetItemText(ObjectivesAIPatrolNPCFormationCB.SelectedItem);
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCLoadoutFile = ObjectivesAIPatrolNPCLoadoutFileCB.GetItemText(ObjectivesAIPatrolNPCLoadoutFileCB.SelectedItem);
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCAccuracyMin = ObjectivesAIPatrolNPCAccuracyMinNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolNPCAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.NPCAccuracyMax = ObjectivesAIPatrolNPCAccuracyMaxNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void darkButton42_Click(object sender, EventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentAIPatrol.AIPatrol.ClassNames.Contains(l))
                        CurrentAIPatrol.AIPatrol.ClassNames.Add(l);
                }
            }
            CurrentAIPatrol.isDirty = true;
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            if (ObjectivesAiPatrolClassnamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            for (int i = 0; i < ObjectivesAiPatrolClassnamesLB.SelectedItems.Count; i++)
            {
                CurrentAIPatrol.AIPatrol.ClassNames.Remove(ObjectivesAiPatrolClassnamesLB.GetItemText(ObjectivesAiPatrolClassnamesLB.SelectedItems[0]));
            }
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolSpecialWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.AIPatrol.SpecialWeapon = ObjectivesAIPatrolSpecialWeaponCB.Checked == true ? 1 : 0;
            CurrentAIPatrol.isDirty = true;
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                foreach (string l in addedtypes)
                {
                    if (!CurrentAIPatrol.AIPatrol.AllowedWeapons.Contains(l))
                    {
                        CurrentAIPatrol.AIPatrol.AllowedWeapons.Add(l);
                        CurrentAIPatrol.isDirty = true;
                    }
                }
                ObjectivesAIPatrolAllowedWeaponsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            if (ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count < 1) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            for (int i = 0; i < ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count; i++)
            {
                CurrentAIPatrol.AIPatrol.AllowedWeapons.Remove(ObjectivesAICampAllowedWeaponsLB.GetItemText(ObjectivesAICampAllowedWeaponsLB.SelectedItems[0]));
            }
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.MinDistRadius = ObjectivesAIPatrolMinDistRadiusNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.MaxDistRadius = ObjectivesAIPatrolMaxDistRadiusNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectivesAIPatrolDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.DespawnRadius = ObjectivesAIPatrolDespawnRadiusNUD.Value;
            CurrentAIPatrol.isDirty = true;
        }
        private void ObjectiovesAIPatrolCanLootAICB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.CanLootAI = ObjectiovesAIPatrolCanLootAICB.Checked == true ? 1 : 0;
            CurrentAIPatrol.isDirty = true;
        }
        /// <summary>
        /// AI VIP
        /// </summary>
        private void SetupObjectiveAIVIP(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesAIVIP CurrentAIVIP = e.Node.Tag as QuestObjectivesAIVIP;
            QuestObjectivesObjectiveTextTB.Text = CurrentAIVIP.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentAIVIP.TimeLimit;
            ObjectivesAIVIPPositionXNUD.Value = (decimal)CurrentAIVIP.Position[0];
            ObjectivesAIVIPPositionYNUD.Value = (decimal)CurrentAIVIP.Position[1];
            ObjectivesAIVIPPositionZNUD.Value = (decimal)CurrentAIVIP.Position[2];
            ObjectivesAIVIPMaxDistanceNUD.Value = CurrentAIVIP.MaxDistance;
            ObjectivesAIVIPNPCLoadoutFileCB.SelectedIndex = ObjectivesAIVIPNPCLoadoutFileCB.FindStringExact(CurrentAIVIP.AIVIP.NPCLoadoutFile);
            ObjectivesAIVIPMarkerNameTB.Text = CurrentAIVIP.MarkerName;
            QuestObjectivesAIVIPShowDistanceCB.Checked = CurrentAIVIP.ShowDistance == 1 ? true : false;
            useraction = true;
        }
        private void ObjectivesAIVIPPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[0] = (float)ObjectivesAIVIPPositionXNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[1] = (float)ObjectivesAIVIPPositionYNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[2] = (float)ObjectivesAIVIPPositionZNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.MaxDistance = ObjectivesAIVIPMaxDistanceNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPNPCLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.AIVIP.NPCLoadoutFile = ObjectivesAIVIPNPCLoadoutFileCB.GetItemText(ObjectivesAIVIPNPCLoadoutFileCB.SelectedItem);
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPMarkerNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.MarkerName = ObjectivesAIVIPMarkerNameTB.Text;
            CurrentAIVIP.isDirty = true;
        }
        private void QuestObjectivesAIVIPShowDistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.ShowDistance = QuestObjectivesAIVIPShowDistanceCB.Checked == true ? 1 : 0;
            CurrentAIVIP.isDirty = true;
        }
        /// <summary>
        /// Collection
        /// </summary>
        private void setupObjectiveCollection(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesCollection CurrentCollection = e.Node.Tag as QuestObjectivesCollection;
            QuestObjectivesObjectiveTextTB.Text = CurrentCollection.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentCollection.TimeLimit;
            ObjectivesCollectionMaxDistanceNUD.Value = CurrentCollection.MaxDistance;
            ObjectivesCollectionMarkernameTB.Text = CurrentCollection.MarkerName;
            ObjectivesCollectionShowDistanceCB.Checked = CurrentCollection.ShowDistance == 1 ? true : false;
            checkBox1.Checked = CurrentCollection.AddItemsToNearbyMarketZone == 1 ? true : false;
            checkBox5.Checked = CurrentCollection.NeedAnyCollection == 1 ? true : false;

            ObjectivesCollectionCollectionsLB.DisplayMember = "DisplayName";
            ObjectivesCollectionCollectionsLB.ValueMember = "Value";
            ObjectivesCollectionCollectionsLB.DataSource = CurrentCollection.Collections;

            useraction = true;
        }
        public Collections CurrentCollections;
        private void ObjectivesCollectionCollectionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesCollectionCollectionsLB.SelectedItems.Count < 1) return;
            CurrentCollections = ObjectivesCollectionCollectionsLB.SelectedItem as Collections;
            useraction = false;
            ObjectivesCollectionClassnameTB.Text = CurrentCollections.ClassName;
            ObjectivesCollectionAmountNUD.Value = CurrentCollections.Amount;
            useraction = true;
        }
        private void ObjectivesCollectionMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.MaxDistance = ObjectivesCollectionMaxDistanceNUD.Value;
            CurrentCollection.isDirty = true;
        }
        private void ObjectivesCollectionMarkernameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.MarkerName = ObjectivesCollectionMarkernameTB.Text;
            CurrentCollection.isDirty = true;
        }
        private void ObjectivesCollectionShowDistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.ShowDistance = ObjectivesCollectionShowDistanceCB.Checked == true ? 1 : 0;
            CurrentCollection.isDirty = true;
        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
                foreach (string l in addedtypes)
                {
                    Collections newcollection = new Collections()
                    {
                        ClassName = l,
                        Amount = 1
                    };
                    CurrentCollection.Collections.Add(newcollection);
                    ObjectivesCollectionCollectionsLB.Refresh();
                }
                CurrentCollection.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton50_Click(object sender, EventArgs e)
        {
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.Collections.Remove(CurrentCollections);
            CurrentCollection.isDirty = true;
            ObjectivesCollectionCollectionsLB.Refresh();
        }
        private void ObjectivesCollectionAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollections.Amount = (int)ObjectivesCollectionAmountNUD.Value;
            CurrentCollection.isDirty = true;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
                
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ObjectivesCollectionClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ObjectivesCollectionClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollections.ClassName = ObjectivesCollectionClassnameTB.Text;
            CurrentCollection.isDirty = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.AddItemsToNearbyMarketZone = checkBox1.Checked == true ? 1 : 0;
            CurrentCollection.isDirty = true;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollection.NeedAnyCollection = checkBox5.Checked == true ? 1 : 0;
            CurrentCollection.isDirty = true;
        }
        /// <summary>
        /// Crafting
        /// </summary>
        private void setupobjectivecrafting(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesCrafting CurrentCrafting = e.Node.Tag as QuestObjectivesCrafting;
            QuestObjectivesObjectiveTextTB.Text = CurrentCrafting.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentCrafting.TimeLimit;
            numericUpDown15.Value = CurrentCrafting.ExecutionAmount;
            ObjectivesCraftingLB.DisplayMember = "DisplayName";
            ObjectivesCraftingLB.ValueMember = "Value";
            ObjectivesCraftingLB.DataSource = CurrentCrafting.ItemNames;

            useraction = true;
        }
        private void darkButton53_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesCrafting CurrentCrafting = CurrentTreeNodeTag as QuestObjectivesCrafting;
                foreach (string l in addedtypes)
                {
                    if (!CurrentCrafting.ItemNames.Contains(l))
                    {
                        CurrentCrafting.ItemNames.Add(l);
                        ObjectivesCraftingLB.Refresh();
                    }
                }
                CurrentCrafting.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton52_Click(object sender, EventArgs e)
        {
            if (ObjectivesCraftingLB.SelectedItems.Count < 1) return;
            QuestObjectivesCrafting CurrentCrafting = CurrentTreeNodeTag as QuestObjectivesCrafting;
            for (int i = 0; i < ObjectivesCraftingLB.SelectedItems.Count; i++)
            {
                CurrentCrafting.ItemNames.Remove(ObjectivesCraftingLB.GetItemText(ObjectivesCraftingLB.SelectedItems[0]));
            }
            CurrentCrafting.isDirty = true;
        }
        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCrafting CurrentCrafting = CurrentTreeNodeTag as QuestObjectivesCrafting;
            CurrentCrafting.ExecutionAmount = (int)numericUpDown15.Value;
            CurrentCrafting.isDirty = true;
        }
        /// <summary>
        /// Delivery
        /// </summary>
        private void setupobjectiveDelivery(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesDelivery CurrentDelivery = e.Node.Tag as QuestObjectivesDelivery;
            QuestObjectivesObjectiveTextTB.Text = CurrentDelivery.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentDelivery.TimeLimit;
            ObjectivesDeliveryMaxDistanceNUD.Value = CurrentDelivery.MaxDistance;
            ObjectivesDeliveryMarkerNameTB.Text = CurrentDelivery.MarkerName;
            ObjectivesDeliveryShowDistanceCB.Checked = CurrentDelivery.ShowDistance == 1 ? true : false;
            checkBox6.Checked = CurrentDelivery.AddItemsToNearbyMarketZone == 1 ? true : false;
            ObjectivesDeliveryDeliveriesLB.DisplayMember = "DisplayName";
            ObjectivesDeliveryDeliveriesLB.ValueMember = "Value";
            ObjectivesDeliveryDeliveriesLB.DataSource = CurrentDelivery.Collections;

            useraction = true;
        }
        public Delivery CurrentDeliveries;
        private void ObjectivesDeliveryDeliveriesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesDeliveryDeliveriesLB.SelectedItems.Count < 1) return;
            CurrentDeliveries = ObjectivesDeliveryDeliveriesLB.SelectedItem as Delivery;
            useraction = false;
            ObjectivesDeliveryClassnameTB.Text = CurrentDeliveries.ClassName;
            ObjectivesDeliveryAmountNUD.Value = CurrentDeliveries.Amount;
            useraction = true;
        }
        private void ObjectivesDeliveryMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.MaxDistance = ObjectivesCollectionMaxDistanceNUD.Value;
            CurrentDelivery.isDirty = true;
        }
        private void ObjectivesDeliveryMarkerNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.MarkerName = ObjectivesDeliveryMarkerNameTB.Text;
            CurrentDelivery.isDirty = true;
        }
        private void ObjectivesDeliveryShowDistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.ShowDistance = ObjectivesDeliveryShowDistanceCB.Checked == true ? 1 : 0;
            CurrentDelivery.isDirty = true;
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.AddItemsToNearbyMarketZone = checkBox6.Checked == true ? 1 : 0;
            CurrentDelivery.isDirty = true;
        }
        private void darkButton55_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
                foreach (string l in addedtypes)
                {
                    Delivery newDelivery = new Delivery()
                    {
                        ClassName = l,
                        Amount = 1
                    };
                    CurrentDelivery.Collections.Add(newDelivery);
                    ObjectivesDeliveryDeliveriesLB.Refresh();
                }
                CurrentDelivery.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton54_Click(object sender, EventArgs e)
        {
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.Collections.Remove(CurrentDeliveries);
            CurrentDelivery.isDirty = true;
            ObjectivesDeliveryDeliveriesLB.Refresh();
        }
        private void ObjectivesDeliveryAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDeliveries.Amount = (int)ObjectivesDeliveryAmountNUD.Value;
            CurrentDelivery.isDirty = true;
        }
        private void ObjectivesDeliveryClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDeliveries.ClassName = ObjectivesDeliveryClassnameTB.Text;
            CurrentDelivery.isDirty = true;
        }
        private void darkButton56_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true

            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ObjectivesDeliveryClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        /// <summary>
        /// Target
        /// </summary>
        private void setupObjectiveTarget(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesTarget CurrentTarget = e.Node.Tag as QuestObjectivesTarget;
            QuestObjectivesObjectiveTextTB.Text = CurrentTarget.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentTarget.TimeLimit;
            ObjectivesTargetPositionXNUD.Value = (decimal)CurrentTarget.Position[0];
            ObjectivesTargetPositionYNUD.Value = (decimal)CurrentTarget.Position[1];
            ObjectivesTargetPositionZNUD.Value = (decimal)CurrentTarget.Position[2];
            ObjectivesTargetMaxDistanceNUD.Value = CurrentTarget.MaxDistance;
            ObjectivesTargetAmountNUD.Value = CurrentTarget.Target.Amount;
            ObjectivesTargetCountSelfKillCB.Checked = CurrentTarget.Target.CountSelfKill == 1 ? true : false;
            ObjectivesTargetSpecialWeaponCB.Checked = CurrentTarget.Target.SpecialWeapon == 1 ? true : false;
            checkBox7.Checked = CurrentTarget.Target.CountAIPlayers == 1 ? true : false;

            ObjectivesTargetClassnameLB.DisplayMember = "DisplayName";
            ObjectivesTargetClassnameLB.ValueMember = "Value";
            ObjectivesTargetClassnameLB.DataSource = CurrentTarget.Target.ClassNames;

            ObjectivesTargetAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesTargetAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesTargetAllowedWeaponsLB.DataSource = CurrentTarget.Target.AllowedWeapons;

            ObjectivesTargetExcludedClassnamesLB.DisplayMember = "DisplayName";
            ObjectivesTargetExcludedClassnamesLB.ValueMember = "Value";
            ObjectivesTargetExcludedClassnamesLB.DataSource = CurrentTarget.Target.ExcludedClassNames;

            useraction = true;

        }
        private void ObjectivesTargetPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[0] = (float)ObjectivesTargetPositionXNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[1] = (float)ObjectivesTargetPositionYNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[2] = (float)ObjectivesTargetPositionZNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.MaxDistance = ObjectivesTargetMaxDistanceNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Target.Amount = (int)ObjectivesTargetAmountNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void darkButton58_Click(object sender, EventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTarget.Target.ClassNames.Contains(l))
                        CurrentTarget.Target.ClassNames.Add(l);
                }
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton57_Click(object sender, EventArgs e)
        {
            if (ObjectivesTargetClassnameLB.SelectedItems.Count < 1) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            for (int i = 0; i < ObjectivesTargetClassnameLB.SelectedItems.Count; i++)
            {
                CurrentTarget.Target.ClassNames.Remove(ObjectivesTargetClassnameLB.GetItemText(ObjectivesTargetClassnameLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetCountSelfKillCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Target.CountSelfKill = ObjectivesTargetCountSelfKillCB.Checked == true ? 1 : 0;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetSpecialWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Target.SpecialWeapon = ObjectivesTargetSpecialWeaponCB.Checked == true ? 1 : 0;
            CurrentTarget.isDirty = true;
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Target.CountAIPlayers = checkBox7.Checked == true ? 1 : 0;
            CurrentTarget.isDirty = true;
        }
        private void darkButton60_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
                foreach (string l in addedtypes)
                {
                    if (!CurrentTarget.Target.AllowedWeapons.Contains(l))
                    {
                        CurrentTarget.Target.AllowedWeapons.Add(l);
                        CurrentTarget.isDirty = true;
                    }
                }
                ObjectivesAICampAllowedWeaponsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void darkButton59_Click(object sender, EventArgs e)
        {
            if (ObjectivesTargetAllowedWeaponsLB.SelectedItems.Count < 1) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            for (int i = 0; i < ObjectivesTargetAllowedWeaponsLB.SelectedItems.Count; i++)
            {
                CurrentTarget.Target.AllowedWeapons.Remove(ObjectivesTargetAllowedWeaponsLB.GetItemText(ObjectivesTargetAllowedWeaponsLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton62_Click(object sender, EventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTarget.Target.ExcludedClassNames.Contains(l))
                        CurrentTarget.Target.ExcludedClassNames.Add(l);
                }
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton61_Click(object sender, EventArgs e)
        {
            if (ObjectivesTargetExcludedClassnamesLB.SelectedItems.Count < 1) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            for (int i = 0; i < ObjectivesTargetExcludedClassnamesLB.SelectedItems.Count; i++)
            {
                CurrentTarget.Target.ExcludedClassNames.Remove(ObjectivesTargetExcludedClassnamesLB.GetItemText(ObjectivesTargetExcludedClassnamesLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        /// <summary>
        /// Travel
        /// </summary>
        private void SetupObjectiveTravel(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTravel CurrentTravel = e.Node.Tag as QuestObjectivesTravel;
            QuestObjectivesObjectiveTextTB.Text = CurrentTravel.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentTravel.TimeLimit;
            ObjectivesTravelPositionXNUD.Value = (decimal)CurrentTravel.Position[0];
            ObjectivesTravelPositionYNUD.Value = (decimal)CurrentTravel.Position[1];
            ObjectivesTravelPositionZNUD.Value = (decimal)CurrentTravel.Position[2];
            ObjectivesTravelMaxDistanceNUD.Value = CurrentTravel.MaxDistance;
            ObjectivesTravelMarkerNameTB.Text = CurrentTravel.MarkerName;
            ObjectivesTravelShowDistanceCB.Checked = CurrentTravel.ShowDistance == 1 ? true : false;
            checkBox8.Checked = CurrentTravel.TriggerOnEnter == 1 ? true : false;
            checkBox9.Checked = CurrentTravel.TriggerOnExit == 1 ? true : false;
        }
        private void ObjectivesTravelPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.Position[0] = (float)ObjectivesTravelPositionXNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.Position[1] = (float)ObjectivesTravelPositionYNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.Position[2] = (float)ObjectivesTravelPositionZNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelShowDistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.ShowDistance = ObjectivesTravelShowDistanceCB.Checked == true ? 1 : 0;
            CurrentTravel.isDirty = true;
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.TriggerOnEnter = checkBox8.Checked == true ? 1 : 0;
            CurrentTravel.isDirty = true;
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.TriggerOnExit = checkBox9.Checked == true ? 1 : 0;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.MaxDistance = ObjectivesTravelMaxDistanceNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelMarkerNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.MarkerName = ObjectivesTravelMarkerNameTB.Text;
            CurrentTravel.isDirty = true;
        }
        /// <summary>
        /// TreasureHunt
        /// </summary>
        public TreasureHuntItems CurrentTreasureHuntItems;
        private void SetupobjectiveTreasueHunt(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = e.Node.Tag as QuestObjectivesTreasureHunt;
            QuestObjectivesObjectiveTextTB.Text = CurrentTreasureHunt.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentTreasureHunt.TimeLimit;
            ObjectivesTreasureHuntShowdistanceCB.Checked = CurrentTreasureHunt.ShowDistance == 1 ? true : false;
            ObjectivesTreasureHuntAmountNUD.Value = CurrentTreasureHunt.LootitemsAmount;
            checkBox10.Checked = CurrentTreasureHunt.DigInStash == 1 ? true : false;
            numericUpDown16.Value = CurrentTreasureHunt.MaxDistance;
            numericUpDown17.Value = CurrentTreasureHunt.MarkerVisibility;
            textBox2.Text = CurrentTreasureHunt.ContainerName;
            textBox1.Text = CurrentTreasureHunt.MarkerName;

            ObjectivesTreasureHuntPositionsLB.DisplayMember = "DisplayName";
            ObjectivesTreasureHuntPositionsLB.ValueMember = "Value";
            ObjectivesTreasureHuntPositionsLB.DataSource = CurrentTreasureHunt.Positions;

            ObjectivesTreasureHuntItemsLB.DisplayMember = "DisplayName";
            ObjectivesTreasureHuntItemsLB.ValueMember = "Value";
            ObjectivesTreasureHuntItemsLB.DataSource = CurrentTreasureHunt.Loot;
        }
        private void ObjectivesTreasureHuntPositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesTreasureHuntPositionsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = ObjectivesTreasureHuntPositionsLB.SelectedItem as float[];
            useraction = false;
            ObjectivesTreasureHuntPositionsXNUD.Value = (decimal)CurrentWapypoint[0];
            ObjectivesTreasureHuntPositionsYNUD.Value = (decimal)CurrentWapypoint[1];
            ObjectivesTreasureHuntPositionsZNUD.Value = (decimal)CurrentWapypoint[2];
            useraction = true;
        }
        private void ObjectivesTreasureHuntItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesTreasureHuntItemsLB.SelectedItems.Count < 1) return;
            CurrentTreasureHuntItems = ObjectivesTreasureHuntItemsLB.SelectedItem as TreasureHuntItems;
            useraction = false;

            if (CurrentTreasureHuntItems.Chance > 1)
                CurrentTreasureHuntItems.Chance = 1;
            ObjectivesTreasureHuntChanceTB.Value = (int)(CurrentTreasureHuntItems.Chance * 1000);
            darkLabel147.Text = ((decimal)(ObjectivesTreasureHuntChanceTB.Value) / 10).ToString() + "%";
            ObjectivesTreasureHuntMaxCountNUD.Value = CurrentTreasureHuntItems.Max;
            ObjectivesTreasureHuntQualityPercentNUD.Value = CurrentTreasureHuntItems.QuantityPercent;

            ObjectivesTreasureHuntAttachmentsLB.DisplayMember = "DisplayName";
            ObjectivesTreasureHuntAttachmentsLB.ValueMember = "Value";
            ObjectivesTreasureHuntAttachmentsLB.DataSource = CurrentTreasureHuntItems.Attachments;

            listBox21.DataSource = null;
            listBox22.DataSource = null;

            if (CurrentTreasureHuntItems.Variants.Count > 0)
            {
                listBox21.DisplayMember = "DisplayName";
                listBox21.ValueMember = "Value";
                listBox21.DataSource = CurrentTreasureHuntItems.Variants;
            }
            useraction = true;
        }
        private void listBox21_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox21.SelectedItems.Count < 1) return;
            LootVarients = listBox21.SelectedItem as treasurehunitemvarients;
            useraction = false;
            VarientChanceTrackBar.Value = (int)(LootVarients.Chance * 1000);
            darkLabel159.Text = ((decimal)(VarientChanceTrackBar.Value) / 10).ToString() + "%";

            listBox22.DisplayMember = "DisplayName";
            listBox22.ValueMember = "Value";
            listBox22.DataSource = LootVarients.Attachments;
            useraction = true;

        }
        private void darkButton68_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.Positions.Add(new float[] { 0, 0, 0 });
            CurrentTreasureHunt.isDirty = true;
        }
        private void darkButton67_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.Positions.Remove(CurrentWapypoint);
            CurrentTreasureHunt.isDirty = true;
            ObjectivesTreasureHuntPositionsLB.Refresh();
        }
        private void darkButton66_Click(object sender, EventArgs e)
        {

            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentTreasureHunt.Positions.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        float[] newfloatarray = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                        CurrentTreasureHunt.Positions.Add(newfloatarray);

                    }
                    ObjectivesTreasureHuntPositionsLB.SelectedIndex = -1;
                    ObjectivesTreasureHuntPositionsLB.SelectedIndex = ObjectivesTreasureHuntPositionsLB.Items.Count - 1;
                    ObjectivesTreasureHuntPositionsLB.Refresh();
                    CurrentTreasureHunt.isDirty = true;
                }
            }
        }
        private void darkButton65_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            StringBuilder SB = new StringBuilder();
            foreach (float[] array in CurrentTreasureHunt.Positions)
            {
                SB.AppendLine("UndergroundStash|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }
        private void darkButton64_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentTreasureHunt.Positions.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        float[] newfloatarray = new float[] { Convert.ToSingle(eo.Position[0]), Convert.ToSingle(eo.Position[1]), Convert.ToSingle(eo.Position[2]) };
                        CurrentTreasureHunt.Positions.Add(newfloatarray);
                    }
                    ObjectivesTreasureHuntPositionsLB.SelectedIndex = -1;
                    ObjectivesTreasureHuntPositionsLB.SelectedIndex = ObjectivesTreasureHuntPositionsLB.Items.Count - 1;
                    ObjectivesTreasureHuntPositionsLB.Refresh();
                    CurrentTreasureHunt.isDirty = true;
                }
            }
        }
        private void darkButton63_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in CurrentTreasureHunt.Positions)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "UndergroundStash",
                    DisplayName = "UndergroundStash",
                    Position = array,
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void ObjectivesTreasureHuntPositionsXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint[0] = (float)ObjectivesTreasureHuntPositionsXNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntPositionsYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint[1] = (float)ObjectivesTreasureHuntPositionsYNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntPositionsZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint[2] = (float)ObjectivesTreasureHuntPositionsZNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntShowdistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.ShowDistance = ObjectivesTreasureHuntShowdistanceCB.Checked == true ? 1 : 0;
            CurrentTreasureHunt.isDirty = true;
        }
        private void darkButton70_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                foreach (string l in addedtypes)
                {
                    TreasureHuntItems newcollection = new TreasureHuntItems()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = (decimal)0.5,
                        QuantityPercent = -1,
                        Max = -1,
                        Variants = new BindingList<treasurehunitemvarients>()
                    };
                    CurrentTreasureHunt.Loot.Add(newcollection);
                    ObjectivesTreasureHuntItemsLB.Refresh();
                }
                CurrentTreasureHunt.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton69_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.Loot.Remove(CurrentTreasureHuntItems);
            CurrentTreasureHunt.isDirty = true;
            ObjectivesTreasureHuntItemsLB.Refresh();
        }
        private void ObjectivesTreasureHuntAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.LootitemsAmount = (int)ObjectivesTreasureHuntAmountNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.DigInStash = checkBox10.Checked == true ? 1 : 0;
            CurrentTreasureHunt.isDirty = true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.ContainerName = textBox2.Text;
            CurrentTreasureHunt.isDirty = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.MarkerName = textBox1.Text;
            CurrentTreasureHunt.isDirty = true;
        }
        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.MaxDistance = (int)numericUpDown16.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.MarkerVisibility = (int)numericUpDown17.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntAttADDButton_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTreasureHuntItems.Attachments.Contains(l))
                    {
                        CurrentTreasureHuntItems.Attachments.Add(l);
                        CurrentTreasureHunt.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the list.....");
                }
            }
        }
        private void ObjectivesTreasureHuntAttREMButton_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHuntItems.Attachments.Remove(ObjectivesTreasureHuntAttachmentsLB.GetItemText(ObjectivesTreasureHuntAttachmentsLB.SelectedItem));
            CurrentTreasureHunt.isDirty = true;
        }
        private void darkButton78_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                List<string> addedtypes = form.addedtypes.ToList();
                if (CurrentTreasureHuntItems.Variants.Count == 0)
                {
                    listBox21.DisplayMember = "DisplayName";
                    listBox21.ValueMember = "Value";
                    listBox21.DataSource = CurrentTreasureHuntItems.Variants;
                }
                foreach (string l in addedtypes)
                {
                    treasurehunitemvarients newlootVarients = new treasurehunitemvarients()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = 0.5f,
                    };
                    CurrentTreasureHuntItems.Variants.Add(newlootVarients);
                    CurrentTreasureHunt.isDirty = true;
                }

            }
        }
        private void darkButton79_Click(object sender, EventArgs e)
        {
            CurrentTreasureHuntItems.Variants.Remove(LootVarients);
            listBox22.DataSource = null;
            listBox21.SelectedIndex = -1;
            if (CurrentTreasureHuntItems.Variants.Count > 0)
            {
                listBox21.SelectedIndex = 0;
            }
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!LootVarients.Attachments.Contains(l))
                    {
                        LootVarients.Attachments.Add(l);
                        CurrentTreasureHunt.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the Varients list.....");
                }
            }
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            LootVarients.Attachments.Remove(listBox22.GetItemText(listBox22.SelectedItem));
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntChanceTB_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentTreasureHuntItems == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHuntItems.Chance = (((decimal)ObjectivesTreasureHuntChanceTB.Value) / 1000);
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntChanceTB_Scroll(object sender, EventArgs e)
        {
            darkLabel147.Text = ((decimal)(ObjectivesTreasureHuntChanceTB.Value) / 10).ToString() + "%";
        }
        private void ObjectivesTreasureHuntQualityPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentTreasureHuntItems == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHuntItems.QuantityPercent = (int)ObjectivesTreasureHuntQualityPercentNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntMaxCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentTreasureHuntItems == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHuntItems.Max = (int)ObjectivesTreasureHuntMaxCountNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void VarientChanceTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (LootVarients == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            LootVarients.Chance = (float)(((decimal)VarientChanceTrackBar.Value) / 1000);
            CurrentTreasureHunt.isDirty = true;
        }
        private void VarientChanceTrackBar_Scroll(object sender, EventArgs e)
        {
            darkLabel159.Text = ((decimal)(VarientChanceTrackBar.Value) / 10).ToString() + "%";
        }
        #endregion objectives

        #region Persistant Player Data
        public ExpansionQuestPersistentQuestData currentExpansionQuestPersistentQuestData;
        public ExpansionQuestObjectiveData currentExpansionQuestObjectiveData;
        private void setupplayerdata()
        {
            useraction = false;

            treeViewMS2.Nodes.Clear();
            TreeNode root = new TreeNode("Quest Player Data")
            {
                Tag = "Parent"
            };
            foreach(QuestPlayerData QPD in QuestPlayerDataList.QuestPlayerDataList)
            {
                root.Nodes.Add(treenodeplayerquestdata(QPD));
            }
            treeViewMS2.Nodes.Add(root);


            useraction = true;
        }
        private TreeNode treenodeplayerquestdata(QuestPlayerData QPD)
        {
            TreeNode Playerdata = new TreeNode(Path.GetFileNameWithoutExtension(QPD.Filename))
            {
                Tag = QPD
            };
            for(int i = 0; i < QPD.ExpansionQuestPersistentQuestDataCount; i++)
            {
                TreeNode newquest = new TreeNode(QPD.QuestDatas[i].ToString());
                newquest.Tag = QPD.QuestDatas[i];
                Playerdata.Nodes.Add(newquest);
            }
            return Playerdata;
        }
        private void treeViewMS2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            useraction = false;
            QuestInfoGB.Visible = false;
            ObjectiveInfoGB.Visible = false;
            if (e.Node.Tag != null && e.Node.Tag is QuestPlayerData)
            {
            }
            else if (e.Node.Tag != null && e.Node.Tag is ExpansionQuestPersistentQuestData)
            {
                QuestInfoGB.Visible = true;
                currentExpansionQuestPersistentQuestData = e.Node.Tag as ExpansionQuestPersistentQuestData;
                currentExpansionQuestPersistentQuestData.AssignedQuestinfo = QuestsList.GetQuestfromid(currentExpansionQuestPersistentQuestData.QuestID);
                if (currentExpansionQuestPersistentQuestData.AssignedQuestinfo == null)
                {
                    QuestInfoGB.Visible = false;
                    MessageBox.Show("Quest Does not exist, Please remove from all Players data");
                }
                else
                {
                    for (int i = 0; i < currentExpansionQuestPersistentQuestData.QuestObjectivesCount; i++)
                    {
                        int ObjectiveIndex = currentExpansionQuestPersistentQuestData.QuestObjectives[i].ObjectiveIndex;
                        try
                        {
                            currentExpansionQuestPersistentQuestData.QuestObjectives[i].AssignedObjective = currentExpansionQuestPersistentQuestData.AssignedQuestinfo.Objectives[ObjectiveIndex];
                        }
                        catch
                        {
                            currentExpansionQuestPersistentQuestData.QuestObjectives[i].AssignedObjective = null;
                        }
                    }
                    numericUpDown1.Value = currentExpansionQuestPersistentQuestData.QuestID;
                    comboBox1.SelectedItem = (ExpansionQuestState)currentExpansionQuestPersistentQuestData.State;
                    numericUpDown13.Value = currentExpansionQuestPersistentQuestData.LastUpdateTime;
                    numericUpDown14.Value = currentExpansionQuestPersistentQuestData.CompletionCount;
                    listBox1.DisplayMember = "DisplayName";
                    listBox1.ValueMember = "Value";
                    listBox1.DataSource = currentExpansionQuestPersistentQuestData.QuestObjectives;
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is ExpansionQuestObjectiveData)
            {
            }
            useraction = true;
        }
        private void treeViewMS2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewMS2.SelectedNode = e.Node;
            if (e.Node.Tag != null && e.Node.Tag is QuestPlayerData)
            {
                if (e.Button == MouseButtons.Right)
                {
                    removePlayerSaveDataToolStripMenuItem.Visible = true;
                    removeQuestFromPlayerToolStripMenuItem.Visible = false;
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is ExpansionQuestPersistentQuestData)
            {
                if (e.Button == MouseButtons.Right)
                {
                    removePlayerSaveDataToolStripMenuItem.Visible = false;
                    removeQuestFromPlayerToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is ExpansionQuestObjectiveData)
            {
            }
        }
        private void removeQuestFromPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuestPlayerData currentplayer = treeViewMS2.SelectedNode.Parent.Tag as QuestPlayerData;
            if (currentplayer == null) return;
            currentplayer.QuestDatas.Remove(treeViewMS2.SelectedNode.Tag as ExpansionQuestPersistentQuestData);
            currentplayer.ExpansionQuestPersistentQuestDataCount -= 1;
            currentplayer.isDirty = true;
            treeViewMS2.SelectedNode.Remove();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count <= 0) return;
            currentExpansionQuestObjectiveData = listBox1.SelectedItem as ExpansionQuestObjectiveData;
            useraction = false;
            ObjectiveInfoGB.Visible = true;

            numericUpDown3.Value = currentExpansionQuestObjectiveData.ObjectiveIndex;
            comboBox2.SelectedItem = (QuExpansionQuestObjectiveTypeestType)currentExpansionQuestObjectiveData.ObjectiveType;
            checkBox2.Checked = currentExpansionQuestObjectiveData.IsCompleted;
            checkBox3.Checked = currentExpansionQuestObjectiveData.IsActive;
            numericUpDown4.Value = currentExpansionQuestObjectiveData.ObjectiveAmount;
            numericUpDown5.Value = currentExpansionQuestObjectiveData.ObjectiveCount;
            numericUpDown6.Value = (decimal)currentExpansionQuestObjectiveData.ObjectivePosition[0];
            numericUpDown8.Value = (decimal)currentExpansionQuestObjectiveData.ObjectivePosition[1];
            numericUpDown10.Value = (decimal)currentExpansionQuestObjectiveData.ObjectivePosition[02];
            checkBox4.Checked = currentExpansionQuestObjectiveData.ActionState;
            numericUpDown7.Value = currentExpansionQuestObjectiveData.TimeLimit;

            useraction = true;
        }
        private void removePlayerSaveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuestPlayerData currentplayer = treeViewMS2.SelectedNode.Parent.Tag as QuestPlayerData;
            if (currentplayer == null) return;
            QuestPlayerDataList.deletePlayerData(currentplayer);
            File.Delete(currentplayer.Filename);
        }
        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {

        }




























        #endregion Persistant Player Data


    }
}
