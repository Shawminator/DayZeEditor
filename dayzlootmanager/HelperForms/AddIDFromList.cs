using DarkUI.Forms;
using DayZeLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddIDFromList : DarkForm
    {
        public BindingList<MPG_Spawner_PointConfig> IDlist { get; set; }
        public BindingList<MPG_Spawner_PointConfig> Selectedids { get; set; }
        public string SetLabel
        {
            get
            {
                return darkLabel1.Text;
            }
            set
            {
                darkLabel1.Text = value;
            }
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
        public AddIDFromList()
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

        private void AddIDFromList_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = IDlist;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            Selectedids = new BindingList<MPG_Spawner_PointConfig>();
            if (listBox1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No IDS Were selected");
                return;
            };
            foreach (var item in listBox1.SelectedItems)
            {
                Selectedids.Add(item as MPG_Spawner_PointConfig);
            }
        }
    }
}
