using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace DayZeEditor
{
    public partial class NewProjectFTP : DarkForm
    {
        public bool hideConsole
        {
            get
            {
                return IsConsoleCB.Visible;
            }
            set
            {
                IsConsoleCB.Visible = value;
            }
        }

        public bool showDriveletter
        {
            set
            {
                darkLabel3.Visible = value;
                darkComboBox1.Visible = value;
                if (value == true)
                {
                    List<string> alpha = "DEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()).ToList();
                    List<string> DriveList = Environment.GetLogicalDrives().ToList();
                    for(int i = 0; i < DriveList.Count; i++)
                    {
                        DriveList[i] = DriveList[i].Replace(":\\", "");
                    }
                    List<string> result = alpha.Except(DriveList).ToList();
                    darkComboBox1.DataSource = result;
                }
            }
        }
        public string GetdriveLetter
        {
            get
            {
                return darkComboBox1.GetItemText(darkComboBox1.SelectedItem);
            }
        }
        

        public Session session { get; set; }
        public string CurrentRemoteDirectory { get; private set; }
        public string RemoteRoot { get; private set; }
        public string ProfileDirecrtory { get; private set; }
        public string MpMissionDirectory { get; private set; }
        public bool Isconsole { get; set; }

        public NewProjectFTP()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
        }

        private void NewProjectFTP_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            RemoteDirectoryInfo directory = session.ListDirectory(session.HomePath);
            CurrentRemoteDirectory = session.HomePath;
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
                    CurrentRemoteDirectory = fileinfo.FullName;
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
                    foreach (RemoteFileInfo fileInfo in session.ListDirectory(dir).Files)
                    {
                        if (fileInfo.IsDirectory)
                        {
                            item = new ListViewItem(fileInfo.Name, 0);
                            item.Tag = fileInfo;
                            listView1.Items.Add(item);
                        }
                    }
                    foreach (RemoteFileInfo fileInfo in session.ListDirectory(dir).Files)
                    {
                        if (!fileInfo.IsDirectory)
                        {
                            item = new ListViewItem(fileInfo.Name, 1);
                            ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[] { new ListViewItem.ListViewSubItem(item, fileInfo.Length.ToString()), new ListViewItem.ListViewSubItem(item, fileInfo.LastWriteTime.ToString()) };
                            item.SubItems.AddRange(subItems);
                            item.Tag = fileInfo;
                            listView1.Items.Add(item);
                        }
                    }
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            darkTextBox1.Text = CurrentRemoteDirectory;
        }

        private void darkButton2_Click(object sender, EventArgs e)
        {
            darkTextBox2.Text = CurrentRemoteDirectory;
        }

        private void darkTextBox1_TextChanged(object sender, EventArgs e)
        {
            ProfileDirecrtory = darkTextBox1.Text;
        }

        private void darkTextBox2_TextChanged(object sender, EventArgs e)
        {
            MpMissionDirectory = darkTextBox2.Text;
        }

        private void IsConsoleCB_CheckedChanged(object sender, EventArgs e)
        {
            if(IsConsoleCB.Checked)
            {
                Isconsole = true;
                darkTextBox1.Visible = false;
            }
            else
            {
                Isconsole = false;
                darkTextBox1.Visible = true;
            }
        }

        private void darkButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
