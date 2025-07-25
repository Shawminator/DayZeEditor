using DarkUI.Forms;
using DayZeLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddnewTerjeRespawnOption : DarkForm
    {
        public string Selectedcondition { get; set; }
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

        public TerjeRespawnOptions options { get; internal set; }

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
        public AddnewTerjeRespawnOption()
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
            listBox1.Items.Clear();
            if (options.SafeRadius == null)
                listBox1.Items.Add("Safe Radius");
            if (options.MapImage == null)
                listBox1.Items.Add("Map Image");
            if (options.MapRender == null)
                listBox1.Items.Add("Map Render");
            if (options.PlayerStats == null)
                listBox1.Items.Add("Player Stats");
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.SelectedItems)
            {
                Selectedcondition = item.ToString();
            }
        }
    }
}
