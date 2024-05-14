using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddNewQuestID : DarkForm
    {
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

        public List<int> NumberofquestsIDs { get; set; }
        public int SelectedID { get; set; }

        public AddNewQuestID()
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


        private void darkButton1_Click(object sender, EventArgs e)
        {
            SelectedID = (int)numericUpDown1.Value;
        }

        private void darkButton2_Click(object sender, EventArgs e)
        {
            int initialID = (int)numericUpDown1.Value;
            if (NumberofquestsIDs.Contains(initialID))
            {
                MessageBox.Show("That quest ID is allready in use, Please select a different ID");
                darkButton1.Enabled = false;
            }
            else
                darkButton1.Enabled = true;
        }

        private void AddNewQuestID_Load(object sender, EventArgs e)
        {
            List<int> result = new List<int>();
            if (NumberofquestsIDs.Count > 0)
            {
                result = Enumerable.Range(1, NumberofquestsIDs.Max() + 1).Except(NumberofquestsIDs).ToList();
                result.Sort();
                numericUpDown1.Value = result[0];
            }
            else
                numericUpDown1.Value = 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            darkButton1.Enabled = false;
        }
    }
}
