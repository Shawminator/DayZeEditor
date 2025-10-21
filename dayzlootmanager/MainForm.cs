﻿using DarkUI.Forms;
using DayZeLib;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class MainForm : DarkForm
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public string changeToolstrip
        {
            get { return toolStripStatusLabel1.Text; }
            set { toolStripStatusLabel1.Text = value; }
        }

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public string VersionNumber = "0.8.4.5";
        private static bool hidden;
        public static String ProjectsJson = Application.StartupPath + "\\Project\\Projects.json";
        public ProjectList Projects;

        public SplashForm frm;
        public event EventHandler LoadCompleted;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnLoadCompleted(EventArgs.Empty);
            if ((Projects.ActiveProject != null && Projects.ActiveProject != "") && Projects.getActiveProject().haswarnings)
            {
                MessageBox.Show("Warnings detected. please see console.....");
            }
        }

        private void CheckChangeLog()
        {
            if (Projects.ShowChangeLog)
            {
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
                string file = File.ReadAllText("Update.clog");
                ChangeLog cl = new ChangeLog();
                cl.Changelog = file;
                DialogResult result = cl.ShowDialog();
                Projects.ShowChangeLog = false;
                Projects.SaveProject(false, false);
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Save();
            }
        }

        protected virtual void OnLoadCompleted(EventArgs e)
        {
            LoadCompleted?.Invoke(this, e);
            if (Properties.Settings.Default.FormSize.Width == 0 || Properties.Settings.Default.FormSize.Height == 0)
            {
                // first start
                // optional: add default values
            }
            else
            {
                this.WindowState = Properties.Settings.Default.FormState;

                // we don't want a minimized window at startup
                if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;

                this.Location = Properties.Settings.Default.FormLocation;
                this.Size = Properties.Settings.Default.FormSize;
            }
            Form_Controls.Setfromsize();
            checkBox1.Checked = Properties.Settings.Default.ShowConsole;
        }
        public MainForm(SplashForm form)
        {
            frm = form;
            
            InitializeComponent();
            Form_Controls.InitializeForm_Controls
            (
                this,
                panel1,
                panel2,
                TitleLabel,
                label1,
                CloseButton,
                MinimiseButton

            );
            SlidePanel.Width = 30;
            hidden = true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if(strExeFilePath.ToLower().Contains("desktop"))
            {
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
                MessageBox.Show("It is best that the application is run from somewhere else outhwith the Desktop.\nit can cause some funny issues with file paths especialy if one drive is active.\nPlease move it elsewhere\nThanks");
                Application.Exit();
                return;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Current Version : " + VersionNumber);
            var culture = CultureInfo.GetCultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            TitleLabel.Text = "DayZeEditor " + VersionNumber + " by Shawminator ";
            if (CheckForUpdate())
            {
                Application.Exit();
                return;
            }


            if (File.Exists(ProjectsJson))
            {
                Projects = (JsonSerializer.Deserialize<ProjectList>(File.ReadAllText(ProjectsJson)));
                if (Projects.getActiveProject() != null)
                {
                    Projects.CheckAllPaths();
                    if (Projects.getActiveProject().ProfilePath == null)
                    {
                        Projects.getActiveProject().ProfilePath = "profile";
                        Projects.SaveProject();
                    }
                    Console.WriteLine(Projects.ActiveProject + " is the Current Active Project");
                    if(!Projects.getActiveProject().checkMapExists())
                    {
                        Projects.SetActiveProject();
                        Projects.SaveProject(false, false);
                        ProjectPanel _TM = Application.OpenForms["ProjectPanel"] as ProjectPanel;
                        _TM = new ProjectPanel
                        {
                            MdiParent = this,
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                            Location = new System.Drawing.Point(30, 0),
                            Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                            projects = Projects
                        };
                        _TM.Show();
                        Console.WriteLine("loading Project manager....");
                        CheckChangeLog();
                        MessageBox.Show("Map File not found for the current active project\nPlease download the appropiate map addon from the Map Addons tab");
                        return;
                    }
                    Console.WriteLine("Will now serialize base economy files from Project " + Projects.ActiveProject);
                    if(!Directory.Exists(Projects.getActiveProject().projectFullName))
                    {
                        Console.WriteLine(Projects.ActiveProject + " Cannot be found......\nRemoving from active project");
                       // MessageBox.Show(Projects.ActiveProject + " Cannot be found......\nRemoving from active project");
                        Project p = Projects.getActiveProject();
                        Projects.Projects.Remove(p);
                        Projects.ActiveProject = null;
                        Projects.SaveProject(false, false);
                        TypeManButton.Visible = false;
                        TraderManButton.Visible = false;
                        ExpansionSettingsButton.Visible = false;
                        MarketButton.Visible = false;
                        TraderPlusButton.Visible = false;
                        Console.WriteLine("No Active Project Found, Please Load a Project from the Projects panel.....");
                        toolStripStatusLabel1.Text = "No Active Project Found, Please Load/Create a Project from the Projects panel.....";
                        ProjectPanel _TM = Application.OpenForms["ProjectPanel"] as ProjectPanel;
                        _TM = new ProjectPanel
                        {
                            MdiParent = this,
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                            Location = new System.Drawing.Point(30, 0),
                            Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                            projects = Projects
                        };
                        _TM.Show();
                        Console.WriteLine("loading Project manager....");
                        return;
                    }
                    Projects.getActiveProject().seteconomycore();
                    Projects.getActiveProject().seteconomydefinitions();
                    Projects.getActiveProject().setuserdefinitions();
                    Projects.getActiveProject().setVanillaTypes();
                    Projects.getActiveProject().SetModListtypes();
                    Projects.getActiveProject().SetTotNomCount();
                    Projects.getActiveProject().AssignRarity();
                    Projects.getActiveProject().SetSpawnabletypes();
                    Projects.getActiveProject().setplayerspawns();
                    Projects.getActiveProject().SetCFGGameplayConfig();
                    Projects.getActiveProject().SetCFGUndergroundTriggerConfig();
                    Projects.getActiveProject().SetcfgEffectAreaConfig();
                    Projects.getActiveProject().SetEvents();
                    Projects.getActiveProject().seteventspawns();
                    Projects.getActiveProject().seteventgroups();
                    Projects.getActiveProject().SetRandompresets();
                    Projects.getActiveProject().SetGlobals();
                    Projects.getActiveProject().SetWeather();
                    Projects.getActiveProject().SetIgnoreList();
                    Projects.getActiveProject().Setmapgrouproto();
                    Projects.getActiveProject().Setmapgroupos();
                    Projects.getActiveProject().SetCfgEnviroment();
                    Projects.getActiveProject().SetTerritories();
                    Console.WriteLine("Project is Running Dr Jones Trader...." + Projects.getActiveProject().usingDrJoneTrader.ToString());
                    Console.WriteLine("Project is Running Expansion Market...." + Projects.getActiveProject().usingexpansionMarket.ToString());
                    Console.WriteLine("Project is Running Trader Plus...." + Projects.getActiveProject().usingtraderplus.ToString());
                    Console.WriteLine("The Current Active Project is " + Projects.ActiveProject);
                    Console.WriteLine("Please click the select section to get the pop out menu");
                    toolStripStatusLabel1.Text = Projects.ActiveProject + " is the Current Active Project";
                }
                else
                {
                    TypeManButton.Visible = false;
                    TraderManButton.Visible = false;
                    ExpansionSettingsButton.Visible = false;
                    MarketButton.Visible = false;
                    TraderPlusButton.Visible = false;
                    Console.WriteLine("No Active Project Found, Please Load a Project from the Projects panel.....");
                    toolStripStatusLabel1.Text = "No Active Project Found, Please Load/Create a Project from the Projects panel.....";
                    ProjectPanel _TM = Application.OpenForms["ProjectPanel"] as ProjectPanel;
                    _TM = new ProjectPanel
                    {
                        MdiParent = this,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                        Location = new System.Drawing.Point(30, 0),
                        Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                        projects = Projects
                    };
                    _TM.Show();
                    Console.WriteLine("loading Project manager....");
                }
            }
            else
            {
                Projects = new ProjectList { };
                Projects.SaveProject(true);
                TypeManButton.Visible = false;
                TraderManButton.Visible = false;
                ExpansionSettingsButton.Visible = false;
                MarketButton.Visible = false;
                Console.WriteLine("No Projects Found, Please Create a new Project from the Projects panel.....");
                toolStripStatusLabel1.Text = "No Projects Found, Please Create a new Project from the Projects panel.....";
                ProjectPanel _TM = Application.OpenForms["ProjectPanel"] as ProjectPanel;
                _TM = new ProjectPanel
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    projects = Projects
                };
                _TM.Show();
                Console.WriteLine("loading Project manager....");
            }
            CheckChangeLog();
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }
        private bool CheckForUpdate()
        {
            Console.WriteLine("Checking GitHub For Newest Release.....");
            GitHub info = Build();
            if (info == null)
            {
                Console.WriteLine("No Internet connectivity, offline mode.....");
                return false;
            }
            Console.WriteLine("Latest release : " + info.name);
            var version1 = new Version(info.name);
            var version2 = new Version(VersionNumber);

            var result = version1.CompareTo(version2);
            if (result > 0)
            {
                Console.WriteLine("Update Found.....");
                string zipfile = Application.StartupPath + "\\" + Path.GetFileName(info.assets[0].browser_download_url);
                using (var client = new WebClient())
                {
                    Console.WriteLine("Downloading Zip file......");
                    client.DownloadFile(info.assets[0].browser_download_url, zipfile);
                }
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
                MessageBox.Show("Update Downloaded, Press OK to Extract and update");
                if (File.Exists(ProjectsJson))
                {
                    Projects = (JsonSerializer.Deserialize<ProjectList>(File.ReadAllText(ProjectsJson)));
                    Projects.ShowChangeLog = true;
                    Projects.SaveProject(false, false);
                }
                string updatepath = Application.StartupPath + "\\Updater.exe";
                System.Diagnostics.Process.Start(updatepath, AddQuotesIfRequired(zipfile));
                return true;
            }
            else if (result == 0)
            {
                Console.WriteLine("Application Upto Date....");
                return false;
            }
            return false;
        }

        public GitHub Build()
        {
            var getData = GetGithubData();
            if (getData.StartsWith("Offline"))
            {
                return null;
            }
            return JsonSerializer.Deserialize<GitHub>(getData);
        }
        private string GetGithubData()
        {
            var url = "https://api.github.com/repos/Shawminator/DayZeEditor/releases/latest";

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = "TestApp";

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return "Offline";
            }
        }
        private void closemdichildren()
        {
            if (MdiChildren.Length >= 1)
            {
                MdiChildren[0].Close();
            }
        }
        private void Slide_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox pb = sender as PictureBox;
                if (pb.Name == "HidePBox")
                {
                    ToolStrip1.Visible = false;
                    if (!hidden)
                        timer1.Start();
                }
                else if (pb.Name == "Slidelabel")
                {
                    ShowButtons();
                    timer1.Start();
                }
            }
            else if (sender is Panel)
            {
                Panel p = sender as Panel;
                if (p.Name == "SlidePanel")
                {
                    ShowButtons();
                    timer1.Start();
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HideConsole(checkBox1.Checked);
        }

        private void HideConsole(bool Hide)
        {
            IntPtr handle = GetConsoleWindow();
            if (Hide)
                ShowWindow(handle, SW_SHOW);
            else
            {
                ShowWindow(handle, SW_HIDE);
            }
        }

        private void DiscordButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/5EHE49Kjsv");
            timer1.Start();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.me/ADecadeOfdecay");
            timer1.Start();
        }
        private void ShowButtons()
        {
            ToolStrip1.Visible = true;
            if (Projects.getActiveProject() != null)
            {
                TypeManButton.Visible = true;
                if (Projects.getActiveProject().usingDrJoneTrader)
                    TraderManButton.Visible = true;
                else
                    TraderManButton.Visible = false;

                if (Projects.getActiveProject().usingexpansionMarket)
                {
                    P2PButton.Visible = true;
                    MarketButton.Visible = true;
                }
                else
                {
                    P2PButton.Visible = false;
                    MarketButton.Visible = false;
                }

                if (Projects.getActiveProject().usingtraderplus)
                    TraderPlusButton.Visible = true;
                else
                    TraderPlusButton.Visible = false;

                if (Projects.getActiveProject().isUsingExpansion())
                    ExpansionSettingsButton.Visible = true;
                else
                    ExpansionSettingsButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\CJ_LootChests\\LootChests_V106.json"))
                    LootchestButton.Visible = true;
                else
                    LootchestButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Capare\\CapareLootPool\\CapareLootPoolConfig.json"))
                    LootPoolManagerButton.Visible = true;
                else
                    LootPoolManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Capare\\CapareLootBox\\CapareLootBoxConfig.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Capare\\CapareLootPool\\CapareLootPoolConfig.json"))
                    RHLootBoxManagerButton.Visible = true;
                else
                    RHLootBoxManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Capare\\CapareHCConfig\\CapareHCConfig.json"))
                    HelicrashManagerButton.Visible = true;
                else
                    HelicrashManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\BaseBuildingPlus\\BBP_Settings.json"))
                    BBPManagerButton.Visible = true;
                else
                    BBPManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\RaG_BaseBuilding\\RaG_BaseBuilding.json"))
                    RAGTysonBBManagerButton.Visible = true;
                else
                    RAGTysonBBManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\SpawnerBubaku\\SpawnerBubaku.json"))
                    SpawnerBukakuManagerButton.Visible = true;
                else
                    SpawnerBukakuManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Airdrop\\AirdropSettings.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Airdrop\\AirdropSafezones.json"))
                    AirdropUpgradedManagerButton.Visible = true;
                else
                    AirdropUpgradedManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\AbandonedVehicleRemover\\Settings.json"))
                    AbandonedVehicleRemoverManagerButton.Visible = true;
                else
                    AbandonedVehicleRemoverManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\BreachingCharge\\breachingcharge.json"))
                    BreachingChargeManagerButton.Visible = true;
                else
                    BreachingChargeManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\KOTH\\" + Projects.getActiveProject().mpmissionpath.Split('.').Last() + ".json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\KOTH\\Loot.json"))
                    KOTHManagerButton.Visible = true;
                else
                    KOTHManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\KosZone\\KZConfig\\KosZoneConfig.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\KosZone\\KZConfig\\PurgeConfigV1.json"))
                    KOSzoneManagerButton.Visible = true;
                else
                    KOSzoneManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionMod\\Settings\\AISettings.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\mpmissions\\" + Projects.getActiveProject().mpmissionpath + "\\expansion\\settings\\AIPatrolSettings.json"))
                    ExpansionAIButton.Visible = true;
                else
                    ExpansionAIButton.Visible = false;

                if (Directory.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionMod\\Loadouts"))
                    ExpansionLoadoutManagerButton.Visible = true;
                else
                    ExpansionLoadoutManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionMod\\Settings\\QuestSettings.json") &&
                    Directory.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionMod\\Quests"))
                    ExpansionQuestsButton.Visible = true;
                else
                    ExpansionQuestsButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionCircleMarker\\Config\\admins.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionCircleMarker\\Config\\dynamicpvpzone.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionCircleMarker\\Config\\ExpansionCircleMarker.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionCircleMarker\\Config\\itemrules.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\ExpansionCircleMarker\\Config\\polygonzones.json"))
                    ExpansionCircleMarkerButton.Visible = true;
                else
                    ExpansionCircleMarkerButton.Visible = false;

                if ((File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Characteristics.xml") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Globals.xml"))||
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\PvZmoD_Information_Panel\\PvZmoD_Information_Panel.xml"))
                    PVZCZManagerButton.Visible = true;
                else
                    PVZCZManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\BP_WorkBench.json"))
                    AdvancedWorkbenchButton.Visible = true;
                else
                    AdvancedWorkbenchButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Workbench_Redux\\Workbench_Redux.json"))
                    WorkBenchReduxButton.Visible = true;
                else
                    WorkBenchReduxButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Capare_Workbench\\CapareWorkBenchConfig.json"))
                    CapareWorkBenchManagerButton.Visible = true;
                else
                    CapareWorkBenchManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\CapareTreasure\\CapareTreasure.json"))
                    CapareTreasureManagerButton.Visible = true;
                else
                    CapareTreasureManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\MagicCrateManagerSettings.json"))
                    MysteryBoxButton.Visible = true;
                else
                    MysteryBoxButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\MPG_Spawner\\Config.json"))
                    MPGSpawnerButton.Visible = true;
                else
                    MPGSpawnerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Utopia_Airdrop\\Config\\UtopiaAirdropSettings.json"))
                    UtopiaAirdropButton.Visible = true;
                else
                    UtopiaAirdropButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\SearchForLoot\\SearchForLoot.json"))
                    SearchForLootManagerButton.Visible = true;
                else
                    SearchForLootManagerButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\MB_TimedCrate\\CrateSettings.json") &&
                    File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\MB_TimedCrate\\CustomLootData.json"))
                    TimedCrateManagerButton.Visible = true;
                else
                    TimedCrateManagerButton.Visible = false;

                if (KillrewardStatics.Checkallfiles(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath))
                    KillRewardManagerButton.Visible = true;
                else
                    KillRewardManagerButton.Visible = false;
                
                if (File.Exists(Projects.getActiveProject().projectFullName + "\\mpmissions\\" + Projects.getActiveProject().mpmissionpath + "\\weather.json"))
                    DynamicWeatherManagerButton.Visible = true;
                else
                    DynamicWeatherManagerButton.Visible = false;

                if (Directory.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\TerjeSettings"))
                    TerjeManagerButton.Visible = true;
                else
                    TerjeManagerButton.Visible = false;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                SlidePanel.Width = SlidePanel.Width + 10;
                if (SlidePanel.Width == 180)
                {
                    timer1.Stop();
                    hidden = false;
                    Refresh();
                }
            }
            else
            {
                SlidePanel.Width = SlidePanel.Width - 10;
                if (SlidePanel.Width == 30)
                {
                    timer1.Stop();
                    hidden = true;
                    Refresh();
                }
            }
        }
        private void ProjectsButton_Click(object sender, EventArgs e)
        {
            ProjectPanel _TM = Application.OpenForms["ProjectPanel"] as ProjectPanel;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ProjectPanel
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    projects = Projects
                };
                _TM.Show();
                Console.WriteLine("loading Project manager....");
                label1.Text = "Project Manager";
            }
            timer1.Start();
        }
        private void EconomyManagerButton_Click(object sender, EventArgs e)
        {
            Economy_Manager _TM = Application.OpenForms["Types_Manager"] as Economy_Manager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new Economy_Manager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Economy manager....");
                label1.Text = "Economy Manager";
            }
            timer1.Start();
        }
        private void TraderManButton_Click(object sender, EventArgs e)
        {
            DRJonesTrader_Manager _TM = Application.OpenForms["Trader_Manager"] as DRJonesTrader_Manager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new DRJonesTrader_Manager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Dr Jones Trader manager....");
                label1.Text = "DR Jones Trader Manager";
            }
            timer1.Start();
        }
        private void TraderPlusButton_Click(object sender, EventArgs e)
        {
            TraderPlus _TM = Application.OpenForms["TraderPlus"] as TraderPlus;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new TraderPlus
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading TraderPlus manager....");
                label1.Text = "Trader Plus Manager";
            }
            timer1.Start();
        }
        private void ExpansionSettingsButton_Click(object sender, EventArgs e)
        {
            ExpansionSettings _TM = Application.OpenForms["ExpansionSettings"] as ExpansionSettings;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionSettings
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Expansion Settings manager....");
                label1.Text = "Expansion Settings Manager";
            }
            timer1.Start();
        }
        private void MarketButton_Click(object sender, EventArgs e)
        {
            ExpansionMarket _TM = Application.OpenForms["ExpansionMarket"] as ExpansionMarket;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionMarket
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Expansion Market manager....");
                label1.Text = "Expansion Market manager";
            }
            timer1.Start();
        }
        private void P2PButton_Click(object sender, EventArgs e)
        {
            ExpansionP2pMarket _TM = Application.OpenForms["ExpansionP2pMarket"] as ExpansionP2pMarket;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionP2pMarket
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Expansion Market manager....");
                label1.Text = "Expansion P2P Market manager";
            }
            timer1.Start();
        }
        private void ExpansionAIButton_Click(object sender, EventArgs e)
        {
            ExpansionAI _TM = Application.OpenForms["ExpansionAI"] as ExpansionAI;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionAI
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading expansion AI manager....");
                label1.Text = "Expansion AI manager";
            }
            timer1.Start();
        }
        private void ExpansionLoadoutManagerButton_Click(object sender, EventArgs e)
        {
            ExpansionLoadoutsManager _TM = Application.OpenForms["ExpansionLoadoutsManager"] as ExpansionLoadoutsManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionLoadoutsManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading expansion Loadout manager....");
                label1.Text = "Expansion Loadouts manager";
            }
            timer1.Start();
        }
        private void ExpansionQuestsButton_Click(object sender, EventArgs e)
        {
            ExpansionQuests _TM = Application.OpenForms["ExpansionQuests"] as ExpansionQuests;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionQuests
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Expasion Quest manager....");
                label1.Text = "Expansion Quests manager";
            }
            timer1.Start();

        }
        private void ExpansionCircleMarkerButton_Click(object sender, EventArgs e)
        {
            ExpansionCircleMarkerManager _TM = Application.OpenForms["ExpansionCircleMarkerManager"] as ExpansionCircleMarkerManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ExpansionCircleMarkerManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Expansion Circle Marker Manager....");
                label1.Text = "Expansion Circle Marker Manager";
            }
            timer1.Start();
        }
        private void Lootchest_Click(object sender, EventArgs e)
        {
            Lootchest _TM = Application.OpenForms["Lootchest"] as Lootchest;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new Lootchest
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Loot Chest manager....");
                label1.Text = "CJ Loot Chests manager";
            }
            timer1.Start();
        }
        private void RHLootBoxManagerButton_Click(object sender, EventArgs e)
        {
            RHLootBoxManager _TM = Application.OpenForms["RHLootBoxManager"] as RHLootBoxManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new RHLootBoxManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Loot Pool manager....");
                label1.Text = "Capare LootBox manager";
            }
            timer1.Start();
        }
        private void LootPoolManagerButton_Click(object sender, EventArgs e)
        {
            LootPoolManager _TM = Application.OpenForms["LootPoolManager"] as LootPoolManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new LootPoolManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Loot Pool manager....");
                label1.Text = "Capare Loot Pool manager";
            }
            timer1.Start();
        }
        private void HelicrashManagerButton_Click(object sender, EventArgs e)
        {

            HelicrashMissionsManager _TM = Application.OpenForms["HelicrashMissionsManager"] as HelicrashMissionsManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new HelicrashMissionsManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Heli crash Mission manager....");
                label1.Text = "Capare Heli Crash manager";
            }
            timer1.Start();
        }
        private void KOTHManagerButton_Click(object sender, EventArgs e)
        {
            KOTHManager _TM = Application.OpenForms["KOTHManager"] as KOTHManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new KOTHManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading KOTH manager....");
                label1.Text = "MDC KOTH manager";
            }
            timer1.Start();
        }
        private void BBPManagerButton_Click(object sender, EventArgs e)
        {
            BaseBuildingPlus _TM = Application.OpenForms["BaseBuildingPlus"] as BaseBuildingPlus;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new BaseBuildingPlus
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Base Building Plus manager....");
                label1.Text = "Base Building Plus manager";
            }
            timer1.Start();
        }
        private void SearchForLootManagerButton_Click(object sender, EventArgs e)
        {
            SearchForLootManager _TM = Application.OpenForms["SearchForLootManager"] as SearchForLootManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new SearchForLootManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Search For Loot manager....");
                label1.Text = "Base Search For Loot manager";
            }
            timer1.Start();
        }
        private void RAGTysonBBManagerButton_Click(object sender, EventArgs e)
        {
            RagTysonBaseBuildingManager _TM = Application.OpenForms["RagTysonBaseBuildingManager"] as RagTysonBaseBuildingManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new RagTysonBaseBuildingManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Rag Tyson Base Building manager....");
                label1.Text = "Rag Tyson Base Building manager";
            }
            timer1.Start();
        }
        private void AirdropUpgradedManagerButton_Click(object sender, EventArgs e)
        {
            AirdropUpgradedManager _TM = Application.OpenForms["AirdropUpgradedManager"] as AirdropUpgradedManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new AirdropUpgradedManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Airdrop Upgraded Manager....");
                label1.Text = "Airdrop Upgraded Manager";
            }
            timer1.Start();
        }
        private void KOSzoneManagerButton_Click(object sender, EventArgs e)
        {
            KOSZonemanager _TM = Application.OpenForms["KOSZone"] as KOSZonemanager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new KOSZonemanager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading KOSZone manager....");
                label1.Text = "RH KOS Zone manager";
            }
            timer1.Start();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "Please select the map_output.txt u wish to convert";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                MapData.CreateNewData(openfile.FileName);
            }
        }
        private void AbandonedVehicleRemoverManagerButton_Click(object sender, EventArgs e)
        {
            ABVManager _TM = Application.OpenForms["ABVManager"] as ABVManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ABVManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading Abandoned Vehicle Remover manager....");
                label1.Text = "Abandoned Vehicle manager";
            }
            timer1.Start();
        }
        private void BreachingChargeManagerButton_Click(object sender, EventArgs e)
        {
            BreachingChargeManager _TM = Application.OpenForms["BreachingChargeManager"] as BreachingChargeManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new BreachingChargeManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading breaching charge manager....");
                label1.Text = "Breaching charge manager";
            }
            timer1.Start();
        }
        private void DynamicWeatherManagerButton_Click(object sender, EventArgs e)
        {
            DynamicWeatherPluginManager _TM = Application.OpenForms["DynamicWeatherPluginManager"] as DynamicWeatherPluginManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new DynamicWeatherPluginManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading Dynamic Weather Plugin Manager....");
                label1.Text = "Dynamic Weather Plugin Manager";
            }
            timer1.Start();
        }
        private void AdvancedWB_Click(object sender, EventArgs e)
        {
            AdvancedWorkBenchManager _TM = Application.OpenForms["AdvancedWorkBenchManager"] as AdvancedWorkBenchManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new AdvancedWorkBenchManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading BP WorkBenchManager manager....");
                label1.Text = "BP Workbench manager";
            }
            timer1.Start();
        }
        private void WorkBenchReduxButton_Click(object sender, EventArgs e)
        {
            WorkBenchReduxManager _TM = Application.OpenForms["WorkBenchReduxManager"] as WorkBenchReduxManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new WorkBenchReduxManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading WorkBenchReduxManager manager....");
                label1.Text = "Workbench Redux manager";
            }
            timer1.Start();
        }
        private void CapareWorkBenchManagerButton_Click(object sender, EventArgs e)
        {
            CapareWorkBenchManager _TM = Application.OpenForms["CapareWorkBenchManager"] as CapareWorkBenchManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new CapareWorkBenchManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading capareWorkBenchManager manager....");
                label1.Text = "Capare Workbench manager";
            }
            timer1.Start();
        }
        private void TerjeManagerButton_Click(object sender, EventArgs e)
        {
            TerjeManager _TM = Application.OpenForms["TerjeManager"] as TerjeManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new TerjeManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading Terje Manager manager....");
                label1.Text = "Terje Manager";
            }
            timer1.Start();
        }
        private void CapareTreasureManagerButton_Click(object sender, EventArgs e)
        {
            CapareTreasureManager _TM = Application.OpenForms["CapareTreasureManager"] as CapareTreasureManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new CapareTreasureManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading capare Treasure Manager....");
                label1.Text = "Capare Treasure Manager";
            }
            timer1.Start();
        }
        private void UtopiaAirdropButton_Click(object sender, EventArgs e)
        {
            UtopiaAirdropManager _TM = Application.OpenForms["UtopiaAirdropManager"] as UtopiaAirdropManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new UtopiaAirdropManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading Utopia Airdrop manager....");
                label1.Text = "Utopia Airdrop Manager";
            }
            timer1.Start();
        }
        private void MysteryBoxButton_Click(object sender, EventArgs e)
        {
            MysteryBoxManager _TM = Application.OpenForms["MysteryBoxManager"] as MysteryBoxManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new MysteryBoxManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading Mystery Box manager....");
                label1.Text = "Mystery Box manager";
            }
            timer1.Start();
        }
        private void MPGSpawnerButton_Click(object sender, EventArgs e)
        {
            MPGSpawnerManager _TM = Application.OpenForms["MPGSpawnerManager"] as MPGSpawnerManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new MPGSpawnerManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading MPG Spawner Manager....");
                label1.Text = "MPG Spawner Manager";
            }
            timer1.Start();
        }
        private void SpawnerBukakuManagerButton_Click(object sender, EventArgs e)
        {
            SpawnerBubakuManager _TM = Application.OpenForms["SpawnerBukakuManager"] as SpawnerBubakuManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new SpawnerBubakuManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loadingSpawner Bubaku Manager....");
                label1.Text = "Spawner Bubaku Manager";
            }
            timer1.Start();
        }
        private void PVZCZManagerButton_Click(object sender, EventArgs e)
        {
            PVZCZManager _TM = Application.OpenForms["PVZCZManager"] as PVZCZManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new PVZCZManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loadingSpawner PVZ Mods Manager....");
                label1.Text = "PVZ Mods Manager";
            }
            timer1.Start();
        }
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            ImageConvertor _TM = Application.OpenForms["ImageConvertor"] as ImageConvertor;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new ImageConvertor
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),

                };
                _TM.Show();
                Console.WriteLine("loading Image Convertor....");
                label1.Text = "Image Convertor";
            }
            timer1.Start();
            

        }
        private void TimedCrateManagerButton_Click(object sender, EventArgs e)
        {
            TimedCrateManager _TM = Application.OpenForms["TimedCrateManager"] as TimedCrateManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new TimedCrateManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Timed Crate Manager....");
                label1.Text = "Timed Crate Manager";
            }
            timer1.Start();
        }
        private void KillRewardManagerButton_Click(object sender, EventArgs e)
        {
            killRewardManager _TM = Application.OpenForms["killRewardManager"] as killRewardManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new killRewardManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading kill Reward Manager....");
                label1.Text = "kill Reward Manager";
            }
            timer1.Start();
        }
        private void PlayerDBButton_Click(object sender, EventArgs e)
        {
            PlayerDBManager _TM = Application.OpenForms["PlayerDBManager"] as PlayerDBManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new PlayerDBManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()
                };
                _TM.Show();
                Console.WriteLine("loading Player DB Manager....");
                label1.Text = "Player DB Manager";
            }
            timer1.Start();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.FormState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormLocation = this.Location;
                Properties.Settings.Default.FormSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.FormLocation = this.RestoreBounds.Location;
                Properties.Settings.Default.FormSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.ShowConsole = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    StringBuilder sb = new StringBuilder();
                    DZE importfile = DZEHelpers.LoadFile(filePath);

                    //SaveFileDialog save = new SaveFileDialog();
                    //save.Title = "Export AI Patrol";
                    //save.Filter = "Expansion Map |*.map|Object Spawner|*.json";
                    //if (save.ShowDialog() == DialogResult.OK)
                    //{
                    //    switch (save.FilterIndex)
                    //    {
                    //        case 1:
                    //            StringBuilder SB = new StringBuilder();
                    //            foreach (Editorobject eo in importfile.EditorObjects)
                    //            {
                    //                SB.AppendLine(eo.DisplayName + "|" + eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2].ToString() + "|" + eo.Orientation[0].ToString() + " " + eo.Orientation[1].ToString() + " " + eo.Orientation[2].ToString());
                    //            }
                    //            foreach (Editordeletedobject eo in importfile.EditorHiddenObjects)
                    //            {
                    //                SB.AppendLine("-" + eo.Type + "|" + eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2].ToString() + "|0 0 0");
                    //            }
                    //            File.WriteAllText(save.FileName, SB.ToString());
                    //            break;
                    //        case 2:
                    //            break;
                    //        case 3:
                    //            break;
                    //    }
                    //}

                    ObjectSpawnerArr newobjectspawnerarr = importfile.convertToObjectSpawner();

                    AddNeweventFile form = new AddNeweventFile
                    {
                        currentproject = Projects.getActiveProject(),
                        newlocation = true,
                        SetTitle = "Add New Object Spawner File",
                        settype = "Object Spanwer File Name",
                        SetFolderName = "Select folder where File will created, must be in mpmission folder....",
                        setbuttontest = "Add Add Object Spawner"
                    };
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Project currentproject = Projects.getActiveProject();
                        string path = form.CustomLocation;
                        string modname = form.TypesName;
                        Directory.CreateDirectory(path);
                        newobjectspawnerarr.Filename = modname + ".json";
                        currentproject.CFGGameplayConfig.AddnewObjectSpawner(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", "") + "/" + modname + ".json");
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(newobjectspawnerarr, options);
                        File.WriteAllText(path + "\\" + newobjectspawnerarr.Filename, jsonString);
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("FIle has been created and save as " + modname + ".json\nThe entry has been added to CFGgameplay.json");
                    }

                }
            }
        }


    }
}


