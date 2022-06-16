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
        public string VersionNumber = "0.6.2";
        private static bool hidden;
        public static String ProjectsJson = Application.StartupPath + "\\Project\\Projects.json";
        public ProjectList Projects;

        public SplashForm frm;
        public event EventHandler LoadCompleted;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OnLoadCompleted(EventArgs.Empty);
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
                CloseButton
            );
            SlidePanel.Width = 30;
            hidden = true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            var culture = CultureInfo.GetCultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            TitleLabel.Text = "DayZeEditor " + VersionNumber + " by Shawminator";
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
                    Projects.getActiveProject().seteconomycore();
                    Projects.getActiveProject().seteconomydefinitions();
                    Projects.getActiveProject().setuserdefinitions();
                    Projects.getActiveProject().setplayerspawns();
                    Projects.getActiveProject().SetCFGGameplayConfig();
                    Projects.getActiveProject().SetcfgEffectAreaConfig();
                    Projects.getActiveProject().SetEvents();
                    Projects.getActiveProject().seteventspawns();
                    Projects.getActiveProject().SetRandompresets();
                    Projects.getActiveProject().SetSpawnabletypes();
                    Projects.getActiveProject().SetGlobals();
                    Projects.getActiveProject().SetWeather();
                    Projects.getActiveProject().setVanillaTypes();
                    Projects.getActiveProject().SetModListtypes();
                    Projects.getActiveProject().SetTotNomCount();
                    Projects.getActiveProject().Setmapgrouproto();
                    Projects.getActiveProject().Setmapgroupos();
                    //Projects.getActiveProject().GetPlayerDB();
                    Console.WriteLine(Projects.ActiveProject + " is the Current Active Project");
                    Console.WriteLine("Project is Running Dr Jones Trader...." + Projects.getActiveProject().usingDrJoneTrader.ToString());
                    Console.WriteLine("Project is Running Expansion Market...." + Projects.getActiveProject().usingexpansionMarket.ToString());
                    Console.WriteLine("Project is Running Trader Plus...." + Projects.getActiveProject().usingtraderplus.ToString());
                    Console.WriteLine("Project is Running the following Mods....");
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
                    toolStripStatusLabel1.Text = "No Active Project Found, Please Load a Project from the Projects panel.....";
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
            }
        }
        private void closemdichildren()
        {
            if (MdiChildren.Length >= 1)
                MdiChildren[0].Close();
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
                    MarketButton.Visible = true;
                else
                    MarketButton.Visible = false;
                if (Projects.getActiveProject().usingtraderplus)
                    TraderPlusButton.Visible = true;
                else
                    TraderPlusButton.Visible = false;
                if (Projects.getActiveProject().isUsingExpansion())
                    ExpansionSettingsButton.Visible = true;
                else
                    ExpansionSettingsButton.Visible = false;
                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\CJ_LootChests\\LootChests_V105.json"))
                    LootchestButton.Visible = true;
                else
                    LootchestButton.Visible = false;
                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\HeliCrashMissions\\Helicrash.json"))
                    HelicrashManagerButton.Visible = true;
                else
                    HelicrashManagerButton.Visible = false;
                if (File.Exists(Projects.getActiveProject().projectFullName + "\\" + Projects.getActiveProject().ProfilePath + "\\KingOfTheHill.json"))
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



            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                SlidePanel.Width = SlidePanel.Width + 10;
                if (SlidePanel.Width == 140)
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
            }
            timer1.Start();
        }
        private void toolStripButton3_Click_1(object sender, EventArgs e)
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
                Console.WriteLine("loading KOSZone manager....");
            }
            timer1.Start();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
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
            }
            timer1.Start();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "Please select the map_output.txt upi wosh to convert";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                MapData data = new MapData(openfile.FileName);
                data.CreateNewData();
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
    }
}
