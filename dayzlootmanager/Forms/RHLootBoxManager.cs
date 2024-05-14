using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class RHLootBoxManager : DarkForm
    {
        internal Project currentproject;
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public string LootPoolConfigPath { get; private set; }
        public CapareLootPool LootPool { get; private set; }

        private string LootBoxConfigPath;

        public RHLootBoxes LootBoxConfig { get; private set; }

        public string Projectname;

        public caparelootboxconfig CurrentRhlootboxconfig { get; private set; }
        public caparelootboxstaticbox CurrentRhlootboxstaticbox { get; private set; }

        private Staticboxposition currentposition;
        private bool useraction = false;
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

        public RHLootBoxManager()
        {
            InitializeComponent();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    break;
                default:
                    break;
            }
        }

        private void RHLootBoxManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LootPoolConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare\\CapareLootPool\\CapareLootPoolConfig.json";
            LootPool = JsonSerializer.Deserialize<CapareLootPool>(File.ReadAllText(LootPoolConfigPath));

            LootBoxConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare\\CapareLootBox\\CapareLootBoxConfig.json";
            LootBoxConfig = JsonSerializer.Deserialize<RHLootBoxes>(File.ReadAllText(LootBoxConfigPath));
            LootBoxConfig.isDirty = false;
            LootBoxConfig.Filename = LootBoxConfigPath;

            LoadLootBoxSettings();
        }
        private void LoadLootBoxSettings()
        {
            useraction = false;

            LifetimeNUD.Value = LootBoxConfig.LootBoxLifetime;
            checkBox1.Checked = LootBoxConfig.StaticLootBoxEnabled == 1 ? true : false;

            listBox1.DisplayMember = "DisplayName";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = LootBoxConfig.CapareLootBoxConfigs;

            LootBoxLocationsLB.DisplayMember = "DisplayName";
            LootBoxLocationsLB.ValueMember = "Value";
            LootBoxLocationsLB.DataSource = LootBoxConfig.CapareLootBoxStaticBoxes;

            useraction = true;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            CurrentRhlootboxconfig = listBox1.SelectedItem as caparelootboxconfig;
            useraction = false;

            listBox2.DisplayMember = "DisplayName";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = CurrentRhlootboxconfig.PossibleLootList;

            useraction = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            LootBoxConfig.CapareLootBoxConfigs.Add(new caparelootboxconfig()
            {
                LootBoxName = "Replace with LootBox Name",
                PossibleLootList = new BindingList<string>()
            }
           );
            listBox1.SelectedIndex = -1;
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItems.Count < 1) return;
            int index = listBox1.SelectedIndex;
            LootBoxConfig.CapareLootBoxConfigs.Remove(CurrentRhlootboxconfig);
            LootBoxConfig.isDirty = true;
            listBox1.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;
            }
            else
            {
                listBox1.SelectedIndex = index - 1;
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
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
                    CurrentRhlootboxconfig.PossibleLootList.Add(weapon);
                    LootBoxConfig.isDirty = true;
                }
            }
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count < 1) return;
            List<string> removeitems = new List<string>();
            foreach (var item in listBox2.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string item in removeitems)
            {
                CurrentRhlootboxconfig.PossibleLootList.Remove(item);
            }
            LootBoxConfig.isDirty = true;
            listBox2.SelectedIndex = -1;
        }

        private void LootChestsLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootBoxLocationsLB.SelectedItems.Count < 1) return;
            CurrentRhlootboxstaticbox = LootBoxLocationsLB.SelectedItem as caparelootboxstaticbox;
            useraction = false;

            nameTB.Text = CurrentRhlootboxstaticbox.LootBoxSetName;
            numberNUD.Value = CurrentRhlootboxstaticbox.NumberOfPositions;
            checkBox2.Checked = CurrentRhlootboxstaticbox.UseCustomLootList == 1 ? true : false;


            chestLB.DisplayMember = "DisplayName";
            chestLB.ValueMember = "Value";
            chestLB.DataSource = CurrentRhlootboxstaticbox.PossibleLootBoxNames;

            posLB.DisplayMember = "DisplayName";
            posLB.ValueMember = "Value";
            posLB.DataSource = CurrentRhlootboxstaticbox.StaticBoxPositions;

            lootLB.DisplayMember = "DisplayName";
            lootLB.ValueMember = "Value";
            lootLB.DataSource = CurrentRhlootboxstaticbox.CustomLootList;

            darkLabel20.Text = "Location count:" + CurrentRhlootboxstaticbox.StaticBoxPositions.Count.ToString();

            useraction = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootBoxConfig.StaticLootBoxEnabled = checkBox1.Checked == true ? 1 : 0;
            LootPool.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            LootBoxConfig.CapareLootBoxStaticBoxes.Add(new caparelootboxstaticbox()
            {
                LootBoxSetName = "New_LootBox-set",
                NumberOfPositions = 1,
                StaticBoxPositions = new BindingList<Staticboxposition>(),
                UseCustomLootList = 1,
                PossibleLootBoxNames = new BindingList<string>(),
                CustomLootList = new BindingList<string>()
            });
            LootBoxLocationsLB.SelectedIndex = -1;
            LootBoxLocationsLB.SelectedIndex = LootBoxLocationsLB.Items.Count - 1;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (LootBoxLocationsLB.SelectedItems.Count < 1) return;
            int index = LootBoxLocationsLB.SelectedIndex;
            LootBoxConfig.CapareLootBoxStaticBoxes.Remove(CurrentRhlootboxstaticbox);
            LootBoxConfig.isDirty = true;
            LootBoxLocationsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (LootBoxLocationsLB.Items.Count > 0)
                    LootBoxLocationsLB.SelectedIndex = 0;
            }
            else
            {
                LootBoxLocationsLB.SelectedIndex = index - 1;
            }
        }
        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentRhlootboxstaticbox.LootBoxSetName = nameTB.Text;
            LootBoxConfig.isDirty = true;
        }
        private void numberNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentRhlootboxstaticbox.NumberOfPositions = (int)numberNUD.Value;
            LootBoxConfig.isDirty = true;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentRhlootboxstaticbox.UseCustomLootList = checkBox2.Checked == true ? 1 : 0;
            LootBoxConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                Rhlootboxconfig = LootBoxConfig.CapareLootBoxConfigs,
                titellabel = "Add Items from Loot list",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = true,
                isUtopiaAirdroplootPools = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    CurrentRhlootboxstaticbox.PossibleLootBoxNames.Add(weapon);
                    LootBoxConfig.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            CurrentRhlootboxstaticbox.PossibleLootBoxNames.Remove(chestLB.GetItemText(chestLB.SelectedItem));
            LootBoxConfig.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            if (CurrentRhlootboxstaticbox.CustomLootList == null)
            {
                CurrentRhlootboxstaticbox.CustomLootList = new BindingList<string>();
                lootLB.DisplayMember = "DisplayName";
                lootLB.ValueMember = "Value";
                lootLB.DataSource = CurrentRhlootboxstaticbox.CustomLootList;
            }
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                Rhlprewardtable = LootPool.CapareLPRewardTables,
                titellabel = "Add Items from Loot list",
                isLootList = false,
                isRewardTable = true,
                isRHTableList = false,
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
                    CurrentRhlootboxstaticbox.CustomLootList.Add(weapon);
                    LootBoxConfig.isDirty = true;
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            CurrentRhlootboxstaticbox.CustomLootList.Remove(lootLB.GetItemText(lootLB.SelectedItem));
            LootBoxConfig.isDirty = true;
        }
        private void posLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (posLB.SelectedItems.Count < 1) return;
            currentposition = posLB.SelectedItem as Staticboxposition;
            useraction = false;
            posXNUD.Value = (decimal)currentposition.Position[0];
            posYNUD.Value = (decimal)currentposition.Position[1];
            posZNUD.Value = (decimal)currentposition.Position[2];

            posXRNUD.Value = (decimal)currentposition.Rotation[0];
            posYRNUD.Value = (decimal)currentposition.Rotation[1];
            posZRNUD.Value = (decimal)currentposition.Rotation[2];

            useraction = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            CurrentRhlootboxstaticbox.StaticBoxPositions.Add(new Staticboxposition()
            {
                Position = new float[] { 0, 0, 0 },
                Rotation = new float[] { 0, 0, 0 }
            });
            posLB.SelectedIndex = -1;
            posLB.SelectedIndex = posLB.Items.Count - 1;
            darkLabel20.Text = "Location count:" + CurrentRhlootboxstaticbox.StaticBoxPositions.Count.ToString();
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            if (posLB.SelectedItems.Count < 1) return;
            int index = posLB.SelectedIndex;
            CurrentRhlootboxstaticbox.StaticBoxPositions.Remove(currentposition);
            LootBoxConfig.isDirty = true;
            posLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (posLB.Items.Count > 0)
                    posLB.SelectedIndex = 0;
            }
            else
            {
                posLB.SelectedIndex = index - 1;
            }
            darkLabel20.Text = "Location count:" + CurrentRhlootboxstaticbox.StaticBoxPositions.Count.ToString();
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentRhlootboxstaticbox.StaticBoxPositions.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        CurrentRhlootboxstaticbox.StaticBoxPositions.Add(new Staticboxposition()
                        {
                            Position = eo.Position,
                            Rotation = eo.Orientation
                        });
                        posLB.SelectedIndex = -1;
                        posLB.SelectedIndex = posLB.Items.Count - 1;
                        posLB.Refresh();
                    }
                    darkLabel20.Text = "Location count:" + CurrentRhlootboxstaticbox.StaticBoxPositions.Count.ToString();
                }
            }
        }
        private void pos_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposition.Position[0] = (float)posXNUD.Value;
            currentposition.Position[1] = (float)posYNUD.Value;
            currentposition.Position[2] = (float)posZNUD.Value;

            currentposition.Rotation[0] = (float)posXRNUD.Value;
            currentposition.Rotation[1] = (float)posYRNUD.Value;
            currentposition.Rotation[2] = (float)posZRNUD.Value;

            LootBoxConfig.isDirty = true;
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (LootBoxConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(LootBoxConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LootBoxConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(LootBoxConfig.Filename, Path.GetDirectoryName(LootBoxConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(LootBoxConfig.Filename) + ".bak", true);
                }
                LootBoxConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootBoxConfig, options);
                File.WriteAllText(LootBoxConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(LootBoxConfig.Filename));
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

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare\\CapareLootBox");
        }

        private void LifetimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootBoxConfig.LootBoxLifetime = (int)LifetimeNUD.Value;
            LootPool.isDirty = true;
        }
    }
}
