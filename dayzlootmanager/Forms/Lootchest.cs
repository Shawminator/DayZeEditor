using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public string currentposition;

        public string LootchestToolPath;
        public LootChestTools LootChestTools;
        public LCTools currentLootchestTool;

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
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LootChestTablePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CJ_LootChests\\LootChests_V104.json";
            LootChestTable = JsonSerializer.Deserialize<LootChestTable>(File.ReadAllText(LootChestTablePath));
            LootChestTable.isDirty = false;
            LootChestTable.Filename = LootChestTablePath;

            LootchestToolPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\CJ_LootChests\\LootChestsTools_V104.json";
            LootChestTools = JsonSerializer.Deserialize<LootChestTools>(File.ReadAllText(LootchestToolPath));
            LootChestTools.isDirty = false;
            LootChestTools.Filename = LootchestToolPath;

            comboBox2.DataSource = Enum.GetValues(typeof(LootchestOpenable));

            loadlootchestsettings();
        }
        private void loadlootchestsettings()
        {
            var sortedListInstance = new BindingList<LCPredefinedWeapons>(LootChestTable.LCPredefinedWeapons.OrderBy(x => x.defname).ToList());
            LootChestTable.LCPredefinedWeapons = sortedListInstance;

            checkBox1.Checked = LootChestTable.EnableDebug == 0 ? false : true;

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
            comboBox2.SelectedItem = (LootchestOpenable)CurrentLootChestLocation.openable;


            chestLB.DisplayMember = "DisplayName";
            chestLB.ValueMember = "Value";
            chestLB.DataSource = CurrentLootChestLocation.chest;

            lootLB.DisplayMember = "DisplayName";
            lootLB.ValueMember = "Value";
            lootLB.DataSource = CurrentLootChestLocation.loot;


            posLB.DisplayMember = "DisplayName";
            posLB.ValueMember = "Value";
            posLB.DataSource = CurrentLootChestLocation.pos;

            darkLabel20.Text = "Location count:" + CurrentLootChestLocation.pos.Count.ToString();

            useraction = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedWeapons form = new AddfromPredefinedWeapons
            {
                LootCategories = LootChestTable.LootCategories,
                titellabel = "Add loot chest types",
                isLootList = false,
                ispredefinedweapon = false,
                isLootchest = true
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
            AddfromPredefinedWeapons form = new AddfromPredefinedWeapons
            {
                LootCategories = LootChestTable.LootCategories,
                titellabel = "Add Items from Loot list",
                isLootList = true,
                ispredefinedweapon = false,
                isLootchest = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string predefweapon = form.predefweapon;
                CurrentLootChestLocation.loot.Add(predefweapon);
                LootChestTable.isDirty = true;
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
                loot = new BindingList<string>()
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
            currentposition = posLB.SelectedItem as string;
            useraction = false;
            string[] split = currentposition.Split('|');
            posXNUD.Value = (decimal)Convert.ToSingle(split[0].Split(' ')[0]);
            posYNUD.Value = (decimal)Convert.ToSingle(split[0].Split(' ')[1]);
            posZNUD.Value = (decimal)Convert.ToSingle(split[0].Split(' ')[2]);

            posXRNUD.Value = (decimal)Convert.ToSingle(split[1].Split(' ')[0]);
            posYRNUD.Value = (decimal)Convert.ToSingle(split[1].Split(' ')[1]);
            posZRNUD.Value = (decimal)Convert.ToSingle(split[1].Split(' ')[2]);

            useraction = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            if (posLB.SelectedItems.Count < 1) return;
            int index = posLB.SelectedIndex;
            CurrentLootChestLocation.pos.Remove(posLB.GetItemText(posLB.SelectedItem));
            LootChestTable.isDirty = true;
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
            darkLabel20.Text = "Location count:" + CurrentLootChestLocation.pos.Count.ToString();
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            CurrentLootChestLocation.pos.Add("0.000000 0.000000 0.000000|0.000000 0.000000 0.000000");
            posLB.SelectedIndex = -1;
            posLB.SelectedIndex = posLB.Items.Count - 1;
            darkLabel20.Text = "Location count:" + CurrentLootChestLocation.pos.Count.ToString();
        }
        private void pos_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string line = posXNUD.Value.ToString("N6") + " " + posYNUD.Value.ToString("N6") + " " + posZNUD.Value.ToString("N6") + "|" + posXRNUD.Value.ToString("N6") + " " + posYRNUD.Value.ToString("N6") + " " + posZRNUD.Value.ToString("N6");
            currentposition = line;
            CurrentLootChestLocation.pos[posLB.SelectedIndex] = currentposition;
            LootChestTable.isDirty = true;
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
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
            if(!useraction) { return; }
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
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
            AddfromPredefinedWeapons form = new AddfromPredefinedWeapons
            {
                LCPredefinedWeapons = LootChestTable.LCPredefinedWeapons,
                titellabel = "Add Items from Predefined Weapons",
                isLootList = false,
                ispredefinedweapon = true,
                isLootchest = false
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
            CurrentLootChestLocation.LootRandomization = (float)LootRandomizationNUD.Value;
            LootChestTable.isDirty = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles()
        {
            if (LootChestTable.isDirty)
            {
                LootChestTable.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootChestTable, options);
                File.WriteAllText(LootChestTable.Filename, jsonString);
                MessageBox.Show(Path.GetFileName(LootChestTable.Filename) + " Saved....");
            }
            if (LootChestTools.isDirty)
            {
                LootChestTools.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootChestTools, options);
                File.WriteAllText(LootChestTools.Filename, jsonString);
                MessageBox.Show(Path.GetFileName(LootChestTools.Filename) + " Saved....");
            }
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

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootChestTable.EnableDebug = checkBox1.Checked == false ? 0 : 1;
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
            }
        ); ;
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
                UseMultiple = false,
                isCategoryitem = false,
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
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    CurrentLootChestLocation.pos.Clear();
                    foreach (string line in fileContent)
                    {
                        string[] linesplit = line.Split('|');
                        if(!linesplit[0] .Contains("CJ_LootChest")) { return; }
                        CurrentLootChestLocation.pos.Add(linesplit[1] + "|" + linesplit[2]);
                        posLB.SelectedIndex = -1;
                        posLB.SelectedIndex = posLB.Items.Count - 1;
                        posLB.Refresh();
                    }
                    darkLabel20.Text = "Location count:" + CurrentLootChestLocation.pos.Count.ToString();
                }
            }
        }

        private void darkButton24_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = true,
                isCategoryitem = true
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
    }
}
