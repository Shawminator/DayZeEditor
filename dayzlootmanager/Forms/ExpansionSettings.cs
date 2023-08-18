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
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeEditor
{

    public partial class ExpansionSettings : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public BindingList<string> Factions { get; private set; }

        public string Projectname;
        private bool _useraction = false;

        public containerLoot SCL { get; private set; }
        public lootVarients SLootVarients { get; private set; }
        public PersonalStorage CurrentPersonalStorage { get; private set; }

        public bool useraction
        {
            get { return _useraction; }
            set
            { 
                _useraction = value; 
            }
        }

        

        public string AirdropsettingPath;
        public string BaseBUildignsettingsPath;
        public string BookSettingsPath;
        public string ChatSettingsPath;
        public string DamageSystemSettingsPath;
        public string DebugSettingsPath;
        public string GarageSettingsPath;
        public string GeneralSettingsPath;
        public string HArdlineSettingsPath;
        public string HardlinePlayerDataPath;
        public string LogsSettingsPath;
        public string MapSettingsPath;
        //public string MarketSettingsPath;
        public string MissionSettingsPath;
        public string MonitoringSettingsPath;
        public string NameTagsettingsPath;
        public string NotificationSchedulerSettingsPath;
        public string NotificationssettingsPath;
        public string PartySettingsPath;
        public string PersonalStoragePath;
        public string PersonalStorageSettingsPath;
        public string PlayerListsettingsPath;
        public string RaidSettingsPath;
        public string SafeZoneSettingspath;
        public string SocialMediaSettingsPath;
        public string SpawnSettingsPath;
        public string TerritorySettingsPath;
        public string VehicleSettingsPath;

        public AirdropsettingsJson AirdropsettingsJson;
        public BaseBuildingSettings BaseBuildingSettings;
        public BookSettings BookSettings;
        public ChatSettings ChatSettings;
        public DamageSystemSettings DamageSystemSettings;
        public DebugSettings DebugSettings;
        public GarageSettings GarageSettings;
        public GeneralSettings GeneralSettings;
        public HardLineSettings HardLineSettings;
        public ExpansionHardlinePlayerDataList ExpansionHardlinePlayerDataList;
        public LogSettings LogSettings;
        public MapSettings MapSettings;
        //public MarketSettings marketsettings;
        public MissionSettings MissionSettings;
        public MonitoringSettings MonitoringSettings;
        public NameTagsettings NameTagSettings;
        public NotificationSchedulerSettings NotificationSchedulerSettings;
        public NotificationSettings NotificationSettings;
        public PartySettings PartySettings;
        public PersonalStorageList PersonalStorageList;
        public PersonalStorageSettings PersonalStorageSettings;
        public PlayerListSettings PlayerListSettings;
        public RaidSettings RaidSettings;
        public SafeZoneSettings SafeZoneSettings;
        public SocialMediaSettings SocialMediaSettings;
        public SpawnSettings SpawnSettings;
        public TerritorySettings TerritorySettings;
        public VehicleSettings VehicleSettings;
 
        public MapData MapData;

        #region GeneralsettingFunctions
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
        public ExpansionSettings()
        {
            InitializeComponent();
            ExpansionSettingsTabPage.ItemSize = new Size(0, 1);
            comboBox2.DataSource = Enum.GetValues(typeof(ContainerTypes));
            EnableLampsComboBox.DataSource = Enum.GetValues(typeof(Lamps));
            BuildingIvysComboBox.DataSource = Enum.GetValues(typeof(buildingIvy));
            VariousTabButton.AutoSize = true;
            AirdropsTabButton.AutoSize = true;
            BookTabButton.AutoSize = true;
            BaseBuildingTabButton.AutoSize = true;
            GeneralTabButton.AutoSize = true;
            NotificationschedularTabButton.AutoSize = true;
            HardlineTabButton.AutoSize = true;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            VariousTabButton.Checked = false;
            AirdropsTabButton.Checked = false;
            BaseBuildingTabButton.Checked = false;
            BookTabButton.Checked = false;
            GarageTabButton.Checked = false;
            GeneralTabButton.Checked = false;
            HardlineTabButton.Checked = false;
            MapTabButton.Checked = false;
            MissionsTabButton.Checked = false;
            NotificationschedularTabButton.Checked = false;
            PersonalStorageTabButton.Checked = false;
            RaidTabButton.Checked = false;
            SafeZoneTabButton.Checked = false;
            SpawnTabButton.Checked = false;
            VehicleTabButton.Checked = false;
        }
        private void VariousTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 0;
            if (ExpansionSettingsTabPage.SelectedIndex == 0)
                VariousTabButton.Checked = true;
        }
        private void AirdropsTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 1;
            if (ExpansionSettingsTabPage.SelectedIndex == 1)
                AirdropsTabButton.Checked = true;
        }
        private void BaseBuildingTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 2;
            if (ExpansionSettingsTabPage.SelectedIndex == 2)
                BaseBuildingTabButton.Checked = true;
        }
        private void BookTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 3;
            if (ExpansionSettingsTabPage.SelectedIndex == 3)
                BookTabButton.Checked = true;
        }
        private void GarageTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 4;
            if (ExpansionSettingsTabPage.SelectedIndex == 4)
                GarageTabButton.Checked = true;
        }
        private void GeneralTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 5;
            if (ExpansionSettingsTabPage.SelectedIndex == 5)
                GeneralTabButton.Checked = true;
        }
        private void HardlineTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 6;
            if (ExpansionSettingsTabPage.SelectedIndex == 6)
                HardlineTabButton.Checked = true;
        }
        private void MapTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 7;
            if (ExpansionSettingsTabPage.SelectedIndex == 7)
                MapTabButton.Checked = true;
        }
        private void MissionsTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 8;
            if (ExpansionSettingsTabPage.SelectedIndex == 8)
                MissionsTabButton.Checked = true;
        }
        private void NotificationschedularTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 9;
            if (ExpansionSettingsTabPage.SelectedIndex == 9)
                NotificationschedularTabButton.Checked = true;
        }
        private void PersonalStorageTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 10;
            if (ExpansionSettingsTabPage.SelectedIndex == 10)
                PersonalStorageTabButton.Checked = true;
        }
        private void RaidTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 11;
            if (ExpansionSettingsTabPage.SelectedIndex == 11)
                RaidTabButton.Checked = true;
        }
        private void SafeZoneTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 12;
            if (ExpansionSettingsTabPage.SelectedIndex == 12)
                SafeZoneTabButton.Checked = true;
        }
        private void SpawnTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 13;
            if (ExpansionSettingsTabPage.SelectedIndex == 13)
                SpawnTabButton.Checked = true;
        }
        private void VehicleTabButton_Click(object sender, EventArgs e)
        {
            ExpansionSettingsTabPage.SelectedIndex = 14;
            if (ExpansionSettingsTabPage.SelectedIndex == 14)
                VehicleTabButton.Checked = true;
        }
        private void expansionsettings_Load(object sender, EventArgs e)
        {
            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            Factions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt").ToList());

            bool needtosave = false;

            DamageSystemSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\DamageSystemSettings.json";
            if (!File.Exists(DamageSystemSettingsPath))
            {
                Console.WriteLine(Path.GetFileName(DamageSystemSettingsPath) + " File not found, Creating new....");
                DamageSystemSettings = new DamageSystemSettings();
                needtosave = true;
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(DamageSystemSettingsPath));
                DamageSystemSettings = JsonSerializer.Deserialize<DamageSystemSettings>(File.ReadAllText(DamageSystemSettingsPath));
                DamageSystemSettings.ConvertDicttolist();
                DamageSystemSettings.isDirty = false;
                if (DamageSystemSettings.checkver())
                    needtosave = true;
            }
            DamageSystemSettings.Filename = DamageSystemSettingsPath;
            loadDamageSystemSettings();

            AirdropsettingPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\settings\\AirdropSettings.json";
            if (!File.Exists(AirdropsettingPath))
            {
                Console.WriteLine(Path.GetFileName(AirdropsettingPath) + " File not found, Creating new....");
                AirdropsettingsJson = new AirdropsettingsJson();
                needtosave = true;
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(AirdropsettingPath));
                AirdropsettingsJson = JsonSerializer.Deserialize<AirdropsettingsJson>(File.ReadAllText(AirdropsettingPath));
                AirdropsettingsJson.isDirty = false;
                if (AirdropsettingsJson.checkver())
                    needtosave = true;
            }
            AirdropsettingsJson.Filename = AirdropsettingPath;
            SetupAirdropsettings();

            BaseBUildignsettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\BaseBuildingSettings.json";
            if (!File.Exists(BaseBUildignsettingsPath))
            {
                BaseBuildingSettings = new BaseBuildingSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(BaseBUildignsettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(BaseBUildignsettingsPath));
                BaseBuildingSettings = JsonSerializer.Deserialize<BaseBuildingSettings>(File.ReadAllText(BaseBUildignsettingsPath));
                BaseBuildingSettings.isDirty = false;
                if (BaseBuildingSettings.checkver())
                    needtosave = true;
            }
            BaseBuildingSettings.Filename = BaseBUildignsettingsPath;
            Setupbasebuildingsettings();


            BookSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\BookSettings.json";
            if (!File.Exists(BookSettingsPath))
            {
                BookSettings = new BookSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(BookSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(BookSettingsPath));
                BookSettings = JsonSerializer.Deserialize<BookSettings>(File.ReadAllText(BookSettingsPath));
                BookSettings.isDirty = false;
                if (BookSettings.checkver())
                    needtosave = true;
            }
            BookSettings.Filename = BookSettingsPath;
            loadBookSettings();

            ChatSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\ChatSettings.json";
            if (!File.Exists(ChatSettingsPath))
            {
                ChatSettings = new ChatSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(ChatSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(ChatSettingsPath));
                ChatSettings = JsonSerializer.Deserialize<ChatSettings>(File.ReadAllText(ChatSettingsPath));
                ChatSettings.isDirty = false;
                if (ChatSettings.checkver())
                    needtosave = true;
            }
            ChatSettings.Filename = ChatSettingsPath;
            LoadChatsettings();

            DebugSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\DebugSettings.json";
            if (!File.Exists(DebugSettingsPath))
            {
                DebugSettings = new DebugSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(DebugSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(DebugSettingsPath));
                DebugSettings = JsonSerializer.Deserialize<DebugSettings>(File.ReadAllText(DebugSettingsPath));
                DebugSettings.isDirty = false;
                if (DebugSettings.checkver())
                    needtosave = true;
            }
            DebugSettings.Filename = DebugSettingsPath;
            loaddebugsettings();

            GarageSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\GarageSettings.json";
            if (!File.Exists(GarageSettingsPath))
            {
                GarageSettings = new GarageSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(GarageSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(GarageSettingsPath));
                GarageSettings = JsonSerializer.Deserialize<GarageSettings>(File.ReadAllText(GarageSettingsPath));
                GarageSettings.isDirty = false;
                if (GarageSettings.checkver())
                    needtosave = true;
            }
            GarageSettings.Filename = GarageSettingsPath;
            loadgaragesettings();


            GeneralSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\GeneralSettings.json";
            if (!File.Exists(GeneralSettingsPath))
            {
                GeneralSettings = new GeneralSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(GeneralSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(GeneralSettingsPath));
                GeneralSettings = JsonSerializer.Deserialize<GeneralSettings>(File.ReadAllText(GeneralSettingsPath));
                GeneralSettings.isDirty = false;
                if (GeneralSettings.checkver())
                    needtosave = true;
            }
            GeneralSettings.Filename = GeneralSettingsPath;
            loadGeneralSettings();

            HArdlineSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\HardlineSettings.json";
            if (!File.Exists(HArdlineSettingsPath))
            {
                HardLineSettings = new HardLineSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(HArdlineSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(HArdlineSettingsPath));
                HardLineSettings = JsonSerializer.Deserialize<HardLineSettings>(File.ReadAllText(HArdlineSettingsPath));
                HardLineSettings.isDirty = false;
                if (HardLineSettings.checkver())
                    needtosave = true;
            }
            HardLineSettings.ConvertDictionarytoLevels();
            HardLineSettings.Filename = HArdlineSettingsPath;
            loadHardlineSettings();

            LogsSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\LogsSettings.json";
            if (!File.Exists(LogsSettingsPath))
            {
                LogSettings = new LogSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(LogsSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(LogsSettingsPath));
                LogSettings = JsonSerializer.Deserialize<LogSettings>(File.ReadAllText(LogsSettingsPath));
                LogSettings.isDirty = false;
                if (GeneralSettings.checkver())
                    needtosave = true;
            }
            LogSettings.Filename = LogsSettingsPath;
            loadlogsettings();

            MapSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\mapSettings.json";
            if (!File.Exists(MapSettingsPath))
            {
                MapSettings = new MapSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(MapSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(MapSettingsPath));
                MapSettings = JsonSerializer.Deserialize<MapSettings>(File.ReadAllText(MapSettingsPath));
                MapSettings.isDirty = false;
                if (MapSettings.checkver())
                    needtosave = true;
            }
            MapSettings.Filename = MapSettingsPath;
            loadmapsettings();

            //MarketSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\MarketSettings.json";
            //if (!File.Exists(MarketSettingsPath))
            //{
            //    marketsettings = new MarketSettings();
            //    marketsettings.Filename = MarketSettingsPath;
            //    needtosave = true;
            //}

            MissionSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\MissionSettings.json";
            if (!File.Exists(MissionSettingsPath))
            {
                MissionSettings = new MissionSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(MissionSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(MissionSettingsPath));
                MissionSettings = JsonSerializer.Deserialize<MissionSettings>(File.ReadAllText(MissionSettingsPath));
                MissionSettings.isDirty = false;
                if (MissionSettings.checkver())
                    needtosave = true;
            }
            MissionSettings.Filename = MissionSettingsPath;
            Console.WriteLine("Loading Expansion Mission files....");
            MissionSettings.LoadIndividualMissions(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath);
            loadMissionSettings();

            MonitoringSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\MonitoringSettings.json";
            if (!File.Exists(MonitoringSettingsPath))
            {
                MonitoringSettings = new MonitoringSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(MonitoringSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(MonitoringSettingsPath));
                MonitoringSettings = JsonSerializer.Deserialize<MonitoringSettings>(File.ReadAllText(MonitoringSettingsPath));
                MonitoringSettings.isDirty = false;
                if (MonitoringSettings.checkver())
                    needtosave = true;
            }
            MonitoringSettings.Filename = MonitoringSettingsPath;
            LoadMonitoringSettingss();

            NameTagsettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\NameTagsSettings.json";
            if (!File.Exists(NameTagsettingsPath))
            {
                NameTagSettings = new NameTagsettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(NameTagsettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(NameTagsettingsPath));
                NameTagSettings = JsonSerializer.Deserialize<NameTagsettings>(File.ReadAllText(NameTagsettingsPath));
                NameTagSettings.isDirty = false;
                if (NameTagSettings.checkver())
                    needtosave = true;
            }
            NameTagSettings.Filename = NameTagsettingsPath;
            LoadNameTagSettings();

            NotificationSchedulerSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\NotificationSchedulerSettings.json";
            if (!File.Exists(NotificationSchedulerSettingsPath))
            {
                NotificationSchedulerSettings = new NotificationSchedulerSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(NotificationSchedulerSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(NotificationSchedulerSettingsPath));
                NotificationSchedulerSettings = JsonSerializer.Deserialize<NotificationSchedulerSettings>(File.ReadAllText(NotificationSchedulerSettingsPath));
                NotificationSchedulerSettings.isDirty = false;
                if (NotificationSchedulerSettings.checkver())
                    needtosave = true;
                if (NotificationSchedulerSettings.checknotificationcols())
                    needtosave = true;
            }
            NotificationSchedulerSettings.Filename = NotificationSchedulerSettingsPath;
            LoadNotificationSchedulerSettings();

            NotificationssettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\NotificationSettings.json";
            if (!File.Exists(NotificationssettingsPath))
            {
                NotificationSettings = new NotificationSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(NotificationssettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(NotificationssettingsPath));
                NotificationSettings = JsonSerializer.Deserialize<NotificationSettings>(File.ReadAllText(NotificationssettingsPath));
                NotificationSettings.isDirty = false;
                if (NotificationSettings.checkver())
                    needtosave = true;
            }
            NotificationSettings.Filename = NotificationssettingsPath;
            LoadNotificationSettings();

            PartySettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\PartySettings.json";
            if (!File.Exists(PartySettingsPath))
            {
                PartySettings = new PartySettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(PartySettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(PartySettingsPath));
                PartySettings = JsonSerializer.Deserialize<PartySettings>(File.ReadAllText(PartySettingsPath));
                PartySettings.isDirty = false;
                if (PartySettings.checkver())
                    needtosave = true;
            }
            PartySettings.Filename = PartySettingsPath;
            loadpartysettings();



            PersonalStorageSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\PersonalStorageNewSettings.json";
            if (!File.Exists(PersonalStorageSettingsPath))
            {
                PersonalStorageSettings = new PersonalStorageSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(PersonalStorageSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(PartySettingsPath));
                PersonalStorageSettings = JsonSerializer.Deserialize<PersonalStorageSettings>(File.ReadAllText(PersonalStorageSettingsPath));
                PersonalStorageSettings.isDirty = false;
                if (PersonalStorageSettings.checkver())
                    needtosave = true;
            }
            PersonalStorageSettings.Filename = PersonalStorageSettingsPath;
           



            Console.WriteLine("Loading Personal Storage files....");
            PersonalStoragePath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\personalstorage";
            PersonalStorageList = new PersonalStorageList(PersonalStoragePath);
            loadPersonalStorageSettings();



            PlayerListsettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\PlayerListSettings.json";
            if (!File.Exists(PlayerListsettingsPath))
            {
                PlayerListSettings = new PlayerListSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(PlayerListsettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(PlayerListsettingsPath));
                PlayerListSettings = JsonSerializer.Deserialize<PlayerListSettings>(File.ReadAllText(PlayerListsettingsPath));
                PlayerListSettings.isDirty = false;
                if (PlayerListSettings.checkver())
                    needtosave = true;
            }
            PlayerListSettings.Filename = PlayerListsettingsPath;
            LoadPlayerListsettings();

            RaidSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\Raidsettings.json";
            if (!File.Exists(RaidSettingsPath))
            {
                RaidSettings = new RaidSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(RaidSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(RaidSettingsPath));
                RaidSettings = JsonSerializer.Deserialize<RaidSettings>(File.ReadAllText(RaidSettingsPath));
                RaidSettings.isDirty = false;
                if (RaidSettings.checkver())
                    needtosave = true;
            }
            RaidSettings.Filename = RaidSettingsPath;
            loadRaidSettings();

            SafeZoneSettingspath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\SafeZonesettings.json";
            if (!File.Exists(SafeZoneSettingspath))
            {
                SafeZoneSettings = new SafeZoneSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(SafeZoneSettingspath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(SafeZoneSettingspath));
                SafeZoneSettings = JsonSerializer.Deserialize<SafeZoneSettings>(File.ReadAllText(SafeZoneSettingspath));
                SafeZoneSettings.isDirty = false;
                if (SafeZoneSettings.checkver())
                    needtosave = true;
            }
            SafeZoneSettings.Filename = SafeZoneSettingspath;
            LoadSafeZonesettings();

            SocialMediaSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\SocialMediaSettings.json";
            if (!File.Exists(SocialMediaSettingsPath))
            {
                SocialMediaSettings = new SocialMediaSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(SocialMediaSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(SocialMediaSettingsPath));
                SocialMediaSettings = JsonSerializer.Deserialize<SocialMediaSettings>(File.ReadAllText(SocialMediaSettingsPath));
                SocialMediaSettings.isDirty = false;
                if (SocialMediaSettings.checkver())
                    needtosave = true;
            }
            SocialMediaSettings.Filename = SocialMediaSettingsPath;
            LoadsocialMediaSettings();

            SpawnSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\SpawnSettings.json";
            if (!File.Exists(SpawnSettingsPath))
            {
                SpawnSettings = new SpawnSettings();
                SpawnSettings.SetStartingWeapons();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(SpawnSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(SpawnSettingsPath));
                SpawnSettings = JsonSerializer.Deserialize<SpawnSettings>(File.ReadAllText(SpawnSettingsPath));
                SpawnSettings.SetStartingWeapons();
                SpawnSettings.isDirty = false;
                if (SpawnSettings.checkver())
                    needtosave = true;
            }
            SpawnSettings.Filename = SpawnSettingsPath;
            LoadSpawnsettings();

            TerritorySettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\TerritorySettings.json";
            if (!File.Exists(TerritorySettingsPath))
            {
                TerritorySettings = new TerritorySettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(TerritorySettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(TerritorySettingsPath));
                TerritorySettings = JsonSerializer.Deserialize<TerritorySettings>(File.ReadAllText(TerritorySettingsPath));
                TerritorySettings.isDirty = false;
                if (TerritorySettings.checkver())
                    needtosave = true;
            }
            TerritorySettings.Filename = TerritorySettingsPath;
            LoadTerritorySettings();

            VehicleSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Expansionmod\\settings\\VehicleSettings.json";
            if (!File.Exists(VehicleSettingsPath))
            {
                VehicleSettings = new VehicleSettings();
                needtosave = true;
                Console.WriteLine(Path.GetFileName(VehicleSettingsPath) + " File not found, Creating new....");
            }
            else
            {
                Console.WriteLine("serializing " + Path.GetFileName(VehicleSettingsPath));
                VehicleSettings = JsonSerializer.Deserialize<VehicleSettings>(File.ReadAllText(VehicleSettingsPath));
                VehicleSettings.isDirty = false;
                if (VehicleSettings.checkver())
                    needtosave = true;
            }
            VehicleSettings.Filename = VehicleSettingsPath;
            LoadvehicleSettings();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");
            
            tabControl3.ItemSize = new Size(0, 1);
            tabControl4.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
            }
        }
        public ContainerTypes getContainertype(string container)
        {
            switch (container)
            {
                case "ExpansionAirdropContainer":
                    return ContainerTypes.ExpansionAirdropContainer;
                case "ExpansionAirdropContainer_Basebuilding":
                    return ContainerTypes.ExpansionAirdropContainer_Basebuilding;
                case "ExpansionAirdropContainer_Blue":
                    return ContainerTypes.ExpansionAirdropContainer_Blue;
                case "ExpansionAirdropContainer_Grey":
                    return ContainerTypes.ExpansionAirdropContainer_Grey;
                case "ExpansionAirdropContainer_Medical":
                    return ContainerTypes.ExpansionAirdropContainer_Medical;
                case "ExpansionAirdropContainer_Military":
                    return ContainerTypes.ExpansionAirdropContainer_Military;
                case "ExpansionAirdropContainer_Military_GreenCamo":
                    return ContainerTypes.ExpansionAirdropContainer_Military_GreenCamo;
                case "ExpansionAirdropContainer_Military_MarineCamo":
                    return ContainerTypes.ExpansionAirdropContainer_Military_MarineCamo;
                case "ExpansionAirdropContainer_Military_OliveCamo":
                    return ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo;
                case "ExpansionAirdropContainer_Military_OliveCamo2":
                    return ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo2;
                case "ExpansionAirdropContainer_Military_WinterCamo":
                    return ContainerTypes.ExpansionAirdropContainer_Military_WinterCamo;
                case "ExpansionAirdropContainer_Olive":
                    return ContainerTypes.ExpansionAirdropContainer_Olive;
                default:
                    return ContainerTypes.ExpansionAirdropContainer;
            }
        }
        public String getContainerString(ContainerTypes containertype)
        {
            switch (containertype)
            {
                case ContainerTypes.ExpansionAirdropContainer:
                    return "ExpansionAirdropContainer";
                case ContainerTypes.ExpansionAirdropContainer_Medical:
                    return "ExpansionAirdropContainer_Medical";
                case ContainerTypes.ExpansionAirdropContainer_Military:
                    return "ExpansionAirdropContainer_Military";
                case ContainerTypes.ExpansionAirdropContainer_Basebuilding:
                    return "ExpansionAirdropContainer_Basebuilding";
                case ContainerTypes.ExpansionAirdropContainer_Grey:
                    return "ExpansionAirdropContainer_Grey";
                case ContainerTypes.ExpansionAirdropContainer_Blue:
                    return "ExpansionAirdropContainer_Blue";
                case ContainerTypes.ExpansionAirdropContainer_Olive:
                    return "ExpansionAirdropContainer_Olive";
                case ContainerTypes.ExpansionAirdropContainer_Military_GreenCamo:
                    return "ExpansionAirdropContainer_Military_GreenCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_MarineCamo:
                    return "ExpansionAirdropContainer_Military_MarineCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo:
                    return "ExpansionAirdropContainer_Military_OliveCamo";
                case ContainerTypes.ExpansionAirdropContainer_Military_OliveCamo2:
                    return "ExpansionAirdropContainer_Military_OliveCamo2";
                case ContainerTypes.ExpansionAirdropContainer_Military_WinterCamo:
                    return "ExpansionAirdropContainer_Military_WinterCamo";
                default:
                    return "ExpansionAirdropContainer";
            }
        }
        private void importDZEToMapFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string DestFile = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\objects\\" + Path.GetFileNameWithoutExtension(filePath) + ".map";
                    StringBuilder sb = new StringBuilder();
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    foreach (Editordeletedobject eod in importfile.EditorDeletedObjects)
                    {
                        sb.AppendLine("-" + eod.Type + "|" + eod.Position[0].ToString("R") + " " + eod.Position[1].ToString("R") + " " + eod.Position[2].ToString("R") + "|0.000000 0.000000 0.000000");
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        sb.AppendLine(eo.Type + "|" + eo.Position[0].ToString("R") + " " + eo.Position[1].ToString("R") + " " + eo.Position[2].ToString("R") + "|" + eo.Orientation[0].ToString("R") + " " + eo.Orientation[1].ToString("R") + " " + eo.Orientation[2].ToString("R"));
                    }
                    File.WriteAllText(DestFile, sb.ToString());
                    MessageBox.Show("File written to " + DestFile);
                }

            }
        }
        #endregion GeneralsettingFunctions

        #region savefiles
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles(bool updated = false)
        {
            if (!Directory.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings"))
                Directory.CreateDirectory(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings");
            if (!Directory.Exists(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings"))
                Directory.CreateDirectory(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");

            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (AirdropsettingsJson.isDirty)
            {
                AirdropsettingsJson.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AirdropsettingsJson, options);
                if (currentproject.Createbackups && File.Exists(AirdropsettingsJson.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AirdropsettingsJson.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AirdropsettingsJson.Filename, Path.GetDirectoryName(AirdropsettingsJson.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AirdropsettingsJson.Filename) + ".bak", true);
                }
                File.WriteAllText(AirdropsettingsJson.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AirdropsettingsJson.Filename));
            }
            if (BaseBuildingSettings.isDirty)
            {
                BaseBuildingSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(BaseBuildingSettings, options);
                if (currentproject.Createbackups && File.Exists(BaseBuildingSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(BaseBuildingSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(BaseBuildingSettings.Filename, Path.GetDirectoryName(BaseBuildingSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(BaseBuildingSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(BaseBuildingSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(BaseBuildingSettings.Filename));
            }
            if (BookSettings.isDirty)
            {
                BookSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(BookSettings, options);
                if (currentproject.Createbackups && File.Exists(BookSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(BookSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(BookSettings.Filename, Path.GetDirectoryName(BookSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(BookSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(BookSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(BookSettings.Filename));
            }
            if (ChatSettings.isDirty)
            {
                ChatSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ChatSettings, options);
                if (currentproject.Createbackups && File.Exists(ChatSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ChatSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ChatSettings.Filename, Path.GetDirectoryName(ChatSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ChatSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(ChatSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(ChatSettings.Filename));
            }
            if (DamageSystemSettings.isDirty)
            {
                DamageSystemSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(DamageSystemSettings, options);
                if (currentproject.Createbackups && File.Exists(DamageSystemSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(DamageSystemSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(DamageSystemSettings.Filename, Path.GetDirectoryName(DamageSystemSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(DamageSystemSettings.Filename) + ".bak", true);
                }
                DamageSystemSettings.ConvertListtodict();
                File.WriteAllText(DamageSystemSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(DamageSystemSettings.Filename));
            }
            if (DebugSettings.isDirty)
            {
                DebugSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(DebugSettings, options);
                if (currentproject.Createbackups && File.Exists(DebugSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(DebugSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(DebugSettings.Filename, Path.GetDirectoryName(DebugSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(DebugSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(DebugSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(DebugSettings.Filename));
            }

            if (GarageSettings.isDirty)
            {
                GarageSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(GarageSettings, options);
                if (currentproject.Createbackups && File.Exists(GarageSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(GarageSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(GarageSettings.Filename, Path.GetDirectoryName(GarageSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(GarageSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(GarageSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(GarageSettings.Filename));
            }

            if (GeneralSettings.isDirty)
            {
                GeneralSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(GeneralSettings, options);
                if (currentproject.Createbackups && File.Exists(GeneralSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(GeneralSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(GeneralSettings.Filename, Path.GetDirectoryName(GeneralSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(GeneralSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(GeneralSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(GeneralSettings.Filename));
            }
            if (HardLineSettings.isDirty)
            {
                HardLineSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                HardLineSettings.convertliststoDict();
                string jsonString = JsonSerializer.Serialize(HardLineSettings, options);
                if (currentproject.Createbackups && File.Exists(HardLineSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(HardLineSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(HardLineSettings.Filename, Path.GetDirectoryName(HardLineSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(HardLineSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(HardLineSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(HardLineSettings.Filename));
            }

            foreach (ExpansionHardlinePlayerData qpd in ExpansionHardlinePlayerDataList.HardlinePlayerDataList)
            {
                if (qpd.isDirty)
                {
                    qpd.isDirty = false;
                    if (currentproject.Createbackups && File.Exists(HardlinePlayerDataPath + "\\" + qpd.Filename + ".bin"))
                    {
                        Directory.CreateDirectory(HardlinePlayerDataPath + "\\Backup\\" + SaveTime);
                        File.Copy(HardlinePlayerDataPath + "\\" + qpd.Filename + ".bin", HardlinePlayerDataPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(qpd.Filename) + ".bak", true);
                    }
                    qpd.SaveFIle(HardlinePlayerDataPath);
                    midifiedfiles.Add(Path.GetFileName(qpd.Filename));
                }
            }


            if (LogSettings.isDirty)
            {
                LogSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LogSettings, options);
                if (currentproject.Createbackups && File.Exists(LogSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LogSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(LogSettings.Filename, Path.GetDirectoryName(LogSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(LogSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(LogSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(LogSettings.Filename));
            }
            if (MapSettings.isDirty)
            {
                MapSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MapSettings, options);
                if (currentproject.Createbackups && File.Exists(MapSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MapSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(MapSettings.Filename, Path.GetDirectoryName(MapSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MapSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(MapSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(MapSettings.Filename));
            }
            //if (marketsettings != null &&  marketsettings.isDirty)
            //{
            //    marketsettings.isDirty = false;
            //    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            //    string jsonString = JsonSerializer.Serialize(marketsettings, options);
            //    if (File.Exists(marketsettings.Filename))
            //    {
            //        Directory.CreateDirectory(Path.GetDirectoryName(marketsettings.Filename) + "\\Backup\\" + SaveTime);
            //        File.Copy(marketsettings.Filename, Path.GetDirectoryName(marketsettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(marketsettings.Filename) + ".bak", true);
            //    }
            //    File.WriteAllText(marketsettings.Filename, jsonString);
            //    midifiedfiles.Add(Path.GetFileName(marketsettings.Filename));
            //}
            if (MissionSettings.isDirty)
            {
                MissionSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MissionSettings, options);
                if (currentproject.Createbackups && File.Exists(MissionSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MissionSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(MissionSettings.Filename, Path.GetDirectoryName(MissionSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MissionSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(MissionSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(MissionSettings.Filename));
            }
            foreach (object msf in MissionSettings.MissionSettingFiles)
            {
                if (msf is AirdropMissionSettingFiles)
                {
                    AirdropMissionSettingFiles amsf = msf as AirdropMissionSettingFiles;
                    if (amsf.isDirty)
                    {
                        amsf.isDirty = false;
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(amsf, options);
                        if (currentproject.Createbackups && File.Exists(amsf.Filename))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(amsf.Filename) + "\\Backup\\" + SaveTime);
                            File.Copy(amsf.Filename, Path.GetDirectoryName(amsf.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(amsf.Filename) + ".bak", true);
                        }
                        File.WriteAllText(amsf.Filename, jsonString);
                        midifiedfiles.Add(Path.GetFileName(amsf.Filename));
                    }
                }
                else if (msf is ContaminatedAreaMissionSettingFiles)
                {
                    ContaminatedAreaMissionSettingFiles camsf = msf as ContaminatedAreaMissionSettingFiles;
                    if (camsf.isDirty)
                    {
                        camsf.isDirty = false;
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(camsf, options);
                        if (currentproject.Createbackups && File.Exists(camsf.Filename))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(camsf.Filename) + "\\Backup\\" + SaveTime);
                            File.Copy(camsf.Filename, Path.GetDirectoryName(camsf.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(camsf.Filename) + ".bak", true);
                        }
                        File.WriteAllText(camsf.Filename, jsonString);
                        midifiedfiles.Add(Path.GetFileName(camsf.Filename));
                    }
                }
            }
            if (MonitoringSettings.isDirty)
            {
                MonitoringSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MonitoringSettings, options);
                if (currentproject.Createbackups && File.Exists(MonitoringSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MonitoringSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(MonitoringSettings.Filename, Path.GetDirectoryName(MonitoringSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MonitoringSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(MonitoringSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(MonitoringSettings.Filename));
            }
            if (NameTagSettings.isDirty)
            {
                NameTagSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(NameTagSettings, options);
                if (currentproject.Createbackups && File.Exists(NameTagSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(NameTagSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(NameTagSettings.Filename, Path.GetDirectoryName(NameTagSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(NameTagSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(NameTagSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(NameTagSettings.Filename));
            }
            if (NotificationSchedulerSettings.isDirty)
            {
                NotificationSchedulerSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(NotificationSchedulerSettings, options);
                if (currentproject.Createbackups && File.Exists(NotificationSchedulerSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(NotificationSchedulerSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(NotificationSchedulerSettings.Filename, Path.GetDirectoryName(NotificationSchedulerSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(NotificationSchedulerSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(NotificationSchedulerSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(NotificationSchedulerSettings.Filename));
            }
            if (NotificationSettings.isDirty)
            {
                NotificationSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(NotificationSettings, options);
                if (currentproject.Createbackups && File.Exists(NotificationSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(NotificationSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(NotificationSettings.Filename, Path.GetDirectoryName(NotificationSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(NotificationSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(NotificationSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(NotificationSettings.Filename));
            }

            if (PartySettings.isDirty)
            {
                PartySettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(PartySettings, options);
                if (File.Exists(PartySettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PartySettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(PartySettings.Filename, Path.GetDirectoryName(PartySettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(PartySettings.Filename) + ".bak", true);
                }
                File.WriteAllText(PartySettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(PartySettings.Filename));
            }

            if (PersonalStorageSettings.isDirty)
            {
                PersonalStorageSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(PersonalStorageSettings, options);
                if (File.Exists(PersonalStorageSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PersonalStorageSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(PersonalStorageSettings.Filename, Path.GetDirectoryName(PersonalStorageSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(PersonalStorageSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(PersonalStorageSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(PersonalStorageSettings.Filename));
            }
            foreach (PersonalStorage ps in PersonalStorageList.personalstorageList)
            {
                if (ps.isDirty)
                {
                    ps.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(ps, options);
                    if (currentproject.Createbackups && File.Exists(ps.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(ps.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(ps.Filename, Path.GetDirectoryName(ps.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ps.Filename) + ".bak", true);
                    }
                    File.WriteAllText(ps.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(ps.Filename));
                }
            }



            if (PlayerListSettings.isDirty)
            {
                PlayerListSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(PlayerListSettings, options);
                if (currentproject.Createbackups && File.Exists(PlayerListSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PlayerListSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(PlayerListSettings.Filename, Path.GetDirectoryName(PlayerListSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(PlayerListSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(PlayerListSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(PlayerListSettings.Filename));
            }
            if (RaidSettings.isDirty)
            {
                RaidSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(RaidSettings, options);
                if (currentproject.Createbackups && File.Exists(RaidSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RaidSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(RaidSettings.Filename, Path.GetDirectoryName(RaidSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(RaidSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(RaidSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(RaidSettings.Filename));
            }
            if (SafeZoneSettings.isDirty)
            {
                SafeZoneSettings.isDirty = false;
                SafeZoneSettings.convertpointstoarray();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(SafeZoneSettings, options);
                if (currentproject.Createbackups && File.Exists(SafeZoneSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SafeZoneSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SafeZoneSettings.Filename, Path.GetDirectoryName(SafeZoneSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SafeZoneSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(SafeZoneSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SafeZoneSettings.Filename));
            }
            if (SocialMediaSettings.isDirty)
            {
                SocialMediaSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(SocialMediaSettings, options);
                if (currentproject.Createbackups && File.Exists(SocialMediaSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SocialMediaSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SocialMediaSettings.Filename, Path.GetDirectoryName(SocialMediaSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SocialMediaSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(SocialMediaSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SocialMediaSettings.Filename));
            }
            if (SpawnSettings.isDirty)
            {
                SpawnSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                options.Converters.Add(new NullToEmptyGearConverter());
                string jsonString = JsonSerializer.Serialize(SpawnSettings, options);
                if (currentproject.Createbackups && File.Exists(SpawnSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SpawnSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SpawnSettings.Filename, Path.GetDirectoryName(SpawnSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SpawnSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(SpawnSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SpawnSettings.Filename));
            }
            if (TerritorySettings.isDirty)
            {
                TerritorySettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TerritorySettings, options);
                if (currentproject.Createbackups && File.Exists(TerritorySettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TerritorySettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(TerritorySettings.Filename, Path.GetDirectoryName(TerritorySettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TerritorySettings.Filename) + ".bak", true);
                }
                File.WriteAllText(TerritorySettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TerritorySettings.Filename));
            }
            if (VehicleSettings.isDirty)
            {
                VehicleSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(VehicleSettings, options);
                if (currentproject.Createbackups && File.Exists(VehicleSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(VehicleSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(VehicleSettings.Filename, Path.GetDirectoryName(VehicleSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(VehicleSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(VehicleSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(VehicleSettings.Filename));
            }
            bool RemovedOnly = true;
            string message = "The Following Files were saved....\n";
            if(updated)
            {
                message = "The following files were either Created or Updated...\n";
            }
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                RemovedOnly = false;
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
            if(RemovedOnly)
            {
                message = "";
            }
            else
            {
                message += "\n";
            }
            if (PersonalStorageList.Markedfordelete != null)
            {
                message += "The following Personal Storage configs were Removed\n";
                i = 0;
                foreach (PersonalStorage del in PersonalStorageList.Markedfordelete)
                {
                    del.backupandDelete(PersonalStoragePath);
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
                    PersonalStorageList.UsedIDS.Remove(del.StorageID);
                }
                PersonalStorageList.Markedfordelete = null;
            }
            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void ExpansionSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (AirdropsettingsJson.isDirty)
            {
                needtosave = true;
            }
            if (BaseBuildingSettings.isDirty)
            {
                needtosave = true;
            }
            if (BookSettings.isDirty)
            {
                needtosave = true; ;
            }
            if (ChatSettings.isDirty)
            {
                needtosave = true;
            }
            if (DamageSystemSettings.isDirty)
            {
                needtosave = true;
            }
            if (DebugSettings.isDirty)
            {
                needtosave = true;
            }
            if (GeneralSettings.isDirty)
            {
                needtosave = true;
            }
            if (HardLineSettings.isDirty)
            {
                needtosave = true;
            }
            if (LogSettings.isDirty)
            {
                needtosave = true;
            }
            if (MapSettings.isDirty)
            {
                needtosave = true;
            }
            if (MissionSettings.isDirty)
            {
                needtosave = true;
            }
            foreach (object msf in MissionSettings.MissionSettingFiles)
            {
                if (msf is AirdropMissionSettingFiles)
                {
                    AirdropMissionSettingFiles amsf = msf as AirdropMissionSettingFiles;
                    if (amsf.isDirty)
                    {
                        needtosave = true;
                    }
                }
                else if (msf is ContaminatedAreaMissionSettingFiles)
                {
                    ContaminatedAreaMissionSettingFiles camsf = msf as ContaminatedAreaMissionSettingFiles;
                    if (camsf.isDirty)
                    {
                        needtosave = true;
                    }
                }
            }
            if (MonitoringSettings.isDirty)
            {
                needtosave = true;
            }
            if (NameTagSettings.isDirty)
            {
                needtosave = true;
            }
            if (NotificationSchedulerSettings.isDirty)
            {
                needtosave = true;
            }
            if (NotificationSettings.isDirty)
            {
                needtosave = true;
            }
            if (PersonalStorageSettings.isDirty)
            {
                needtosave = true;
            }
            foreach (PersonalStorage ps in PersonalStorageList.personalstorageList)
            {
                if (ps.isDirty)
                {
                    needtosave = true;
                }
            }
            if (PartySettings.isDirty)
            {
                needtosave = true;
            }
            if (PlayerListSettings.isDirty)
            {
                needtosave = true;
            }
            if (RaidSettings.isDirty)
            {
                needtosave = true;
            }
            if (SafeZoneSettings.isDirty)
            {
                needtosave = true;
            }
            if (SocialMediaSettings.isDirty)
            {
                needtosave = true;
            }
            if (SpawnSettings.isDirty)
            {
                needtosave = true;
            }
            if (TerritorySettings.isDirty)
            {
                needtosave = true;
            }
            if (VehicleSettings.isDirty)
            {
                needtosave = true;
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
        #endregion savefiles

        #region Airdropsettings
        public AirdropContainers CurrentAirdropContainer;
        public containerLoot CurrentAirdropContainerLoot;
        private NoBuildZones currentZone;
        public lootVarients LootVarients;

        private void SetupAirdropsettings()
        {
            populate();
            setupairdropZombies(listBox5);
        }
        private void setupairdropZombies(ListBox lb)
        {
            lb.Items.Clear();
            foreach (typesType type in vanillatypes.SerachTypes("zmbm_"))
            {
                lb.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("zmbf_"))
            {
                lb.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("animal_"))
            {
                lb.Items.Add(type.name);
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbm_"))
                {
                    lb.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbf_"))
                {
                    lb.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("animal_"))
                {
                    lb.Items.Add(type.name);
                }
            }
        }
        public TreeNode returntreenode(List<LootPart> partlist, string name, bool ignoredisabled = false)
        {
            TreeNode tn = new TreeNode(name)
            {
                Tag = name
            };
            foreach (LootPart lp in partlist)
            {
                TreeNode partnod = new TreeNode(lp.name);
                tn.Nodes.Add(partnod);
            }
            return tn;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (ExpansionSettingsTabPage.SelectedIndex)
            {
                case 0:
                case 1:
                case 3:
                case 6:
                case 7:
                case 8:
                case 10:
                case 11:
                case 13:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\expansionMod\\Settings");
                    break;
                case 2:
                case 4:
                case 5:
                case 9:
                case 12:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
            }
        }
        private void populate()
        {
            useraction = false;
            checkBox1.Checked = AirdropsettingsJson.ServerMarkerOnDropLocation == 1 ? true : false;
            checkBox2.Checked = AirdropsettingsJson.Server3DMarkerOnDropLocation == 1 ? true : false;
            checkBox3.Checked = AirdropsettingsJson.ShowAirdropTypeOnMarker == 1 ? true : false;
            checkBox4.Checked = AirdropsettingsJson.HeightIsRelativeToGroundLevel == 1 ? true : false;
            checkBox8.Checked = AirdropsettingsJson.HideCargoWhileParachuteIsDeployed == 1 ? true : false;
            numericUpDown1.Value = (decimal)AirdropsettingsJson.Height;
            numericUpDown2.Value = (decimal)AirdropsettingsJson.FollowTerrainFraction;
            numericUpDown3.Value = (decimal)AirdropsettingsJson.Speed;
            numericUpDown4.Value = (decimal)AirdropsettingsJson.Radius;
            numericUpDown5.Value = (decimal)AirdropsettingsJson.InfectedSpawnRadius;
            numericUpDown6.Value = (decimal)AirdropsettingsJson.InfectedSpawnInterval;
            numericUpDown7.Value = (decimal)AirdropsettingsJson.ItemCount;
            PopelateContainerList();
            useraction = true;
        }
        private void PopelateContainerList()
        {
            listBox2.DisplayMember = "DisplayName";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = AirdropsettingsJson.Containers;
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count < 1) return;
            CurrentAirdropContainer = listBox2.SelectedItem as AirdropContainers;
            useraction = false;
            comboBox2.SelectedItem = getContainertype(CurrentAirdropContainer.Container);
            numericUpDown8.Value = CurrentAirdropContainer.Usage;
            numericUpDown32.Value = CurrentAirdropContainer.FallSpeed;
            numericUpDown9.Value = (decimal)CurrentAirdropContainer.Weight;
            numericUpDown10.Value = CurrentAirdropContainer.ItemCount;
            numericUpDown11.Value = CurrentAirdropContainer.InfectedCount;
            checkBox5.Checked = CurrentAirdropContainer.SpawnInfectedForPlayerCalledDrops.Equals(1) ? true : false;
            darkLabel19.Text = "Number of Loot Items = " + CurrentAirdropContainer.Loot.Count.ToString();
            darkLabel20.Text = "Number of Infected = " + CurrentAirdropContainer.Infected.Count.ToString();
            populatelistbox();
            populateZombies();
            useraction = true;
        }
        private void populatelistbox()
        {
            listBox1.DisplayMember = "DisplayName";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = CurrentAirdropContainer.Loot;
        }
        private void populateZombies()
        {
            listBox3.DisplayMember = "DisplayName";
            listBox3.ValueMember = "Value";
            listBox3.DataSource = CurrentAirdropContainer.Infected;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            CurrentAirdropContainerLoot = listBox1.SelectedItem as containerLoot;
            useraction = false;
            textBox1.Text = CurrentAirdropContainerLoot.Name;
            if (CurrentAirdropContainerLoot.Chance > 1)
                CurrentAirdropContainerLoot.Chance = 1;
            trackBar1.Value = (int)(CurrentAirdropContainerLoot.Chance * 1000);
            numericUpDown12.Value = CurrentAirdropContainerLoot.Max;
            numericUpDown33.Value = CurrentAirdropContainerLoot.Min;
            numericUpDown31.Value = CurrentAirdropContainerLoot.QuantityPercent;
            listBox4.DisplayMember = "DisplayName";
            listBox4.ValueMember = "Value";
            listBox4.DataSource = CurrentAirdropContainerLoot.Attachments;

            listBox21.DataSource = null;
            listBox22.DataSource = null;

            if (CurrentAirdropContainerLoot.Variants.Count > 0)
            {
                listBox21.DisplayMember = "DisplayName";
                listBox21.ValueMember = "Value";
                listBox21.DataSource = CurrentAirdropContainerLoot.Variants;
            }
            useraction = true;
        }
        private void listBox21_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox21.SelectedItems.Count < 1) return;
            LootVarients = listBox21.SelectedItem as lootVarients;
            useraction = false;

            VarientChanceTrackBar.Value = (int)(LootVarients.Chance * 1000);

            listBox22.DisplayMember = "DisplayName";
            listBox22.ValueMember = "Value";
            listBox22.DataSource = LootVarients.Attachments;

            useraction = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            CurrentAirdropContainer.Loot.Remove(CurrentAirdropContainerLoot);
            AirdropsettingsJson.isDirty = true;
            populatelistbox();
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AirdropsettingsJson.Containers.Remove(CurrentAirdropContainer);
            AirdropsettingsJson.isDirty = true;
            PopelateContainerList();
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            List<string> removezombies = new List<string>();
            foreach (var item in listBox3.SelectedItems)
            {
                removezombies.Add(item.ToString());

            }
            foreach (string s in removezombies)
            {
                CurrentAirdropContainer.Infected.Remove(s);
                AirdropsettingsJson.isDirty = true;
            }
            darkLabel20.Text = "Number of Infected = " + CurrentAirdropContainer.Infected.Count.ToString();
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox5.SelectedItems)
            {
                string zombie = item.ToString();
                if (!CurrentAirdropContainer.Infected.Contains(zombie))
                {
                    CurrentAirdropContainer.Infected.Add(zombie);
                    AirdropsettingsJson.isDirty = true;
                    darkLabel20.Text = "Number of Infected = " + CurrentAirdropContainer.Infected.Count.ToString();
                }
                else
                {
                    MessageBox.Show("Infected Type allready in the list.....");
                }
            }
        }
        private void darkButton84_Click(object sender, EventArgs e)
        {
            string zombie = textBox20.Text;
            if (!CurrentAirdropContainer.Infected.Contains(zombie))
            {
                CurrentAirdropContainer.Infected.Add(zombie);
                AirdropsettingsJson.isDirty = true;
                darkLabel20.Text = "Number of Infected = " + CurrentAirdropContainer.Infected.Count.ToString();
            }
            else
            {
                MessageBox.Show("Infected Type allready in the list.....");
            }
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            AirdropContainers NewContainer = new AirdropContainers();
            Add_new_container anc = new Add_new_container();
            DialogResult result = anc.ShowDialog();
            if (result == DialogResult.OK)
            {
                NewContainer.Container = anc.containerType;
                NewContainer.FallSpeed = anc.getFallspeedValue;
                NewContainer.Usage = anc.getUsagevalue;
                NewContainer.Weight = (decimal)anc.getWeightValue;
                NewContainer.Loot = new BindingList<containerLoot>();
                NewContainer.Infected = new BindingList<string>();
                NewContainer.ItemCount = anc.getItemevalue;
                NewContainer.InfectedCount = anc.getinfectedcountValue;
                NewContainer.SpawnInfectedForPlayerCalledDrops = anc.GetSpawnInfected.Equals(true) ? 1 : 0;
                AirdropsettingsJson.Containers.Add(NewContainer);
                AirdropsettingsJson.isDirty = true;

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
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
                    containerLoot Newloot = new containerLoot()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = (decimal)0.5,
                        Max = -1,
                        Min = 0,
                        Variants = new BindingList<lootVarients>()
                    };
                    CurrentAirdropContainer.Loot.Add(Newloot);
                    AirdropsettingsJson.isDirty = true;
                    darkLabel19.Text = "Number of Loot Items = " + CurrentAirdropContainer.Loot.Count.ToString();
                }
            }
        }
        private void darkButton9_Click(object sender, EventArgs e)
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
                    if (!CurrentAirdropContainerLoot.Attachments.Contains(l))
                    {
                        CurrentAirdropContainerLoot.Attachments.Add(l);
                        AirdropsettingsJson.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the list.....");
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            CurrentAirdropContainerLoot.Attachments.Remove(listBox4.GetItemText(listBox4.SelectedItem));
            AirdropsettingsJson.isDirty = true;
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.InfectedCount = (int)numericUpDown11.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.Container = getContainerString((ContainerTypes)comboBox2.SelectedItem);
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.Usage = (int)numericUpDown8.Value;
                AirdropsettingsJson.isDirty = true;

            }
        }
        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.FallSpeed = numericUpDown32.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.Weight = (decimal)numericUpDown9.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainer.ItemCount = (int)numericUpDown10.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAirdropContainer.SpawnInfectedForPlayerCalledDrops = checkBox5.Checked == true ? 1 : 0;
            AirdropsettingsJson.isDirty = true;
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainerLoot.Max = (int)numericUpDown12.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainerLoot.Min = (int)numericUpDown33.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                CurrentAirdropContainerLoot.QuantityPercent = (int)numericUpDown31.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.ServerMarkerOnDropLocation = checkBox1.Checked == true ? 1 : 0;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.Server3DMarkerOnDropLocation = checkBox1.Checked == true ? 1 : 0;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.ShowAirdropTypeOnMarker = checkBox3.Checked == true ? 1 : 0;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.HeightIsRelativeToGroundLevel = checkBox1.Checked == true ? 1 : 0;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.HideCargoWhileParachuteIsDeployed = checkBox8.Checked == true ? 1 : 0;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.Height = (decimal)numericUpDown1.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.FollowTerrainFraction = (decimal)numericUpDown2.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.Speed = (decimal)numericUpDown3.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.Radius = (decimal)numericUpDown4.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.InfectedSpawnRadius = (decimal)numericUpDown5.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.InfectedSpawnInterval = (int)numericUpDown6.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                AirdropsettingsJson.ItemCount = (int)numericUpDown7.Value;
                AirdropsettingsJson.isDirty = true;
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            darkLabel23.Text = ((decimal)(trackBar1.Value) / 10).ToString() + "%";
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            darkLabel23.Text = ((decimal)(trackBar1.Value) / 10).ToString() + "%";
        }
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentAirdropContainerLoot == null) return;
            CurrentAirdropContainerLoot.Chance = ((decimal)trackBar1.Value) / 1000;
            AirdropsettingsJson.isDirty = true;
        }
        private void darkButton52_Click(object sender, EventArgs e)
        {
            foreach (containerLoot cl in CurrentAirdropContainer.Loot)
            {
                cl.Chance = ((decimal)trackBar1.Value) / 1000;
            }
            AirdropsettingsJson.isDirty = true;
        }
        private void VarientChanceTrackBar_ValueChanged(object sender, EventArgs e)
        {
            darkLabel159.Text = ((decimal)(VarientChanceTrackBar.Value) / 10).ToString() + "%";
        }
        private void VarientChanceTrackBar_Scroll(object sender, EventArgs e)
        {
            darkLabel159.Text = ((decimal)(VarientChanceTrackBar.Value) / 10).ToString() + "%";
        }
        private void VarientChanceTrackBar_MouseUp(object sender, MouseEventArgs e)
        { 
            if(LootVarients == null)return;
            LootVarients.Chance =((decimal)VarientChanceTrackBar.Value) / 1000;
            AirdropsettingsJson.isDirty = true;
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
                if (CurrentAirdropContainerLoot.Variants.Count == 0)
                {
                    listBox21.DisplayMember = "DisplayName";
                    listBox21.ValueMember = "Value";
                    listBox21.DataSource = CurrentAirdropContainerLoot.Variants;
                }
                foreach (string l in addedtypes)
                {
                    lootVarients newlootVarients = new lootVarients()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = (decimal)0.5,
                    };
                    CurrentAirdropContainerLoot.Variants.Add(newlootVarients);
                    AirdropsettingsJson.isDirty = true;
                }
               
            }
        }
        private void darkButton56_Click(object sender, EventArgs e)
        {
            CurrentAirdropContainerLoot.Variants.Remove(LootVarients);
            listBox22.DataSource = null;
            listBox21.SelectedIndex = -1;
            if (CurrentAirdropContainerLoot.Variants.Count > 0)
            {
                listBox21.SelectedIndex = 0;
            }
            AirdropsettingsJson.isDirty = true;
        }
        private void darkButton57_Click(object sender, EventArgs e)
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
                    if (!LootVarients.Attachments.Contains(l))
                    {
                        LootVarients.Attachments.Add(l);
                        AirdropsettingsJson.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the Varients list.....");
                }
            }
        }
        private void darkButton58_Click(object sender, EventArgs e)
        {
            LootVarients.Attachments.Remove(listBox22.GetItemText(listBox22.SelectedItem));
            AirdropsettingsJson.isDirty = true;
        }
        private void darkButton97_Click(object sender, EventArgs e)
        {

        }
        private void darkButton97_Click_1(object sender, EventArgs e)
        {
            string lootitem = CurrentAirdropContainerLoot.Name;
            BindingList<Spawnabletypesconfig> spawnabletypes = currentproject.spawnabletypesList;
            foreach(Spawnabletypesconfig stc in spawnabletypes)
            {
                foreach(spawnabletypesType st in stc.spawnabletypes.type)
                {
                    if(st.name == lootitem)
                    {
                        
                    }
                }
            }
        }

        #endregion Airdropsettings

        #region basebuildingsettings
        public int BaseBuildingMapscale = 1;

        private void Setupbasebuildingsettings()
        {
            useraction = false;
            CodelockAttachModeCB.DataSource = Enum.GetValues(typeof(canattacchcodelock));
            FlagMenuModeComboBox.DataSource = Enum.GetValues(typeof(FlagMenuMode));
            DismantleFlagModeComboBox.DataSource = Enum.GetValues(typeof(DismantleFlagMode));
            CanBuildAnywhereCB.Checked = BaseBuildingSettings.CanBuildAnywhere == 1 ? true : false;
            AllowBuildingWithoutATerritoryCB.Checked = BaseBuildingSettings.AllowBuildingWithoutATerritory == 1 ? true : false;
            CanCraftVanillaBasebuildingCB.Checked = BaseBuildingSettings.CanCraftVanillaBasebuilding == 1 ? true : false;
            CanCraftExpansionBasebuildingCB.Checked = BaseBuildingSettings.CanCraftExpansionBasebuilding == 1 ? true : false;
            DestroyFlagOnDismantleCB.Checked = BaseBuildingSettings.DestroyFlagOnDismantle == 1 ? true : false;
            DismantleFlagModeComboBox.SelectedItem = (DismantleFlagMode)BaseBuildingSettings.DismantleFlagMode;
            DismantleOutsideTerritoryCB.Checked = BaseBuildingSettings.DismantleOutsideTerritory == 1 ? true : false;
            DismantleInsideTerritoryCB.Checked = BaseBuildingSettings.DismantleInsideTerritory == 1 ? true : false;
            DismantleAnywhereCB.Checked = BaseBuildingSettings.DismantleAnywhere == 1 ? true : false;
            CodelockAttachModeCB.SelectedItem = (canattacchcodelock)BaseBuildingSettings.CodelockAttachMode;
            CodelockActionsAnywhereCB.Checked = BaseBuildingSettings.CodelockActionsAnywhere == 1 ? true : false;
            CodeLockLengthNUD.Value = BaseBuildingSettings.CodeLockLength;
            DoDamageWhenEnterWrongCodeLockCB.Checked = BaseBuildingSettings.DoDamageWhenEnterWrongCodeLock == 1 ? true : false;
            DamageWhenEnterWrongCodeLockNUD.Value = (decimal)BaseBuildingSettings.DamageWhenEnterWrongCodeLock;
            RememberCodeCB.Checked = BaseBuildingSettings.RememberCode == 1 ? true : false;
            CanCraftTerritoryFlagKitCB.Checked = BaseBuildingSettings.CanCraftTerritoryFlagKit == 1 ? true : false;
            SimpleTerritoryCB.Checked = BaseBuildingSettings.SimpleTerritory == 1 ? true : false;
            AutomaticFlagOnCreationCB.Checked = BaseBuildingSettings.AutomaticFlagOnCreation == 1 ? true : false;
            FlagMenuModeComboBox.SelectedItem = (FlagMenuMode)BaseBuildingSettings.FlagMenuMode;
            GetTerritoryFlagKitAfterBuildCB.Checked = BaseBuildingSettings.GetTerritoryFlagKitAfterBuild == 1 ? true : false;
            textBox2.Text = BaseBuildingSettings.BuildZoneRequiredCustomMessage;
            ZonesAreNoBuildZonesCB.Checked = BaseBuildingSettings.ZonesAreNoBuildZones == 1 ? true : false;
            EnableVirtualStorageCB.Checked = BaseBuildingSettings.EnableVirtualStorage == 1 ? true : false;

            listBox6.DisplayMember = "DisplayName";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = BaseBuildingSettings.DeployableOutsideATerritory;

            listBox7.DisplayMember = "DisplayName";
            listBox7.ValueMember = "Value";
            listBox7.DataSource = BaseBuildingSettings.DeployableInsideAEnemyTerritory;

            listBox8.DisplayMember = "DisplayName";
            listBox8.ValueMember = "Value";
            listBox8.DataSource = BaseBuildingSettings.Zones;

            VirtualStorageExcludedContainersLB.DisplayMember = "DisplayName";
            VirtualStorageExcludedContainersLB.ValueMember = "Value";
            VirtualStorageExcludedContainersLB.DataSource = BaseBuildingSettings.VirtualStorageExcludedContainers;

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAll);
            trackBar2.Value = 1;
            SetsBBcale();
            useraction = true;
        }
        private void SimpleTerritoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.SimpleTerritory = SimpleTerritoryCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void AllowBuildingWithoutATerritoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.AllowBuildingWithoutATerritory = AllowBuildingWithoutATerritoryCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void CanBuildAnywhereCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CanBuildAnywhere = CanBuildAnywhereCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void AutomaticFlagOnCreationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.AutomaticFlagOnCreation = AutomaticFlagOnCreationCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void DismantleOutsideTerritoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DismantleOutsideTerritory = DismantleOutsideTerritoryCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void DismantleInsideTerritoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DismantleInsideTerritory = DismantleInsideTerritoryCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void CanCraftVanillaBasebuildingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CanCraftVanillaBasebuilding = CanCraftVanillaBasebuildingCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void CanCraftExpansionBasebuildingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CanCraftExpansionBasebuilding = CanCraftExpansionBasebuildingCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void CanCraftTerritoryFlagKitCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CanCraftTerritoryFlagKit = CanCraftTerritoryFlagKitCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void DestroyFlagOnDismantleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DestroyFlagOnDismantle = DestroyFlagOnDismantleCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void DismantleAnywhereCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DismantleAnywhere = DismantleAnywhereCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void GetTerritoryFlagKitAfterBuildCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.GetTerritoryFlagKitAfterBuild = GetTerritoryFlagKitAfterBuildCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            canattacchcodelock cacl = (canattacchcodelock)CodelockAttachModeCB.SelectedItem;
            BaseBuildingSettings.CodelockAttachMode = (int)cacl;
            BaseBuildingSettings.isDirty = true;
        }
        private void CodelockActionsAnywhereCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CodelockActionsAnywhere = CodelockActionsAnywhereCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void CodeLockLengthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.CodeLockLength = (int)CodeLockLengthNUD.Value;
            BaseBuildingSettings.isDirty = true;
        }
        private void DoDamageWhenEnterWrongCodeLockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DoDamageWhenEnterWrongCodeLock = DoDamageWhenEnterWrongCodeLockCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void DamageWhenEnterWrongCodeLockNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.DamageWhenEnterWrongCodeLock = (float)DamageWhenEnterWrongCodeLockNUD.Value;
            BaseBuildingSettings.isDirty = true;
        }
        private void RememberCodeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.RememberCode = RememberCodeCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.BuildZoneRequiredCustomMessage = textBox2.Text;
            BaseBuildingSettings.isDirty = true;
        }
        private void ZonesAreNoBuildZonesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.ZonesAreNoBuildZones = ZonesAreNoBuildZonesCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            BaseBuildingSettings.DeployableOutsideATerritory.Remove(listBox6.GetItemText(listBox6.SelectedItem));
            BaseBuildingSettings.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
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
                    if (!BaseBuildingSettings.DeployableOutsideATerritory.Contains(l))
                    {
                        BaseBuildingSettings.DeployableOutsideATerritory.Add(l);
                        BaseBuildingSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
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
                    if (!BaseBuildingSettings.DeployableInsideAEnemyTerritory.Contains(l))
                    {
                        BaseBuildingSettings.DeployableInsideAEnemyTerritory.Add(l);
                        BaseBuildingSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            BaseBuildingSettings.DeployableInsideAEnemyTerritory.Remove(listBox7.GetItemText(listBox7.SelectedItem));
            BaseBuildingSettings.isDirty = true;
        }
        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

            currentZone = listBox8.SelectedItem as NoBuildZones;
            if(currentZone == null) { return; }
            useraction = false;
            textBox3.Text = currentZone.Name;

            numericUpDown13.Value = (decimal)currentZone.Radius;
            numericUpDown14.Value = (decimal)currentZone.Center[0];
            numericUpDown15.Value = (decimal)currentZone.Center[1];
            numericUpDown16.Value = (decimal)currentZone.Center[2];
            checkBox6.Checked = currentZone.IsWhitelist == 1 ? true : false;
            textBox4.Text = currentZone.CustomMessage;
            listBox9.DisplayMember = "DisplayName";
            listBox9.ValueMember = "Value";
            listBox9.DataSource = currentZone.Items;
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            BaseBuildingMapscale = trackBar2.Value;
            SetsBBcale();

        }
        private void SetsBBcale()
        {
            float scalevalue = BaseBuildingMapscale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawAll(object sender, PaintEventArgs e)
        {
            float scalevalue = BaseBuildingMapscale * 0.05f;
            foreach (NoBuildZones zones in BaseBuildingSettings.Zones)
            {
                int centerX = (int)(Math.Round(zones.Center[0], 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(zones.Center[2], 0) * scalevalue);

                int radius = (int)(zones.Radius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentZone.Name == zones.Name)
                    pen.Color = Color.LimeGreen;
                else
                    pen.Color = Color.Red;
                getCircle(e.Graphics, pen, center, radius);
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void darkButton15_Click(object sender, EventArgs e)
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
                    if (!currentZone.Items.Contains(l))
                    {
                        currentZone.Items.Add(l);
                        BaseBuildingSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            currentZone.Items.Remove(listBox9.GetItemText(listBox9.SelectedItem));
            BaseBuildingSettings.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            NoBuildZones newzone = new NoBuildZones()
            {
                Name = "new No Build Zone",
                CustomMessage = "",
                Radius = 100,
                Center = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 },
                IsWhitelist = 0,
                Items = new BindingList<string>()
            };
            BaseBuildingSettings.Zones.Add(newzone);
            BaseBuildingSettings.isDirty = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            BaseBuildingSettings.Zones.Remove(currentZone);
            BaseBuildingSettings.isDirty = true;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.Name = textBox3.Text;
            BaseBuildingSettings.isDirty = true;
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.CustomMessage = textBox4.Text;
            BaseBuildingSettings.isDirty = true;
        }
        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.Radius = (float)numericUpDown13.Value;
            BaseBuildingSettings.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.Center[0] = (float)numericUpDown14.Value;
            BaseBuildingSettings.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.Center[1] = (float)numericUpDown15.Value;
            BaseBuildingSettings.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.Center[2] = (float)numericUpDown16.Value;
            BaseBuildingSettings.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZone.IsWhitelist = checkBox6.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = BaseBuildingMapscale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (currentZone == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                numericUpDown14.Value = (decimal)(mouseEventArgs.X / scalevalue);
                numericUpDown16.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                if (MapData.FileExists)
                {
                    numericUpDown15.Value = (decimal)(MapData.gethieght(currentZone.Center[0], currentZone.Center[2]));
                }
                Cursor.Current = Cursors.Default;
                BaseBuildingSettings.isDirty = true;
                pictureBox1.Invalidate();
            }
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 1;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 0;
        }
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl3.SelectedIndex)
            {
                case 0:
                    toolStripButton7.Checked = false;
                    toolStripButton8.Checked = true;
                    toolStripButton7.ForeColor = Color.FromArgb(75, 110, 175);
                    toolStripButton8.ForeColor = SystemColors.Control;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton7.Checked = true;
                    toolStripButton8.ForeColor = Color.FromArgb(75, 110, 175);
                    toolStripButton7.ForeColor = SystemColors.Control;
                    break;
                default:
                    break;
            }
        }
        private void FlagMenuModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            FlagMenuMode cacl = (FlagMenuMode)FlagMenuModeComboBox.SelectedItem;
            BaseBuildingSettings.FlagMenuMode = (int)cacl;
            BaseBuildingSettings.isDirty = true;
        }
        private void DismantleFlagModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DismantleFlagMode cacl = (DismantleFlagMode)DismantleFlagModeComboBox.SelectedItem;
            BaseBuildingSettings.DismantleFlagMode = (int)cacl;
            BaseBuildingSettings.isDirty = true;
        }
        private void darkButton75_Click(object sender, EventArgs e)
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
                    if (!BaseBuildingSettings.VirtualStorageExcludedContainers.Contains(l))
                    {
                        BaseBuildingSettings.VirtualStorageExcludedContainers.Add(l);
                        BaseBuildingSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton76_Click(object sender, EventArgs e)
        {
            BaseBuildingSettings.VirtualStorageExcludedContainers.Remove(VirtualStorageExcludedContainersLB.GetItemText(VirtualStorageExcludedContainersLB.SelectedItem));
            BaseBuildingSettings.isDirty = true;
        }
        private void EnableVirtualStorageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseBuildingSettings.EnableVirtualStorage = EnableVirtualStorageCB.Checked == true ? 1 : 0;
            BaseBuildingSettings.isDirty = true;
        }
        #endregion basebuildingsettings

        #region BookSettings
        public Rulecats CurrentRulecat;
        public Rules currentRules;
        public Descript currentDescription;
        public DT currentdecriptiontext;
        public Links currentLink;
        public CraftingCategories CurrentCraftingCategory;

        public void loadBookSettings()
        {
            useraction = false;
            EnableStatusTabCB.Checked = BookSettings.EnableStatusTab == 1 ? true : false;
            EnablePartyTabCB.Checked = BookSettings.EnablePartyTab == 1 ? true : false;
            EnableServerInfoTabCB.Checked = BookSettings.EnableServerInfoTab == 1 ? true : false;
            EnableServerRulesTabCB.Checked = BookSettings.EnableServerRulesTab == 1 ? true : false;
            EnableTerritoryTabCB.Checked = BookSettings.EnableTerritoryTab == 1 ? true : false;
            EnableBookMenuCB.Checked = BookSettings.EnableBookMenu == 1 ? true : false;
            CreateBookmarksCB.Checked = BookSettings.CreateBookmarks == 1 ? true : false;
            DisplayServerSettingsInServerInfoTabCB.Checked = BookSettings.DisplayServerSettingsInServerInfoTab == 1 ? true : false;
            ShowHaBStatsCB.Checked = BookSettings.ShowHaBStats == 1 ? true : false;
            ShowPlayerFactionCB.Checked = BookSettings.ShowPlayerFaction == 1 ? true : false;

            listBox10.DisplayMember = "DisplayName";
            listBox10.ValueMember = "Value";
            listBox10.DataSource = BookSettings.RuleCategories;

            listBox13.DisplayMember = "DisplayName";
            listBox13.ValueMember = "Value";
            listBox13.DataSource = BookSettings.Descriptions;
            
            listBox18.DisplayMember = "DisplayName";
            listBox18.ValueMember = "Value";
            listBox18.DataSource = BookSettings.Links;

            listBox19.DisplayMember = "DisplayName";
            listBox19.ValueMember = "Value";
            listBox19.DataSource = BookSettings.CraftingCategories;
            useraction = true;
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentRulecat.CategoryName = textBox5.Text;
            listBox10.Invalidate();
            BookSettings.isDirty = true;
        }
        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox10.SelectedIndex == -1 ){ return; }
            CurrentRulecat = listBox10.SelectedItem as Rulecats;
            useraction = false;
            textBox6.Text = "";
            textBox7.Text = "";
            textBox5.Text = CurrentRulecat.CategoryName;
            listBox11.DisplayMember = "DisplayName";
            listBox11.ValueMember = "Value";
            listBox11.DataSource = CurrentRulecat.Rules;
            useraction = true;
        }
        private void listBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox11.Items.Count == 0) { return; }
            if (listBox11.SelectedIndex == -1) { return; }
            currentRules = listBox11.SelectedItem as Rules;
            useraction = false;
            textBox6.Text = currentRules.RuleParagraph;
            textBox7.Text = currentRules.RuleText;

            useraction = true;
        }
        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox13.SelectedItem as Descript == currentDescription) { return; }
            if(listBox13.SelectedIndex == -1) { return; }
            currentDescription = listBox13.SelectedItem as Descript;
            useraction = false;
            textBox10.Text = currentDescription.CategoryName;

            int i = 0;
            foreach(DT dt in currentDescription.Descriptions)
            {
                dt.DTName = "Decription Text " + i.ToString();
                i++;
            }

            textBox8.Text = "";

            listBox12.DisplayMember = "DisplayName";
            listBox12.ValueMember = "Value";
            listBox12.DataSource = currentDescription.Descriptions;


            useraction = true;
        }
        private void listBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox12.Items.Count == 0 ){ return; }
            if (listBox12.SelectedItem == null) { return; }

            currentdecriptiontext = listBox12.SelectedItem as DT;
            useraction = false;
            textBox8.Text = currentdecriptiontext.DescriptionText.Replace("<p>", "").Replace("</p>", "");
            useraction = true;
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            BookSettings.Descriptions.Add(new Descript() { CategoryName = "New Category", Descriptions = new BindingList<DT>() });
            BookSettings.isDirty = true;
            listBox13.SelectedIndex = -1;
            listBox13.SelectedIndex = listBox13.Items.Count - 1;
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            int index = listBox13.SelectedIndex;
            BookSettings.Descriptions.Remove(currentDescription);
            BookSettings.isDirty = true;
            useraction = false;
            listBox13.SelectedIndex = -1;
            if (index > listBox13.Items.Count - 1)
                listBox13.SelectedIndex = index - 1;
            else
                listBox13.SelectedIndex = index;
            useraction = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            currentDescription.Descriptions.Add(new DT() { DescriptionText = "New DescriptionText", DTName = "New DescriptionText"});
            int i = 0;
            foreach (DT dt in currentDescription.Descriptions)
            {
                dt.DTName = "Decription Text " + i.ToString();
                i++;
            }
            BookSettings.isDirty = true;
            listBox12.SelectedIndex = -1;
            listBox12.SelectedIndex = listBox12.Items.Count - 1;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            currentDescription.Descriptions.Remove(currentdecriptiontext);
            BookSettings.isDirty = true;
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            BookSettings.RuleCategories.Add(new Rulecats() { CategoryName = "New Rule", Rules = new BindingList<Rules>() });
            BookSettings.isDirty = true;
            listBox10.SelectedIndex = -1;
            listBox10.SelectedIndex = listBox10.Items.Count - 1;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            BookSettings.RuleCategories.Remove(CurrentRulecat);
            BookSettings.isDirty = true;
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdecriptiontext.DescriptionText = "<p>" + textBox8.Text + "</p>";
            BookSettings.isDirty = true;
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            CurrentRulecat.Rules.Add(new Rules() { RuleParagraph = "new paragraph", RuleText = "NewText"});
            BookSettings.RenameRules();
            BookSettings.isDirty = true;
            listBox11.SelectedIndex = -1;
            listBox11.SelectedIndex = listBox11.Items.Count - 1;
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            useraction = false;
            textBox6.Text = "";
            textBox7.Text = "";
            if (listBox11.Items.Count == 0) { return; }
            if (listBox11.SelectedItem == null) { return; }
            int index = listBox11.SelectedIndex;
            CurrentRulecat.Rules.Remove(currentRules);
            BookSettings.RenameRules();
            BookSettings.isDirty = Capture;
            listBox11.SelectedIndex = -1;
            if (index > listBox11.Items.Count - 1)
                listBox11.SelectedIndex = index - 1;
            else
                listBox11.SelectedIndex = index;
            useraction = true;

        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDescription.CategoryName = textBox10.Text;
            listBox13.Invalidate();
            BookSettings.isDirty = true;
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentRules.RuleText = textBox7.Text;
            BookSettings.isDirty = true;
        }
        private void EnableStatusTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnableStatusTab = EnableStatusTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void EnablePartyTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnablePartyTab = EnablePartyTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void EnableServerInfoTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnableServerInfoTab = EnableServerInfoTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void EnableServerRulesTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnableServerRulesTab = EnableServerRulesTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void EnableTerritoryTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnableTerritoryTab = EnableTerritoryTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void EnableBookMenuCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.EnableBookMenu = EnableBookMenuCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void CreateBookmarksCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.CreateBookmarks = CreateBookmarksCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void ShowHaBStatsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.ShowHaBStats = ShowHaBStatsCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void ShowPlayerFactionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.ShowPlayerFaction = ShowPlayerFactionCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void DisplayServerSettingsInServerInfoTabCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BookSettings.DisplayServerSettingsInServerInfoTab = DisplayServerSettingsInServerInfoTabCB.Checked == true ? 1 : 0;
            BookSettings.isDirty = true;
        }
        private void listBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox18.SelectedIndex == -1) { return; }
            currentLink = listBox18.SelectedItem as Links;
            useraction = false;
            textBox12.Text = currentLink.Name;
            textBox13.Text = currentLink.URL;
            comboBox5.SelectedIndex = comboBox5.FindStringExact(currentLink.IconName);
            LinkIconColour.Invalidate();
            useraction = true;
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentLink.Name = textBox12.Text;
            BookSettings.isDirty = true;
            listBox18.Invalidate();
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentLink.URL = textBox13.Text;
            BookSettings.isDirty = true;
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentLink.IconName = comboBox5.SelectedItem.ToString();
            BookSettings.isDirty = true;
        }
        private void LinkIconColour_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(-1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                currentLink.IconColor = cpick.Color.ToArgb();
                LinkIconColour.Invalidate();
                MapSettings.isDirty = true;
            }
        }
        private void LinkIconColour_Paint(object sender, PaintEventArgs e)
        {
            if(currentLink == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(currentLink.IconColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            BookSettings.Links.Add(new Links() {Name = "New link", URL = "Some Url", IconColor = -1, IconName = "Homepage" });
            BookSettings.isDirty = true;
            listBox18.SelectedIndex = -1;
            listBox18.SelectedIndex = listBox18.Items.Count - 1;
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            int index = listBox18.SelectedIndex;
            BookSettings.Links.Remove(currentLink);
            BookSettings.isDirty = true;
            useraction = false;
            textBox12.Text = "";
            textBox13.Text = "";
            listBox18.SelectedIndex = -1;
            if (index > listBox18.Items.Count - 1)
                listBox18.SelectedIndex = index - 1;
            else
                listBox18.SelectedIndex = index;
            useraction = true;
        }
        private void listBox19_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            CurrentCraftingCategory = listBox19.SelectedItem as CraftingCategories;
            if (CurrentCraftingCategory == null) { return; }
            useraction = false;
            textBox14.Text = CurrentCraftingCategory.CategoryName;


            listBox20.DisplayMember = "DisplayName";
            listBox20.ValueMember = "Value";
            listBox20.DataSource = CurrentCraftingCategory.Results;

            useraction = true;
        }
        private void darkButton38_Click(object sender, EventArgs e)
        {
            BookSettings.CraftingCategories.Add(new CraftingCategories() { CategoryName = "New Crafting Category", Results = new BindingList<string>() });
            BookSettings.isDirty = true;
            listBox19.SelectedIndex = -1;
            listBox19.SelectedIndex = listBox19.Items.Count - 1;
        }
        private void darkButton37_Click(object sender, EventArgs e)
        {
            BookSettings.CraftingCategories.Remove(CurrentCraftingCategory);
            BookSettings.isDirty = true;
        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentCraftingCategory.CategoryName = textBox14.Text;
            listBox19.Invalidate();
            BookSettings.isDirty = true;
        }
        private void listBox20_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void darkButton36_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentCraftingCategory.Results.Add(l.ToLower());
                    BookSettings.isDirty = true;
                }
            }
        }
        private void darkButton35_Click(object sender, EventArgs e)
        {
            CurrentCraftingCategory.Results.Remove(listBox20.GetItemText(listBox20.SelectedItem));
            BookSettings.isDirty = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            BookSettings.CraftingCategories = new BindingList<CraftingCategories>(BookSettings.CraftingCategories.OrderBy(x => x.CategoryName).ToList());
            listBox19.DisplayMember = "DisplayName";
            listBox19.ValueMember = "Value";
            listBox19.DataSource = BookSettings.CraftingCategories;
            BookSettings.isDirty = true;
        }
        #endregion Booksettings

        #region ChatSettings

        public void LoadChatsettings()
        {
            useraction = false;
            EnableGlobalChatCB.Checked = ChatSettings.EnableGlobalChat == 1 ? true : false;
            EnablePartyChatCB.Checked = ChatSettings.EnablePartyChat == 1 ? true : false;
            EnableTransportChatCB.Checked = ChatSettings.EnableTransportChat == 1 ? true : false;
            useraction = true;
        }
        private void SystemChatColorPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            string col = ChatSettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            cpick.Color = ColorTranslator.FromHtml(col1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                ChatSettings.setcolour(pb.Name, cpick.Color.Name.ToUpper());
                pb.Invalidate();
                ChatSettings.isDirty = true;
            }

        }
        private void ChatColorPB_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = ChatSettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            Color colour = ColorTranslator.FromHtml(col1);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }

        private void EnableGlobalChatCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            ChatSettings.EnableGlobalChat = EnableGlobalChatCB.Checked == true ? 1 : 0;
            ChatSettings.isDirty = true;
        }
        private void EnablePartyChatCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            ChatSettings.EnablePartyChat = EnablePartyChatCB.Checked == true ? 1 : 0;
            ChatSettings.isDirty = true;
        }
        private void EnableTransportChatCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            ChatSettings.EnableTransportChat = EnableTransportChatCB.Checked == true ? 1 : 0;
            ChatSettings.isDirty = true;
        }
        #endregion chatSettings

        #region dfamagesystemsettigns
        private void loadDamageSystemSettings()
        {
            useraction = false;
            DSEnabledCB.Checked = DamageSystemSettings.Enabled == 1 ? true : false;
            CheckForBlockingObjectsCB.Checked = DamageSystemSettings.CheckForBlockingObjects == 1 ? true : false;

            ExplosionTargetsLB.DisplayMember = "DisplayName";
            ExplosionTargetsLB.ValueMember = "Value";
            ExplosionTargetsLB.DataSource = DamageSystemSettings.ExplosionTargets;

            ExplosiveProjectilesLB.DisplayMember = "DisplayName";
            ExplosiveProjectilesLB.ValueMember = "Value";
            ExplosiveProjectilesLB.DataSource = DamageSystemSettings.explosinvesList;

            useraction = true;
        }
        private void ExplosiveProjectilesLB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void DSEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            DamageSystemSettings.Enabled = DSEnabledCB.Checked == true ? 1 : 0;
            DamageSystemSettings.isDirty = true;
        }
        private void CheckForBlockingObjectsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            DamageSystemSettings.CheckForBlockingObjects = CheckForBlockingObjectsCB.Checked == true ? 1 : 0;
            DamageSystemSettings.isDirty = true;
        }
        private void darkButton67_Click(object sender, EventArgs e)
        {

        }
        private void darkButton66_Click(object sender, EventArgs e)
        {

        }
        private void darkButton69_Click(object sender, EventArgs e)
        {

        }
        private void darkButton68_Click(object sender, EventArgs e)
        {

        }
        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion damagesystemsettings
        #region debugsettings
        private void loaddebugsettings()
        {
            useraction = false;
            DebugVehiclePlayerNetworkBubbleModeNUD.Value = DebugSettings.DebugVehiclePlayerNetworkBubbleMode;
            ServerUpdateRateLimitNUD.Value = DebugSettings.ServerUpdateRateLimit;
            useraction = true;
        }
        private void DebugVehiclePlayerNetworkBubbleModeNUD_ValueChanged(object sender, EventArgs e)
        {

            if (!useraction) return;
            DebugSettings.DebugVehiclePlayerNetworkBubbleMode = (int)DebugVehiclePlayerNetworkBubbleModeNUD.Value;
            DebugSettings.isDirty = true;
        }
        private void ServerUpdateRateLimitNUD_ValueChanged(object sender, EventArgs e)
        {

            if (!useraction) return;
            DebugSettings.ServerUpdateRateLimit = (int)ServerUpdateRateLimitNUD.Value;
            DebugSettings.isDirty = true;
        }
        #endregion debugsettings

        #region Generalsettings
        private void loadGeneralSettings()
        {
            useraction = false;

            DisableShootToUnlockCB.Checked = GeneralSettings.DisableShootToUnlock == 1 ? true : false;
            EnableGravecrossCB.Checked = GeneralSettings.EnableGravecross == 1 ? true : false;
            GravecrossDeleteBodyCB.Checked = GeneralSettings.GravecrossDeleteBody == 1 ? true : false;
            GravecrossTimeThresholdNUD.Value = GeneralSettings.GravecrossTimeThreshold;
            GravecrossSpawnTimeDelayNUD.Value = GeneralSettings.GravecrossSpawnTimeDelay;
            UseCustomMappingModuleCB.Checked = GeneralSettings.Mapping.UseCustomMappingModule == 1 ? true : false;
            BuildingInteriorsCB.Checked = GeneralSettings.Mapping.BuildingInteriors == 1 ? true : false;
            BuildingIvysComboBox.SelectedItem = (buildingIvy)GeneralSettings.Mapping.BuildingIvys;
            EnableLampsComboBox.SelectedItem = (Lamps)GeneralSettings.EnableLamps;
            EnableGeneratorsCB.Checked = GeneralSettings.EnableGenerators == 1 ? true : false;
            EnableLighthousesCB.Checked = GeneralSettings.EnableLighthouses == 1 ? true : false;
            EnableHUDNightvisionOverlayCB.Checked = GeneralSettings.EnableHUDNightvisionOverlay == 1 ? true : false;
            DisableMagicCrosshairCB.Checked = GeneralSettings.DisableMagicCrosshair == 1 ? true : false;
            EnableAutoRunCB.Checked = GeneralSettings.EnableAutoRun == 1 ? true : false;
            UnlimitedStaminaCB.Checked = GeneralSettings.UnlimitedStamina == 1 ? true : false;
            UseDeathScreenCB.Checked = GeneralSettings.UseDeathScreen == 1 ? true : false;
            UseDeathScreenStatisticsCB.Checked = GeneralSettings.UseDeathScreenStatistics == 1 ? true : false;
            UseNewsFeedInGameMenuCB.Checked = GeneralSettings.UseNewsFeedInGameMenu == 1 ? true : false;
            EnableEarPlugsCB.Checked = GeneralSettings.EnableEarPlugs == 1 ? true : false;
            InGameMenuLogoPathTB.Text = GeneralSettings.InGameMenuLogoPath;
            useraction = true;
        }
        private void HudColourPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            string col = GeneralSettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            cpick.Color = ColorTranslator.FromHtml(col1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                GeneralSettings.setcolour(pb.Name, cpick.Color.Name.ToUpper());
                pb.Invalidate();
                GeneralSettings.isDirty = true;
            }
        }
        private void HUDColourPB_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = GeneralSettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            Color colour = ColorTranslator.FromHtml(col1);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void GeneralsettingsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            GeneralSettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            GeneralSettings.isDirty = true;
        }
        private void InGameMenuLogoPathTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            GeneralSettings.InGameMenuLogoPath = InGameMenuLogoPathTB.Text;
            GeneralSettings.isDirty = true;
        }
        private void GeneralsettingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            GeneralSettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            GeneralSettings.isDirty = true;
        }
        private void GravecrossTimeThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            GeneralSettings.GravecrossTimeThreshold = (decimal)GravecrossTimeThresholdNUD.Value;
            GeneralSettings.isDirty = true;
        }
        private void GravecrossSpawnTimeDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            GeneralSettings.GravecrossSpawnTimeDelay = (decimal)GravecrossSpawnTimeDelayNUD.Value;
            GeneralSettings.isDirty = true;
        }
        private void UseCustomMappingModuleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            GeneralSettings.Mapping.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            GeneralSettings.isDirty = true;

        }
        private void EnableLampsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Lamps lamp = (Lamps)EnableLampsComboBox.SelectedItem;
            GeneralSettings.EnableLamps = (int)lamp;
            GeneralSettings.isDirty = true;
        }
        private void BuildingIvysComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            buildingIvy ivies = (buildingIvy)BuildingIvysComboBox.SelectedItem;
            GeneralSettings.Mapping.BuildingIvys = (int)ivies;
            GeneralSettings.isDirty = true;
        }
        #endregion Generalsettings

        #region Hartdline
        private void loadHardlineSettings()
        {
            useraction = false;
            ShowHardlineHUDCB.Checked = HardLineSettings.ShowHardlineHUD == 1 ? true : false;
            UseReputationCB.Checked = HardLineSettings.UseReputation == 1 ? true : false;
            EnableItemRarityCB.Checked = HardLineSettings.EnableItemRarity == 1 ? true : false;
            UseItemRarityForMarketPurchaseNCB.Checked = HardLineSettings.UseItemRarityForMarketPurchase == 1 ? true : false;
            UseItemRarityForMarketSellCB.Checked = HardLineSettings.UseItemRarityForMarketSell == 1 ? true : false;
            EnableFactionPersistenceCB.Checked = HardLineSettings.EnableFactionPersistence == 1 ? true : false;
            UseFactionReputationCB.Checked = HardLineSettings.UseFactionReputation == 1 ? true : false;
            ReputationMaxReputationNUD.Value = HardLineSettings.MaxReputation;
            ReputationLossOnDeathNUD.Value = HardLineSettings.ReputationLossOnDeath;
            ReputationOnKillInfectedNUD.Value = HardLineSettings.ReputationOnKillInfected;
            ReputationOnKillPlayerNUD.Value = HardLineSettings.ReputationOnKillPlayer;
            ReputationOnKillAnimalNUD.Value = HardLineSettings.ReputationOnKillAnimal;
            ReputationOnKillAINUD.Value = HardLineSettings.ReputationOnKillAI;

            HardlinePlayerDataPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Hardline\\PlayerData";
            if(!Directory.Exists(HardlinePlayerDataPath))
            {
                Directory.CreateDirectory(HardlinePlayerDataPath);
            }
            ExpansionHardlinePlayerDataList = new ExpansionHardlinePlayerDataList(HardlinePlayerDataPath);
            setupExpansionHardlinePlayerData();

            useraction = true;
        }
        private void UseItemRarityForMarketPurchaseNCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.UseItemRarityForMarketPurchase = UseItemRarityForMarketPurchaseNCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void UseItemRarityForMarketSellCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.UseItemRarityForMarketSell = UseItemRarityForMarketSellCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void EnableItemRarityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.EnableItemRarity = EnableItemRarityCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void UseHumanityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.UseReputation = UseReputationCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void ShowHardlineHUDCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.ShowHardlineHUD = ShowHardlineHUDCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void HardlineINT_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            HardLineSettings.setIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            HardLineSettings.isDirty = true;
        }
        private void ReputationMaxReputationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.MaxReputation = (int)ReputationMaxReputationNUD.Value;
            HardLineSettings.isDirty = true;
        }
        private void ReputationLossOnDeathNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.ReputationLossOnDeath = (int)ReputationLossOnDeathNUD.Value;
            HardLineSettings.isDirty = true;
        }
        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            string Type = comboBox4.GetItemText(comboBox4.SelectedItem);
            useraction = false;

            ItemRequirementNUD.Value = HardLineSettings.getRequirment(Type);

            ItemRarityLB.DisplayMember = "DisplayName";
            ItemRarityLB.ValueMember = "Value";
            ItemRarityLB.DataSource = HardLineSettings.getlist(Type);
            useraction = true;
        }
        private void ItemRequirementNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.SetRequirment(comboBox4.GetItemText(comboBox4.SelectedItem), (int)ItemRequirementNUD.Value);
            HardLineSettings.isDirty = true;
        }
        private void darkButton71_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                LowerCase = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string Type = comboBox4.GetItemText(comboBox4.SelectedItem);
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!HardLineSettings.getlist(Type).Contains(l))
                    {
                        HardLineSettings.getlist(Type).Add(l);
                    }
                    HardLineSettings.isDirty = true;
                }
            }
        }
        private void darkButton72_Click(object sender, EventArgs e)
        {

        }
        private void darkButton70_Click(object sender, EventArgs e)
        {
            string Type = comboBox4.GetItemText(comboBox4.SelectedItem);
            HardLineSettings.getlist(Type).Remove(ItemRarityLB.GetItemText(ItemRarityLB.SelectedItem));
            HardLineSettings.isDirty = true;
        }
        private void setupExpansionHardlinePlayerData()
        {
            useraction = false;

            treeViewMS2.Nodes.Clear();
            TreeNode root = new TreeNode("Hardline Player Data")
            {
                Tag = "Parent"
            };
            foreach (ExpansionHardlinePlayerData QPD in ExpansionHardlinePlayerDataList.HardlinePlayerDataList)
            {
                TreeNode Playerdata = new TreeNode(Path.GetFileNameWithoutExtension(QPD.Filename))
                {
                    Tag = QPD
                };
                root.Nodes.Add(Playerdata);
            }
            treeViewMS2.Nodes.Add(root);


            useraction = true;
        }
        public ExpansionHardlinePlayerData currentExpansionHardlinePlayerData;
        private void treeViewMS2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            useraction = false;
            if (e.Node.Tag != null && e.Node.Tag is ExpansionHardlinePlayerData)
            {
                currentExpansionHardlinePlayerData = e.Node.Tag as ExpansionHardlinePlayerData;
                HardlineReputationNUD.Value = currentExpansionHardlinePlayerData.GetReputation();
                HardlinePlayerKillsNUD.Value = currentExpansionHardlinePlayerData.GetPlayerKills();
                HardlineAIKillsNUD.Value = currentExpansionHardlinePlayerData.GetAIKills();
                HardLineInfectedKillsNUD.Value = currentExpansionHardlinePlayerData.GetInfectedKills();
                hardLinePlayerDeathsNUD.Value = currentExpansionHardlinePlayerData.GetPlayerDeaths();
            }
            useraction = true;
        }
        private void HardlineReputationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentExpansionHardlinePlayerData.SetReputation((int)HardlineReputationNUD.Value);
            currentExpansionHardlinePlayerData.isDirty = true;
        }
        private void treeViewMS2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }
        private void EnableFactionPersistenceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.EnableFactionPersistence = EnableFactionPersistenceCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        private void UseFactionReputationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            HardLineSettings.UseFactionReputation = UseFactionReputationCB.Checked == true ? 1 : 0;
            HardLineSettings.isDirty = true;
        }
        #endregion

        #region logsettings
        private void loadlogsettings()
        {
            useraction = false;
            SafezoneCB.Checked = LogSettings.Safezone == 1 ? true : false;
            VehicleCarKeyCB.Checked = LogSettings.VehicleCarKey == 1 ? true : false;
            VehicleDestroyedCB.Checked = LogSettings.VehicleDestroyed == 1 ? true : false;
            VehicleTowingCB.Checked = LogSettings.VehicleTowing == 1 ? true : false;
            VehicleLockPickingCB.Checked = LogSettings.VehicleLockPicking == 1 ? true : false;
            VehicleDestroyedCB.Checked = LogSettings.VehicleDestroyed == 1 ? true : false;
            VehicleAttachmentsCB.Checked = LogSettings.VehicleAttachments == 1 ? true : false;
            VehicleEnterCB.Checked = LogSettings.VehicleEnter == 1 ? true : false;
            VehicleLeaveCB.Checked = LogSettings.VehicleLeave == 1 ? true : false;
            VehicleDeletedCB.Checked = LogSettings.VehicleDeleted == 1 ? true : false;
            VehicleEngineCB.Checked = LogSettings.VehicleEngine == 1 ? true : false;
            BaseBuildingRaidingCB.Checked = LogSettings.BaseBuildingRaiding == 1 ? true : false;
            CodeLockRaidingCB.Checked = LogSettings.CodeLockRaiding == 1 ? true : false;
            TerritoryCB.Checked = LogSettings.Territory == 1 ? true : false;
            KillfeedCB.Checked = LogSettings.Killfeed == 1 ? true : false;
            PartyCB.Checked = LogSettings.Party == 1 ? true : false;
            ChatCB.Checked = LogSettings.Chat == 1 ? true : false;
            AdminToolsCB.Checked = LogSettings.AdminTools == 1 ? true : false;
            SpawnSelectionCB.Checked = LogSettings.SpawnSelection == 1 ? true : false;
            MissionAirdropCB.Checked = LogSettings.MissionAirdrop == 1 ? true : false;
            MarketCB.Checked = LogSettings.Market == 1 ? true : false;
            ATMCB.Checked = LogSettings.ATM == 1 ? true : false;
            LogToScriptsCB.Checked = LogSettings.LogToScripts == 1 ? true : false;
            LogToADMCB.Checked = LogSettings.LogToADM == 1 ? true : false;
            AIObjectPatrolCB.Checked = LogSettings.AIObjectPatrol == 1 ? true : false;
            AIGeneralCB.Checked = LogSettings.AIGeneral == 1 ? true : false;
            AIObjectPatrolCB.Checked = LogSettings.AIObjectPatrol == 1 ? true : false;
            AIPatrolCB.Checked = LogSettings.AIPatrol == 1 ? true : false;
            HardlineCB.Checked = LogSettings.Hardline == 1 ? true : false;
            ExplosionDamageSystemCB.Checked = LogSettings.ExplosionDamageSystem == 1 ? true : false;
            EntityStorageCB.Checked = LogSettings.EntityStorage == 1 ? true : false;
            GarageCB.Checked = LogSettings.Garage == 1 ? true : false;
            VehicleCoverCB.Checked = LogSettings.VehicleCover == 1 ? true : false;
            useraction = true;
        }
        private void LogSettingsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox cb = sender as CheckBox;
            LogSettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            LogSettings.isDirty = true;
        }
        #endregion logsettings

        #region mapsettings
        public int markerScale = 1;
        public ServerMarkers currentmapmapmarker;

        private void loadmapsettings()
        {
            useraction = false;

            EnableMapCB.Checked = MapSettings.EnableMap == 1 ? true : false;
            UseMapOnMapItemCB.Checked = MapSettings.UseMapOnMapItem == 1 ? true : false;
            ShowPlayerPositionCB.Checked = MapSettings.ShowPlayerPosition == 1 ? true : false;
            ShowMapStatsCB.Checked = MapSettings.ShowMapStats == 1 ? true : false;
            NeedPenItemForCreateMarkerCB.Checked = MapSettings.NeedPenItemForCreateMarker == 1 ? true : false;
            NeedGPSItemForCreateMarkerCB.Checked = MapSettings.NeedGPSItemForCreateMarker == 1 ? true : false;
            CanCreateMarkerCB.Checked = MapSettings.CanCreateMarker == 1 ? true : false;
            CanCreate3DMarkerCB.Checked = MapSettings.CanCreate3DMarker == 1 ? true : false;
            CanOpenMapWithKeyBindingCB.Checked = MapSettings.CanOpenMapWithKeyBinding == 1 ? true : false;
            ShowDistanceOnPersonalMarkersCB.Checked = MapSettings.ShowDistanceOnPersonalMarkers == 1 ? true : false;
            EnableHUDGPSCB.Checked = MapSettings.EnableHUDGPS == 1 ? true : false;
            NeedGPSItemForKeyBindingCB.Checked = MapSettings.NeedGPSItemForKeyBinding == 1 ? true : false;
            NeedMapItemForKeyBindingCB.Checked = MapSettings.NeedMapItemForKeyBinding == 1 ? true : false;
            EnableServerMarkersCB.Checked = MapSettings.EnableServerMarkers == 1 ? true : false;
            ShowNameOnServerMarkersCB.Checked = MapSettings.ShowNameOnServerMarkers == 1 ? true : false;
            ShowDistanceOnServerMarkersCB.Checked = MapSettings.ShowDistanceOnServerMarkers == 1 ? true : false;
            EnableHUDCompassCB.Checked = MapSettings.EnableHUDCompass == 1 ? true : false;
            NeedCompassItemForHUDCompassCB.Checked = MapSettings.NeedCompassItemForHUDCompass == 1 ? true : false;
            NeedGPSItemForHUDCompassCB.Checked = MapSettings.NeedGPSItemForHUDCompass == 1 ? true : false;
            CreateDeathMarkerCB.Checked = MapSettings.CreateDeathMarker == 1 ? true : false;
            PlayerLocationNotifierCB.Checked = MapSettings.PlayerLocationNotifier == 1 ? true : false;
            CompassColor.Invalidate();
            pictureBox7.Invalidate();

            comboBox1.DisplayMember = "Description";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = Enum.GetValues(typeof(MapMarkerVisibility))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
            
            String[] Icons = File.ReadAllLines(Application.StartupPath + "\\Maps\\Icons\\IconNames.txt");
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Value";
            comboBox3.DataSource = Icons.ToList();

            listBox17.DisplayMember = "DisplayName";
            listBox17.ValueMember = "Value";
            listBox17.DataSource = MapSettings.ServerMarkers;

            pictureBox3.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox3.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox3.Paint += new PaintEventHandler(DrawMapIcons);
            trackBar4.Value = 1;
            SetNMarkermapscale();
            useraction = true;
        }
        private void DrawMapIcons(object sender, PaintEventArgs e)
        {
            float scalevalue = markerScale * 0.05f;
            foreach (ServerMarkers marker in MapSettings.ServerMarkers)
            {
                Bitmap image;
                if(File.Exists(Application.StartupPath + "\\Maps\\Icons\\" + marker.m_IconName + ".png"))
                {
                    image = new Bitmap(Application.StartupPath + "\\Maps\\Icons\\" + marker.m_IconName + ".png");
                }
                else
                {
                    image = new Bitmap(Application.StartupPath + "\\Maps\\Icons\\Exclamationmark.png");
                }
                Image image2 = Helper.MultiplyColorToBitmap(image, Color.FromArgb(marker.m_Color), 200, true);
                Size msize = new Size(30, 30);
                Image image3 = resizeImage(image2, msize);
                int centerX = (int)(Math.Round(marker.m_Position[0], 0) * scalevalue) - 15;
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(marker.m_Position[2], 0) * scalevalue) - 15;
                Point center = new Point(centerX, centerY);
                TextureBrush tBrush = new TextureBrush(image3);
                e.Graphics.DrawImage(image3, center);
            }
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        private void listBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentmapmapmarker = listBox17.SelectedItem as ServerMarkers;
            if(currentmapmapmarker == null ) { return; }
            useraction = false;
            comboBox1.SelectedValue = (MapMarkerVisibility)currentmapmapmarker.m_Visibility;
            comboBox3.SelectedIndex = comboBox3.FindStringExact(currentmapmapmarker.m_IconName);
            textBox9.Text = currentmapmapmarker.m_UID;
            textBox11.Text = currentmapmapmarker.m_Text;
            numericUpDown24.Value = (decimal)currentmapmapmarker.m_Position[0];
            numericUpDown25.Value = (decimal)currentmapmapmarker.m_Position[1];
            numericUpDown26.Value = (decimal)currentmapmapmarker.m_Position[2];
            m_Is3DCB.Checked = currentmapmapmarker.m_Is3D == 1 ? true : false;
            m_LockedCB.Checked = currentmapmapmarker.m_Locked == 1 ? true : false;
            m_Color.Invalidate();
            pictureBox3.Invalidate();
            useraction = true;
        }
        private void BackGround_Paint(object sender, PaintEventArgs e)
        {
            if(currentmapmapmarker == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(currentmapmapmarker.m_Color);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void CompassColor_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(MapSettings.CompassColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            markerScale = trackBar4.Value;
            SetNMarkermapscale();
        }
        private void SetNMarkermapscale()
        {
            float scalevalue = markerScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox3.Size = new Size(newsize, newsize);
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ServerMarkers newmarker = new ServerMarkers()
            {
                m_UID = "New Marker",
                m_Color = -1,
                m_Text = "New Marker",
                m_IconName = comboBox3.Items[0].ToString(),
                m_Is3D = 0,
                m_Visibility = 0,
                m_Position = new float[] { currentproject.MapSize / 2, MapData.gethieght(currentproject.MapSize / 2, currentproject.MapSize / 2), currentproject.MapSize / 2 },
            };
            MapSettings.ServerMarkers.Add(newmarker);
            listBox17.SelectedIndex = -1;
            listBox17.SelectedIndex = listBox17.Items.Count - 1;
            Cursor.Current = Cursors.Default;
        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            MapSettings.ServerMarkers.Remove(currentmapmapmarker);
            MapSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void m_LockedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Locked = m_LockedCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void m_Is3DCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Is3D = m_Is3DCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_UID = textBox9.Text;
            MapSettings.isDirty = true;
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Text = textBox11.Text;
            MapSettings.isDirty = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Visibility = (int)comboBox1.SelectedValue;
            MapSettings.isDirty = true;
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_IconName = comboBox3.SelectedItem.ToString();
            MapSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void m_Color_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(currentmapmapmarker.m_Color);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                currentmapmapmarker.m_Color = cpick.Color.ToArgb();
                m_Color.Invalidate();
                pictureBox3.Invalidate();
                MapSettings.isDirty = true;
            }
        }
        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Position[0] = (float)numericUpDown24.Value;
            MapSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Position[1] = (float)numericUpDown25.Value;
            MapSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentmapmapmarker.m_Position[2] = (float)numericUpDown26.Value;
            MapSettings.isDirty = true;
            pictureBox3.Invalidate();
        }
        private void CompassColor_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(MapSettings.CompassColor);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                MapSettings.CompassColor = cpick.Color.ToArgb();
                CompassColor.Invalidate();
                MapSettings.isDirty = true;
            }
        }
        private void EnableMapCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.EnableMap = EnableMapCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;

        }
        private void UseMapOnMapItemCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.UseMapOnMapItem = UseMapOnMapItemCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void ShowPlayerPositionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.ShowPlayerPosition = ShowPlayerPositionCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void ShowMapStatsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.ShowMapStats = ShowMapStatsCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void CanOpenMapWithKeyBindingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.CanOpenMapWithKeyBinding = CanOpenMapWithKeyBindingCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedGPSItemForKeyBindingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedGPSItemForKeyBinding = NeedGPSItemForKeyBindingCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedMapItemForKeyBindingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedMapItemForKeyBinding = NeedMapItemForKeyBindingCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void CreateDeathMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.CreateDeathMarker = CreateDeathMarkerCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void PlayerLocationNotifierCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.PlayerLocationNotifier = PlayerLocationNotifierCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void EnableServerMarkersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.EnableServerMarkers = EnableServerMarkersCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;

        }
        private void ShowNameOnServerMarkersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.ShowNameOnServerMarkers = ShowNameOnServerMarkersCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void ShowDistanceOnServerMarkersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.ShowDistanceOnServerMarkers = ShowDistanceOnServerMarkersCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void CanCreateMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.CanCreateMarker = CanCreateMarkerCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void CanCreate3DMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.CanCreate3DMarker = CanCreate3DMarkerCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedPenItemForCreateMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedPenItemForCreateMarker = NeedPenItemForCreateMarkerCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedGPSItemForCreateMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedGPSItemForCreateMarker = NeedGPSItemForCreateMarkerCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void ShowDistanceOnPersonalMarkersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.ShowDistanceOnPersonalMarkers = ShowDistanceOnPersonalMarkersCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void EnableHUDGPSCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.EnableHUDGPS = EnableHUDGPSCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void EnableHUDCompassCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.EnableHUDCompass = EnableHUDCompassCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedCompassItemForHUDCompassCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedCompassItemForHUDCompass = NeedCompassItemForHUDCompassCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void NeedGPSItemForHUDCompassCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MapSettings.NeedGPSItemForHUDCompass = NeedGPSItemForHUDCompassCB.Checked == true ? 1 : 0;
            MapSettings.isDirty = true;
        }
        private void pictureBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = markerScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (currentmapmapmarker == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                numericUpDown24.Value = (decimal)(mouseEventArgs.X / scalevalue);
                numericUpDown26.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                if (MapData.FileExists)
                {
                    numericUpDown25.Value = (decimal)(MapData.gethieght(currentmapmapmarker.m_Position[0], currentmapmapmarker.m_Position[2]));
                }
                Cursor.Current = Cursors.Default;

                MapSettings.isDirty = true;
                pictureBox3.Invalidate();
            }
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(MapSettings.CompassBadgesColor);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                MapSettings.CompassBadgesColor = cpick.Color.ToArgb();
                pictureBox7.Invalidate();
                MapSettings.isDirty = true;
            }
        }
        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(MapSettings.CompassBadgesColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        #endregion mapsettings

        #region MissionSettings
        public AirdropMissionSettingFiles currentAirdropmissionfile;
        public decimal MissionMapscale = 1;
        private void loadMissionSettings()
        {
            useraction = false;
            setupairdropZombies(listBox24);
            MissionsEnabledCB.Checked = MissionSettings.Enabled == 1 ? true : false;
            InitialMissionStartDelayNUD.Value = (decimal)Helper.ConvertMillisecondsToMinutes(MissionSettings.InitialMissionStartDelay);
            TimeBetweenMissionsNUD.Value = (decimal)Helper.ConvertMillisecondsToMinutes(MissionSettings.TimeBetweenMissions);
            MinMissionsNUD.Value = (decimal)MissionSettings.MinMissions;
            MaxMissionsNUD.Value = (decimal)MissionSettings.MaxMissions;
            MinPlayersToStartMissionsNUD.Value = (decimal)MissionSettings.MinPlayersToStartMissions;
            MissionsLB.DisplayMember = "DisplayName";
            MissionsLB.ValueMember = "Value";
            MissionsLB.DataSource = MissionSettings.MissionSettingFiles;

            pictureBox6.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox6.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox6.Paint += new PaintEventHandler(DrawAllMissions);
            trackBar6.Value = 1;
            SetsMissionScale();
            panel5.AutoScrollPosition = new Point(0, 0);
            useraction = true;
        }
        private void trackBar6_MouseUp(object sender, MouseEventArgs e)
        {
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
        }
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X , e.Location.Y - _mouseLastPosition.Y );
                _newscrollPosition = new Point(-panel5.AutoScrollPosition.X - changePoint.X, -panel5.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel5.AutoScrollPosition = _newscrollPosition;
                pictureBox6.Invalidate();

                darkLabel272.Text = "Y:" + panel5.AutoScrollPosition.Y.ToString();
            }
        }
        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox6.Focused == false)
            {
                pictureBox6.Focus();
                panel5.AutoScrollPosition = _newscrollPosition;
                pictureBox6.Invalidate();
            }
        }
        private void PicBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }
            
        }
        private void ZoomIn()
        {
            int oldpictureboxhieght = pictureBox6.Height;
            int oldpitureboxwidht = pictureBox6.Width;
            Point oldscrollpos = panel5.AutoScrollPosition;
            int tbv = trackBar6.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar6.Value = newval;
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
            if (pictureBox6.Height > panel5.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox6.Height * newy);
                _newscrollPosition.Y = y *-1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            if(pictureBox6.Width > panel5.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox6.Width * newy);
                _newscrollPosition.X = x * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox6.Invalidate();
        }
        private void ZoomOut()
        {
            int oldpictureboxhieght = pictureBox6.Height;
            int oldpitureboxwidht = pictureBox6.Width;
            Point oldscrollpos = panel5.AutoScrollPosition;
            int tbv = trackBar6.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar6.Value = newval;
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
            if (pictureBox6.Height > panel5.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox6.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox6.Width > panel5.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox6.Width * newy);
                _newscrollPosition.X = x * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox6.Invalidate();
        }
        private void SetsMissionScale()
        {
            
            decimal scalevalue = MissionMapscale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox6.Size = new Size(newsize, newsize);
            darkLabel273.Text = "MapSize:" + newsize.ToString();
            darkLabel269.Text = "panelsize" + panel5.Height.ToString();
            //panel5.VerticalScroll.Maximum = newsize;
            //panel5.HorizontalScroll.Maximum = newsize;
        }
        private void DrawAllMissions(object sender, PaintEventArgs e)
        {
            decimal scalevalue = MissionMapscale * (decimal)0.05;
            string missiontype = "";
            if (MissionsLB.SelectedItem is AirdropMissionSettingFiles)
                missiontype = "Airdrop";
            else if (MissionsLB.SelectedItem is ContaminatedAreaMissionSettingFiles)
                missiontype = "ContaminsatedArea";


            if (MissionSettings.MissionSettingFiles.Count > 0)
            {
                foreach (var item in MissionSettings.MissionSettingFiles)
                {
                    if (item is AirdropMissionSettingFiles)
                    {
                        AirdropMissionSettingFiles pitem = item as AirdropMissionSettingFiles;

                        int centerX = (int)(Math.Round(pitem.DropLocation.x, 0) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pitem.DropLocation.z, 0) * scalevalue);

                        int radius = (int)(pitem.DropLocation.Radius * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red)
                        {
                            Width = 4
                        };
                        if (item == currentAirdropmissionfile && missiontype == "Airdrop")
                            pen.Color = Color.LimeGreen;
                        else
                            pen.Color = Color.Red;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                    else if (item is ContaminatedAreaMissionSettingFiles)
                    {
                        ContaminatedAreaMissionSettingFiles pitem = item as ContaminatedAreaMissionSettingFiles;

                        int centerX = (int)(Math.Round(pitem.Data.Pos[0], 0) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pitem.Data.Pos[2], 0) * scalevalue);

                        int radius = (int)((decimal)pitem.Data.Radius * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red)
                        {
                            Width = 4
                        };
                        if (item == currentContaminatedAreaMissionFile && missiontype == "ContaminsatedArea")
                            pen.Color = Color.LimeGreen;
                        else
                            pen.Color = Color.Red;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                }
            }
        }
        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                decimal scalevalue = MissionMapscale * (decimal)0.05;
                decimal mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (MissionsLB.SelectedItem is AirdropMissionSettingFiles)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MissionDropXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                    MissionDropYNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                    Cursor.Current = Cursors.Default;
                    currentAirdropmissionfile.isDirty = true;
                }
                else if (MissionsLB.SelectedItem is ContaminatedAreaMissionSettingFiles)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PosXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                    posZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                    posYNUD.Value = 0;
                    //if (MapData.FileExists)
                    //{
                    //    posYNUD.Value = (decimal)(MapData.gethieght((float)PosXNUD.Value, (float)posZNUD.Value));
                    //}
                    Cursor.Current = Cursors.Default;
                    currentAirdropmissionfile.isDirty = true;
                }
                pictureBox6.Invalidate();
            }
        }
        private void MissionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MissionsLB.SelectedItems.Count < 1) return;
            
            useraction = false;
            if(MissionsLB.SelectedItem is AirdropMissionSettingFiles)
            {
                AirdropMissionGB.Visible = true;
                ContaminatedAreaGB.Visible = false;
                currentAirdropmissionfile = MissionsLB.SelectedItem as AirdropMissionSettingFiles;
                MissionPathTB.Text = currentAirdropmissionfile.Filename;
                MissionNameTB.Text = currentAirdropmissionfile.MissionName;
                MIssionContainerCB.SelectedIndex = MIssionContainerCB.FindStringExact(currentAirdropmissionfile.Container);
                MissionEnabledCB.Checked = currentAirdropmissionfile.Enabled == 1 ? true : false;
                MissionShowNotificationCB.Checked = currentAirdropmissionfile.ShowNotification == 1 ? true : false;
                MissionWeightNUD.Value = (decimal)currentAirdropmissionfile.Weight;
                MissionMissionMaxTimeNUD.Value = (decimal)Helper.ConvertSecondsToMinutes(currentAirdropmissionfile.MissionMaxTime);
                MissionHeightNUD.Value = (decimal)currentAirdropmissionfile.Height;
                MissionSpeedNUD.Value = (decimal)currentAirdropmissionfile.Speed;
                MissionDropXNUD.Value = (decimal)currentAirdropmissionfile.DropLocation.x;
                MissionDropYNUD.Value = (decimal)currentAirdropmissionfile.DropLocation.z;
                MissionDropRadiusNUD.Value = (decimal)currentAirdropmissionfile.DropLocation.Radius;
                MissionDropNameTB.Text = currentAirdropmissionfile.DropLocation.Name;
                numericUpDown38.Value = currentAirdropmissionfile.InfectedCount;
                numericUpDown37.Value = currentAirdropmissionfile.ItemCount;
                Specificpopulatelistbox();
                SpecificpopulateZombies();
            }
            else if (MissionsLB.SelectedItem is ContaminatedAreaMissionSettingFiles)
            {
                AirdropMissionGB.Visible = false;
                ContaminatedAreaGB.Visible = true;
                currentContaminatedAreaMissionFile = MissionsLB.SelectedItem as ContaminatedAreaMissionSettingFiles;
                textBox15.Text = currentContaminatedAreaMissionFile.Filename;
                textBox16.Text = currentContaminatedAreaMissionFile.MissionName;
                checkBox7.Checked = currentContaminatedAreaMissionFile.Enabled == 1 ? true : false;
                numericUpDown29.Value = (decimal)currentContaminatedAreaMissionFile.Weight;
                numericUpDown30.Value = (decimal)Helper.ConvertSecondsToMinutes(currentContaminatedAreaMissionFile.MissionMaxTime);
                PosXNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.Pos[0];
                posYNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.Pos[1];
                posZNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.Pos[2];
                RadiusNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.Radius;
                PosHeightNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.PosHeight;
                NegHeightNUD.Value = (decimal)currentContaminatedAreaMissionFile.Data.NegHeight;
                InnerRingCountNUD.Value = currentContaminatedAreaMissionFile.Data.InnerRingCount;
                InnerPartDistNUD.Value = currentContaminatedAreaMissionFile.Data.InnerPartDist;
                OuterRingToggleCB.Checked = currentContaminatedAreaMissionFile.Data.OuterRingToggle == 1 ? true : false;
                OuterPartDistNUD.Value = currentContaminatedAreaMissionFile.Data.OuterPartDist;
                OuterOffsetNUD.Value = currentContaminatedAreaMissionFile.Data.OuterOffset;
                VerticalLayersNUD.Value = currentContaminatedAreaMissionFile.Data.VerticalLayers;
                VerticalOffsetNUD.Value = currentContaminatedAreaMissionFile.Data.VerticalOffset;
                ParticleNameTB.Text = currentContaminatedAreaMissionFile.Data.ParticleName;
                AroundPartNameTB.Text = currentContaminatedAreaMissionFile.PlayerData.AroundPartName;
                TinyPartNameTB.Text = currentContaminatedAreaMissionFile.PlayerData.TinyPartName;
                PPERequesterTypeTB.Text = currentContaminatedAreaMissionFile.PlayerData.PPERequesterType;
                numericUpDown27.Value = currentContaminatedAreaMissionFile.StartDecayLifetime;
                numericUpDown28.Value = currentContaminatedAreaMissionFile.FinishDecayLifetime;
            }
            pictureBox6.Invalidate();
            useraction = true;
        }

        //Airdrop Missions
        private void Specificpopulatelistbox()
        {
            listBox30.DisplayMember = "DisplayName";
            listBox30.ValueMember = "Value";
            listBox30.DataSource = currentAirdropmissionfile.Loot;
        }
        private void SpecificpopulateZombies()
        {
            listBox26.DisplayMember = "DisplayName";
            listBox26.ValueMember = "Value";
            listBox26.DataSource = currentAirdropmissionfile.Infected;
        }
        private void MissionsEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            MissionSettings.Enabled = MissionsEnabledCB.Checked == true ? 1 : 0;
            MissionSettings.isDirty = true;
        }
        private void MissionEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.Enabled = MissionEnabledCB.Checked == true ? 1 : 0;
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionShowNotificationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.ShowNotification = MissionShowNotificationCB.Checked == true ? 1 : 0;
            currentAirdropmissionfile.isDirty = true;
        }
        private void missionsettingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            MissionSettings.SetIntValue(nud.Tag as string, (int)nud.Value);
            MissionSettings.isDirty = true;
        }
        private void missionsettingsTosecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            MissionSettings.SetIntValue(nud.Tag as string, (int)Helper.ConvertMinutesToMilliseconds((int)nud.Value));
            MissionSettings.isDirty = true;
        }
        private void MissionFileIntNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            currentAirdropmissionfile.SetIntValue(nud.Tag as string, (int)nud.Value);
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionFileIntTosecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            currentAirdropmissionfile.SetIntValue(nud.Tag as string, (int)Helper.ConvertMinutesToSeconds((int)nud.Value));
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionWeightFloatNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            currentAirdropmissionfile.SetdecimalValue(nud.Tag as string, (decimal)nud.Value);
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionPathTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.Filename = MissionPathTB.Text;
            MissionSettings.isDirty = true;
        }
        private void MissionNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.MissionName = MissionNameTB.Text;
            currentAirdropmissionfile.isDirty = true;
        }
        private void MIssionContainerCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.Container = MIssionContainerCB.SelectedItem.ToString();
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionDropNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.DropLocation.Name = MissionDropNameTB.Text;
            currentAirdropmissionfile.isDirty = true;
        }
        private void MissionDropXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.DropLocation.x = (decimal)MissionDropXNUD.Value;
            currentAirdropmissionfile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void MissionDropYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.DropLocation.z = (decimal)MissionDropYNUD.Value;
            currentAirdropmissionfile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void MissionDropRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentAirdropmissionfile.DropLocation.Radius = (decimal)MissionDropRadiusNUD.Value;
            currentAirdropmissionfile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            SpecificInfectedAirdropGB.Visible = numericUpDown38.Value == -1 ? false : true;
            if (!useraction) { return; }
            currentAirdropmissionfile.InfectedCount = (int)numericUpDown38.Value;
            currentAirdropmissionfile.isDirty = true;
        }
        private void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            SpecifcLootAirdropsGB.Visible = numericUpDown37.Value == -1 ? false : true;
            if (!useraction) { return; }
            currentAirdropmissionfile.ItemCount = (int)numericUpDown37.Value;
            currentAirdropmissionfile.isDirty = true;
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            AirdropMissionSettingFiles newmission = new AirdropMissionSettingFiles()
            {
                Filename = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\missions\\Airdrop_NewMission.json",
                MissionName = "NewMission",
                Difficulty = 0,
                Objective = 0,
                Reward = "",
                Enabled = 1,
                ShowNotification = 1,
                Weight = 0,
                MissionMaxTime = 0,
                Height = 0,
                Speed = 0,
                Container = "Random",
                DropLocation = new DropLocation()
                {
                    Name = "New Drop Location",
                    Radius = 100,
                    x = currentproject.MapSize / 2,
                    z = currentproject.MapSize / 2
                },
                Loot = new BindingList<containerLoot>(),
                Infected = new BindingList<string>(),
                ItemCount = -1,
                InfectedCount = -1
            };
            MissionSettings.MissionSettingFiles.Add(newmission);
            MissionsLB.SelectedIndex = -1;
            MissionsLB.SelectedIndex = MissionsLB.Items.Count - 1;
            pictureBox6.Invalidate();
        }
        private void darkButton65_Click(object sender, EventArgs e)
        {
            ContaminatedAreaMissionSettingFiles newcasf = new ContaminatedAreaMissionSettingFiles()
            {
                Filename = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\missions\\ContaminatedArea_NewMission.json",
                MissionName = "NewMission",
                Difficulty = 0,
                Objective = 0,
                Reward = "",
                Enabled = 1,
                Weight = 0,
                MissionMaxTime = 0,
                Data = new ContaminatedAreaMissionData()
                {
                    Pos = new decimal[] { currentproject.MapSize / 2,0, currentproject.MapSize / 2, },
                    Radius = 100,
                    PosHeight = 25,
                    NegHeight = 20,
                    InnerRingCount = 2,
                    InnerPartDist = 50,
                    OuterRingToggle = 1,
                    OuterPartDist = 40,
                    OuterOffset = 0,
                    VerticalLayers = 0,
                    VerticalOffset = 0,
                    ParticleName = "graphics/particles/contaminated_area_gas_bigass"
                },
                PlayerData = new ContaminatedAreaMissionPlayerdata()
                {
                    AroundPartName = "graphics/particles/contaminated_area_gas_around",
                    TinyPartName = "graphics/particles/contaminated_area_gas_around_tiny",
                    PPERequesterType = "PPERequester_ContaminatedAreaTint"
                },
                StartDecayLifetime = 600,
                FinishDecayLifetime = 300
            };
            MissionSettings.MissionSettingFiles.Add(newcasf);
            MissionsLB.SelectedIndex = -1;
            MissionsLB.SelectedIndex = MissionsLB.Items.Count - 1;
            pictureBox6.Invalidate();
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            int index = MissionsLB.SelectedIndex;
            if(MissionsLB.SelectedItem is AirdropMissionSettingFiles)
            {
                AirdropMissionSettingFiles amsf = MissionsLB.SelectedItem as AirdropMissionSettingFiles;
                File.Delete(amsf.Filename);
                MissionSettings.MissionSettingFiles.Remove(amsf);
            }
            else if (MissionsLB.SelectedItem is ContaminatedAreaMissionSettingFiles)
            {
                ContaminatedAreaMissionSettingFiles camsf = MissionsLB.SelectedItem as ContaminatedAreaMissionSettingFiles;
                File.Delete(camsf.Filename);
                MissionSettings.MissionSettingFiles.Remove(camsf);
            }
            MissionSettings.isDirty = true;
            if (MissionsLB.Items.Count == 0)
                MissionsLB.SelectedIndex = -1;
            else if (index == 0)
            {
                MissionsLB.SelectedIndex = -1;
                MissionsLB.SelectedIndex = 0;
            }
            else
                MissionsLB.SelectedIndex = index - 1;
            pictureBox6.Invalidate();
        }
        private void darkButton87_Click(object sender, EventArgs e)
        {
            List<string> removezombies = new List<string>();
            foreach (var item in listBox26.SelectedItems)
            {
                removezombies.Add(item.ToString());

            }
            foreach (string s in removezombies)
            {
                currentAirdropmissionfile.Infected.Remove(s);
                currentAirdropmissionfile.isDirty = true;
            }
        }
        private void darkButton86_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox24.SelectedItems)
            {
                string zombie = item.ToString();
                if (!currentAirdropmissionfile.Infected.Contains(zombie))
                {
                    currentAirdropmissionfile.Infected.Add(zombie);
                    currentAirdropmissionfile.isDirty = true;
                }
                else
                {
                    MessageBox.Show("Infected Type allready in the list.....");
                }
            }
        }
        private void darkButton85_Click(object sender, EventArgs e)
        {
            string zombie = textBox21.Text;
            if (!currentAirdropmissionfile.Infected.Contains(zombie))
            {
                currentAirdropmissionfile.Infected.Add(zombie);
                currentAirdropmissionfile.isDirty = true;
            }
            else
            {
                MessageBox.Show("Infected Type allready in the list.....");
            }
        }
        private void listBox30_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox30.SelectedItems.Count < 1) return;
            SCL = listBox30.SelectedItem as containerLoot;
            useraction = false;
            textBox22.Text = SCL.Name;
            if (SCL.Chance > 1)
                SCL.Chance = 1;
            trackBar8.Value = (int)(SCL.Chance * 1000);
            numericUpDown36.Value = SCL.Max;
            numericUpDown34.Value = SCL.Min;
            numericUpDown35.Value = SCL.QuantityPercent;
            listBox29.DisplayMember = "DisplayName";
            listBox29.ValueMember = "Value";
            listBox29.DataSource = SCL.Attachments;

            listBox28.DataSource = null;
            listBox27.DataSource = null;

            if (SCL.Variants.Count > 0)
            {
                listBox28.DisplayMember = "DisplayName";
                listBox28.ValueMember = "Value";
                listBox28.DataSource = SCL.Variants;
            }
            useraction = true;
        }
        private void darkButton95_Click(object sender, EventArgs e)
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
                    containerLoot Newloot = new containerLoot()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = (decimal)0.5,
                        Max = -1,
                        Min = 0,
                        Variants = new BindingList<lootVarients>()
                    };
                    currentAirdropmissionfile.Loot.Add(Newloot);
                    currentAirdropmissionfile.isDirty = true;
                }
            }
        }
        private void darkButton96_Click(object sender, EventArgs e)
        {
            currentAirdropmissionfile.Loot.Remove(CurrentAirdropContainerLoot);
            currentAirdropmissionfile.isDirty = true;
            Specificpopulatelistbox();
        }
        private void trackBar8_MouseUp(object sender, MouseEventArgs e)
        {
            if (SCL == null) return;
            SCL.Chance = ((decimal)trackBar8.Value) / 1000;
            currentAirdropmissionfile.isDirty = true;
        }
        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            darkLabel266.Text = ((decimal)(trackBar8.Value) / 10).ToString() + "%";
        }
        private void trackBar8_ValueChanged(object sender, EventArgs e)
        {
            darkLabel266.Text = ((decimal)(trackBar8.Value) / 10).ToString() + "%";
        }
        private void darkButton93_Click(object sender, EventArgs e)
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
                    if (!SCL.Attachments.Contains(l))
                    {
                        SCL.Attachments.Add(l);
                        currentAirdropmissionfile.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the list.....");
                }
            }
        }
        private void darkButton94_Click(object sender, EventArgs e)
        {
            SCL.Attachments.Remove(listBox29.GetItemText(listBox29.SelectedItem));
            currentAirdropmissionfile.isDirty = true;
        }
        private void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                SCL.QuantityPercent = (int)numericUpDown35.Value;
                currentAirdropmissionfile.isDirty = true;
            }
        }
        private void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                SCL.Min = (int)numericUpDown34.Value;
                currentAirdropmissionfile.isDirty = true;
            }
        }
        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {

            if (useraction)
            {
                SCL.Max = (int)numericUpDown36.Value;
                currentAirdropmissionfile.isDirty = true;
            }
        }
        private void darkButton92_Click(object sender, EventArgs e)
        {
            foreach (containerLoot scl in currentAirdropmissionfile.Loot)
            {
                scl.Chance = ((decimal)trackBar1.Value) / 1000;
            }
            currentAirdropmissionfile.isDirty = true;
        }
        private void listBox28_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox28.SelectedItems.Count < 1) return;
            SLootVarients = listBox28.SelectedItem as lootVarients;
            useraction = false;

            VarientChanceTrackBar.Value = (int)(SLootVarients.Chance * 1000);

            listBox27.DisplayMember = "DisplayName";
            listBox27.ValueMember = "Value";
            listBox27.DataSource = SLootVarients.Attachments;
        }
        private void darkButton90_Click(object sender, EventArgs e)
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
                if (CurrentAirdropContainerLoot.Variants.Count == 0)
                {
                    listBox28.DisplayMember = "DisplayName";
                    listBox28.ValueMember = "Value";
                    listBox28.DataSource = CurrentAirdropContainerLoot.Variants;
                }
                foreach (string l in addedtypes)
                {
                    lootVarients newlootVarients = new lootVarients()
                    {
                        Name = l,
                        Attachments = new BindingList<string>(),
                        Chance = (decimal)0.5,
                    };
                    SCL.Variants.Add(newlootVarients);
                    currentAirdropmissionfile.isDirty = true;
                }

            }
        }
        private void darkButton91_Click(object sender, EventArgs e)
        {
            SCL.Variants.Remove(SLootVarients);
            listBox28.DataSource = null;
            listBox28.SelectedIndex = -1;
            if (SCL.Variants.Count > 0)
            {
                listBox28.SelectedIndex = 0;
            }
            currentAirdropmissionfile.isDirty = true;
        }
        private void trackBar7_MouseUp(object sender, MouseEventArgs e)
        {
            if (SLootVarients == null) return;
            SLootVarients.Chance = ((decimal)trackBar7.Value) / 1000;
            currentAirdropmissionfile.isDirty = true;
        }
        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            darkLabel263.Text = ((decimal)(trackBar7.Value) / 10).ToString() + "%";
        }
        private void trackBar7_ValueChanged(object sender, EventArgs e)
        {
            darkLabel263.Text = ((decimal)(trackBar7.Value) / 10).ToString() + "%";
        }
        private void darkButton88_Click(object sender, EventArgs e)
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
                    if (!SLootVarients.Attachments.Contains(l))
                    {
                        SLootVarients.Attachments.Add(l);
                        currentAirdropmissionfile.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the Varients list.....");
                }
            }
        }
        private void darkButton89_Click(object sender, EventArgs e)
        {
            SLootVarients.Attachments.Remove(listBox28.GetItemText(listBox28.SelectedItem));
            currentAirdropmissionfile.isDirty = true;
        }
        //Contaminated Area missions
        public ContaminatedAreaMissionSettingFiles currentContaminatedAreaMissionFile { get; private set; }
        private void PosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.Pos[0] = (decimal)PosXNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void posYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.Pos[1] = (decimal)posYNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void posZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.Pos[2] = (decimal)posZNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void RadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.Radius = (int)RadiusNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void textBox15_TextChanged_1(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Filename = textBox15.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.MissionName = textBox16.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Enabled = checkBox7.Checked == true ? 1 : 0;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void numericUpDown30_ValueChanged_1(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.MissionMaxTime = (int)Helper.ConvertMinutesToSeconds((int)numericUpDown30.Value);
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Weight = numericUpDown29.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void InnerRingCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.InnerRingCount = (int)InnerRingCountNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void PosHeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.PosHeight = PosHeightNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void InnerPartDistNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.InnerPartDist = (int)InnerPartDistNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void NegHeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.NegHeight = NegHeightNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void OuterRingToggleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.OuterRingToggle = OuterRingToggleCB.Checked == true ? 1 : 0;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void VerticalLayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.VerticalLayers = (int)VerticalLayersNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void OuterPartDistNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.OuterPartDist = (int)OuterPartDistNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void VerticalOffsetNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.VerticalOffset = (int)VerticalOffsetNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void OuterOffsetNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.OuterOffset = (int)OuterOffsetNUD.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void ParticleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.Data.ParticleName = AroundPartNameTB.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void AroundPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.PlayerData.AroundPartName = AroundPartNameTB.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void TinyPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.PlayerData.TinyPartName = TinyPartNameTB.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void PPERequesterTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.PlayerData.PPERequesterType = PPERequesterTypeTB.Text;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.StartDecayLifetime = numericUpDown27.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentContaminatedAreaMissionFile.FinishDecayLifetime = numericUpDown28.Value;
            currentContaminatedAreaMissionFile.isDirty = true;
        }
        #endregion Missionsettings

        #region monitoringSettings
        public void LoadMonitoringSettingss()
        {
            useraction = false;
            MonitoringSettingsEnabledCB.Checked = MonitoringSettings.Enabled == 1 ? true : false;
            useraction = true;
        }
        private void MonitoringSettingsEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MonitoringSettings.Enabled = MonitoringSettingsEnabledCB.Checked == true ? 1 : 0;
            MonitoringSettings.isDirty = true;
        }
        #endregion monitoringsettings

        #region nametagsettings
        public void LoadNameTagSettings()
        {
            useraction = false;
            EnablePlayerTagsCB.Checked = NameTagSettings.EnablePlayerTags == 1 ? true : false;
            PlayerTagViewRangeNUD.Value = NameTagSettings.PlayerTagViewRange;
            PlayerTagsIconTB.Text = NameTagSettings.PlayerTagsIcon;
            ShowPlayerTagsInSafeZonesCB.Checked = NameTagSettings.OnlyInSafeZones == 1 ? true : false;
            ShowPlayerTagsInTerritoriesCB.Checked = NameTagSettings.OnlyInTerritories == 1 ? true : false;
            PlayerTagsColorPB.Invalidate();
            PlayerNameColorPB.Invalidate();
            useraction = true;
        }
        private void PlayerTagsColor_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(NameTagSettings.PlayerTagsColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void PlayerNameColorPB_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(NameTagSettings.PlayerNameColor);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void EnablePlayerTagsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NameTagSettings.EnablePlayerTags = EnablePlayerTagsCB.Checked == true ? 1 : 0;
            NameTagSettings.isDirty = true;
        }
        private void ShowPlayerTagsInSafeZonesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NameTagSettings.OnlyInSafeZones = ShowPlayerTagsInSafeZonesCB.Checked == true ? 1 : 0;
            NameTagSettings.isDirty = true;
        }
        private void ShowPlayerTagsInTerritoriesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NameTagSettings.OnlyInTerritories = ShowPlayerTagsInTerritoriesCB.Checked == true ? 1 : 0;
            NameTagSettings.isDirty = true;
        }
        private void PlayerTagViewRangeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NameTagSettings.PlayerTagViewRange = (int)PlayerTagViewRangeNUD.Value;
            NameTagSettings.isDirty = true;
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NameTagSettings.PlayerTagsIcon = PlayerTagsIconTB.Text;
            NameTagSettings.isDirty = true;
        }
        private void PlayerTagsColorPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(NameTagSettings.PlayerTagsColor);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                NameTagSettings.PlayerTagsColor = cpick.Color.ToArgb();
                PlayerTagsColorPB.Invalidate();
                NameTagSettings.isDirty = true;
            }
        }
        private void PlayerNameColorPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(NameTagSettings.PlayerNameColor);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                NameTagSettings.PlayerNameColor = cpick.Color.ToArgb();
                PlayerNameColorPB.Invalidate();
                NameTagSettings.isDirty = true;
            }
        }
        #endregion nametagsettings

        #region NortificationSchedulerSettings
        public Notification currentNotification;
        public void LoadNotificationSchedulerSettings()
        {
            useraction = false;
            SchedulerEnabledCB.Checked = NotificationSchedulerSettings.Enabled == 1 ? true : false;
            UTCTimeCB.Checked = NotificationSchedulerSettings.UTC == 1 ? true : false;
            UseMissionTimeCB.Checked = NotificationSchedulerSettings.UseMissionTime == 1 ? true : false;

            SchedulerNotificaionLB.DisplayMember = "DisplayName";
            SchedulerNotificaionLB.ValueMember = "Value";
            SchedulerNotificaionLB.DataSource = NotificationSchedulerSettings.Notifications;

            useraction = true;
        }
        private void SchedulerNotificaionLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SchedulerNotificaionLB.SelectedItems.Count < 1) return;
            currentNotification = SchedulerNotificaionLB.SelectedItem as Notification;
            useraction = false;
            NSTitleTB.Text = currentNotification.Title;
            DateTime Dtime = DateTime.Now.Date;
            Dtime = Dtime.AddHours(currentNotification.Hour);
            Dtime = Dtime.AddMinutes(currentNotification.Minute);
            Dtime = Dtime.AddSeconds(currentNotification.Second);
            NSTimeTP.Value = Dtime;
            NSTextTB.Text = currentNotification.Text;
            NotfifcationIconComboBox.SelectedIndex = NotfifcationIconComboBox.FindStringExact(currentNotification.Icon);
            pictureBox5.Invalidate();
            useraction = true;
        }
        private void NSTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNotification.Title = NSTitleTB.Text;
            SchedulerNotificaionLB.Refresh();
            NotificationSchedulerSettings.isDirty = true;
        }
        private void NSTimeTP_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DateTime Dtime = NSTimeTP.Value;
            currentNotification.Hour = Dtime.Hour;
            currentNotification.Minute = Dtime.Minute;
            currentNotification.Second = Dtime.Second;
            NotificationSchedulerSettings.isDirty = true;
        }
        private void UTCTimeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NotificationSchedulerSettings.UTC = UTCTimeCB.Checked == true ? 1 : 0;
            NotificationSchedulerSettings.isDirty = true;
        }
        private void UseMissionTimeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NotificationSchedulerSettings.UseMissionTime = UseMissionTimeCB.Checked == true ? 1 : 0;
            NotificationSchedulerSettings.isDirty = true;
        }
        private void SchedulerEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NotificationSchedulerSettings.Enabled = SchedulerEnabledCB.Checked == true ? 1 : 0;
            NotificationSchedulerSettings.isDirty = true;
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            string col = currentNotification.Color;
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            cpick.Color = ColorTranslator.FromHtml(col1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                string Colour = cpick.Color.Name.ToUpper().Substring(2) + cpick.Color.Name.ToUpper().Substring(0, 2);
                currentNotification.Color = Colour;
                pb.Invalidate();
                NotificationSchedulerSettings.isDirty = true;
            }
        }
        private void darkButton81_Click(object sender, EventArgs e)
        {
            DateTime localtime = dateTimePicker1.Value;
            darkLabel252.Text = "UTC Time : " + TimeZoneInfo.ConvertTimeToUtc(localtime, TimeZoneInfo.Local).ToString();
        }
        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            if (currentNotification == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = currentNotification.Color;
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            Color colour = ColorTranslator.FromHtml(col1);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void NotfifcationIconComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNotification.Icon = NotfifcationIconComboBox.SelectedItem.ToString();
            NotificationSchedulerSettings.isDirty = true;
        }
        private void NSTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNotification.Text = NSTextTB.Text;
            NotificationSchedulerSettings.isDirty = true;
        }
        private void darkButton59_Click(object sender, EventArgs e)
        {
            Notification newnotification = new Notification()
            {
                Title = "New Notification",
                Hour = 00,
                Minute = 00,
                Second = 00,
                Text = "",
                Icon = "Info",
                Color = "27272DFF"
            };
            NotificationSchedulerSettings.Notifications.Add(newnotification);
            NotificationSchedulerSettings.isDirty = true;
        }
        private void darkButton60_Click(object sender, EventArgs e)
        {
            NotificationSchedulerSettings.Notifications.Remove(SchedulerNotificaionLB.SelectedItem as Notification);
            NotificationSchedulerSettings.isDirty = true;
        }
        #endregion NotificatioSchedulerSettings

        #region notifications
        private void LoadNotificationSettings()
        {
            useraction = false;
            EnableNotificationCB.Checked = NotificationSettings.EnableNotification == 1 ? true : false;
            ShowPlayerJoinServerCB.Checked = NotificationSettings.ShowPlayerJoinServer == 1 ? true : false;
            JoinMessageTypeCB.Checked = NotificationSettings.JoinMessageType == 1 ? true : false;
            ShowPlayerLeftServerCB.Checked = NotificationSettings.ShowPlayerLeftServer == 1 ? true : false;
            LeftMessageTypeCB.Checked = NotificationSettings.LeftMessageType == 1 ? true : false;
            ShowAirdropStartedCB.Checked = NotificationSettings.ShowAirdropStarted == 1 ? true : false;
            ShowAirdropClosingOnCB.Checked = NotificationSettings.ShowAirdropClosingOn == 1 ? true : false;
            ShowAirdropDroppedCB.Checked = NotificationSettings.ShowAirdropDropped == 1 ? true : false;
            ShowAirdropEndedCB.Checked = NotificationSettings.ShowAirdropEnded == 1 ? true : false;
            ShowPlayerAirdropStartedCB.Checked = NotificationSettings.ShowPlayerAirdropStarted == 1 ? true : false;
            ShowPlayerAirdropClosingOnCB.Checked = NotificationSettings.ShowPlayerAirdropClosingOn == 1 ? true : false;
            ShowPlayerAirdropDroppedCB.Checked = NotificationSettings.ShowPlayerAirdropDropped == 1 ? true : false;
            ShowTerritoryNotificationsCB.Checked = NotificationSettings.ShowTerritoryNotifications == 1 ? true : false;
            EnableKillFeedCB.Checked = NotificationSettings.EnableKillFeed == 1 ? true : false;
            KillFeedMessageTypeCB.Checked = NotificationSettings.KillFeedMessageType == 1 ? true : false;
            KillFeedFallCB.Checked = NotificationSettings.KillFeedFall == 1 ? true : false;
            KillFeedCarHitDriverCB.Checked = NotificationSettings.KillFeedCarHitDriver == 1 ? true : false;
            KillFeedCarHitNoDriverCB.Checked = NotificationSettings.KillFeedCarHitNoDriver == 1 ? true : false;
            KillFeedCarCrashCB.Checked = NotificationSettings.KillFeedCarCrash == 1 ? true : false;
            KillFeedCarCrashCrewCB.Checked = NotificationSettings.KillFeedCarCrashCrew == 1 ? true : false;
            KillFeedHeliHitDriverCB.Checked = NotificationSettings.KillFeedHeliHitDriver == 1 ? true : false;
            KillFeedHeliHitNoDriverCB.Checked = NotificationSettings.KillFeedHeliHitNoDriver == 1 ? true : false;
            KillFeedHeliCrashCB.Checked = NotificationSettings.KillFeedHeliCrash == 1 ? true : false;
            KillFeedHeliCrashCrewCB.Checked = NotificationSettings.KillFeedHeliCrashCrew == 1 ? true : false;
            KillFeedBoatHitDriverCB.Checked = NotificationSettings.KillFeedBoatHitDriver == 1 ? true : false;
            KillFeedBoatHitNoDriverCB.Checked = NotificationSettings.KillFeedBoatHitNoDriver == 1 ? true : false;
            KillFeedBoatCrashCB.Checked = NotificationSettings.KillFeedBoatCrash == 1 ? true : false;
            KillFeedBoatCrashCrewCB.Checked = NotificationSettings.KillFeedBoatCrashCrew == 1 ? true : false;
            KillFeedBarbedWireCB.Checked = NotificationSettings.KillFeedBarbedWire == 1 ? true : false;
            KillFeedFireCB.Checked = NotificationSettings.KillFeedFire == 1 ? true : false;
            KillFeedWeaponExplosionCB.Checked = NotificationSettings.KillFeedWeaponExplosion == 1 ? true : false;
            KillFeedDehydrationCB.Checked = NotificationSettings.KillFeedDehydration == 1 ? true : false;
            KillFeedStarvationCB.Checked = NotificationSettings.KillFeedStarvation == 1 ? true : false;
            KillFeedBleedingCB.Checked = NotificationSettings.KillFeedBleeding == 1 ? true : false;
            KillFeedStatusEffectsCB.Checked = NotificationSettings.KillFeedStatusEffects == 1 ? true : false;
            KillFeedSuicideCB.Checked = NotificationSettings.KillFeedSuicide == 1 ? true : false;
            KillFeedWeaponCB.Checked = NotificationSettings.KillFeedWeapon == 1 ? true : false;
            KillFeedMeleeWeaponCB.Checked = NotificationSettings.KillFeedMeleeWeapon == 1 ? true : false;
            KillFeedBarehandsCB.Checked = NotificationSettings.KillFeedBarehands == 1 ? true : false;
            KillFeedInfectedCB.Checked = NotificationSettings.KillFeedInfected == 1 ? true : false;
            KillFeedAnimalCB.Checked = NotificationSettings.KillFeedAnimal == 1 ? true : false;
            KillFeedKilledUnknownCB.Checked = NotificationSettings.KillFeedKilledUnknown == 1 ? true : false;
            KillFeedDiedUnknownCB.Checked = NotificationSettings.KillFeedDiedUnknown == 1 ? true : false;
            useraction = true;
        }
        private void NotificationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            NotificationSettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            NotificationSettings.isDirty = true;
        }
        #endregion notifications

        #region PartySettings
        private void loadpartysettings()
        {
            useraction = false;
            EnablePartiesCB.Checked = PartySettings.EnableParties == 1 ? true : false;
            MaxMembersInPartyNUD.Value = PartySettings.MaxMembersInParty;
            UseWholeMapForInviteListCB.Checked = PartySettings.UseWholeMapForInviteList == 1 ? true : false;
            ShowPartyMember3DMarkersCB.Checked = PartySettings.ShowPartyMember3DMarkers == 1 ? true : false;
            ShowDistanceUnderPartyMembersMarkersCB.Checked = PartySettings.ShowDistanceUnderPartyMembersMarkers == 1 ? true : false;
            ShowNameOnPartyMembersMarkersCB.Checked = PartySettings.ShowNameOnPartyMembersMarkers == 1 ? true : false;
            EnableQuickMarkerCB.Checked = PartySettings.EnableQuickMarker == 1 ? true : false;
            ShowDistanceUnderQuickMarkersCB.Checked = PartySettings.ShowDistanceUnderQuickMarkers == 1 ? true : false;
            ShowNameOnQuickMarkersCB.Checked = PartySettings.ShowNameOnQuickMarkers == 1 ? true : false;
            CanCreatePartyMarkersCB.Checked = PartySettings.CanCreatePartyMarkers == 1 ? true : false;
            ShowPartyMemberHUDCB.Checked = PartySettings.ShowPartyMemberHUD == 1 ? true : false;
            ShowHUDMemberBloodCB.Checked = PartySettings.ShowHUDMemberBlood == 1 ? true : false;
            ShowHUDMemberStatesCB.Checked = PartySettings.ShowHUDMemberStates == 1 ? true : false;
            ShowHUDMemberStanceCB.Checked = PartySettings.ShowHUDMemberStance == 1 ? true : false;
            ShowPartyMemberMapMarkersCB.Checked = PartySettings.ShowPartyMemberMapMarkers == 1 ? true : false;
            ShowHUDMemberDistanceCB.Checked = PartySettings.ShowHUDMemberDistance == 1 ? true : false;
            useraction = true;
        }
        private void PartySettingsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox cb = sender as CheckBox;
            PartySettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            PartySettings.isDirty = true;
        }
        private void PartySettingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            PartySettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            PartySettings.isDirty = true;
        }
        #endregion PArtySettings

        #region PlayerListsettings
        private void LoadPlayerListsettings()
        {
            useraction = false;
            EnablePlayerListCB.Checked = PlayerListSettings.EnablePlayerList == 1 ? true : false;
            EnableTooltipCB.Checked = PlayerListSettings.EnableTooltip == 1 ? true : false;
            useraction = true;
        }
        private void checkBox7EnableTooltipCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            PlayerListSettings.EnableTooltip = EnableTooltipCB.Checked == true ? 1 : 0;
            PlayerListSettings.isDirty = true;
        }
        private void EnablePlayerListCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            PlayerListSettings.EnablePlayerList = EnablePlayerListCB.Checked == true ? 1 : 0;
            PlayerListSettings.isDirty = true;
        }
        #endregion PlayerListsettings

        #region Raidsettings
        private void loadRaidSettings()
        {
            useraction = false;
            BaseBuildingRaidModeComboBox.DataSource = Enum.GetValues(typeof(BaseRaidMode));
            BaseBuildingRaidModeComboBox.SelectedItem = (BaseRaidMode)RaidSettings.BaseBuildingRaidMode;
            ExplosionTimeNUD.Value = (decimal)RaidSettings.ExplosionTime;

            EnableExplosiveWhitelistCB.Checked = RaidSettings.EnableExplosiveWhitelist == 1 ? true : false;
            ExplosionDamageMultiplierNUD.Value = (decimal)RaidSettings.ExplosionDamageMultiplier;
            ProjectileDamageMultiplierNUD.Value = (decimal)RaidSettings.ProjectileDamageMultiplier;
            CanRaidSafesCB.Checked = RaidSettings.CanRaidSafes == 1 ? true : false;
            SafeRaidUseScheduleCB.Checked = RaidSettings.SafeRaidUseSchedule == 1 ? true : false;
            SafeExplosionDamageMultiplierNUD.Value = (decimal)RaidSettings.SafeExplosionDamageMultiplier;
            SafeProjectileDamageMultiplierNUD.Value = (decimal)RaidSettings.SafeProjectileDamageMultiplier;
            SafeRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.SafeRaidToolTimeSeconds;
            SafeRaidToolCyclesNUD.Value = (decimal)RaidSettings.SafeRaidToolCycles;
            SafeRaidToolDamagePercentNUD.Value = (decimal)RaidSettings.SafeRaidToolDamagePercent;
            CanRaidBarbedWireCB.Checked = RaidSettings.CanRaidBarbedWire == 1 ? true : false;
            BarbedWireRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.BarbedWireRaidToolTimeSeconds;
            BarbedWireRaidToolCyclesNUD.Value = (decimal)RaidSettings.BarbedWireRaidToolCycles;
            BarbedWireRaidToolDamagePercentNUD.Value = (decimal)RaidSettings.BarbedWireRaidToolDamagePercent;
            CanRaidLocksOnWallsCB.Checked = RaidSettings.CanRaidLocksOnWalls == 1 ? true : false;
            CanRaidLocksOnFencesCB.Checked = RaidSettings.CanRaidLocksOnFences == 1 ? true : false;
            CanRaidLocksOnTentsCB.Checked = RaidSettings.CanRaidLocksOnTents == 1 ? true : false;
            LockOnWallRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.LockOnWallRaidToolTimeSeconds;
            LockOnFenceRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.LockOnFenceRaidToolTimeSeconds;
            LockOnTentRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.LockOnTentRaidToolTimeSeconds;
            LockRaidToolCyclesNUD.Value = (decimal)RaidSettings.LockRaidToolCycles;
            LockRaidToolDamagePercentNUD.Value = (decimal)RaidSettings.LockRaidToolDamagePercent;
            CanRaidLocksOnContainersCB.Checked = RaidSettings.CanRaidLocksOnContainers == 1 ? true : false;
            LockOnContainerRaidToolTimeSecondsNUD.Value = (decimal)RaidSettings.LockOnContainerRaidToolTimeSeconds;
            LockOnContainerRaidToolCyclesNUD.Value = (decimal)RaidSettings.LockOnContainerRaidToolCycles;
            LockOnContainerRaidToolDamagePercentNUD.Value = (decimal)RaidSettings.LockOnContainerRaidToolDamagePercent;

            ExplosiveDamageWhitelistLB.DisplayMember = "DisplayName";
            ExplosiveDamageWhitelistLB.ValueMember = "Value";
            ExplosiveDamageWhitelistLB.DataSource = RaidSettings.ExplosiveDamageWhitelist;

            SafeRaidToolsLB.DisplayMember = "DisplayName";
            SafeRaidToolsLB.ValueMember = "Value";
            SafeRaidToolsLB.DataSource = RaidSettings.SafeRaidTools;

            BarbedWireRaidToolsLB.DisplayMember = "DisplayName";
            BarbedWireRaidToolsLB.ValueMember = "Value";
            BarbedWireRaidToolsLB.DataSource = RaidSettings.BarbedWireRaidTools;

            LockRaidToolsLB.DisplayMember = "DisplayName";
            LockRaidToolsLB.ValueMember = "Value";
            LockRaidToolsLB.DataSource = RaidSettings.LockRaidTools;

            LockOnContainerRaidToolsLB.DisplayMember = "DisplayName";
            LockOnContainerRaidToolsLB.ValueMember = "Value";
            LockOnContainerRaidToolsLB.DataSource = RaidSettings.LockOnContainerRaidTools;

            LockOnContainerRaidToolsLB.DisplayMember = "DisplayName";
            LockOnContainerRaidToolsLB.ValueMember = "Value";
            LockOnContainerRaidToolsLB.DataSource = RaidSettings.LockOnContainerRaidTools;

            ScheduleLB.DisplayMember = "DisplayName";
            ScheduleLB.ValueMember = "Value";
            ScheduleLB.DataSource = RaidSettings.Schedule;

            useraction = true;
        }
        private void BaseBuildingRaidModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BaseRaidMode brm = (BaseRaidMode)BaseBuildingRaidModeComboBox.SelectedItem;
            int br = (int)brm;
            RaidSettings.BaseBuildingRaidMode = (int)brm;
            RaidSettings.isDirty = true;
        }
        private void RaidSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            RaidSettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            RaidSettings.isDirty = true;
        }
        private void RaidSettings_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.DecimalPlaces > 0)
                RaidSettings.SetFloatValue(nud.Name.Substring(0, nud.Name.Length - 3), (float)nud.Value);
            else
                RaidSettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            RaidSettings.isDirty = true;
        }
        private void RaidsettingsButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "AddEDWButton":
                case "AddBWRTButton":
                case "AddSRTButton":
                case "AddLRTButton":
                case "AddLOCRTButton":
                    AddItemfromTypes form = new AddItemfromTypes();
                    form.vanillatypes = vanillatypes;
                    form.ModTypes = ModTypes;
                    form.currentproject = currentproject;
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        List<string> addedtypes = form.addedtypes.ToList();
                        foreach (string l in addedtypes)
                        {
                            switch (button.Name)
                            {
                                case "AddEDWButton":
                                    if (!RaidSettings.ExplosiveDamageWhitelist.Contains(l))
                                    {
                                        RaidSettings.ExplosiveDamageWhitelist.Add(l);
                                        RaidSettings.isDirty = true;
                                    }
                                    break;
                                case "AddBWRTButton":
                                    if (!RaidSettings.BarbedWireRaidTools.Contains(l))
                                    {
                                        RaidSettings.BarbedWireRaidTools.Add(l);
                                        RaidSettings.isDirty = true;
                                    }
                                    break;
                                case "AddSRTButton":
                                    if (!RaidSettings.SafeRaidTools.Contains(l))
                                    {
                                        RaidSettings.SafeRaidTools.Add(l);
                                        RaidSettings.isDirty = true;
                                    }
                                    break;
                                case "AddLRTButton":
                                    if (!RaidSettings.LockRaidTools.Contains(l))
                                    {
                                        RaidSettings.LockRaidTools.Add(l);
                                        RaidSettings.isDirty = true;
                                    }
                                    break;
                                case "AddLOCRTButton":
                                    if (!RaidSettings.LockOnContainerRaidTools.Contains(l))
                                    {
                                        RaidSettings.LockOnContainerRaidTools.Add(l);
                                        RaidSettings.isDirty = true;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "RemoveEDWButton":
                    RaidSettings.ExplosiveDamageWhitelist.Remove(ExplosiveDamageWhitelistLB.GetItemText(ExplosiveDamageWhitelistLB.SelectedItem));
                    break;
                case "RemoveBWRTButton":
                    RaidSettings.BarbedWireRaidTools.Remove(BarbedWireRaidToolsLB.GetItemText(BarbedWireRaidToolsLB.SelectedItem));
                    break;
                case "RemoveSRTButton":
                    RaidSettings.SafeRaidTools.Remove(SafeRaidToolsLB.GetItemText(SafeRaidToolsLB.SelectedItem));
                    break;
                case "RemoveLRTButton":
                    RaidSettings.LockRaidTools.Remove(LockRaidToolsLB.GetItemText(LockRaidToolsLB.SelectedItem));
                    break;
                case "RemoveLOCRTButton":
                    RaidSettings.LockOnContainerRaidTools.Remove(LockOnContainerRaidToolsLB.GetItemText(LockOnContainerRaidToolsLB.SelectedItem));
                    break;
            }
            RaidSettings.isDirty = true;
        }
        public Schedule currentSchedule;
        private void ScheduleLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScheduleLB.SelectedItems.Count < 1) return;
            currentSchedule = ScheduleLB.SelectedItem as Schedule;
            useraction = false;
            WeekdayTB.Text = currentSchedule.Weekday;
            StartHourNUD.Value = currentSchedule.StartHour;
            StartMinuteNUD.Value = currentSchedule.StartMinute;
            DurationMinutesNUD.Value = currentSchedule.DurationMinutes;
            useraction = true;
        }
        private void WeekdayTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSchedule.Weekday = WeekdayTB.Text;
            SafeZoneSettings.isDirty = true;
        }
        private void StartHourNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSchedule.StartHour = (int)StartMinuteNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void StartMinuteNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSchedule.StartMinute = (int)StartMinuteNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void DurationMinutesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentSchedule.DurationMinutes = (int)DurationMinutesNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void darkButton83_Click(object sender, EventArgs e)
        {
            Schedule newschedule = new Schedule()
            {
                Weekday = "Weekday",
                StartHour = 0,
                StartMinute = 0,
                DurationMinutes = 60
            };
            RaidSettings.Schedule.Add(newschedule);
            RaidSettings.isDirty = true;
        }
        private void darkButton82_Click(object sender, EventArgs e)
        {
            Schedule removeschedule = ScheduleLB.SelectedItem as Schedule;
            RaidSettings.Schedule.Remove(removeschedule);
            RaidSettings.isDirty = true;
        }
        #endregion Raid Settigns

        #region SafeZoneSettings
        public CircleZones currentcircleZone;
        public PolygonZones currentpolygonZones;
        public Polygonpoints currentpolygonpoint;
        public string CurrentSafeZoneType;
        public int ZoneScale = 1;

        private void LoadSafeZonesettings()
        {
            useraction = false;
            SafeZoneSettings.SetCircleNames();
            SafeZoneSettings.SetPolygonNames();
            SafeZoneSettings.Convertpolygonarrays();

            EnabledCB.Checked = SafeZoneSettings.Enabled == 1 ? true : false;
            FrameRateCheckSafeZoneInMsNUD.Value = (decimal)SafeZoneSettings.FrameRateCheckSafeZoneInMs;
            DisableVehicleDamageInSafeZoneCB.Checked = SafeZoneSettings.DisableVehicleDamageInSafeZone == 1 ? true : false;
            EnableForceSZCleanupCB.Checked = SafeZoneSettings.EnableForceSZCleanup == 1 ? true : false;
            ItemLifetimeInSafeZoneNUD.Value = (decimal)SafeZoneSettings.ItemLifetimeInSafeZone;
            ActorsPerTickNUD.Value = SafeZoneSettings.ActorsPerTick;


            listBox14.DisplayMember = "DisplayName";
            listBox14.ValueMember = "Value";
            listBox14.DataSource = SafeZoneSettings.CircleZones;

            listBox15.DisplayMember = "DisplayName";
            listBox15.ValueMember = "Value";
            listBox15.DataSource = SafeZoneSettings.PolygonZones;

            ForceSZCleanup_ExcludedItemsLB.DisplayMember = "DisplayName";
            ForceSZCleanup_ExcludedItemsLB.ValueMember = "Value";
            ForceSZCleanup_ExcludedItemsLB.DataSource = SafeZoneSettings.ForceSZCleanup_ExcludedItems;


            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawSafeZones);
            ZoneScale = 1;
            Setscale();
            useraction = true;
        }
        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar3.Value;
            Setscale();
        }
        private void Setscale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void DrawSafeZones(object sender, PaintEventArgs e)
        {
            foreach (CircleZones zones in SafeZoneSettings.CircleZones)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(zones.Center[0], 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(zones.Center[2], 0) * scalevalue);
                int radius = (int)(Math.Round(zones.Radius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (currentcircleZone != null && currentcircleZone.CircleSafeZoneName == zones.CircleSafeZoneName)
                    pen.Color = Color.LimeGreen;
                getCircle(e.Graphics, pen, center, radius);
            }
            foreach (PolygonZones pz in SafeZoneSettings.PolygonZones)
            {
                if (pz.Polygonpoints.Count > 1)
                {
                    float scalevalue = ZoneScale * 0.05f;
                    for (int i = 0; i < pz.Polygonpoints.Count; i++)
                    {
                        Pen pen = new Pen(Color.Blue, 4);
                        int ax = (int)(Math.Round(pz.Polygonpoints[i].points[0], 0) * scalevalue);
                        int ay = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz.Polygonpoints[i].points[2], 0) * scalevalue);
                        int bx = 0;
                        int by = 0;
                        if (i == pz.Polygonpoints.Count - 1)
                        {
                            bx = (int)(Math.Round(pz.Polygonpoints[0].points[0], 0) * scalevalue);
                            by = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz.Polygonpoints[0].points[2], 0) * scalevalue);
                        }
                        else
                        {
                            bx = (int)(Math.Round(pz.Polygonpoints[i + 1].points[0], 0) * scalevalue);
                            by = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz.Polygonpoints[i + 1].points[2], 0) * scalevalue);
                        }
                        if (pz.polygonSafeZoneName == currentpolygonZones.polygonSafeZoneName)
                            pen.Color = Color.Purple;
                        e.Graphics.DrawLine(pen, ax, ay, bx, by);
                    }
                }
            }
        }
        private void listBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedItems.Count < 1) return;
            currentcircleZone = listBox14.SelectedItem as CircleZones;
            useraction = false;
            CurrentSafeZoneType = "Circle";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            numericUpDown17.Value = (decimal)currentcircleZone.Radius;
            numericUpDown18.Value = (decimal)currentcircleZone.Center[0];
            numericUpDown19.Value = (decimal)currentcircleZone.Center[1];
            numericUpDown20.Value = (decimal)currentcircleZone.Center[2];
            useraction = true;
            pictureBox2.Invalidate();
        }
        private void listBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox15.SelectedItems.Count < 1) return;
            currentpolygonZones = listBox15.SelectedItem as PolygonZones;
            useraction = false;
            CurrentSafeZoneType = "Polygon";
            radioButton1.Checked = false;
            radioButton2.Checked = true; ;
            listBox16.DisplayMember = "DisplayName";
            listBox16.ValueMember = "Value";
            listBox16.DataSource = currentpolygonZones.Polygonpoints;
            pictureBox2.Invalidate();
            useraction = true;
        }
        private void listBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox16.SelectedItems.Count < 1) return;
            currentpolygonpoint = listBox16.SelectedItem as Polygonpoints;
            useraction = false;
            radioButton2.Checked = true;
            numericUpDown21.Value = (decimal)currentpolygonpoint.points[0];
            numericUpDown22.Value = (decimal)currentpolygonpoint.points[1];
            numericUpDown23.Value = (decimal)currentpolygonpoint.points[2];

            useraction = true;
        }
        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = ZoneScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (CurrentSafeZoneType == "Circle" && currentcircleZone != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    numericUpDown18.Value = (decimal)(mouseEventArgs.X / scalevalue);
                    numericUpDown20.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                    if (MapData.FileExists)
                    {
                        numericUpDown19.Value = (decimal)(MapData.gethieght(currentcircleZone.Center[0], currentcircleZone.Center[2]));
                    }
                    Cursor.Current = Cursors.Default;
                    SafeZoneSettings.isDirty = true;
                    pictureBox2.Invalidate();
                }
                if (CurrentSafeZoneType == "Polygon" && currentpolygonpoint != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    numericUpDown21.Value = (decimal)(mouseEventArgs.X / scalevalue);
                    numericUpDown23.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                    if (MapData.FileExists)
                    {
                        numericUpDown22.Value = (decimal)(MapData.gethieght(currentpolygonpoint.points[0], currentpolygonpoint.points[2]));
                    }
                    Cursor.Current = Cursors.Default;
                    SafeZoneSettings.isDirty = true;
                    pictureBox2.Invalidate();
                }
            }
        }
        private void Zones_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Tag.ToString())
            {
                case "Circle":
                    if (currentcircleZone != null)
                    {
                        CurrentSafeZoneType = "Circle";
                    }
                    break;
                case "Polygon":
                    if (currentpolygonZones != null)
                    {
                        CurrentSafeZoneType = "Polygon";
                    }
                    break;
                default:
                    break;
            }

        }
        private void DisableVehicleDamageInSafeZoneCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.DisableVehicleDamageInSafeZone = DisableVehicleDamageInSafeZoneCB.Checked == true ? 1 : 0;
            SafeZoneSettings.isDirty = true;

        }
        private void EnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.Enabled = EnabledCB.Checked == true ? 1 : 0;
            SafeZoneSettings.isDirty = true;
        }
        private void EnableForceSZCleanupCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.EnableForceSZCleanup = EnableForceSZCleanupCB.Checked == true ? 1 : 0;
            SafeZoneSettings.isDirty = true;
        }
        private void FrameRateCheckSafeZoneInMsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.FrameRateCheckSafeZoneInMs = (int)FrameRateCheckSafeZoneInMsNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void ActorsPerTickNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.ActorsPerTick = (int)ActorsPerTickNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void ItemLifetimeInSafeZoneNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SafeZoneSettings.ItemLifetimeInSafeZone = (float)ItemLifetimeInSafeZoneNUD.Value;
            SafeZoneSettings.isDirty = true;
        }
        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentcircleZone.Center[0] = (float)numericUpDown18.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentcircleZone.Center[1] = (float)numericUpDown19.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentcircleZone.Center[2] = (float)numericUpDown20.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();

        }
        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentcircleZone.Radius = (float)numericUpDown17.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentpolygonpoint.points[0] = (float)numericUpDown21.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentpolygonpoint.points[1] = (float)numericUpDown22.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentpolygonpoint.points[2] = (float)numericUpDown23.Value;
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            CircleZones newcircle = new CircleZones();
            newcircle.Center = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 };
            newcircle.Radius = 500;
            newcircle.CircleSafeZoneName = "New circle Zone";
            SafeZoneSettings.CircleZones.Add(newcircle);
            SafeZoneSettings.SetCircleNames();
            listBox14.SelectedIndex = listBox14.Items.Count - 1;
            pictureBox2.Invalidate();
            SafeZoneSettings.isDirty = true;
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            SafeZoneSettings.RemoveCircleZone(currentcircleZone);
            SafeZoneSettings.SetCircleNames();
            pictureBox2.Invalidate();
            SafeZoneSettings.isDirty = true;
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            PolygonZones newpzone = new PolygonZones();
            newpzone.Polygonpoints = new BindingList<Polygonpoints>();
            newpzone.polygonSafeZoneName = "New polygon Zone";
            SafeZoneSettings.PolygonZones.Add(newpzone);
            SafeZoneSettings.SetPolygonNames();
            listBox15.SelectedIndex = -1;
            listBox15.SelectedIndex = listBox15.Items.Count - 1;
            SafeZoneSettings.isDirty = true;
        }
        private void darkButton27_Click(object sender, EventArgs e)
        {
            SafeZoneSettings.RemovePolygonZone(currentpolygonZones);
            SafeZoneSettings.SetPolygonNames();
            pictureBox2.Invalidate();
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            Polygonpoints pp = new Polygonpoints();
            int i = currentproject.MapSize / 2;
            pp.points = new float[] { i, 0, i };
            pp.name = "New point";
            currentpolygonZones.Polygonpoints.Add(pp);
            currentpolygonZones.SetPointnames();
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            currentpolygonZones.removepoints(currentpolygonpoint);
            currentpolygonZones.SetPointnames();
            SafeZoneSettings.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton54_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!SafeZoneSettings.ForceSZCleanup_ExcludedItems.Contains(l))
                    {
                        SafeZoneSettings.ForceSZCleanup_ExcludedItems.Add(l);
                        SafeZoneSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton53_Click(object sender, EventArgs e)
        {
            SafeZoneSettings.ForceSZCleanup_ExcludedItems.Remove(ForceSZCleanup_ExcludedItemsLB.GetItemText(ForceSZCleanup_ExcludedItemsLB.SelectedItem));
            SafeZoneSettings.isDirty = true;
        }
        #endregion SafeZonesettings

        #region SocialMediaSettings
        private void LoadsocialMediaSettings()
        {
            useraction = false;
            toolStripButton22.AutoSize = false;
            toolStripButton22.AutoSize = true;
            toolStripButton23.AutoSize = false;
            toolStripButton23.AutoSize = true;


            listBox23.DisplayMember = "DisplayName";
            listBox23.ValueMember = "Value";
            listBox23.DataSource = SocialMediaSettings.NewsFeedTexts;

            listBox25.DisplayMember = "DisplayName";
            listBox25.ValueMember = "Value";
            listBox25.DataSource = SocialMediaSettings.NewsFeedLinks;

            useraction = true;
        }
        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 0;
            if (tabControl4.SelectedIndex == 0)
                toolStripButton22.Checked = true;
        }
        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 1;
            if (tabControl4.SelectedIndex == 1)
                toolStripButton23.Checked = true;
        }
        private void tabControl4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (tabControl4.SelectedIndex)
            {
                case 0:
                    toolStripButton23.Checked = false;
                    toolStripButton23.ForeColor = Color.FromArgb(75, 110, 175);  
                    toolStripButton22.ForeColor = SystemColors.Control;

                    break;
                case 1:
                    toolStripButton22.Checked = false;
                    toolStripButton23.ForeColor = SystemColors.Control;
                    toolStripButton22.ForeColor = Color.FromArgb(75, 110, 175);
                    break;
                default:
                    break;
            }
        }
        public Newsfeedtext currentNewsfeedtext;
        public Newsfeedlink currentNewsfeedlink;
        private void listBox23_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox23.SelectedItems.Count < 1) return;
            currentNewsfeedtext = listBox23.SelectedItem as Newsfeedtext;
            useraction = false;
            textBox19.Text = currentNewsfeedtext.m_Title;
            DiscordTB.Text = currentNewsfeedtext.m_Text;
            useraction = true;
        }
        private void darkButton80_Click(object sender, EventArgs e)
        {
            Newsfeedtext newfeedtext = new Newsfeedtext()
            {
                m_Title = "New Title",
                m_Text = "New Text"
            };
            SocialMediaSettings.NewsFeedTexts.Add(newfeedtext);
            SocialMediaSettings.isDirty = true;

        }
        private void darkButton79_Click(object sender, EventArgs e)
        {
            if (listBox23.SelectedItems.Count < 1) return;
            SocialMediaSettings.NewsFeedTexts.Remove(currentNewsfeedtext);
            SocialMediaSettings.isDirty = true;

        }
        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNewsfeedtext.m_Title = textBox19.Text;
            SocialMediaSettings.isDirty = true;
        }
        private void DiscordTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNewsfeedtext.m_Text = DiscordTB.Text;
            SocialMediaSettings.isDirty = true;
        }
        private void listBox25_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox25.SelectedItems.Count < 1) return;
            currentNewsfeedlink = listBox25.SelectedItem as Newsfeedlink;
            useraction = false;
            textBox23.Text = currentNewsfeedlink.m_Label;
            textBox24.Text = currentNewsfeedlink.m_Icon;
            textBox25.Text = currentNewsfeedlink.m_URL;

            useraction = true;
        }
        private void darkButton77_Click(object sender, EventArgs e)
        {
            Newsfeedlink newNewsfeedlink = new Newsfeedlink()
            {
                m_Label = "New label",
                m_Icon = "New icon",
                m_URL = "New Url"
            };
            SocialMediaSettings.NewsFeedLinks.Add(newNewsfeedlink);
            SocialMediaSettings.isDirty = true;
        }
        private void darkButton78_Click(object sender, EventArgs e)
        {
            if (listBox23.SelectedItems.Count < 1) return;
            SocialMediaSettings.NewsFeedLinks.Remove(currentNewsfeedlink);
            SocialMediaSettings.isDirty = true;
        }
        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNewsfeedlink.m_Label = textBox23.Text;
            SocialMediaSettings.isDirty = true;
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNewsfeedlink.m_Icon = textBox24.Text;
            SocialMediaSettings.isDirty = true;
        }
        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentNewsfeedlink.m_URL = textBox25.Text;
            SocialMediaSettings.isDirty = true;
        }
        #endregion SocialMediaSettings

        #region SpawnSettings
        public int SpawnScale = 1;
        public SpawnLocations currentSpawnLocation;
        public float[] currentSpawnPosition;
        public Gear currentuppergear;
        public Gear currentpantsgear;
        public Gear currentbackpackgear;
        public Gear currentvestgear;

        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton15.Checked = false;
            toolStripButton15.ForeColor = Color.FromArgb(75, 110, 175);
            toolStripButton16.Checked = false;
            toolStripButton16.ForeColor = Color.FromArgb(75, 110, 175);
            toolStripButton17.Checked = false;
            toolStripButton17.ForeColor = Color.FromArgb(75, 110, 175);
            toolStripButton19.Checked = false;
            toolStripButton19.ForeColor = Color.FromArgb(75, 110, 175);
            switch (SpawnTabControl.SelectedIndex)
            {
                case 0:
                    toolStripButton15.Checked = true;
                    toolStripButton15.ForeColor = SystemColors.Control;
                    break;
                case 1:
                    toolStripButton16.Checked = true;
                    toolStripButton16.ForeColor = SystemColors.Control;
                    break;
                case 2:
                    toolStripButton17.Checked = true;
                    toolStripButton17.ForeColor = SystemColors.Control;
                    break;
                case 3:
                    toolStripButton19.Checked = true;
                    toolStripButton19.ForeColor = SystemColors.Control;
                    break;
            }
        }
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            SpawnTabControl.SelectedIndex = 0;
        }
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            SpawnTabControl.SelectedIndex = 1;
        }
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            SpawnTabControl.SelectedIndex = 2;
        }
        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            SpawnTabControl.SelectedIndex = 3;
        }
        private void LoadSpawnsettings()
        {
            useraction = false;
            SpawnTabControl.ItemSize = new Size(0, 1);
            EnableCustomClothingCB.Checked = SpawnSettings.StartingClothing.EnableCustomClothing == 1 ? true : false;
            SetRandomHealthCB.Checked = SpawnSettings.StartingClothing.SetRandomHealth == 1 ? true : false;
            HeadGearLB.DisplayMember = "DisplayName";
            HeadGearLB.ValueMember = "Value";
            HeadGearLB.DataSource = SpawnSettings.StartingClothing.Headgear;
            GlassesLB.DisplayMember = "DisplayName";
            GlassesLB.ValueMember = "Value";
            GlassesLB.DataSource = SpawnSettings.StartingClothing.Glasses;
            MasksLB.DisplayMember = "DisplayName";
            MasksLB.ValueMember = "Value";
            MasksLB.DataSource = SpawnSettings.StartingClothing.Masks;
            TopsLB.DisplayMember = "DisplayName";
            TopsLB.ValueMember = "Value";
            TopsLB.DataSource = SpawnSettings.StartingClothing.Tops;
            VestsLB.DisplayMember = "DisplayName";
            VestsLB.ValueMember = "Value";
            VestsLB.DataSource = SpawnSettings.StartingClothing.Vests;
            GlovesLB.DisplayMember = "DisplayName";
            GlovesLB.ValueMember = "Value";
            GlovesLB.DataSource = SpawnSettings.StartingClothing.Gloves;
            PantsLB.DisplayMember = "DisplayName";
            PantsLB.ValueMember = "Value";
            PantsLB.DataSource = SpawnSettings.StartingClothing.Pants;
            BeltsLB.DisplayMember = "DisplayName";
            BeltsLB.ValueMember = "Value";
            BeltsLB.DataSource = SpawnSettings.StartingClothing.Belts;
            ShoesLB.DisplayMember = "DisplayName";
            ShoesLB.ValueMember = "Value";
            ShoesLB.DataSource = SpawnSettings.StartingClothing.Shoes;
            ArmbandsLB.DisplayMember = "DisplayName";
            ArmbandsLB.ValueMember = "Value";
            ArmbandsLB.DataSource = SpawnSettings.StartingClothing.Armbands;
            BackpacksLB.DisplayMember = "DisplayName";
            BackpacksLB.ValueMember = "Value";
            BackpacksLB.DataSource = SpawnSettings.StartingClothing.Backpacks;
            useraction = false;
            EnableStartingGearCB.Checked = SpawnSettings.StartingGear.EnableStartingGear == 1 ? true : false;
            SetRandomHealthSGCB.Checked = SpawnSettings.StartingGear.SetRandomHealth == 1 ? true : false;
            ApplyEnergySourcesCB.Checked = SpawnSettings.StartingGear.ApplyEnergySources == 1 ? true : false;

            useraction = false;
            UseUpperGearCB.Checked = SpawnSettings.StartingGear.UseUpperGear == 1 ? true : false;
            UpperGearLB.DisplayMember = "DisplayName";
            UpperGearLB.ValueMember = "Value";
            UpperGearLB.DataSource = SpawnSettings.StartingGear.UpperGear;

            useraction = false;
            UsePantsGearCB.Checked = SpawnSettings.StartingGear.UsePantsGear == 1 ? true : false;
            PantsGearLB.DisplayMember = "DisplayName";
            PantsGearLB.ValueMember = "Value";
            PantsGearLB.DataSource = SpawnSettings.StartingGear.PantsGear;

            useraction = false;
            UseBackpackGearCB.Checked = SpawnSettings.StartingGear.UseBackpackGear == 1 ? true : false;
            BackpackGearLB.DisplayMember = "DisplayName";
            BackpackGearLB.ValueMember = "Value";
            BackpackGearLB.DataSource = SpawnSettings.StartingGear.BackpackGear;

            useraction = false;
            UseVestGearCB.Checked = SpawnSettings.StartingGear.UseVestGear == 1 ? true : false;
            VestGearLB.DisplayMember = "DisplayName";
            VestGearLB.ValueMember = "Value";
            VestGearLB.DataSource = SpawnSettings.StartingGear.VestGear;

            UsePrimaryWeaponCB.Checked = SpawnSettings.StartingGear.UsePrimaryWeapon == 1 ? true : false;
            if (SpawnSettings.StartingGear.PrimaryWeapon != null)
            {
                PrimaryWeaponNameTB.Text = SpawnSettings.StartingGear.PrimaryWeapon.ClassName;
                PrimaryWeaponQuantityNUD.Value = (decimal)SpawnSettings.StartingGear.PrimaryWeapon.Quantity;
                PrimaryWeaponAttachLB.DisplayMember = "DisplayName";
                PrimaryWeaponAttachLB.ValueMember = "Value";
                PrimaryWeaponAttachLB.DataSource = SpawnSettings.StartingGear.PrimaryWeapon.Attachments;
            }

            UseSecondaryWeaponCB.Checked = SpawnSettings.StartingGear.UseSecondaryWeapon == 1 ? true : false;
            if (SpawnSettings.StartingGear.SecondaryWeapon != null)
            {
                SecondaryWeaponNameTB.Text = SpawnSettings.StartingGear.SecondaryWeapon.ClassName;
                SecondaryWeaponQuantityNUD.Value = (decimal)SpawnSettings.StartingGear.SecondaryWeapon.Quantity;
                SecondaryWeaponAttachLB.DisplayMember = "DisplayName";
                SecondaryWeaponAttachLB.ValueMember = "Value";
                SecondaryWeaponAttachLB.DataSource = SpawnSettings.StartingGear.SecondaryWeapon.Attachments;
            }

            EnableSpawnSelectionCB.Checked = SpawnSettings.EnableSpawnSelection == 1 ? true : false;
            SpawnOnTerritoryCB.Checked = SpawnSettings.SpawnOnTerritory == 1 ? true : false;
            SpawnHealthValueNUD.Value = (decimal)SpawnSettings.SpawnHealthValue;
            SpawnEnergyValueNUD.Value = (decimal)SpawnSettings.SpawnEnergyValue;
            SpawnWaterValueNUD.Value = (decimal)SpawnSettings.SpawnWaterValue;
            EnableRespawnCooldownsCB.Checked = SpawnSettings.EnableRespawnCooldowns == 1 ? true : false;
            RespawnCooldownNUD.Value = SpawnSettings.RespawnCooldown;
            TerritoryRespawnCooldownNUD.Value = SpawnSettings.TerritoryRespawnCooldown;
            PunishMultispawnCB.Checked = SpawnSettings.PunishMultispawn == 1 ? true : false;
            PunishCooldownNUD.Value = SpawnSettings.PunishCooldown;
            SpawnCreateDeathMarkerCB.Checked = SpawnSettings.CreateDeathMarker == 1 ? true : false;
            BackgroundImagePathTB.Text = SpawnSettings.BackgroundImagePath;
            UseLoadoutsCB.Checked = SpawnSettings.UseLoadouts == 1 ? true : false;

            MaleLoadoutLB.DisplayMember = "DisplayName";
            MaleLoadoutLB.ValueMember = "Value";
            MaleLoadoutLB.DataSource = SpawnSettings.MaleLoadouts;
            useraction = false;

            FemaleLoadoutLB.DisplayMember = "DisplayName";
            FemaleLoadoutLB.ValueMember = "Value";
            FemaleLoadoutLB.DataSource = SpawnSettings.FemaleLoadouts;
            useraction = false;

            //spawn locations
            SpawnLocationLB.DisplayMember = "DisplayName";
            SpawnLocationLB.ValueMember = "Value";
            SpawnLocationLB.DataSource = SpawnSettings.SpawnLocations;
            useraction = false;

            pictureBox4.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox4.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox4.Paint += new PaintEventHandler(DrawCurrentSelectedSpawns);
            trackBar5.Value = 1;
            SetSpawnScale();

            useraction = true;
        }
        private void DrawCurrentSelectedSpawns(object sender, PaintEventArgs e)
        {
            if(currentSpawnPosition == null) { return; }
            float scalevalue = SpawnScale * 0.05f;
            foreach (float[] position in currentSpawnLocation.Positions)
            {
                int centerX = (int)(Math.Round(position[0], 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(position[2], 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (position == currentSpawnPosition)
                    pen.Color = Color.LimeGreen;
                getCircle(e.Graphics, pen, center, 4);
            }
        }
        private void BackgroundImagePathTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.BackgroundImagePath = BackgroundImagePathTB.Text;
            SpawnSettings.isDirty = true;
        }
        private void SpawnLocationLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnLocationLB.SelectedItems.Count < 1) return;
            currentSpawnLocation = SpawnLocationLB.SelectedItem as SpawnLocations;
            useraction = false;
            SpawnLocationNameTB.Text = currentSpawnLocation.Name;
            UseCooldownCB.Checked = currentSpawnLocation.UseCooldown == 1 ? true : false;
            SpawnPositionsLB.DisplayMember = "DisplayName";
            SpawnPositionsLB.ValueMember = "Value";
            SpawnPositionsLB.DataSource = currentSpawnLocation.Positions;

            pictureBox4.Invalidate();
            useraction = true;
        }
        private void SpawnPositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnPositionsLB.SelectedItems.Count < 1) return;
            currentSpawnPosition = SpawnPositionsLB.SelectedItem as float[];
            useraction = false;
            pictureBox4.Invalidate();
            SpawnXNUD.Value = (decimal)currentSpawnPosition[0];
            SpawnYNUD.Value = (decimal)currentSpawnPosition[1];
            SpawnZNUD.Value = (decimal)currentSpawnPosition[2];
            useraction = true;
        }
        private void trackBar5_MouseUp(object sender, MouseEventArgs e)
        {
            SpawnScale = trackBar5.Value;
            SetSpawnScale();
        }
        private void SetSpawnScale()
        {
            float scalevalue = SpawnScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox4.Size = new Size(newsize, newsize);
        }
        private void pictureBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = SpawnScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                Cursor.Current = Cursors.WaitCursor;
                currentSpawnPosition[0] = (float)(mouseEventArgs.X / scalevalue);
                currentSpawnPosition[2] = (float)((newsize - mouseEventArgs.Y) / scalevalue);
                if (MapData.FileExists)
                {
                    currentSpawnPosition[1] = (float)(MapData.gethieght(currentSpawnPosition[0], currentSpawnPosition[2]));
                }
                Cursor.Current = Cursors.Default;
                SpawnSettings.isDirty = true;
                pictureBox4.Invalidate();
                SpawnXNUD.Value = (decimal)currentSpawnPosition[0];
                SpawnYNUD.Value = (decimal)currentSpawnPosition[1];
                SpawnZNUD.Value = (decimal)currentSpawnPosition[2];
            }
        }
        private void darkButton48_Click(object sender, EventArgs e)
        {
            SpawnLocations newspawnLocations = new SpawnLocations();
            newspawnLocations.Name = "New Locations";
            newspawnLocations.Positions = new BindingList<float[]>();
            SpawnSettings.SpawnLocations.Add(newspawnLocations);
            SpawnLocationLB.SelectedIndex = SpawnLocationLB.Items.Count - 1;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            SpawnSettings.SpawnLocations.Remove(currentSpawnLocation);
            SpawnSettings.isDirty = true;
            SpawnLocationLB.SelectedIndex = -1;
            if (SpawnLocationLB.Items.Count > 0)
                SpawnLocationLB.SelectedIndex = 0;
        }
        private void darkButton50_Click(object sender, EventArgs e)
        {
            currentSpawnLocation.Positions.Add(new float[] { currentproject.MapSize/2, 0, currentproject.MapSize/2 });
            SpawnSettings.isDirty = true;

        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            currentSpawnLocation.Positions.Remove(currentSpawnPosition);
            SpawnSettings.isDirty = true;
            SpawnPositionsLB.SelectedIndex = -1;
            if (SpawnPositionsLB.Items.Count > 0)
                SpawnPositionsLB.SelectedIndex = 0;

        }
        private void UseCooldownCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentSpawnLocation.UseCooldown = UseCooldownCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void SpawnLocationNameTB_TextChanged(object sender, EventArgs e)
        {
            if(!useraction) { return; }
            currentSpawnLocation.Name = SpawnLocationNameTB.Text;
            SpawnSettings.isDirty = true;
            SpawnLocationLB.Refresh();
        }
        private void SpawnWaterValue_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.SpawnWaterValue = (int)SpawnWaterValueNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void SpawnEnergyValueNUD_ValueChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            SpawnSettings.SpawnEnergyValue = (int)SpawnEnergyValueNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void SpawnHealthValueNUD_ValueChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            SpawnSettings.SpawnHealthValue = (int)SpawnHealthValueNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void PunishMultispawnCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            SpawnSettings.PunishMultispawn = PunishMultispawnCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void SpawnCreateDeathMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            SpawnSettings.CreateDeathMarker = SpawnCreateDeathMarkerCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void RespawnCooldownNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.RespawnCooldown = (int)RespawnCooldownNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void PunishCooldownNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.PunishCooldown = (int)PunishCooldownNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void PunishTimeframeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.PunishTimeframe = (int)PunishTimeframeNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void RespawnUTCTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.TerritoryRespawnCooldown = (int)TerritoryRespawnCooldownNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void SpawnOnTerritoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.SpawnOnTerritory = SpawnOnTerritoryCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        //private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!useraction) return;
        //    SpawnSettings.SpawnSelectionScreenMenuID = (int)SpawnSelectionIDNUD.Value;
        //    SpawnSettings.isDirty = true;
        //}
        private void EnableSpawnSelectionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.EnableSpawnSelection = EnableSpawnSelectionCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void EnableRespawnCooldownsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.EnableRespawnCooldowns = EnableRespawnCooldownsCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;

        }
        private void EnableCustomClothingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingClothing.EnableCustomClothing = EnableCustomClothingCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void SetRandomHealthCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingClothing.SetRandomHealth = SetRandomHealthCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void StartingClothingADD_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Button button = sender as Button;
                    switch (button.Name)
                    {
                        case "HeaderGearAddBT":
                            if (!SpawnSettings.StartingClothing.Headgear.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Headgear.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "GlassesAddBT":
                            if (!SpawnSettings.StartingClothing.Glasses.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Glasses.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "MasksAddBT":
                            if (!SpawnSettings.StartingClothing.Masks.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Masks.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "TopsAddBT":
                            if (!SpawnSettings.StartingClothing.Tops.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Tops.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "VestsAddBT":
                            if (!SpawnSettings.StartingClothing.Vests.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Vests.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "GlovesAddBT":
                            if (!SpawnSettings.StartingClothing.Gloves.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Gloves.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "PantsAddBT":
                            if (!SpawnSettings.StartingClothing.Pants.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Pants.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "BeltsAddBT":
                            if (!SpawnSettings.StartingClothing.Belts.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Belts.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "ShoesAddBT":
                            if (!SpawnSettings.StartingClothing.Shoes.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Shoes.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "ArmbandsAddBT":
                            if (!SpawnSettings.StartingClothing.Armbands.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Armbands.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                        case "BackPacksAddBT":
                            if (!SpawnSettings.StartingClothing.Backpacks.Contains(l))
                            {
                                SpawnSettings.StartingClothing.Backpacks.Add(l);
                                SpawnSettings.isDirty = true;
                            }
                            break;
                    }
                }
            }
        }
        private void StartingClothingRemove_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "HeaderGearRemoveBT":
                    SpawnSettings.StartingClothing.Headgear.Remove(HeadGearLB.GetItemText(HeadGearLB.SelectedItem));
                    break;
                case "GlassesRemoveBT":
                    SpawnSettings.StartingClothing.Glasses.Remove(GlassesLB.GetItemText(GlassesLB.SelectedItem));
                    break;
                case "MasksRemoveBT":
                    SpawnSettings.StartingClothing.Masks.Remove(MasksLB.GetItemText(MasksLB.SelectedItem));
                    break;
                case "TopsRemoveBT":
                    SpawnSettings.StartingClothing.Tops.Remove(TopsLB.GetItemText(TopsLB.SelectedItem));
                    break;
                case "VestsRemoveBT":
                    SpawnSettings.StartingClothing.Vests.Remove(VestsLB.GetItemText(VestsLB.SelectedItem));
                    break;
                case "GlovesRemoveBT":
                    SpawnSettings.StartingClothing.Gloves.Remove(GlovesLB.GetItemText(GlovesLB.SelectedItem));
                    break;
                case "PantsRemoveBT":
                    SpawnSettings.StartingClothing.Pants.Remove(PantsLB.GetItemText(PantsLB.SelectedItem));
                    break;
                case "BeltsRemoveBT":
                    SpawnSettings.StartingClothing.Belts.Remove(BeltsLB.GetItemText(BeltsLB.SelectedItem));
                    break;
                case "ShoesRemoveBT":
                    SpawnSettings.StartingClothing.Shoes.Remove(ShoesLB.GetItemText(ShoesLB.SelectedItem));
                    break;
                case "ArmbandsRemoveBT":
                    SpawnSettings.StartingClothing.Armbands.Remove(ArmbandsLB.GetItemText(ArmbandsLB.SelectedItem));
                    break;
                case "BackPacksRemoveBT":
                    SpawnSettings.StartingClothing.Backpacks.Remove(BackpacksLB.GetItemText(BackpacksLB.SelectedItem));
                    break;
            }
        }
        private void EnableStartingGearCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingGear.EnableStartingGear = EnableStartingGearCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void ApplyEnergySourcesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingGear.ApplyEnergySources = ApplyEnergySourcesCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void SetRandomHealthSGCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingGear.SetRandomHealth = SetRandomHealthSGCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }

        private void UseUpperGearCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UseUpperGearCB.Checked)
            {
                case true:
                    UseUpperGearGB.Visible = true;
                    break;
                case false:
                    UseUpperGearGB.Visible = false;
                    break;
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UseUpperGear = UseUpperGearCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void UsePantsGearCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UsePantsGearCB.Checked)
            {
                case true:
                    UsePantsGB.Visible = true;
                    break;
                case false:
                    UsePantsGB.Visible = false;
                    break;
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UsePantsGear = UsePantsGearCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void UseBackpackGearCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UseBackpackGearCB.Checked)
            {
                case true:
                    BackpackGearGB.Visible = true;
                    break;
                case false:
                    BackpackGearGB.Visible = false;
                    break;
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UseBackpackGear = UseBackpackGearCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void UseVestGearCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UseVestGearCB.Checked)
            {
                case true:
                    VestGearGB.Visible = true;
                    break;
                case false:
                    VestGearGB.Visible = false;
                    break;
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UseVestGear = UseVestGearCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void UsePrimaryWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UsePrimaryWeaponCB.Checked)
            {
                case true:
                    PrimaryWeaponGB.Visible = true;
                    break;
                case false:
                    PrimaryWeaponGB.Visible = false;
                    break;
            }
            if(SpawnSettings.StartingGear.PrimaryWeapon == null)
            {
                foreach(Control c in PrimaryWeaponGB.Controls)
                {
                    if (c.Name == "PrimaryWeaponAddBT")
                        c.Visible = true;
                    else 
                        c.Visible = false;
                }
            }
            else
            {
                foreach (Control c in PrimaryWeaponGB.Controls)
                {
                    if (c.Name == "PrimaryWeaponAddBT")
                        c.Visible = false;
                    else
                        c.Visible = true;
                }
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UsePrimaryWeapon = UsePrimaryWeaponCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }
        private void UseSecondaryWeaponCB_CheckedChanged(object sender, EventArgs e)
        {
            switch (UseSecondaryWeaponCB.Checked)
            {
                case true:
                    SecondaryWeaponGB.Visible = true;
                    break;
                case false:
                    SecondaryWeaponGB.Visible = false;
                    break;
            }
            if (SpawnSettings.StartingGear.SecondaryWeapon == null)
            {
                foreach (Control c in SecondaryWeaponGB.Controls)
                {
                    if (c.Name == "SecondaryWeaponAddTB")
                        c.Visible = true;
                    else
                        c.Visible = false;
                }
            }
            else
            {
                foreach (Control c in PrimaryWeaponGB.Controls)
                {
                    if (c.Name == "SecondaryWeaponAddTB")
                        c.Visible = false;
                    else
                        c.Visible = true;
                }
            }
            if (!useraction) return;
            SpawnSettings.StartingGear.UseSecondaryWeapon = UseSecondaryWeaponCB.Checked == true ? 1 : 0;
            SpawnSettings.isDirty = true;
        }

        private void UpperGearLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UpperGearLB.SelectedItems.Count < 1) return;
            currentuppergear = UpperGearLB.SelectedItem as Gear;
            useraction = false;
            UGClassNameTB.Text = currentuppergear.ClassName;
            UGQuantityNUD.Value = (decimal)currentuppergear.Quantity;
            UpperGearAttachLB.DisplayMember = "DisplayName";
            UpperGearAttachLB.ValueMember = "Value";
            UpperGearAttachLB.DataSource = currentuppergear.Attachments;
            useraction = true;
        }
        private void UpperGearAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newuppergear = new Gear();
                    newuppergear.ClassName = l;
                    newuppergear.Quantity = -1;
                    newuppergear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.UpperGear.Add(newuppergear);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void UpperGearRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.UpperGear.Remove(currentuppergear);
            SpawnSettings.isDirty = true;
        }
        private void UpperGearAddAttachmentBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentuppergear.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void UpperGearRemoveAttchmentBT_Click(object sender, EventArgs e)
        {
            currentuppergear.Attachments.Remove(UpperGearAttachLB.GetItemText(UpperGearAttachLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }
        private void UGQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentuppergear.Quantity = (int)UGQuantityNUD.Value;
            SpawnSettings.isDirty = true;
        }

        private void PantsGearLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PantsGearLB.SelectedItems.Count < 1) return;
            currentpantsgear = PantsGearLB.SelectedItem as Gear;
            useraction = false;
            PantsGearNameBT.Text = currentpantsgear.ClassName;
            PantsGearQuantityNUD.Value = (decimal)currentpantsgear.Quantity;
            PantsGearAttachLB.DisplayMember = "DisplayName";
            PantsGearAttachLB.ValueMember = "Value";
            PantsGearAttachLB.DataSource = currentpantsgear.Attachments;
            useraction = true;
        }
        private void PantsGearAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newgear = new Gear();
                    newgear.ClassName = l;
                    newgear.Quantity = -1;
                    newgear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.PantsGear.Add(newgear);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void PantsGearRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.PantsGear.Remove(currentpantsgear);
            SpawnSettings.isDirty = true;
        }
        private void PantsGearQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentpantsgear.Quantity = (int)PantsGearQuantityNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void PantsGearAttachAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentpantsgear.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void PantsGearAttachRemoveBT_Click(object sender, EventArgs e)
        {
            currentpantsgear.Attachments.Remove(PantsGearAttachLB.GetItemText(PantsGearAttachLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }

        private void BackpackGearLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BackpackGearLB.SelectedItems.Count < 1) return;
            currentbackpackgear = BackpackGearLB.SelectedItem as Gear;
            useraction = false;
            BackpackGearNameTB.Text = currentbackpackgear.ClassName;
            BackpackGearQuantityNUD.Value = (decimal)currentbackpackgear.Quantity;
            BackpackGearAttachLB.DisplayMember = "DisplayName";
            BackpackGearAttachLB.ValueMember = "Value";
            BackpackGearAttachLB.DataSource = currentbackpackgear.Attachments;
            useraction = true;
        }
        private void BackpackGearADDBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newgear = new Gear();
                    newgear.ClassName = l;
                    newgear.Quantity = -1;
                    newgear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.BackpackGear.Add(newgear);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void BackpackGearRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.BackpackGear.Remove(currentbackpackgear);
            SpawnSettings.isDirty = true;
        }
        private void BackpackGearQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentbackpackgear.Quantity = (int)BackpackGearQuantityNUD.Value;
            SpawnSettings.isDirty = true;

        }
        private void BackpackGearAttachAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentbackpackgear.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void BackpackGearAttachRemoveBT_Click(object sender, EventArgs e)
        {
            currentbackpackgear.Attachments.Remove(BackpackGearAttachLB.GetItemText(BackpackGearAttachLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }

        private void VestGearLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VestGearLB.SelectedItems.Count < 1) return;
            currentvestgear = VestGearLB.SelectedItem as Gear;
            useraction = false;
            VestGearNameTB.Text = currentvestgear.ClassName;
            VestGearQuantityNUD.Value = (decimal)currentvestgear.Quantity;
            VestGearAttachLB.DisplayMember = "DisplayName";
            VestGearAttachLB.ValueMember = "Value";
            VestGearAttachLB.DataSource = currentvestgear.Attachments;
            useraction = true;
        }
        private void VestGearAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newgear = new Gear();
                    newgear.ClassName = l;
                    newgear.Quantity = -1;
                    newgear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.VestGear.Add(newgear);
                    SpawnSettings.isDirty = true;
                }

            }
        }
        private void VestGearRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.VestGear.Remove(currentbackpackgear);
            SpawnSettings.isDirty = true;

        }
        private void VestGearQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentbackpackgear.Quantity = (int)VestGearQuantityNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void VestGearAttachAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentvestgear.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void VestGearAttachRemoveBT_Click(object sender, EventArgs e)
        {
            currentvestgear.Attachments.Remove(VestGearLB.GetItemText(VestGearLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }

        private void PrimaryWeaponAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newgear = new Gear();
                    newgear.ClassName = l;
                    newgear.Quantity = -1;
                    newgear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.PrimaryWeapon = newgear;
                    SpawnSettings.isDirty = true;
                }
                PrimaryWeaponNameTB.Text = SpawnSettings.StartingGear.PrimaryWeapon.ClassName;
                PrimaryWeaponQuantityNUD.Value = (decimal)SpawnSettings.StartingGear.PrimaryWeapon.Quantity;
                PrimaryWeaponAttachLB.DisplayMember = "DisplayName";
                PrimaryWeaponAttachLB.ValueMember = "Value";
                PrimaryWeaponAttachLB.DataSource = SpawnSettings.StartingGear.PrimaryWeapon.Attachments;
                foreach (Control c in PrimaryWeaponGB.Controls)
                {
                    if (c.Name == "PrimaryWeaponAddBT")
                        c.Visible = false;
                    else
                        c.Visible = true;
                }
            }
        }
        private void PrimaryWeaponRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.PrimaryWeapon = null;
            SpawnSettings.isDirty = true;
            foreach (Control c in PrimaryWeaponGB.Controls)
            {
                if (c.Name == "PrimaryWeaponAddBT")
                    c.Visible = true;
                else
                    c.Visible = false;
            }
        }
        private void PrimaryWeaponQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingGear.PrimaryWeapon.Quantity = (int)PrimaryWeaponQuantityNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void PrimaryWeaponAttachAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    SpawnSettings.StartingGear.PrimaryWeapon.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void PrimaryWeaponAttachRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.PrimaryWeapon.Attachments.Remove(PrimaryWeaponAttachLB.GetItemText(PrimaryWeaponAttachLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }

        private void SecondaryWeaponAddTB_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Gear newgear = new Gear();
                    newgear.ClassName = l;
                    newgear.Quantity = -1;
                    newgear.Attachments = new BindingList<string>();
                    SpawnSettings.StartingGear.SecondaryWeapon = newgear;
                    SpawnSettings.isDirty = true;
                }
                SecondaryWeaponNameTB.Text = SpawnSettings.StartingGear.SecondaryWeapon.ClassName;
                SecondaryWeaponQuantityNUD.Value = (decimal)SpawnSettings.StartingGear.SecondaryWeapon.Quantity;
                SecondaryWeaponAttachLB.DisplayMember = "DisplayName";
                SecondaryWeaponAttachLB.ValueMember = "Value";
                SecondaryWeaponAttachLB.DataSource = SpawnSettings.StartingGear.SecondaryWeapon.Attachments;
                foreach (Control c in SecondaryWeaponGB.Controls)
                {
                    if (c.Name == "SecondaryWeaponAddTB")
                        c.Visible = false;
                    else
                        c.Visible = true;
                }
            }
        }
        private void SecondaryWeaponRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.SecondaryWeapon = null;
            SpawnSettings.isDirty = true;
            foreach (Control c in SecondaryWeaponGB.Controls)
            {
                if (c.Name == "SecondaryWeaponAddTB")
                    c.Visible = true;
                else
                    c.Visible = false;
            }
        }
        private void SecondaryWeaponQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnSettings.StartingGear.SecondaryWeapon.Quantity = (int)SecondaryWeaponQuantityNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void SecondaryWeaponAttachAddBT_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    SpawnSettings.StartingGear.SecondaryWeapon.Attachments.Add(l);
                    SpawnSettings.isDirty = true;
                }
            }
        }
        private void SecondaryWeaponAttachRemoveBT_Click(object sender, EventArgs e)
        {
            SpawnSettings.StartingGear.SecondaryWeapon.Attachments.Remove(SecondaryWeaponAttachLB.GetItemText(SecondaryWeaponAttachLB.SelectedItem));
            SpawnSettings.isDirty = true;
        }

        public SpawnLoadouts CurrentMaleLoaouts;
        public SpawnLoadouts CurrentFemaleLoaouts;
        private void UseLoadoutsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            SpawnSettings.UseLoadouts = UseLoadoutsCB.Checked == true ? 1 : 0;
            groupBox66.Visible = SpawnSettings.UseLoadouts == 1 ? true : false;
            groupBox67.Visible = SpawnSettings.UseLoadouts == 1 ? true : false;
            SpawnSettings.isDirty = true;
        }
        private void listBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaleLoadoutLB.SelectedItems.Count < 1) return;
            CurrentMaleLoaouts = MaleLoadoutLB.SelectedItem as SpawnLoadouts;
            useraction = false;

            MaleNameTB.Text = CurrentMaleLoaouts.Loadout;
            MaleChanceNUD.Value = CurrentMaleLoaouts.Chance;

            useraction = true;
        }
        private void listBox23_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FemaleLoadoutLB.SelectedItems.Count < 1) return;
            CurrentFemaleLoaouts = FemaleLoadoutLB.SelectedItem as SpawnLoadouts;
            useraction = false;
            FemaleNameTB.Text = CurrentFemaleLoaouts.Loadout;
            FemaleChanceNUD.Value = CurrentFemaleLoaouts.Chance;
            useraction = true;
        }
        private void MaleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentMaleLoaouts.Loadout = MaleNameTB.Text;
            SpawnSettings.isDirty = true;
        }
        private void MaleChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentMaleLoaouts.Chance = MaleChanceNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void FemaleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentFemaleLoaouts.Loadout = FemaleNameTB.Text;
            SpawnSettings.isDirty = true;
        }
        private void FemaleChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentFemaleLoaouts.Chance = FemaleChanceNUD.Value;
            SpawnSettings.isDirty = true;
        }
        private void darkButton63_Click(object sender, EventArgs e)
        {
            SpawnLoadouts newloadout = new SpawnLoadouts()
            { Chance = (decimal)1.0, Loadout = "Player" };
            SpawnSettings.MaleLoadouts.Add(newloadout);
            SpawnSettings.isDirty = true;
        }
        private void darkButton64_Click(object sender, EventArgs e)
        {
            SpawnSettings.MaleLoadouts.Remove(CurrentMaleLoaouts);
            SpawnSettings.isDirty = true;
            MaleLoadoutLB.SelectedIndex = -1;
            if (MaleLoadoutLB.Items.Count > 0)
                MaleLoadoutLB.SelectedIndex = 0;
        }
        private void darkButton61_Click(object sender, EventArgs e)
        {
            SpawnLoadouts newloadout = new SpawnLoadouts()
            { Chance = (decimal)1.0, Loadout = "Player" };
            SpawnSettings.FemaleLoadouts.Add(newloadout);
            SpawnSettings.isDirty = true;
        }
        private void darkButton62_Click(object sender, EventArgs e)
        {
            SpawnSettings.FemaleLoadouts.Remove(CurrentFemaleLoaouts);
            SpawnSettings.isDirty = true;
            FemaleLoadoutLB.SelectedIndex = -1;
            if (FemaleLoadoutLB.Items.Count > 0)
                FemaleLoadoutLB.SelectedIndex = 0;
        }
        private void SpawnXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentSpawnPosition[0] = (float)SpawnXNUD.Value;
            SpawnSettings.isDirty = true;
            pictureBox4.Invalidate();
        }
        private void SpawnYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentSpawnPosition[1] = (float)SpawnYNUD.Value;
            SpawnSettings.isDirty = true;
            pictureBox4.Invalidate();
        }
        private void SpawnZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentSpawnPosition[2] = (float)SpawnZNUD.Value;
            SpawnSettings.isDirty = true;
            pictureBox4.Invalidate();
        }
        #endregion SpawnSettings

        #region TerritorySettings
        private void LoadTerritorySettings()
        {
            useraction = false;
            EnableTerritoriesTCB.Checked = TerritorySettings.EnableTerritories == 1 ? true : false;
            UseWholeMapForInviteListTCB.Checked = TerritorySettings.UseWholeMapForInviteList == 1 ? true : false;
            TerritorySizeTNUD.Value = (decimal)TerritorySettings.TerritorySize;
            TerritoryPerimeterSizeTNUD.Value = (decimal)TerritorySettings.TerritoryPerimeterSize;
            MaxMembersInTerritoryTNUD.Value = TerritorySettings.MaxMembersInTerritory;
            MaxTerritoryPerPlayerTNUD.Value = TerritorySettings.MaxTerritoryPerPlayer;
            TerritoryAuthenticationRadiusNUD.Value = TerritorySettings.TerritoryAuthenticationRadius;
            useraction = true;
        }
        private void TerritoriesTCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            TerritorySettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 3), cb.Checked == true ? 1 : 0);
            TerritorySettings.isDirty = true;
        }
        private void TerritoryTNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.DecimalPlaces > 0)
                TerritorySettings.SetdeciamlValue(nud.Name.Substring(0, nud.Name.Length - 4), nud.Value);
            else
                TerritorySettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 4), (int)nud.Value);
            TerritorySettings.isDirty = true;
        }
        #endregion Territory settings

        #region VehicleSettings
        public VConfigs CurrentCVehicleConfig;

        private void LoadvehicleSettings()
        {
            useraction = false;
            VehicleSyncComboBox.DataSource = Enum.GetValues(typeof(VehicleSync));
            VehicleRequireKeyToStartComboBox.DataSource = Enum.GetValues(typeof(VehicleRequireKeyToStart));
            MasterKeyPairingModeComboBox.DataSource = Enum.GetValues(typeof(MasterKeyPairingMode));
            PlacePlayerOnGroundOnReconnectInVehicleComboBox.DataSource = Enum.GetValues(typeof(PlacePlayerOnGroundOnReconnectInVehicle));


            VehicleSyncComboBox.SelectedItem = (VehicleSync)VehicleSettings.VehicleSync;
            VehicleRequireKeyToStartComboBox.SelectedItem = (VehicleRequireKeyToStart)VehicleSettings.VehicleRequireKeyToStart;
            VehicleRequireAllDoorsCB.Checked = VehicleSettings.VehicleRequireAllDoors == 1 ? true : false;
            VehicleLockedAllowInventoryAccessCB.Checked = VehicleSettings.VehicleLockedAllowInventoryAccess == 1 ? true : false;
            VehicleLockedAllowInventoryAccessWithoutDoorsCB.Checked = VehicleSettings.VehicleLockedAllowInventoryAccessWithoutDoors == 1 ? true : false;
            MasterKeyPairingModeComboBox.SelectedItem = (MasterKeyPairingMode)VehicleSettings.MasterKeyPairingMode;
            MasterKeyUsesNUD.Value = VehicleSettings.MasterKeyUses;
            CanPickLockCB.Checked = VehicleSettings.CanPickLock == 1 ? true : false;



            PickLockChancePercentNUD.Value = (decimal)VehicleSettings.PickLockChancePercent;
            PickLockTimeSecondsNUD.Value = (decimal)VehicleSettings.PickLockTimeSeconds;
            PickLockToolDamagePercentNUD.Value = (decimal)VehicleSettings.PickLockToolDamagePercent;
            EnableWindAerodynamicsCB.Checked = VehicleSettings.EnableWindAerodynamics == 1 ? true : false;
            EnableTailRotorDamageCB.Checked = VehicleSettings.EnableTailRotorDamage == 1 ? true : false;
            TowingCB.Checked = VehicleSettings.Towing == 1 ? true : false;
            EnableHelicopterExplosionsCB.Checked = VehicleSettings.EnableHelicopterExplosions == 1 ? true : false;
            DisableVehicleDamageCB.Checked = VehicleSettings.DisableVehicleDamage == 1 ? true : false;
            VehicleCrewDamageMultiplierNUD.Value = (decimal)VehicleSettings.VehicleCrewDamageMultiplier;
            VehicleSpeedDamageMultiplierNUD.Value = (decimal)VehicleSettings.VehicleSpeedDamageMultiplier;
            VehicleRoadKillDamageMultiplierNUD.Value = VehicleSettings.VehicleRoadKillDamageMultiplier;
            CollisionDamageIfEngineOffCB.Checked = VehicleSettings.CollisionDamageIfEngineOff == 1 ? true : false;
            CollisionDamageMinSpeedKmhNUD.Value = VehicleSettings.CollisionDamageMinSpeedKmh;
            CanChangeLockCB.Checked = VehicleSettings.CanChangeLock == 1 ? true : false;
            DesyncInvulnerabilityTimeoutSecondsNUD.Value = VehicleSettings.DesyncInvulnerabilityTimeoutSeconds;

            DamagedEngineStartupChancePercentNUD.Value = VehicleSettings.DamagedEngineStartupChancePercent;
            ChangeLockTimeSecondsNUD.Value = (decimal)VehicleSettings.ChangeLockTimeSeconds;
            ChangeLockToolDamagePercentNUD.Value = (decimal)VehicleSettings.ChangeLockToolDamagePercent;
            PlacePlayerOnGroundOnReconnectInVehicleComboBox.SelectedItem = (PlacePlayerOnGroundOnReconnectInVehicle)VehicleSettings.PlacePlayerOnGroundOnReconnectInVehicle;
            RevvingOverMaxRPMRuinsEngineInstantlyCB.Checked = VehicleSettings.RevvingOverMaxRPMRuinsEngineInstantly == 1 ? true : false;
            VehicleDropsRuinedDoorsCB.Checked = VehicleSettings.VehicleDropsRuinedDoors == 1 ? true : false;
            ExplodingVehicleDropsAttachmentsCB.Checked = VehicleSettings.ExplodingVehicleDropsAttachments == 1 ? true : false;
            EnableVehicleCoversCB.Checked = VehicleSettings.EnableVehicleCovers == 1 ? true : false;
            AllowCoveringDEVehiclesCB.Checked = VehicleSettings.AllowCoveringDEVehicles == 1 ? true : false;
            CanCoverWithCargoCB.Checked = VehicleSettings.CanCoverWithCargo == 1 ? true : false;
            UseVirtualStorageForCoverCargoCB.Checked = VehicleSettings.UseVirtualStorageForCoverCargo == 1 ? true : false;
            VehicleAutoCoverTimeSecondsNUD.Value = VehicleSettings.VehicleAutoCoverTimeSeconds;
            VehicleAutoCoverRequireCamonetCB.Checked = VehicleSettings.VehicleAutoCoverRequireCamonet == 1 ? true : false;
            EnableAutoCoveringDEVehiclesCB.Checked = VehicleSettings.EnableAutoCoveringDEVehicles == 1 ? true : false;
            VehicleHeliCoverIconNameTB.Text = VehicleSettings.CFToolsHeliCoverIconName;
            VehicleBoatCoverIconNameTB.Text = VehicleSettings.CFToolsBoatCoverIconName;
            VehicleCarCoverIconNameTB.Text = VehicleSettings.CFToolsCarCoverIconName;

            ChangeLockToolsLB.DisplayMember = "DisplayName";
            ChangeLockToolsLB.ValueMember = "Value";
            ChangeLockToolsLB.DataSource = VehicleSettings.ChangeLockTools;

            PickLockToolsLB.DisplayMember = "DisplayName";
            PickLockToolsLB.ValueMember = "Value";
            PickLockToolsLB.DataSource = VehicleSettings.PickLockTools;

            VehiclesConfigLB.DisplayMember = "DisplayName";
            VehiclesConfigLB.ValueMember = "Value";
            VehiclesConfigLB.DataSource = VehicleSettings.VehiclesConfig;
            useraction = false;


            useraction = true;
        }
        private void darkButton46_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    VConfigs newvcofig = new VConfigs();
                    newvcofig.ClassName = l;
                    newvcofig.CanPlayerAttach = 0;

                    if (!VehicleSettings.VehiclesConfig.Contains(newvcofig))
                    {
                        VehicleSettings.VehiclesConfig.Add(newvcofig);
                        VehicleSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton47_Click(object sender, EventArgs e)
        {
            VehicleSettings.VehiclesConfig.Remove(CurrentCVehicleConfig);
            VehicleSettings.isDirty = true;
        }
        private void darkButton43_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!VehicleSettings.PickLockTools.Contains(l))
                    {
                        VehicleSettings.PickLockTools.Add(l);
                        VehicleSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton42_Click(object sender, EventArgs e)
        {
            VehicleSettings.PickLockTools.Remove(PickLockToolsLB.GetItemText(PickLockToolsLB.SelectedItem));
            VehicleSettings.isDirty = true;
        }
        private void darkButton45_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!VehicleSettings.ChangeLockTools.Contains(l))
                    {
                        VehicleSettings.ChangeLockTools.Add(l);
                        VehicleSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton44_Click(object sender, EventArgs e)
        {
            VehicleSettings.ChangeLockTools.Remove(ChangeLockToolsLB.GetItemText(ChangeLockToolsLB.SelectedItem));
            VehicleSettings.isDirty = true;
        }
        private void VehiclesConfigLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VehiclesConfigLB.SelectedItems.Count < 1) return;
            CurrentCVehicleConfig = VehiclesConfigLB.SelectedItem as VConfigs;
            useraction = false;
            ClassNameTB.Text = CurrentCVehicleConfig.ClassName;
            CanPlayerAttachCB.Checked = CurrentCVehicleConfig.CanPlayerAttach == 1 ? true : false;
            LockComplexityNUD.Value = (decimal)CurrentCVehicleConfig.LockComplexity;
            useraction = true;
        }
        private void CanPlayerAttachCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (VehiclesConfigLB.SelectedItems.Count > 1)
            {
                foreach (var item in VehiclesConfigLB.SelectedItems)
                {
                    VConfigs pitem = item as VConfigs;
                    pitem.CanPlayerAttach = CanPlayerAttachCB.Checked == true ? 1 : 0;
                }
                VehicleSettings.isDirty = true;
            }
        }
        private void VehicleSettingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.DecimalPlaces > 0)
                VehicleSettings.SetFloatValue(nud.Name.Substring(0, nud.Name.Length - 3), (decimal)nud.Value);
            else
                VehicleSettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            VehicleSettings.isDirty = true;
        }
        private void VehicleSettingsCB_CheckChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CheckBox cb = sender as CheckBox;
            VehicleSettings.SetIntValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            VehicleSettings.isDirty = true;
        }
        private void VehicleSyncComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSync cacl = (VehicleSync)VehicleSyncComboBox.SelectedItem;
            VehicleSettings.VehicleSync = (int)cacl;
            VehicleSettings.isDirty = true;
        }
        private void PlacePlayerOnGroundOnReconnectInVehicleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            PlacePlayerOnGroundOnReconnectInVehicle cacl = (PlacePlayerOnGroundOnReconnectInVehicle)PlacePlayerOnGroundOnReconnectInVehicleComboBox.SelectedItem;
            VehicleSettings.PlacePlayerOnGroundOnReconnectInVehicle = (int)cacl;
            VehicleSettings.isDirty = true;
        }
        private void VehicleRequireKeyToStartComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleRequireKeyToStart cacl = (VehicleRequireKeyToStart)VehicleRequireKeyToStartComboBox.SelectedItem;
            VehicleSettings.VehicleRequireKeyToStart = (int)cacl;
            VehicleSettings.isDirty = true;
        }
        private void MasterKeyPairingModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MasterKeyPairingMode cacl = (MasterKeyPairingMode)MasterKeyPairingModeComboBox.SelectedItem;
            VehicleSettings.MasterKeyPairingMode = (int)cacl;
            VehicleSettings.isDirty = true;
        }
        private void LockComplexityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (VehiclesConfigLB.SelectedItems.Count > 0)
            {
                foreach (var item in VehiclesConfigLB.SelectedItems)
                {
                    VConfigs pitem = item as VConfigs;
                    pitem.LockComplexity = LockComplexityNUD.Value;
                }
                VehicleSettings.isDirty = true;
            }
        }
        private void DesyncInvulnerabilityTimeoutSecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.DesyncInvulnerabilityTimeoutSeconds = DesyncInvulnerabilityTimeoutSecondsNUD.Value;
            VehicleSettings.isDirty = true;
        }
        private void VehicleRoadKillDamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.VehicleRoadKillDamageMultiplier = VehicleRoadKillDamageMultiplierNUD.Value;
            VehicleSettings.isDirty = true;
        }
        private void DamagedEngineStartupChancePercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.DamagedEngineStartupChancePercent = DamagedEngineStartupChancePercentNUD.Value;
            VehicleSettings.isDirty = true;
        }
        private void EnableVehicleCoversCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.EnableVehicleCovers = EnableVehicleCoversCB.Checked == true ? 1 : 0;
            VehicleSettings.isDirty = true;
        }
        private void AllowCoveringDEVehiclesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.AllowCoveringDEVehicles = AllowCoveringDEVehiclesCB.Checked == true ? 1 : 0;
            VehicleSettings.isDirty = true;
        }
        private void UseVirtualStorageForCoverCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.UseVirtualStorageForCoverCargo = UseVirtualStorageForCoverCargoCB.Checked == true ? 1 : 0;
            VehicleSettings.isDirty = true;
        }
        private void VehicleAutoCoverTimeSecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.VehicleAutoCoverTimeSeconds = VehicleAutoCoverTimeSecondsNUD.Value;
            VehicleSettings.isDirty = true;
        }
        private void VehicleAutoCoverRequireCamonetCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.VehicleAutoCoverRequireCamonet = VehicleAutoCoverRequireCamonetCB.Checked == true? 1: 0;
            VehicleSettings.isDirty = true;
        }
        private void VehicleHeliCoverIconNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.CFToolsHeliCoverIconName = VehicleCarCoverIconNameTB.Text;
            VehicleSettings.isDirty = true;
        }
        private void VehicleBoatCoverIconNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.CFToolsBoatCoverIconName = VehicleBoatCoverIconNameTB.Text;
            VehicleSettings.isDirty = true;
        }
        private void VehicleCarCoverIconNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.CFToolsCarCoverIconName = VehicleCarCoverIconNameTB.Text;
            VehicleSettings.isDirty = true;
        }
        private void CanCoverWithCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.CanCoverWithCargo = CanCoverWithCargoCB.Checked == true ? 1 : 0; 
            VehicleSettings.isDirty = true;
        }
        private void EnableAutoCoveringDEVehiclesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            VehicleSettings.EnableAutoCoveringDEVehicles = EnableAutoCoveringDEVehiclesCB.Checked == true ? 1 : 0;
            VehicleSettings.isDirty = true;
        }
        #endregion VehicleSettings

        #region Garegesettings
        private void loadgaragesettings()
        {
            useraction = false;
            GarageModeCB.DataSource = Enum.GetValues(typeof(GarageMode));
            GarageStoreModeCB.DataSource = Enum.GetValues(typeof(GarageStoreMode));
            GarageRetrieveModeCB.DataSource = Enum.GetValues(typeof(GarageRetrieveMode));
            GarageGroupStoreModeCB.DataSource = Enum.GetValues(typeof(GroupStoreMode));

            GarageEnabledCB.Checked = GarageSettings.Enabled == 1 ? true:false;
            AllowStoringDEVehiclesCB.Checked = GarageSettings.AllowStoringDEVehicles == 1 ? true : false;
            GarageModeCB.SelectedItem = (GarageMode)GarageSettings.GarageMode;
            GarageStoreModeCB.SelectedItem = (GarageStoreMode)GarageSettings.GarageStoreMode;
            GarageRetrieveModeCB.SelectedItem = (GarageRetrieveMode)GarageSettings.GarageRetrieveMode;
            MaxStorableVehiclesNUD.Value = GarageSettings.MaxStorableVehicles;
            GarageVehicleSearchRadiusNUD.Value = GarageSettings.VehicleSearchRadius;
            GarageMaxDistanceFromStoredPositionNUD.Value = GarageSettings.MaxDistanceFromStoredPosition;
            GarageCanStoreWithCargoCB.Checked = GarageSettings.CanStoreWithCargo == 1 ? true : false;
            GarageUseVirtualStorageForCargoCB.Checked = GarageSettings.UseVirtualStorageForCargo == 1 ? true : false;
            GarageNeedKeyToStoreCB.Checked = GarageSettings.NeedKeyToStore == 1 ? true : false;
            GarageEnableGroupFeaturesCB.Checked = GarageSettings.EnableGroupFeatures == 1 ? true : false;
            GarageGroupStoreModeCB.SelectedItem = (GroupStoreMode)GarageSettings.GroupStoreMode;
            GarageEnableMarketFeaturesCB.Checked = GarageSettings.EnableMarketFeatures == 1 ? true : false;
            GarageStorePricePercentNUD.Value = GarageSettings.StorePricePercent;
            GarageStaticStorePriceNUD.Value = GarageSettings.StaticStorePrice;
            GarageMaxStorableTier1NUD.Value = GarageSettings.MaxStorableTier1;
            GarageMaxStorableTier2NUD.Value = GarageSettings.MaxStorableTier2;
            GarageMaxStorableTier3NUD.Value = GarageSettings.MaxStorableTier3;
            GarageMaxRangeTier1NUD.Value = GarageSettings.MaxRangeTier1;
            GarageMaxRangeTier2NUD.Value = GarageSettings.MaxRangeTier2;
            GarageMaxRangeTier3NUD.Value = GarageSettings.MaxRangeTier3;
            GarageParkingMeterEnableFlavorCB.Checked = GarageSettings.ParkingMeterEnableFlavor == 1 ? true : false;

            GarageEntityWhitelistLB.DisplayMember = "DisplayName";
            GarageEntityWhitelistLB.ValueMember = "Value";
            GarageEntityWhitelistLB.DataSource = GarageSettings.EntityWhitelist;
            useraction = true;
        }
        private void GarageEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.Enabled = GarageEnabledCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void GarageModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageMode cacl = (GarageMode)GarageModeCB.SelectedItem;
            GarageSettings.GarageMode = (int)cacl;
            GarageSettings.isDirty = true;
        }
        private void GarageStoreModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageStoreMode cacl = (GarageStoreMode)GarageStoreModeCB.SelectedItem;
            GarageSettings.GarageStoreMode = (int)cacl;
            GarageSettings.isDirty = true;
        }
        private void GarageRetrieveModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageRetrieveMode cacl = (GarageRetrieveMode)GarageRetrieveModeCB.SelectedItem;
            GarageSettings.GarageRetrieveMode = (int)cacl;
            GarageSettings.isDirty = true;
        }
        private void MaxStorableVehiclesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxStorableVehicles = (int)MaxStorableVehiclesNUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageVehicleSearchRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.VehicleSearchRadius = GarageVehicleSearchRadiusNUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxDistanceFromStoredPositionNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxDistanceFromStoredPosition = GarageMaxDistanceFromStoredPositionNUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageCanStoreWithCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.CanStoreWithCargo = GarageCanStoreWithCargoCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void GarageUseVirtualStorageForCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.UseVirtualStorageForCargo = GarageUseVirtualStorageForCargoCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;

        }
        private void GarageNeedKeyToStoreCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.NeedKeyToStore = GarageNeedKeyToStoreCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void darkButton74_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    GarageSettings.EntityWhitelist.Add(l);
                    GarageSettings.isDirty = true;
                }
            }
        }
        private void darkButton73_Click(object sender, EventArgs e)
        {
            GarageSettings.EntityWhitelist.Remove(GarageEntityWhitelistLB.GetItemText(GarageEntityWhitelistLB.SelectedItem));
            GarageSettings.isDirty = true;
        }
        private void GarageEnableGroupFeaturesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.EnableMarketFeatures = GarageEnableMarketFeaturesCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void GarageGroupStoreModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GroupStoreMode cacl = (GroupStoreMode)GarageGroupStoreModeCB.SelectedItem;
            GarageSettings.GroupStoreMode = (int)cacl;
            GarageSettings.isDirty = true;
        }
        private void GarageEnableMarketFeaturesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.EnableMarketFeatures = GarageEnableMarketFeaturesCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void GarageStorePricePercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.StorePricePercent = GarageStorePricePercentNUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageStaticStorePriceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.StaticStorePrice = (int)GarageStaticStorePriceNUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxStorableTier1NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxStorableTier1 = (int)GarageMaxStorableTier1NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxStorableTier2NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxStorableTier2 = (int)GarageMaxStorableTier2NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxStorableTier3NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxStorableTier3 = (int)GarageMaxStorableTier3NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxRangeTier1NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxRangeTier1 = GarageMaxRangeTier1NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxRangeTier2NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxRangeTier2 = GarageMaxRangeTier2NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void GarageMaxRangeTier3NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.MaxRangeTier3 = GarageMaxRangeTier3NUD.Value;
            GarageSettings.isDirty = true;
        }
        private void AllowStoringDEVehiclesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.AllowStoringDEVehicles = AllowStoringDEVehiclesCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        private void GarageParkingMeterEnableFlavorCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            GarageSettings.ParkingMeterEnableFlavor = GarageParkingMeterEnableFlavorCB.Checked == true ? 1 : 0;
            GarageSettings.isDirty = true;
        }
        #endregion Garagesettings

        #region pertsonalstorage
        private void loadPersonalStorageSettings()
        {
            SetupFactionsDropDownBoxes();
            listBox31.DisplayMember = "DisplayName";
            listBox31.ValueMember = "Value";
            listBox31.DataSource = PersonalStorageList.personalstorageList;
        }
        private void SetupFactionsDropDownBoxes()
        {
            useraction = false;

            List<string> questrequiredfaction = new List<string>();
            questrequiredfaction.Add("");
            foreach (string rf in Factions)
            {
                questrequiredfaction.Add(rf);
            }
            FactionCB.DataSource = new BindingList<string>(questrequiredfaction);

            useraction = true;
        }
        private void listBox31_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox31.SelectedItems.Count < 1) return;
            CurrentPersonalStorage = listBox31.SelectedItem as PersonalStorage;
            useraction = false;
            ConfigVersionlabel.Text = CurrentPersonalStorage.ConfigVersion.ToString();
            StorageIDNUD.Value = CurrentPersonalStorage.StorageID;
            PSClassNameTB.Text = CurrentPersonalStorage.ClassName;
            PSDisplayNameTB.Text = CurrentPersonalStorage.DisplayName;
            PSDiplayIconTB.Text = CurrentPersonalStorage.DisplayIcon;
            PSpositionXNUD.Value = CurrentPersonalStorage.Position[0];
            PSpositionYNUD.Value = CurrentPersonalStorage.Position[1];
            PSpositionZNUD.Value = CurrentPersonalStorage.Position[2];
            PSorientaionXNUD.Value = CurrentPersonalStorage.Orientation[0];
            PSorientaionYNUD.Value = CurrentPersonalStorage.Orientation[1];
            PSorientaionZNUD.Value = CurrentPersonalStorage.Orientation[2];
            QuestIDNUD.Value = CurrentPersonalStorage.QuestID;
            ReputationNUD.Value = CurrentPersonalStorage.Reputation;
            FactionCB.SelectedIndex = FactionCB.FindStringExact(CurrentPersonalStorage.Faction);
            IsGlobalStorageCB.Checked = CurrentPersonalStorage.IsGlobalStorage == 1 ? true : false;
            useraction = true;
        }
        private void darkButton98_Click(object sender, EventArgs e)
        {
            int Newid = PersonalStorageList.GetNextID();
            PersonalStorageList.AddNewStorage(Newid);
        }
        private void darkButton99_Click(object sender, EventArgs e)
        {
            if (CurrentPersonalStorage == null) return;
            if (MessageBox.Show("This Will Remove The All reference to this Personal storage, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                PersonalStorageList.RemovePS(CurrentPersonalStorage);
                if (listBox31.Items.Count == 0)
                    listBox31.SelectedIndex = -1;
                else
                    listBox31.SelectedIndex = 0;
            }

        }
        private void PSClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.ClassName = PSClassNameTB.Text;
            listBox31.Invalidate();
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSDisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.DisplayName = PSDisplayNameTB.Text;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSDiplayIconTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.DisplayIcon = PSDiplayIconTB.Text;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSpositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Position[0] = PSpositionXNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSpositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Position[1] = PSpositionYNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSpositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Position[2] = PSpositionZNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSorientaionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Orientation[0] = PSorientaionXNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSorientaionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Orientation[1] = PSorientaionYNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void PSorientaionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Orientation[2] = PSorientaionZNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void QuestIDNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.QuestID = (int)QuestIDNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void ReputationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Reputation = (int)ReputationNUD.Value;
            CurrentPersonalStorage.isDirty = true;
        }
        private void FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.Faction = FactionCB.GetItemText(FactionCB.SelectedItem);
            CurrentPersonalStorage.isDirty = true;
        }
        private void IsGlobalStorageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPersonalStorage.IsGlobalStorage = IsGlobalStorageCB.Checked == true ? 1 : 0;
            CurrentPersonalStorage.isDirty = true;
        }
        #endregion personalstroage


    }
    public class NullToEmptyGearConverter : JsonConverter<Gear>
    {
        // Override default null handling
        public override bool HandleNull => true;
        // Check the type
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Gear);
        }

        public override Gear Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        // 
        public override void Write(Utf8JsonWriter writer, Gear value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ClassName");
                writer.WriteStringValue(value.ClassName);
                writer.WritePropertyName("Quantity");
                writer.WriteNumberValue(value.Quantity);
                writer.WritePropertyName("Attachments");
                writer.WriteStartArray();
                foreach (string s in value.Attachments)
                    writer.WriteStringValue(s);
                writer.WriteEndArray();
                writer.WriteEndObject();

            }

        }
    }
}
