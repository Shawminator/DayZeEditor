using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class addKillRewardWeaponBox : DarkForm
    {
        public string titellabel { get; set; }
        public KillReward_KillRewardWEAPONBOXSettings SelectedWeaponBox { get; set; }
        public BindingList<KillReward_KillRewardWEAPONBOXSettings> WeaponBoxes { get; set; }

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
        public addKillRewardWeaponBox()
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

        private void AddQuestNPC_Load(object sender, EventArgs e)
        {
            QuestNPCLB.DisplayMember = "DisplayName";
            QuestNPCLB.ValueMember = "Value";
            QuestNPCLB.DataSource = WeaponBoxes;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            SelectedWeaponBox = QuestNPCLB.SelectedItem as KillReward_KillRewardWEAPONBOXSettings;
        }
    }
}
