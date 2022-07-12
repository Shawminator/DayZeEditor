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

        public NPCEmotes NPCEmotes { get; private set; }
        public QuestSettings QuestSettings { get; private set; }
        public QuestObjectives QuestObjectives { get; set; }
        public ExpansioQuestList QuestsList { get; private set; }
        public QuestNPCLists QuestNPCs { get; private set; }

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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton7.Checked = false;
            toolStripButton1.Checked = false;
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
                if (QuestSettings.checkver())
                    needtosave = true;
            }
            QuestSettings.Filename = QuestsSettingsPath;
            setupQuestsettings();

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

            tabControl1.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
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
            QuestNPCs.setupquestlists(QuestsList);

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
                if (currentproject.Createbackups && File.Exists(QuestsPath + "\\" + npcs.Filename + ".json"))
                {
                    Directory.CreateDirectory(QuestNPCPath + "\\Backup\\" + SaveTime);
                    File.Copy(QuestsPath + "\\" + npcs.Filename + ".json", QuestsPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(npcs.Filename) + ".bak", true);
                }
                File.WriteAllText(QuestsPath + "\\" + npcs.Filename + ".json", jsonString);
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

            if (QuestNPCs.Markedfordelete == null) return;
            foreach (ExpansionQuestNPCs del in QuestNPCs.Markedfordelete)
            {
                
                del.backupandDelete(QuestNPCPath);
            }
            foreach (Quests del in QuestsList.Markedfordelete)
            {
                del.backupandDelete(QuestsPath);
            }
        }


        #region questsettings
        private void setupQuestsettings()
        {
            useraction = false;

            string[] boolignorenames = new string[] { "m_Version", "WeeklyQuestResetHour", "WeeklyQuestResteMinute", "DailyQuestResetHour", "DailyQuestResetMinute" };
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
            if (QuestStringsLB.SelectedItems.Count < 1) return;
            useraction = false;
            QuestIntsNUD.Value = (int)Helper.GetPropValue(QuestSettings, QuestIntsLB.GetItemText(QuestIntsLB.SelectedItem));
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
            QuestObjectivesNPCLoadoutsCB.DisplayMember = "DisplayName";
            QuestObjectivesNPCLoadoutsCB.ValueMember = "Value";
            QuestObjectivesNPCLoadoutsCB.DataSource = loadoutsobjectives;



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
        #endregion npc
        #region quests
        public Quests CurrentQuest { get; private set; }
        public void setupquests()
        {
            useraction = false;

            

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
            QuestQuestGiverIDCB.SelectedItem = QuestQuestGiverIDCB.Items.Cast<ExpansionQuestNPCs>().FirstOrDefault(z => z.ID == CurrentQuest.QuestGiverID);
            QuestQuestTurnInIDCB.SelectedItem = QuestQuestTurnInIDCB.Items.Cast<ExpansionQuestNPCs>().FirstOrDefault(z => z.ID == CurrentQuest.QuestTurnInID);
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
                        CurrentQuest.Objectives[i].isDirty = true;
                    }
                }
            }
            QuestNeedToSelectRewardCB.Checked = CurrentQuest.NeedToSelectReward == 1 ? true : false;
            QuestRewardsForGroupOwnerOnlyCB.Checked = CurrentQuest.RewardsForGroupOwnerOnly == 1 ? true : false;
            QuestHumanityRewardCB.Checked = CurrentQuest.HumanityReward == 1 ? true : false;

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
        private void QuestRewardsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestRewardsLB.SelectedItems.Count < 1) return;
            QuestReward qr = QuestRewardsLB.SelectedItem as QuestReward;
            useraction = false;
            QuestRewardsAmountNUD.Value = qr.Amount;
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
            CurrentQuest.ObjectiveText = QuestObjectivesObjectiveTextTB.Text;
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
            CurrentQuest.QuestGiverID = npc.ID;
            CurrentQuest.isDirty = true;
        }
        private void QuestQuestTurnInIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            ExpansionQuestNPCs npc = QuestQuestTurnInIDCB.SelectedItem as ExpansionQuestNPCs;
            CurrentQuest.QuestTurnInID = npc.ID;
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
            CurrentQuest.IsAchivement = QuestIsAchivementCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void QuestIsDailyQuestCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentQuest.IsAchivement = QuestIsAchivementCB.Checked == true ? 1 : 0;
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
            CurrentQuest.HumanityReward = QuestHumanityRewardCB.Checked == true ? 1 : 0;
            CurrentQuest.isDirty = true;
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = true,
                isCategoryitem = true
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
                        Amount = 1
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
                currentproject = currentproject,
                UseMultiple = true,
                isCategoryitem = true
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
                    CurrentQuest.QuestItems.Add(newrawrd);
                    CurrentQuest.isDirty = true;
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
            QuestObjectivesObjectiveTypeCB.DataSource = Enum.GetValues(typeof(QuestType));
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
            foreach (QuestObjectivesBase objs in QuestObjectives.Objectives)
            {
                switch (objs.QuestType)
                {
                    case QuestType.TARGET:
                        TreeNode newnode = new TreeNode(objs.Filename);
                        QuestObjectivesTarget newobjtarget = ReadJson<QuestObjectivesTarget>(QuestObjectivesPath + "\\Target\\" + objs.Filename + ".json") as QuestObjectivesTarget;
                        newobjtarget.Filename = objs.Filename;
                        newobjtarget.QuestType = QuestType.TARGET;
                        newnode.Tag = newobjtarget;
                        ObjectivesTarget.Nodes.Add(newnode);
                        break;
                    case QuestType.TRAVEL:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesTravel newobjtravel = ReadJson<QuestObjectivesTravel>(QuestObjectivesPath + "\\Travel\\" + objs.Filename + ".json") as QuestObjectivesTravel;
                        newobjtravel.Filename = objs.Filename;
                        newobjtravel.QuestType = QuestType.TRAVEL;
                        newnode.Tag = newobjtravel;
                        ObjectivesTravel.Nodes.Add(newnode);
                        break;
                    case QuestType.COLLECT:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesCollection newobjcollect = ReadJson<QuestObjectivesCollection>(QuestObjectivesPath + "\\Collection\\" + objs.Filename + ".json") as QuestObjectivesCollection;
                        newobjcollect.Filename = objs.Filename;
                        newobjcollect.QuestType = QuestType.COLLECT;
                        newnode.Tag = newobjcollect;
                        ObjectivesCollection.Nodes.Add(newnode);
                        break;
                    case QuestType.DELIVERY:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesDelivery newobjdelivery = ReadJson<QuestObjectivesDelivery>(QuestObjectivesPath + "\\Delivery\\" + objs.Filename + ".json") as QuestObjectivesDelivery;
                        newobjdelivery.Filename = objs.Filename;
                        newobjdelivery.QuestType = QuestType.DELIVERY;
                        newnode.Tag = newobjdelivery;
                        ObjectivesDelivery.Nodes.Add(newnode);
                        break;
                    case QuestType.TREASUREHUNT:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesTreasureHunt newobjtresurehunt = ReadJson<QuestObjectivesTreasureHunt>(QuestObjectivesPath + "\\TreasureHunt\\" + objs.Filename + ".json") as QuestObjectivesTreasureHunt;
                        newobjtresurehunt.TreasureHunt.ConvertDictToList();
                        newobjtresurehunt.Filename = objs.Filename;
                        newobjtresurehunt.QuestType = QuestType.TREASUREHUNT;
                        newnode.Tag = newobjtresurehunt;
                        ObjectivesTreasureHunt.Nodes.Add(newnode);
                        break;
                    case QuestType.AIPATROL:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesAIPatrol newobjaipatrol = ReadJson<QuestObjectivesAIPatrol>(QuestObjectivesPath + "\\AIPatrol\\" + objs.Filename + ".json") as QuestObjectivesAIPatrol;
                        newobjaipatrol.Filename = objs.Filename;
                        newobjaipatrol.QuestType = QuestType.AIPATROL;
                        newnode.Tag = newobjaipatrol;
                        ObjectivesAIPatrol.Nodes.Add(newnode);
                        break;
                    case QuestType.AICAMP:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesAICamp newobjaicamp = ReadJson<QuestObjectivesAICamp>(QuestObjectivesPath + "\\AICamp\\" + objs.Filename + ".json") as QuestObjectivesAICamp;
                        newobjaicamp.Filename = objs.Filename;
                        newobjaicamp.QuestType = QuestType.AICAMP;
                        newnode.Tag = newobjaicamp;
                        ObjectivesAICamp.Nodes.Add(newnode);
                        break;
                    case QuestType.AIVIP:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesAIVIP newobjaivip = ReadJson<QuestObjectivesAIVIP>(QuestObjectivesPath + "\\AIVIP\\" + objs.Filename + ".json") as QuestObjectivesAIVIP;
                        newobjaivip.Filename = objs.Filename;
                        newobjaivip.QuestType = QuestType.AIVIP;
                        newnode.Tag = newobjaivip;
                        ObjectivesAIVIP.Nodes.Add(newnode);
                        break;
                    case QuestType.ACTION:
                        newnode = new TreeNode(objs.Filename);
                        QuestObjectivesAction newobjaction = ReadJson<QuestObjectivesAction>(QuestObjectivesPath + "\\Action\\" + objs.Filename + ".json") as QuestObjectivesAction;
                        newobjaction.Filename = objs.Filename;
                        newobjaction.QuestType = QuestType.ACTION;
                        newnode.Tag = newobjaction;
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
            }
            else
            {
                CurrentTreeNodeTag = e.Node.Tag as QuestObjectivesBase;
                QuestObjectivesFilenameTB.Text = CurrentTreeNodeTag.Filename;
                QuestObjectivesConfigVersionNUD.Value = CurrentTreeNodeTag.ConfigVersion;
                QuestsObjectivesIDNUD.Value = CurrentTreeNodeTag.ID;
                QuestObjectivesObjectiveTypeCB.SelectedItem = (QuestType)CurrentTreeNodeTag.ObjectiveType;
                QuestObjectivesObjectiveTextTB.Text = CurrentTreeNodeTag.ObjectiveText;
                QuestObjectivesTimeLimitNUD.Value = CurrentTreeNodeTag.TimeLimit;
                foreach (GroupBox gpBox in flowLayoutPanel3.Controls.OfType<GroupBox>())
                {
                    if (gpBox.Name == "QuestObjectivesBaseInfoGB") continue;
                    gpBox.Visible = false;
                }
                foreach(Panel p in flowLayoutPanel4.Controls.OfType<Panel>())
                {
                    p.Visible = false;
                }
                switch (CurrentTreeNodeTag.QuestType)
                {
                    case QuestType.TARGET:
                        QuestObjectivesPositionGB.Visible = true;
                        QuestObjectivesMaxdistanceGB.Visible = true;
                        QuestObjectivesNPCInfoGB.Visible = true;
                        QuestObjectivesNPCInfoGB.Height = 50;
                        NPCUnitsP.Visible = true;
                        NPCAIClassnamesP.Visible = true;
                        QuestObjectivesSpecialWeaponGB.Visible = true;
                        setupObjectiveTarget(e);
                        break;
                    case QuestType.TRAVEL:
                        QuestObjectivesPositionGB.Visible = true;
                        QuestObjectivesMaxdistanceGB.Visible = true;
                        QuestObjectivesMarkerNameGB.Visible = true;
                        QuestObjectivesShowDistanceGB.Visible = true;
                        SetupObjectiveTravel(e);
                        break;
                    case QuestType.COLLECT:
                        QuestObjectivesColectionGB.Visible = true;
                        setupObjectiveCollection(e);
                        break;
                    case QuestType.DELIVERY:
                        QuestObjectivesPositionGB.Visible = true;
                        QuestObjectivesDeleveriesGB.Visible = true;
                        QuestObjectivesMaxdistanceGB.Visible = true;
                        QuestObjectivesMarkerNameGB.Visible = true;
                        setupobjectiveDelivery(e);
                        break;
                    case QuestType.TREASUREHUNT:
                        QuestObjectivesAIPositionsGB.Visible = true;
                        QuestObjectivesAIPositionsGB.Text = "Tresure Hunt Position";
                        QuestObjectivesShowDistanceGB.Visible = true;
                        QuestObjectivesTreasureHuntItemsGB.Visible = true;
                        SetupobjectiveTreasueHunt(e);
                        break;
                    case QuestType.AIPATROL:
                        NPCUnitsP.Visible = true;
                        NPCSpeedP.Visible = true;
                        NPCBehaviourP.Visible = true;
                        NPCFactionP.Visible = true;
                        NPCLoadoutP.Visible = true;
                        NPCAIClassnamesP.Visible = true;
                        QuestObjectivesNPCInfoGB.Height = 283;
                        QuestObjectivesAIPositionsGB.Visible = true;
                        QuestObjectivesAIPositionsGB.Text = "Patrol Waypoints";
                        QuestObjectivesNPCInfoGB.Visible = true;
                        QuestObjectivesSpecialWeaponGB.Visible = true;
                        QuestObjectivesAISpawnInfoGB.Visible = true;
                        SetupobjectiveAIPatrol(e);
                        break;
                    case QuestType.AICAMP:
                        NPCSpeedP.Visible = true;
                        NPCBehaviourP.Visible = true;
                        NPCFactionP.Visible = true;
                        NPCLoadoutP.Visible = true;
                        NPCAIClassnamesP.Visible = true;
                        QuestObjectivesNPCInfoGB.Height = 263;
                        QuestObjectivesAIPositionsGB.Visible = true;
                        QuestObjectivesAIPositionsGB.Text = "Camp Positions";
                        QuestObjectivesNPCInfoGB.Visible = true;
                        QuestObjectivesSpecialWeaponGB.Visible = true;
                        QuestObjectivesAISpawnInfoGB.Visible = true;
                        SetupObjectiveAICamp(e);
                        break;
                    case QuestType.AIVIP:
                        QuestObjectivesPositionGB.Visible = true;
                        QuestObjectivesMaxdistanceGB.Visible = true;
                        NPCClassNameP.Visible = true;
                        NPCSpeedP.Visible = true;
                        NPCBehaviourP.Visible = true;
                        NPCFactionP.Visible = true;
                        NPCLoadoutP.Visible = true;
                        QuestObjectivesNPCInfoGB.Height = 150;
                        QuestObjectivesNPCInfoGB.Visible = true;
                        QuestObjectivesMarkerNameGB.Visible = true;
                        SetupObjectiveAIVIP(e);
                        break;
                    case QuestType.ACTION:
                        QuestsObjectivesActionNamesGB.Visible = true;
                        SetupObjectiveAction(e);
                        break;
                    default:
                        break;
                }

            }
            useraction = true;
        }
        private void setupObjectiveTarget(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTarget CurrentTarget = e.Node.Tag as QuestObjectivesTarget;
            QuestObjectivesPosXNUD.Value = (decimal)CurrentTarget.Position[0];
            QuestObjectivesPosYNUD.Value = (decimal)CurrentTarget.Position[1];
            QuestObjectivesPosZNUD.Value = (decimal)CurrentTarget.Position[2];
            QuestObjectivesMaxDistanceNUD.Value = CurrentTarget.MaxDistance;
            QuestObjectivesNPCUnitsNUD.Value = CurrentTarget.Target.Amount;
            QuestObjectivesNPCAICLassNamesLB.DisplayMember = "DisplayName";
            QuestObjectivesNPCAICLassNamesLB.ValueMember = "Value";
            QuestObjectivesNPCAICLassNamesLB.DataSource = CurrentTarget.Target.ClassNames;
            QuestObjectivesSpecialWeaponCB.Checked = CurrentTarget.Target.SpecialWeapon == 1 ? true : false;
            QuestObjectivesAllowedWeaponsLB.DisplayMember = "DisplayName";
            QuestObjectivesAllowedWeaponsLB.ValueMember = "Value";
            QuestObjectivesAllowedWeaponsLB.DataSource = CurrentTarget.Target.AllowedWeapons;

        }
        private void SetupObjectiveTravel(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTravel CurrentTravel = e.Node.Tag as QuestObjectivesTravel;
            QuestObjectivesPosXNUD.Value = (decimal)CurrentTravel.Position[0];
            QuestObjectivesPosYNUD.Value = (decimal)CurrentTravel.Position[1];
            QuestObjectivesPosZNUD.Value = (decimal)CurrentTravel.Position[2];
            QuestObjectivesMaxDistanceNUD.Value = CurrentTravel.MaxDistance;
            QuestObjectivesMarkerNameTB.Text = CurrentTravel.MarkerName;
            QuestObjectivesShowDistanceCB.Checked = CurrentTravel.ShowDistance == 1 ? true : false;
        }
        private void setupObjectiveCollection(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesCollection CurrentCollection = e.Node.Tag as QuestObjectivesCollection;
            QuestObjectivesCollectionAmountNUD.Value = CurrentCollection.Collection.Amount;
            QuestObjectivesCollectionClassnameTB.Text = CurrentCollection.Collection.ClassName;

        }
        private void setupobjectiveDelivery(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesDelivery CurrentDelivery = e.Node.Tag as QuestObjectivesDelivery;
            QuestObjectivesDeleveriesLB.DisplayMember = "DisplayName";
            QuestObjectivesDeleveriesLB.ValueMember = "Value";
            QuestObjectivesDeleveriesLB.DataSource = CurrentDelivery.Deliveries;
            QuestObjectivesPosXNUD.Value = (decimal)CurrentDelivery.Position[0];
            QuestObjectivesPosYNUD.Value = (decimal)CurrentDelivery.Position[1];
            QuestObjectivesPosZNUD.Value = (decimal)CurrentDelivery.Position[2];
            QuestObjectivesMaxDistanceNUD.Value = CurrentDelivery.MaxDistance;
            QuestObjectivesMarkerNameTB.Text = CurrentDelivery.MarkerName;
        }
        private void QuestObjectivesDeleveriesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Delivery del = QuestObjectivesDeleveriesLB.SelectedItem as Delivery;
            QuestObjectivesDeliveriesAmountNUD.Value = del.Amount;
            QuestObjectivesDeliveriesClassnameTB.Text = del.ClassName;
        }
        private void SetupobjectiveTreasueHunt(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesTreasureHunt CurrentTreasureHunt = e.Node.Tag as QuestObjectivesTreasureHunt;
            QuestObjectivesShowDistanceCB.Checked = CurrentTreasureHunt.ShowDistance == 1 ? true : false;
            questObjectivesPositionsLB.DisplayMember = "DisplayName";
            questObjectivesPositionsLB.ValueMember = "Value";
            questObjectivesPositionsLB.DataSource = CurrentTreasureHunt.TreasureHunt.Positions;
            QuestObjectivesTreasureHUntItemListLB.DisplayMember = "DisplayName";
            QuestObjectivesTreasureHUntItemListLB.ValueMember = "Value";
            QuestObjectivesTreasureHUntItemListLB.DataSource = CurrentTreasureHunt.TreasureHunt.ListItems;
        }
        private void QuestObjectivesTreasureHUntItemListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestObjectivesTreasureHUntItemListLB.SelectedItems.Count == 0) return;
            TreasureHuntItems thitems = QuestObjectivesTreasureHUntItemListLB.SelectedItem as TreasureHuntItems;
            QuestObjectivesTreasureHuntitemClassnameTB.Text = thitems.ClassName;
            QuestObjectiveTreasureHuntitemAmountNUD.Value = thitems.Amount;
        }
        private void SetupobjectiveAIPatrol(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAIPatrol CurrentAIPatrol = e.Node.Tag as QuestObjectivesAIPatrol;
            QuestObjectivesNPCUnitsNUD.Value = CurrentAIPatrol.AIPatrol.NPCUnits;
            questObjectivesPositionsLB.DisplayMember = "DisplayName";
            questObjectivesPositionsLB.ValueMember = "Value";
            questObjectivesPositionsLB.DataSource = CurrentAIPatrol.AIPatrol.Waypoints;
            QuestObjectivesNPCSpeedCB.SelectedIndex = QuestObjectivesNPCSpeedCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCSpeed);
            QuestObjectivesNPCModeCB.SelectedIndex = QuestObjectivesNPCModeCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCMode);
            QuestObjectivesNPCFactionCB.SelectedIndex = QuestObjectivesNPCFactionCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCFaction);
            QuestObjectivesNPCLoadoutsCB.SelectedIndex = QuestObjectivesNPCLoadoutsCB.FindStringExact(CurrentAIPatrol.AIPatrol.NPCLoadoutFile);
            QuestObjectivesNPCAICLassNamesLB.DisplayMember = "DisplayName";
            QuestObjectivesNPCAICLassNamesLB.ValueMember = "Value";
            QuestObjectivesNPCAICLassNamesLB.DataSource = CurrentAIPatrol.AIPatrol.ClassNames;
            QuestObjectivesSpecialWeaponCB.Checked = CurrentAIPatrol.AIPatrol.SpecialWeapon == 1 ? true : false;
            QuestObjectivesAllowedWeaponsLB.DisplayMember = "DisplayName";
            QuestObjectivesAllowedWeaponsLB.ValueMember = "Value";
            QuestObjectivesAllowedWeaponsLB.DataSource = CurrentAIPatrol.AIPatrol.AllowedWeapons;
            QuestObjectivesMinDistRadiusNUD.Value = CurrentAIPatrol.MinDistRadius;
            QuestObjectivesMaxDistRadiusNUD.Value = CurrentAIPatrol.MaxDistRadius;
            QuestObjectivesDespawnRadiusNUD.Value = CurrentAIPatrol.DespawnRadius;
        }
        private void SetupObjectiveAICamp(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAICamp CurrentAICamp = e.Node.Tag as QuestObjectivesAICamp;
            questObjectivesPositionsLB.DisplayMember = "DisplayName";
            questObjectivesPositionsLB.ValueMember = "Value";
            questObjectivesPositionsLB.DataSource = CurrentAICamp.AICamp.Positions;
            QuestObjectivesNPCModeCB.SelectedIndex = QuestObjectivesNPCModeCB.FindStringExact(CurrentAICamp.AICamp.NPCMode);
            QuestObjectivesNPCLoadoutsCB.SelectedIndex = QuestObjectivesNPCLoadoutsCB.FindStringExact(CurrentAICamp.AICamp.NPCLoadoutFile);
            QuestObjectivesNPCSpeedCB.SelectedIndex = QuestObjectivesNPCSpeedCB.FindStringExact(CurrentAICamp.AICamp.NPCSpeed);
            QuestObjectivesNPCFactionCB.SelectedIndex = QuestObjectivesNPCFactionCB.FindStringExact(CurrentAICamp.AICamp.NPCFaction);
            QuestObjectivesNPCAICLassNamesLB.DisplayMember = "DisplayName";
            QuestObjectivesNPCAICLassNamesLB.ValueMember = "Value";
            QuestObjectivesNPCAICLassNamesLB.DataSource = CurrentAICamp.AICamp.ClassNames;
            QuestObjectivesSpecialWeaponCB.Checked = CurrentAICamp.AICamp.SpecialWeapon == 1 ? true: false;
            QuestObjectivesAllowedWeaponsLB.DisplayMember = "DisplayName";
            QuestObjectivesAllowedWeaponsLB.ValueMember = "Value";
            QuestObjectivesAllowedWeaponsLB.DataSource = CurrentAICamp.AICamp.AllowedWeapons;
            QuestObjectivesMinDistRadiusNUD.Value = CurrentAICamp.MinDistRadius;
            QuestObjectivesMaxDistRadiusNUD.Value = CurrentAICamp.MaxDistRadius;
            QuestObjectivesDespawnRadiusNUD.Value = CurrentAICamp.DespawnRadius;
        }
        private void SetupObjectiveAIVIP(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAIVIP CurrentAIVIP = e.Node.Tag as QuestObjectivesAIVIP;
            QuestObjectivesPosXNUD.Value = (decimal)CurrentAIVIP.Position[0];
            QuestObjectivesPosYNUD.Value = (decimal)CurrentAIVIP.Position[1];
            QuestObjectivesPosZNUD.Value = (decimal)CurrentAIVIP.Position[2];
            QuestObjectivesMaxDistanceNUD.Value = CurrentAIVIP.MaxDistance;
            QuestObjectivesNPCClassnameTB.Text = CurrentAIVIP.AIVIP.NPCClassName;
            QuestObjectivesNPCSpeedCB.SelectedIndex = QuestObjectivesNPCSpeedCB.FindStringExact(CurrentAIVIP.AIVIP.NPCSpeed);
            QuestObjectivesNPCModeCB.SelectedIndex = QuestObjectivesNPCModeCB.FindStringExact(CurrentAIVIP.AIVIP.NPCMode);
            QuestObjectivesNPCFactionCB.SelectedIndex = QuestObjectivesNPCFactionCB.FindStringExact(CurrentAIVIP.AIVIP.NPCFaction);
            QuestObjectivesNPCLoadoutsCB.SelectedIndex = QuestObjectivesNPCLoadoutsCB.FindStringExact(CurrentAIVIP.AIVIP.NPCLoadoutFile);
            QuestObjectivesMarkerNameTB.Text = CurrentAIVIP.MarkerName;
        }
        private void SetupObjectiveAction(TreeNodeMouseClickEventArgs e)
        {
            QuestObjectivesAction CurrentAction = e.Node.Tag as QuestObjectivesAction;
            QuestObjectivesActionNamesLB.DisplayMember = "DisplayName";
            QuestObjectivesActionNamesLB.ValueMember = "Value";
            QuestObjectivesActionNamesLB.DataSource = CurrentAction.ActionNames;
        }
        private void QuestObjectivesSpecialWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            QuestObjectivesAllowedWeaponsGB.Visible = QuestObjectivesSpecialWeaponCB.Checked;
        }
        #endregion objectives
    }
}
