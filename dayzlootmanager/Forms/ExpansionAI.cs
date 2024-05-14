using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{

    public partial class ExpansionAI : DarkForm
    {
        public Project currentproject { get; internal set; }
        public bool IsDynamicAiLoaded = false;
        public int AIPatrolScale = 1;
        public ExpansionAISettings AISettings { get; set; }
        public ExpansionAIPatrolSettings AIPatrolSettings { get; set; }
        public BindingList<AILoadouts> LoadoutList { get; private set; }
        public BindingList<string> LoadoutNameList { get; private set; }
        public BindingList<string> LoadoutNameList2 { get; private set; }
        public BindingList<string> Factions { get; set; }
        public MapData MapData { get; private set; }
        public Spatial_Notifications m_Spatial_Notifications { get; set; }
        public Spatial_Players m_Spatial_Players { get; set; }
        public Spatial_Groups m_Spatial_Groups { get; set; }
        public string AISettingsPath;
        public string AILoadoutsPath;
        public string AIPatrolSettingsPath;
        public string AIDynamicSettingsPath;
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
            FactionEditLB.DataSource = Factions;
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
            AIPatrolSettings.GetAIPatrolWaypoints();
            AIPatrolSettings.Filename = AIPatrolSettingsPath;
            SetupAIPatrolSettings();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAIPAtrols);
            trackBar4.Value = 1;
            SetAIPatrolZonescale();

            if (File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\Spatial\\SpatialSettings.json") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\Spatial\\SpatialPlayerSettings.json") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\Spatial\\Notificationsettings.json"))
            {
                IsDynamicAiLoaded = true;
                toolStripSeparator1.Visible = true;
                toolStripButton6.Visible = true;
                AIDynamicSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\Spatial";
                Console.WriteLine("Dynamic AI Installed.....\n\tWill now serialize");
                if (!File.Exists(AIDynamicSettingsPath + "\\Notificationsettings.json") && IsDynamicAiLoaded == true)
                {
                    m_Spatial_Notifications = new Spatial_Notifications();
                    m_Spatial_Notifications.isDirty = true;
                    needtosave = true;
                    Console.WriteLine("\tNotificationsettings.json" + " File not found, Creating new....");
                }
                else if (IsDynamicAiLoaded)
                {
                    Console.WriteLine("\tserializing " + "Notificationsettings.json");
                    m_Spatial_Notifications = JsonSerializer.Deserialize<Spatial_Notifications>(File.ReadAllText(AIDynamicSettingsPath + "\\Notificationsettings.json"));
                    m_Spatial_Notifications.isDirty = false;
                    if (m_Spatial_Notifications.checkver())
                        needtosave = true;
                }
                m_Spatial_Notifications.Filename = AIDynamicSettingsPath + "\\Notificationsettings.json";
                SetupSpatialNotifications();

                if (!File.Exists(AIDynamicSettingsPath + "\\SpatialPlayerSettings.json") && IsDynamicAiLoaded == true)
                {
                    m_Spatial_Players = new Spatial_Players();
                    m_Spatial_Players.isDirty = true;
                    needtosave = true;
                    Console.WriteLine("\tSpatialPlayerSettings.json" + " File not found, Creating new....");
                }
                else if (IsDynamicAiLoaded)
                {
                    Console.WriteLine("\tserializing " + "SpatialPlayerSettings.json");
                    m_Spatial_Players = JsonSerializer.Deserialize<Spatial_Players>(File.ReadAllText(AIDynamicSettingsPath + "\\SpatialPlayerSettings.json"));
                    m_Spatial_Players.isDirty = false;
                    if (m_Spatial_Players.checkver())
                        needtosave = true;
                }
                m_Spatial_Players.Filename = AIDynamicSettingsPath + "\\SpatialPlayerSettings.json";
                SetupSpatialPlayerSettings();

                if (!File.Exists(AIDynamicSettingsPath + "\\SpatialSettings.json") && IsDynamicAiLoaded == true)
                {
                    m_Spatial_Groups = new Spatial_Groups();
                    m_Spatial_Groups.isDirty = true;
                    needtosave = true;
                    Console.WriteLine("\tSpatialSettings.json" + " File not found, Creating new....");
                }
                else if (IsDynamicAiLoaded)
                {
                    Console.WriteLine("\tserializing " + "SpatialSettings.json");
                    m_Spatial_Groups = JsonSerializer.Deserialize<Spatial_Groups>(File.ReadAllText(AIDynamicSettingsPath + "\\SpatialSettings.json"));
                    m_Spatial_Groups.isDirty = false;
                    m_Spatial_Groups.getAllPoints();
                    if (m_Spatial_Groups.checkver())
                        needtosave = true;
                }
                m_Spatial_Groups.Filename = AIDynamicSettingsPath + "\\SpatialSettings.json";
                SetupSpatialGroups();
            }
            else
            {
                toolStripSeparator1.Visible = false;
                toolStripButton6.Visible = false;
            }

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawDynamicAI);
            trackBar1.Value = 1;
            SetDynamicAIScale();



            tabControl1.ItemSize = new Size(0, 1);
            tabControl2.ItemSize = new Size(0, 1);
            tabControl3.ItemSize = new Size(0, 1);
            tabControl4.ItemSize = new Size(0, 1);
            toolStripButton8.Checked = true;
            toolStripButton4.Checked = true;
            toolStripButton7.Checked = true;
            toolStripButton12.Checked = true;
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
            if (IsDynamicAiLoaded)
            {
                if (m_Spatial_Notifications.isDirty)
                {
                    needtosave = true;
                }
                if (m_Spatial_Players.isDirty)
                {
                    needtosave = true;
                }
                if (m_Spatial_Groups.isDirty)
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
            SpatialGroupsPointSpatial_FactionCB.DataSource = new BindingList<string>(Factions);
            SpatialGroupsGroupSpatial_FactionCB.DataSource = new BindingList<string>(Factions);
            SpatialGroupsLocationSpatial_FactionCB.DataSource = new BindingList<string>(Factions);
            SpatialGroupsAudioSpatial_FactionCB.DataSource = new BindingList<string>(Factions);

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
                AIPatrolSettings.SetAIPatrolWaypoints();
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

            foreach (AILoadouts AILO in LoadoutList)
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

            if (IsDynamicAiLoaded)
            {
                if (m_Spatial_Notifications.isDirty)
                {
                    if (currentproject.Createbackups && File.Exists(m_Spatial_Notifications.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(m_Spatial_Notifications.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(m_Spatial_Notifications.Filename, Path.GetDirectoryName(m_Spatial_Notifications.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(m_Spatial_Notifications.Filename) + ".bak", true);
                    }
                    m_Spatial_Notifications.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(m_Spatial_Notifications, options);
                    File.WriteAllText(m_Spatial_Notifications.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(m_Spatial_Notifications.Filename));
                }
                if (m_Spatial_Players.isDirty)
                {
                    if (currentproject.Createbackups && File.Exists(m_Spatial_Players.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(m_Spatial_Players.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(m_Spatial_Players.Filename, Path.GetDirectoryName(m_Spatial_Players.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(m_Spatial_Players.Filename) + ".bak", true);
                    }
                    m_Spatial_Players.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(m_Spatial_Players, options);
                    File.WriteAllText(m_Spatial_Players.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(m_Spatial_Players.Filename));
                }
                if (m_Spatial_Groups.isDirty)
                {
                    if (currentproject.Createbackups && File.Exists(m_Spatial_Groups.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(m_Spatial_Groups.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(m_Spatial_Groups.Filename, Path.GetDirectoryName(m_Spatial_Groups.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(m_Spatial_Groups.Filename) + ".bak", true);
                    }
                    m_Spatial_Groups.isDirty = false;
                    m_Spatial_Groups.setAllPoints();
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(m_Spatial_Groups, options);
                    File.WriteAllText(m_Spatial_Groups.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(m_Spatial_Groups.Filename));
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
            LoadoutNameList = new BindingList<string>();
            foreach (AILoadouts lo in LoadoutList)
            {
                LoadoutNameList.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }
            CrashLoadoutFileCB.DisplayMember = "DisplayName";
            CrashLoadoutFileCB.ValueMember = "Value";
            CrashLoadoutFileCB.DataSource = new BindingList<string>(LoadoutNameList);
            StaticPatrolLB.Refresh();

            StaticPatrolLoadoutsCB.DisplayMember = "DisplayName";
            StaticPatrolLoadoutsCB.ValueMember = "Value";
            StaticPatrolLoadoutsCB.DataSource = new BindingList<string>(LoadoutNameList);
            EventCrachPatrolLB.Refresh();

            LoadoutNameList2 = new BindingList<string>();
            foreach (AILoadouts lo in LoadoutList)
            {
                LoadoutNameList2.Add(Path.GetFileName(lo.Filename));
            }
            SpatialGroupsGroupSpatial_LoadoutCB.DisplayMember = "DisplayName";
            SpatialGroupsGroupSpatial_LoadoutCB.ValueMember = "Value";
            SpatialGroupsGroupSpatial_LoadoutCB.DataSource = new BindingList<string>(LoadoutNameList2);

            SpatialGroupsLocationSpatial_ZoneLoadoutCB.DisplayMember = "DisplayName";
            SpatialGroupsLocationSpatial_ZoneLoadoutCB.ValueMember = "Value";
            SpatialGroupsLocationSpatial_ZoneLoadoutCB.DataSource = new BindingList<string>(LoadoutNameList2);

            SpatialGroupsAudioSpatial_ZoneLoadoutCB.DisplayMember = "DisplayName";
            SpatialGroupsAudioSpatial_ZoneLoadoutCB.ValueMember = "Value";
            SpatialGroupsAudioSpatial_ZoneLoadoutCB.DataSource = new BindingList<string>(LoadoutNameList2);

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
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton1.Checked = false;
            toolStripButton6.Checked = false;
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
                case 3:
                    toolStripButton6.Checked = true;
                    break;
            }
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    toolStripButton4.Checked = true;
                    break;
                case 1:
                    toolStripButton5.Checked = true;
                    break;
            }
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 0;
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 1;
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 2;
        }
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 3;
        }
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton7.Checked = false;
            toolStripButton9.Checked = false;
            toolStripButton10.Checked = false;
            toolStripButton11.Checked = false;
            switch (tabControl3.SelectedIndex)
            {
                case 0:
                    toolStripButton7.Checked = true;
                    break;
                case 1:
                    toolStripButton9.Checked = true;
                    break;
                case 2:
                    toolStripButton10.Checked = true;
                    break;
                case 3:
                    toolStripButton11.Checked = true;
                    break;
            }
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 0;
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 1;
        }

        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton12.Checked = false;
            toolStripButton13.Checked = false;
            switch (tabControl4.SelectedIndex)
            {
                case 0:
                    toolStripButton12.Checked = true;
                    break;
                case 1:
                    toolStripButton13.Checked = true;
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
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
                case 3:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\Spatial");
                    break;
            }
        }
        #region aipatrolsettings
        public ExpansionAIObjectPatrol CurrentEventcrashpatrol;
        public ExpansionAIPatrol CurrentPatrol;
        public Vec3 CurrentWapypoint;
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
            AIGeneralNoiseInvestigationDistanceLimitNUD.Value = AIPatrolSettings.NoiseInvestigationDistanceLimit;
            AIGeneralDanageMultiplierNUD.Value = AIPatrolSettings.DamageMultiplier;
            AIGeneralDamageReceivedMultiplierNUD.Value = AIPatrolSettings.DamageReceivedMultiplier;

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
            AIPatrolSettings.NoiseInvestigationDistanceLimit = AIGeneralNoiseInvestigationDistanceLimitNUD.Value;
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
            crashNoiseInvestigationDistanceLimitNUD.Value = CurrentEventcrashpatrol.NoiseInvestigationDistanceLimit;
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

            CrashUnitsLB.DisplayMember = "DisplayName";
            CrashUnitsLB.ValueMember = "Value";
            CrashUnitsLB.DataSource = CurrentEventcrashpatrol.Units;

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
        private void CrashNoiseInvestigationDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.NoiseInvestigationDistanceLimit = crashNoiseInvestigationDistanceLimitNUD.Value;
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
        private void darkButton16_Click(object sender, EventArgs e)
        {
            AddNewfileName form = new AddNewfileName()
            {
                setdescription = "Please enter the Unit you wish to add",
                SetTitle = "Add Unit",
                Setbutton = "Add"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                CurrentEventcrashpatrol.Units.Add(form.NewFileName);
                CrashUnitsLB.Invalidate();
                AIPatrolSettings.isDirty = true;
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            CurrentEventcrashpatrol.Units.Remove(CrashUnitsLB.GetItemText(CrashUnitsLB.SelectedItem));
            CrashUnitsLB.Invalidate();
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
            StaticPatrolNoiseInvestigationDistanceLimitNUD.Value = CurrentPatrol.NoiseInvestigationDistanceLimit;

            StaticPatrolUnitsLB.DisplayMember = "DisplayName";
            StaticPatrolUnitsLB.ValueMember = "Value";
            StaticPatrolUnitsLB.DataSource = CurrentPatrol.Units;

            StaticPatrolWayPointsLB.DisplayMember = "DisplayName";
            StaticPatrolWayPointsLB.ValueMember = "Value";
            StaticPatrolWayPointsLB.DataSource = CurrentPatrol._waypoints;

            if (CurrentPatrol._waypoints.Count == 0)
            {
                StaticPatrolWaypointPOSXNUD.Visible = false;
                StaticPatrolWaypointPOSYNUD.Visible = false;
                StaticPatrolWaypointPOSZNUD.Visible = false;
            }
            else
            {
                StaticPatrolWaypointPOSXNUD.Visible = true;
                StaticPatrolWaypointPOSYNUD.Visible = true;
                StaticPatrolWaypointPOSZNUD.Visible = true;
            }
            pictureBox2.Invalidate();
            useraction = true;
        }
        private void StaticPatrolWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = StaticPatrolWayPointsLB.SelectedItem as Vec3;
            useraction = false;
            StaticPatrolWaypointPOSXNUD.Value = (decimal)CurrentWapypoint.X;
            StaticPatrolWaypointPOSYNUD.Value = (decimal)CurrentWapypoint.Y;
            StaticPatrolWaypointPOSZNUD.Value = (decimal)CurrentWapypoint.Z;
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
                        CurrentPatrol._waypoints.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        CurrentPatrol._waypoints.Add(new Vec3(XYZ));

                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                    StaticPatrolWaypointPOSXNUD.Visible = true;
                    StaticPatrolWaypointPOSYNUD.Visible = true;
                    StaticPatrolWaypointPOSZNUD.Visible = true;
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
                        CurrentPatrol._waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        CurrentPatrol._waypoints.Add(new Vec3(eo.Position));
                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                    StaticPatrolWaypointPOSXNUD.Visible = true;
                    StaticPatrolWaypointPOSYNUD.Visible = true;
                    StaticPatrolWaypointPOSZNUD.Visible = true;
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
        private void StaticPatrolNoiseInvestigationDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.NoiseInvestigationDistanceLimit = AIGeneralNoiseInvestigationDistanceLimitNUD.Value;
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
            CurrentWapypoint.X = (float)StaticPatrolWaypointPOSXNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint.Y = (float)StaticPatrolWaypointPOSYNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint.Z = (float)StaticPatrolWaypointPOSZNUD.Value;
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
                Faction = "West",
                Formation = "",
                FormationLooseness = (decimal)0.0,
                LoadoutFile = "",
                Units = new BindingList<string>(),
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
            foreach (Vec3 array in CurrentPatrol._waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array.GetString() + "|0.0 0.0 0.0");
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
            int m_Id = 0;
            foreach (Vec3 array in CurrentPatrol._waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
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
        private void darkButton22_Click(object sender, EventArgs e)
        {
            if (CurrentPatrol._waypoints == null)
                CurrentPatrol._waypoints = new BindingList<Vec3>();
            CurrentPatrol._waypoints.Add(new Vec3(0, 0, 0));
            AIPatrolSettings.isDirty = true;
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWaypointPOSXNUD.Visible = true;
            StaticPatrolWaypointPOSYNUD.Visible = true;
            StaticPatrolWaypointPOSZNUD.Visible = true;
            StaticPatrolWayPointsLB.SelectedIndex = -1;
            StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            int index = StaticPatrolWayPointsLB.SelectedIndex;
            CurrentPatrol._waypoints.Remove(CurrentWapypoint);
            AIPatrolSettings.isDirty = true;
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWayPointsLB.SelectedIndex = -1;
            if (StaticPatrolWayPointsLB.Items.Count > 0)
            {
                if (StaticPatrolWayPointsLB.Items.Count == index)
                {
                    StaticPatrolWayPointsLB.SelectedIndex = index - 1;
                }
                else
                {
                    StaticPatrolWayPointsLB.SelectedIndex = index;
                }
            }
            else
            {
                StaticPatrolWayPointsLB.SelectedIndex = -1;
                StaticPatrolWaypointPOSXNUD.Visible = false;
                StaticPatrolWaypointPOSYNUD.Visible = false;
                StaticPatrolWaypointPOSZNUD.Visible = false;
            }
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            AddNewfileName form = new AddNewfileName()
            {
                setdescription = "Please enter the Unit you wish to add",
                SetTitle = "Add Unit",
                Setbutton = "Add"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                CurrentPatrol.Units.Add(form.NewFileName);
                StaticPatrolUnitsLB.Invalidate();
                AIPatrolSettings.isDirty = true;
            }
        }
        private void darkButton10_Click_1(object sender, EventArgs e)
        {
            CurrentPatrol.Units.Remove(StaticPatrolUnitsLB.GetItemText(StaticPatrolUnitsLB.SelectedItem));
            StaticPatrolUnitsLB.Invalidate();
            AIPatrolSettings.isDirty = true;
        }
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            AIPatrolScale = trackBar4.Value;
            SetAIPatrolZonescale();
        }
        private void SetAIPatrolZonescale()
        {
            float scalevalue = AIPatrolScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }
        private void DrawAIPAtrols(object sender, PaintEventArgs e)
        {
            if (checkBox10.Checked)
            {
                foreach (ExpansionAIPatrol aipatrol in AIPatrolSettings.Patrols)
                {
                    int c = 1;
                    foreach (Vec3 waypoints in aipatrol._waypoints)
                    {
                        float scalevalue = AIPatrolScale * 0.05f;
                        int centerX = (int)(Math.Round(waypoints.X) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(waypoints.Z, 0) * scalevalue);
                        int eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (aipatrol == CurrentPatrol)
                            pen = new Pen(Color.Green, 4);
                        string num = c.ToString();
                        if (c == 1)
                            getCircle(e.Graphics, pen, center, eventradius, aipatrol.Name + "\n" + num);
                        else
                            getCircle(e.Graphics, pen, center, eventradius, "\n" + num);
                        c++;
                    }
                }
            }
            else
            {
                int c = 1;
                foreach (Vec3 waypoints in CurrentPatrol._waypoints)
                {
                    float scalevalue = AIPatrolScale * 0.05f;
                    int centerX = (int)(Math.Round(waypoints.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(waypoints.Z, 0) * scalevalue);
                    int eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    string num = c.ToString();
                    if (c == 1)
                        getCircle(e.Graphics, pen, center, eventradius, CurrentPatrol.Name + "\n" + num);
                    else
                        getCircle(e.Graphics, pen, center, eventradius, "\n" + num);
                    c++;
                }
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius, string c)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
            Rectangle rect3 = new Rectangle(center.X - radius, center.Y - (radius + 30), 200, 40);
            drawingArea.DrawString(c, new Font("Tahoma", 9), Brushes.White, rect3);
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox2.Focused == false)
            {
                pictureBox2.Focus();
                panel2.AutoScrollPosition = _newscrollPosition;
                pictureBox2.Invalidate();
            }
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel2.AutoScrollPosition.X - changePoint.X, -panel2.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel2.AutoScrollPosition = _newscrollPosition;
                pictureBox2.Invalidate();
            }
            decimal scalevalue = AIPatrolScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label1.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox2_ZoomOut();
            }
            else
            {
                pictureBox2_ZoomIn();
            }

        }
        private void pictureBox2_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar4.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar4.Value = newval;
            AIPatrolScale = trackBar4.Value;
            SetAIPatrolZonescale();
            if (pictureBox2.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox2.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
        }
        private void pictureBox2_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar4.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar4.Value = newval;
            AIPatrolScale = trackBar4.Value;
            SetAIPatrolZonescale();
            if (pictureBox2.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox2.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
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
            LogAIHitByCB.Checked = AISettings.LogAIHitBy == 1 ? true : false;
            LogAIKilledCB.Checked = AISettings.LogAIKilled == 1 ? true : false;
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
        private void LogAIHitByCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.LogAIHitBy = LogAIHitByCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void LogAIKilledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.LogAIKilled = LogAIKilledCB.Checked == true ? 1 : 0;
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
            AISettings.NoiseInvestigationDistanceLimit = NoiseInvestigationDistanceLimitNUD.Value;
            AISettings.isDirty = true;
        }
        #endregion AISettings
        #region dynamic AI Notifications
        private void SetupSpatialNotifications()
        {
            useraction = false;

            SpatialNorifictaionsLB.DisplayMember = "DisplayName";
            SpatialNorifictaionsLB.ValueMember = "Value";
            SpatialNorifictaionsLB.DataSource = m_Spatial_Notifications.notification;

            useraction = true;
        }
        public Spatial_Notification CurrentSpatialNotification { get; set; }
        private void SpatialNorifictaionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpatialNorifictaionsLB.SelectedItems.Count < 1) return;
            CurrentSpatialNotification = SpatialNorifictaionsLB.SelectedItem as Spatial_Notification;
            useraction = false;
            SpatialNotificationSpatial_NameTB.Text = CurrentSpatialNotification.Spatial_Name;
            SpatialNotificationsStartTimeNUD.Value = CurrentSpatialNotification.StartTime;
            SpatialNotificationsStopTimeNUD.Value = CurrentSpatialNotification.StopTime;
            SpatialNotificationsAgeTimeNUD.Value = CurrentSpatialNotification.AgeTime;
            SpatailNotificationsMessageTypeCB.SelectedIndex = CurrentSpatialNotification.MessageType;
            SpatialNotificationsMessageTitleTB.Text = CurrentSpatialNotification.MessageTitle;

            SpatialNotificationMessageTextLB.DisplayMember = "DisplayName";
            SpatialNotificationMessageTextLB.ValueMember = "Value";
            SpatialNotificationMessageTextLB.DataSource = CurrentSpatialNotification.MessageText;

            useraction = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            m_Spatial_Notifications.notification.Add(new Spatial_Notification("New Notification", (decimal)0.0, (decimal)24.0, (decimal)1.0, 1, "New Notification Title", new BindingList<string>() { "New Notification Text" }));
            m_Spatial_Notifications.isDirty = true;
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            m_Spatial_Notifications.notification.Remove(CurrentSpatialNotification);
            m_Spatial_Notifications.isDirty = true;
            SpatialNorifictaionsLB.Refresh();
        }
        private void SpatialNotificationMessageTextLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpatialNotificationMessageTextLB.SelectedItems.Count < 1) return;
            useraction = false;
            SpatialNotificationMessageTextTB.Text = SpatialNotificationMessageTextLB.SelectedItem.ToString();
            useraction = true;
        }
        private void SpatialNotificationSpatial_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.Spatial_Name = SpatialNotificationSpatial_NameTB.Text;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatialNotificationsStartTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.StartTime = SpatialNotificationsStartTimeNUD.Value;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatialNotificationsStopTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.StopTime = SpatialNotificationsStopTimeNUD.Value;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatialNotificationsAgeTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.AgeTime = SpatialNotificationsAgeTimeNUD.Value;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatailNotificationsMessageTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.MessageType = SpatailNotificationsMessageTypeCB.SelectedIndex;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatialNotificationsMessageTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentSpatialNotification.MessageTitle = SpatialNotificationsMessageTitleTB.Text;
            m_Spatial_Notifications.isDirty = true;
        }
        private void SpatialNotificationMessageTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            int index = SpatialNotificationMessageTextLB.SelectedIndex;
            CurrentSpatialNotification.MessageText[index] = SpatialNotificationMessageTextTB.Text;
            SpatialNotificationMessageTextLB.Invalidate();
            m_Spatial_Notifications.isDirty = true;
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            CurrentSpatialNotification.MessageText.Add("New Message, Please change me.....");
            SpatialNotificationMessageTextLB.Invalidate();
            m_Spatial_Notifications.isDirty = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            if (SpatialNotificationMessageTextLB.SelectedItems.Count < 1) return;
            CurrentSpatialNotification.MessageText.Remove(SpatialNotificationMessageTextLB.GetItemText(SpatialNotificationMessageTextLB.SelectedItem));
            m_Spatial_Notifications.isDirty = true;
        }
        #endregion dynamic AI Notifications
        #region dynamic AI Player Settings
        public Spatial_Player currentSpatialPlayers { get; set; }
        private void SetupSpatialPlayerSettings()
        {
            useraction = false;
            SpatialPlayersLB.DisplayMember = "DisplayName";
            SpatialPlayersLB.ValueMember = "Value";
            SpatialPlayersLB.DataSource = m_Spatial_Players.Group;
            useraction = true;
        }
        private void SpatialPlayersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpatialPlayersLB.SelectedItems.Count < 1) return;
            currentSpatialPlayers = SpatialPlayersLB.SelectedItem as Spatial_Player;
            useraction = false;
            SpatialPlayersUIDTB.Text = currentSpatialPlayers.UID;
            SpatialPlayersPlayer_BirthdayNUD.Maximum = int.MaxValue;
            SpatialPlayersPlayer_BirthdayNUD.Value = (int)currentSpatialPlayers.Player_Birthday;
            useraction = true;
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            m_Spatial_Players.Group.Add(new Spatial_Player()
            {
                UID = "ChangeMe",
                Player_Birthday = 0
            });
            SpatialPlayersLB.Refresh();
            m_Spatial_Players.isDirty = true;
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            m_Spatial_Players.Group.Remove(currentSpatialPlayers);
            m_Spatial_Players.isDirty = true;
            SpatialPlayersLB.Refresh();
        }
        private void SpatialPlayersUIDTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPlayers.UID = SpatialPlayersUIDTB.Text;
            m_Spatial_Players.isDirty = true;
        }
        private void SpatialPlayersPlayer_BirthdayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPlayers.Player_Birthday = (int)SpatialPlayersPlayer_BirthdayNUD.Value;
            m_Spatial_Players.isDirty = true;
        }
        #endregion dynamic AI Player Settings
        #region dynamic AI Spatial groups

        public Spatial_Point currentSpatialPoint { get; set; }
        public Spatial_Group currentSpatialGroup { get; set; }
        public Spatial_Location currentSpatialLocation { get; set; }
        public Spatial_Audio currentSpatialAudio { get; set; }
        public Vec3 currentspawnPosition { get; set; }

        private void SetupSpatialGroups()
        {
            useraction = false;

            SpatialGroupSpatial_MinTimerNUD.Value = m_Spatial_Groups.Spatial_MinTimer;
            SpatialGroupSpatial_MaxTimerNUD.Value = m_Spatial_Groups.Spatial_MaxTimer;
            SpatialGroupMinDistanceNUD.Value = m_Spatial_Groups.MinDistance;
            SpatialGroupMaxDistanceNUD.Value = m_Spatial_Groups.MaxDistance;
            SpatialGroupHuntModeCB.SelectedIndex = m_Spatial_Groups.HuntMode - 1;
            SpatialGroupsPoints_EnabledCB.SelectedIndex = m_Spatial_Groups.Points_Enabled;
            SpatialGroupsLocations_EnabledCB.SelectedIndex = m_Spatial_Groups.Locations_Enabled;
            SpatialGroupsAudio_EnabledCB.SelectedIndex = m_Spatial_Groups.Audio_Enabled;
            SpatialGroupsEngageTimerNUD.Value = m_Spatial_Groups.EngageTimer;
            SpatialGroupsCleanupTimerNUD.Value = m_Spatial_Groups.CleanupTimer;
            SpatialGroupsPlayerChecksNUD.Value = m_Spatial_Groups.PlayerChecks;
            SpatialGroupsMaxAINUD.Value = m_Spatial_Groups.MaxAI;
            SpatialGroupsGroupDifficultyNUD.Value = m_Spatial_Groups.GroupDifficulty;
            SpatialgroupsMinimumPlayerDistanceNUD.Value = m_Spatial_Groups.MinimumPlayerDistance;
            SpatialGroupsMinimumAgeNUD.Value = m_Spatial_Groups.MinimumAge;
            SpatialGroupsActiveHoursEnabledCB.SelectedIndex = m_Spatial_Groups.ActiveHoursEnabled;
            SpatialGroupsActiveStartTimeNUD.Value = m_Spatial_Groups.ActiveStartTime;
            SpatialGroupsActiveStopTimeNUD.Value = m_Spatial_Groups.ActiveStopTime;
            SpatialGroupsMessageTypeCB.SelectedIndex = m_Spatial_Groups.MessageType;
            SpatialGroupsMessageTitleTB.Text = m_Spatial_Groups.MessageTitle;
            SpatialGroupsMessageTextTB.Text = m_Spatial_Groups.MessageText;

            SpatialGroupsCB.SelectedIndex = 0;

            useraction = true;
        }
        private void SpatialGroupSpatial_MinTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_MinTimer = SpatialGroupSpatial_MinTimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupSpatial_MaxTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_MaxTimer = SpatialGroupSpatial_MaxTimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupMinDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MinDistance = (int)SpatialGroupMinDistanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupMaxDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MaxDistance = (int)SpatialGroupMaxDistanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupHuntModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.HuntMode = SpatialGroupHuntModeCB.SelectedIndex + 1;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPoints_EnabledCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Points_Enabled = SpatialGroupsPoints_EnabledCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocations_EnabledCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Locations_Enabled = SpatialGroupsLocations_EnabledCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudio_EnabledCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Audio_Enabled = SpatialGroupsAudio_EnabledCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsEngageTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.EngageTimer = SpatialGroupsEngageTimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsCleanupTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.CleanupTimer = SpatialGroupsCleanupTimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPlayerChecksNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.PlayerChecks = (int)SpatialGroupsPlayerChecksNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsMaxAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MaxAI = (int)SpatialGroupsMaxAINUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupDifficultyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.GroupDifficulty = (int)SpatialGroupsGroupDifficultyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialgroupsMinimumPlayerDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MinimumPlayerDistance = (int)SpatialgroupsMinimumPlayerDistanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsMinimumAgeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MinimumAge = (int)SpatialGroupsMinimumAgeNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsActiveHoursEnabledCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.ActiveHoursEnabled = SpatialGroupsActiveHoursEnabledCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsActiveStartTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.ActiveStartTime = SpatialGroupsActiveStartTimeNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsActiveStopTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.ActiveStopTime = SpatialGroupsActiveStopTimeNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsMessageTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MessageType = SpatialGroupsMessageTypeCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsMessageTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MessageTitle = SpatialGroupsMessageTitleTB.Text;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsMessageTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.MessageText = SpatialGroupsMessageTextTB.Text;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_InVehicle = checkBox1.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_IsBleeding = checkBox2.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_IsRestrained = checkBox3.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_IsUnconscious = checkBox4.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_IsInSafeZone = checkBox5.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_TPSafeZone = checkBox6.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            m_Spatial_Groups.Spatial_InOwnTerritory = checkBox7.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }

        private void SpatialGroupsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 0:
                    SpatialGroupsLB.DisplayMember = "DisplayName";
                    SpatialGroupsLB.ValueMember = "Value";
                    SpatialGroupsLB.DataSource = m_Spatial_Groups.Group;
                    break;
                case 1:
                    SpatialGroupsLB.DisplayMember = "DisplayName";
                    SpatialGroupsLB.ValueMember = "Value";
                    SpatialGroupsLB.DataSource = m_Spatial_Groups.Point;
                    break;
                case 2:
                    SpatialGroupsLB.DisplayMember = "DisplayName";
                    SpatialGroupsLB.ValueMember = "Value";
                    SpatialGroupsLB.DataSource = m_Spatial_Groups.Location;
                    break;
                case 3:
                    SpatialGroupsLB.DisplayMember = "DisplayName";
                    SpatialGroupsLB.ValueMember = "Value";
                    SpatialGroupsLB.DataSource = m_Spatial_Groups.Audio;
                    break;

            }

        }
        private void SpatialGroupsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpatialGroupsLB.SelectedItems.Count < 1) return;
            currentSpatialGroup = null;
            currentSpatialPoint = null;
            currentSpatialLocation = null;
            currentSpatialAudio = null;
            SpatialGroupsSpawnPositionLB.DataSource = null;
            SpatialGroupGB.Visible = false;
            SpatialPointGB.Visible = false;
            SpatialLocationGB.Visible = false;
            SpatialAudioGB.Visible = false;
            SpawnPositionGB.Visible = false;
            useraction = false;
            if (SpatialGroupsLB.SelectedItem is Spatial_Group)
            {
                currentSpatialGroup = SpatialGroupsLB.SelectedItem as Spatial_Group;
                SpatialGroupGB.Visible = true;
                SpatialGroupImportGB.Visible = false;
                SpatialGroupsGroupSpatial_NameTB.Text = currentSpatialGroup.Spatial_Name;
                SpatialGroupsGroupSpatial_MinCountNUD.Value = currentSpatialGroup.Spatial_MinCount;
                SpatialGroupsGroupSpatial_MaxCountNUD.Value = currentSpatialGroup.Spatial_MaxCount;
                SpatialGroupsGroupSpatial_WeightNUD.Value = currentSpatialGroup.Spatial_Weight;
                SpatialGroupsGroupSpatial_LoadoutCB.SelectedIndex = SpatialGroupsGroupSpatial_LoadoutCB.FindStringExact(currentSpatialGroup.Spatial_Loadout);
                SpatialGroupsGroupSpatial_FactionCB.SelectedIndex = SpatialGroupsGroupSpatial_FactionCB.FindStringExact(currentSpatialGroup.Spatial_Faction);
                SpatialGroupsGroupSpatial_LootableCB.SelectedIndex = currentSpatialGroup.Spatial_Lootable;
                SpatialGroupsGroupSpatial_ChanceNUD.Value = currentSpatialGroup.Spatial_Chance;
                SpatialGroupsGroupSpatial_MinAccuracyNUD.Value = currentSpatialGroup.Spatial_MinAccuracy;
                SpatialGroupsGroupSpatial_MaxAccuracyNUD.Value = currentSpatialGroup.Spatial_MaxAccuracy;
                SpatialGroupsGroupSpatial_UnlimitedReloadCB.Checked = currentSpatialGroup.Spatial_UnlimitedReload == 1 ? true : false;
            }
            else if (SpatialGroupsLB.SelectedItem is Spatial_Point)
            {
                currentSpatialPoint = SpatialGroupsLB.SelectedItem as Spatial_Point;
                SpatialPointGB.Visible = true;
                SpatialGroupImportGB.Visible = true;
                SpatialGroupImportGB.Location = new Point(496, 6);
                SpatialGroupsPointSpatial_NameTB.Text = currentSpatialPoint.Spatial_Name;
                SpatialGroupsPointSpatial_SafeCB.Checked = currentSpatialPoint.Spatial_Safe == 1 ? true : false;
                SpatialGroupsPointSpatial_RadiusNUD.Value = currentSpatialPoint.Spatial_Radius;
                SpatialGroupsPointSpatial_MinCountNUD.Value = currentSpatialPoint.Spatial_MinCount;
                SpatialGroupsPointSpatial_MaxCountNUD.Value = currentSpatialPoint.Spatial_MaxCount;
                SpatialGroupsPointSpatial_HuntModeCB.SelectedIndex = currentSpatialPoint.Spatial_HuntMode - 1;
                SpatialGroupsPointSpatial_FactionCB.SelectedIndex = SpatialGroupsPointSpatial_FactionCB.FindStringExact(currentSpatialPoint.Spatial_Faction);
                SpatialGroupsPointSpatial_LootableCB.SelectedIndex = currentSpatialPoint.Spatial_Lootable;
                SpatialGroupsPointSpatial_ChanceNUD.Value = currentSpatialPoint.Spatial_Chance;
                SpatialGroupsPointSpatial_MinAccuracyNUD.Value = currentSpatialPoint.Spatial_MinAccuracy;
                SpatialGroupsPointSpatial_Max_AccuracyNUD.Value = currentSpatialPoint.Spatial_MaxAccuracy;
                SpatialGroupsPointSpatial_UnlimitedReloadCB.Checked = currentSpatialPoint.Spatial_UnlimitedReload == 1 ? true : false;
                SpatialGroupsPointSpatial_PositionXNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.X;
                SpatialGroupsPointSpatial_PositionYNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.Y;
                SpatialGroupsPointSpatial_PositionZNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.Z;

                SpatialGroupsSpatial_ZoneLoadoutLB.DisplayMember = "DisplayName";
                SpatialGroupsSpatial_ZoneLoadoutLB.ValueMember = "Value";
                SpatialGroupsSpatial_ZoneLoadoutLB.DataSource = currentSpatialPoint.Spatial_ZoneLoadout;

            }
            else if (SpatialGroupsLB.SelectedItem is Spatial_Location)
            {
                currentSpatialLocation = SpatialGroupsLB.SelectedItem as Spatial_Location;
                SpatialLocationGB.Visible = true;
                SpawnPositionGB.Visible = true;
                SpawnPositionGB.Height = 472;
                SpatialGroupImportGB.Visible = true;
                SpatialGroupImportGB.Location = new Point(787, 6);
                SpatialGroupsLocationSpatial_NameTB.Text = currentSpatialLocation.Spatial_Name;
                SpatialGroupsLocationSpatial_TriggerRadiusNUD.Value = currentSpatialLocation.Spatial_TriggerRadius;
                SpatialGroupsLocationSpatial_ZoneLoadoutCB.SelectedIndex = SpatialGroupsLocationSpatial_ZoneLoadoutCB.FindStringExact(currentSpatialLocation.Spatial_ZoneLoadout);
                SpatialGroupsLocationSpatial_MinCountNUD.Value = currentSpatialLocation.Spatial_MinCount;
                SpatialGroupsLocationSpatial_MaxCountNUD.Value = currentSpatialLocation.Spatial_MaxCount;
                SpatialGroupsLocationSpatial_HuntModeCB.SelectedIndex = currentSpatialLocation.Spatial_HuntMode - 1;
                SpatialGroupsLocationSpatial_FactionCB.SelectedIndex = SpatialGroupsLocationSpatial_FactionCB.FindStringExact(currentSpatialLocation.Spatial_Faction);
                SpatialGroupsLocationSpatial_LootableCB.SelectedIndex = currentSpatialLocation.Spatial_Lootable;
                SpatialGroupsLocationSpatial_ChanceNUD.Value = currentSpatialLocation.Spatial_Chance;
                SpatialGroupsLocationSpatial_MinAccuracyNUD.Value = currentSpatialLocation.Spatial_MinAccuracy;
                SpatialGroupsLocationSpatial_MaxAccuracyNUD.Value = currentSpatialLocation.Spatial_MaxAccuracy;
                SpatialGroupsLocationSpatial_TimerNUD.Value = currentSpatialLocation.Spatial_Timer;
                SpatialGroupsLocationSpatial_SpawnModeCB.SelectedIndex = currentSpatialLocation.Spatial_SpawnMode;
                SpatialGroupsLocationSpatial_UnlimitedReloadCB.Checked = currentSpatialLocation.Spatial_UnlimitedReload == 1 ? true : false;
                SpatialGroupsLocationSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.X;
                SpatialGroupsLocationSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.Y;
                SpatialGroupsLocationSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.Z;

                SpatialGroupsSpawnPositionLB.DataSource = currentSpatialLocation._Spatial_SpawnPosition;
            }
            else if (SpatialGroupsLB.SelectedItem is Spatial_Audio)
            {
                currentSpatialAudio = SpatialGroupsLB.SelectedItem as Spatial_Audio;
                SpatialAudioGB.Visible = true;
                SpawnPositionGB.Visible = true;
                SpawnPositionGB.Height = 496;
                SpatialGroupImportGB.Visible = true;
                SpatialGroupImportGB.Location = new Point(787, 6);
                SpatialGroupsAudioSpatial_NameTB.Text = currentSpatialAudio.Spatial_Name;
                SpatialGroupsAudioSpatial_TriggerRadiusNUD.Value = currentSpatialAudio.Spatial_TriggerRadius;
                SpatialGroupsAudioSpatial_ZoneLoadoutCB.SelectedIndex = SpatialGroupsAudioSpatial_ZoneLoadoutCB.FindStringExact(currentSpatialAudio.Spatial_ZoneLoadout);
                SpatialGroupsAudioSpatial_MinCountNUD.Value = currentSpatialAudio.Spatial_MinCount;
                SpatialGroupsAudioSpatial_MaxCountNUD.Value = currentSpatialAudio.Spatial_MaxCount;
                SpatialGroupsAudioSpatial_HuntModeCB.SelectedIndex = currentSpatialAudio.Spatial_HuntMode - 1;
                SpatialGroupsAudioSpatial_FactionCB.SelectedIndex = SpatialGroupsAudioSpatial_FactionCB.FindStringExact(currentSpatialAudio.Spatial_Faction);
                SpatialGroupsAudioSpatial_LootableCB.SelectedIndex = currentSpatialAudio.Spatial_Lootable;
                SpatialGroupsAudioSpatial_ChanceNUD.Value = currentSpatialAudio.Spatial_Chance;
                SpatialGroupsAudioSpatial_MinAccuracyNUD.Value = currentSpatialAudio.Spatial_MinAccuracy;
                SpatialGroupsAudioSpatial_MaxAccuracyNUD.Value = currentSpatialAudio.Spatial_MaxAccuracy;
                SpatialGroupsAudioSpatial_TimerNUD.Value = currentSpatialAudio.Spatial_Timer;
                SpatialGroupsAudioSpatial_SensitivityNUD.Value = currentSpatialAudio.Spatial_Sensitivity;
                SpatialGroupsAudioSpatial_SpawnModeCB.SelectedIndex = currentSpatialAudio.Spatial_SpawnMode;
                SpatialGroupsAudioSpatial_UnlimitedReloadCB.Checked = currentSpatialAudio.Spatial_UnlimitedReload == 1 ? true : false;
                SpatialGroupsAudioSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.X;
                SpatialGroupsAudioSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Y;
                SpatialGroupsAudioSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Z;

                SpatialGroupsSpawnPositionLB.DataSource = currentSpatialAudio._Spatial_SpawnPosition;
            }
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void darkButton27_Click(object sender, EventArgs e)
        {
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 0:
                    Spatial_Group newgroup = new Spatial_Group()
                    {
                        Spatial_Name = "New Group",
                        Spatial_MinCount = 0,
                        Spatial_MaxCount = 2,
                        Spatial_Weight = 300,
                        Spatial_Loadout = "HumanLoadout.json",
                        Spatial_Faction = "Mercenaries",
                        Spatial_Lootable = 1,
                        Spatial_Chance = (decimal)0.3,
                        Spatial_MinAccuracy = (decimal)0.5,
                        Spatial_MaxAccuracy = (decimal)0.8,
                        Spatial_UnlimitedReload = 1
                    };
                    m_Spatial_Groups.Group.Add(newgroup);
                    break;
                case 1:
                    Spatial_Point newpoint = new Spatial_Point()
                    {
                        Spatial_Name = "New point",
                        Spatial_Radius = 100,
                        Spatial_ZoneLoadout = new BindingList<string>()
                        {
                            "HumanLoadout.json"
                        },
                        Spatial_MinCount = 0,
                        Spatial_MaxCount = 2,
                        Spatial_HuntMode = 5,
                        Spatial_Faction = "Mercenaries",
                        Spatial_Lootable = 1,
                        Spatial_Chance = (decimal)0.3,
                        Spatial_MinAccuracy = (decimal)0.5,
                        Spatial_MaxAccuracy = (decimal)0.8,
                        Spatial_UnlimitedReload = 1,
                        _Spatial_Position = new Vec3(0, 0, 0)
                    };
                    m_Spatial_Groups.Point.Add(newpoint);
                    break;
                case 2:
                    Spatial_Location newlocation = new Spatial_Location()
                    {
                        Spatial_Name = "New point",
                        Spatial_TriggerRadius = 100,
                        Spatial_ZoneLoadout = "HumanLoadout.json",
                        Spatial_MinCount = 0,
                        Spatial_MaxCount = 2,
                        Spatial_HuntMode = 5,
                        Spatial_Faction = "Mercenaries",
                        Spatial_Lootable = 1,
                        Spatial_Chance = (decimal)0.3,
                        Spatial_MinAccuracy = (decimal)0.5,
                        Spatial_MaxAccuracy = (decimal)0.8,
                        Spatial_Timer = 120,
                        Spatial_SpawnMode = 1,
                        Spatial_UnlimitedReload = 1,
                        _Spatial_TriggerPosition = new Vec3(0, 0, 0)
                    };
                    m_Spatial_Groups.Location.Add(newlocation);
                    break;
                case 3:
                    Spatial_Audio newAudio = new Spatial_Audio()
                    {
                        Spatial_Name = "New point",
                        Spatial_TriggerRadius = 100,
                        Spatial_ZoneLoadout = "HumanLoadout.json",
                        Spatial_MinCount = 0,
                        Spatial_MaxCount = 2,
                        Spatial_HuntMode = 5,
                        Spatial_Faction = "Mercenaries",
                        Spatial_Lootable = 1,
                        Spatial_Chance = (decimal)0.3,
                        Spatial_MinAccuracy = (decimal)0.5,
                        Spatial_MaxAccuracy = (decimal)0.8,
                        Spatial_Timer = 120,
                        Spatial_Sensitivity = 3,
                        Spatial_SpawnMode = 1,
                        Spatial_UnlimitedReload = 1,
                        _Spatial_TriggerPosition = new Vec3(0, 0, 0)
                    };
                    m_Spatial_Groups.Audio.Add(newAudio);
                    break;
            }
            m_Spatial_Groups.isDirty = true; ;
            SpatialGroupsLB.Invalidate();
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 0:
                    m_Spatial_Groups.Group.Remove(currentSpatialGroup);
                    break;
                case 1:
                    m_Spatial_Groups.Point.Remove(currentSpatialPoint);
                    break;
                case 2:
                    m_Spatial_Groups.Location.Remove(currentSpatialLocation);
                    break;
                case 3:
                    m_Spatial_Groups.Audio.Remove(currentSpatialAudio);
                    break;
            }
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsSpawnPositionLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpatialGroupsSpawnPositionLB.SelectedItems.Count < 1) return;
            currentspawnPosition = SpatialGroupsSpawnPositionLB.SelectedItem as Vec3;
            useraction = false;
            SpatialGroupsSpatial_SpawnPositionXNUD.Value = (decimal)currentspawnPosition.X;
            SpatialGroupsSpatial_SpawnPositionYNUD.Value = (decimal)currentspawnPosition.Y;
            SpatialGroupsSpatial_SpawnPositionZNUD.Value = (decimal)currentspawnPosition.Z;
            useraction = true;
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            Vec3 newvec3 = new Vec3(0, 0, 0);
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 2:
                    currentSpatialLocation._Spatial_SpawnPosition.Add(newvec3);
                    break;
                case 3:
                    currentSpatialAudio._Spatial_SpawnPosition.Add(newvec3);
                    break;
            }
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 2:
                    currentSpatialLocation._Spatial_SpawnPosition.Remove(currentspawnPosition);
                    break;
                case 3:
                    currentSpatialAudio._Spatial_SpawnPosition.Remove(currentspawnPosition);
                    break;
            }
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsSpatial_SpawnPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentspawnPosition.X = (float)SpatialGroupsSpatial_SpawnPositionXNUD.Value;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsSpatial_SpawnPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentspawnPosition.Y = (float)SpatialGroupsSpatial_SpawnPositionYNUD.Value;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsSpatial_SpawnPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentspawnPosition.Z = (float)SpatialGroupsSpatial_SpawnPositionZNUD.Value;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }

        #endregion dynamic AI Spatial groups
        #region draw AI Spatial Groups
        public int DynamicAIPatrolScale = 1;
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            DynamicAIPatrolScale = trackBar1.Value;
            SetDynamicAIScale();
        }
        private void SetDynamicAIScale()
        {
            float scalevalue = DynamicAIPatrolScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawDynamicAI(object sender, PaintEventArgs e)
        {
            if (checkBox9.Checked == false)
            {
                switch (SpatialGroupsCB.SelectedIndex)
                {
                    case 0:
                        return;
                    case 1:
                        if (checkBox8.Checked == true)
                        {
                            foreach (Spatial_Point sp in m_Spatial_Groups.Point)
                            {
                                float scalevalue = DynamicAIPatrolScale * 0.05f;
                                int centerX = (int)(Math.Round(sp._Spatial_Position.X) * scalevalue);
                                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_Position.Z, 0) * scalevalue);
                                int eventradius = (int)((float)sp.Spatial_Radius * scalevalue);
                                Point center = new Point(centerX, centerY);
                                Pen pen = new Pen(Color.Red, 4);
                                if (sp == currentSpatialPoint)
                                    pen = new Pen(Color.Green, 4);
                                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + sp.Spatial_Name);
                            }
                        }
                        else
                        {
                            float scalevalue = DynamicAIPatrolScale * 0.05f;
                            int centerX = (int)(Math.Round(currentSpatialPoint._Spatial_Position.X) * scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(currentSpatialPoint._Spatial_Position.Z, 0) * scalevalue);
                            int eventradius = (int)((float)currentSpatialPoint.Spatial_Radius * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Green, 4);
                            getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + currentSpatialPoint.Spatial_Name);
                        }
                        break;
                    case 2:
                        if (checkBox8.Checked == true)
                        {
                            foreach (Spatial_Location sp in m_Spatial_Groups.Location)
                            {
                                float scalevalue = DynamicAIPatrolScale * 0.05f;
                                int centerX = (int)(Math.Round(sp._Spatial_TriggerPosition.X) * scalevalue);
                                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_TriggerPosition.Z, 0) * scalevalue);
                                int eventradius = (int)((float)sp.Spatial_TriggerRadius * scalevalue);
                                Point center = new Point(centerX, centerY);
                                Pen pen = new Pen(Color.Red, 4);
                                if (sp == currentSpatialLocation)
                                    pen = new Pen(Color.Green, 4);
                                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + sp.Spatial_Name);
                                foreach (Vec3 v3 in sp._Spatial_SpawnPosition)
                                {
                                    scalevalue = DynamicAIPatrolScale * 0.05f;
                                    centerX = (int)(Math.Round(v3.X) * scalevalue);
                                    centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                                    eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                                    center = new Point(centerX, centerY);
                                    pen = new Pen(Color.Yellow, 4);
                                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                                }
                            }
                        }
                        else
                        {
                            float scalevalue = DynamicAIPatrolScale * 0.05f;
                            int centerX = (int)(Math.Round(currentSpatialLocation._Spatial_TriggerPosition.X) * scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(currentSpatialLocation._Spatial_TriggerPosition.Z, 0) * scalevalue);
                            int eventradius = (int)((float)currentSpatialLocation.Spatial_TriggerRadius * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Green, 4);
                            getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + currentSpatialLocation.Spatial_Name);
                            foreach (Vec3 v3 in currentSpatialLocation._Spatial_SpawnPosition)
                            {
                                scalevalue = DynamicAIPatrolScale * 0.05f;
                                centerX = (int)(Math.Round(v3.X) * scalevalue);
                                centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                                eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                                center = new Point(centerX, centerY);
                                pen = new Pen(Color.Yellow, 4);
                                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                            }
                        }
                        break;
                    case 3:
                        if (checkBox8.Checked == true)
                        {
                            foreach (Spatial_Audio sp in m_Spatial_Groups.Audio)
                            {
                                float scalevalue = DynamicAIPatrolScale * 0.05f;
                                int centerX = (int)(Math.Round(sp._Spatial_TriggerPosition.X) * scalevalue);
                                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_TriggerPosition.Z, 0) * scalevalue);
                                int eventradius = (int)((float)sp.Spatial_TriggerRadius * scalevalue);
                                Point center = new Point(centerX, centerY);
                                Pen pen = new Pen(Color.Red, 4);
                                if (sp == currentSpatialAudio)
                                    pen = new Pen(Color.Green, 4);
                                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + sp.Spatial_Name);
                                foreach (Vec3 v3 in sp._Spatial_SpawnPosition)
                                {
                                    scalevalue = DynamicAIPatrolScale * 0.05f;
                                    centerX = (int)(Math.Round(v3.X) * scalevalue);
                                    centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                                    eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                                    center = new Point(centerX, centerY);
                                    pen = new Pen(Color.Yellow, 4);
                                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                                }
                            }
                        }
                        else
                        {
                            float scalevalue = DynamicAIPatrolScale * 0.05f;
                            int centerX = (int)(Math.Round(currentSpatialAudio._Spatial_TriggerPosition.X) * scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(currentSpatialAudio._Spatial_TriggerPosition.Z, 0) * scalevalue);
                            int eventradius = (int)((float)currentSpatialAudio.Spatial_TriggerRadius * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Green, 4);
                            getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + currentSpatialAudio.Spatial_Name);
                            foreach (Vec3 v3 in currentSpatialAudio._Spatial_SpawnPosition)
                            {
                                scalevalue = DynamicAIPatrolScale * 0.05f;
                                centerX = (int)(Math.Round(v3.X) * scalevalue);
                                centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                                eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                                center = new Point(centerX, centerY);
                                pen = new Pen(Color.Yellow, 4);
                                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                            }
                        }
                        break;
                }
            }
            else if (checkBox9.Checked == true)
            {
                foreach (Spatial_Point sp in m_Spatial_Groups.Point)
                {
                    float scalevalue = DynamicAIPatrolScale * 0.05f;
                    int centerX = (int)(Math.Round(sp._Spatial_Position.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_Position.Z, 0) * scalevalue);
                    int eventradius = (int)((float)sp.Spatial_Radius * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    if (sp == currentSpatialPoint)
                        pen = new Pen(Color.Green, 4);
                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "Point\n" + sp.Spatial_Name);
                }
                foreach (Spatial_Location sp in m_Spatial_Groups.Location)
                {
                    float scalevalue = DynamicAIPatrolScale * 0.05f;
                    int centerX = (int)(Math.Round(sp._Spatial_TriggerPosition.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_TriggerPosition.Z, 0) * scalevalue);
                    int eventradius = (int)((float)sp.Spatial_TriggerRadius * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    if (sp == currentSpatialLocation)
                        pen = new Pen(Color.Green, 4);
                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "Location\n" + sp.Spatial_Name);
                    foreach (Vec3 v3 in sp._Spatial_SpawnPosition)
                    {
                        scalevalue = DynamicAIPatrolScale * 0.05f;
                        centerX = (int)(Math.Round(v3.X) * scalevalue);
                        centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                        eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                        center = new Point(centerX, centerY);
                        pen = new Pen(Color.Yellow, 4);
                        getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                    }
                }
                foreach (Spatial_Audio sp in m_Spatial_Groups.Audio)
                {
                    float scalevalue = DynamicAIPatrolScale * 0.05f;
                    int centerX = (int)(Math.Round(sp._Spatial_TriggerPosition.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp._Spatial_TriggerPosition.Z, 0) * scalevalue);
                    int eventradius = (int)((float)sp.Spatial_TriggerRadius * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    if (sp == currentSpatialAudio)
                        pen = new Pen(Color.Green, 4);
                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "Audio\n" + sp.Spatial_Name);
                    foreach (Vec3 v3 in sp._Spatial_SpawnPosition)
                    {
                        scalevalue = DynamicAIPatrolScale * 0.05f;
                        centerX = (int)(Math.Round(v3.X) * scalevalue);
                        centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Z, 0) * scalevalue);
                        eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                        center = new Point(centerX, centerY);
                        pen = new Pen(Color.Yellow, 4);
                        getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                    }
                }
            }
        }
        private void getCircleDynamicAI(Graphics drawingArea, Pen penToUse, Point center, int radius, string c)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
            Rectangle rect3 = new Rectangle(center.X - radius, center.Y - (radius + 30), 200, 40);
            drawingArea.DrawString(c, new Font("Tahoma", 9), Brushes.White, rect3);
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panelEx1.AutoScrollPosition.X - changePoint.X, -panelEx1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
            decimal scalevalue = AIPatrolScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label2.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox1_ZoomOut();
            }
            else
            {
                pictureBox1_ZoomIn();
            }

        }
        private void pictureBox1_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            DynamicAIPatrolScale = trackBar1.Value;
            SetDynamicAIScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            DynamicAIPatrolScale = trackBar1.Value;
            SetDynamicAIScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        #endregion draw Ai Spatial Groups

        private void SpatialGroupsGroupSpatial_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Name = SpatialGroupsGroupSpatial_NameTB.Text;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsGroupSpatial_MinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_MinCount = (int)SpatialGroupsGroupSpatial_MinCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_MaxCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_MaxCount = (int)SpatialGroupsGroupSpatial_MaxCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_WeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Weight = SpatialGroupsGroupSpatial_WeightNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_LoadoutCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Loadout = SpatialGroupsGroupSpatial_LoadoutCB.GetItemText(SpatialGroupsGroupSpatial_LoadoutCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Faction = SpatialGroupsGroupSpatial_FactionCB.GetItemText(SpatialGroupsGroupSpatial_FactionCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_LootableCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Lootable = SpatialGroupsGroupSpatial_LootableCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_ChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_Weight = SpatialGroupsGroupSpatial_ChanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_MinAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_MinAccuracy = SpatialGroupsGroupSpatial_MinAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;

        }
        private void SpatialGroupsGroupSpatial_MaxAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_MaxAccuracy = SpatialGroupsGroupSpatial_MaxAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsGroupSpatial_UnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialGroup.Spatial_UnlimitedReload = SpatialGroupsGroupSpatial_UnlimitedReloadCB.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }

        private void SpatialGroupsPointSpatial_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Name = SpatialGroupsPointSpatial_NameTB.Text;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsPointSpatial_SafeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Safe = SpatialGroupsPointSpatial_SafeCB.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_RadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Radius = SpatialGroupsPointSpatial_RadiusNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            AddfromLoadoutList form = new AddfromLoadoutList();
            form.LoadoutLIst = LoadoutNameList;
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (string l in form.SelectedLoadouts)
                {
                    currentSpatialPoint.Spatial_ZoneLoadout.Add(l);
                    SpatialGroupsSpatial_ZoneLoadoutLB.Invalidate();
                }
                m_Spatial_Groups.isDirty = true;
            }
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            if (SpatialGroupsSpatial_ZoneLoadoutLB.SelectedItems.Count < 1) return;
            currentSpatialPoint.Spatial_ZoneLoadout.Remove(SpatialGroupsSpatial_ZoneLoadoutLB.GetItemText(SpatialGroupsSpatial_ZoneLoadoutLB.SelectedItem));
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_MinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_MinCount = (int)SpatialGroupsPointSpatial_MinCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_MaxCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_MaxCount = (int)SpatialGroupsPointSpatial_MaxCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_HuntModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_HuntMode = SpatialGroupsPointSpatial_HuntModeCB.SelectedIndex + 1;
            m_Spatial_Groups.isDirty = true;

        }
        private void SpatialGroupsPointSpatial_FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Faction = SpatialGroupsPointSpatial_FactionCB.GetItemText(SpatialGroupsPointSpatial_FactionCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_LootableCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Lootable = SpatialGroupsPointSpatial_LootableCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_ChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_Chance = SpatialGroupsPointSpatial_ChanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_MinAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_MinAccuracy = SpatialGroupsPointSpatial_MinAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_Max_AccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_MaxAccuracy = SpatialGroupsPointSpatial_Max_AccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_UnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint.Spatial_UnlimitedReload = SpatialGroupsPointSpatial_UnlimitedReloadCB.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_PositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint._Spatial_Position.X = (float)SpatialGroupsPointSpatial_PositionXNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_PositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint._Spatial_Position.Y = (float)SpatialGroupsPointSpatial_PositionYNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsPointSpatial_PositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialPoint._Spatial_Position.Z = (float)SpatialGroupsPointSpatial_PositionZNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }


        private void SpatialGroupsLocationSpatial_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_Name = SpatialGroupsLocationSpatial_NameTB.Text;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsLocationSpatial_TriggerRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_TriggerRadius = SpatialGroupsLocationSpatial_TriggerRadiusNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_ZoneLoadoutCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_ZoneLoadout = SpatialGroupsLocationSpatial_ZoneLoadoutCB.GetItemText(SpatialGroupsLocationSpatial_ZoneLoadoutCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_MinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_MinCount = (int)SpatialGroupsLocationSpatial_MinCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_MaxCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_MaxCount = (int)SpatialGroupsLocationSpatial_MaxCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_HuntModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_HuntMode = SpatialGroupsLocationSpatial_HuntModeCB.SelectedIndex + 1;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_Faction = SpatialGroupsLocationSpatial_FactionCB.GetItemText(SpatialGroupsLocationSpatial_FactionCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_LootableCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_Lootable = SpatialGroupsLocationSpatial_LootableCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_ChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_MinAccuracy = SpatialGroupsLocationSpatial_MinAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_MinAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_MinAccuracy = SpatialGroupsLocationSpatial_MinAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_MaxAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_MaxAccuracy = SpatialGroupsLocationSpatial_MaxAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_TimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_Timer = SpatialGroupsLocationSpatial_TimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_SpawnModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_Lootable = SpatialGroupsLocationSpatial_SpawnModeCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_UnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation.Spatial_UnlimitedReload = SpatialGroupsLocationSpatial_UnlimitedReloadCB.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_TriggerPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation._Spatial_TriggerPosition.X = (float)SpatialGroupsLocationSpatial_TriggerPositionXNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_TriggerPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation._Spatial_TriggerPosition.Y = (float)SpatialGroupsLocationSpatial_TriggerPositionYNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsLocationSpatial_TriggerPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialLocation._Spatial_TriggerPosition.Z = (float)SpatialGroupsLocationSpatial_TriggerPositionZNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }

        private void SpatialGroupsAudioSpatial_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Name = SpatialGroupsAudioSpatial_NameTB.Text;
            m_Spatial_Groups.isDirty = true;
            SpatialGroupsLB.Invalidate();
        }
        private void SpatialGroupsAudioSpatial_TriggerRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_TriggerRadius = SpatialGroupsAudioSpatial_TriggerRadiusNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_ZoneLoadoutCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_ZoneLoadout = SpatialGroupsAudioSpatial_ZoneLoadoutCB.GetItemText(SpatialGroupsAudioSpatial_ZoneLoadoutCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_MinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_MinCount = (int)SpatialGroupsAudioSpatial_MinCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_MaxCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_MaxCount = (int)SpatialGroupsAudioSpatial_MaxCountNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_HuntModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_HuntMode = SpatialGroupsAudioSpatial_HuntModeCB.SelectedIndex = 1;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Faction = SpatialGroupsAudioSpatial_FactionCB.GetItemText(SpatialGroupsAudioSpatial_FactionCB.SelectedItem);
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_LootableCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Lootable = SpatialGroupsAudioSpatial_LootableCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_ChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Chance = SpatialGroupsAudioSpatial_ChanceNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_MinAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_MinAccuracy = SpatialGroupsAudioSpatial_MinAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_MaxAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_MaxAccuracy = SpatialGroupsAudioSpatial_MaxAccuracyNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_TimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Timer = SpatialGroupsAudioSpatial_TimerNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_SensitivityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_Sensitivity = SpatialGroupsAudioSpatial_SensitivityNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_SpawnModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_SpawnMode = SpatialGroupsAudioSpatial_SpawnModeCB.SelectedIndex;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_UnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio.Spatial_UnlimitedReload = SpatialGroupsAudioSpatial_UnlimitedReloadCB.Checked == true ? 1 : 0;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_TriggerPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio._Spatial_TriggerPosition.X = (float)SpatialGroupsAudioSpatial_TriggerPositionXNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_TriggerPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio._Spatial_TriggerPosition.Y = (float)SpatialGroupsAudioSpatial_TriggerPositionYNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }
        private void SpatialGroupsAudioSpatial_TriggerPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSpatialAudio._Spatial_TriggerPosition.Z = (float)SpatialGroupsAudioSpatial_TriggerPositionZNUD.Value;
            m_Spatial_Groups.isDirty = true;
        }

        private void darkButton35_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    switch (SpatialGroupsCB.SelectedIndex)
                    {
                        case 0:
                            MessageBox.Show("Spatial Groups dont have a trigger position or and spawn points.");
                            return;
                        case 1:
                            m_Spatial_Groups.AddnewPoint(importfile);
                            break;
                        case 2:
                            m_Spatial_Groups.AddnewLocation(importfile);
                            break;
                        case 3:
                            m_Spatial_Groups.AddNewAudio(importfile);
                            break;
                    }
                    m_Spatial_Groups.isDirty = true;
                    SpatialGroupsLB.Invalidate();
                }
            }
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    bool ImportTrigger = false;
                    switch (SpatialGroupsCB.SelectedIndex)
                    {
                        case 0:
                            MessageBox.Show("Spatial Groups dont have a trigger position or and spawn points.");
                            return;
                        case 1:
                            currentSpatialPoint.ImportDZE(importfile);
                            SpatialGroupsPointSpatial_PositionXNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.X;
                            SpatialGroupsPointSpatial_PositionYNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.Y;
                            SpatialGroupsPointSpatial_PositionZNUD.Value = (decimal)currentSpatialPoint._Spatial_Position.Z;
                            break;
                        case 2:
                            var result = MessageBox.Show("Would you like to import the trigger as well?", "Import options", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                                ImportTrigger = true;
                            result = MessageBox.Show("Would you like to clear existing Spawn Points??", "Import options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if ((result == DialogResult.Cancel))
                            {
                                return;
                            }
                            else if (result == DialogResult.Yes)
                            {
                                currentSpatialLocation._Spatial_SpawnPosition = new BindingList<Vec3>();
                            }
                            currentSpatialLocation.ImportDZE(importfile, ImportTrigger);
                            SpatialGroupsLocationSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.X;
                            SpatialGroupsLocationSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.Y;
                            SpatialGroupsLocationSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialLocation._Spatial_TriggerPosition.Z;
                            SpatialGroupsSpawnPositionLB.DataSource = currentSpatialLocation._Spatial_SpawnPosition;
                            break;
                        case 3:
                            result = MessageBox.Show("Would you like to import the trigger as well?", "Import options", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                                ImportTrigger = true;
                            result = MessageBox.Show("Would you like to clear existing Spawn Points??", "Import options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if ((result == DialogResult.Cancel))
                            {
                                return;
                            }
                            else if (result == DialogResult.Yes)
                            {
                                currentSpatialAudio._Spatial_SpawnPosition = new BindingList<Vec3>();
                            }
                            currentSpatialAudio.ImportDZE(importfile, ImportTrigger);
                            SpatialGroupsAudioSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.X;
                            SpatialGroupsAudioSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Y;
                            SpatialGroupsAudioSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Z;
                            SpatialGroupsSpawnPositionLB.DataSource = currentSpatialAudio._Spatial_SpawnPosition;
                            break;
                    }
                    m_Spatial_Groups.isDirty = true;
                }
            }
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            int m_Id = 0;
            string filename = "";
            switch (SpatialGroupsCB.SelectedIndex)
            {
                case 0:
                    MessageBox.Show("Spatial Groups dont have a trigger position or and spawn points.");
                    return;
                case 1:
                    Editorobject Triggerobject = new Editorobject()
                    {
                        Type = "GiftBox_Large_1",
                        DisplayName = "GiftBox_Large_1",
                        Position = currentSpatialPoint._Spatial_Position.getfloatarray(),
                        Orientation = new float[] { 0, 0, 0 },
                        Scale = 1.0f,
                        Model = "",
                        Flags = 2147483647,
                        m_Id = m_Id
                    };
                    newdze.EditorObjects.Add(Triggerobject);
                    m_Id++;
                    filename = "SpatialPoint_" + currentSpatialPoint.Spatial_Name;
                    break;
                case 2:
                    var result = MessageBox.Show("Would yo ulike to export the trigger as well?", "Export options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        Triggerobject = new Editorobject()
                        {
                            Type = "GiftBox_Large_1",
                            DisplayName = "GiftBox_Large_1",
                            Position = currentSpatialLocation._Spatial_TriggerPosition.getfloatarray(),
                            Orientation = new float[] { 0, 0, 0 },
                            Scale = 1.0f,
                            Model = "",
                            Flags = 2147483647,
                            m_Id = m_Id
                        };
                        newdze.EditorObjects.Add(Triggerobject);
                        m_Id++;
                    }
                    foreach (Vec3 vec3 in currentSpatialLocation._Spatial_SpawnPosition)
                    {
                        Editorobject SpawnObject = new Editorobject()
                        {
                            Type = "GiftBox_Small_1",
                            DisplayName = "GiftBox_Small_1",
                            Position = vec3.getfloatarray(),
                            Orientation = new float[] { 0, 0, 0 },
                            Scale = 1.0f,
                            Model = "",
                            Flags = 2147483647,
                            m_Id = m_Id
                        };
                        newdze.EditorObjects.Add(SpawnObject);
                        m_Id++;
                    }
                    filename = "SpatialLocation_" + currentSpatialLocation.Spatial_Name;
                    break;
                case 3:
                    result = MessageBox.Show("Would yo ulike to export the trigger as well?", "Export options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        Triggerobject = new Editorobject()
                        {
                            Type = "GiftBox_Large_1",
                            DisplayName = "GiftBox_Large_1",
                            Position = currentSpatialAudio._Spatial_TriggerPosition.getfloatarray(),
                            Orientation = new float[] { 0, 0, 0 },
                            Scale = 1.0f,
                            Model = "",
                            Flags = 2147483647,
                            m_Id = m_Id
                        };
                        newdze.EditorObjects.Add(Triggerobject);
                        m_Id++;
                    }
                    foreach (Vec3 vec3 in currentSpatialAudio._Spatial_SpawnPosition)
                    {
                        Editorobject SpawnObject = new Editorobject()
                        {
                            Type = "GiftBox_Small_1",
                            DisplayName = "GiftBox_Small_1",
                            Position = vec3.getfloatarray(),
                            Orientation = new float[] { 0, 0, 0 },
                            Scale = 1.0f,
                            Model = "",
                            Flags = 2147483647,
                            m_Id = m_Id
                        };
                        newdze.EditorObjects.Add(SpawnObject);
                        m_Id++;
                    }
                    filename = "SpatialLocation_" + currentSpatialAudio.Spatial_Name;
                    break;

            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = filename;
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void FactionEditLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FactionEditLB.SelectedItems.Count < 1) return;
            useraction = false;
            FactionEditTB.Text = FactionEditLB.GetItemText(FactionEditLB.SelectedItem);
            useraction = true;
        }
        private void FactionEditTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Factions[FactionEditLB.SelectedIndex] = FactionEditTB.Text;
        }
        private void darkButton36_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in form.addedtypes)
                {
                    if (!Factions.Contains(s))
                        Factions.Add(s);
                }
                FactionEditLB.Refresh();
            }
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            Factions.Remove(FactionEditLB.GetItemText(FactionEditLB.SelectedItem));
            FactionEditLB.Refresh();
        }
        private void darkButton37_Click(object sender, EventArgs e)
        {
            SetupFactionsDropDownBoxes();
            File.WriteAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt", Factions);
        }


    }
}
