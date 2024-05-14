using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddNeweventFile : DarkForm
    {
        public Project currentproject { get; set; }
        public string TypesName { get; private set; }
        public string CustomLocation { get; private set; }
        public bool newlocation { get; set; }
        public List<string> modtypes;

        public AddNeweventFile()
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
        private void AddNewTypes_Load(object sender, EventArgs e)
        {
            if (!newlocation)
            {
                darkButton1.Visible = false;
                CustomFolderTB.Visible = false;
                textBox3.Visible = false;
                darkLabel7.Visible = false;
                darkLabel2.Visible = false;
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            string startingpath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath;
            fb.SelectedPath = startingpath;
            if (fb.ShowDialog() == DialogResult.OK)
            {
                CustomFolderTB.Text = fb.SelectedPath;
            }
        }
        private void CustomFolderTB_TextChanged(object sender, EventArgs e)
        {
            CustomLocation = CustomFolderTB.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            TypesName = textBox3.Text;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
        }

        public string SetTitle
        {
            set { TitleLabel.Text = value; }
        }
        public string SetFolderName
        {
            set { darkLabel2.Text = value; }
        }
        public string settype
        {
            set { darkLabel7.Text = value; }
        }
        public string setbuttontest
        {
            set { darkButton2.Text = value; }
        }

    }
}
