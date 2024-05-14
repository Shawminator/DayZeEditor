using DarkUI.Forms;
using DayZeLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddfromLoadoutList : DarkForm
    {
        public BindingList<string> LoadoutLIst { get; set; }
        public BindingList<string> SelectedLoadouts { get; set; }
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
        public AddfromLoadoutList()
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
            BindingList<string> list = new BindingList<string>();
            foreach (string l in LoadoutLIst)
            {
                list.Add(l + ".json");
            }
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = list;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            SelectedLoadouts = new BindingList<string>();
            if (listBox1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("No IDS Were selected");
                return;
            };
            foreach (var item in listBox1.SelectedItems)
            {
                SelectedLoadouts.Add(item.ToString());
            }
        }
    }
}
