﻿using DarkUI.Forms;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Windows.Forms;
using WinSCP;

namespace DayZeEditor
{
    public partial class ProjectPanel : DarkForm
    {
        public ProjectList projects { get; set; }
        public Project ActiveProject;

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
        public ProjectPanel()
        {
            InitializeComponent();

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (FTPPasswordTB.Text == null || FTPPasswordTB.Text == "")
                toolStripButton2.Visible = false;
            else
            {
                toolStripButton2.Visible = true;
                toolStripButton2.AutoSize = true;
            }
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FTPTSB.Checked = false;
            ProjectTSB.Checked = false;
            toolStripButton1.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    ProjectTSB.Checked = true;
                    break;
                case 1:
                    FTPTSB.Checked = true;
                    break;
                case 2:
                    toolStripButton1.Checked = true;
                    break;
            }
        }
        private void ProjectPanel_Load(object sender, EventArgs e)
        {

            if (this.FTPPasswordTB.Control is TextBox)
            {
                TextBox tb = this.FTPPasswordTB.Control as TextBox;
                tb.PasswordChar = '*';
            }

            getActiveProject();
            LoadProjectstoList();
            LoadFileExplorer();
            LoadMappAddons();
            LoadFTPInfo();
            tabControl1.ItemSize = new Size(0, 1);

        }
        public void LoadMappAddons()
        {
            Console.WriteLine("Checking GitHub For Newest Release.....");
            GitHub info = getavaiableMapAddons();
            foreach (Asset ass in info.assets)
            {
                if (ass.name == "MapSizes.txt") continue;
                ListViewItem item = new ListViewItem();
                item.Text = ass.name;
                item.Tag = ass;
                string name = ass.name.Split(new string[] { "Map" }, StringSplitOptions.None)[0];
                if (File.Exists(Application.StartupPath + "\\Maps\\" + name + "_Map.png"))
                    item.SubItems.Add("Installed");
                listView3.Items.Add(item);
            }
        }
        public GitHub getavaiableMapAddons()
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
            var url = "https://api.github.com/repos/Shawminator/DayZeEditor/releases/tags/DayZeEditor_mapAddons";

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
        public void getActiveProject()
        {
            ActiveProject = projects.getActiveProject();
            if (ActiveProject != null)
            {
                darkLabel4.Text = projects.ActiveProject;
                if (ActiveProject.usingDrJoneTrader)
                    radioButton3.Checked = true;
                else if (ActiveProject.usingexpansionMarket)
                    radioButton2.Checked = true;
                else if (ActiveProject.usingtraderplus)
                    radioButton4.Checked = true;
                else
                    radioButton1.Checked = true;

                checkBox1.Checked = ActiveProject.Createbackups;
                ((MainForm)this.MdiParent).toolStripStatusLabel1.Text = ActiveProject.ProjectName + ":" + ActiveProject.mpmissionpath.Split('.')[1] + " is the Current Active Project";
            }

        }
        private void LoadProjectstoList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = projects.Projects;
            listBox1.Invalidate();
            listBox1.SelectedItem = ActiveProject;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectFolderTB.Text = fb.SelectedPath;
            }
            else
                ProjectFolderTB.Text = "";
        }
        private void selectProfilefolderNamebutton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectProfileTB.Text = fb.SelectedPath;
            }
        }

        private void MissionFoldertoUsebutton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectMissionFolderTB.Text = fb.SelectedPath;

                string path1 = ProjectMissionFolderTB.Text;
                string path2 = ProjectProfileTB.Text;

                // Get two levels up from each path
                string basePath1 = Directory.GetParent(Directory.GetParent(path1).FullName).FullName;
                string basePath2 = Directory.GetParent(path2).FullName;

                // Print results
                Console.WriteLine("Base Path 1: " + basePath1);
                Console.WriteLine("Base Path 2: " + basePath2);

                // Compare
                if (string.Equals(basePath1, basePath2, StringComparison.OrdinalIgnoreCase))
                {
                    ProjectFolderTB.Text = basePath1;
                }
                else
                    MessageBox.Show("Root Directory seems to be different, check yo uhave selected the correct paths.....");
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            string projecttype = ProjectTypeComboBox.GetItemText(ProjectTypeComboBox.SelectedItem);
            if (projecttype == "Create Blank")
            {
                string ProjectFolder = ProjectFolderTB.Text;
                string ProjectName = ProjectNameTB.Text;
                if (ProjectFolder == "" || ProjectName == "")
                {
                    MessageBox.Show("Please select both a Project name and a directory where to save it.");
                    return;
                }
                string ProjectPath = ProjectFolder + "\\" + ProjectName;
                Directory.CreateDirectory(ProjectPath);

                if (ProjectProfileTB.Text == "" || ProjectMissionFolderTB.Text == "")
                {
                    MessageBox.Show("Please select both a Profile name and an exact mpmission name.");
                    return;
                }
                string missionsfolder = ProjectMissionFolderTB.Text;
                string profilefolder = ProjectProfileTB.Text;
                string mpmissionpath = Path.GetFileName(missionsfolder);
                string PmissionFolder = ProjectPath + "\\mpmissions\\" + Path.GetFileName(missionsfolder);
                string Pprofilefolder = ProjectPath + "\\" + profilefolder;

                Directory.CreateDirectory(PmissionFolder);
                Directory.CreateDirectory(Pprofilefolder);

                Project project = new Project();
                project.AddNames(ProjectName, ProjectPath);
                project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                project.mpmissionpath = mpmissionpath;
                project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                project.ProfilePath = profilefolder;
                projects.addtoprojects(project, false);
                LoadProjectstoList();
                MessageBox.Show("Project created, Please Close the editor and populate the missions files before trying to load this project if not using existing files...");
                Process.Start(project.projectFullName);
            }
            else if (projecttype == "Create Local from FTP/SFTP")
            {
                try
                {
                    if (session == null || !session.Opened)
                    {
                        if (FTPHostNameTB.Text == "" || FTPHostNameTB.Text == null)
                        {
                            MessageBox.Show("Please fill in FTP Connection info in the FTP Tab....");
                            return;
                        }
                        SessionOptions sessionOptions = new SessionOptions
                        {
                            Protocol = Protocol.Ftp,
                            HostName = FTPHostNameTB.Text,
                            PortNumber = Convert.ToInt32(FTPPortTB.Text),
                            UserName = FTPUSernameTB.Text,
                            Password = FTPPasswordTB.Text,
                        };
                        try
                        {
                            Console.WriteLine("Attemptimg FTP First");
                            session = new Session();
                            session.FileTransferProgress += SessionFileTransferProgress;
                            session.Open(sessionOptions);
                            Console.WriteLine("FTP Session Open.....");
                        }
                        catch 
                        {
                            Console.WriteLine("Now Attemptimg SFTP");
                            sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = FTPHostNameTB.Text,
                                PortNumber = Convert.ToInt32(FTPPortTB.Text),
                                SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny,
                                UserName = FTPUSernameTB.Text,
                                Password = FTPPasswordTB.Text
                            };
                            session = new Session();
                            session.FileTransferProgress += SessionFileTransferProgress;
                            session.Open(sessionOptions);
                            Console.WriteLine("SFTP Session Open.....");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Please check connection info.....");
                    return;
                }

                string ProjectFolder = ProjectFolderTB.Text;
                string ProjectName = ProjectNameTB.Text;
                if (ProjectFolder == "" || ProjectName == "")
                {
                    MessageBox.Show("Please select both a Project name and a directory where to save it.");
                    return;
                }
                string ProjectPath = ProjectFolder + "\\" + ProjectName;
                Directory.CreateDirectory(ProjectPath);
                Console.WriteLine("Directory Created:" + ProjectPath);
                NewProjectFTP ftpproject = new NewProjectFTP();
                ftpproject.SetTitle = "New Project from FTP";
                ftpproject.session = session;
                DialogResult result = ftpproject.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Thread load = new Thread(new ThreadStart(showLoading));
                    load.Start();
                    Project project = new Project();
                    if (ftpproject.Isconsole)
                    {
                        string mpmissiondirectory = ftpproject.MpMissionDirectory;
                        string mpmissionpath = Path.GetFileName(mpmissiondirectory);

                        Directory.CreateDirectory(ProjectPath + "\\mpmissions\\" + mpmissionpath);
                        session.GetFilesToDirectory(mpmissiondirectory, ProjectPath + "\\mpmissions\\" + mpmissionpath);

                        project.AddNames(ProjectName, ProjectPath);
                        project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                        project.mpmissionpath = mpmissionpath;
                        project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                        project.ProfilePath = "";
                        projects.addtoprojects(project);
                    }
                    else
                    {
                        string profiledir = ftpproject.ProfileDirecrtory;
                        string mpmissiondirectory = ftpproject.MpMissionDirectory;
                        string mpmissionpath = Path.GetFileName(mpmissiondirectory);
                        string profile = Path.GetFileName(profiledir);

                        TransferOptions transferoptions = new TransferOptions()
                        {
                            FileMask = "|*.log;*.ADM,*.RPT"
                        };

                        Directory.CreateDirectory(ProjectPath + "\\" + profile);
                        Console.WriteLine("Directory Created:" + ProjectPath + "\\" + profile);
                        Directory.CreateDirectory(ProjectPath + "\\mpmissions\\" + mpmissionpath);
                        Console.WriteLine("Directory Created:" + ProjectPath + "\\mpmissions\\" + mpmissionpath);
                        session.GetFilesToDirectory(profiledir, ProjectPath + "\\" + profile, null, false, transferoptions);
                        session.GetFilesToDirectory(mpmissiondirectory, ProjectPath + "\\mpmissions\\" + mpmissionpath, null, false, transferoptions);

                        project.AddNames(ProjectName, ProjectPath);
                        project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                        project.mpmissionpath = mpmissionpath;
                        project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                        project.ProfilePath = profile;
                        projects.addtoprojects(project);
                    }
                    SetActiveProject(project);
                    LoadProjectstoList();

                    load.Abort();
                }
            }
            else if (projecttype == "Connect Direct to FTP/SFTP")
            {
            }
            else if (projecttype == "Connect to Exisiting Server")
            {
                string ProjectPath = ProjectFolderTB.Text;
                string ProjectName = ProjectNameTB.Text;
                if (ProjectName == "")
                {
                    MessageBox.Show("Please select a Project name.");
                    return;
                }
                string missionsfolder = ProjectMissionFolderTB.Text;
                string profilefolder = Path.GetFileName(ProjectProfileTB.Text);
                string mpmissionpath = Path.GetFileName(missionsfolder);

                Project project = new Project();
                project.AddNames(ProjectName, ProjectPath);
                project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                project.mpmissionpath = mpmissionpath;
                project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                project.ProfilePath = profilefolder;
                projects.addtoprojects(project, false);
                LoadProjectstoList();
                MessageBox.Show("Project created, select the project from the list and load....");

            }
        }
        private static string _lastFileName;
        private static void SessionFileTransferProgress( object sender, FileTransferProgressEventArgs e)
        {
            // New line for every new file
            if ((_lastFileName != null) && (_lastFileName != e.FileName))
            {
                Console.WriteLine();
            }

            // Print transfer progress
            Console.Write("\r{0} ({1:P0})", e.FileName, e.FileProgress);
            // Remember a name of the last file reported
            _lastFileName = e.FileName;
        }
        private void showLoading()
        {
            LoadingScreen loading = new LoadingScreen();
            loading.ShowDialog();
        }
        private int Getmapsizefrommissionpath(string mpmissionpath)
        {
            string[] MapSizeList = File.ReadAllLines("Maps/MapSizes.txt");
            Dictionary<string, int> maplist = new Dictionary<string, int>();
            foreach (string line in MapSizeList)
            {
                maplist.Add(line.Split(':')[0], Convert.ToInt32(line.Split(':')[1]));
            }
            string currentmap = mpmissionpath.ToLower().Split('.')[1];
            int size;
            if (maplist.TryGetValue(currentmap, out size))
            {
                return size;
            }
            return 0;
        }
        private void ProjectTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string projecttype = ProjectTypeComboBox.GetItemText(ProjectTypeComboBox.SelectedItem);
            if (projecttype == "Create Local from FTP/SFTP" || projecttype == "Connect Direct to FTP/SFTP")
            {
                ProjectNameLabel.Visible = true;
                ProjectNameTB.Visible = true;

                SelectProjectFolderlabel.Visible = true;
                SelectProjectFolderlabel.Text = "Select Project Folder";
                ProjectFolderTB.Visible = true;
                ProjectFolderTB.ReadOnly = false;
                ProjectFolderTB.Size = new Size(630, 20);
                SelectProjectFolderbutton.Visible = true;

                ProfileFolderNamelabel.Visible = false;
                ProjectProfileTB.Visible = false;
                selectProfilefolderNamebutton.Visible = false;

                MissionFoldertoUselabel.Visible = false;
                ProjectMissionFolderTB.Visible = false;
                MissionFoldertoUsebutton.Visible = false;

                CreateProjectbutton.Location = new Point(401, 105);
            }
            else if (projecttype == "Create Blank")
            {
                ProjectNameLabel.Visible = true;
                ProjectNameTB.Visible = true;

                SelectProjectFolderlabel.Visible = true;
                SelectProjectFolderlabel.Text = "Select Project Folder";
                ProjectFolderTB.Visible = true;
                ProjectFolderTB.ReadOnly = false;
                ProjectFolderTB.Size = new Size(630, 20);
                SelectProjectFolderbutton.Visible = true;

                ProfileFolderNamelabel.Visible = true;
                ProfileFolderNamelabel.Text = "Profile Folder Name";
                ProjectProfileTB.Visible = true;
                ProjectProfileTB.Size = new Size(657, 20);
                selectProfilefolderNamebutton.Visible = false;

                MissionFoldertoUselabel.Visible = true;
                MissionFoldertoUselabel.Text = "Mission Folder to use";
                ProjectMissionFolderTB.Visible = true;
                ProjectMissionFolderTB.Size = new Size(657, 20);
                MissionFoldertoUsebutton.Visible = false;

                CreateProjectbutton.Location = new Point(402, 149);
            }
            else if (projecttype == "Connect to Exisiting Server")
            {
                ProjectNameLabel.Visible = true;
                ProjectNameTB.Visible = true;

                SelectProjectFolderlabel.Visible = true;
                SelectProjectFolderlabel.Text = "Project Folder";
                ProjectFolderTB.Visible = true;
                ProjectFolderTB.ReadOnly = true;
                ProjectFolderTB.Size = new Size(657, 20);
                SelectProjectFolderbutton.Visible = false;

                ProfileFolderNamelabel.Visible = true;
                ProfileFolderNamelabel.Text = "Profile Path";
                ProjectProfileTB.Visible = true;
                ProjectProfileTB.Size = new Size(630, 20);
                selectProfilefolderNamebutton.Visible = true;

                MissionFoldertoUselabel.Visible = true;
                MissionFoldertoUselabel.Text = "Mission Path";
                ProjectMissionFolderTB.Visible = true;
                ProjectMissionFolderTB.Size = new Size(630, 20);
                MissionFoldertoUsebutton.Visible = true;

                CreateProjectbutton.Location = new Point(402, 149);
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }

            string profilename = listBox1.GetItemText(listBox1.SelectedItem);
            Project p = listBox1.SelectedItem as Project;
            SetActiveProject(p);
            LoadFTPInfo();
            Console.WriteLine("The Current Active Project is " + projects.ActiveProject);
            Console.WriteLine("Please click the select section to get the pop out menu");
        }
        private void SetActiveProject(Project p)
        {
            
            if(!p.checkMapExists())
            {
                MessageBox.Show("Map File not found for selected project\nPlease download the appropiate map addon from the Map Addons tab");
                return;
            };
            projects.SetActiveProject(p);
            darkLabel4.Text = projects.ActiveProject;
            projects.getActiveProject().seteconomycore();
            projects.getActiveProject().seteconomydefinitions();
            projects.getActiveProject().setuserdefinitions();
            projects.getActiveProject().setplayerspawns();
            projects.getActiveProject().SetCFGGameplayConfig();
            projects.getActiveProject().SetCFGUndergroundTriggerConfig();
            projects.getActiveProject().SetcfgEffectAreaConfig();
            projects.getActiveProject().SetEvents();
            projects.getActiveProject().seteventspawns();
            projects.getActiveProject().seteventgroups();
            projects.getActiveProject().SetRandompresets();
            projects.getActiveProject().SetSpawnabletypes();
            projects.getActiveProject().SetGlobals();
            projects.getActiveProject().SetWeather();
            projects.getActiveProject().SetIgnoreList();
            projects.getActiveProject().setVanillaTypes();
            projects.getActiveProject().SetModListtypes();
            projects.getActiveProject().SetTotNomCount();
            projects.getActiveProject().AssignRarity();
            projects.getActiveProject().Setmapgrouproto();
            projects.getActiveProject().Setmapgroupos();
            projects.getActiveProject().SetCfgEnviroment();
            projects.getActiveProject().SetTerritories();

            getActiveProject();
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            projects.SaveProject();
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Yes will remove project and all files in the project folder\nNo will only remove the project from the editor\nCancel will return with no changes", "Delete All Files.....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string profilename = listBox1.GetItemText(listBox1.SelectedItem);
                listBox1.Items.Remove(profilename);
                Project p = projects.getprojectfromname(profilename);
                projects.DeleteProject(profilename);
                if (MessageBox.Show("Double checking you want to remove all files\nselecting no will still remove the project from the editor but keep all files in the project folder\nAre you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Directory.Delete(p.projectFullName, true);
                }
                if (ActiveProject == p)
                {
                    ActiveProject = null;
                    projects.ActiveProject = "";
                }
                projects.SaveProject(false, false);
                LoadProjectstoList();
                MessageBox.Show("Project and files Removed....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (result == DialogResult.No)
            {
                string profilename = listBox1.GetItemText(listBox1.SelectedItem);
                listBox1.Items.Remove(profilename);
                Project p = projects.getprojectfromname(profilename);
                projects.DeleteProject(profilename);
                if (ActiveProject == p)
                {
                    ActiveProject = null;
                    projects.ActiveProject = "";
                }
                projects.SaveProject(false, false);
                LoadProjectstoList();
                MessageBox.Show("Project Removed....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void TraderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (ActiveProject == null) return;
            if (radioButton1.Checked)
            {
                ActiveProject.usingDrJoneTrader = false;
                ActiveProject.usingexpansionMarket = false;
                ActiveProject.usingtraderplus = false;
            }
            else if (radioButton2.Checked)
            {
                ActiveProject.usingexpansionMarket = true;
                ActiveProject.usingDrJoneTrader = false;
                ActiveProject.usingtraderplus = false;
                // Do other stuff
            }
            else if (radioButton3.Checked)
            {
                ActiveProject.usingDrJoneTrader = true;
                ActiveProject.usingexpansionMarket = false;
                ActiveProject.usingtraderplus = false;

                if (!Directory.Exists(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\Trader"))
                    Directory.CreateDirectory(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\Trader");
                DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + "\\Trader");
                FileInfo[] Files = dinfo.GetFiles("*.txt");
                foreach (FileInfo file in Files)
                {
                    if (!File.Exists(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\Trader\\" + file.Name))
                        File.Copy(file.FullName, ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\Trader\\" + file.Name);
                }
            }
            else if (radioButton4.Checked)
            {
                ActiveProject.usingDrJoneTrader = false;
                ActiveProject.usingexpansionMarket = false;
                ActiveProject.usingtraderplus = true;

                if (!Directory.Exists(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus"))
                    Directory.CreateDirectory(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus");
                if (!Directory.Exists(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus\\TraderPlusConfig"))
                    Directory.CreateDirectory(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus\\TraderPlusConfig");
                if (!Directory.Exists(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus\\TraderPlusConfig"))
                    Directory.CreateDirectory(ActiveProject.projectFullName + "\\" + ActiveProject.ProfilePath + "\\TraderPlus\\TraderPlusConfig");
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveProject == null) return;
            ActiveProject.Createbackups = checkBox1.Checked;
        }

        public IEnumerable<RemoteFileInfo> fileInfos;
        public Session session;
        public string RemoteRoot;
        public DirectoryInfo ProjectRoot;
        public RemoteFileInfo CurrentRemoteDirectory;
        public DirectoryInfo CurrentProjectDirectory;
        public RemoteFileInfo focusedremoteItem;
        public FileInfo FocusedProjectfile;
        public DirectoryInfo FocusedProjectDirectory;

        private void FTPConnectTSB_Click(object sender, EventArgs e)
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = FTPHostNameTB.Text,
                PortNumber = Convert.ToInt32(FTPPortTB.Text),
                UserName = FTPUSernameTB.Text,
                Password = FTPPasswordTB.Text,
            };
            session = new Session();
            session.FileTransferProgress += SessionFileTransferProgress;
            session.Open(sessionOptions);
            getrootdir();
        }
        private void SFTPConnectTSB1_Click(object sender, EventArgs e)
        {
            try
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = FTPHostNameTB.Text,
                    PortNumber = Convert.ToInt32(FTPPortTB.Text),
                    SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny,
                    UserName = FTPUSernameTB.Text,
                    Password = FTPPasswordTB.Text
                };
                session = new Session();
                session.FileTransferProgress += SessionFileTransferProgress;
                session.Open(sessionOptions);
                getrootdir();
            }
            catch (Exception excep)
            {
                MessageBox.Show("Error: " + excep.Message.ToString());
            }
        }
        private void FTPDisconnectTSB_Click(object sender, EventArgs e)
        {
            if (session != null && session.Opened)
            {
                listView1.Items.Clear();
                session.Close();
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!session.Opened) { MessageBox.Show("No Remote Session....."); return; }
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {

                if (item.Tag is RemoteFileInfo)
                {

                    RemoteFileInfo fileinfo = item.Tag as RemoteFileInfo;
                    DisplayFTPLayout(fileinfo);

                }

            }
        }
        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView2.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                if (item.Tag is DirectoryInfo)
                {
                    DirectoryInfo dirinfo = item.Tag as DirectoryInfo;
                    CurrentProjectDirectory = dirinfo;
                    DisplayDirlayout(dirinfo);
                }
                else if (item.Tag is FileInfo)
                {
                    FileInfo fileinfo = item.Tag as FileInfo;
                }
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = listView1.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    if (focusedItem.Tag is RemoteFileInfo)
                    {
                        RemoteFileInfo fileinfo = focusedItem.Tag as RemoteFileInfo;
                        if (!fileinfo.IsDirectory)
                        {
                            focusedremoteItem = fileinfo;
                            downloadFolderToolStripMenuItem.Visible = false;
                            deleteFileToolStripMenuItem.Visible = true;
                            deleteFolderToolStripMenuItem.Visible = false;
                            downloadFileToolStripMenuItem.Visible = true;
                            FTPDownloadMenu.Show(Cursor.Position);
                        }
                        else if (fileinfo.IsParentDirectory)
                        {
                            focusedremoteItem = fileinfo;
                            downloadFolderToolStripMenuItem.Visible = false;
                            deleteFileToolStripMenuItem.Visible = false;
                            deleteFolderToolStripMenuItem.Visible = false;
                            downloadFileToolStripMenuItem.Visible = false;
                            FTPDownloadMenu.Show(Cursor.Position);
                        }
                        else if (fileinfo.IsDirectory && !fileinfo.IsParentDirectory)
                        {
                            focusedremoteItem = fileinfo;
                            downloadFolderToolStripMenuItem.Visible = true;
                            deleteFileToolStripMenuItem.Visible = false;
                            deleteFolderToolStripMenuItem.Visible = true;
                            downloadFileToolStripMenuItem.Visible = false;
                            FTPDownloadMenu.Show(Cursor.Position);
                        }
                    }
                }
            }
        }
        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = listView2.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    if (focusedItem.Tag is FileInfo)
                    {
                        FocusedProjectfile = focusedItem.Tag as FileInfo;
                        UploadFileToolStripMenuItem.Visible = true;
                        uploadFolderToolStripMenuItem.Visible = false;
                        deleteFIleToolStripMenuItem1.Visible = true;
                        deleteFolderToolStripMenuItem1.Visible = false;
                        FTPUploadMenu.Show(Cursor.Position);
                    }
                    else if (focusedItem.Tag is DirectoryInfo)
                    {
                        FocusedProjectDirectory = focusedItem.Tag as DirectoryInfo;
                        if (CurrentProjectDirectory.Parent.FullName == FocusedProjectDirectory.FullName) { return; }
                        UploadFileToolStripMenuItem.Visible = false;
                        uploadFolderToolStripMenuItem.Visible = true;
                        deleteFIleToolStripMenuItem1.Visible = false;
                        deleteFolderToolStripMenuItem1.Visible = true;
                        FTPUploadMenu.Show(Cursor.Position);
                    }
                }
            }
        }

        private void LoadFileExplorer()
        {
            listView2.Items.Clear();
            if (ActiveProject == null) { return; }
            label1.Text = ActiveProject.projectFullName;
            DirectoryInfo dir = new DirectoryInfo(ActiveProject.projectFullName);
            CurrentProjectDirectory = dir;
            ProjectRoot = dir;
            ListViewItem item = new ListViewItem("..", 0);
            item.Tag = dir;
            listView2.Items.Add(item);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in dirs)
            {
                item = new ListViewItem(subDir.Name, 0);
                item.Tag = subDir;
                listView2.Items.Add(item);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                item = new ListViewItem(file.Name, 1);
                item.Tag = file;
                listView2.Items.Add(item);
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void DisplayDirlayout(DirectoryInfo dirinfo)
        {
            if (dirinfo.Name == ProjectRoot.Parent.Name) { return; }
            string dir = dirinfo.FullName;
            label1.Text = dir;
            listView2.Items.Clear();
            ListViewItem oitem = new ListViewItem("..", 0);
            oitem.Tag = new DirectoryInfo(dirinfo.Parent.FullName);
            listView2.Items.Add(oitem);

            DirectoryInfo[] dirs = dirinfo.GetDirectories();
            foreach (DirectoryInfo subDir in dirs)
            {
                ListViewItem item = new ListViewItem(subDir.Name, 0);
                item.Tag = subDir;
                listView2.Items.Add(item);
            }
            FileInfo[] files = dirinfo.GetFiles();
            foreach (FileInfo file in files)
            {
                ListViewItem item = new ListViewItem(file.Name, 1);
                ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[] { new ListViewItem.ListViewSubItem(item, file.Length.ToString()), new ListViewItem.ListViewSubItem(item, file.LastWriteTime.ToString()) };
                item.SubItems.AddRange(subItems);
                item.Tag = file;
                listView2.Items.Add(item);
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void getrootdir()
        {
            listView1.Items.Clear();
            RemoteDirectoryInfo directory = session.ListDirectory(session.HomePath);
            CurrentRemoteDirectory = directory.Files[0];
            label2.Text = session.HomePath;
            foreach (RemoteFileInfo fileInfo in directory.Files)
            {
                if (fileInfo.IsDirectory)
                {
                    ListViewItem item = new ListViewItem(fileInfo.Name, 0);
                    item.Tag = fileInfo;
                    listView1.Items.Add(item);
                }
            }
            foreach (RemoteFileInfo fileInfo in directory.Files)
            {
                if (!fileInfo.IsDirectory)
                {
                    ListViewItem item = new ListViewItem(fileInfo.Name, 1);
                    item.Tag = fileInfo;
                    listView1.Items.Add(item);
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void DisplayFTPLayout(RemoteFileInfo fileinfo)
        {
            if (!fileinfo.IsDirectory) { return; }
            listView1.Items.Clear();
            string dir = fileinfo.FullName;
            if (dir.EndsWith(".."))
            {
                if (dir.TrimEnd('/').Remove(dir.LastIndexOf('/')) == session.HomePath)
                    dir = dir.Substring(0, fileinfo.FullName.Length - 2);
                else
                {
                    dir = dir.TrimEnd('/').Remove(dir.LastIndexOf('/'));
                    dir = dir.TrimEnd('/').Remove(dir.LastIndexOf('/') + 1);
                }
            }
            CurrentRemoteDirectory = fileinfo;
            label2.Text = dir;
            foreach (RemoteFileInfo fileInfo in session.ListDirectory(dir).Files)
            {
                if (fileInfo.IsDirectory)
                {
                    ListViewItem item = new ListViewItem(fileInfo.Name, 0);
                    item.Tag = fileInfo;
                    listView1.Items.Add(item);
                }
            }
            foreach (RemoteFileInfo fileInfo in session.ListDirectory(dir).Files)
            {
                if (!fileInfo.IsDirectory)
                {
                    ListViewItem item = new ListViewItem(fileInfo.Name, 1);
                    ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[] { new ListViewItem.ListViewSubItem(item, fileInfo.Length.ToString()), new ListViewItem.ListViewSubItem(item, fileInfo.LastWriteTime.ToString()) };
                    item.SubItems.AddRange(subItems);
                    item.Tag = fileInfo;
                    listView1.Items.Add(item);
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void UploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferOptions options = new TransferOptions();
            options.OverwriteMode = OverwriteMode.Overwrite;
            session.PutFileToDirectory(FocusedProjectfile.FullName, CurrentRemoteDirectory.FullName, false, options);
            DisplayFTPLayout(CurrentRemoteDirectory);
        }
        private void uploadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (session.FileExists(CurrentRemoteDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(FocusedProjectDirectory.FullName)))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                session.CreateDirectory(CurrentRemoteDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(FocusedProjectDirectory.FullName));
                Console.WriteLine("The directory was created successfully");
            }
            session.PutFilesToDirectory(FocusedProjectDirectory.FullName, CurrentRemoteDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(FocusedProjectDirectory.FullName)).Check();
            DisplayFTPLayout(CurrentRemoteDirectory);
        }
        private void deleteFIleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            File.Delete(FocusedProjectfile.FullName);
            DisplayDirlayout(CurrentProjectDirectory);
        }
        private void deleteFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Directory.Delete(FocusedProjectDirectory.FullName, true);
            DisplayDirlayout(CurrentProjectDirectory);
        }
        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferOptions option = new TransferOptions();
            option.OverwriteMode = OverwriteMode.Overwrite;
            session.GetFileToDirectory(focusedremoteItem.FullName, CurrentProjectDirectory.FullName, false, option);
            DisplayDirlayout(CurrentProjectDirectory);
        }
        private void downloadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName)))
                Directory.CreateDirectory(CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName));
            session.GetFilesToDirectory(focusedremoteItem.FullName, CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName));
            DisplayDirlayout(CurrentProjectDirectory);

        }
        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            session.RemoveFile(focusedremoteItem.FullName);
            DisplayFTPLayout(CurrentRemoteDirectory);
        }
        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            session.RemoveFiles(focusedremoteItem.FullName);
            DisplayFTPLayout(CurrentRemoteDirectory);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectProfileTB.Text = fb.SelectedPath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectMissionFolderTB.Text = fb.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ProjectFolderTB.Text = fb.SelectedPath;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            Project p = listBox1.SelectedItem as Project;
            EditProjectNameTB.Text = p.ProjectName;
            EditProfileNameTB.Text = p.ProfilePath;
            EditPathTB.Text = p.projectFullName;
            EditMapPathTB.Text = p.MapPath;
            EditMapSizeNUD.Value = p.MapSize;
            EditMissionPathTB.Text = p.mpmissionpath;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {

            Project p = listBox1.SelectedItem as Project;
            if (ActiveProject == p)
            {
                MessageBox.Show("Cant edit active projects.....");
                return;
            }
            p.ProjectName = EditProjectNameTB.Text;
            p.ProfilePath = EditProfileNameTB.Text;
            p.projectFullName = EditPathTB.Text;
            p.MapPath = EditMapPathTB.Text;
            p.MapSize = (int)EditMapSizeNUD.Value;
            p.mpmissionpath = EditMissionPathTB.Text;
            projects.SaveProject(false, true);
            listBox1.Invalidate();
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count <= 0) return;
            ListViewItem item = listView3.SelectedItems[0];
            int index = listView3.SelectedIndices[0];
            if (item != null)
            {
                if (item.Tag is Asset)
                {
                    Asset asset = (Asset)item.Tag;
                    Console.WriteLine("Downloading Map Addon.....");
                    string zipfile = Application.StartupPath + "\\Maps\\" + Path.GetFileName(asset.browser_download_url);
                    using (var client = new WebClient())
                    {
                        Console.WriteLine("Downloading Zip file......");
                        client.DownloadFile(asset.browser_download_url, zipfile);
                    }
                    using (ArchiveFile archiveFile = new ArchiveFile(zipfile, Application.StartupPath + "\\lib\\7z.dll"))
                    {
                        foreach (Entry entry in archiveFile.Entries)
                        {
                            Console.WriteLine(entry.FileName);

                            // extract to file
                            entry.Extract(Application.StartupPath + "\\Maps\\" + entry.FileName);

                        }
                    }
                    File.Delete(zipfile);
                    item.SubItems.Add("Installed");
                    listView3.Items[index] = item;
                    listView3.Refresh();

                    Console.WriteLine("Updating MapSizes.txt");
                    using (var client = new WebClient())
                    {
                        GitHub info = getavaiableMapAddons();
                        Asset ass = info.assets.Find(x => x.name == "MapSizes.txt");
                        client.DownloadFile(ass.browser_download_url, "Maps/MapSizes.txt");
                    }
                

                }
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Updating MapSizes.txt");
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading Zip file......");
                GitHub info = getavaiableMapAddons();
                Asset ass = info.assets.Find(x => x.name == "MapSizes.txt");
                client.DownloadFile(ass.browser_download_url, "Maps/MapSizes.txt");
            }
        }
        private void LoadFTPInfo()
        {
            string infostring = Properties.Settings.Default.SFTPINFO;
            if (infostring == "") return;
            SecureString password = DecryptString(infostring);
            string readable = ToInsecureString(password);
            SFTPInfoList ftpinfolist = JsonSerializer.Deserialize<SFTPInfoList>(readable);
            SFTPInfo info = ftpinfolist.infolist.FirstOrDefault(x => x.projectname == ActiveProject.ToString());
            if (info != null)
            {
                FTPHostNameTB.Text = info.url;
                FTPPortTB.Text = info.port;
                FTPUSernameTB.Text = info.login;
                FTPPasswordTB.Text = info.password;
            }
            else
            {
                FTPHostNameTB.Text = "";
                FTPPortTB.Text = "";
                FTPUSernameTB.Text = "";
                FTPPasswordTB.Text = "";
            }
        }
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            SFTPInfo ftpInfo = new SFTPInfo()
            {
                projectname = ActiveProject.ToString(),
                url = FTPHostNameTB.Text,
                port = FTPPortTB.Text,
                login = FTPUSernameTB.Text,
                password = FTPPasswordTB.Text,
            };
            string infostring = Properties.Settings.Default.SFTPINFO;
            SFTPInfoList ftpinfolist = new SFTPInfoList()
            {
                infolist = new BindingList<SFTPInfo>()
            };
            if (infostring != "")
            {
                SecureString password = DecryptString(infostring);
                string readable = ToInsecureString(password);
                ftpinfolist = JsonSerializer.Deserialize<SFTPInfoList>(readable);
                SFTPInfo info = ftpinfolist.infolist.FirstOrDefault(x => x.projectname == ftpInfo.projectname);
                while (info != null)
                {
                    
                    if (info != null)
                    {
                        ftpinfolist.infolist.Remove(info);
                    }
                    info = ftpinfolist.infolist.FirstOrDefault(x => x.projectname == ftpInfo.projectname);
                }
                ftpinfolist.infolist.Add(ftpInfo);
            }
            else
            {
                ftpinfolist.infolist.Add(ftpInfo);
            }
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string newinfolist = JsonSerializer.Serialize(ftpinfolist, options);
            Properties.Settings.Default.SFTPINFO = EncryptString(ToSecureString(newinfolist));
            Properties.Settings.Default.Save();
            Console.WriteLine("INFO: FTP/SFTP Details saved.....");
        }
        static byte[] entropy = Encoding.Unicode.GetBytes("Sh4wm1nat0r5 3d1t0r 15 4he B3st");
        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }
        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        private void FTPPasswordTB_Click(object sender, EventArgs e)
        {
            
        }

        private void FTPPasswordTB_TextChanged(object sender, EventArgs e)
        {
            if(FTPPasswordTB.Text == "")
                toolStripButton2.Visible = false;
            else
                toolStripButton2.Visible = true;
        }


    }
    public class SFTPInfoList
    {
        public BindingList<SFTPInfo> infolist { get; set; }

    }
    public class SFTPInfo
    {
        public string projectname { get; set; }
        public string url { get; set; }
        public string login { get; set; }
        public string port { get; set; }
        public string password { get; set; }
    }
}
