using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddNewTypes : DarkForm
    {
        public Project currentproject { get;  set; }
        public string TypesName { get; private set; }
        public string CustomLocation { get; private set; }
        public bool newlocation { get; set; }
        public List<string> modtypes;

        public AddNewTypes()
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
            if(!newlocation)
            {
                darkButton1.Visible = false;
                CustomFolderTB.Visible = false;
                textBox3.Visible = false;
                richTextBox1.Size = new Size(314, 401);
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
             modtypes = richTextBox1.Lines.ToList();
        }
    }
}
