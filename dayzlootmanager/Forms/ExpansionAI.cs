using DarkUI.Forms;
using DayZeLib;
using SevenZipExtractor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using TreeViewMS;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DayZeEditor
{

    public partial class ExpansionAI : DarkForm
    {
        public Project currentproject { get; internal set; }
        public bool IsDynamicAiLoaded = false;
        public int AIPatrolScale = 1;
        public ExpansionAISettings AISettings { get; set; }
        public ExpansionAILocationSettings ExpansionAILocationSettings { get; set; }
        public ExpansionAIPatrolSettings AIPatrolSettings { get; set; }
        public AILoadoutsConfig AILoadoutsConfig { get; set; }
       
        public BindingList<string> LoadoutNameList { get; private set; }
        public BindingList<string> LoadoutNameList2 { get; private set; }
        public BindingList<string> LootDropOnDeathNameList { get; private set; }
        public BindingList<string> Factions { get; set; }
        public MapData MapData { get; private set; }
        public Spatial_Notifications m_Spatial_Notifications { get; set; }
        public Spatial_Players m_Spatial_Players { get; set; }
        public Spatial_Groups m_Spatial_Groups { get; set; }
        public string AISettingsPath;
        public string AILoadoutsPath;
        public string LootDropOnDeathListPath;
        public string AIPatrolSettingsPath;
        public string AILocationsPath;
        public string AIDynamicSettingsPath;
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        private bool useraction;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
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
        ~ExpansionAI()
        {
        }
        private void ExpansionAI_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();
            Factions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt").ToList());
            FactionEditLB.DataSource = Factions;
            SetupFactionsDropDownBoxes();


            bool needtosave = false;

            AILoadoutsConfig = new AILoadoutsConfig();

            AILoadoutsConfig.LoadoutsData = new BindingList<AILoadouts>();
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
                    AILoadoutsConfig.LoadoutsData.Add(AILoadouts);
                }
                catch { }
            }

            AILoadoutsConfig.AILootDropsData = new BindingList<AILootDrops>();
            LootDropOnDeathListPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\LootDrops";
            DirectoryInfo dinfo1 = new DirectoryInfo(LootDropOnDeathListPath);
            FileInfo[] Files1 = dinfo1.GetFiles("*.json");
            foreach (FileInfo file in Files1)
            {
                try
                {
                    Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
                    AILootDrops AILootDrops = new AILootDrops();
                    AILootDrops.LootdropList = JsonSerializer.Deserialize<BindingList<AILoadouts>>(File.ReadAllText(file.FullName));
                    AILootDrops.Filename = file.FullName;
                    AILootDrops.Setname();
                    AILootDrops.isDirty = false;
                    AILoadoutsConfig.AILootDropsData.Add(AILootDrops);
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
            AISettings.createlistfromdict();
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
                if(AIPatrolSettings.SetLoadBalancingCategoriestoList())
                    needtosave= true;
                if (AIPatrolSettings.checkver())
                    needtosave = true;
                if (AIPatrolSettings.SetPatrolNames())
                    needtosave = true;


            }
            AIPatrolSettings.GetAIPatrolWaypoints();
            AIPatrolSettings.Filename = AIPatrolSettingsPath;
            SetupAIPatrolSettings();


            AILocationsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\AILocationSettings.json";
            if (!File.Exists(AILocationsPath))
            {
                ExpansionAILocationSettings = new ExpansionAILocationSettings();
                ExpansionAILocationSettings.isDirty = true;
                needtosave = true;
                Console.WriteLine(Path.GetFileName(AILocationsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(AILocationsPath));
                var options = new JsonSerializerOptions
                {
                    Converters = { new BoolConverter() },
                };
                ExpansionAILocationSettings = JsonSerializer.Deserialize<ExpansionAILocationSettings>(File.ReadAllText(AILocationsPath), options);
                ExpansionAILocationSettings.isDirty = false;
                if (ExpansionAILocationSettings.checkver())
                    needtosave = true;
            }
            ExpansionAILocationSettings.Filename = AILocationsPath;
            ExpansionAILocationSettings.GetVec3Points();
            SetupExpansionAILocationSettingss();


            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz", currentproject.MapSize);
            //MapData.loadpoints();

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAIPAtrols);
            trackBar2.Value = 1;
            SetAIPatrolZonescale();

            pictureBox3.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox3.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox3.Paint += new PaintEventHandler(DrawAILocations);
            trackBar4.Value = 1;
            SetAILocationZonescale();

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
            tabControl5.ItemSize = new Size(0, 1);
            toolStripButton8.Checked = true;
            toolStripButton4.Checked = true;
            toolStripButton7.Checked = true;
            toolStripButton12.Checked = true;
            toolStripButton15.Checked = true;
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

            foreach (AILootDrops AILO in AILoadoutsConfig.AILootDropsData)
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
            Form form = sender as Form;
            form.MdiParent = null;
        }
        private void SetupFactionsDropDownBoxes()
        {
            useraction = false;
            PlayerFactionCB.DataSource = new BindingList<string>(Factions);
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
                AISettings.CreateDictionary();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AISettings, options);
                File.WriteAllText(AISettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AISettings.Filename));
            }
            if (AIPatrolSettings.isDirty)
            {
                AIPatrolSettings.SetAIPatrolWaypoints();
                AIPatrolSettings.SetLoadBalancingCategoriestoDictionary();
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

            foreach (AILoadouts AILO in AILoadoutsConfig.LoadoutsData)
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
            if(ExpansionAILocationSettings.isDirty)
            {
                ExpansionAILocationSettings.SetAILocationsPoints();
                if (currentproject.Createbackups && File.Exists(ExpansionAILocationSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ExpansionAILocationSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ExpansionAILocationSettings.Filename, Path.GetDirectoryName(ExpansionAILocationSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ExpansionAILocationSettings.Filename) + ".bak", true);
                }
                ExpansionAILocationSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Converters = { new BoolConverter() } };
                string jsonString = JsonSerializer.Serialize(ExpansionAILocationSettings, options);
                File.WriteAllText(ExpansionAILocationSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(ExpansionAILocationSettings.Filename));
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
            LoadoutNameList = new BindingList<string>
            {
                ""
            };
            foreach (AILoadouts lo in AILoadoutsConfig.LoadoutsData)
            {
                LoadoutNameList.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }
            LootDropOnDeathNameList = new BindingList<string>
            {
                ""
            };
            foreach (AILootDrops AILootDrops in AILoadoutsConfig.AILootDropsData)
            {
               LootDropOnDeathNameList.Add(Path.GetFileNameWithoutExtension(AILootDrops.Name));
            }

            StaticPatrolLB.Refresh();

            StaticPatrolLoadoutsCB.DisplayMember = "DisplayName";
            StaticPatrolLoadoutsCB.ValueMember = "Value";
            StaticPatrolLoadoutsCB.DataSource = new BindingList<string>(LoadoutNameList);

            StaticPatrolLootDropOnDeathCB.DisplayMember = "DisplayName";
            StaticPatrolLootDropOnDeathCB.ValueMember = "Value";
            StaticPatrolLootDropOnDeathCB.DataSource = new BindingList<string>(LootDropOnDeathNameList);

            LoadoutNameList2 = new BindingList<string>();
            foreach (AILoadouts lo in AILoadoutsConfig.LoadoutsData)
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
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            toolStripButton15.AutoSize = true;
            toolStripButton16.AutoSize = true;
            toolStripButton17.AutoSize = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton4.AutoSize = true;
            toolStripButton5.AutoSize = true;
            tabControl1.SelectedIndex = 1;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            toolStripButton7.AutoSize = true;
            toolStripButton9.AutoSize = true;
            toolStripButton10.AutoSize = true;
            toolStripButton11.AutoSize = true;
            tabControl1.SelectedIndex = 2;
        }
        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            LBCGB.Visible = false;
            nameLBCGB.Visible = false;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton14.Checked = false;
            toolStripButton1.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton18.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton8.Checked = true;
                    break;
                case 1:
                    toolStripButton1.Checked = true;
                    break;
                case 2:
                    toolStripButton6.Checked = true;
                    break;
                case 3:
                    toolStripButton14.Checked = true;
                    break;
                case 4:
                    toolStripButton18.Checked = true;
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
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedIndex = 0;
        }
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedIndex = 1;
        }
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedIndex = 2;
        }
        private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton15.Checked = false;
            toolStripButton16.Checked = false;
            toolStripButton17.Checked = false;
            switch (tabControl5.SelectedIndex)
            {
                case 0:
                    toolStripButton15.Checked = true;
                    break;
                case 1:
                    toolStripButton16.Checked = true;
                    break;
                case 2:
                    toolStripButton17.Checked = true;
                    break;
            }
            pictureBox3.Invalidate();
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
                case 4:
                     Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
            }
        }
        #region aipatrolsettings
        public ExpansionAIPatrol CurrentPatrol;
        public Vec3 CurrentWapypoint;
        public Loadbalancingcategorie currentLCB;
        public Loadbalancingcategories currentLCBC;
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
            AIGeneralFormationScaleNUD.Value = AIPatrolSettings.FormationScale;

            SuspendLayout();

            StaticPatrolLB.DisplayMember = "DisplayName";
            StaticPatrolLB.ValueMember = "Value";
            StaticPatrolLB.DataSource = AIPatrolSettings.Patrols;

            ResumeLayout();

            SetupLoadBalancing();
            useraction = true;
        }

        private void SetupLoadBalancing()
        {
            useraction = false;
            LoadBalancingTV.Nodes.Clear();
            TreeNode root = new TreeNode("Load Blanacing Categories")
            {
                Tag = "Parent"
            };
            foreach (Loadbalancingcategorie cat in AIPatrolSettings._LoadBalancingCategories)
            {
                TreeNode LBCategroy = new TreeNode($"Category Name : - {cat.name}")
                {
                    Tag = cat,
                    Name = "LoadBalancingCategory"
                };
                for (int i = 0; i < cat.Categorieslist.Count; i++)
                {
                    LBCategroy.Nodes.Add(new TreeNode($"Load Balancing : {i.ToString()}")
                    {
                        Tag = cat.Categorieslist[i],
                        Name = "Loadbalancingcategories"
                    });
                }
                root.Nodes.Add(LBCategroy);
            }
            LoadBalancingTV.Nodes.Add(root);

        }
        private void LoadBalancingTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currenmttreenode = e.Node;
            LBCGB.Visible = false;
            nameLBCGB.Visible = false;
            if(e.Node.Tag is Loadbalancingcategorie)
            {
                nameLBCGB.Visible = true;
                currentLCB = e.Node.Tag as Loadbalancingcategorie;
                useraction = false;
                NameLBCTB.Text = currentLCB.name;
                useraction = true;
            }
            else if (e.Node.Tag is DayZeLib.Loadbalancingcategories)
            {
                LBCGB.Visible = true;
                currentLCBC = e.Node.Tag as Loadbalancingcategories;
                useraction = false;
                MinPlayersLBCNUD.Value = currentLCBC.MinPlayers;
                MaxPlayersLBCNUD.Value = currentLCBC.MaxPlayers;
                MaxPatrolsLBCNUD.Value = currentLCBC.MaxPatrols;
                useraction = true;
            }
            
        }
        private void NameLBCTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCB.name = NameLBCTB.Text;
            currenmttreenode.Text = $"Category Name : - {currentLCB.name}";
            AIPatrolSettings.isDirty = true;
        }
        private void MinPlayersLBCNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCBC.MinPlayers = (int)MinPlayersLBCNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void MaxPlayersLBCNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCBC.MaxPlayers = (int)MaxPlayersLBCNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void MaxPatrolsLBCNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCBC.MaxPatrols = (int)MaxPatrolsLBCNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void LoadBalancingTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            currenmttreenode = e.Node;
            LoadBalancingTV.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                addNewGroupToolStripMenuItem.Visible = false;
                removeGroupToolStripMenuItem.Visible = false;
                addNewLoadBlancingToolStripMenuItem.Visible = false;
                removeLoadBalancingToolStripMenuItem.Visible = false;
                if (e.Node.Tag.ToString() == "Parent")
                {
                    addNewGroupToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is Loadbalancingcategorie)
                {
                    removeGroupToolStripMenuItem.Visible = true;
                    addNewLoadBlancingToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is DayZeLib.Loadbalancingcategories)
                {
                    removeLoadBalancingToolStripMenuItem.Visible = true;
                }
                contextMenuStrip1.Show(Cursor.Position);
            }
        }
        private void addNewGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loadbalancingcategorie newcat = new Loadbalancingcategorie()
            {
                name = "New Category - Change Me",
                Categorieslist = new BindingList<Loadbalancingcategories>()
            };
            AIPatrolSettings._LoadBalancingCategories.Add(newcat);
            currenmttreenode.Nodes.Add(new TreeNode($"Category Name : - {newcat.name}")
            {
                Tag = newcat,
                Name = "LoadBalancingCategory"
            });
            LoadBalancingTV.SelectedNode = currenmttreenode.Nodes[currenmttreenode.Nodes.Count - 1];
            AIPatrolSettings.isDirty = true;
        }
        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AIPatrolSettings._LoadBalancingCategories.Remove(currentLCB);
            currenmttreenode.Parent.Nodes.Remove(currenmttreenode);
            AIPatrolSettings.isDirty = true;

        }
        private void addNewLoadBlancingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentLCB.Categorieslist.Add(new Loadbalancingcategories()
            {
                MinPlayers = 0,
                MaxPlayers = 255,
                MaxPatrols = -1
            });
            currenmttreenode.Nodes.Add(new TreeNode($"Load Balancing : {(currentLCB.Categorieslist.Count -1).ToString()}")
            {
                Tag = currentLCB.Categorieslist.Last(),
                Name = "Loadbalancingcategories"
            });
            LoadBalancingTV.SelectedNode = currenmttreenode.Nodes[currenmttreenode.Nodes.Count - 1];
            AIPatrolSettings.isDirty = true;
        }
        private void removeLoadBalancingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (currenmttreenode.Parent.Tag as Loadbalancingcategorie).Categorieslist.Remove(currentLCBC);
            currenmttreenode.Parent.Nodes.Remove(currenmttreenode);
            AIPatrolSettings.isDirty = true;
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
        private void AIGeneralFormationScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.FormationScale = AIGeneralFormationScaleNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void NoiseInvestigationDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.NoiseInvestigationDistanceLimit = AIGeneralNoiseInvestigationDistanceLimitNUD.Value;
            AIPatrolSettings.isDirty = true;
        }

        /// <summary>
        /// Stataic Patrol Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string UpdateCheckedItemsString(CheckedListBox list, int changingIndex, CheckState newState)
        {
            List<string> selected = new List<string>();
            for (int i = 0; i < list.Items.Count; i++)
            {
                bool isChecked = (i == changingIndex) ? (newState == CheckState.Checked) : list.GetItemChecked(i);
                if (isChecked)
                {
                    selected.Add(list.Items[i].ToString());
                }
            }
            return string.Join(" | ", selected);
        }
        private void StaticPatrolLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolLB.SelectedItems.Count < 1) return;
            useraction = false;
            CurrentPatrol = StaticPatrolLB.SelectedItem as ExpansionAIPatrol;
            StaticPatrolLoadBalancingCategoryCB.DataSource = new BindingSource(AIPatrolSettings._LoadBalancingCategories, null);
            StaticPatrolNameTB.Text = CurrentPatrol.Name;
            textBox6.Text = CurrentPatrol.ObjectClassName;
            StaticPatrolPersistCB.Checked = CurrentPatrol.Persist == 1 ? true : false;
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
            StaticPatrolLoadoutsCB.SelectedIndex = StaticPatrolLoadoutsCB.FindStringExact(CurrentPatrol.Loadout);
            StaticPatrolMinSpreadRadiusNUD.Value = CurrentPatrol.MinSpreadRadius;
            StaticPatrolMaxSpreadRadiusNUD.Value = CurrentPatrol.MaxSpreadRadius;
            StaticPatrolFormationCB.SelectedIndex = StaticPatrolFormationCB.FindStringExact(CurrentPatrol.Formation);
            StaticPatrolFormationLoosenessNUD.Value = CurrentPatrol.FormationLooseness;
            StaticPatrolLoadBalancingCategoryCB.SelectedIndex = StaticPatrolLoadBalancingCategoryCB.FindStringExact(CurrentPatrol.LoadBalancingCategory);
            StaticPatrolWaypointInterpolationCB.SelectedIndex = StaticPatrolWaypointInterpolationCB.FindStringExact(CurrentPatrol.WaypointInterpolation);
            StaticPatrolLootDropOnDeathCB.SelectedIndex = StaticPatrolLootDropOnDeathCB.FindStringExact(CurrentPatrol.LootDropOnDeath);
            StaticPatrolUseRandomWaypointAsStartPointCB.Checked = CurrentPatrol.UseRandomWaypointAsStartPoint == 1 ? true : false;
            StaticPatrolCanBeTriggeredByAICB.Checked = CurrentPatrol.CanBeTriggeredByAI == 1 ? true : false;
            StaticPatrolNoiseInvestigationDistanceLimitNUD.Value = CurrentPatrol.NoiseInvestigationDistanceLimit;
            StaticPatrolFormationScaleNUD.Value = CurrentPatrol.FormationScale;
            int StaticPatrolUnlimitedReloadBitmask = CurrentPatrol.UnlimitedReload;
            if (StaticPatrolUnlimitedReloadBitmask == 1)
                StaticPatrolUnlimitedReloadBitmask = 30;
            StaticPatrolURAnimalsCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 2) != 0) ? true : false;
            StaticPatrolURInfectedCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 4) != 0) ? true : false;
            StaticPatrolURPlayersCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 8) != 0) ? true : false;
            StaticPatrolURVehiclesCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 16) != 0) ? true : false;

            for (int i = 0; i < StaticPatrolLootingBehaviousCLB.Items.Count; i++)
            {
                StaticPatrolLootingBehaviousCLB.SetItemChecked(i, false);
            }
            foreach (string s in CurrentPatrol.LootingBehaviour.Split('|'))
            {
                if (s == "") continue;
                StaticPatrolLootingBehaviousCLB.SetItemChecked(StaticPatrolLootingBehaviousCLB.Items.IndexOf(s.Trim()), true);
            }

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
        private void ImportObjectPositions(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import AI Patrol";
            openFileDialog.Filter = "Expansion Map|*.map|Object Spawner|*.json|DayZ Editor|*.dze";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    CurrentPatrol._waypoints.Clear();
                }
                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        string[] fileContent = File.ReadAllLines(filePath);
                        for (int i = 0; i < fileContent.Length; i++)
                        {
                            if (fileContent[i] == "") continue;
                            string[] linesplit = fileContent[i].Split('|');
                            string[] XYZ = linesplit[1].Split(' ');
                            CurrentPatrol._waypoints.Add(new Vec3(XYZ));
                        }
                        break;
                    case 2:
                        ObjectSpawnerArr newobjectspawner = JsonSerializer.Deserialize<ObjectSpawnerArr>(File.ReadAllText(filePath));
                        foreach(SpawnObjects so in newobjectspawner.Objects)
                        {
                            CurrentPatrol._waypoints.Add(new Vec3(so.pos));
                        }
                        break;
                    case 3:
                        DZE importfile = DZEHelpers.LoadFile(filePath);
                        foreach (Editorobject eo in importfile.EditorObjects)
                        {
                            CurrentPatrol._waypoints.Add(new Vec3(eo.Position));
                        }
                        break;
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
        private void ExportObjectPositions(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Export AI Patrol";
            save.Filter = "Expansion Map |*.map|Object Spawner|*.json";
            save.FileName = CurrentPatrol.Name;
            if (save.ShowDialog() == DialogResult.OK)
            {
                switch (save.FilterIndex)
                {
                    case 1:
                        StringBuilder SB = new StringBuilder();
                        foreach (Vec3 array in CurrentPatrol._waypoints)
                        {
                            SB.AppendLine("eAI_SurvivorM_Lewis|" + array.GetString() + "|0.0 0.0 0.0");
                        }
                        File.WriteAllText(save.FileName, SB.ToString());
                        break;
                    case 2:
                        ObjectSpawnerArr newobjectspawner = new ObjectSpawnerArr();
                        newobjectspawner.Objects = new BindingList<SpawnObjects>();
                        foreach (Vec3 array in CurrentPatrol._waypoints)
                        {
                            SpawnObjects newobject = new SpawnObjects();
                            newobject.name = "eAI_SurvivorM_Lewis";
                            newobject.pos = array.getfloatarray();
                            newobject.ypr = new float[] { 0, 0, 0 };
                            newobject.scale = 1;
                            newobject.enableCEPersistency = false;
                            newobjectspawner.Objects.Add(newobject);
                        }
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(newobjectspawner, options);
                        File.WriteAllText(save.FileName, jsonString);
                        break;
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
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.ObjectClassName = textBox6.Text;
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
            CurrentPatrol.Loadout = StaticPatrolLoadoutsCB.GetItemText(StaticPatrolLoadoutsCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolLootDropOnDeathCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.LootDropOnDeath = StaticPatrolLootDropOnDeathCB.GetItemText(StaticPatrolLootDropOnDeathCB.SelectedItem);
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

        private void StaticPatrolLoadBalancingCategoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.LoadBalancingCategory = StaticPatrolLoadBalancingCategoryCB.GetItemText(StaticPatrolLoadBalancingCategoryCB.SelectedItem);
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
            CurrentPatrol.NoiseInvestigationDistanceLimit = StaticPatrolNoiseInvestigationDistanceLimitNUD.Value;
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
        private void StaticPatrolPersistCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Persist = StaticPatrolPersistCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolURBitmaskCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            int StaticPatrolUnlimitedReloadBitmask = 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURAnimalsCB.Checked ? 2 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURInfectedCB.Checked ? 4 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURPlayersCB.Checked ? 8 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURVehiclesCB.Checked ? 16 : 0;
            if (StaticPatrolUnlimitedReloadBitmask == 30)
                StaticPatrolUnlimitedReloadBitmask = 1;
            CurrentPatrol.UnlimitedReload = StaticPatrolUnlimitedReloadBitmask;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolCanBeTriggeredByAICB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.CanBeTriggeredByAI = StaticPatrolCanBeTriggeredByAICB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolLootingBehaviousCLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CheckedListBox)sender).ClearSelected();
        }
        private void StaticPatrolLootingBehaviousCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!useraction) return;
            var list = (CheckedListBox)sender;
            string changedItem = list.Items[e.Index].ToString();
            bool willBeChecked = e.NewValue == CheckState.Checked;

            // Temporarily remove the event handler to avoid recursion
            list.ItemCheck -= StaticPatrolLootingBehaviousCLB_ItemCheck;

            if (changedItem == "ALL" && willBeChecked)
            {
                // Uncheck everything else
                for (int i = 0; i < list.Items.Count; i++)
                {
                    if (i != e.Index)
                        list.SetItemChecked(i, false);
                }
            }
            else
            {
                // Uncheck "ALL" if anything else is checked
                int allIndex = list.Items.IndexOf("ALL");
                if (allIndex >= 0)
                    list.SetItemChecked(allIndex, false);


                // WEAPONS logic
                if (changedItem == "WEAPONS" && willBeChecked)
                {
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        if (list.Items[i].ToString().StartsWith("WEAPONS_"))
                            list.SetItemChecked(i, false);
                    }
                }
                else if (changedItem.StartsWith("WEAPONS_") && willBeChecked)
                {
                    int weaponsIndex = list.Items.IndexOf("WEAPONS");
                    if (weaponsIndex >= 0)
                        list.SetItemChecked(weaponsIndex, false);
                }

                // CLOTHING logic
                if (changedItem == "CLOTHING" && willBeChecked)
                {
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        if (list.Items[i].ToString().StartsWith("CLOTHING_"))
                            list.SetItemChecked(i, false);
                    }
                }
                else if (changedItem.StartsWith("CLOTHING_") && willBeChecked)
                {
                    int clothingIndex = list.Items.IndexOf("CLOTHING");
                    if (clothingIndex >= 0)
                        list.SetItemChecked(clothingIndex, false);
                }

                // CLOTHING_BACK hierarchy logic
                if (changedItem == "CLOTHING_BACK" && willBeChecked)
                {
                    // Uncheck all sub-options
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        string item = list.Items[i].ToString();
                        if (item == "CLOTHING_BACK_SMALL" ||
                            item == "CLOTHING_BACK_MEDIUM" ||
                            item == "CLOTHING_BACK_LARGE")
                        {
                            list.SetItemChecked(i, false);
                        }
                    }
                }
                else if ((changedItem == "CLOTHING_BACK_SMALL" ||
                          changedItem == "CLOTHING_BACK_MEDIUM" ||
                          changedItem == "CLOTHING_BACK_LARGE") && willBeChecked)
                {
                    int backIndex = list.Items.IndexOf("CLOTHING_BACK");
                    if (backIndex >= 0)
                        list.SetItemChecked(backIndex, false);
                }
            }

            // Reattach the event handler
            list.ItemCheck += StaticPatrolLootingBehaviousCLB_ItemCheck;

            // Finally, update the checked items string (now safely updated)

            CurrentPatrol.LootingBehaviour = UpdateCheckedItemsString(list, e.Index, e.NewValue);
            AIPatrolSettings.isDirty = true;
        }

        private void StaticPatrolFormationScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.FormationScale = StaticPatrolFormationScaleNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            ExpansionAIPatrol newpatrol = new ExpansionAIPatrol()
            {
                Name = "NewPatrol",
                Persist = 0,
                Faction = "West",
                Formation = "",
                FormationLooseness = (decimal)0.0,
                Loadout = "",
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
        private void darkButton52_Click(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count <= 0) return;
            int index = StaticPatrolWayPointsLB.SelectedIndex;
            if (index == 0) return;
            int newindex = index - 1;
            Vec3 waypoint = StaticPatrolWayPointsLB.SelectedItem as Vec3;
            CurrentPatrol._waypoints.RemoveAt(index);
            CurrentPatrol._waypoints.Insert(newindex, waypoint);
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWayPointsLB.SelectedIndex = newindex;
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count <= 0) return;
            int index = StaticPatrolWayPointsLB.SelectedIndex;
            if (index == StaticPatrolWayPointsLB.Items.Count - 1) return;
            int newindex = index + 1;
            Vec3 waypoint = StaticPatrolWayPointsLB.SelectedItem as Vec3;
            CurrentPatrol._waypoints.RemoveAt(index);
            CurrentPatrol._waypoints.Insert(newindex, waypoint);
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWayPointsLB.SelectedIndex = newindex;
            AIPatrolSettings.isDirty = true;
        }

        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;
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
                        int eventradius = (int)(Math.Round(5f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (aipatrol == CurrentPatrol)
                            pen = new Pen(Color.Green, 4);
                            Pen pen2 = new Pen(Color.Green, 2);
                            if (CurrentWapypoint == waypoints)
                                pen2 = new Pen(Color.Yellow, 2);
                            int centerX2 = 0;
                            int centerY2 = 0;
                            if (c < aipatrol._waypoints.Count)
                            {
                                centerX2 = (int)(Math.Round(aipatrol._waypoints[c].X) * scalevalue);
                                centerY2 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(aipatrol._waypoints[c].Z, 0) * scalevalue);
                            }
                            else
                            {
                                centerX2 = (int)(Math.Round(aipatrol._waypoints[0].X) * scalevalue);
                                centerY2 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(aipatrol._waypoints[0].Z, 0) * scalevalue);
                            }
                            Point center2 = new Point(centerX2, centerY2);
                            e.Graphics.DrawLine(pen2, center, center2);
                        if (CurrentWapypoint == waypoints)
                            pen = new Pen(Color.Yellow, 4);
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
                if (CurrentPatrol == null) return;
                int c = 1;
                foreach (Vec3 waypoints in CurrentPatrol._waypoints)
                {
                    float scalevalue = AIPatrolScale * 0.05f;
                    string num = c.ToString();

                    int centerX = (int)(Math.Round(waypoints.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(waypoints.Z, 0) * scalevalue);
                    int eventradius = (int)(Math.Round(5f, 0) * scalevalue);
                    Point center = new Point(centerX, centerY);

                    Pen pen2 = new Pen(Color.Green, 2);
                    if (CurrentWapypoint == waypoints)
                        pen2 = new Pen(Color.Yellow, 2);
                    int centerX2 = 0;
                    int centerY2 = 0;
                    if (c < CurrentPatrol._waypoints.Count)
                    {
                        centerX2 = (int)(Math.Round(CurrentPatrol._waypoints[c].X) * scalevalue);
                        centerY2 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(CurrentPatrol._waypoints[c].Z, 0) * scalevalue);
                    }
                    else
                    {
                        centerX2 = (int)(Math.Round(CurrentPatrol._waypoints[0].X) * scalevalue);
                        centerY2 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(CurrentPatrol._waypoints[0].Z, 0) * scalevalue);
                    }
                    Point center2 = new Point(centerX2, centerY2);
                    e.Graphics.DrawLine(pen2, center, center2);

                    Pen pen = new Pen(Color.Green, 4);
                    if (CurrentWapypoint == waypoints)
                        pen = new Pen(Color.Yellow, 4);
                    if (c == 1)
                        getCircle(e.Graphics, pen, center, eventradius, CurrentPatrol.Name + "\n" + num);
                    else
                        getCircle(e.Graphics, pen, center, eventradius, "\n" + num);
                    c++;
                }
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius, string c, bool drawCenter = true)
        {
            if (drawCenter)
            {
                Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
                drawingArea.DrawEllipse(penToUse, rect);
            }
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
            else if (e.Button == MouseButtons.Left)
            {
                mouseeventargs = e;
                // This is the first mouse click.
                if (isFirstClick)
                {
                    isFirstClick = false;

                    // Determine the location and size of the double click
                    // rectangle area to draw around the cursor point.
                    doubleClickRectangle = new Rectangle(
                        e.X - (SystemInformation.DoubleClickSize.Width / 2),
                        e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                        SystemInformation.DoubleClickSize.Width,
                        SystemInformation.DoubleClickSize.Height);
                    Invalidate();

                    // Start the double click timer.
                    doubleClickTimer.Start();
                }

                // This is the second mouse click.
                else
                {
                    // Verify that the mouse click is within the double click
                    // rectangle and is within the system-defined double
                    // click period.
                    if (doubleClickRectangle.Contains(e.Location) &&
                        milliseconds < SystemInformation.DoubleClickTime)
                    {
                        isDoubleClick = true;
                    }
                }
            }
        }
        void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer.Stop();

                if (isDoubleClick)
                {
                    //Console.WriteLine("Perform double click action");
                    if (CurrentPatrol == null) return;
                    if (CurrentWapypoint == null) return;
                    //if (e is MouseEventArgs mouseEventArgs)
                    //{
                    Cursor.Current = Cursors.WaitCursor;
                    decimal scalevalue = AIPatrolScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    StaticPatrolWaypointPOSXNUD.Value = Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                    StaticPatrolWaypointPOSZNUD.Value = Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                    if (MapData.FileExists)
                    {
                        StaticPatrolWaypointPOSYNUD.Value = (decimal)(MapData.gethieght(CurrentWapypoint.X, CurrentWapypoint.Z));
                    }
                    Cursor.Current = Cursors.Default;
                    AIPatrolSettings.isDirty = true;
                    pictureBox2.Invalidate();
                    //}
                }
                else
                {
                    //Console.WriteLine("Perform single click action");
                    if (CurrentPatrol == null) return;
                    //if (e is MouseEventArgs mouseEventArgs)
                    //{
                    decimal scalevalue = AIPatrolScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                    foreach (Vec3 waypoints in CurrentPatrol._waypoints)
                    {
                        PointF pP = new PointF((float)waypoints.X, (float)waypoints.Z);
                        if (IsWithinCircle(pC, pP, (float)5))
                        {
                            StaticPatrolWayPointsLB.SelectedItem = waypoints;
                            StaticPatrolWayPointsLB.Refresh();
                            pictureBox2.Invalidate();
                            continue;
                        }
                    }
                    //}
                }

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }
        public bool IsWithinCircle(PointF pC, PointF pP, Single fRadius)
        {
            return Distance(pC, pP) <= fRadius;
        }
        public Single Distance(PointF p1, PointF p2)
        {
            Single dX = p1.X - p2.X;
            Single dY = p1.Y - p2.Y;
            Single multi = dX * dX + dY * dY;
            Single dist = (Single)Math.Round((Single)Math.Sqrt(multi), 3);
            return (Single)dist;
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
            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            useraction = false;
            AccuracyMinNUD.Value = AISettings.AccuracyMin;
            AccuracyMaxNUD.Value = AISettings.AccuracyMax;
            ThreatDistanceLimitNUD.Value = AISettings.ThreatDistanceLimit;
            NoiseInvestigationDistanceLimitNUD.Value = AISettings.NoiseInvestigationDistanceLimit;
            DamageMultiplierNUD.Value = AISettings.DamageMultiplier;
            FormationScaleNUD.Value = AISettings.FormationScale;
            SniperProneDistanceThresholdNUD.Value = AISettings.SniperProneDistanceThreshold;
            DamageReceivedMultiplierNUD.Value = AISettings.DamageReceivedMultiplier;
            VaultingCB.Checked = AISettings.Vaulting == 1 ? true : false;
            MannersCB.Checked = AISettings.Manners == 1 ? true : false;
            CanRecruitGuardsCB.Checked = AISettings.CanRecruitGuards == 1 ? true : false;
            CanRecruitFriendlyCB.Checked = AISettings.CanRecruitFriendly == 1 ? true : false;
            MaxRecruitableAINUD.Value = AISettings.MaxRecruitableAI;
            LogAIHitByCB.Checked = AISettings.LogAIHitBy == 1 ? true : false;
            LogAIKilledCB.Checked = AISettings.LogAIKilled == 1 ? true : false;

            EnableZombieVehicleAttackHandlerCB.Checked = AISettings.EnableZombieVehicleAttackHandler == 1 ? true : false;
            EnableZombieVehicleAttackPhysicsCB.Checked = AISettings.EnableZombieVehicleAttackPhysics == 1 ? true : false;

            AISettingsAdminsLB.DisplayMember = "DisplayName";
            AISettingsAdminsLB.ValueMember = "Value";
            AISettingsAdminsLB.DataSource = AISettings.Admins;

            PlayerFactionsLB.DisplayMember = "DisplayName";
            PlayerFactionsLB.ValueMember = "Value";
            PlayerFactionsLB.DataSource = AISettings.PlayerFactions;

            PreventClimbLB.DisplayMember = "DisplayName";
            PreventClimbLB.ValueMember = "Value";
            PreventClimbLB.DataSource = AISettings.PreventClimb;


            LightingConfigMinNightVisibilityMetersLB.DataSource = AISettings.AILightEntries;  // creates List<KeyValuePair<int, decimal>>

            useraction = true;
        }
        private void LightingConfigMinNightVisibilityMetersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LightingConfigMinNightVisibilityMetersLB.SelectedItem is AILightEntries entry)
            {
                numericUpDownKey.Value = entry.Key;
                numericUpDownValue.Value = entry.Value;
            }
        }
        private void numericUpDownKey_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (LightingConfigMinNightVisibilityMetersLB.SelectedItem is AILightEntries entry)
            {
                entry.Key = (int)numericUpDownKey.Value;
                AISettings.isDirty = true;
            }
        }
        private void numericUpDownValue_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (LightingConfigMinNightVisibilityMetersLB.SelectedItem is AILightEntries entry)
            {
                entry.Value = numericUpDownValue.Value;
                AISettings.isDirty = true;
            }
            ;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            int nextKey = AISettings.AILightEntries.Any() ? AISettings.AILightEntries.Max(e2 => e2.Key) + 1 : 1;
            decimal newValue = 0m;
            var newEntry = new AILightEntries { Key = nextKey, Value = newValue };
            AISettings.AILightEntries.Add(newEntry);
            LightingConfigMinNightVisibilityMetersLB.SelectedItem = newEntry;
            AISettings.isDirty = true;
        }

        private void darkButton38_Click(object sender, EventArgs e)
        {
            if (LightingConfigMinNightVisibilityMetersLB.SelectedItem is AILightEntries entry)
            {
                AISettings.AILightEntries.Remove(entry);
                AISettings.isDirty = true;
            }
            else
            {
                MessageBox.Show("Please select an item to remove.");
            }
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
        private void FormationScaleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.FormationScale = FormationScaleNUD.Value;
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
        private void MaxRecruitableAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.MaxRecruitableAI = (int)MaxRecruitableAINUD.Value;
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
        private void EnableZombieVehicleAttackHandlerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.EnableZombieVehicleAttackHandler = EnableZombieVehicleAttackHandlerCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void EnableZombieVehicleAttackPhysicsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.EnableZombieVehicleAttackPhysics = EnableZombieVehicleAttackPhysicsCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        #endregion AISettings
        #region AIlocations
        public ExpansionAIRoamingLocation CurrentAiLocation { get; set; }
        public ExpansionAINoGoAreaConfig CurrentAiLocationNOGOArea { get; set; }
        private Timer doubleClickTimer2 = new Timer();
        public int AILocationScale = 1;
        private void SetupExpansionAILocationSettingss()
        {
            useraction = false;
            listBox2.DisplayMember = "DisplayName";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = ExpansionAILocationSettings.ExcludedRoamingBuildings;

            listBox1.DisplayMember = "DisplayName";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = ExpansionAILocationSettings.RoamingLocations;

            listBox3.DisplayMember = "DisplayName";
            listBox3.ValueMember = "Value";
            listBox3.DataSource = ExpansionAILocationSettings.NoGoAreas;

            pictureBox3.Invalidate();
            doubleClickTimer2.Tick += new EventHandler(doubleClickTimer_Tick2);
            useraction = true;
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            string classname = textBox3.Text;
            if (classname == "") return;
            if (!ExpansionAILocationSettings.ExcludedRoamingBuildings.Contains(classname))
            {
                ExpansionAILocationSettings.ExcludedRoamingBuildings.Add(classname);
                ExpansionAILocationSettings.isDirty = true;
                listBox2.Refresh();
            }
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            ExpansionAILocationSettings.ExcludedRoamingBuildings.Remove(listBox2.GetItemText(listBox2.SelectedItem));
            ExpansionAILocationSettings.isDirty = true;
            listBox2.Refresh();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            CurrentAiLocation = listBox1.SelectedItem as ExpansionAIRoamingLocation;
            useraction = false;
            textBox2.Text = CurrentAiLocation.Name;
            numericUpDown1.Value = (decimal)CurrentAiLocation._Position.X;
            numericUpDown2.Value = (decimal)CurrentAiLocation._Position.Y;
            numericUpDown3.Value = (decimal)CurrentAiLocation._Position.Z;
            numericUpDown4.Value = (decimal)CurrentAiLocation.Radius;
            textBox4.Text = CurrentAiLocation.Type;
            checkBox12.Checked = CurrentAiLocation.Enabled;
            useraction = true;
            pictureBox3.Invalidate();
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocation.Enabled = checkBox12.Checked;
            ExpansionAILocationSettings.isDirty = true;
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItems.Count < 1) return;
            CurrentAiLocationNOGOArea = listBox3.SelectedItem as ExpansionAINoGoAreaConfig;
            useraction = false;
            textBox5.Text = CurrentAiLocationNOGOArea.Name;
            numericUpDown6.Value = (decimal)CurrentAiLocationNOGOArea._Position.X; 
            numericUpDown7.Value = (decimal)CurrentAiLocationNOGOArea._Position.Y;
            numericUpDown8.Value = (decimal)CurrentAiLocationNOGOArea._Position.Z;
            numericUpDown5.Value = (decimal)CurrentAiLocationNOGOArea.Radius;
            numericUpDown9.Value = (decimal)CurrentAiLocationNOGOArea.Height;
            useraction = true;
            pictureBox3.Invalidate();
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            ExpansionAINoGoAreaConfig newnogo = new ExpansionAINoGoAreaConfig()
            {
                Name = "New NoGO Area",
                _Position = new Vec3(currentproject.MapSize / 2, 0, currentproject.MapSize / 2),
                Radius = 300,
                Height = MapData.gethieght(currentproject.MapSize / 2, currentproject.MapSize / 2) + 200
            };
            ExpansionAILocationSettings.NoGoAreas.Add(newnogo);
            listBox3.SelectedIndex = listBox3.Items.Count - 1;
            ExpansionAILocationSettings.isDirty = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            ExpansionAILocationSettings.NoGoAreas.Remove(CurrentAiLocationNOGOArea);
            ExpansionAILocationSettings.isDirty = true;
            listBox3.Refresh();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea.Name = textBox5.Text;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown6_ValueChanged_2(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea._Position.X = (float)numericUpDown6.Value;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown7_ValueChanged_1(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea._Position.Y = (float)numericUpDown7.Value;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea._Position.Z = (float)numericUpDown8.Value;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea.Radius = (float)numericUpDown5.Value;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAiLocationNOGOArea.Height = (float)numericUpDown9.Value;
            ExpansionAILocationSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            AILocationScale = trackBar2.Value;
            SetAILocationZonescale();
        }
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
            else if (e.Button == MouseButtons.Left)
            {
                mouseeventargs = e;
                if (isFirstClick)
                {
                    isFirstClick = false;

                    // Determine the location and size of the double click
                    // rectangle area to draw around the cursor point.
                    doubleClickRectangle = new Rectangle(
                        e.X - (SystemInformation.DoubleClickSize.Width / 2),
                        e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                        SystemInformation.DoubleClickSize.Width,
                        SystemInformation.DoubleClickSize.Height);
                    Invalidate();

                    // Start the double click timer.
                    doubleClickTimer2.Start();
                }

                // This is the second mouse click.
                else
                {
                    // Verify that the mouse click is within the double click
                    // rectangle and is within the system-defined double
                    // click period.
                    if (doubleClickRectangle.Contains(e.Location) &&
                        milliseconds < SystemInformation.DoubleClickTime)
                    {
                        isDoubleClick = true;
                    }
                }
            }
        }
        void doubleClickTimer_Tick2(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer2.Stop();

                if (isDoubleClick)
                {
                    switch (tabControl5.SelectedIndex)
                    {
                        case 0:
                        case 1:
                            break;
                        case 2:
                            //Console.WriteLine("Perform double click action");
                            if (CurrentAiLocationNOGOArea == null) return;
                            Cursor.Current = Cursors.WaitCursor;
                            decimal scalevalue = AILocationScale * (decimal)0.05;
                            decimal mapsize = currentproject.MapSize;
                            int newsize = (int)(mapsize * scalevalue);
                            numericUpDown6.Value = Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                            numericUpDown8.Value = Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                            if (MapData.FileExists)
                            {
                                numericUpDown7.Value = (decimal)(MapData.gethieght(CurrentAiLocationNOGOArea._Position.X, CurrentAiLocationNOGOArea._Position.Z));
                            }
                            Cursor.Current = Cursors.Default;
                            ExpansionAILocationSettings.isDirty = true;
                            pictureBox3.Invalidate();
                            break;
                    }
                }
                else
                {
                    switch (tabControl5.SelectedIndex)
                    {
                        case 0:

                            break;
                        case 1:
                            //Console.WriteLine("Perform single click action");
                            if (CurrentAiLocation == null) return;
                            decimal scalevalue = AILocationScale * (decimal)0.05;
                            decimal mapsize = currentproject.MapSize;
                            int newsize = (int)(mapsize * scalevalue);
                            PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                            foreach (ExpansionAIRoamingLocation poiny in ExpansionAILocationSettings.RoamingLocations)
                            {
                                PointF pP = new PointF((float)poiny._Position.X, (float)poiny._Position.Z);
                                if (IsWithinCircle(pC, pP, (float)poiny.Radius))
                                {
                                    listBox1.SelectedItem = poiny;
                                    StaticPatrolWayPointsLB.Refresh();
                                    pictureBox2.Invalidate();
                                    continue;
                                }
                            }
                            break;
                        case 2:
                            //Console.WriteLine("Perform single click action");
                            if (CurrentAiLocationNOGOArea == null) return;
                            scalevalue = AILocationScale * (decimal)0.05;
                            mapsize = currentproject.MapSize;
                            newsize = (int)(mapsize * scalevalue);
                            pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                            foreach (ExpansionAINoGoAreaConfig ExpansionAINoGoAreaConfig in ExpansionAILocationSettings.NoGoAreas)
                            {
                                PointF pP = new PointF((float)ExpansionAINoGoAreaConfig._Position.X, (float)ExpansionAINoGoAreaConfig._Position.Z);
                                if (IsWithinCircle(pC, pP, ExpansionAINoGoAreaConfig.Radius))
                                {
                                    listBox3.SelectedItem = ExpansionAINoGoAreaConfig;
                                    StaticPatrolWayPointsLB.Refresh();
                                    pictureBox2.Invalidate();
                                    continue;
                                }
                            }
                            break;
                    }
                }

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }
        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox3.Focused == false)
            {
                pictureBox3.Focus();
                panelEx2.AutoScrollPosition = _newscrollPosition;
                pictureBox3.Invalidate();
            }
        }
        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panelEx2.AutoScrollPosition.X - changePoint.X, -panelEx2.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panelEx2.AutoScrollPosition = _newscrollPosition;
                pictureBox3.Invalidate();
            }
            decimal scalevalue = AILocationScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label3.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox3_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox3_ZoomOut();
            }
            else
            {
                pictureBox3_ZoomIn();
            }

        }
        private void pictureBox3_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox3.Height;
            int oldpitureboxwidht = pictureBox3.Width;
            Point oldscrollpos = panelEx2.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar2.Value = newval;
            AILocationScale = trackBar2.Value;
            SetAILocationZonescale();
            if (pictureBox3.Height > panelEx2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox3.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox3.Width > panelEx2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox3.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox3.Invalidate();
        }
        private void pictureBox3_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox3.Height;
            int oldpitureboxwidht = pictureBox3.Width;
            Point oldscrollpos = panelEx2.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar2.Value = newval;
            AILocationScale = trackBar2.Value;
            SetAILocationZonescale();
            if (pictureBox3.Height > panelEx2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox3.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox3.Width > panelEx2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox3.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox3.Invalidate();
        }
        private void SetAILocationZonescale()
        {
            float scalevalue = AILocationScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox3.Size = new Size(newsize, newsize);
        }
        private void DrawAILocations(object sender, PaintEventArgs e)
        {
            switch (tabControl5.SelectedIndex)
            {
                case 0:
                    
                    break;
                case 1:
                    if (checkBox11.Checked)
                    {
                        foreach (ExpansionAIRoamingLocation ExpansionAIRoamingLocation in ExpansionAILocationSettings.RoamingLocations)
                        {
                            float scalevalue = AILocationScale * 0.05f;
                            int centerX = (int)(Math.Round(ExpansionAIRoamingLocation._Position.X) * scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(ExpansionAIRoamingLocation._Position.Z, 0) * scalevalue);
                            int eventradius = (int)((float)ExpansionAIRoamingLocation.Radius * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Red, 1);
                            if (CurrentAiLocation == ExpansionAIRoamingLocation)
                                pen = new Pen(Color.Green, 1);
                            getCircle(e.Graphics, pen, center, eventradius, ExpansionAIRoamingLocation.Name);
                        }
                    }
                    else
                    {
                        float scalevalue = AILocationScale * 0.05f;
                        int centerX = (int)(Math.Round(CurrentAiLocation._Position.X) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(CurrentAiLocation._Position.Z, 0) * scalevalue);
                        int eventradius = (int)((float)CurrentAiLocation.Radius * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Green, 1);
                        getCircle(e.Graphics, pen, center, eventradius, CurrentAiLocation.Name);
                    }
                    break;
                case 2:
                    foreach (ExpansionAINoGoAreaConfig ExpansionAINoGoAreaConfig in ExpansionAILocationSettings.NoGoAreas)
                    {
                        float scalevalue = AILocationScale * 0.05f;
                        int centerX = (int)(Math.Round(ExpansionAINoGoAreaConfig._Position.X) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(ExpansionAINoGoAreaConfig._Position.Z, 0) * scalevalue);
                        int eventradius = (int)((float)ExpansionAINoGoAreaConfig.Radius * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 1);
                        if (CurrentAiLocationNOGOArea == ExpansionAINoGoAreaConfig)
                            pen = new Pen(Color.Green, 1);
                        getCircle(e.Graphics, pen, center, eventradius, ExpansionAINoGoAreaConfig.Name);
                    }
                    break;
            }
            
        }

        #endregion AIlocations
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
        private TreeNode currenmttreenode;

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import AI Patrol";
            openFileDialog.Filter = "Expansion Map|*.map|Object Spawner|*.json|DayZ Editor|*.dze";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    CurrentPatrol._waypoints.Clear();
                }
                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        string[] fileContent = File.ReadAllLines(filePath);
                        bool ImportTrigger = false;
                        switch (SpatialGroupsCB.SelectedIndex)
                        {
                            case 0:
                                MessageBox.Show("Spatial Groups dont have a trigger position or and spawn points.");
                                return;
                            case 1:
                                currentSpatialPoint.ImportMap(fileContent);
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
                                currentSpatialLocation.ImportMap(fileContent, ImportTrigger);
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
                                currentSpatialAudio.ImportMap(fileContent, ImportTrigger);
                                SpatialGroupsAudioSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.X;
                                SpatialGroupsAudioSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Y;
                                SpatialGroupsAudioSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Z;
                                SpatialGroupsSpawnPositionLB.DataSource = currentSpatialAudio._Spatial_SpawnPosition;
                                break;
                        }
                        m_Spatial_Groups.isDirty = true;
                        break;
                    case 2:
                        ImportTrigger = false;
                        ObjectSpawnerArr newobjectspawner = JsonSerializer.Deserialize<ObjectSpawnerArr>(File.ReadAllText(filePath));
                        switch (SpatialGroupsCB.SelectedIndex)
                        {
                            case 0:
                                MessageBox.Show("Spatial Groups dont have a trigger position or and spawn points.");
                                return;
                            case 1:
                                currentSpatialPoint.ImportOpbjectSpawner(newobjectspawner);
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
                                currentSpatialLocation.ImportOpbjectSpawner(newobjectspawner, ImportTrigger);
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
                                currentSpatialAudio.ImportOpbjectSpawner(newobjectspawner, ImportTrigger);
                                SpatialGroupsAudioSpatial_TriggerPositionXNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.X;
                                SpatialGroupsAudioSpatial_TriggerPositionYNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Y;
                                SpatialGroupsAudioSpatial_TriggerPositionZNUD.Value = (decimal)currentSpatialAudio._Spatial_TriggerPosition.Z;
                                SpatialGroupsSpawnPositionLB.DataSource = currentSpatialAudio._Spatial_SpawnPosition;
                                break;
                        }
                        m_Spatial_Groups.isDirty = true;
                        break;
                    case 3:
                        DZE importfile = DZEHelpers.LoadFile(filePath);
                        ImportTrigger = false;
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
                        break;
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

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox3.Invalidate();
        }

        private void SpatialPointGB_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void crashLootingBehaviousCLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CheckedListBox)sender).ClearSelected();
        }


    }
}
