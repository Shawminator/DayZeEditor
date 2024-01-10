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
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class LootPoolManager : DarkForm
    {
        public Project currentproject { get; set; }
        public caparelprewardtable CurrentRHLPRewardTables { get; private set; }
        public capareLPAttachment currentRHLPAttachment { get; private set; }

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        private string LootPoolConfigPath;
        public string Projectname;
        private capareLPdefinedItems currentRHPredefineditems;
        private caparelploottable currentRhlploottable;
        private bool useraction = false;

        public CapareLootPool LootPool;

        public LootPoolManager()
        {
            InitializeComponent();
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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton7.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton7.Checked = false;
                    break;
                case 2:
                    toolStripButton8.Checked = false;
                    toolStripButton3.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void LootPoolManager_Load(object sender, EventArgs e)
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
            LootPool.isDirty = false;
            LootPool.Filename = LootPoolConfigPath;

            loadlootpoolsettings();
        }

        private void loadlootpoolsettings()
        {
            useraction = false;

            MaxMagsNUD.Value = LootPool.NumberOfExtraMagsForDefinedWeapons;

            LootChestsLocationsLB.DisplayMember = "DisplayName";
            LootChestsLocationsLB.ValueMember = "Value";
            LootChestsLocationsLB.DataSource = LootPool.CapareLPRewardTables;

            LootCategoriesLB.DisplayMember = "DisplayName";
            LootCategoriesLB.ValueMember = "Value";
            LootCategoriesLB.DataSource = LootPool.CapareLPLootTables;

            LCPredefinedWeaponsLB.DisplayMember = "DisplayName";
            LCPredefinedWeaponsLB.ValueMember = "Value";
            LCPredefinedWeaponsLB.DataSource = LootPool.CapareLPdefinedItems;

            useraction = true;
        }

        private void LCPredefinedWeaponsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LCPredefinedWeaponsLB.SelectedItems.Count < 1) return;
            currentRHPredefineditems = LCPredefinedWeaponsLB.SelectedItem as capareLPdefinedItems;
            useraction = false;
            SetItemInfo();
            useraction = true;
        }
        private void SetItemInfo()
        {
            defnameTB.Text = currentRHPredefineditems.DefineName;
            weaponTB.Text = currentRHPredefineditems.Item;
            SpawnExactCB.Checked = currentRHPredefineditems.SpawnExact == 1 ? true : false;

            magazinesLB.DisplayMember = "DisplayName";
            magazinesLB.ValueMember = "Value";
            magazinesLB.DataSource = currentRHPredefineditems.magazines;

            attachmentsLB.DisplayMember = "DisplayName";
            attachmentsLB.ValueMember = "Value";
            attachmentsLB.DataSource = currentRHPredefineditems.attachments;

            if (currentRHPredefineditems.attachments.Count == 0)
            {
                attatchmentslistLB.DataSource = null;
                attatchmentslistLB.Refresh();
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
                    if (!LootPool.CapareLPdefinedItems.Any(x => x.DefineName == "Defined_" + l))
                    {
                        currentRHPredefineditems.DefineName = "Defined_" + l;
                        currentRHPredefineditems.Item = l;
                        SetItemInfo();
                        LootPool.isDirty = true;
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
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!currentRHPredefineditems.magazines.Any(x => x == l))
                    {
                        currentRHPredefineditems.magazines.Add(l);
                        LootPool.isDirty = true;
                    }
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            capareLPAttachment newattachemnt = new capareLPAttachment()
            {
                attachments = new BindingList<string>()
            };
            currentRHPredefineditems.attachments.Add(newattachemnt);
            LootPool.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (attachmentsLB.SelectedItems.Count < 1) return;
            int index = attachmentsLB.SelectedIndex;
            currentRHPredefineditems.attachments.Remove(currentRHLPAttachment);
            LootPool.isDirty = true;
            attachmentsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (attachmentsLB.Items.Count > 0)
                    attachmentsLB.SelectedIndex = 0;
            }
            else
            {
                attachmentsLB.SelectedIndex = index - 1;
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
                    SetItemInfo();
                    LootPool.isDirty = true;
                }
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            LootPool.CapareLPdefinedItems.Add(new capareLPdefinedItems()
            {
                DefineName = "Defined_New_Item",
                Item = "",
                magazines = new BindingList<string>(),
                attachments = new BindingList<capareLPAttachment>()
            });
            LCPredefinedWeaponsLB.SelectedIndex = -1;
            LCPredefinedWeaponsLB.SelectedIndex = LCPredefinedWeaponsLB.Items.Count - 1;
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (LCPredefinedWeaponsLB.SelectedItems.Count < 1) return;
            List<capareLPdefinedItems> removeitems = new List<capareLPdefinedItems>();
            foreach (var item in LCPredefinedWeaponsLB.SelectedItems)
            {
                removeitems.Add(item as capareLPdefinedItems);
            }
            foreach (capareLPdefinedItems removeitem in removeitems)
            {
                int index = LCPredefinedWeaponsLB.SelectedIndex;
                LootPool.CapareLPdefinedItems.Remove(currentRHPredefineditems);
                LootPool.isDirty = true;
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
        }

        private void attachmentsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (attachmentsLB.SelectedItems.Count < 1) return;
            currentRHLPAttachment = attachmentsLB.SelectedItem as capareLPAttachment;

            attatchmentslistLB.DisplayMember = "DisplayName";
            attatchmentslistLB.ValueMember = "Value";
            attatchmentslistLB.DataSource = currentRHLPAttachment.attachments;
            attatchmentslistLB.Refresh();
        }

        private void LootCategoriesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootCategoriesLB.SelectedItems.Count < 1) return;
            currentRhlploottable = LootCategoriesLB.SelectedItem as caparelploottable;
            useraction = false;

            LootCatNameTB.Text = currentRhlploottable.TableName;

            LootCatLootLB.DisplayMember = "DisplayName";
            LootCatLootLB.ValueMember = "Value";
            LootCatLootLB.DataSource = currentRhlploottable.LootItems;


            useraction = true;
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            currentRhlploottable.LootItems.Remove(LootCatLootLB.GetItemText(LootCatLootLB.SelectedItem));
            if (currentRhlploottable.LootItems.Count == 0)
                currentRhlploottable.LootItems.Add("");

            LootPool.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                RHLPdefinedItems = LootPool.CapareLPdefinedItems,
                titellabel = "Add Items from Predefined Weapons",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = true,
                isLootchest = false,
                isLootBoxList = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    if (!currentRhlploottable.LootItems.Any(x => x == weapon))
                    {
                        currentRhlploottable.LootItems.Add(weapon);
                        LootPool.isDirty = true;
                    }
                }
            }
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
                    if (!currentRhlploottable.LootItems.Any(x => x == l))
                    {
                        if (currentRhlploottable.LootItems.Count > 0 && currentRhlploottable.LootItems[0] == "")
                            currentRhlploottable.LootItems.RemoveAt(0);
                        currentRhlploottable.LootItems.Add(l);
                        LootPool.isDirty = true;
                    }
                }
            }
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            if (LootCategoriesLB.SelectedItems.Count < 1) return;
            int index = LootCategoriesLB.SelectedIndex;
            LootPool.CapareLPLootTables.Remove(currentRhlploottable);
            LootPool.isDirty = true;
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
        private void darkButton19_Click(object sender, EventArgs e)
        {
            LootPool.CapareLPLootTables.Add(new caparelploottable()
            {
                TableName = "Loot_Table_",
                LootItems = new BindingList<string>()
            });
            LCPredefinedWeaponsLB.SelectedIndex = -1;
            LCPredefinedWeaponsLB.SelectedIndex = LCPredefinedWeaponsLB.Items.Count - 1;
        }

        private void LootChestsLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootChestsLocationsLB.SelectedItems.Count < 1) return;
            CurrentRHLPRewardTables = LootChestsLocationsLB.SelectedItem as caparelprewardtable;
            useraction = false;

            RewardTableNameTB.Text = CurrentRHLPRewardTables.RewardName;

            lootLB.DisplayMember = "DisplayName";
            lootLB.ValueMember = "Value";
            lootLB.DataSource = CurrentRHLPRewardTables.Rewards;

            useraction = true;
        }
        private void RewardTableNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentRHLPRewardTables.RewardName = RewardTableNameTB.Text;
            lootLB.Refresh();
            LootPool.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            LootPool.CapareLPRewardTables.Add(new caparelprewardtable()
            {
                RewardName = "Reward_Table_NewReward",
                Rewards = new BindingList<string>()
            }
           );
            LootChestsLocationsLB.SelectedIndex = -1;
            LootChestsLocationsLB.SelectedIndex = LootChestsLocationsLB.Items.Count - 1;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (LootChestsLocationsLB.SelectedItems.Count < 1) return;
            int index = LootChestsLocationsLB.SelectedIndex;
            LootPool.CapareLPRewardTables.Remove(CurrentRHLPRewardTables);
            LootPool.isDirty = true;
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
                    CurrentRHLPRewardTables.Rewards.Add(l);
                    LootPool.isDirty = true;
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
                    CurrentRHLPRewardTables.Rewards.Add(l);
                    LootPool.isDirty = true;
                }
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                RHLPdefinedItems = LootPool.CapareLPdefinedItems,
                titellabel = "Add Items from Predefined Weapons",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = true,
                isLootchest = false,
                isLootBoxList = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    CurrentRHLPRewardTables.Rewards.Add(weapon);
                    LootPool.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems
            {
                LootTables = LootPool.CapareLPLootTables,
                titellabel = "Add Items from Loot list",
                isLootList = false,
                isRHTableList = true,
                isRewardTable = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                     CurrentRHLPRewardTables.Rewards.Add(weapon);
                    LootPool.isDirty = true;
                }
            }
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            CurrentRHLPRewardTables.Rewards.Remove(lootLB.GetItemText(lootLB.SelectedItem));
            LootPool.isDirty = true;
        }
        private void SpawnExactCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentRHPredefineditems.SpawnExact = SpawnExactCB.Checked == true ? 1 : 0;
            LootPool.isDirty = true;
        }
        private void MaxMagsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            LootPool.NumberOfExtraMagsForDefinedWeapons = (int)MaxMagsNUD.Value;
            LootPool.isDirty = true;
        }
        private void defnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentRHPredefineditems.DefineName = defnameTB.Text;
            LootPool.isDirty = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (LootPool.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(LootPool.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LootPool.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(LootPool.Filename, Path.GetDirectoryName(LootPool.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(LootPool.Filename) + ".bak", true);
                }
                LootPool.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(LootPool, options);
                File.WriteAllText(LootPool.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(LootPool.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare\\CapareLootPool");
        }
        private void LootCatNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentRhlploottable.TableName = LootCatNameTB.Text;
            LootCatLootLB.Refresh();
            LootPool.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
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
                    if (!currentRHLPAttachment.attachments.Any(x => x == l))
                    {
                        currentRHLPAttachment.attachments.Add(l);
                        LootPool.isDirty = true;
                    }
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (attatchmentslistLB.SelectedItems.Count < 1) return;
            int index = attatchmentslistLB.SelectedIndex;
            currentRHLPAttachment.attachments.Remove(attatchmentslistLB.GetItemText(attatchmentslistLB.SelectedItem));
            LootPool.isDirty = true;
            attatchmentslistLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (attatchmentslistLB.Items.Count > 0)
                    attatchmentslistLB.SelectedIndex = 0;
                else
                    attatchmentslistLB.SelectedIndex = -1;
            }
            else
            {
                attatchmentslistLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton12_Click_1(object sender, EventArgs e)
        {
            if (magazinesLB.SelectedItems.Count < 1) return;
            int index = magazinesLB.SelectedIndex;
            currentRHPredefineditems.magazines.Remove(magazinesLB.GetItemText(magazinesLB.SelectedItem));
            LootPool.isDirty = true;
            magazinesLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (magazinesLB.Items.Count > 0)
                    magazinesLB.SelectedIndex = 0;
                else
                    magazinesLB.SelectedIndex = -1;
            }
            else
            {
                magazinesLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            List<spawnabletypesType> ST = new List<spawnabletypesType>();
            foreach (Spawnabletypesconfig stc in currentproject.spawnabletypesList)
            {
                foreach (spawnabletypesType sp in stc.spawnabletypes.type)
                {
                    if (sp.name == currentRHPredefineditems.Item)
                    {
                        ST.Add(sp);
                    }
                }
            }
            spawnabletypesType useablespantypes = ST.Last();

            foreach(var item in useablespantypes.Items)
            {
                if (item is spawnabletypesTypeAttachments)
                {
                    spawnabletypesTypeAttachments attachments = item as spawnabletypesTypeAttachments;
                    capareLPAttachment NEWLIST = new capareLPAttachment();
                    NEWLIST.attachments = new BindingList<string>();
                    foreach (spawnabletypesTypeAttachmentsItem attitem in attachments.item)
                    {
                        if (attitem.name.ToLower().Contains("mag"))
                        {
                            currentRHPredefineditems.magazines.Add(attitem.name);
                            continue;
                        }
                        else
                        {
                            NEWLIST.attachments.Add(attitem.name);
                        }
                    }
                    if (NEWLIST.attachments.Count > 0)
                    {
                        currentRHPredefineditems.attachments.Add(NEWLIST);
                    }
                }
                else if (item is spawnabletypesTypeCargo)
                {
                    spawnabletypesTypeCargo attachments = item as spawnabletypesTypeCargo;
                    capareLPAttachment NEWLIST = new capareLPAttachment();
                    NEWLIST.attachments = new BindingList<string>();
                    foreach (spawnabletypesTypeCargoItem attitem in attachments.item)
                    {
                        NEWLIST.attachments.Add(attitem.name);
                    }
                    if (NEWLIST.attachments.Count > 0)
                    {
                        currentRHPredefineditems.attachments.Add(NEWLIST);
                    }
                }
            }
            
        }

        private void darkButton21_Click(object sender, EventArgs e)
        {
            List<spawnabletypesType> ST = new List<spawnabletypesType>();
            foreach (Spawnabletypesconfig stc in currentproject.spawnabletypesList)
            {
                foreach (spawnabletypesType sp in stc.spawnabletypes.type)
                {
                    if (sp.name.ToLower().StartsWith("zmb"))
                        continue;
                    if (sp.Items.Count() > 0)
                    {
                        if(sp.ContainsAttchorcargo())
                        {
                            if(ST.Any(x => x.name == sp.name))
                            {
                                spawnabletypesType remove = ST.FirstOrDefault(x => x.name == sp.name);
                                ST.Remove(remove);

                            }
                            ST.Add(sp);
                        }
                    }
                }
            }
            foreach(spawnabletypesType sp in ST)
            {
                capareLPdefinedItems newitem = new capareLPdefinedItems()
                {
                    DefineName = "Defined_" + sp.name,
                    Item = sp.name,
                    magazines = new BindingList<string>(),
                    attachments = new BindingList<capareLPAttachment>()
                };
                foreach (var item in sp.Items)
                {
                    if (item is spawnabletypesTypeAttachments)
                    {
                        spawnabletypesTypeAttachments attachments = item as spawnabletypesTypeAttachments;
                        capareLPAttachment NEWLIST = new capareLPAttachment();
                        NEWLIST.attachments = new BindingList<string>();
                        foreach (spawnabletypesTypeAttachmentsItem attitem in attachments.item)
                        {
                            if (attitem.name.ToLower().Contains("mag"))
                            {
                                newitem.magazines.Add(attitem.name);
                                continue;
                            }
                            else
                            {
                                NEWLIST.attachments.Add(attitem.name);
                            }
                        }
                        if (NEWLIST.attachments.Count > 0)
                        {
                            newitem.attachments.Add(NEWLIST);
                        }
                    }
                    else if (item is spawnabletypesTypeCargo)
                    {
                        spawnabletypesTypeCargo attachments = item as spawnabletypesTypeCargo;
                        capareLPAttachment NEWLIST = new capareLPAttachment();
                        NEWLIST.attachments = new BindingList<string>();
                        foreach (spawnabletypesTypeCargoItem attitem in attachments.item)
                        {
                            NEWLIST.attachments.Add(attitem.name);
                        }
                        if (NEWLIST.attachments.Count > 0)
                        {
                            newitem.attachments.Add(NEWLIST);
                        }
                    }
                }
                
                LootPool.CapareLPdefinedItems.Add(newitem);
                LootPool.isDirty = true;
            }
        }
    }
}
