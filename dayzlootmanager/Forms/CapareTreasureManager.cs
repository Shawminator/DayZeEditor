using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DayZeEditor
{
    public partial class CapareTreasureManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public string LootPoolConfigPath { get; private set; }
        public CapareLootPool LootPool { get; private set; }

        public string CapareTreasureConfigPath { get; private set; }
        public CapareTreasureConfig CapareTreasure;
        public M_Capare_Treasure_Stashs M_Capare_Treasure_Stashs;
        public string Projectname;
        private bool useraction = false;
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            var CurrentItemWidth = (int)this.CreateGraphics().MeasureString(lb.Items[lb.Items.Count - 1].ToString(), lb.Font, TextRenderer.MeasureText(lb.Items[lb.Items.Count - 1].ToString(), new Font("Arial", 20.0F))).Width;
            lb.HorizontalExtent = CurrentItemWidth + 5;
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

        public CapareTreasureManager()
        {
            InitializeComponent();
        }
        private void CapareTreasureManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CapareTreasure.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void CapareWorkBenchManager_Load(object sender, EventArgs e)
        {
            useraction = false;

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LootPoolConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare\\CapareLootPool\\CapareLootPoolConfig.json";
            LootPool = JsonSerializer.Deserialize<CapareLootPool>(File.ReadAllText(LootPoolConfigPath));

            useraction = false;
            CapareTreasureConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CapareTreasure\\CapareTreasure.json";
            var options = new JsonSerializerOptions {Converters = { new BoolConverter() } };
            CapareTreasure = JsonSerializer.Deserialize<CapareTreasureConfig>(File.ReadAllText(CapareTreasureConfigPath), options);
            CapareTreasure.getstashpositions();
            CapareTreasure.isDirty = false;
            CapareTreasure.Filename = CapareTreasureConfigPath;


            TreasureStashLB.DisplayMember = "DisplayName";
            TreasureStashLB.ValueMember = "Value";
            TreasureStashLB.DataSource = CapareTreasure.m_Capare_Treasure_Stashs;

            useraction = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (CapareTreasure.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(CapareTreasure.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(CapareTreasure.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(CapareTreasure.Filename, Path.GetDirectoryName(CapareTreasure.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(CapareTreasure.Filename) + ".bak", true);
                }
                CapareTreasure.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Converters = { new BoolConverter() } };
                string jsonString = JsonSerializer.Serialize(CapareTreasure, options);
                File.WriteAllText(CapareTreasure.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(CapareTreasure.Filename));
            }
            string message = "The Following Files were saved....\n";
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                if (i == 5)
                {
                    message += l + "\n";
                    i = 0;
                }
                else
                {
                    message += l + ", ";
                    i++;
                }
            }
            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CapareTreasure");
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            M_Capare_Treasure_Stashs newstash = new M_Capare_Treasure_Stashs()
            {
                StashName = "Change me to the exact name of your photo.paa",
                StashPosition = new Stashposition()
                {
                    _Position = new Vec3(0, 0, 0),
                    _Rotation = new Vec3(0, 0, 0)
                },
                TriggerRadius = 25,
                Description = "Placeholder",
                ContainerType = "Capare_Treasure_Seachest",
                IsUnderground = true,
                RewardTables = new BindingList<string>()
            };
            CapareTreasure.m_Capare_Treasure_Stashs.Add(newstash);
            CapareTreasure.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (TreasureStashLB.SelectedItems.Count > 0)
            {
                List<M_Capare_Treasure_Stashs> removeitems = new List<M_Capare_Treasure_Stashs>();
                foreach (var item in TreasureStashLB.SelectedItems)
                {
                    M_Capare_Treasure_Stashs citem = item as M_Capare_Treasure_Stashs;
                    removeitems.Add(citem);
                }
                foreach (M_Capare_Treasure_Stashs item in removeitems)
                {
                    CapareTreasure.m_Capare_Treasure_Stashs.Remove(item);
                }
                CapareTreasure.isDirty = true;
                if (TreasureStashLB.Items.Count == 0)
                    TreasureStashLB.SelectedIndex = -1;
                else
                    TreasureStashLB.SelectedIndex = 0;
            }
        }
        private void TreasureStashLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TreasureStashLB.SelectedItems.Count <= 0) return;
            useraction = false;
            M_Capare_Treasure_Stashs = TreasureStashLB.SelectedItem as M_Capare_Treasure_Stashs;

            StashNameTB.Text = M_Capare_Treasure_Stashs.StashName;
            posXNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Position.X;
            posYNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Position.Y;
            posZNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Position.Z;
            rotXNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Rotation.X;
            rotYNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Rotation.Y;
            rotZNUD.Value = (decimal)M_Capare_Treasure_Stashs.StashPosition._Rotation.Z;
            TriggerRadiusNUD.Value =(decimal)M_Capare_Treasure_Stashs.TriggerRadius;
            DescriptionTB.Text = M_Capare_Treasure_Stashs.Description;
            ContainerTypeTB.Text = M_Capare_Treasure_Stashs.ContainerType;
            IsUndergroundCB.Checked = M_Capare_Treasure_Stashs.IsUnderground;
            RewardTablesLB.DisplayMember = "DisplayName";
            RewardTablesLB.ValueMember = "Value";
            RewardTablesLB.DataSource = M_Capare_Treasure_Stashs.RewardTables;

            useraction = true;
        }

        private void darkButton3_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    M_Capare_Treasure_Stashs.StashName = l;
                }
                CapareTreasure.isDirty = true;
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                Rhlprewardtable = LootPool.CapareLPRewardTables,
                titellabel = "Add Items from Loot list",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = true,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false,
                isUtopiaAirdroplootPools = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    M_Capare_Treasure_Stashs.RewardTables.Add(weapon);
                    CapareTreasure.isDirty = true;
                }
            }
        }

        private void darkButton2_Click(object sender, EventArgs e)
        {

        }

        private void DescriptionTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            M_Capare_Treasure_Stashs.Description = DescriptionTB.Text;
            CapareTreasure.isDirty = true;
        }
    }
}
