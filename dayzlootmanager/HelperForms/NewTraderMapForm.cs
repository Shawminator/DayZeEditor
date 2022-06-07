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
    public partial class NewTraderMapForm : DarkForm
    {
        public TraderZones Zones { get; set; }
        public TraderModelMapping TraderMaps  { get; set; }
        public TradersList Traders { get; set; }

        public Zones SelectedZone { get; set; }
        public Traders selectedTrader { get; set; }
        public string NPC { get; set; }

        public NewTraderMapForm()
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
        private void NewTraderMapForm_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = Zones.ZoneList;

            listBox16.DisplayMember = "Name";
            listBox16.ValueMember = "Value";
            listBox16.DataSource = Traders.Traderlist;

            NPC = "";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedZone = listBox1.SelectedItem as Zones;
        }

        private void listBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTrader = listBox16.SelectedItem as Traders;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            NPC = textBox15.Text;
        }

        private void darkButton26_Click(object sender, EventArgs e)
        {
            TraderNPCs newtrader = new TraderNPCs();
            if(newtrader.ShowDialog() == DialogResult.OK)
            {
                textBox15.Text = newtrader.selectedNPC;
            }

        }
    }
}
