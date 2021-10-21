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
    public partial class AddfromPredefinedWeapons : DarkForm
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

        public BindingList<LCPredefinedWeapons> LCPredefinedWeapons { get; set; }
        public BindingList<LootCategories> LootCategories { get; set; }
        public string predefweapon { get; set; }
        public bool ispredefinedweapon { get; set; }
        public bool isLootList { get; set; }
        public string titellabel { get; set; }
        public bool isLootchest { get; set; }
        public AddfromPredefinedWeapons()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
            TitleLabel.Text = titellabel;
        }

        private void AddfromPredefinedWeapons_Load(object sender, EventArgs e)
        {
            if (ispredefinedweapon)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = LCPredefinedWeapons;
            }
            else if(isLootList)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = LootCategories;
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {

        }

        private void LCPredefinedWeaponsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ispredefinedweapon)
            {
                LCPredefinedWeapons predefweaponclass = LCPredefinedWeaponsLB.SelectedItem as LCPredefinedWeapons;
                predefweapon = predefweaponclass.defname;
            }
            else if (isLootList)
            {
                LootCategories predefweaponclass = LCPredefinedWeaponsLB.SelectedItem as LootCategories;
                predefweapon = predefweaponclass.name;
            }
            else if (isLootchest)
            {
                predefweapon = LCPredefinedWeaponsLB.GetItemText(LCPredefinedWeaponsLB.SelectedItem);
            }
        }
    }
}
