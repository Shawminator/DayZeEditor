using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddfromPredefinedItems : DarkForm
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
        public BindingList<capareLPdefinedItems> RHLPdefinedItems { get; set; }
        public BindingList<CapareLPLootItemSet> CapareLPItemSets { get; set; }
        public BindingList<LootCategories> LootCategories { get; set; }
        public BindingList<caparelploottable> LootTables { get; set; }
        public BindingList<caparelprewardtable> Rhlprewardtable { get; set; }
        public BindingList<caparelootboxconfig> Rhlootboxconfig { get; set; }
        public BindingList<Lootpool> UtopiaAirdropLootPools { get; set; }
        public BindingList<string> Stringlist { get; set; }
        public string predefweapon { get; set; }
        public List<string> ReturnList { get; set; }
        public bool ispredefinedweapon { get; set; }
        public bool isRHPredefinedWeapon { get; set; }
        public bool isLootList { get; set; }
        public bool isRHTableList { get; set; }
        public bool isRewardTable { get; set; }
        public string titellabel { get; set; }
        public string _titlellabel
        {
            set { TitleLabel.Text = value; }
        }
        public bool isLootchest { get; set; }
        public bool isLootBoxList { get; set; }
        public bool isUtopiaAirdroplootPools { get; set; }
        public AddfromPredefinedItems()
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
            if(Stringlist != null && Stringlist.Count > 0)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = Stringlist;
                ReturnList = new List<string>();
            }
            else if (ispredefinedweapon)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = LCPredefinedWeapons;
                ReturnList = new List<string>();
            }

            else if (isUtopiaAirdroplootPools)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = UtopiaAirdropLootPools;
                ReturnList = new List<string>();
            }
            else if (isRHPredefinedWeapon)
            {
                if (RHLPdefinedItems != null && CapareLPItemSets == null)
                {
                    LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                    LCPredefinedWeaponsLB.ValueMember = "Value";
                    LCPredefinedWeaponsLB.DataSource = RHLPdefinedItems;
                }
                else if (RHLPdefinedItems == null && CapareLPItemSets != null)
                {
                    LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                    LCPredefinedWeaponsLB.ValueMember = "Value";
                    LCPredefinedWeaponsLB.DataSource = CapareLPItemSets;
                }
                ReturnList = new List<string>();
            }
            else if (isLootList)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = LootCategories;
                ReturnList = new List<string>();
            }
            else if (isRHTableList)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = LootTables;
                ReturnList = new List<string>();
            }
            else if (isRewardTable)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = Rhlprewardtable;
                ReturnList = new List<string>();
            }
            else if (isLootBoxList)
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = Rhlootboxconfig;
                ReturnList = new List<string>();
            }
            else
            {
                LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
                LCPredefinedWeaponsLB.ValueMember = "Value";
                LCPredefinedWeaponsLB.DataSource = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\CJLootboxContainers.txt").ToList());
                ReturnList = new List<string>();
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if(Stringlist != null && Stringlist.Count > 0)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    ReturnList.Add(item.ToString());
                }
            }
            else if (ispredefinedweapon)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    LCPredefinedWeapons predefweaponclass = item as LCPredefinedWeapons;
                    ReturnList.Add(predefweaponclass.defname);
                }
            }
            else if (isUtopiaAirdroplootPools)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    Lootpool predefweaponclass = item as Lootpool;
                    ReturnList.Add(predefweaponclass.lootPoolName);
                }
            }
            else if (isRHPredefinedWeapon)
            {
                if (RHLPdefinedItems != null && CapareLPItemSets == null)
                {
                    foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                    {
                        capareLPdefinedItems predefweaponclass = item as capareLPdefinedItems;
                        ReturnList.Add(predefweaponclass.DefineName);
                    }
                }
                else if (RHLPdefinedItems == null && CapareLPItemSets != null)
                {
                    foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                    {
                        CapareLPLootItemSet predefweaponclass = item as CapareLPLootItemSet;
                        ReturnList.Add(predefweaponclass.SetName);
                    }
                }
            }
            else if (isLootList)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    LootCategories predefweaponclass = item as LootCategories;
                    ReturnList.Add(predefweaponclass.name);
                }
            }
            else if (isRHTableList)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    caparelploottable predefweaponclass = item as caparelploottable;
                    ReturnList.Add(predefweaponclass.TableName);
                }
            }
            else if (isRewardTable)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    caparelprewardtable predefweaponclass = item as caparelprewardtable;
                    ReturnList.Add(predefweaponclass.RewardName);
                }
            }
            else if (isLootBoxList)
            {
                foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
                {
                    caparelootboxconfig predefweaponclass = item as caparelootboxconfig;
                    ReturnList.Add(predefweaponclass.LootBoxName);
                }
            }
        }
        private void LCPredefinedWeaponsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLootchest)
            {
                predefweapon = LCPredefinedWeaponsLB.GetItemText(LCPredefinedWeaponsLB.SelectedItem);
            }
        }
    }
}
