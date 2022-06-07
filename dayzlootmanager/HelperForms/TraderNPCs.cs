using DarkUI.Forms;
using DayZeLib;
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

namespace DayZeEditor
{
    public partial class TraderNPCs : DarkForm
    {
        public string selectedNPC { get; set; }
        public bool isTraderplus { get; set; }
        public TraderNPCs()
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

        private void TraderNPCs_Load(object sender, EventArgs e)
        {
            if (isTraderplus)
            {
                String[] npcs = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\TraderPlusNPCs.txt");
                listBox1.DisplayMember = "Name";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = npcs.ToList();
            }
            else
            {
                String[] npcs = File.ReadAllLines(Application.StartupPath + "\\traderNPCs\\NPCs.txt");
                listBox1.DisplayMember = "Name";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = npcs.ToList();
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            selectedNPC = listBox1.GetItemText(listBox1.SelectedItem);
        }
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
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

    }
}
