using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DayZeLib;
using System.Net;
using System.Text.RegularExpressions;
using WinSCP;
using System.Threading;

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
            if (tabControl1.SelectedIndex == 0)
                ProjectTSB.Checked = true;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                FTPTSB.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    FTPTSB.Checked = false;

                    break;
                case 1:
                    ProjectTSB.Checked = false;
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
            LoadProjectstoList();
            getActiveProject();
            LoadFileExplorer();
            tabControl1.ItemSize = new Size(0, 1);
        }
        public void getActiveProject()
        {
            ActiveProject = projects.getActiveProject();
            if (ActiveProject != null)
            {
                darkLabel4.Text = projects.ActiveProject;
            }
            if (ActiveProject != null && ActiveProject.usingDrJoneTrader)
                radioButton3.Checked = true;
            else if (ActiveProject != null && ActiveProject.usingexpansionMarket)
                radioButton2.Checked = true;
            else if (ActiveProject != null && ActiveProject.usingtraderplus)
                radioButton4.Checked = true;
            else if (ActiveProject != null)
                radioButton1.Checked = true;
            
        }
        private void LoadProjectstoList()
        {
            listBox1.Items.Clear();
            foreach (Project p in projects.Projects)
            {
                listBox1.Items.Add(p.ProjectName);
            }
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
        private void darkButton2_Click(object sender, EventArgs e)
        {
            string ProjectFolder = ProjectFolderTB.Text;
            string ProjectName = ProjectNameTB.Text;
            if(ProjectFolder == "" || ProjectName == "")
            {
                MessageBox.Show("Please select both a Project name and a directory where to save it.");
                return;
            }
            string ProjectPath = ProjectFolder + "\\" + ProjectName;
            Directory.CreateDirectory(ProjectPath);

            string projecttype = ProjectTypeComboBox.GetItemText(ProjectTypeComboBox.SelectedItem);
            if(projecttype == "Blank project")
            {
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
                if (mpmissionpath.ToLower().Contains("expansion"))
                {
                    Directory.CreateDirectory(Pprofilefolder + "\\ExpansionMod");
                    project.isExpansion = true;
                }
                projects.addtoprojects(project, false);
                LoadProjectstoList();
                MessageBox.Show("Blank Project created, Please Close the editor and populate the missions files before trying to load this project...");
            }
            else if (projecttype == "Exisitng mission files")
            {
                if (ProjectProfileTB.Text == "" || ProjectMissionFolderTB.Text == "")
                {
                    MessageBox.Show("Please select both a Profile name and a mpmission folder.");
                    return;
                }
                string missionsfolder = ProjectMissionFolderTB.Text;
                string profilefolder = ProjectProfileTB.Text;
                string mpmissionpath = Path.GetFileName(missionsfolder);
                string PmissionFolder = ProjectPath + "\\mpmissions\\" + Path.GetFileName(missionsfolder);
                string Pprofilefolder = ProjectPath + "\\" + profilefolder;
               
                Directory.CreateDirectory(PmissionFolder);
                Directory.CreateDirectory(Pprofilefolder);
               
                Helper.CopyFilesRecursively(missionsfolder, PmissionFolder);


                Project project = new Project();
                project.AddNames(ProjectName, ProjectPath);
                project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                project.mpmissionpath = mpmissionpath;
                project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                project.ProfilePath = profilefolder;
                if (mpmissionpath.ToLower().Contains("expansion"))
                {
                    Directory.CreateDirectory(Pprofilefolder + "\\ExpansionMod");
                    project.isExpansion = true;
                }
                projects.addtoprojects(project);
                LoadProjectstoList();
                SetActiveProject(ProjectName);
            }
            else if (projecttype == "Get from FTP")
            {
                try
                {
                    if (session == null || !session.Opened)
                    {
                        if(FTPHostNameTB.Text == "" || FTPHostNameTB.Text == null)
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
                        session = new Session();
                        session.Open(sessionOptions);
                    }
                }
                catch
                {
                    MessageBox.Show("Please check connection info.....");
                    return;
                }
                NewProjectFTP ftpproject = new NewProjectFTP();
                ftpproject.session = session;
                DialogResult result = ftpproject.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Thread load = new Thread(new ThreadStart(showLoading));
                    load.Start();
                    if (ftpproject.Isconsole)
                    {
                        string mpmissiondirectory = ftpproject.MpMissionDirectory;
                        string mpmissionpath = Path.GetFileName(mpmissiondirectory);

                        Directory.CreateDirectory(ProjectPath + "\\mpmissions\\" + mpmissionpath);
                        session.GetFilesToDirectory(mpmissiondirectory, ProjectPath + "\\mpmissions\\" + mpmissionpath);

                        Project project = new Project();
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

                        Directory.CreateDirectory(ProjectPath + "\\" + profile);
                        Directory.CreateDirectory(ProjectPath + "\\mpmissions\\" + mpmissionpath);
                        session.GetFilesToDirectory(profiledir, ProjectPath + "\\" + profile);
                        session.GetFilesToDirectory(mpmissiondirectory, ProjectPath + "\\mpmissions\\" + mpmissionpath);

                        Project project = new Project();
                        project.AddNames(ProjectName, ProjectPath);
                        project.MapSize = Getmapsizefrommissionpath(mpmissionpath);
                        project.mpmissionpath = mpmissionpath;
                        project.MapPath = "\\Maps\\" + mpmissionpath.ToLower().Split('.')[1] + "_Map.png";
                        project.ProfilePath = profile;
                        if (mpmissionpath.ToLower().Contains("expansion"))
                            project.isExpansion = true;
                        projects.addtoprojects(project);
                    }
                    LoadProjectstoList();
                    SetActiveProject(ProjectName);
                    load.Abort();
                }
            }
        }
        private void showLoading()
        {
            LoadingScreen loading = new LoadingScreen();
            loading.ShowDialog();
        }
        private int Getmapsizefrommissionpath(string mpmissionpath)
        {
            switch (mpmissionpath.ToLower())
            {
                case "dayzoffline.iztek":
                case "expansion.iztek":
                    return 8192;
                case "dayzoffline.chernarusplus":
                case "expansion.chernarusplus":
                case "expansion.chernarusplusgloom":
                case "empty.banov":
                case "expansion.banov":
                    return 15360;
                case "expansionhard.namalsk":
                case "expansionregular.namalsk":
                case "dayzoffline.enoch":
                case "expansion.enoch":
                case "expansion.enochgloom":
                case "expansion.takistanplus":
                case "hardcore.namalsk":
                case "regular.namalsk":
                case "expansion.esseker":
                    return 12800;
                case "expansion.deerisle":
                case "empty.deerisle":
                    return 16384;
                case "expansion.rostow":
                case "offline.rostow":
                    return 14336;
                default:
                    return 0;
            }
        }
        private void ProjectTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string projecttype = ProjectTypeComboBox.GetItemText(ProjectTypeComboBox.SelectedItem);
            if (projecttype == "Get from FTP")
            {
                darkLabel12.Visible = false;
                darkLabel6.Visible = false;
                ProjectProfileTB.Visible = false;
                ProjectMissionFolderTB.Visible = false;
                button3.Visible = false;
                darkButton2.Location = new Point(360, 105);
            }
            else if (projecttype == "Blank project")
            {
                darkLabel6.Text = "Exact Name of mission";
                button3.Visible = false;
                darkLabel12.Visible = true;
                darkLabel6.Visible = true;
                ProjectProfileTB.Visible = true;
                ProjectMissionFolderTB.Visible = true;
                ProjectMissionFolderTB.Size = new Size(614, 20);
                darkButton2.Location = new Point(360, 156);
            }
            else
            {
                darkLabel6.Text = "Mission Folder to use";
                darkLabel12.Visible = true;
                darkLabel6.Visible = true;
                ProjectProfileTB.Visible = true;
                ProjectMissionFolderTB.Visible = true;
                ProjectMissionFolderTB.Size = new Size(587,20);
                button3.Visible = true;
                darkButton2.Location = new Point(360, 156);
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem == null) { return; }
            string profilename = listBox1.GetItemText(listBox1.SelectedItem);
            SetActiveProject(profilename);
        }
        private void SetActiveProject(string profilename)
        {
            projects.SetActiveProject(profilename);
            darkLabel4.Text = projects.ActiveProject;
            projects.getActiveProject().seteconomycore();
            projects.getActiveProject().seteconomydefinitions();
            projects.getActiveProject().setuserdefinitions();
            projects.getActiveProject().setplayerspawns();
            projects.getActiveProject().SetEvents();
            projects.getActiveProject().SetGlobals();
            projects.getActiveProject().setVanillaTypes();
            projects.getActiveProject().SetModListtypes();
            projects.getActiveProject().SetTotNomCount();
            getActiveProject();
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            projects.SaveProject();
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All Projects files, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string profilename = listBox1.GetItemText(listBox1.SelectedItem);
                listBox1.Items.Remove(profilename);
                Project p = projects.getprojectfromname(profilename);
                projects.DeleteProject(profilename);
                Directory.Delete(p.projectFullName, true);
                if (ActiveProject == p)
                {
                    ActiveProject = null;
                    projects.ActiveProject = "";
                }
                projects.SaveProject();
                MessageBox.Show("Project Removed....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            //SessionOptions sessionOptions = new SessionOptions
            //{
            //    Protocol = Protocol.Ftp,
            //    HostName = "51.195.133.137",
            //    PortNumber = 8821,
            //    UserName = "craigs3",
            //    Password = "@%$.!c6yHUG9",
            //};
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = FTPHostNameTB.Text,
                PortNumber = Convert.ToInt32(FTPPortTB.Text),
                UserName = FTPUSernameTB.Text,
                Password = FTPPasswordTB.Text,
            };
            session = new Session();
            session.Open(sessionOptions);
            getrootdir();
        }
        private void FTPDisconnectTSB_Click(object sender, EventArgs e)
        {
            if (session.Opened)
            {
                listView1.Items.Clear();
                session.Close();
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(!session.Opened) { MessageBox.Show("No Remote Session.....");return; }
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
            if(ActiveProject == null) { return; }
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
            RemoteDirectoryInfo directory = session.ListDirectory("/");
            CurrentRemoteDirectory = directory.Files[0];
            label2.Text = "/";
            RemoteRoot = directory.Files[0].FullName;
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
                if (dir == RemoteRoot)
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
            session.PutFilesToDirectory(FocusedProjectDirectory.FullName, CurrentRemoteDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(FocusedProjectDirectory.FullName));
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
            session.GetFileToDirectory(focusedremoteItem.FullName, CurrentProjectDirectory.FullName,false, option);
            DisplayDirlayout(CurrentProjectDirectory);
        }
        private void downloadFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName)))
                Directory.CreateDirectory(CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName));
            session.GetFilesToDirectory(focusedremoteItem.FullName, CurrentProjectDirectory.FullName + "//" + Path.GetFileNameWithoutExtension(focusedremoteItem.FullName) );
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

    }
}
