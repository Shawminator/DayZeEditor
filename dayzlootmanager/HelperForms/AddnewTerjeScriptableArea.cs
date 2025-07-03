using DarkUI.Forms;
using DayZeLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddnewTerjeScriptableArea : DarkForm
    {
        public string SelectedAreaType { get; set; }
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
        public AddnewTerjeScriptableArea()
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
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.SelectedItems)
            {
                switch(item.ToString())
                {
                    case "Psionic area":
                        SelectedAreaType = "TerjePsionicScriptableArea";
                        break;
                    case "Radioactive area":
                        SelectedAreaType = "TerjeRadioactiveScriptableArea";
                        break;
                    case "Skills experience modifier area":
                        SelectedAreaType = "TerjeExperienceModScriptableArea";
                        break;
                }
            }
        }
    }
}
