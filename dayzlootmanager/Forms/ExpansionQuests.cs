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
using System.Windows.Forms;
using TreeViewMS;

namespace DayZeEditor
{

    public partial class ExpansionQuests : DarkForm
    {
        public Project currentproject { get; internal set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public BindingList<string> Factions { get; private set; }


        private bool _useraction;

        private bool useraction
        {
            get { return _useraction; }
            set { _useraction = value; }
        }

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
            lb.HorizontalExtent = CurrentItemWidth + 5;
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
            tabControl1.ItemSize = new Size(0, 1);
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
            Setupallquests();
        }
        private void reloadAllQuestsDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("\nReloading all Quest Data......");
            Setupallquests();
        }
        private void Setupallquests()
        {
            Factions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt").ToList());
            SetupFactionsDropDownBoxes();

            QuestActions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Vanilla_Quest_Actions.txt").ToList());
            ObjectivesActionsCB.DataSource = new BindingList<string>(QuestActions);



            bool needtosave = false;

            LoadoutList = new BindingList<AILoadouts>();
            AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("\nserializing AI Loadouts.");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("\tserializing " + Path.GetFileName(file.FullName));
                    AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
                    AILoadouts.Filename = file.FullName;
                    AILoadouts.Setname();
                    AILoadouts.isDirty = false;
                    LoadoutList.Add(AILoadouts);
                }
                catch { }
            }
            expansionQuestAISpawnControlAICamp.LoadoutList = LoadoutList;
            expansionQuestAISpawnControlAICamp.setuplists();

            ObjectivesAIVIPNPCLoadoutFileCB.DisplayMember = "DisplayName";
            ObjectivesAIVIPNPCLoadoutFileCB.ValueMember = "Value";
            ObjectivesAIVIPNPCLoadoutFileCB.DataSource = LoadoutList;


            expansionQuestAISpawnControlAIPatrol.LoadoutList = LoadoutList;
            expansionQuestAISpawnControlAIPatrol.setuplists();

            QuestsSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings\\QuestSettings.json";
            if (!File.Exists(QuestsSettingsPath))
            {
                Console.WriteLine(Path.GetFileName(QuestsSettingsPath) + "Not found, Creating new...");
                QuestSettings = new QuestSettings();
                needtosave = true;
            }
            else
            {
                Console.WriteLine("\nserializing " + Path.GetFileName(QuestsSettingsPath));
                QuestSettings = JsonSerializer.Deserialize<QuestSettings>(File.ReadAllText(QuestsSettingsPath));
                QuestSettings.isDirty = false;
                if (QuestSettings.checkver())
                {
                    Console.WriteLine("Updating " + Path.GetFileName(QuestsSettingsPath) + " version " + QuestSettings.CurrentVersion.ToString());
                    QuestSettings.isDirty = true;
                    needtosave = true;
                }
            }
            QuestSettings.Filename = QuestsSettingsPath;
            setupQuestsettings();

            QuestPersistantServerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\PersistentServerData.json";
            if (!File.Exists(QuestPersistantServerDataPath))
            {
                Console.WriteLine(Path.GetFileName(QuestPersistantServerDataPath) + "Not found, Creating new...");
                QuestPersistentServerData = new QuestPersistentServerData();
                needtosave = true;
            }
            else
            {
                Console.WriteLine("\nserializing " + Path.GetFileName(QuestPersistantServerDataPath));
                QuestPersistentServerData = JsonSerializer.Deserialize<QuestPersistentServerData>(File.ReadAllText(QuestPersistantServerDataPath));
                QuestPersistentServerData.isDirty = false;
                if (QuestPersistentServerData.checkver())
                {
                    Console.WriteLine("Updating " + Path.GetFileName(QuestPersistantServerDataPath) + " version " + QuestSettings.CurrentVersion.ToString());
                    QuestPersistentServerData.isDirty = true;
                    needtosave = true;
                }
            }

            QuestObjectivesPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Objectives";
            QuestObjectives = new QuestObjectives(QuestObjectivesPath);
            setupobjectives();
            QuestObjectives.Checkver();
            if (QuestObjectives.Objectives.Any(x => x.isDirty == true))
                needtosave = true;
            //setupobjectives();

            QuestNPCPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\NPCs";
            QuestNPCs = new QuestNPCLists(QuestNPCPath);
            if (QuestNPCs.NPCList.Any(x => x.isDirty == true))
                needtosave = true;
            setupNPCs();

            QuestsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\Quests";
            QuestsList = new ExpansioQuestList(QuestsPath);
            QuestsList.setobjectiveenums();
            needtosave = QuestsList.GetNPCLists(QuestNPCs);
            QuestsList.GetPreQuests();
            if (QuestsList.QuestList.Any(x => x.isDirty == true))
                needtosave = true;
            setupquests();

            SetupSharedLists();

            Console.WriteLine("\nChecking Objectives against Quests.");
            foreach (Quests quest in QuestsList.QuestList)
            {
                for (int i = 0; i < quest.Objectives.Count; i++)
                {
                    QuestObjectivesBase checkobj = QuestObjectives.CheckObjectiveExists(quest.Objectives[i]);
                    if (checkobj == null)
                    {
                        MessageBox.Show("Quest " + quest.ID.ToString() + " objective type " + quest.Objectives[i]._ObjectiveTypeEnum.ToString() + ",Objective ID " + quest.Objectives[i].ID.ToString() + " does not exist in the list of objectives,\nplease check\nmanually remove the objective from the quest and save.\n");
                        Console.WriteLine("Quest " + quest.ID.ToString() + " objective type " + quest.Objectives[i]._ObjectiveTypeEnum.ToString() + ",Objective ID " + quest.Objectives[i].ID.ToString() + " does not exist in the list of objectives,\nplease check\nmanually remove the objective from the quest and save.\n");
                    }
                    else
                    {
                        if (quest.Objectives[i].ConfigVersion != checkobj.ConfigVersion)
                        {
                            Console.WriteLine("\tUpdating objective config in quest to match version " + QuestObjectivesBase.GetconfigVersion);
                            quest.Objectives[i].ConfigVersion = checkobj.ConfigVersion;
                            quest.isDirty = true;
                            needtosave = true;
                        }

                    }
                }
            }

            QuestPlayerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Quests\\PlayerData";
            if (!Directory.Exists(QuestPlayerDataPath))
            {
                Directory.CreateDirectory(QuestPlayerDataPath);
            }
            QuestPlayerDataList = new QuestPersistantDataPlayersList(QuestPlayerDataPath);
            setupplayerdata();



            if (needtosave)
            {
                savefiles(true);
            }
        }

        private void SetupFactionsDropDownBoxes()
        {
            useraction = false;
            expansionQuestAISpawnControlAICamp.Factions = Factions;
            expansionQuestAISpawnControlAIPatrol.Factions = Factions;
            QuestNPCFactionLB.DataSource = new BindingList<string>(Factions);

            List<string> questrequiredfaction = new List<string>();
            questrequiredfaction.Add("");
            foreach (string rf in Factions)
            {
                questrequiredfaction.Add(rf);
            }
            QuestRequiredFactionCB.DataSource = new BindingList<string>(questrequiredfaction);
            QuestFactionRewardCB.DataSource = new BindingList<string>(questrequiredfaction);
            QuestsFactionReputationRequirementsCB.DataSource = new BindingList<string>(questrequiredfaction);
            QuestFactionReputationRewardsCB.DataSource = new BindingList<string>(questrequiredfaction);

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
            foreach (QuestPlayerData qpd in QuestPlayerDataList.QuestPlayerDataList)
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
                Console.WriteLine("\tserializing Full Objective" + filename);
                return JsonSerializer.Deserialize<T>(File.ReadAllText(filename));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("there is an error in the following file\n" + filename + Environment.NewLine + ex.InnerException.Message);
                    MessageBox.Show("there is an error in the following file\n" + filename + Environment.NewLine + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
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
            foreach (ExpansionQuestNPCs npcs in QuestNPCs.NPCList)
            {
                if (!npcs.isDirty) continue;
                npcs.isDirty = false;
                npcs.SetVec3Lists();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(npcs, options);
                if (currentproject.Createbackups && File.Exists(QuestNPCPath + "\\" + npcs.Filename + ".json"))
                {
                    Directory.CreateDirectory(QuestNPCPath + "\\Backup\\" + SaveTime);
                    File.Copy(QuestNPCPath + "\\" + npcs.Filename + ".json", QuestNPCPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(npcs.Filename) + ".bak", true);
                }
                if (npcs.Filename != npcs.OriginalFilename)
                {
                    if (File.Exists(QuestNPCPath + "\\" + npcs.OriginalFilename + ".json"))
                        File.Delete(QuestNPCPath + "\\" + npcs.OriginalFilename + ".json");
                    npcs.OriginalFilename = npcs.Filename;
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
                Quest.SetLists();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(Quest, options);
                if (currentproject.Createbackups && File.Exists(QuestsPath + "\\" + Quest.Filename + ".json"))
                {
                    Directory.CreateDirectory(QuestsPath + "\\Backup\\" + SaveTime);
                    File.Copy(QuestsPath + "\\" + Quest.Filename + ".json", QuestsPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Quest.Filename) + ".bak", true);
                }
                if (Quest.Filename != Quest.OriginalFilename)
                {
                    if (File.Exists(QuestsPath + "\\" + Quest.OriginalFilename + ".json"))
                        File.Delete(QuestsPath + "\\" + Quest.OriginalFilename + ".json");
                    Quest.OriginalFilename = Quest.Filename;
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
                        TreasureHunt.SetVec3List();
                        jsonString = JsonSerializer.Serialize(TreasureHunt, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        QuestObjectivesAIPatrol AIPatrol = obj as QuestObjectivesAIPatrol;
                        AIPatrol.SetVec3List();
                        jsonString = JsonSerializer.Serialize(AIPatrol, options);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        QuestObjectivesAICamp AICamp = obj as QuestObjectivesAICamp;
                        AICamp.SetVec3List();
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
                if (obj.Filename != obj.OriginalFilename)
                {
                    if (File.Exists(filepath + "\\" + obj.OriginalFilename + ".json"))
                        File.Delete(filepath + "\\" + obj.OriginalFilename + ".json");
                    obj.OriginalFilename = obj.Filename;
                }
                File.WriteAllText(filepath + "\\" + obj.Filename + ".json", jsonString);
                midifiedfiles.Add(Path.GetFileName(obj.Filename));
            }

            foreach (QuestPlayerData qpd in QuestPlayerDataList.QuestPlayerDataList)
            {
                if (qpd.isDirty)
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

            string message = "\nThe Following Files were saved....\n";
            if (updated)
            {
                message = "\nThe following files were either Created or Updated...\n";
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
                message += "\nThe following Quest NPC files were Removed\n";
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
                message += "\nThe following Quest files were Removed\n";
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
                message += "\nThe following Quest Objective files were Removed\n";
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
                message += "\nThe following Quest Player Data files were Removed\n";
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

            string[] boolignorenames = new string[] { "m_Version", "DailyResetMinute", "DailyResetHour", "WeeklyResetHour", "WeeklyResetMinute", "GroupQuestMode", "MaxActiveQuests" };
            List<string> questbools = Helper.GetPropertiesNameOfClass<int>(QuestSettings, boolignorenames);
            QuestBoolsLB.DisplayMember = "DisplayName";
            QuestBoolsLB.ValueMember = "Value";
            QuestBoolsLB.DataSource = questbools;

            string[] stringignorenames = new string[] { "Filename" };
            List<string> queststrings = Helper.GetPropertiesNameOfClass<string>(QuestSettings, stringignorenames);
            QuestStringsLB.DisplayMember = "DisplayName";
            QuestStringsLB.ValueMember = "Value";
            QuestStringsLB.DataSource = queststrings;

            string[] intIgnoreNames = new string[] { "m_Version", "EnableQuests", "EnableQuestLogTab", "CreateQuestNPCMarkers", "UseUTCTime", "UseQuestNPCIndicators" };
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
                case "UseQuestNPCIndicators":
                    InfoLabel.Text = "";
                    break;
            }

            useraction = true;
        }
        private void QuestStringsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestStringsLB.SelectedItems.Count < 1) return;
            useraction = false;
            QuestStringTB.Text = Helper.GetPropValue(QuestSettings, QuestStringsLB.GetItemText(QuestStringsLB.SelectedItem)).ToString();
            groupBox8.Text = "String Info";
            InfoLabel.Text = "";
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
                case "DailyResetMinute":
                    InfoLabel.Text = "Integer.\n\nMinute at when the quest reset will happend for all daily quests.";
                    break;
                case "DailyResetHour":
                    InfoLabel.Text = "Integer.\n\nHour at when the quest reset will happend for all daily quests.";
                    break;
                case "WeeklyResetMinute":
                    InfoLabel.Text = "Integer.\n\nMinute at when the quest reset will happend for all weekly quests.";
                    break;
                case "WeeklyResetHour":
                    InfoLabel.Text = "Integer.\n\nHour at when the quest reset will happend for all weekly quests.";
                    break;
                case "MaxActiveQuests":
                    InfoLabel.Text = "";
                    break;
            }
            useraction = true;
        }
        private void QuestBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetFakeBoolValue(QuestSettings, QuestBoolsLB.GetItemText(QuestBoolsLB.SelectedItem), QuestBoolsCB.Checked);
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
            QuestNPCActiveCB.Checked = currentQuestNPC.Active == 1 ? true : false;
            QuestNPCConfigVersionNUD.Value = currentQuestNPC.ConfigVersion;
            QuestNPCIDNUD.Value = currentQuestNPC.ID;
            QuestNPCsClassNameCB.SelectedIndex = QuestNPCsClassNameCB.FindStringExact(currentQuestNPC.ClassName);
            QuestNPCsPOSXNUD.Value = (decimal)currentQuestNPC._Position.X;
            QuestNPCsPOSYNUD.Value = (decimal)currentQuestNPC._Position.Y;
            QuestNPCsPOSZNUD.Value = (decimal)currentQuestNPC._Position.Z;
            QuestNPCsOXNUD.Value = (decimal)currentQuestNPC._Orientation.X;
            QuestNPCsOYNUD.Value = (decimal)currentQuestNPC._Orientation.Y;
            QuestNPCsOZNUD.Value = (decimal)currentQuestNPC._Orientation.Z;
            QuestsNPCsNameTB.Text = currentQuestNPC.NPCName;
            QuestsNPCsDefaultNPCTextTB.Text = currentQuestNPC.DefaultNPCText;
            questsNPCsNPCEmoteIDCB.SelectedValue = currentQuestNPC.NPCEmoteID;
            NPCInteractionEmoteIDCB.SelectedValue = currentQuestNPC.NPCInteractionEmoteID;
            NPCQuestCancelEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestCancelEmoteID;
            NPCQuestStartEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestStartEmoteID;
            NPCQuestCompleteEmoteIDCB.SelectedValue = currentQuestNPC.NPCQuestCompleteEmoteID;

            QuestNPCIsEmoteStaticCB.Checked = currentQuestNPC.NPCEmoteIsStatic == 1 ? true : false;
            QuestNPCsLoadoutsCB.SelectedIndex = QuestNPCsLoadoutsCB.FindStringExact(currentQuestNPC.NPCLoadoutFile);
            QuestNPCFactionLB.SelectedIndex = QuestNPCFactionLB.FindStringExact(currentQuestNPC.NPCFaction);
            NPCQuestNPCTypeCB.SelectedIndex = currentQuestNPC.NPCType;

            QuestNPCWaypointsLB.DisplayMember = "DisplayName";
            QuestNPCWaypointsLB.ValueMember = "Value";
            QuestNPCWaypointsLB.DataSource = currentQuestNPC._Waypoints;


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
                ExpansionQuestNPCs removingnpc = currentQuestNPC;
                QuestNPCs.RemoveNPC(removingnpc);
                QuestsList.RemoveNPCFromQuests(removingnpc);
                SetupSharedLists();
                if (QuestNPCListLB.Items.Count == 0)
                    QuestNPCListLB.SelectedIndex = -1;
                else
                    QuestNPCListLB.SelectedIndex = 0;
            }
        }
        private void QuestNPCFilenameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Filename = QuestNPCFilenameTB.Text;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.Active = QuestNPCActiveCB.Checked == true ? 1 : 0;
            currentQuestNPC.isDirty = true;
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
            Vec3 waypoint = QuestNPCWaypointsLB.SelectedItem as Vec3;
            useraction = false;
            QuestNPCWaypointXNUD.Value = (decimal)waypoint.X;
            QuestNPCWaypointYNUD.Value = (decimal)waypoint.Y;
            QuestNPCWaypointZNUD.Value = (decimal)waypoint.Z;
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
        private void QuestNPCIsEmoteStaticCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC.NPCEmoteIsStatic = QuestNPCIsEmoteStaticCB.Checked == true ? 1 : 0;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Position.X = (float)QuestNPCsPOSXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Position.Y = (float)QuestNPCsPOSYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Position.Z = (float)QuestNPCsPOSZNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Orientation.X = (float)QuestNPCsOXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Orientation.Y = (float)QuestNPCsOYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void QuestNPCsOZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentQuestNPC._Orientation.Z = (float)QuestNPCsOZNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            currentQuestNPC._Waypoints.Add(new Vec3(0, 0, 0));
            currentQuestNPC.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            currentQuestNPC.Waypoints.Remove(QuestNPCWaypointsLB.SelectedItem as decimal[]);
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            Vec3 waypoint = QuestNPCWaypointsLB.SelectedItem as Vec3;
            waypoint.X = (float)QuestNPCWaypointXNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            Vec3 waypoint = QuestNPCWaypointsLB.SelectedItem as Vec3;
            waypoint.Y = (float)QuestNPCWaypointYNUD.Value;
            currentQuestNPC.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestNPCWaypointsLB.SelectedItems.Count < 1) return;
            Vec3 waypoint = QuestNPCWaypointsLB.SelectedItem as Vec3;
            waypoint.Z = (float)QuestNPCWaypointZNUD.Value;
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
                    fileContent = File.ReadAllLines(filePath);
                    currentQuestNPC._Waypoints = new BindingList<Vec3>();
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');

                        currentQuestNPC._Waypoints.Add(new Vec3(XYZ));
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
            foreach (Vec3 vec3 in currentQuestNPC._Waypoints)
            {
                SB.AppendLine(currentQuestNPC.NPCName + "|" + vec3.GetString() + "|0.0 0.0 0.0");
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
                        currentQuestNPC._Waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        int i = 0;
                        if (i == 0)
                        {
                            currentQuestNPC._Position = new Vec3(eo.Position);
                            QuestNPCsPOSXNUD.Value = (decimal)currentQuestNPC.Position[0];
                            QuestNPCsPOSYNUD.Value = (decimal)currentQuestNPC.Position[1];
                            QuestNPCsPOSZNUD.Value = (decimal)currentQuestNPC.Position[2];
                        }
                        else
                        {
                            currentQuestNPC._Waypoints.Add(new Vec3(eo.Position));
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
            int m_Id = 0;
            foreach (Vec3 array in currentQuestNPC._Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = currentQuestNPC.NPCName,
                    DisplayName = currentQuestNPC.NPCName,
                    Position = array.getfloatarray(),
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(eo);
                m_Id++;
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
        private void NPCQuestNPCTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> npclist = new List<string>();
            switch (NPCQuestNPCTypeCB.SelectedIndex)
            {
                case 0:
                    npclist = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\QuestDefaultNPCs.txt").ToList();
                    QuestNPCsClassNameCB.DisplayMember = "Name";
                    QuestNPCsClassNameCB.ValueMember = "Value";
                    QuestNPCsClassNameCB.DataSource = npclist;
                    panel35.Visible = false;
                    panel38.Visible = false;
                    panel32.Visible = true;
                    panel5.Visible = false;
                    panel18.Visible = false;
                    panel67.Visible = false;
                    panel19.Visible = false;
                    panel68.Visible = false;
                    groupBox6.Visible = false;
                    break;
                case 1:
                    npclist = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\QuestStaticObject.txt").ToList();
                    QuestNPCsClassNameCB.DisplayMember = "Name";
                    QuestNPCsClassNameCB.ValueMember = "Value";
                    QuestNPCsClassNameCB.DataSource = npclist;
                    panel35.Visible = false;
                    panel38.Visible = false;
                    panel32.Visible = false;
                    panel5.Visible = false;
                    panel18.Visible = false;
                    panel67.Visible = false;
                    panel19.Visible = false;
                    panel68.Visible = false;
                    groupBox6.Visible = false;
                    break;
                case 2:
                    npclist = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\QuestDefaultAINPCs.txt").ToList();
                    QuestNPCsClassNameCB.DisplayMember = "Name";
                    QuestNPCsClassNameCB.ValueMember = "Value";
                    QuestNPCsClassNameCB.DataSource = npclist;
                    panel35.Visible = true;
                    panel38.Visible = true;
                    panel32.Visible = true;
                    panel5.Visible = true;
                    panel18.Visible = true;
                    panel67.Visible = true;
                    panel19.Visible = true;
                    panel68.Visible = true;
                    groupBox6.Visible = true;
                    break;
            }
            if (!npclist.Contains(currentQuestNPC.ClassName))
            {
                currentQuestNPC.ClassName = npclist[0];
                currentQuestNPC.isDirty = true;
                MessageBox.Show("NPC CLassname set to first name in list as it was not the correct type.\nPlease save");
            }
            if (!useraction) return;
            currentQuestNPC.NPCType = NPCQuestNPCTypeCB.SelectedIndex;
            currentQuestNPC.isDirty = true;
        }
        #endregion npc
        #region quests
        public Quests CurrentQuest { get; private set; }
        public BindingList<string> QuestActions { get; private set; }

        public void setupquests()
        {
            useraction = false;
            SetupFollowupCombobox();

            QuestTypeCB.DataSource = Enum.GetValues(typeof(ExpansionQuestsQuestType));

            QuestsListLB.DisplayMember = "DisplayName";
            QuestsListLB.ValueMember = "Value";
            QuestsListLB.DataSource = QuestsList.QuestList;

            useraction = true;
        }

        private void SetupFollowupCombobox()
        {
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
        }

        private void QuestsListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestsListLB.SelectedItems.Count < 1) return;
            CurrentQuest = QuestsListLB.SelectedItem as Quests;
            useraction = false;
            QuestRewardBehavorCB.DataSource = Enum.GetValues(typeof(ExpansionQuestRewardBehavior));
            QuestFileNameTB.Text = CurrentQuest.Filename;
            QuestConfigVersionNUD.Value = CurrentQuest.ConfigVersion;
            QuestIDNUD.Value = CurrentQuest.ID;
            QuestTypeCB.SelectedItem = (ExpansionQuestsQuestType)CurrentQuest.Type;
            QuestTitleTB.Text = CurrentQuest.Title;
            if (CurrentQuest.Descriptions.Count == 0)
            {
                CurrentQuest.Descriptions = new BindingList<string>(new string[] { "", "", "" });
            }
            if (CurrentQuest.Descriptions.Count != 3)
            {
                switch (CurrentQuest.Descriptions.Count)
                {
                    case 0:
                        QuestDescription1TB.Text = "";
                        QuestDescription2TB.Text = "";
                        QuestDescription3TB.Text = "";
                        break;
                    case 1:
                        QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
                        QuestDescription2TB.Text = "";
                        QuestDescription3TB.Text = "";
                        break;
                    case 2:
                        QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
                        QuestDescription2TB.Text = CurrentQuest.Descriptions[1];
                        QuestDescription3TB.Text = "";
                        break;
                }
                CurrentQuest.isDirty = true;
                MessageBox.Show("Incorrect number of lines for description. Please save to fix the file.");
                Console.WriteLine("Quest " + CurrentQuest.ID.ToString() + "has incorrect number of lines for description. Please save to fix the file.\n");
            }
            else
            {
                QuestDescription1TB.Text = CurrentQuest.Descriptions[0];
                QuestDescription2TB.Text = CurrentQuest.Descriptions[1];
                QuestDescription3TB.Text = CurrentQuest.Descriptions[2];
            }
            useraction = false;
            QuestObjectiveTextTB.Text = CurrentQuest.ObjectiveText;

            QuestFollowupQuestCB.SelectedItem = QuestFollowupQuestCB.Items.Cast<Quests>().FirstOrDefault(z => z.ID == CurrentQuest.FollowUpQuest);
            QuestFactionRewardCB.SelectedIndex = QuestFactionRewardCB.FindStringExact(CurrentQuest.FactionReward);
            QuestRequiredFactionCB.SelectedIndex = QuestRequiredFactionCB.FindStringExact(CurrentQuest.RequiredFaction);

            QuestActiveCB.Checked = CurrentQuest.Active == 1 ? true : false;
            QuestSuppressQuestLogOnCompetionCB.Checked = CurrentQuest.SuppressQuestLogOnCompetion == 1 ? true : false;

            QuestIsAchievementCB.Checked = CurrentQuest.IsAchievement == 1 ? true : false;
            QuestRepeatableCB.Checked = CurrentQuest.Repeatable == 1 ? true : false;
            QuestIsDailyQuestCB.Checked = CurrentQuest.IsDailyQuest == 1 ? true : false;
            QuestIsWeeklyQuestCB.Checked = CurrentQuest.IsWeeklyQuest == 1 ? true : false;
            QuestCancelQuestOnPlayerDeathCB.Checked = CurrentQuest.CancelQuestOnPlayerDeath == 1 ? true : false;
            questAutocompleteCB.Checked = CurrentQuest.Autocomplete == 1 ? true : false;
            QuestIsGroupQuestCB.Checked = CurrentQuest.IsGroupQuest == 1 ? true : false;
            QuestObjectSetFileNameTB.Text = CurrentQuest.ObjectSetFileName;
            QuestPlayerNeedQuestItemsCB.Checked = CurrentQuest.PlayerNeedQuestItems == 1 ? true : false;
            QuestDeleteQuestItemsCB.Checked = CurrentQuest.DeleteQuestItems == 1 ? true : false;


            QuestNeedToSelectRewardCB.Checked = CurrentQuest.NeedToSelectReward == 1 ? true : false;
            QuestRewardsForGroupOwnerOnlyCB.Checked = CurrentQuest.RewardsForGroupOwnerOnly == 1 ? true : false;
            QuestReputationRewardNUD.Value = CurrentQuest.ReputationReward;
            QuestReputationRequirmentNUD.Value = CurrentQuest.ReputationRequirement;
            QuestRandomRewardCB.Checked = CurrentQuest.RandomReward == 1 ? true : false;
            QuestRandomRewardAmountNUD.Value = CurrentQuest.RandomRewardAmount;
            QuestRewardBehavorCB.SelectedItem = (ExpansionQuestRewardBehavior)CurrentQuest.RewardBehavior;
            QuestSequentialObjectivesCB.Checked = CurrentQuest.SequentialObjectives == 1 ? true : false;

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

            QuestsFactionReputationRequirementsLB.DisplayMember = "Value";
            QuestsFactionReputationRequirementsLB.ValueMember = "Key";
            QuestsFactionReputationRequirementsLB.DataSource = CurrentQuest.FactionReputationRequirementsList;

            QuestFactionReputationRewardsLB.DisplayMember = "Value";
            QuestFactionReputationRewardsLB.ValueMember = "Key";
            QuestFactionReputationRewardsLB.DataSource = CurrentQuest.FactionReputationRewardsList;

            QuestColour.Invalidate();

            useraction = true;
        }
        private void QuestFileNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.Filename = QuestFileNameTB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.Active = QuestActiveCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestSuppressQuestLogOnCompetionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.SuppressQuestLogOnCompetion = QuestSuppressQuestLogOnCompetionCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
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
            QuestRewardsHealthPercentNUD.Value = qr.HealthPercent;
            QuestRewardsDamagePercentNUD.Value = qr.DamagePercent;
            QuestRewardsQuestIDNUD.Value = qr.QuestID;

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
        private void QuestRewardsHealthPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            qr.HealthPercent = (int)QuestRewardsHealthPercentNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void QuestRewardsDamagePercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            qr.DamagePercent = (int)QuestRewardsDamagePercentNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void QuestRewardsQuestIDNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            qr.QuestID = (int)QuestRewardsQuestIDNUD.Value;
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
            SetupFollowupCombobox();
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
            QuestsListLB.Invalidate();
            CurrentQuest.isDirty = true;

        }
        private void QuestDescription1TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentQuest.Descriptions.Count == 3)
                CurrentQuest.Descriptions[0] = QuestDescription1TB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestDescription2TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentQuest.Descriptions.Count == 3)
                CurrentQuest.Descriptions[1] = QuestDescription2TB.Text;
            CurrentQuest.isDirty = true;
        }
        private void QuestDescription3TB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentQuest.Descriptions.Count == 3)
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
            if (!useraction) return;

            Quests quest = QuestFollowupQuestCB.SelectedItem as Quests;
            CurrentQuest.FollowUpQuest = quest.ID;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsAchievementCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsAchievement = QuestIsAchievementCB.Checked == true ? 1 : 0;
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
        private void QuestRewardBehavorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            ExpansionQuestRewardBehavior cacl = (ExpansionQuestRewardBehavior)QuestRewardBehavorCB.SelectedItem;
            CurrentQuest.RewardBehavior = (int)cacl;
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
                        Attachments = new BindingList<string>(),
                        HealthPercent = 0,
                        DamagePercent = 0,
                        QuestID = -1

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
                    if (!CurrentQuest.QuestGivers.Any(x => x.ID == obj.ID))
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
        private void QuestRequiredFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.RequiredFaction = QuestRequiredFactionCB.GetItemText(QuestRequiredFactionCB.SelectedItem);
            CurrentQuest.isDirty = true;
        }
        private void QuestFactionRewardCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.FactionReward = QuestFactionRewardCB.GetItemText(QuestFactionRewardCB.SelectedItem);
            CurrentQuest.isDirty = true;
        }
        private void QuestPlayerNeedQuestItemsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.PlayerNeedQuestItems = QuestPlayerNeedQuestItemsCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestDeleteQuestItemsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.DeleteQuestItems = QuestDeleteQuestItemsCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestRandomRewardCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.RandomReward = QuestRandomRewardCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestRandomRewardAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.RandomRewardAmount = (int)QuestRandomRewardAmountNUD.Value;
            CurrentQuest.isDirty = true;
        }
        private void QuestSequentialObjectivesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.SequentialObjectives = QuestSequentialObjectivesCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestsFactionReputationRequirementsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestsFactionReputationRequirementsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestsFactionReputationRequirementsLB.SelectedItem as FactionQuestReps;
            useraction = false;
            QuestsFactionReputationRequirementsNUD.Value = fqr.rep;
            useraction = true;
        }
        private void QuestFactionReputationRewardsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestFactionReputationRewardsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestFactionReputationRewardsLB.SelectedItem as FactionQuestReps;
            useraction = false;
            QuestFactionReputationRewardsNUD.Value = fqr.rep;
            useraction = true;
        }
        private void QuestsFactionReputationRequirementsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (QuestsFactionReputationRequirementsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestsFactionReputationRequirementsLB.SelectedItem as FactionQuestReps;
            useraction = false;
            fqr.rep = (int)QuestsFactionReputationRequirementsNUD.Value;
            useraction = true;
        }
        private void QuestFactionReputationRewardsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (QuestFactionReputationRewardsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestFactionReputationRewardsLB.SelectedItem as FactionQuestReps;
            useraction = false;
            fqr.rep = (int)QuestFactionReputationRewardsNUD.Value;
            useraction = true;
        }
        private void darkButton82_Click(object sender, EventArgs e)
        {
            FactionQuestReps newrep = new FactionQuestReps()
            {
                faction = QuestsFactionReputationRequirementsCB.GetItemText(QuestsFactionReputationRequirementsCB.SelectedItem),
                rep = 100
            };
            if (!CurrentQuest.FactionReputationRequirementsList.Any(x => x.faction == newrep.faction))
            {
                CurrentQuest.FactionReputationRequirementsList.Add(newrep);
                CurrentQuest.isDirty = true;
                QuestsFactionReputationRequirementsCB.Refresh();
            }
            else
            {
                MessageBox.Show("Faction allready exists in list......");
            }

        }
        private void darkButton83_Click(object sender, EventArgs e)
        {
            if (QuestsFactionReputationRequirementsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestsFactionReputationRequirementsLB.SelectedItem as FactionQuestReps;
            CurrentQuest.FactionReputationRequirementsList.Remove(fqr);
            CurrentQuest.isDirty = true;
            QuestsFactionReputationRequirementsLB.Refresh();
            if (QuestsFactionReputationRequirementsLB.Items.Count == 0)
                QuestsFactionReputationRequirementsLB.SelectedIndex = -1;
            else
                QuestsFactionReputationRequirementsLB.SelectedIndex = 0;
        }
        private void darkButton84_Click(object sender, EventArgs e)
        {
            FactionQuestReps newrep = new FactionQuestReps()
            {
                faction = QuestFactionReputationRewardsCB.GetItemText(QuestFactionReputationRewardsCB.SelectedItem),
                rep = 100
            };
            if (!CurrentQuest.FactionReputationRewardsList.Any(x => x.faction == newrep.faction))
            {
                CurrentQuest.FactionReputationRewardsList.Add(newrep);
                CurrentQuest.isDirty = true;
                QuestFactionReputationRewardsLB.Refresh();
            }
            else
            {
                MessageBox.Show("Faction allready exists in list......");
            }
        }
        private void darkButton85_Click(object sender, EventArgs e)
        {
            if (QuestFactionReputationRewardsLB.SelectedItems.Count < 1) return;
            FactionQuestReps fqr = QuestFactionReputationRewardsLB.SelectedItem as FactionQuestReps;
            CurrentQuest.FactionReputationRewardsList.Remove(fqr);
            CurrentQuest.isDirty = true;
            QuestFactionReputationRewardsLB.Refresh();
            if (QuestFactionReputationRewardsLB.Items.Count == 0)
                QuestFactionReputationRewardsLB.SelectedIndex = -1;
            else
                QuestFactionReputationRewardsLB.SelectedIndex = 0;
        }

        #endregion quests
        #region objectives
        public QuestObjectivesBase CurrentTreeNodeTag;
        public TreeNode currenttreenode;
        private void setupobjectives()
        {
            Console.WriteLine("\nserializing Full Objectives");
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
            for (int i = 0; i < QuestObjectives.Objectives.Count; i++)
            {
                switch (QuestObjectives.Objectives[i]._ObjectiveTypeEnum)
                {
                    case QuExpansionQuestObjectiveTypeestType.TARGET:
                        TreeNode newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTarget>(QuestObjectivesPath + "\\Target\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTarget;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TARGET;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTravel>(QuestObjectivesPath + "\\Travel\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTravel;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TRAVEL;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.COLLECT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCollection>(QuestObjectivesPath + "\\Collection\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCollection;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.COLLECT;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesCrafting>(QuestObjectivesPath + "\\Crafting\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesCrafting;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.CRAFTING;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesCrafting.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesDelivery>(QuestObjectivesPath + "\\Delivery\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesDelivery;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.DELIVERY;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesTreasureHunt>(QuestObjectivesPath + "\\TreasureHunt\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesTreasureHunt;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TREASUREHUNT;
                        QuestObjectives.Objectives[i].GetVec3List();
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIPatrol>(QuestObjectivesPath + "\\AIPatrol\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIPatrol;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIPATROL;
                        QuestObjectives.Objectives[i].GetVec3List();
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AICAMP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAICamp>(QuestObjectivesPath + "\\AICamp\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAICamp;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AICAMP;
                        QuestObjectives.Objectives[i].GetVec3List();
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.AIVIP:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAIVIP>(QuestObjectivesPath + "\\AIVIP\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAIVIP;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
                        QuestObjectives.Objectives[i]._ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIVIP;
                        newnode.Tag = QuestObjectives.Objectives[i];
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuExpansionQuestObjectiveTypeestType.ACTION:
                        newnode = new TreeNode(QuestObjectives.Objectives[i].Filename);
                        QuestObjectives.Objectives[i] = ReadJson<QuestObjectivesAction>(QuestObjectivesPath + "\\Action\\" + QuestObjectives.Objectives[i].Filename + ".json") as QuestObjectivesAction;
                        QuestObjectives.Objectives[i].Filename = newnode.Text;
                        QuestObjectives.Objectives[i].OriginalFilename = newnode.Text;
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
            currenttreenode = e.Node;
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
                ObjectivesQuestsListLB.DataSource = QuestsList.GetallQuests(CurrentTreeNodeTag);
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
        private void QuestObjectivesFilenameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            switch (CurrentTreeNodeTag._ObjectiveTypeEnum)
            {
                case QuExpansionQuestObjectiveTypeestType.TARGET:
                    QuestObjectivesTarget t = CurrentTreeNodeTag as QuestObjectivesTarget;
                    t.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = t.Filename;
                    t.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                    QuestObjectivesTravel tr = CurrentTreeNodeTag as QuestObjectivesTravel;
                    tr.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = tr.Filename;
                    tr.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.COLLECT:
                    QuestObjectivesCollection coll = CurrentTreeNodeTag as QuestObjectivesCollection;
                    coll.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = coll.Filename;
                    coll.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                    QuestObjectivesDelivery del = CurrentTreeNodeTag as QuestObjectivesDelivery;
                    del.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = del.Filename;
                    del.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                    QuestObjectivesTreasureHunt th = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                    th.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = th.Filename;
                    th.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                    QuestObjectivesAIPatrol aip = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                    aip.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = aip.Filename;
                    aip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AICAMP:
                    QuestObjectivesAICamp aic = CurrentTreeNodeTag as QuestObjectivesAICamp;
                    aic.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = aic.Filename;
                    aic.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIVIP:
                    QuestObjectivesAIVIP aivip = CurrentTreeNodeTag as QuestObjectivesAIVIP;
                    aivip.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = aivip.Filename;
                    aivip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.ACTION:
                    QuestObjectivesAction a = CurrentTreeNodeTag as QuestObjectivesAction;
                    a.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = a.Filename;
                    a.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                    QuestObjectivesCrafting c = CurrentTreeNodeTag as QuestObjectivesCrafting;
                    c.Filename = QuestObjectivesFilenameTB.Text;
                    currenttreenode.Text = c.Filename;
                    c.isDirty = true;
                    break;
                default:
                    break;
            }
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
        private void QuestObjectivesActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            switch (CurrentTreeNodeTag._ObjectiveTypeEnum)
            {
                case QuExpansionQuestObjectiveTypeestType.TARGET:
                    QuestObjectivesTarget t = CurrentTreeNodeTag as QuestObjectivesTarget;
                    t.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    t.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TRAVEL:
                    QuestObjectivesTravel tr = CurrentTreeNodeTag as QuestObjectivesTravel;
                    tr.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    tr.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.COLLECT:
                    QuestObjectivesCollection coll = CurrentTreeNodeTag as QuestObjectivesCollection;
                    coll.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    coll.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.DELIVERY:
                    QuestObjectivesDelivery del = CurrentTreeNodeTag as QuestObjectivesDelivery;
                    del.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    del.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.TREASUREHUNT:
                    QuestObjectivesTreasureHunt th = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
                    th.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    th.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIPATROL:
                    QuestObjectivesAIPatrol aip = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                    aip.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    aip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AICAMP:
                    QuestObjectivesAICamp aic = CurrentTreeNodeTag as QuestObjectivesAICamp;
                    aic.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    aic.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.AIVIP:
                    QuestObjectivesAIVIP aivip = CurrentTreeNodeTag as QuestObjectivesAIVIP;
                    aivip.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    aivip.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.ACTION:
                    QuestObjectivesAction a = CurrentTreeNodeTag as QuestObjectivesAction;
                    a.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
                    a.isDirty = true;
                    break;
                case QuExpansionQuestObjectiveTypeestType.CRAFTING:
                    QuestObjectivesCrafting c = CurrentTreeNodeTag as QuestObjectivesCrafting;
                    c.Active = QuestObjectivesActiveCB.Checked == true ? 1 : 0;
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
                OriginalFilename = "Objective_A_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.ACTION,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.ACTION,
                ObjectiveText = "New Action Objective",
                TimeLimit = -1,
                Active = 1,
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
                OriginalFilename = "Objective_AIC_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AICAMP,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AICAMP,
                ObjectiveText = "New AICamp Objective",
                TimeLimit = -1,
                Active = 1,
                InfectedDeletionRadius = (decimal)500.0,
                AISpawns = new BindingList<ExpansionAIPatrol>()
                {
                    new ExpansionAIPatrol()
                    {
                        Name = "NewAICamp",
                        Persist = 0,
                        Faction = "West",
                        Formation = "",
                        FormationLooseness = (decimal)0.0,
                        Loadout = "",
                        Units = new BindingList<string>(){
                            "eAI_SurvivorF_Eva",
                            "eAI_SurvivorF_Frida",
                            "eAI_SurvivorF_Gabi",
                            "eAI_SurvivorF_Helga",
                            "eAI_SurvivorF_Irena",
                            "eAI_SurvivorF_Judy",
                            "eAI_SurvivorF_Keiko",
                            "eAI_SurvivorF_Linda",
                            "eAI_SurvivorF_Maria",
                            "eAI_SurvivorF_Naomi",
                            "eAI_SurvivorF_Baty",
                            "eAI_SurvivorM_Boris",
                            "eAI_SurvivorM_Cyril",
                            "eAI_SurvivorM_Denis",
                            "eAI_SurvivorM_Elias",
                            "eAI_SurvivorM_Francis",
                            "eAI_SurvivorM_Guo",
                            "eAI_SurvivorM_Hassan",
                            "eAI_SurvivorM_Indar",
                            "eAI_SurvivorM_Jose",
                            "eAI_SurvivorM_Kaito",
                            "eAI_SurvivorM_Lewis",
                            "eAI_SurvivorM_Manua",
                            "eAI_SurvivorM_Mirek",
                            "eAI_SurvivorM_Niki",
                            "eAI_SurvivorM_Oliver",
                            "eAI_SurvivorM_Peter",
                            "eAI_SurvivorM_Quinn",
                            "eAI_SurvivorM_Rolf",
                            "eAI_SurvivorM_Seth",
                            "eAI_SurvivorM_Taiki"
                        },
                        NumberOfAI = 1,
                        Behaviour = "ALTERNATE",
                        Speed = "WALK",
                        UnderThreatSpeed = "SPRINT",
                        CanBeLooted = 1,
                        UnlimitedReload = 1,
                        SniperProneDistanceThreshold = (decimal)0.0,
                        AccuracyMin = -1,
                        AccuracyMax = -1,
                        ThreatDistanceLimit = -1,
                        NoiseInvestigationDistanceLimit = -1,
                        DamageMultiplier = -1,
                        DamageReceivedMultiplier = -1,
                        MinDistRadius = -1,
                        MaxDistRadius = -1,
                        DespawnRadius = -1,
                        MinSpreadRadius = 1,
                        MaxSpreadRadius = 100,
                        Chance = 1,
                        WaypointInterpolation = "",
                        DespawnTime = -1,
                        RespawnTime = -2,
                        UseRandomWaypointAsStartPoint = 1,
                        Waypoints = new BindingList<float[]>(),
                        _waypoints = new BindingList<Vec3>()
                    }
                },
                MinDistance = (decimal)-1.0,
                MaxDistance = (decimal)-1.0,
                AllowedWeapons = new BindingList<string>(),
                AllowedDamageZones = new BindingList<string>(),
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
                OriginalFilename = "Objective_AIP_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AIPATROL,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIPATROL,
                ObjectiveText = "New AIPatrol Objective",
                TimeLimit = -1,
                AISpawn = new ExpansionAIPatrol()
                {
                    Name = "NewAICamp",
                    Persist = 0,
                    Faction = "West",
                    Formation = "",
                    FormationLooseness = (decimal)0.0,
                    Loadout = "",
                    Units = new BindingList<string>()
                    {
                            "eAI_SurvivorF_Eva",
                            "eAI_SurvivorF_Frida",
                            "eAI_SurvivorF_Gabi",
                            "eAI_SurvivorF_Helga",
                            "eAI_SurvivorF_Irena",
                            "eAI_SurvivorF_Judy",
                            "eAI_SurvivorF_Keiko",
                            "eAI_SurvivorF_Linda",
                            "eAI_SurvivorF_Maria",
                            "eAI_SurvivorF_Naomi",
                            "eAI_SurvivorF_Baty",
                            "eAI_SurvivorM_Boris",
                            "eAI_SurvivorM_Cyril",
                            "eAI_SurvivorM_Denis",
                            "eAI_SurvivorM_Elias",
                            "eAI_SurvivorM_Francis",
                            "eAI_SurvivorM_Guo",
                            "eAI_SurvivorM_Hassan",
                            "eAI_SurvivorM_Indar",
                            "eAI_SurvivorM_Jose",
                            "eAI_SurvivorM_Kaito",
                            "eAI_SurvivorM_Lewis",
                            "eAI_SurvivorM_Manua",
                            "eAI_SurvivorM_Mirek",
                            "eAI_SurvivorM_Niki",
                            "eAI_SurvivorM_Oliver",
                            "eAI_SurvivorM_Peter",
                            "eAI_SurvivorM_Quinn",
                            "eAI_SurvivorM_Rolf",
                            "eAI_SurvivorM_Seth",
                            "eAI_SurvivorM_Taiki"
                        },
                    NumberOfAI = -3,
                    Behaviour = "ALTERNATE",
                    Speed = "WALK",
                    UnderThreatSpeed = "SPRINT",
                    CanBeLooted = 1,
                    UnlimitedReload = 1,
                    SniperProneDistanceThreshold = (decimal)0.0,
                    AccuracyMin = -1,
                    AccuracyMax = -1,
                    ThreatDistanceLimit = -1,
                    NoiseInvestigationDistanceLimit = -1,
                    DamageMultiplier = -1,
                    DamageReceivedMultiplier = -1,
                    MinDistRadius = -1,
                    MaxDistRadius = -1,
                    DespawnRadius = -1,
                    MinSpreadRadius = 1,
                    MaxSpreadRadius = 100,
                    Chance = 1,
                    WaypointInterpolation = "",
                    DespawnTime = -1,
                    RespawnTime = -2,
                    UseRandomWaypointAsStartPoint = 1,
                    Waypoints = new BindingList<float[]>(),
                    _waypoints = new BindingList<Vec3>()
                },
                MaxDistance = -1,
                MinDistance = -1,
                AllowedWeapons = new BindingList<string>(),
                AllowedDamageZones = new BindingList<string>(),
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
                Filename = "Objective_AIESCORT_" + newid.ToString(),
                OriginalFilename = "Objective_AIESCORT_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.AIVIP,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.AIVIP,
                ObjectiveText = "New AIESCORT Objective",
                TimeLimit = -1,
                Active = 1,
                Position = new decimal[] { 0, 0, 0 },
                MaxDistance = -1,
                NPCLoadoutFile = "BanditLoadout",
                NPCClassName = "eAI_SurvivorF_Maria",
                NPCName = "Survivior",
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
                OriginalFilename = "Objective_C_" + newid.ToString(),
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
                OriginalFilename = "Objective_CR_" + newid.ToString(),
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
                OriginalFilename = "Objective_D_" + newid.ToString(),
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
                OriginalFilename = "Objective_TA_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TARGET,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TARGET,
                ObjectiveText = "New Target Objective",
                TimeLimit = -1,
                Position = new decimal[] { 0, 0, 0 },
                MaxDistance = -1,
                Amount = 0,
                ClassNames = new BindingList<string>(),
                CountSelfKill = 0,
                CountAIPlayers = 0,
                AllowedWeapons = new BindingList<string>(),
                ExcludedClassNames = new BindingList<string>(),
                AllowedDamageZones = new BindingList<string>(),
                AllowedTargetFactions = new BindingList<string>(),
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
                OriginalFilename = "Objective_T_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TRAVEL,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TRAVEL,
                ObjectiveText = "New Travel Objective",
                TimeLimit = -1,
                Position = new decimal[] { 0, 0, 0 },
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
                OriginalFilename = "Objective_TH_" + newid.ToString(),
                ObjectiveType = (int)QuExpansionQuestObjectiveTypeestType.TREASUREHUNT,
                _ObjectiveTypeEnum = QuExpansionQuestObjectiveTypeestType.TREASUREHUNT,
                ObjectiveText = "New TreasureHunt Objective",
                TimeLimit = -1,
                ShowDistance = 0,
                ContainerName = "ExpansionQuestSeaChest",
                DigInStash = 1,
                MarkerName = "???",
                MarkerVisibility = 4,
                _Positions = new BindingList<Vec3>(),
                Positions = new BindingList<decimal[]>(),
                Loot = new BindingList<ExpansionLoot>(),
                LootItemsAmount = 2,
                MaxDistance = 10,
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
            foreach (Quests quests in QuestsList.QuestList)
            {
                if (quests.Objectives.Any(x => x.ID == CurrentTreeNodeTag.ID) &&
                   quests.Objectives.Any(y => y.ObjectiveType == CurrentTreeNodeTag.ObjectiveType))
                {
                    quests.Objectives.Remove(CurrentTreeNodeTag);
                    quests.isDirty = true;
                }
            }
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
            QuestObjectivesActiveCB.Checked = CurrentAction.Active == 1 ? true : false;

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
        private void darkButton71_Click(object sender, EventArgs e)
        {
            QuestObjectivesAction CurrentAction = CurrentTreeNodeTag as QuestObjectivesAction;
            CurrentAction.ActionNames.Add(ObjectivesActionsCB.GetItemText(ObjectivesActionsCB.SelectedItem));
            CurrentTreeNodeTag.isDirty = true;
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
            QuestObjectivesActiveCB.Checked = CurrentAICamp.Active == 1 ? true : false;
            QuestObjectovesInfectedDeletionRadiusNUD.Value = CurrentAICamp.InfectedDeletionRadius;
            ObjectivesAICampMaxDistanceNUD.Value = CurrentAICamp.MaxDistance;
            ObjectivesAICampMinDistanceNUD.Value = CurrentAICamp.MinDistance;

            ObjectivesAICampAISpawnsLB.DisplayMember = "DisplayName";
            ObjectivesAICampAISpawnsLB.ValueMember = "Value";
            ObjectivesAICampAISpawnsLB.DataSource = CurrentAICamp.AISpawns;

            ObjectivesAICampAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesAICampAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesAICampAllowedWeaponsLB.DataSource = CurrentAICamp.AllowedWeapons;

            ObjectivesAICampAllowedDamageZonesLB.DisplayMember = "DisplayName";
            ObjectivesAICampAllowedDamageZonesLB.ValueMember = "Value";
            ObjectivesAICampAllowedDamageZonesLB.DataSource = CurrentAICamp.AllowedDamageZones;

            useraction = true;
        }
        private void QuestObjectovesInfectedDeletionRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.InfectedDeletionRadius = (int)QuestObjectovesInfectedDeletionRadiusNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampMinDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.MinDistance = ObjectivesAICampMinDistanceNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.MaxDistance = ObjectivesAICampMaxDistanceNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampAllowedWeaponsAddButton_Click(object sender, EventArgs e)
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
                    if (!CurrentAICam.AllowedWeapons.Contains(l))
                    {
                        CurrentAICam.AllowedWeapons.Add(l);
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
        private void ObjectivesAICampAllowedWeaponsRemoveButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count < 1) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            for (int i = 0; i < ObjectivesAICampAllowedWeaponsLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AllowedWeapons.Remove(ObjectivesAICampAllowedWeaponsLB.GetItemText(ObjectivesAICampAllowedWeaponsLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampAllowedDamageZonesAddButton_Click(object sender, EventArgs e)
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
                    if (!CurrentAICam.AllowedDamageZones.Contains(l))
                    {
                        CurrentAICam.AllowedDamageZones.Add(l);
                        CurrentAICam.isDirty = true;
                    }
                }
                ObjectivesAICampAllowedDamageZonesLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ObjectivesAICampAllowedDamageZonesRemoveButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAICampAllowedDamageZonesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            for (int i = 0; i < ObjectivesAICampAllowedDamageZonesLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AllowedDamageZones.Remove(ObjectivesAICampAllowedDamageZonesLB.GetItemText(ObjectivesAICampAllowedDamageZonesLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAICampAISpawnsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesAICampAISpawnsLB.SelectedItems.Count < 1) return;
            expansionQuestAISpawnControlAICamp.currentAISpawn = ObjectivesAICampAISpawnsLB.SelectedItem as ExpansionAIPatrol;
            expansionQuestAISpawnControlAICamp.setAInumASReadOnly(true);
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            expansionQuestAISpawnControlAICamp.isDirty = CurrentAICam.isDirty;
        }
        private void darkButton89_Click(object sender, EventArgs e)
        {
            ExpansionAIPatrol newAISpawn = new ExpansionAIPatrol()
            {
                Name = "NewAICamp",
                Persist = 0,
                Faction = "West",
                Formation = "",
                FormationLooseness = (decimal)0.0,
                Loadout = "",
                Units = new BindingList<string>()
                {
                            "eAI_SurvivorF_Eva",
                            "eAI_SurvivorF_Frida",
                            "eAI_SurvivorF_Gabi",
                            "eAI_SurvivorF_Helga",
                            "eAI_SurvivorF_Irena",
                            "eAI_SurvivorF_Judy",
                            "eAI_SurvivorF_Keiko",
                            "eAI_SurvivorF_Linda",
                            "eAI_SurvivorF_Maria",
                            "eAI_SurvivorF_Naomi",
                            "eAI_SurvivorF_Baty",
                            "eAI_SurvivorM_Boris",
                            "eAI_SurvivorM_Cyril",
                            "eAI_SurvivorM_Denis",
                            "eAI_SurvivorM_Elias",
                            "eAI_SurvivorM_Francis",
                            "eAI_SurvivorM_Guo",
                            "eAI_SurvivorM_Hassan",
                            "eAI_SurvivorM_Indar",
                            "eAI_SurvivorM_Jose",
                            "eAI_SurvivorM_Kaito",
                            "eAI_SurvivorM_Lewis",
                            "eAI_SurvivorM_Manua",
                            "eAI_SurvivorM_Mirek",
                            "eAI_SurvivorM_Niki",
                            "eAI_SurvivorM_Oliver",
                            "eAI_SurvivorM_Peter",
                            "eAI_SurvivorM_Quinn",
                            "eAI_SurvivorM_Rolf",
                            "eAI_SurvivorM_Seth",
                            "eAI_SurvivorM_Taiki"
                },
                NumberOfAI = 1,
                Behaviour = "ALTERNATE",
                Speed = "WALK",
                UnderThreatSpeed = "SPRINT",
                CanBeLooted = 1,
                UnlimitedReload = 1,
                SniperProneDistanceThreshold = (decimal)0.0,
                AccuracyMin = -1,
                AccuracyMax = -1,
                ThreatDistanceLimit = -1,
                NoiseInvestigationDistanceLimit = -1,
                DamageMultiplier = -1,
                DamageReceivedMultiplier = -1,
                MinDistRadius = -1,
                MaxDistRadius = -1,
                DespawnRadius = -1,
                MinSpreadRadius = 1,
                MaxSpreadRadius = 100,
                Chance = 1,
                WaypointInterpolation = "",
                DespawnTime = -1,
                RespawnTime = -2,
                UseRandomWaypointAsStartPoint = 1,
                Waypoints = new BindingList<float[]>(),
                _waypoints = new BindingList<Vec3>()
            };
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            if (CurrentAICam.AISpawns.Count <= 0)
            {
                CurrentAICam.AISpawns = new BindingList<ExpansionAIPatrol>();
                ObjectivesAICampAISpawnsLB.DisplayMember = "DisplayName";
                ObjectivesAICampAISpawnsLB.ValueMember = "Value";
                ObjectivesAICampAISpawnsLB.DataSource = CurrentAICam.AISpawns;
            }
            CurrentAICam.AISpawns.Add(newAISpawn);
            ObjectivesAICampAISpawnsLB.Invalidate();
            expansionQuestAISpawnControlAICamp.isDirty = true;
        }
        private void darkButton88_Click(object sender, EventArgs e)
        {
            if (ObjectivesAICampAISpawnsLB.SelectedItems.Count < 1) return;
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            ExpansionAIPatrol currentAISpawn = ObjectivesAICampAISpawnsLB.SelectedItem as ExpansionAIPatrol;
            CurrentAICam.AISpawns.Remove(currentAISpawn);
            ObjectivesAICampAISpawnsLB.Invalidate();
            CurrentAICam.isDirty = true;

        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            if (CurrentAICam.AISpawns.Count <= 0) CurrentAICam.AISpawns = new BindingList<ExpansionAIPatrol>();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng AI Spawns?", "Clear Spawns", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentAICam.AISpawns = new BindingList<ExpansionAIPatrol>();
                        ObjectivesAICampAISpawnsLB.DisplayMember = "DisplayName";
                        ObjectivesAICampAISpawnsLB.ValueMember = "Value";
                        ObjectivesAICampAISpawnsLB.DataSource = CurrentAICam.AISpawns;
                    }
                    ExpansionAIPatrol newAISpawn = null;
                    string objname = "";
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        if (objname != "" && eo.DisplayName != objname)
                        {
                            CurrentAICam.AISpawns.Add(newAISpawn);
                            newAISpawn = null;
                        }
                        if (newAISpawn == null)
                        {
                            newAISpawn = new ExpansionAIPatrol()
                            {
                                Name = "NewAICamp",
                                Faction = "West",
                                Formation = "",
                                FormationLooseness = (decimal)0.0,
                                Loadout = "",
                                Units = new BindingList<string>()
                                {
                                            "eAI_SurvivorF_Eva",
                                            "eAI_SurvivorF_Frida",
                                            "eAI_SurvivorF_Gabi",
                                            "eAI_SurvivorF_Helga",
                                            "eAI_SurvivorF_Irena",
                                            "eAI_SurvivorF_Judy",
                                            "eAI_SurvivorF_Keiko",
                                            "eAI_SurvivorF_Linda",
                                            "eAI_SurvivorF_Maria",
                                            "eAI_SurvivorF_Naomi",
                                            "eAI_SurvivorF_Baty",
                                            "eAI_SurvivorM_Boris",
                                            "eAI_SurvivorM_Cyril",
                                            "eAI_SurvivorM_Denis",
                                            "eAI_SurvivorM_Elias",
                                            "eAI_SurvivorM_Francis",
                                            "eAI_SurvivorM_Guo",
                                            "eAI_SurvivorM_Hassan",
                                            "eAI_SurvivorM_Indar",
                                            "eAI_SurvivorM_Jose",
                                            "eAI_SurvivorM_Kaito",
                                            "eAI_SurvivorM_Lewis",
                                            "eAI_SurvivorM_Manua",
                                            "eAI_SurvivorM_Mirek",
                                            "eAI_SurvivorM_Niki",
                                            "eAI_SurvivorM_Oliver",
                                            "eAI_SurvivorM_Peter",
                                            "eAI_SurvivorM_Quinn",
                                            "eAI_SurvivorM_Rolf",
                                            "eAI_SurvivorM_Seth",
                                            "eAI_SurvivorM_Taiki"
                                },
                                NumberOfAI = 1,
                                Behaviour = "ALTERNATE",
                                Speed = "WALK",
                                UnderThreatSpeed = "SPRINT",
                                CanBeLooted = 1,
                                UnlimitedReload = 1,
                                SniperProneDistanceThreshold = (decimal)0.0,
                                AccuracyMin = -1,
                                AccuracyMax = -1,
                                ThreatDistanceLimit = -1,
                                NoiseInvestigationDistanceLimit = -1,
                                DamageMultiplier = -1,
                                DamageReceivedMultiplier = -1,
                                MinDistRadius = -1,
                                MaxDistRadius = -1,
                                DespawnRadius = -1,
                                MinSpreadRadius = 1,
                                MaxSpreadRadius = 100,
                                Chance = 1,
                                WaypointInterpolation = "",
                                DespawnTime = -1,
                                RespawnTime = -2,
                                UseRandomWaypointAsStartPoint = 1,
                                Waypoints = new BindingList<float[]>(),
                                _waypoints = new BindingList<Vec3>()
                            };
                        }
                        newAISpawn._waypoints.Add(new Vec3(eo.Position));
                        objname = eo.DisplayName;
                    }
                    CurrentAICam.AISpawns.Add(newAISpawn);
                    ObjectivesAICampAISpawnsLB.Invalidate();
                    CurrentAICam.isDirty = true;
                }
            }
        }
        private void expansionQuestAISpawnControlAICamp_IsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            QuestObjectivesAICamp CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAICamp;
            CurrentAICam.isDirty = expansionQuestAISpawnControlAICamp.isDirty;
        }
        /// <summary>
        /// AI Patrol
        /// </summary>
        private void SetupobjectiveAIPatrol(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesAIPatrol CurrentAIPatrol = e.Node.Tag as QuestObjectivesAIPatrol;
            QuestObjectivesObjectiveTextTB.Text = CurrentAIPatrol.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentAIPatrol.TimeLimit;
            QuestObjectivesActiveCB.Checked = CurrentAIPatrol.Active == 1 ? true : false;
            ObjectivesAIPatrolMaxDistanceNUD.Value = CurrentAIPatrol.MaxDistance;
            ObjectivesAIPatrolMinDistanceNUD.Value = CurrentAIPatrol.MinDistance;

            expansionQuestAISpawnControlAIPatrol.currentAISpawn = CurrentAIPatrol.AISpawn;
            expansionQuestAISpawnControlAIPatrol.isDirty = CurrentAIPatrol.isDirty;

            ObjectivesAIPatrolAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesAIPatrolAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesAIPatrolAllowedWeaponsLB.DataSource = CurrentAIPatrol.AllowedWeapons;

            ObjectivesAIPatrolAllowedDamageZonesLB.DisplayMember = "DisplayName";
            ObjectivesAIPatrolAllowedDamageZonesLB.ValueMember = "Value";
            ObjectivesAIPatrolAllowedDamageZonesLB.DataSource = CurrentAIPatrol.AllowedDamageZones;
            useraction = true;
        }
        private void expansionQuestAISpawnControlAIPatrol_IsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAIPatrol.isDirty = expansionQuestAISpawnControlAIPatrol.isDirty;
        }
        private void ObjectivesAIPatrolMinDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAICam.MinDistance = ObjectivesAIPatrolMinDistanceNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAIPatrolMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            CurrentAICam.MaxDistance = ObjectivesAIPatrolMaxDistanceNUD.Value;
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAIPatrolAllowedWeaponsAddButton_Click(object sender, EventArgs e)
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
                QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                foreach (string l in addedtypes)
                {
                    if (!CurrentAICam.AllowedWeapons.Contains(l))
                    {
                        CurrentAICam.AllowedWeapons.Add(l);
                        CurrentAICam.isDirty = true;
                    }
                }
                ObjectivesAIPatrolAllowedWeaponsLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ObjectivesAIPatrolAllowedWeaponsRemoveButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAIPatrolAllowedWeaponsLB.SelectedItems.Count < 1) return;
            QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            for (int i = 0; i < ObjectivesAIPatrolAllowedWeaponsLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AllowedWeapons.Remove(ObjectivesAIPatrolAllowedWeaponsLB.GetItemText(ObjectivesAIPatrolAllowedWeaponsLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
        }
        private void ObjectivesAIPatrolAllowedDamageZonesAddButton_Click(object sender, EventArgs e)
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
                QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
                foreach (string l in addedtypes)
                {
                    if (!CurrentAICam.AllowedDamageZones.Contains(l))
                    {
                        CurrentAICam.AllowedDamageZones.Add(l);
                        CurrentAICam.isDirty = true;
                    }
                }
                ObjectivesAIPatrolAllowedDamageZonesLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ObjectivesAIPatrolAllowedDamageZonesRemoveButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAIPatrolAllowedDamageZonesLB.SelectedItems.Count < 1) return;
            QuestObjectivesAIPatrol CurrentAICam = CurrentTreeNodeTag as QuestObjectivesAIPatrol;
            for (int i = 0; i < ObjectivesAIPatrolAllowedDamageZonesLB.SelectedItems.Count; i++)
            {
                CurrentAICam.AllowedDamageZones.Remove(ObjectivesAIPatrolAllowedDamageZonesLB.GetItemText(ObjectivesAIPatrolAllowedDamageZonesLB.SelectedItems[0]));
            }
            CurrentAICam.isDirty = true;
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
            QuestObjectivesActiveCB.Checked = CurrentAIVIP.Active == 1 ? true : false;
            ObjectivesAIVIPPositionXNUD.Value = (decimal)CurrentAIVIP.Position[0];
            ObjectivesAIVIPPositionYNUD.Value = (decimal)CurrentAIVIP.Position[1];
            ObjectivesAIVIPPositionZNUD.Value = (decimal)CurrentAIVIP.Position[2];
            ObjectivesAIVIPMaxDistanceNUD.Value = CurrentAIVIP.MaxDistance;
            ObjectivesAIVIPNPCLoadoutFileCB.SelectedIndex = ObjectivesAIVIPNPCLoadoutFileCB.FindStringExact(CurrentAIVIP.NPCLoadoutFile);
            ObjectivesAIVIPMarkerNameTB.Text = CurrentAIVIP.MarkerName;
            QuestObjectivesAIVIPShowDistanceCB.Checked = CurrentAIVIP.ShowDistance == 1 ? true : false;
            QuestObjectivesAIVIPCanLootAICB.Checked = CurrentAIVIP.CanLootAI == 1 ? true : false;
            ObjectivesAIVIPNPCNPCClassnameTB.Text = CurrentAIVIP.NPCClassName;
            ObjectivesAIVIPNPCNameTB.Text = CurrentAIVIP.NPCName;
            useraction = true;
        }
        private void ObjectivesAIVIPPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[0] = (decimal)ObjectivesAIVIPPositionXNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[1] = (decimal)ObjectivesAIVIPPositionYNUD.Value;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.Position[2] = (decimal)ObjectivesAIVIPPositionZNUD.Value;
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
            CurrentAIVIP.NPCLoadoutFile = ObjectivesAIVIPNPCLoadoutFileCB.GetItemText(ObjectivesAIVIPNPCLoadoutFileCB.SelectedItem);
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
        private void QuestObjectivesAIVIPCanLootAICB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.CanLootAI = QuestObjectivesAIVIPCanLootAICB.Checked == true ? 1 : 0;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPNPCNPCClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.NPCClassName = ObjectivesAIVIPNPCNPCClassnameTB.Text;
            CurrentAIVIP.isDirty = true;
        }
        private void ObjectivesAIVIPNPCNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesAIVIP CurrentAIVIP = CurrentTreeNodeTag as QuestObjectivesAIVIP;
            CurrentAIVIP.NPCName = ObjectivesAIVIPNPCNameTB.Text;
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
            QuestObjectivesActiveCB.Checked = CurrentCollection.Active == 1 ? true : false;
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
            ObjectivesCollectionQuantityPercentNUD.Value = CurrentCollections.QuantityPercent;
            ObjectivesCollectionMinQuantityPercentNUD.Value = CurrentCollections.MinQuantityPercent;
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
        private void ObjectivesCollectionQuantityPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollections.QuantityPercent = (int)ObjectivesCollectionQuantityPercentNUD.Value;
            CurrentCollection.isDirty = true;
        }
        private void ObjectivesCollectionMinQuantityPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesCollection CurrentCollection = CurrentTreeNodeTag as QuestObjectivesCollection;
            CurrentCollections.MinQuantityPercent = (int)ObjectivesCollectionMinQuantityPercentNUD.Value;
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
            QuestObjectivesActiveCB.Checked = CurrentCrafting.Active == 1 ? true : false;
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
            QuestObjectivesActiveCB.Checked = CurrentDelivery.Active == 1 ? true : false;
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
            ObjectivesDeliveryQuantityPercentNUD.Value = CurrentDeliveries.QuantityPercent;
            ObjectivesDeliveryMinQuantityPerentNUD.Value = CurrentDeliveries.MinQuantityPercent;
            useraction = true;
        }
        private void ObjectivesDeliveryMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDelivery.MaxDistance = ObjectivesDeliveryMaxDistanceNUD.Value;
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
        private void ObjectivesDeliveryQuantityPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDeliveries.QuantityPercent = (int)ObjectivesDeliveryQuantityPercentNUD.Value;
            CurrentDelivery.isDirty = true;
        }
        private void ObjectivesDeliveryMinQuantityPerentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesDelivery CurrentDelivery = CurrentTreeNodeTag as QuestObjectivesDelivery;
            CurrentDeliveries.MinQuantityPercent = (int)ObjectivesDeliveryMinQuantityPerentNUD.Value;
            CurrentDelivery.isDirty = true;
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
            QuestObjectivesActiveCB.Checked = CurrentTarget.Active == 1 ? true : false;
            ObjectivesTargetPositionXNUD.Value = (decimal)CurrentTarget.Position[0];
            ObjectivesTargetPositionYNUD.Value = (decimal)CurrentTarget.Position[1];
            ObjectivesTargetPositionZNUD.Value = (decimal)CurrentTarget.Position[2];
            ObjectivesTargetMaxDistanceNUD.Value = CurrentTarget.MaxDistance;
            ObjectivesTargetAmountNUD.Value = CurrentTarget.Amount;
            ObjectivesTargetCountSelfKillCB.Checked = CurrentTarget.CountSelfKill == 1 ? true : false;
            checkBox7.Checked = CurrentTarget.CountAIPlayers == 1 ? true : false;

            ObjectivesTargetClassnameLB.DisplayMember = "DisplayName";
            ObjectivesTargetClassnameLB.ValueMember = "Value";
            ObjectivesTargetClassnameLB.DataSource = CurrentTarget.ClassNames;

            ObjectivesTargetAllowedWeaponsLB.DisplayMember = "DisplayName";
            ObjectivesTargetAllowedWeaponsLB.ValueMember = "Value";
            ObjectivesTargetAllowedWeaponsLB.DataSource = CurrentTarget.AllowedWeapons;

            ObjectivesTargetExcludedClassnamesLB.DisplayMember = "DisplayName";
            ObjectivesTargetExcludedClassnamesLB.ValueMember = "Value";
            ObjectivesTargetExcludedClassnamesLB.DataSource = CurrentTarget.ExcludedClassNames;

            ObjectivesTargetAllowedTargetFactionsLB.DisplayMember = "DisplayName";
            ObjectivesTargetAllowedTargetFactionsLB.ValueMember = "Value";
            ObjectivesTargetAllowedTargetFactionsLB.DataSource = CurrentTarget.AllowedTargetFactions;

            ObjectivesTargetAllowedDamageZonesLB.DisplayMember = "DisplayName";
            ObjectivesTargetAllowedDamageZonesLB.ValueMember = "Value";
            ObjectivesTargetAllowedDamageZonesLB.DataSource = CurrentTarget.AllowedDamageZones;

            useraction = true;

        }
        private void ObjectivesTargetPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[0] = (decimal)ObjectivesTargetPositionXNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[1] = (decimal)ObjectivesTargetPositionYNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Position[2] = (decimal)ObjectivesTargetPositionZNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.MaxDistance = ObjectivesTargetMaxDistanceNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetMinDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.MinDistance = ObjectivesTargetMinDistanceNUD.Value;
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.Amount = (int)ObjectivesTargetAmountNUD.Value;
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
                    if (!CurrentTarget.ClassNames.Contains(l))
                        CurrentTarget.ClassNames.Add(l);
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
                CurrentTarget.ClassNames.Remove(ObjectivesTargetClassnameLB.GetItemText(ObjectivesTargetClassnameLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        private void ObjectivesTargetCountSelfKillCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.CountSelfKill = ObjectivesTargetCountSelfKillCB.Checked == true ? 1 : 0;
            CurrentTarget.isDirty = true;
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            CurrentTarget.CountAIPlayers = checkBox7.Checked == true ? 1 : 0;
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
                    if (!CurrentTarget.AllowedWeapons.Contains(l))
                    {
                        CurrentTarget.AllowedWeapons.Add(l);
                        CurrentTarget.isDirty = true;
                    }
                }
                ObjectivesTargetAllowedWeaponsLB.Refresh();
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
                CurrentTarget.AllowedWeapons.Remove(ObjectivesTargetAllowedWeaponsLB.GetItemText(ObjectivesTargetAllowedWeaponsLB.SelectedItems[0]));
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
                    if (!CurrentTarget.ExcludedClassNames.Contains(l))
                        CurrentTarget.ExcludedClassNames.Add(l);
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
                CurrentTarget.ExcludedClassNames.Remove(ObjectivesTargetExcludedClassnamesLB.GetItemText(ObjectivesTargetExcludedClassnamesLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton81_Click(object sender, EventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTarget.AllowedTargetFactions.Contains(l))
                        CurrentTarget.AllowedTargetFactions.Add(l);
                }
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton80_Click(object sender, EventArgs e)
        {
            if (ObjectivesTargetAllowedTargetFactionsLB.SelectedItems.Count < 1) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            for (int i = 0; i < ObjectivesTargetAllowedTargetFactionsLB.SelectedItems.Count; i++)
            {
                CurrentTarget.AllowedTargetFactions.Remove(ObjectivesTargetAllowedTargetFactionsLB.GetItemText(ObjectivesTargetAllowedTargetFactionsLB.SelectedItems[0]));
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTarget.AllowedDamageZones.Contains(l))
                        CurrentTarget.AllowedDamageZones.Add(l);
                }
            }
            CurrentTarget.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            if (ObjectivesTargetAllowedDamageZonesLB.SelectedItems.Count < 1) return;
            QuestObjectivesTarget CurrentTarget = CurrentTreeNodeTag as QuestObjectivesTarget;
            for (int i = 0; i < ObjectivesTargetAllowedDamageZonesLB.SelectedItems.Count; i++)
            {
                CurrentTarget.AllowedDamageZones.Remove(ObjectivesTargetAllowedDamageZonesLB.GetItemText(ObjectivesTargetAllowedDamageZonesLB.SelectedItems[0]));
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
            QuestObjectivesActiveCB.Checked = CurrentTravel.Active == 1 ? true : false;
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
            CurrentTravel.Position[0] = (decimal)ObjectivesTravelPositionXNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.Position[1] = (decimal)ObjectivesTravelPositionYNUD.Value;
            CurrentTravel.isDirty = true;
        }
        private void ObjectivesTravelPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTravel CurrentTravel = CurrentTreeNodeTag as QuestObjectivesTravel;
            CurrentTravel.Position[2] = (decimal)ObjectivesTravelPositionZNUD.Value;
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
        public Vec3 CurrentWapypoint { get; private set; }
        private void SetupobjectiveTreasueHunt(TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = e.Node.Tag as QuestObjectivesTreasureHunt;
            QuestObjectivesObjectiveTextTB.Text = CurrentTreasureHunt.ObjectiveText;
            QuestObjectivesTimeLimitNUD.Value = CurrentTreasureHunt.TimeLimit;
            QuestObjectivesActiveCB.Checked = CurrentTreasureHunt.Active == 1 ? true : false;
            ObjectivesTreasureHuntShowdistanceCB.Checked = CurrentTreasureHunt.ShowDistance == 1 ? true : false;
            ObjectivesTreasureHuntAmountNUD.Value = CurrentTreasureHunt.LootItemsAmount;
            checkBox10.Checked = CurrentTreasureHunt.DigInStash == 1 ? true : false;
            numericUpDown16.Value = CurrentTreasureHunt.MaxDistance;
            numericUpDown17.Value = CurrentTreasureHunt.MarkerVisibility;
            textBox2.Text = CurrentTreasureHunt.ContainerName;
            textBox1.Text = CurrentTreasureHunt.MarkerName;

            ObjectivesTreasureHuntPositionsLB.DisplayMember = "DisplayName";
            ObjectivesTreasureHuntPositionsLB.ValueMember = "Value";
            ObjectivesTreasureHuntPositionsLB.DataSource = CurrentTreasureHunt._Positions;

            expansionLootControlTH.LootparentName = CurrentTreasureHunt.ObjectiveText;
            expansionLootControlTH.currentExpansionLoot = CurrentTreasureHunt.Loot;
            expansionLootControlTH.vanillatypes = vanillatypes;
            expansionLootControlTH.ModTypes = ModTypes;
            expansionLootControlTH.currentproject = currentproject;

            useraction = true;
        }
        private void ObjectivesTreasureHuntPositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesTreasureHuntPositionsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = ObjectivesTreasureHuntPositionsLB.SelectedItem as Vec3;
            useraction = false;
            ObjectivesTreasureHuntPositionsXNUD.Value = (decimal)CurrentWapypoint.X;
            ObjectivesTreasureHuntPositionsYNUD.Value = (decimal)CurrentWapypoint.Y;
            ObjectivesTreasureHuntPositionsZNUD.Value = (decimal)CurrentWapypoint.Z;
            useraction = true;
        }
        private void darkButton68_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt._Positions.Add(new Vec3(0, 0, 0));
            if (ObjectivesTreasureHuntPositionsLB.Items.Count > 0)
            {
                ObjectivesTreasureHuntPositionsLB.SelectedIndex = ObjectivesTreasureHuntPositionsLB.Items.Count - 1;
            }
            else
            {
                ObjectivesTreasureHuntPositionsLB.SelectedIndex = 0;
            }
            CurrentTreasureHunt.isDirty = true;
        }
        private void darkButton67_Click(object sender, EventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt._Positions.Remove(CurrentWapypoint);
            CurrentTreasureHunt.isDirty = true;
            ObjectivesTreasureHuntPositionsLB.Refresh();
            if (ObjectivesTreasureHuntPositionsLB.Items.Count > 0)
            {
                ObjectivesTreasureHuntPositionsLB.SelectedIndex = 0;
            }
            else
            {
                CurrentWapypoint = null;
            }
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
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentTreasureHunt._Positions.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        decimal[] newfloatarray = new decimal[] { Convert.ToDecimal(XYZ[0]), Convert.ToDecimal(XYZ[1]), Convert.ToDecimal(XYZ[2]) };
                        CurrentTreasureHunt._Positions.Add(new Vec3(newfloatarray));

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
            foreach (Vec3 v3 in CurrentTreasureHunt._Positions)
            {
                SB.AppendLine("UndergroundStash|" + v3.GetString() + "|0.0 0.0 0.0");
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
                        CurrentTreasureHunt._Positions.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        CurrentTreasureHunt._Positions.Add(new Vec3(eo.Position));
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
            int m_Id = 0;
            foreach (Vec3 v3 in CurrentTreasureHunt._Positions)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "UndergroundStash",
                    DisplayName = "UndergroundStash",
                    Position = v3.getfloatarray(),
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(eo);
                m_Id++;
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
            if (CurrentWapypoint == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint.X = (float)ObjectivesTreasureHuntPositionsXNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntPositionsYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentWapypoint == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint.Y = (float)ObjectivesTreasureHuntPositionsYNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntPositionsZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentWapypoint == null) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentWapypoint.Z = (float)ObjectivesTreasureHuntPositionsZNUD.Value;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntShowdistanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.ShowDistance = ObjectivesTreasureHuntShowdistanceCB.Checked == true ? 1 : 0;
            CurrentTreasureHunt.isDirty = true;
        }
        private void ObjectivesTreasureHuntAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.LootItemsAmount = (int)ObjectivesTreasureHuntAmountNUD.Value;
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
        private void expansionLootControlTH_IsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = CurrentTreeNodeTag as QuestObjectivesTreasureHunt;
            CurrentTreasureHunt.isDirty = expansionLootControlTH.isDirty;
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
            foreach (QuestPlayerData QPD in QuestPlayerDataList.QuestPlayerDataList)
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
            for (int i = 0; i < QPD.ExpansionQuestPersistentQuestDataCount; i++)
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
