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

    public partial class ExpansionAI : DarkForm
    {
        public Project currentproject { get; internal set; }

        public ExpansionAISettings AISettings { get; set; }
        public ExpansionAIPatrolSettings AIPatrolSettings { get; set; }
        public BindingList<AILoadouts> LoadoutList { get; private set; }
        public BindingList<string> LoadoutNameList1 { get; private set; }
        public BindingList<string> LoadoutNameList2 { get; private set; }
        public BindingList<string> Factions { get; set; }

        public string AISettingsPath;
        public string AILoadoutsPath;
        public string AIPatrolSettingsPath;

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        private bool useraction;

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
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
        public ExpansionAI()
        {
            InitializeComponent();
        }
        private void ExpansionAI_Load(object sender, EventArgs e)
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
            SetupLoadoutList();

            AISettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings\\AISettings.json";
            if (!File.Exists(AISettingsPath))
            {
                AISettings = new ExpansionAISettings();
                AISettings.isDirty = true;
                needtosave = true;
                Console.WriteLine(Path.GetFileName(AISettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(AISettingsPath));
                AISettings = JsonSerializer.Deserialize<ExpansionAISettings>(File.ReadAllText(AISettingsPath));
                AISettings.isDirty = false;
                if (AISettings.checkver())
                    needtosave = true;
            }
            AISettings.Filename = AISettingsPath;
            SetupAISettings();



            AIPatrolSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\AIPatrolSettings.json";
            if (!File.Exists(AIPatrolSettingsPath))
            {
                AIPatrolSettings = new ExpansionAIPatrolSettings();
                AIPatrolSettings.isDirty = true;
                AIPatrolSettings.SetPatrolNames();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(AIPatrolSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(AIPatrolSettingsPath));
                AIPatrolSettings = JsonSerializer.Deserialize<ExpansionAIPatrolSettings>(File.ReadAllText(AIPatrolSettingsPath));
                AIPatrolSettings.isDirty = false;
                if (AIPatrolSettings.checkver())
                    needtosave = true;
                if (AIPatrolSettings.SetPatrolNames())
                    needtosave = true;
            }
            
            AIPatrolSettings.Filename = AIPatrolSettingsPath;
            SetupAIPatrolSettings();

            tabControl1.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
            }


        }
        private void ExpansionAI_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (AISettings.isDirty)
            {
                needtosave = true;
            }
            if (AIPatrolSettings.isDirty)
            {
                needtosave = true;
            }

            foreach (AILoadouts AILO in LoadoutList)
            {
                if (AILO.isDirty)
                {
                    needtosave = true;
                }
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    savefiles();
                }
            }
        }
        private void SetupFactionsDropDownBoxes()
        {
            useraction = false;
            PlayerFactionCB.DataSource = new BindingList<string>(Factions);
            CrashFactionCB.DataSource = new BindingList<string>(Factions);
            StaticPatrolFactionCB.DataSource = new BindingList<string>(Factions);
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
            if (AISettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AISettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AISettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AISettings.Filename, Path.GetDirectoryName(AISettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AISettings.Filename) + ".bak", true);
                }
                AISettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AISettings, options);
                File.WriteAllText(AISettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AISettings.Filename));
            }
            if (AIPatrolSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AIPatrolSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AIPatrolSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AIPatrolSettings.Filename, Path.GetDirectoryName(AIPatrolSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AIPatrolSettings.Filename) + ".bak", true);
                }
                AIPatrolSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AIPatrolSettings, options);
                File.WriteAllText(AIPatrolSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AIPatrolSettings.Filename));
            }

            foreach(AILoadouts AILO in LoadoutList)
            {
                if (AILO.isDirty)
                {
                    if (currentproject.Createbackups && File.Exists(AILO.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(AILO.Filename, Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak", true);
                    }
                    AILO.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(AILO, options);
                    File.WriteAllText(AILO.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(AILO.Filename));
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
        }
        private void SetupLoadoutList()
        {
            useraction = false;
            LoadoutNameList1 = new BindingList<string>();
            foreach(AILoadouts lo in LoadoutList)
            {
                LoadoutNameList1.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }
            CrashLoadoutFileCB.DisplayMember = "DisplayName";
            CrashLoadoutFileCB.ValueMember = "Value";
            CrashLoadoutFileCB.DataSource = LoadoutNameList1;
            StaticPatrolLB.Refresh();

            LoadoutNameList2 = new BindingList<string>();
            foreach (AILoadouts lo in LoadoutList)
            {
                LoadoutNameList2.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }

            StaticPatrolLoadoutsCB.DisplayMember = "DisplayName";
            StaticPatrolLoadoutsCB.ValueMember = "Value";
            StaticPatrolLoadoutsCB.DataSource = LoadoutNameList2;
            EventCrachPatrolLB.Refresh();
            useraction = true;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton1.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton8.Checked = true;
                    break;
                case 1:
                    toolStripButton3.Checked = true;
                    break;
                case 2:
                    toolStripButton1.Checked = true;
                    break;

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings");
                    break;
                case 1:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
                case 2:
                    Process.Start(AILoadoutsPath);
                    break;
            }
        }

        #region aipatrolsettings
        public ExpansionAIObjectPatrol CurrentEventcrashpatrol;
        public ExpansionAIPatrol CurrentPatrol;
        public float[] CurrentWapypoint;
        private void SetupAIPatrolSettings()
        {
            useraction = false;

            AIGeneralEnabledCB.Checked = AIPatrolSettings.Enabled == 1 ? true : false;
            AIGeneralDespawnTimeNUD.Value = AIPatrolSettings.DespawnTime;
            AIGeneralRespawnTimeNUD.Value = AIPatrolSettings.RespawnTime;
            AIGeneralMinDistRadiusNUD.Value = AIPatrolSettings.MinDistRadius;
            AIGeneralMaxDistRadiusNUD.Value = AIPatrolSettings.MaxDistRadius;
            AIGeneralDespawnRadiusNUD.Value = AIPatrolSettings.DespawnRadius;
            AIGeneralAccuracyMinNUD.Value = AIPatrolSettings.AccuracyMin;
            AIGeneralAccuracyMaxNUD.Value = AIPatrolSettings.AccuracyMax;
            AIGeneralThreatDistanceLimitNUD.Value = AIPatrolSettings.ThreatDistanceLimit;
            AINoiseInvestigationDistanceLimitNUD.Value = AIPatrolSettings.NoiseInvestigationDistanceLimit;
            AIGeneralDanageMultiplierNUD.Value = AIPatrolSettings.DamageMultiplier;


            EventCrachPatrolLB.DisplayMember = "DisplayName";
            EventCrachPatrolLB.ValueMember = "Value";
            EventCrachPatrolLB.DataSource = AIPatrolSettings.ObjectPatrols;

            SuspendLayout();

            StaticPatrolLB.DisplayMember = "DisplayName";
            StaticPatrolLB.ValueMember = "Value";
            StaticPatrolLB.DataSource = AIPatrolSettings.Patrols;

            ResumeLayout();
            useraction = true;
        }
        private void AIGeneralDamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.DamageReceivedMultiplier = AIGeneralDamageReceivedMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.Enabled = AIGeneralEnabledCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralRespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.RespawnTime = AIGeneralRespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.MinDistRadius = AIGeneralMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.MaxDistRadius = AIGeneralMaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.DespawnTime = AIGeneralDespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.AccuracyMin = AIGeneralAccuracyMinNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGenralAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.AccuracyMax = AIGeneralAccuracyMaxNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.DespawnRadius = AIGeneralDespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.ThreatDistanceLimit = AIGeneralThreatDistanceLimitNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void AIGeneralDanageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.DamageMultiplier = AIGeneralDanageMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void NoiseInvestigationDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.NoiseInvestigationDistanceLimit = AINoiseInvestigationDistanceLimitNUD.Value;
            AIPatrolSettings.isDirty = true;
        }

        /// <summary>
        /// Event Crash Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventCrachPatrolLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventCrachPatrolLB.SelectedItems.Count < 1) return;
            CurrentEventcrashpatrol = EventCrachPatrolLB.SelectedItem as ExpansionAIObjectPatrol;
            useraction = false;
            CrashEventNameTB.Text = CurrentEventcrashpatrol.Name;
            CrashEventClassNameTB.Text = CurrentEventcrashpatrol.ClassName;
            CrashFactionCB.SelectedIndex = CrashFactionCB.FindStringExact(CurrentEventcrashpatrol.Faction);
            CrashNumberOfAINUD.Value = CurrentEventcrashpatrol.NumberOfAI;
            CrashBehaviourCB.SelectedIndex = CrashBehaviourCB.FindStringExact(CurrentEventcrashpatrol.Behaviour);
            CrashSpeedCB.SelectedIndex = CrashSpeedCB.FindStringExact(CurrentEventcrashpatrol.Speed);
            CrashUnderThreatSpeedCB.SelectedIndex = CrashUnderThreatSpeedCB.FindStringExact(CurrentEventcrashpatrol.UnderThreatSpeed);
            CrashAccuracyMinNud.Value = CurrentEventcrashpatrol.AccuracyMin;
            CrashAccuracyMaxNUD.Value = CurrentEventcrashpatrol.AccuracyMax;
            CrashThreatDistanceLimitNUD.Value = CurrentEventcrashpatrol.ThreatDistanceLimit;
            CrashDamageMultiplierNUD.Value = CurrentEventcrashpatrol.DamageMultiplier;
            CrashSniperProneDistanceThresholdNUD.Value = CurrentEventcrashpatrol.SniperProneDistanceThreshold;
            CrashMinDistRadiusNUD.Value = CurrentEventcrashpatrol.MinDistRadius;
            CrashMaxDistRadiusNUD.Value = CurrentEventcrashpatrol.MaxDistRadius;
            CrashDespawnRadiusNUD.Value = CurrentEventcrashpatrol.DespawnRadius;
            CrashDespawnTimeNUD.Value = CurrentEventcrashpatrol.DespawnTime;
            CrashMinSpreadRadiusNUD.Value = CurrentEventcrashpatrol.MinSpreadRadius;
            CrashMaxSpreadRadiusNUD.Value = CurrentEventcrashpatrol.MaxSpreadRadius;
            CrashRespawnTimeNUD.Value = CurrentEventcrashpatrol.RespawnTime;
            CrashChanceNUD.Value = CurrentEventcrashpatrol.Chance;
            CrashDamageReceivedMultiplierNUD.Value = CurrentEventcrashpatrol.DamageReceivedMultiplier;
            CrashCanBeLootedCB.Checked = CurrentEventcrashpatrol.CanBeLooted == 1 ? true : false;
            CrashUnlimitedReloadCB.Checked = CurrentEventcrashpatrol.UnlimitedReload == 1 ? true : false;
            CrashLoadoutFileCB.SelectedIndex = CrashLoadoutFileCB.FindStringExact(CurrentEventcrashpatrol.LoadoutFile);
            CrashFormationCB.SelectedIndex = CrashFormationCB.FindStringExact(CurrentEventcrashpatrol.Formation);
            CrashWaypointInterpolationCB.SelectedIndex = CrashWaypointInterpolationCB.FindStringExact(CurrentEventcrashpatrol.WaypointInterpolation);
            crashFormationLoosenessNUD.Value = CurrentEventcrashpatrol.FormationLooseness;
            useraction = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            ExpansionAIObjectPatrol newpatrol = new ExpansionAIObjectPatrol()
            {
                Name = "NewName",
                Faction = "WEST",
                LoadoutFile = "",
                NumberOfAI = 5,
                Behaviour = "PATROL",
                Speed = "WALK",
                UnderThreatSpeed = "SPRINT",
                CanBeLooted = 1,
                UnlimitedReload = 0,
                AccuracyMin = (decimal)-1.0,
                AccuracyMax = (decimal)-1.0,
                ThreatDistanceLimit = (decimal)-1.0,
                DamageMultiplier = (decimal)-1.0,
                MinDistRadius = (decimal)-2.0,
                MaxDistRadius = (decimal)-2.0,
                DespawnRadius = (decimal)-2.0,
                MinSpreadRadius = (decimal)-2.0,
                MaxSpreadRadius = (decimal)-2.0,
                Chance = (decimal)1.0,
                WaypointInterpolation = "",
                DespawnTime = (decimal)-1.0,
                RespawnTime = (decimal)-2.0,
                ClassName = "Your Classname Goes Here"
            };
            
            AIPatrolSettings.ObjectPatrols.Add(newpatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AIPatrolSettings.ObjectPatrols.Remove(CurrentEventcrashpatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
        }
        private void CrashEventNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Name = CrashEventNameTB.Text;
            AIPatrolSettings.isDirty = true;
            EventCrachPatrolLB.Refresh();
        }
        private void CrashEventClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.ClassName = CrashEventClassNameTB.Text;
            AIPatrolSettings.isDirty = true;
            EventCrachPatrolLB.Refresh();
        }
        private void CrashFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Faction = CrashFactionCB.GetItemText(CrashFactionCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.LoadoutFile = CrashLoadoutFileCB.GetItemText(CrashLoadoutFileCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.NumberOfAI = (int)CrashNumberOfAINUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashBehaviourCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Behaviour = CrashBehaviourCB.GetItemText(CrashBehaviourCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Speed = CrashSpeedCB.GetItemText(CrashSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashUnderThreatSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.UnderThreatSpeed = CrashUnderThreatSpeedCB.GetItemText(CrashUnderThreatSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MinDistRadius = CrashMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MaxDistRadius = CrashMaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.DespawnRadius = CrashDespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.DespawnTime = CrashDespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.AccuracyMin = CrashAccuracyMinNud.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.AccuracyMax = CrashAccuracyMaxNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashDamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.DamageMultiplier = CrashDamageMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashSniperProneDistanceThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.SniperProneDistanceThreshold = CrashSniperProneDistanceThresholdNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.ThreatDistanceLimit = CrashThreatDistanceLimitNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMinSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MinSpreadRadius = CrashMinSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMaxSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MaxSpreadRadius = CrashMaxSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Chance = CrashChanceNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashUnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.UnlimitedReload = CrashUnlimitedReloadCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashCanBeLootedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.CanBeLooted = CrashCanBeLootedCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Formation = CrashFormationCB.GetItemText(CrashFormationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashWaypointInterpolationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.WaypointInterpolation = CrashWaypointInterpolationCB.GetItemText(CrashWaypointInterpolationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void numericUpDown6_ValueChanged_1(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.RespawnTime = CrashRespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void crashFormationLoosenessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.FormationLooseness = crashFormationLoosenessNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashDamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.DamageReceivedMultiplier = CrashDamageReceivedMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        /// <summary>
        /// Stataic Patrol Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StaticPatrolLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolLB.SelectedItems.Count < 1) return;
            CurrentPatrol = StaticPatrolLB.SelectedItem as ExpansionAIPatrol;
            useraction = false;
            StaticPatrolNameTB.Text = CurrentPatrol.Name;
            StaticPatrolFactionCB.SelectedIndex = StaticPatrolFactionCB.FindStringExact(CurrentPatrol.Faction);
            StaticPatrolNumberOfAINUD.Value = CurrentPatrol.NumberOfAI;
            StaticPatrolBehaviorCB.SelectedIndex = StaticPatrolBehaviorCB.FindStringExact(CurrentPatrol.Behaviour);
            StaticPatrolSpeedCB.SelectedIndex = StaticPatrolSpeedCB.FindStringExact(CurrentPatrol.Speed);
            StaticPatrolUnderThreatSpeedCB.SelectedIndex = StaticPatrolUnderThreatSpeedCB.FindStringExact(CurrentPatrol.UnderThreatSpeed);
            StaticPatrolRespawnTimeNUD.Value = CurrentPatrol.RespawnTime;
            StaticPatrolDespawnTimeNUD.Value = CurrentPatrol.DespawnTime;
            StaticPatrolMinDistRadiusNUD.Value = CurrentPatrol.MinDistRadius;
            StaticPatrolMaxDistRadiusNUD.Value = CurrentPatrol.MaxDistRadius;
            StaticPatrolDespawnRadiusNUD.Value = CurrentPatrol.DespawnRadius;
            StaticPatrolAccuracyMinNUD.Value = CurrentPatrol.AccuracyMin;
            StaticPatrolAccuracyMaxNUD.Value = CurrentPatrol.AccuracyMax;
            StaticPatrolDamageReceivedMultiplierNUD.Value = CurrentPatrol.DamageReceivedMultiplier;
            StaticPatrolThreatDistanceLimitNUD.Value = CurrentPatrol.ThreatDistanceLimit;
            StaticPatrolSniperProneDistanceThresholdNUD.Value = CurrentPatrol.SniperProneDistanceThreshold;
            StaticPatrolDamageMultiplierNUD.Value = CurrentPatrol.DamageMultiplier;
            StaticPatrolChanceCB.Value = CurrentPatrol.Chance;
            StaticPatrolCanBeLotedCB.Checked = CurrentPatrol.CanBeLooted == 1 ? true : false;
            StaticPatrolUnlimitedReloadCB.Checked = CurrentPatrol.UnlimitedReload == 1 ? true : false;
            StaticPatrolLoadoutsCB.SelectedIndex = StaticPatrolLoadoutsCB.FindStringExact(CurrentPatrol.LoadoutFile);
            StaticPatrolMinSpreadRadiusNUD.Value = CurrentPatrol.MinSpreadRadius;
            StaticPatrolMaxSpreadRadiusNUD.Value = CurrentPatrol.MaxSpreadRadius;
            StaticPatrolFormationCB.SelectedIndex = StaticPatrolFormationCB.FindStringExact(CurrentPatrol.Formation);
            StaticPatrolFormationLoosenessNUD.Value = CurrentPatrol.FormationLooseness;
            StaticPatrolWaypointInterpolationCB.SelectedIndex = StaticPatrolWaypointInterpolationCB.FindStringExact(CurrentPatrol.WaypointInterpolation);
            StaticPatrolUseRandomWaypointAsStartPointCB.Checked = CurrentPatrol.UseRandomWaypointAsStartPoint == 1 ? true : false;

            StaticPatrolWayPointsLB.DisplayMember = "DisplayName";
            StaticPatrolWayPointsLB.ValueMember = "Value";
            StaticPatrolWayPointsLB.DataSource = CurrentPatrol.Waypoints;

            useraction = true;
        }
        private void StaticPatrolWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = StaticPatrolWayPointsLB.SelectedItem as float[];
            useraction = false;
            StaticPatrolWaypointPOSXNUD.Value = (decimal)CurrentWapypoint[0];
            StaticPatrolWaypointPOSYNUD.Value = (decimal)CurrentWapypoint[1];
            StaticPatrolWaypointPOSZNUD.Value = (decimal)CurrentWapypoint[2];
            useraction = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
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
                        CurrentPatrol.Waypoints.Clear();
                     }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        float[] newfloatarray = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                        CurrentPatrol.Waypoints.Add(newfloatarray);

                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
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
                        CurrentPatrol.Waypoints.Clear();
                    }
                    foreach(Editorobject eo in importfile.EditorObjects)
                    {
                        float[] newfloatarray = new float[] { Convert.ToSingle(eo.Position[0]), Convert.ToSingle(eo.Position[1]), Convert.ToSingle(eo.Position[2]) };
                        CurrentPatrol.Waypoints.Add(newfloatarray);
                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                    AIPatrolSettings.isDirty = true;
                }
            }
        }
        private void StaticPatrolNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Name = StaticPatrolNameTB.Text;
            StaticPatrolLB.Refresh();
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Faction = StaticPatrolFactionCB.GetItemText(StaticPatrolFactionCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolLoadoutsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.LoadoutFile = StaticPatrolLoadoutsCB.GetItemText(StaticPatrolLoadoutsCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.NumberOfAI = (int)StaticPatrolNumberOfAINUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolFormationLoosenessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.FormationLooseness = (int)StaticPatrolFormationLoosenessNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolBehaviorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Behaviour = StaticPatrolBehaviorCB.GetItemText(StaticPatrolBehaviorCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Speed = StaticPatrolSpeedCB.GetItemText(StaticPatrolSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolUnderThreatSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.UnderThreatSpeed = StaticPatrolUnderThreatSpeedCB.GetItemText(StaticPatrolUnderThreatSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolRespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.RespawnTime = StaticPatrolRespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.DespawnTime = StaticPatrolDespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MinDistRadius = StaticPatrolMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolDamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.DamageReceivedMultiplier = StaticPatrolDamageReceivedMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MaxDistRadius = StaticPatrolMaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolSniperProneDistanceThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.SniperProneDistanceThreshold = StaticPatrolSniperProneDistanceThresholdNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.DespawnRadius = StaticPatrolDespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolChanceCB_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Chance = StaticPatrolChanceCB.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.AccuracyMin = StaticPatrolAccuracyMinNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.AccuracyMax = StaticPatrolAccuracyMaxNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolDamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.DamageMultiplier = StaticPatrolDamageMultiplierNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.ThreatDistanceLimit = StaticPatrolThreatDistanceLimitNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMinSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MinSpreadRadius = (int)StaticPatrolMinSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMaxSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MaxSpreadRadius = (int)StaticPatrolMaxSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolUnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.UnlimitedReload = StaticPatrolUnlimitedReloadCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolCanBeLotedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.CanBeLooted = StaticPatrolCanBeLotedCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolUseRandomWaypointAsStartPointCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.UseRandomWaypointAsStartPoint = StaticPatrolUseRandomWaypointAsStartPointCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[0] = (float)StaticPatrolWaypointPOSXNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[1] = (float)StaticPatrolWaypointPOSYNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            CurrentWapypoint[2] = (float)StaticPatrolWaypointPOSZNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Formation = StaticPatrolFormationCB.GetItemText(StaticPatrolFormationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointInterpolationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.WaypointInterpolation = StaticPatrolWaypointInterpolationCB.GetItemText(StaticPatrolWaypointInterpolationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            ExpansionAIPatrol newpatrol = new ExpansionAIPatrol()
            {
                Name = "NewPatrol",
                Faction = "WEST",
                FormationLooseness = (decimal)0.0,
                LoadoutFile = "",
                NumberOfAI = 5,
                Behaviour = "PATROL",
                Speed = "WALK",
                UnderThreatSpeed = "SPRINT",
                CanBeLooted = 1,
                UnlimitedReload = 1,
                AccuracyMin = (decimal)-2.0,
                AccuracyMax = (decimal)-2.0,
                ThreatDistanceLimit = 800,
                MinDistRadius = (decimal)-2.0,
                MaxDistRadius = (decimal)-2.0,
                DespawnRadius = (decimal) - 2.0,
                MinSpreadRadius = (decimal)5.0,
                MaxSpreadRadius = (decimal)20.0,
                Chance = (decimal)1.0,
                WaypointInterpolation = "NaturalCubic",
                DespawnTime = (decimal)-1.0,
                RespawnTime = (decimal)-2.0,
                UseRandomWaypointAsStartPoint = 0,
                Waypoints = new BindingList<float[]>()
            };
            AIPatrolSettings.Patrols.Add(newpatrol);
            AIPatrolSettings.isDirty = true;
            StaticPatrolLB.Refresh();
            StaticPatrolWayPointsLB.Refresh();
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            AIPatrolSettings.Patrols.Remove(CurrentPatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
            StaticPatrolLB.Refresh();
            StaticPatrolWayPointsLB.Refresh();
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            foreach (float[] array in CurrentPatrol.Waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }

        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in CurrentPatrol.Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = array,
                    Orientation = new float[] {0,0,0},
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
        #endregion aipatrolsettings
        #region AISettings
        private void SetupAISettings()
        {
            useraction = false;
            AccuracyMinNUD.Value = AISettings.AccuracyMin;
            AccuracyMaxNUD.Value = AISettings.AccuracyMax;
            ThreatDistanceLimitNUD.Value = AISettings.ThreatDistanceLimit;
            NoiseInvestigationDistanceLimitNUD.Value = AISettings.NoiseInvestigationDistanceLimit;
            DamageMultiplierNUD.Value = AISettings.DamageMultiplier;
            MaximumDynamicPatrolsNUD.Value = AISettings.MaximumDynamicPatrols;
            SniperProneDistanceThresholdNUD.Value = AISettings.SniperProneDistanceThreshold;
            DamageReceivedMultiplierNUD.Value = AISettings.DamageReceivedMultiplier;
            VaultingCB.Checked = AISettings.Vaulting == 1 ? true : false;
            MannersCB.Checked = AISettings.Manners == 1 ? true : false;
            CanRecruitGuardsCB.Checked = AISettings.CanRecruitGuards == 1 ? true : false;
            CanRecruitFriendlyCB.Checked = AISettings.CanRecruitFriendly == 1 ? true : false;
            AISettingsAdminsLB.DisplayMember = "DisplayName";
            AISettingsAdminsLB.ValueMember = "Value";
            AISettingsAdminsLB.DataSource = AISettings.Admins;

            PlayerFactionsLB.DisplayMember = "DisplayName";
            PlayerFactionsLB.ValueMember = "Value";
            PlayerFactionsLB.DataSource = AISettings.PlayerFactions;

            PreventClimbLB.DisplayMember = "DisplayName";
            PreventClimbLB.ValueMember = "Value";
            PreventClimbLB.DataSource = AISettings.PreventClimb;

            useraction = true;
        }
        private void AccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.AccuracyMin = AccuracyMinNUD.Value;
            AISettings.isDirty = true;
        }
        private void AccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.AccuracyMax = AccuracyMaxNUD.Value;
            AISettings.isDirty = true;
        }
        private void ThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.ThreatDistanceLimit = ThreatDistanceLimitNUD.Value;
            AISettings.isDirty = true;
        }
        private void DamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.DamageMultiplier = DamageMultiplierNUD.Value;
            AISettings.isDirty = true;
        }
        private void MaximumDynamicPatrolsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.MaximumDynamicPatrols = (int)MaximumDynamicPatrolsNUD.Value;
            AISettings.isDirty = true;
        }
        private void DamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.DamageReceivedMultiplier = DamageReceivedMultiplierNUD.Value;
            AISettings.isDirty = true;
        }
        private void MannersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.Manners = MannersCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void VaultingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.Vaulting = VaultingCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void CanRecruitGuardsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.CanRecruitGuards = CanRecruitGuardsCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;

        }
        private void CanRecruitFriendlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.CanRecruitFriendly = CanRecruitFriendlyCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    AISettings.Admins.Add(l.ToLower());
                    AISettings.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            AISettings.Admins.Remove(AISettingsAdminsLB.GetItemText(AISettingsAdminsLB.SelectedItem));
            AISettings.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            AISettings.PlayerFactions.Add(PlayerFactionCB.GetItemText(PlayerFactionCB.SelectedItem));
            AISettings.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            AISettings.PlayerFactions.Remove(PlayerFactionsLB.GetItemText(PlayerFactionsLB.SelectedItem));
            AISettings.isDirty = true;
        }
        private void SniperProneDistanceThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.SniperProneDistanceThreshold = SniperProneDistanceThresholdNUD.Value;
            AISettings.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            string classname = PreventClimbTB.Text;
            if (classname == "") return;
            if (!AISettings.PreventClimb.Contains(classname))
            {
                AISettings.PreventClimb.Add(classname);
                AIPatrolSettings.isDirty = true;
                PreventClimbLB.Refresh();
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            AISettings.PreventClimb.Remove(PreventClimbLB.GetItemText(PreventClimbLB.SelectedItem));
            AISettings.isDirty = true;
            PreventClimbLB.Refresh();
        }
        private void NoiseInvestigationDistanceLimitNUD_ValueChanged_1(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.NoiseInvestigationDistanceLimit = (int)NoiseInvestigationDistanceLimitNUD.Value;
            AISettings.isDirty = true;
        }
        #endregion AISettings



        private void groupBox4_Enter(object sender, EventArgs e)
        {
            

        }


    }
}
