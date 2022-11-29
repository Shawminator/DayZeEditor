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

        private bool useraction;

        public string QuestsSettingsPath { get; set; }
        public string QuestObjectivesPath { get; private set; }
        public string QuestNPCPath { get; private set; }
        public string QuestsPath { get; private set; }
        public string QuestPlayerDataPath { get; set; }
        public string QuestPersistantServerDataPath { get; set; }

        public NPCEmotes NPCEmotes { get; private set; }
        public QuestSettings QuestSettings { get; private set; }
        public QuestObjectives QuestObjectives { get; set; }
        public ExpansioQuestList QuestsList { get; private set; }
        public QuestNPCLists QuestNPCs { get; private set; }
        public QuestPersistantDataPlayersList QuestPlayerDataList { get; private set; }
        public QuestPersistentServerData QuestPersistentServerData { get; private set; }


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

            bool needtosave = false;

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
                if (Helper.checkver(QuestSettings.m_Version, QuestSettings.CurrentVersion))
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
                if (Helper.checkver(QuestPersistentServerData.ConfigVersion, QuestPersistentServerData.m_currentConfigVersion))
                {
                    QuestPersistentServerData.isDirty = true;
                    needtosave = true;
                }
            }

            QuestObjectivesPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Objectives";
            QuestObjectives = new QuestObjectives(QuestObjectivesPath);
            setupobjectives();

            QuestNPCPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\NPCs";
            QuestNPCs = new QuestNPCLists(QuestNPCPath);
            setupNPCs();

            QuestsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Quests";
            QuestsList = new ExpansioQuestList(QuestsPath);
            setupquests();
                        
            SetupSharedLists();


            QuestPlayerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\PlayerData";
            QuestPlayerDataList = new QuestPersistantDataPlayersList(QuestPlayerDataPath);
            setupplayerdata();


            


            tabControl1.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
            }
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

            QuestNPCs.setupquestlists(QuestsList);

            comboBox1.DisplayMember = "DisplayName";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = Enum.GetValues(typeof(ExpansionQuestState));

            comboBox2.DisplayMember = "DisplayName";
            comboBox2.ValueMember = "Value";
            comboBox2.DataSource = Enum.GetValues(typeof(QuExpansionQuestObjectiveTypeestType));


            List<Quests> quests = new List<Quests>();
            quests.Add(new Quests() { Title = "NONE", ID = -1 });
            quests.AddRange(QuestsList.QuestList.ToArray());
            QuestPreQuestCB.DisplayMember = "DisplayName";
            QuestPreQuestCB.ValueMember = "Value";
            QuestPreQuestCB.DataSource = quests;

            List<Quests> quests2 = new List<Quests>(quests);
            QuestFollowupQuestCB.DisplayMember = "DisplayName";
            QuestFollowupQuestCB.ValueMember = "Value";
            QuestFollowupQuestCB.DataSource = quests2;

            List<ExpansionQuestNPCs> questnpcs = new List<ExpansionQuestNPCs>();
            questnpcs.Add(new ExpansionQuestNPCs() { NPCName = "NONE", ID = -1 });
            questnpcs.AddRange(QuestNPCs.NPCList);

            QuestQuestGiverIDCB.DisplayMember = "DisplayName";
            QuestQuestGiverIDCB.ValueMember = "Value";
            QuestQuestGiverIDCB.DataSource = questnpcs;

            List<ExpansionQuestNPCs> questnpcs2 = new List<ExpansionQuestNPCs>(questnpcs);
            QuestQuestTurnInIDCB.DisplayMember = "DisplayName";
            QuestQuestTurnInIDCB.ValueMember = "Value";
            QuestQuestTurnInIDCB.DataSource = questnpcs2;

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
                string filepath = QuestObjectivesPath + "\\" + obj.getfoldernames()[(int)obj.QuestType];
                switch (obj.QuestType)
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
                        TreasureHunt.TreasureHunt.ConvertListToDict();
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

            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            if (QuestNPCs.Markedfordelete != null)
            {
                foreach (ExpansionQuestNPCs del in QuestNPCs.Markedfordelete)
                {
                    del.backupandDelete(QuestNPCPath);
                }
            }
            if (QuestsList.Markedfordelete != null)
            {
                foreach (Quests del in QuestsList.Markedfordelete)
                {
                    del.backupandDelete(QuestsPath);
                }
            }
            if (QuestObjectives.Markedfordelete != null)
            {
                foreach (QuestObjectivesBase del in QuestObjectives.Markedfordelete)
                {
                    del.backupandDelete(QuestObjectivesPath);
                }
            }
            if (QuestPlayerDataList.Markedfordelete != null)
            {
                foreach (QuestPlayerData del in QuestPlayerDataList.Markedfordelete)
                {
                    del.backupandDelete(QuestPlayerDataPath);
                }
            }
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
            questsNPCsNPCEmoteIDCB.DisplayMember = "DisplayName";
            questsNPCsNPCEmoteIDCB.ValueMember = "Value";
            questsNPCsNPCEmoteIDCB.DataSource = NPCEmotes.Emotes;

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

            List<string> loadoutsobjectives = new List<string>(loadouts);
            //QuestObjectivesNPCLoadoutsCB.DisplayMember = "DisplayName";
            //QuestObjectivesNPCLoadoutsCB.ValueMember = "Value";
            //QuestObjectivesNPCLoadoutsCB.DataSource = loadoutsobjectives;



            QuestNPCListLB.DisplayMember = "DisplayName";
            QuestNPCListLB.ValueMember = "Value";
            QuestNPCListLB.DataSource = QuestNPCs.NPCList;




            useraction = true;
        }
        private void QuestNPCListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuestNPCADDQuestButton.Visible = false;
            QuestNPCAddQuestLB.Visible = false;
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
            QuestsNPCsQuestIDSLB.DisplayMember = "DisplayName";
            QuestsNPCsQuestIDSLB.ValueMember = "Value";
            QuestsNPCsQuestIDSLB.DataSource = currentQuestNPC.CurrentQuests;
            QuestsNPCsNameTB.Text = currentQuestNPC.NPCName;
            QuestsNPCsDefaultNPCTextTB.Text = currentQuestNPC.DefaultNPCText;
            QuestNPCWaypointsLB.DisplayMember = "DisplayName";
            QuestNPCWaypointsLB.ValueMember = "Value";
            QuestNPCWaypointsLB.DataSource = currentQuestNPC.Waypoints;
            questsNPCsNPCEmoteIDCB.SelectedValue = currentQuestNPC.NPCEmoteID;
            QuestNPCIsEmoteStaticCB.Checked = currentQuestNPC.NPCEmoteIsStatic == 1 ? true : false;
            QuestNPCsLoadoutsCB.SelectedIndex = QuestNPCsLoadoutsCB.FindStringExact(currentQuestNPC.NPCLoadoutFile);
            QuestNPCsIsStaticCB.Checked = currentQuestNPC.IsStatic == 1 ? true:false;
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
        private void darkButton6_Click(object sender, EventArgs e)
        {
            QuestNPCADDQuestButton.Visible = true;
            QuestNPCAddQuestLB.Visible = true;
            QuestNPCAddQuestLB.Items.AddRange(QuestsList.QuestList.ToArray());
        }
        private void QuestNPCADDQuestButton_Click(object sender, EventArgs e)
        {
            foreach(var item in QuestNPCAddQuestLB.SelectedItems)
            {
                Quests newquest = item as Quests;
                currentQuestNPC.AddNewQuest(newquest);
            }
            QuestsNPCsQuestIDSLB.Refresh();
            QuestNPCADDQuestButton.Visible = false;
            QuestNPCAddQuestLB.Visible = false;
            QuestNPCAddQuestLB.Items.Clear();
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            Quests currentQuests = QuestsNPCsQuestIDSLB.SelectedItem as Quests;
            currentQuestNPC.removequest(currentQuests);
            QuestsNPCsQuestIDSLB.Refresh();
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

            QuestTypeCB.DataSource = Enum.GetValues(typeof(QuExpansionQuestObjectiveTypeestType));

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
            QuestTypeCB.SelectedItem = (QuExpansionQuestObjectiveTypeestType)CurrentQuest.Type;
            QuestTitleTB.Text = CurrentQuest.Title;
            if (CurrentQuest.Descriptions.Count > 0)
            {
                QuestUsedescriptionCB.Checked = true;
                QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
                QuestDescription2TB.Text = CurrentQuest.Descriptions[1];
                QuestDescription3TB.Text = CurrentQuest.Descriptions[2];
            }
            else
            {
                QuestUsedescriptionCB.Checked = false;
                QuestDescription1TB.Text = "";
                QuestDescription2TB.Text = "";
                QuestDescription3TB.Text = "";
            }
            useraction = false;
            QuestObjectiveTextTB.Text = CurrentQuest.ObjectiveText;
            QuestPreQuestCB.SelectedItem = QuestPreQuestCB.Items.Cast<Quests>().FirstOrDefault(z => z.ID == CurrentQuest.PreQuest);
            QuestFollowupQuestCB.SelectedItem = QuestFollowupQuestCB.Items.Cast<Quests>().FirstOrDefault(z => z.ID == CurrentQuest.FollowUpQuest);
            //QuestQuestGiverIDCB.SelectedItem = QuestQuestGiverIDCB.Items.Cast<ExpansionQuestNPCs>().FirstOrDefault(z => z.ID == CurrentQuest.QuestGiverID);
            //QuestQuestTurnInIDCB.SelectedItem = QuestQuestTurnInIDCB.Items.Cast<ExpansionQuestNPCs>().FirstOrDefault(z => z.ID == CurrentQuest.QuestTurnInID);
            QuestIsAchivementCB.Checked = CurrentQuest.IsAchivement == 1 ? true : false;
            QuestRepeatableCB.Checked = CurrentQuest.Repeatable == 1 ? true : false;
            QuestIsDailyQuestCB.Checked = CurrentQuest.IsDailyQuest == 1 ? true : false;
            QuestIsWeeklyQuestCB.Checked = CurrentQuest.IsWeeklyQuest == 1 ? true : false;
            QuestCancelQuestOnPlayerDeathCB.Checked = CurrentQuest.CancelQuestOnPlayerDeath == 1 ? true : false;
            questAutocompleteCB.Checked = CurrentQuest.Autocomplete == 1 ? true : false;
            QuestIsGroupQuestCB.Checked = CurrentQuest.IsGroupQuest == 1 ? true : false;
            QuestIsBanditQuestCB.Checked = CurrentQuest.IsBanditQuest == 1 ? true : false;
            QuestIsHeroQuestCB.Checked = CurrentQuest.IsHeroQuest == 1 ? true : false;
            QuestObjectSetFileNameTB.Text = CurrentQuest.ObjectSetFileName;
            QuestQuestClassNameTB.Text = CurrentQuest.QuestClassName;

            for( int i = 0; i < CurrentQuest.Objectives.Count; i++)
            {
                QuestObjectivesBase checkobj = QuestObjectives.CheckObjectiveExists(CurrentQuest.Objectives[i]);
                if (checkobj == null)
                    MessageBox.Show("Type " + CurrentQuest.Objectives[i].ObjectiveType.ToString() + ",Objective " + CurrentQuest.Objectives[i].ID.ToString() + " doesn not exist in the list of objectives, please check");
                else
                {
                    if (CurrentQuest.Objectives[i].ObjectiveText != checkobj.ObjectiveText)
                    {
                        CurrentQuest.Objectives[i].ObjectiveText = checkobj.ObjectiveText;
                        CurrentQuest.isDirty = true;
                    }
                    if(CurrentQuest.Objectives[i].ConfigVersion != checkobj.ConfigVersion)
                    {
                        CurrentQuest.Objectives[i].ConfigVersion = checkobj.ConfigVersion;
                        CurrentQuest.isDirty = true;
                    }
                }
            }
            QuestNeedToSelectRewardCB.Checked = CurrentQuest.NeedToSelectReward == 1 ? true : false;
            QuestRewardsForGroupOwnerOnlyCB.Checked = CurrentQuest.RewardsForGroupOwnerOnly == 1 ? true : false;
            //QuestHumanityRewardCB.Checked = CurrentQuest.HumanityReward == 1 ? true : false;

            QuestObjectivesLB.DisplayMember = "DisplayName";
            QuestObjectivesLB.ValueMember = "Value";
            QuestObjectivesLB.DataSource = CurrentQuest.Objectives;
            useraction = false;
            QuestQuestItemsLB.DisplayMember = "DisplayName";
            QuestQuestItemsLB.ValueMember = "Value";
            QuestQuestItemsLB.DataSource = CurrentQuest.QuestItems;
            useraction = true;
            QuestRewardsLB.DisplayMember = "DisplayName";
            QuestRewardsLB.ValueMember = "Value";
            QuestRewardsLB.DataSource = CurrentQuest.Rewards;
            useraction = true;
        }
        private void QuestTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuExpansionQuestObjectiveTypeestType QuestType = (QuExpansionQuestObjectiveTypeestType)QuestTypeCB.SelectedItem;
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
            QuestsList.CreateNewQuest();
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
                QuestNPCs.RemoveQuest(CurrentQuest);
                if (QuestsListLB.Items.Count == 0)
                    QuestsListLB.SelectedIndex = -1;
                else
                    QuestsListLB.SelectedIndex = 0;
            }
        }
        private void QuestUsedescriptionCB_CheckedChanged(object sender, EventArgs e)
        {
            QuestDescriptionsP.Visible = CurrentQuest.isusingdescription = QuestUsedescriptionCB.Checked;
            if (CurrentQuest.Descriptions.Count == 0 && CurrentQuest.isusingdescription)
            {
                CurrentQuest.Descriptions.Add("Description on getting quest.");
                CurrentQuest.Descriptions.Add("Description while quest is active.");
                CurrentQuest.Descriptions.Add("Description when take in quest.");
                QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
                QuestDescription2TB.Text = CurrentQuest.Descriptions[1];
                QuestDescription3TB.Text = CurrentQuest.Descriptions[2];
            }
            else if (!CurrentQuest.isusingdescription && useraction == true)
            {
                CurrentQuest.Descriptions = new BindingList<string>();
                QuestDescription1TB.Text = "";
                QuestDescription2TB.Text = "";
                QuestDescription3TB.Text = "";
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
        private void QuestPreQuestCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Quests quest = QuestPreQuestCB.SelectedItem as Quests;
            CurrentQuest.PreQuest = quest.ID;
            CurrentQuest.isDirty = true;
        }
        private void QuestFollowupQuestCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            Quests quest = QuestFollowupQuestCB.SelectedItem as Quests;
            CurrentQuest.FollowUpQuest = quest.ID;
            CurrentQuest.isDirty = true;
        }
        private void QuestQuestGiverIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            ExpansionQuestNPCs npc = QuestQuestGiverIDCB.SelectedItem as ExpansionQuestNPCs;

            CurrentQuest.isDirty = true;
            QuestNPCs.addQuesttoNPC(npc, CurrentQuest);
        }
        private void QuestQuestTurnInIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            ExpansionQuestNPCs npc = QuestQuestTurnInIDCB.SelectedItem as ExpansionQuestNPCs;

            CurrentQuest.isDirty = true;
            QuestNPCs.addQuesttoNPC(npc, CurrentQuest);
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
        private void QuestIsBanditQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsBanditQuest = QuestIsBanditQuestCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsHeroQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsHeroQuest = QuestIsHeroQuestCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestObjectSetFileNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.ObjectSetFileName = QuestObjectSetFileNameTB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestQuestClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.QuestClassName = QuestQuestClassNameTB.Text;
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
                    CurrentQuest.Objectives.Add(obj);
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
                switch (QuestObjectives.Objectives[i].QuestType)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        TreeNode newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTarget>(QuestObjectivesPath + "\\Target\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTarget;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.TARGET;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTravel>(QuestObjectivesPath + "\\Travel\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTravel;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.TRAVEL;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCollection>(QuestObjectivesPath + "\\Collection\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCollection;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.COLLECT;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCrafting>(QuestObjectivesPath + "\\Crafting\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCrafting;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.CRAFTING;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCrafting.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesDelivery>(QuestObjectivesPath + "\\Delivery\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesDelivery;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.DELIVERY;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTreasureHunt>(QuestObjectivesPath + "\\TreasureHunt\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTreasureHunt;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.TREASUREHUNT;
                        QuestObjectivesTreasureHunt qoth = QuestObjectives.Objectives[i] as QuestObjectivesTreasureHunt;
                        qoth.TreasureHunt.ConvertDictToList();
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIPatrol>(QuestObjectivesPath + "\\AIPatrol\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIPatrol;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.AIPATROL;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAICamp>(QuestObjectivesPath + "\\AICamp\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAICamp;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.AICAMP;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIVIP>(QuestObjectivesPath + "\\AIVIP\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIVIP;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.AIVIP;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAction>(QuestObjectivesPath + "\\Action\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAction;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].QuestType = QuExpansionQuestObjectiveTypeestType.ACTION;
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
                QuestObjectivesObjectiveTextTB.Text = CurrentTreeNodeTag.ObjectiveText;
                QuestObjectivesTimeLimitNUD.Value = CurrentTreeNodeTag.TimeLimit;
                foreach (GroupBox gpBox in flowLayoutPanel3.Controls.OfType<GroupBox>())
                {
                    if (gpBox.Name == "QuestObjectivesBaseInfoGB") continue;
                    gpBox.Visible = false;
                }
                switch (CurrentTreeNodeTag.QuestType)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        setupObjectiveTarget(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        SetupObjectiveTravel(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        setupObjectiveCollection(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        setupobjectivecrafting(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        setupobjectiveDelivery(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        SetupobjectiveTreasueHunt(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        SetupObjectiveAIVIP(e);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
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
        /// <summary>
        /// Action Objectives
        /// </summary>
        /// <param name="e"></param>
        private void SetupObjectiveAction(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAction CurrentAction = e.Node.Tag as QuestObjectivesAction;
            useraction = false;

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

        /// <summary>
        /// AI Camp
        /// </summary>
        private void SetupObjectiveAICamp(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAICamp CurrentAICamp = e.Node.Tag as QuestObjectivesAICamp;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {

        }
        private void darkButton19_Click(object sender, EventArgs e)
        {

        }
        private void darkButton18_Click(object sender, EventArgs e)
        {

        }
        private void darkButton17_Click(object sender, EventArgs e)
        {

        }
        private void darkButton16_Click(object sender, EventArgs e)
        {

        }
        private void darkButton15_Click(object sender, EventArgs e)
        {

        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {

        }
        private void ObjectiovesAICampNPCSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampNPCModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampNPCFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampNPCLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampNPCAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampNPCAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {

        }
        private void darkButton22_Click(object sender, EventArgs e)
        {

        }
        private void darkButton21_Click(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampSpecialWeaponCB_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void darkButton31_Click(object sender, EventArgs e)
        {

        }
        private void darkButton29_Click(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {

        }
        private void ObjectivesAICampDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {

        }
        private void ObjectiovesAICampCanLootAICB_CheckedChanged(object sender, EventArgs e)
        {

        }




        private void setupObjectiveTarget(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = e.Node.Tag as QuestObjectivesTarget;

        }
        private void SetupObjectiveTravel(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTravel CurrentTravel = e.Node.Tag as QuestObjectivesTravel;
        }
        private void setupObjectiveCollection(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesCollection CurrentCollection = e.Node.Tag as QuestObjectivesCollection;

        }
        private void setupobjectiveDelivery(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesDelivery CurrentDelivery = e.Node.Tag as QuestObjectivesDelivery;
        }
        private void SetupobjectiveTreasueHunt(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = e.Node.Tag as QuestObjectivesTreasureHunt;
        }
        private void SetupobjectiveAIPatrol(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = e.Node.Tag as QuestObjectivesAIPatrol;
        }

        private void SetupObjectiveAIVIP(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAIVIP CurrentAIVIP = e.Node.Tag as QuestObjectivesAIVIP;
        }

        private void setupobjectivecrafting(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesCrafting CurrentAction = e.Node.Tag as QuestObjectivesCrafting;
        }
        private void QuestObjectivesObjectiveTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentTreeNodeTag.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
            CurrentTreeNodeTag.isDirty = true;
            foreach(Quests quest in QuestsList.QuestList)
            {
                foreach(QuestObjectivesBase obj in quest.Objectives)
                {
                    if(CurrentTreeNodeTag.ID == obj.ID && CurrentTreeNodeTag.ObjectiveType == obj.ObjectiveType)
                    {
                        obj.ObjectiveText = CurrentTreeNodeTag.ObjectiveText;
                        quest.isDirty = true;
                    }
                }
            }
        }
        private void QuestObjectivesTimeLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentTreeNodeTag.TimeLimit = (int)QuestObjectivesTimeLimitNUD.Value;
            CurrentTreeNodeTag.isDirty = true;
            foreach (Quests quest in QuestsList.QuestList)
            {
                foreach (QuestObjectivesBase obj in quest.Objectives)
                {
                    if (CurrentTreeNodeTag.ID == obj.ID && CurrentTreeNodeTag.ObjectiveType == obj.ObjectiveType)
                    {
                        obj.TimeLimit = CurrentTreeNodeTag.TimeLimit;
                        quest.isDirty = true;
                    }
                }
            }
        }


        #endregion objectives


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


    }
}
