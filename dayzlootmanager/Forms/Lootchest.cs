using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DayZeEditor
{
    public partial class Lootchest : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string Projectname;
        private bool useraction = false;

        public LootChestTable LootChestTable;
        public string LootChestTablePath;

        public LootChestsLocations CurrentLootChestLocation;
        public LCPredefinedWeapons currentLCPredefinedWeapons;
        public LootCategories currentLootCategories;
        public Vec3PandR currentposition;

        public string LootchestToolPath;
        public LootChestTools LootChestTools;
        public LCTools currentLootchestTool;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
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

        public Lootchest()
        {
            InitializeComponent();
        }
        private void Lootchest_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            tabControl2.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LootChestTablePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CJ_LootChests\\LootChests_V106.json";
            LootChestTable = JsonSerializer.Deserialize<LootChestTable>(File.ReadAllText(LootChestTablePath));
            LootChestTable.isDirty = false;
            LootChestTable.Getalllists();
            LootChestTable.Filename = LootChestTablePath;

            LootchestToolPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CJ_LootChests\\LootChestsTools_V106.json";
            LootChestTools = JsonSerializer.Deserialize<LootChestTools>(File.ReadAllText(LootchestToolPath));
            LootChestTools.isDirty = false;
            LootChestTools.Filename = LootchestToolPath;

            comboBox2.DataSource = Enum.GetValues(typeof(LootchestOpenable));

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawLootchests);
            trackBar1.Value = 1;
            SetLootChestsScale();

            loadlootchestsettings();
        }
        private void loadlootchestsettings()
        {
            var sortedListInstance = new BindingList<LCPredefinedWeapons>(LootChestTable.LCPredefinedWeapons.OrderBy(x => x.defname).ToList());
            LootChestTable.LCPredefinedWeapons = sortedListInstance;

            checkBox1.Checked = LootChestTable.EnableDebug == 0 ? false : true;
            DeleteLogsCB.Checked = LootChestTable.DeleteLogs == 0 ? false : true;
            checkBox2.Checked = LootChestTable.RandomQuantity == 0 ? false : true;
            MaxMagsNUD.Value = LootChestTable.MaxSpareMags;

            LootChestsLocationsLB.DisplayMember = "DisplayName";
            LootChestsLocationsLB.ValueMember = "Value";
            LootChestsLocationsLB.DataSource = LootChestTable.LootChestsLocations;


            LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
            LCPredefinedWeaponsLB.ValueMember = "Value";
            LCPredefinedWeaponsLB.DataSource = LootChestTable.LCPredefinedWeapons;

            LootCategoriesLB.DisplayMember = "DisplayName";
            LootCategoriesLB.ValueMember = "Value";
            LootCategoriesLB.DataSource = LootChestTable.LootCategories;

            ToolsLB.DisplayMember = "DisplayName";
            ToolsLB.ValueMember = "Value";
            ToolsLB.DataSource = LootChestTools.LCTools;
        }
        private void LootChestsLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootChestsLocationsLB.SelectedItems.Count < 1) return;
            CurrentLootChestLocation = LootChestsLocationsLB.SelectedItem as LootChestsLocations;
            useraction = false;
            nameTB.Text = CurrentLootChestLocation.name;
            numberNUD.Value = CurrentLootChestLocation.number;
            keyclassTB.Text = CurrentLootChestLocation.keyclass;
            LootRandomizationNUD.Value = (decimal)CurrentLootChestLocation.LootRandomization;
            LightCB.Checked = CurrentLootChestLocation.light == 1 ? true : false;
            comboBox2.SelectedItem = (LootchestOpenable)CurrentLootChestLocation.openable;


            chestLB.DisplayMember = "DisplayName";
            chestLB.ValueMember = "Value";
            chestLB.DataSource = CurrentLootChestLocation.chest;

            lootLB.DisplayMember = "DisplayName";
            lootLB.ValueMember = "Value";
            lootLB.DataSource = CurrentLootChestLocation.loot;


            posLB.DisplayMember = "DisplayName";
            posLB.ValueMember = "Value";
            posLB.DataSource = CurrentLootChestLocation._pos;

            darkLabel20.Text = "Location count:" + CurrentLootChestLocation._pos.Count.ToString();
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                LootCategories = LootChestTable.LootCategories,
                titellabel = "Add loot chest types",
                isLootList = false,
                isRewardTable = false,
                isRHTableList = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = true,
                isLootBoxList = false,
                isUtopiaAirdroplootPools = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string predefweapon = form.predefweapon;
                CurrentLootChestLocation.chest.Add(predefweapon);
                LootChestTable.isDirty = true;
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                LootCategories = LootChestTable.LootCategories,
                titellabel = "Add Items from Loot list",
                isLootList = true,
                isRewardTable = false,
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
                    CurrentLootChestLocation.loot.Add(weapon);
                    LootChestTable.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            CurrentLootChestLocation.chest.Remove(chestLB.GetItemText(chestLB.SelectedItem));
            LootChestTable.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            CurrentLootChestLocation.loot.Remove(lootLB.GetItemText(lootLB.SelectedItem));
            LootChestTable.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            LootChestTable.LootChestsLocations.Add(new LootChestsLocations()
            {
                name = "New_Location",
                number = 1,
                pos = new BindingList<string>(),
                keyclass = "",
                chest = new BindingList<string>(),
                loot = new BindingList<string>(),
                _pos = new BindingList<Vec3PandR>()
            }
            );
            LootChestsLocationsLB.SelectedIndex = -1;
            LootChestsLocationsLB.SelectedIndex = LootChestsLocationsLB.Items.Count - 1;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (LootChestsLocationsLB.SelectedItems.Count < 1) return;
            int index = LootChestsLocationsLB.SelectedIndex;
            LootChestTable.LootChestsLocations.Remove(CurrentLootChestLocation);
            LootChestTable.isDirty = true;
            LootChestsLocationsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (LootChestsLocationsLB.Items.Count > 0)
                    LootChestsLocationsLB.SelectedIndex = 0;
            }
            else
            {
                LootChestsLocationsLB.SelectedIndex = index - 1;
            }
        }
        private void LCPredefinedWeaponsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LCPredefinedWeaponsLB.SelectedItems.Count < 1) return;
            currentLCPredefinedWeapons = LCPredefinedWeaponsLB.SelectedItem as LCPredefinedWeapons;
            useraction = false;
            SetweaponInfo();
            useraction = true;
        }
        private void SetweaponInfo()
        {
            defnameTB.Text = currentLCPredefinedWeapons.defname;
            weaponTB.Text = currentLCPredefinedWeapons.weapon;
            magazineTB.Text = currentLCPredefinedWeapons.magazine;
            opticTB.Text = currentLCPredefinedWeapons.optic;
            opticbatteryCB.Checked = currentLCPredefinedWeapons.opticbattery == 1 ? true : false;

            attachmentsLB.DisplayMember = "DisplayName";
            attachmentsLB.ValueMember = "Value";
            attachmentsLB.DataSource = currentLCPredefinedWeapons.attachments;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (posLB.SelectedItems.Count < 1) return;
            currentposition = posLB.SelectedItem as Vec3PandR;
            useraction = false;
            posXNUD.Value = (decimal)currentposition.Position.X;
            posYNUD.Value = (decimal)currentposition.Position.Y;
            posZNUD.Value = (decimal)currentposition.Position.Z;

            posXRNUD.Value = (decimal)currentposition.Rotation.X;
            posYRNUD.Value = (decimal)currentposition.Rotation.Y;
            posZRNUD.Value = (decimal)currentposition.Rotation.Z;

            useraction = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            int index = posLB.SelectedIndex;
            CurrentLootChestLocation._pos.Remove(currentposition);
            posLB.Invalidate();
            posLB.SelectedIndex = -1;
            if (CurrentLootChestLocation._pos.Count > 0)
            {
                posLB.SelectedIndex = index - 1;
            }
            pictureBox1.Invalidate();
            LootChestTable.isDirty = true;

            darkLabel20.Text = "Location count:" + CurrentLootChestLocation._pos.Count.ToString();
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            CurrentLootChestLocation._pos.Add(new Vec3PandR(new float[]{ 0,0,0}, new float[] { 0,0,0}, true));
            posLB.SelectedIndex = -1;
            posLB.SelectedIndex = posLB.Items.Count - 1;
            darkLabel20.Text = "Location count:" + CurrentLootChestLocation._pos.Count.ToString();
        }
        private void pos_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentposition.rotspecified)
            {
                currentposition.Rotation = new Vec3((float)posXRNUD.Value, (float)posYRNUD.Value, (float)posZRNUD.Value);
            }
            currentposition.Position = new Vec3((float)posXNUD.Value, (float)posYNUD.Value, (float)posZNUD.Value);
            LootChestTable.isDirty = true;
            posLB.Invalidate();
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            LootChestTable.LCPredefinedWeapons.Add(new LCPredefinedWeapons()
            {
                defname = "LC_predefined_NewWeapon",
                weapon = "",
                magazine = "",
                attachments = new BindingList<string>(),
                optic = "",
                opticbattery = 0
            }
            ); ;
            LCPredefinedWeaponsLB.SelectedIndex = -1;
            LCPredefinedWeaponsLB.SelectedIndex = LCPredefinedWeaponsLB.Items.Count - 1;
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (LCPredefinedWeaponsLB.SelectedItems.Count < 1) return;
            int index = LCPredefinedWeaponsLB.SelectedIndex;
            LootChestTable.LCPredefinedWeapons.Remove(currentLCPredefinedWeapons);
            LootChestTable.isDirty = true;
            LCPredefinedWeaponsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (LCPredefinedWeaponsLB.Items.Count > 0)
                    LCPredefinedWeaponsLB.SelectedIndex = 0;
            }
            else
            {
                LCPredefinedWeaponsLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
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
                    if (!LootChestTable.LCPredefinedWeapons.Any(x => x.defname == "LC_predefined_" + l))
                    {
                        currentLCPredefinedWeapons.defname = "LC_predefined_" + l;
                        currentLCPredefinedWeapons.weapon = l;
                        SetweaponInfo();
                        LootChestTable.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("There is allready a pre defined weapon set up for " + l);
                    }
                }
            }
        }
        private void darkButton11_Click(object sender, EventArgs e)
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
                    currentLCPredefinedWeapons.magazine = l;
                    SetweaponInfo();
                    LootChestTable.isDirty = true;
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!currentLCPredefinedWeapons.attachments.Any(x => x == l))
                    {
                        currentLCPredefinedWeapons.attachments.Add(l);
                        SetweaponInfo();
                        LootChestTable.isDirty = true;
                    }
                }
            }
        }
        private void darkButton12_Click(object sender, EventArgs e)
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
                    currentLCPredefinedWeapons.optic = l;
                    SetweaponInfo();
                    LootChestTable.isDirty = true;
                }
            }
        }
        private void opticbatteryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCPredefinedWeapons.opticbattery = opticbatteryCB.Checked == true ? 1 : 0;
            SetweaponInfo();
            LootChestTable.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (attachmentsLB.SelectedItems.Count < 1) return;
            int index = attachmentsLB.SelectedIndex;
            currentLCPredefinedWeapons.attachments.Remove(attachmentsLB.GetItemText(attachmentsLB.SelectedItem));
            LootChestTable.isDirty = true;
            attachmentsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (attachmentsLB.Items.Count > 0)
                    attachmentsLB.SelectedIndex = 0;
                else
                    attachmentsLB.SelectedIndex = -1;
            }
            else
            {
                attachmentsLB.SelectedIndex = index - 1;
            }
        }
        private void LootCategoriesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootCategoriesLB.SelectedItems.Count < 1) return;
            currentLootCategories = LootCategoriesLB.SelectedItem as LootCategories;
            useraction = false;

            LootCatNameTB.Text = currentLootCategories.name;

            LootCatLootLB.DisplayMember = "DisplayName";
            LootCatLootLB.ValueMember = "Value";
            LootCatLootLB.DataSource = currentLootCategories.Loot;


            useraction = true;
        }

        private void LootCatNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLootCategories.name = LootCatNameTB.Text;
            LootChestTable.isDirty = true;
        }

        private void darkButton18_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!currentLootCategories.Loot.Any(x => x == l))
                    {
                        if (currentLootCategories.Loot.Count > 0 && currentLootCategories.Loot[0] == "")
                            currentLootCategories.Loot.RemoveAt(0);
                        currentLootCategories.Loot.Add(l);
                        LootChestTable.isDirty = true;
                    }
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            currentLootCategories.Loot.Remove(LootCatLootLB.GetItemText(LootCatLootLB.SelectedItem));
            if (currentLootCategories.Loot.Count == 0)
                currentLootCategories.Loot.Add("");

            LootChestTable.isDirty = true;
        }
        private void opticTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCPredefinedWeapons.optic = opticTB.Text;
            LootChestTable.isDirty = true;
        }
        private void magazineTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCPredefinedWeapons.magazine = magazineTB.Text;
            LootChestTable.isDirty = true;
        }
        private void weaponTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCPredefinedWeapons.weapon = weaponTB.Text;
            LootChestTable.isDirty = true;
        }
        private void defnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLCPredefinedWeapons.defname = defnameTB.Text;
            LootChestTable.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                LCPredefinedWeapons = LootChestTable.LCPredefinedWeapons,
                titellabel = "Add Items from Predefined Weapons",
                isLootList = false,
                isRewardTable = false,
                isRHTableList = false,
                ispredefinedweapon = true,
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
                    if (!currentLootCategories.Loot.Any(x => x == weapon))
                    {
                        currentLootCategories.Loot.Add(weapon);
                        LootChestTable.isDirty = true;
                    }
                }
            }
        }
        private void keyclassTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentLootChestLocation.keyclass = keyclassTB.Text;
            LootChestTable.isDirty = true;
        }
        private void numberNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentLootChestLocation.number = (int)numberNUD.Value;
            LootChestTable.isDirty = true;
        }
        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentLootChestLocation.name = nameTB.Text;
            LootChestTable.isDirty = true;
        }
        private void LootRandomizationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentLootChestLocation.LootRandomization = LootRandomizationNUD.Value;
            LootChestTable.isDirty = true;
        }
        private void LightCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentLootChestLocation.light = LightCB.Checked == true ? 1 : 0;
            LootChestTable.isDirty = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (LootChestTable.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(LootChestTable.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LootChestTable.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(LootChestTable.Filename, Path.GetDirectoryName(LootChestTable.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(LootChestTable.Filename) + ".bak", true);
                }
                LootChestTable.SetAllLists();
                LootChestTable.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootChestTable, options);
                File.WriteAllText(LootChestTable.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(LootChestTable.Filename));
            }
            if (LootChestTools.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(LootChestTools.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LootChestTools.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(LootChestTools.Filename, Path.GetDirectoryName(LootChestTools.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(LootChestTools.Filename) + ".bak", true);
                }
                LootChestTools.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootChestTools, options);
                File.WriteAllText(LootChestTools.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(LootChestTools.Filename));
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
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton7.Checked = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            if (tabControl1.SelectedIndex == 3)
                toolStripButton1.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton7.Checked = false;
                    toolStripButton1.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton7.Checked = false;
                    toolStripButton1.Checked = false;
                    break;
                case 2:
                    toolStripButton8.Checked = false;
                    toolStripButton3.Checked = false;
                    toolStripButton1.Checked = false;
                    break;
                case 3:
                    toolStripButton8.Checked = false;
                    toolStripButton3.Checked = false;
                    toolStripButton7.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CJ_LootChests\\");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootChestTable.EnableDebug = checkBox1.Checked == false ? 0 : 1;
            LootChestTable.isDirty = true;
        }
        private void MaxMagsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootChestTable.MaxSpareMags = (int)MaxMagsNUD.Value;
            LootChestTable.isDirty = true;
        }

        private void DeleteLogsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootChestTable.DeleteLogs = DeleteLogsCB.Checked == false ? 0 : 1;
            LootChestTable.isDirty = true;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootChestTable.RandomQuantity = checkBox2.Checked == false ? 0 : 1;
            LootChestTable.isDirty = true;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!useraction) return;
            LootchestOpenable cacl = (LootchestOpenable)comboBox2.SelectedItem;
            CurrentLootChestLocation.openable = (int)cacl;
            LootChestTable.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            LootChestTable.LootCategories.Add(new LootCategories()
            {
                name = "LC_Table_",
                Loot = new BindingList<string>()
            });
            LCPredefinedWeaponsLB.SelectedIndex = -1;
            LCPredefinedWeaponsLB.SelectedIndex = LCPredefinedWeaponsLB.Items.Count - 1;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            if (LootCategoriesLB.SelectedItems.Count < 1) return;
            int index = LootCategoriesLB.SelectedIndex;
            LootChestTable.LootCategories.Remove(currentLootCategories);
            LootChestTable.isDirty = true;
            LootCategoriesLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (LootCategoriesLB.Items.Count > 0)
                    LootCategoriesLB.SelectedIndex = 0;
            }
            else
            {
                LootCategoriesLB.SelectedIndex = index - 1;
            }
        }
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ToolsLB.SelectedItems.Count < 1) return;
            currentLootchestTool = ToolsLB.SelectedItem as LCTools;
            useraction = false;
            textBox4.Text = currentLootchestTool.name;
            numericUpDown1.Value = (decimal)currentLootchestTool.time;
            numericUpDown2.Value = (decimal)currentLootchestTool.dmg;
            string desc = currentLootchestTool.desc;
            if (desc.Contains("|"))
            {
                ToolsNameDescTB.Text = desc.Split('|')[0];
                ToolDescDescTB.Text = desc.Split('|')[1];
            }
            else
            {
                ToolsNameDescTB.Text = "";
                ToolDescDescTB.Text = "";
            }
            useraction = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            LootChestTools.LCTools.Add(new LCTools()
            {
                name = "New_Tool",
                time = 10,
                dmg = 33
            }
            ); ;
            ToolsLB.SelectedIndex = -1;
            ToolsLB.SelectedIndex = ToolsLB.Items.Count - 1;
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            if (ToolsLB.SelectedItems.Count < 1) return;
            int index = ToolsLB.SelectedIndex;
            LootChestTools.LCTools.Remove(currentLootchestTool);
            LootChestTools.isDirty = true;
            ToolsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (ToolsLB.Items.Count > 0)
                    ToolsLB.SelectedIndex = 0;
            }
            else
            {
                ToolsLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = false,
                LowerCase = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!LootChestTools.LCTools.Any(x => x.name == l))
                    {
                        currentLootchestTool.name = l;
                        LootChestTools.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("There is allready an entry set up for " + l);
                    }
                }
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLootchestTool.time = (int)numericUpDown1.Value;
            LootChestTools.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLootchestTool.dmg = (int)numericUpDown2.Value;
            LootChestTools.isDirty = true;
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
                    DialogResult result = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNoCancel);
                    if ((result == DialogResult.Cancel))
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        CurrentLootChestLocation._pos = new BindingList<Vec3PandR>();
                    }
                    CurrentLootChestLocation.ImportDZE(importfile);
                    useraction = false;
                    posLB.DataSource = CurrentLootChestLocation._pos;
                    LootChestTable.isDirty = true;
                    darkLabel20.Text = "Location count:" + CurrentLootChestLocation._pos.Count.ToString();
                    useraction = true;
                }
            }
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            int m_Id = 0;
            string filename = "";
            foreach (Vec3PandR vec3pandr in CurrentLootChestLocation._pos)
            {
                Editorobject SpawnObject = new Editorobject()
                {
                    Type = CurrentLootChestLocation.chest[0],
                    DisplayName = CurrentLootChestLocation.chest[0],
                    Position = vec3pandr.GetPositionFloatArray(),
                    Orientation = vec3pandr.GetRotationFloatArray(),
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(SpawnObject);
                m_Id++;
            }
            filename = "CJ Loot Chest - " + CurrentLootChestLocation.name;
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = filename;
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentLootChestLocation.loot.Add(l);
                    LootChestTable.isDirty = true;
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentLootChestLocation.loot.Add(l);
                    LootChestTable.isDirty = true;
                }
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            List<string> selectedLoot = new List<string>();
            foreach (var item in lootLB.SelectedItems)
            {
                selectedLoot.Add(item.ToString());
            }
            if (selectedLoot.Count <= 0) return;
            Clipboard.SetText(string.Join(Environment.NewLine, selectedLoot.Cast<object>().Select(o => o.ToString()).ToArray()));
            Console.WriteLine("\nCopied to Clipboard:\n" + string.Join(Environment.NewLine, selectedLoot.Cast<object>().Select(o => o.ToString()).ToArray()));
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            List<string> selectedLoot = new List<string>();
            foreach (var item in LootCatLootLB.SelectedItems)
            {
                selectedLoot.Add(item.ToString());
            }
            Clipboard.SetText(string.Join(Environment.NewLine, selectedLoot.Cast<object>().Select(o => o.ToString()).ToArray()));
            Console.WriteLine("\nCopied to Clipboard:\n" + string.Join(Environment.NewLine, selectedLoot.Cast<object>().Select(o => o.ToString()).ToArray()));
        }
        private void ToolsNameDescTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLootchestTool.desc = ToolsNameDescTB.Text + "|" + ToolDescDescTB.Text;
            LootChestTools.isDirty = true;
        }
        private void ToolDescDescTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentLootchestTool.desc = ToolsNameDescTB.Text + "|" + ToolDescDescTB.Text;
            LootChestTools.isDirty = true;
        }
        private void Lootchest_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (LootChestTable.isDirty)
            {
                needtosave = true;
            }
            if (LootChestTools.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    savefiles();
                }
            }
        }


        public string currentloot;
        private void LootCatLootLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootCatLootLB.SelectedItems.Count < 1) return;
            currentloot = LootCatLootLB.SelectedItem as string;
            useraction = false;
            if (currentloot.StartsWith("LC_predefined"))
            {
                ItemRarityTableCB.Visible = false;
                ItemRarityTableNUD.Visible = false;
            }
            else if (currentloot.Contains("|"))
            {
                ItemRarityTableCB.Visible = true;
                ItemRarityTableNUD.Visible = true;
                ItemRarityTableCB.Checked = true;
                ItemRarityTableNUD.Value = Convert.ToDecimal(currentloot.Split('|')[1]);
            }
            else
            {
                ItemRarityTableCB.Visible = true;
                ItemRarityTableCB.Checked = false;

            }
            useraction = true;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ItemRarityTableNUD.Visible = ItemRarityTableCB.Checked;
            if (!useraction) { return; }
            string loot = "";
            if (ItemRarityTableCB.Checked)
            {
                loot = currentloot + "|1.0";
            }
            else
            {
                loot = currentloot.Split('|')[0];
            }
            currentLootCategories.Loot[LootCatLootLB.SelectedIndex] = loot;
            LootChestTable.isDirty = true;
        }
        private void ItemRarityTableNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            string loot = currentloot.Split('|')[0] + "|" + ItemRarityTableNUD.Value.ToString();
            currentLootCategories.Loot[LootCatLootLB.SelectedIndex] = loot;
            LootChestTable.isDirty = true;
        }
        private void lootLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootLB.SelectedItems.Count < 1) return;
            currentloot = lootLB.SelectedItem as string;
            useraction = false;
            if (currentloot.StartsWith("LC_predefined_") || currentloot.StartsWith("LC_Table_"))
            {
                ItemrarityChestCB.Visible = false;
                ItemrarityChestNUD.Visible = false;
            }
            else if (currentloot.Contains("|"))
            {
                ItemrarityChestCB.Visible = true;
                ItemrarityChestNUD.Visible = true;
                ItemrarityChestCB.Checked = true;
                ItemrarityChestNUD.Value = Convert.ToDecimal(currentloot.Split('|')[1]);
            }
            else
            {
                ItemrarityChestCB.Visible = true;
                ItemrarityChestCB.Checked = false;

            }
            useraction = true;
        }
        private void ItemrarityChestCB_CheckedChanged(object sender, EventArgs e)
        {
            ItemrarityChestNUD.Visible = ItemrarityChestCB.Checked;
            if (!useraction) { return; }
            string loot = "";
            if (ItemrarityChestCB.Checked)
            {
                loot = currentloot + "|1.0";
            }
            else
            {
                loot = currentloot.Split('|')[0];
            }
            CurrentLootChestLocation.loot[lootLB.SelectedIndex] = loot;
            LootChestTable.isDirty = true;
        }
        private void ItemrarityChestNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            string loot = currentloot.Split('|')[0] + "|" + ItemrarityChestNUD.Value.ToString();
            CurrentLootChestLocation.loot[lootLB.SelectedIndex] = loot;
            LootChestTable.isDirty = true;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
            if (tabControl2.SelectedIndex == 0)
                toolStripButton12.Checked = true;
        }
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
            if (tabControl2.SelectedIndex == 1)
                toolStripButton13.Checked = true;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    toolStripButton13.Checked = false;
                    break;
                case 1:
                    toolStripButton12.Checked = false;
                    break;
                default:
                    break;
            }
        }

        public int LootChestScale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            LootChestScale = trackBar1.Value;
            SetLootChestsScale();
        }
        private void SetLootChestsScale()
        {
            float scalevalue = LootChestScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawLootchests(object sender, PaintEventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                foreach (LootChestsLocations locs in LootChestTable.LootChestsLocations)
                {
                    float scalevalue = LootChestScale * 0.05f;
                    foreach (Vec3PandR v3 in locs._pos)
                    {
                        int centerX = (int)(Math.Round(v3.Position.X) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Position.Z, 0) * scalevalue);
                        int eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (locs == CurrentLootChestLocation)
                            pen = new Pen(Color.Yellow, 4);
                        getCircleLootChest(e.Graphics, pen, center, eventradius, "");
                    }
                }
            }
            else
            {
                if (CurrentLootChestLocation == null) return;
                float scalevalue = LootChestScale * 0.05f;
                foreach (Vec3PandR v3 in CurrentLootChestLocation._pos)
                {
                    int centerX = (int)(Math.Round(v3.Position.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3.Position.Z, 0) * scalevalue);
                    int eventradius = (int)(Math.Round(1f, 0) * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Yellow, 4);
                    getCircleLootChest(e.Graphics, pen, center, eventradius, "");
                }
            }
        }
        private void getCircleLootChest(Graphics drawingArea, Pen penToUse, Point center, int radius, string c)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
            Rectangle rect3 = new Rectangle(center.X - radius, center.Y - (radius + 30), 200, 40);
            drawingArea.DrawString(c, new Font("Tahoma", 9), Brushes.White, rect3);
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panelEx1.AutoScrollPosition.X - changePoint.X, -panelEx1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
            decimal scalevalue = LootChestScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label2.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox1_ZoomOut();
            }
            else
            {
                pictureBox1_ZoomIn();
            }

        }
        private void pictureBox1_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            LootChestScale = trackBar1.Value;
            SetLootChestsScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            LootChestScale = trackBar1.Value;
            SetLootChestsScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }


    }
}
