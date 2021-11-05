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
            comboBox1.DataSource = currentproject.limitfefinitions.lists.categories.category;
            if (!newlocation)
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
            if(tabControl1.SelectedIndex == 0)
                modtypes = richTextBox1.Lines.ToList();
            else
            {
                StringBuilder sb = new StringBuilder();
                category c = comboBox1.SelectedItem as category;
                foreach (string line in richTextBox2.Lines)
                {
                    sb.Append("<type name=\"" + line + "\">" + Environment.NewLine);
                    sb.Append("<nominal>" + numericUpDown1.Value.ToString() + "</nominal>" + Environment.NewLine);
                    sb.Append("<lifetime>" + numericUpDown3.Value.ToString() + "</lifetime>" + Environment.NewLine);
                    sb.Append("<restock>" + numericUpDown4.Value.ToString() + "</restock>" + Environment.NewLine);
                    sb.Append("<min>" + numericUpDown2.Value.ToString() + "</min>" + Environment.NewLine);
                    sb.Append("<quantmin>" + numericUpDown5.Value.ToString() + "</quantmin>"  + Environment.NewLine);
                    sb.Append("<quantmax>" + numericUpDown6.Value.ToString() + "</quantmax>"  + Environment.NewLine);
                    sb.Append("<cost>" + numericUpDown7.Value.ToString() + "</cost>"  + Environment.NewLine);
                    sb.Append("<flags count_in_cargo = \"0\" count_in_hoarder = \"0\" count_in_map = \"1\" count_in_player = \"0\" crafted = \"0\" deloot = \"0\"/>"  + Environment.NewLine);
                    sb.Append("<category name=\"" + c.ToString() + "\"/>"  + Environment.NewLine);
                    sb.Append("</type>");
                }
                modtypes = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
        }
    }
}
