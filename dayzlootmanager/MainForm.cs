using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using DarkUI.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;
using DayZeLib;
using System.Net;
using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Linq;

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
        public string VersionNumber = "0.7.8.8";
        private static bool hidden;
        public static String ProjectsJson = Application.StartupPath + "\\Project\\Projects.json";
        public ProjectList Projects;

        public SplashForm frm;
        public event EventHandler LoadCompleted;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnLoadCompleted(EventArgs.Empty);
            if((Projects.ActiveProject != null && Projects.ActiveProject != "") && Projects.getActiveProject().haswarnings)
            {
                MessageBox.Show("Warnings detected. please see console.....");
            }
        }

        private void CheckChangeLog()
        {
            if(Projects.ShowChangeLog)
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
            }
        }

        protected virtual void OnLoadCompleted(EventArgs e)
        {
            LoadCompleted?.Invoke(this, e);
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Current Version : " + VersionNumber);
            var culture = CultureInfo.GetCultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            TitleLabel.Text = "DayZeEditor " + VersionNumber + " by Shawminator ";
            if(CheckForUpdate())
            {
                Application.Exit();
                return;
            }


            if (File.Exists(ProjectsJson))
            {
                Projects = (JsonSerializer.Deserialize<ProjectList>(File.ReadAllText(ProjectsJson)));
                if (Projects.getActiveProject() != null)
                {
                    if (Projects.getActiveProject().ProfilePath == null)
                    {
                        Projects.getActiveProject().ProfilePath = "profile";
                        Projects.SaveProject();
                    }
                    Console.WriteLine(Projects.ActiveProject + " is the Current Active Project");
                    Console.WriteLine("Will now serialize base economy files from Project " + Projects.ActiveProject);
                    if(Projects.getActiveProject().Setmappedrive())
                    {
                        Projects.getActiveProject().seteconomycore();
                        Projects.getActiveProject().seteconomydefinitions();
                        Projects.getActiveProject().setuserdefinitions();
                        Projects.getActiveProject().setVanillaTypes();
                        Projects.getActiveProject().SetModListtypes();
                        Projects.getActiveProject().SetTotNomCount();
                        Projects.getActiveProject().SetSpawnabletypes();
                        Projects.getActiveProject().setplayerspawns();
                        Projects.getActiveProject().SetCFGGameplayConfig();
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
                        var form = Application.OpenForms["SplashForm"];
                        if (form != null)
                        {
                            form.Invoke(new Action(() => { form.Close(); }));
                        }
                        string project = Projects.ActiveProject;
                        Projects.SetActiveProject();
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
                        MessageBox.Show("The Project " + project + " could not be loaded, please load another project...." );
                    }
                    
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
                Projects = new ProjectList{};
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
                System.Diagnostics.Process.Start("Updater.exe", zipfile);
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
            if(getData.StartsWith("Offline"))
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
            if(sender is PictureBox)
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
            var handle = GetConsoleWindow();
            if (checkBox1.Checked)
                ShowWindow(handle, SW_SHOW);
            else
                ShowWindow(handle, SW_HIDE);
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

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\Advanced_Workbench\\AdvancedWorkBenchConfig.json"))
                    AdvancedWorkbenchButton.Visible = true;
                else
                    AdvancedWorkbenchButton.Visible = false;

                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\MagicCrateManagerSettings.json"))
                    MysteryBoxButton.Visible = true;
                else
                    MysteryBoxButton.Visible = false;

                // Cant be arsed with this, fucking stupid config........
                //if (Directory.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\DNA_Keycards"))
                //    DNAKeyCardsButton.Visible = true;
                //else
                //    DNAKeyCardsButton.Visible = false;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                SlidePanel.Width = SlidePanel.Width + 10;
                if (SlidePanel.Width == 160)
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
                MapData data = new MapData(openfile.FileName);
                data.CreateNewData();
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
                Console.WriteLine("loading AdvancedWorkBenchManager manager....");
                label1.Text = "RH Advanced Workbench manager";
            }
            timer1.Start();
        }
        private void DNAKeyCardsButton_Click(object sender, EventArgs e)
        {
            DNAKeyCardsManager _TM = Application.OpenForms["DNAKeyCardsManager"] as DNAKeyCardsManager;
            if (_TM != null)
            {
                _TM.WindowState = FormWindowState.Normal;
                _TM.BringToFront();
                _TM.Activate();
            }
            else
            {
                closemdichildren();
                _TM = new DNAKeyCardsManager
                {
                    MdiParent = this,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new System.Drawing.Point(30, 0),
                    Size = Form_Controls.Formsize - new System.Drawing.Size(37, 61),
                    currentproject = Projects.getActiveProject()

                };
                _TM.Show();
                Console.WriteLine("loading DNAKeyCards manager....");
                label1.Text = "DNA Keycards manager";
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
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    StringBuilder sb = new StringBuilder();
                    DZE importfile = DZEHelpers.LoadFile(filePath);

                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(importfile, options);
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(savefile.FileName, jsonString);
                    }
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Projects.getActiveProject() != null)
            {
                if (Projects.getActiveProject().MapNetworkDrive == true)
                {
                    NetworkDrive.DisconnectNetworkDrive(Projects.getActiveProject().Networkdriveletter, true);
                }
            }
        }


    }
}


