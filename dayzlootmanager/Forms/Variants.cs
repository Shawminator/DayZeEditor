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
    public partial class Variants : DarkForm
    {
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

        public marketItem CurrentItem { get; set; }
        public List<marketItem> ListOfpossibleVariants { get; set; }
        public List<marketItem> returneditems { get; set; }

        public Variants()
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

        private void Variants_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = ListOfpossibleVariants;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            returneditems = listBox1.SelectedItems.Cast<marketItem>().ToList();
        }
    }
}
