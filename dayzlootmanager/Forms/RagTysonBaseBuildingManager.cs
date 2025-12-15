using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class RagTysonBaseBuildingManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public string Projectname { get; private set; }

        public string RaGBBConfigPath { get; private set; }
        public RaGBBConfig RaGBBConfig { get; set; }
        public string RaGCoreConfigPath { get; private set; }
        public RaGCoreConfig RaGCoreConfig { get; set; }

        public RaGBBPartCost creentRaGBBPartCost { get;set; }
        public RaGBBCraftToggle currentRaGBBCraftToggle { get; set; }

        private bool useraction;

        public BindingList<string> CurrentList { get; private set; }

        public RagTysonBaseBuildingManager()
        {
            InitializeComponent();
        }
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
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (RaGBBConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(RaGBBConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RaGBBConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(RaGBBConfig.Filename, Path.GetDirectoryName(RaGBBConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(RaGBBConfig.Filename) + ".bak", true);
                }
                RaGBBConfig.isDirty = false;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = { new BoolConverter() }
                };
                string jsonString = JsonSerializer.Serialize(RaGBBConfig, options);
                File.WriteAllText(RaGBBConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(RaGBBConfig.Filename));
            }
            if(RaGCoreConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(RaGCoreConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RaGCoreConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(RaGCoreConfig.Filename, Path.GetDirectoryName(RaGCoreConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(RaGCoreConfig.Filename) + ".bak", true);
                }
                RaGCoreConfig.isDirty = false;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = { new BoolConverter() }
                };
                string jsonString = JsonSerializer.Serialize(RaGCoreConfig, options);
                File.WriteAllText(RaGCoreConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(RaGCoreConfig.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_Core\\Configs");
        }
        private void RagTysonBaseBuildingManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (RaGBBConfig.isDirty || RaGCoreConfig.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
        private void RagTysonBaseBuildingManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();
            bool needtosave = false;
            useraction = false;
            RaGBBConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_Core\\Configs\\RaG_BaseBuilding\\RaG_BaseBuilding.json";
            RaGCoreConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_Core\\Configs\\RaG_Core\\RaG_Core.json";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() }
            };
            RaGBBConfig = JsonSerializer.Deserialize<RaGBBConfig>(File.ReadAllText(RaGBBConfigPath), options);
            RaGBBConfig.isDirty = false;
            RaGBBConfig.Filename = RaGBBConfigPath;
            if (RaGBBConfig.checkver())
                needtosave = true;

            PropertyInfo[] properties = typeof(RaGBBConfig).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Version" || property.Name == "Filename" || property.Name == "BaseBuildTools" || property.Name == "BaseDismantleTools" || property.Name == "BaseDestroyTools" || property.Name == "PartCosts" || property.Name == "CraftToggles")
                    continue;
                BBPConfigLB.Items.Add(property.Name);
            }
            BBPConfigLB.SelectedIndex = 0;
            List<string> bbplists = Helper.GetPropertiesNameOfClass<BindingList<string>>(RaGBBConfig);
            BBPlistsLB.DisplayMember = "DisplayName";
            BBPlistsLB.ValueMember = "Value";
            BBPlistsLB.DataSource = bbplists;

            BBMaterailsLB.DisplayMember = "DisplayName";
            BBMaterailsLB.ValueMember = "Value";
            BBMaterailsLB.DataSource = RaGBBConfig.PartCosts;

            BBPCraftingLB.DisplayMember = "DisplayName";
            BBPCraftingLB.ValueMember = "Value";
            BBPCraftingLB.DataSource = RaGBBConfig.CraftToggles;

            RaGCoreConfig = JsonSerializer.Deserialize<RaGCoreConfig>(File.ReadAllText(RaGCoreConfigPath), options);
            RaGCoreConfig.isDirty = false;
            RaGCoreConfig.Filename = RaGCoreConfigPath;
            if (RaGCoreConfig.checkver())
                needtosave = true;

            PropertyInfo[] properties2 = typeof(RaGCoreConfig).GetProperties();
            foreach (PropertyInfo property in properties2)
            {
                if (property.Name == "Version" || property.Name == "Filename")
                    continue;
                RAGCoreConfigLB.Items.Add(property.Name);
            }
            RAGCoreConfigLB.SelectedIndex = 0;

            useraction = true;

            if (needtosave)
            {
                SaveFile();
            }
        }
        private void BBPConfigLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPConfigLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPBoolsCB.Visible = false;
            BBPIntsNUD.Visible = false;
            BBPDecimalNUD.Visible = false;
            if (RaGBBConfig.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(bool))
            {
                BBPBoolsCB.Visible = true;
                BBPBoolsCB.Checked = (bool)Helper.GetPropValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem));
            }
            else if (RaGBBConfig.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(int))
            {
                BBPIntsNUD.Visible = true;
                BBPIntsNUD.Value = (int)Helper.GetPropValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem));
            }
            else if (RaGBBConfig.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(float))
            {
                BBPDecimalNUD.Visible = true;
                BBPDecimalNUD.Value = Convert.ToDecimal(Helper.GetPropValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)));
            }
            useraction = true;
        }
        private void BBPBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), BBPBoolsCB.Checked);
            RaGBBConfig.isDirty = true;
        }

        private void BBPIntsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), (int)BBPIntsNUD.Value);
            RaGBBConfig.isDirty = true;
        }
        private void BBPDecimalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetSingleValue(RaGBBConfig, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), Convert.ToSingle(BBPDecimalNUD.Value));
            RaGBBConfig.isDirty = true;
        }
        private void BBPlistsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPlistsLB.SelectedItems.Count < 1) return;
            useraction = false;
            CurrentList = Helper.GetPropValue(RaGBBConfig, BBPlistsLB.GetItemText(BBPlistsLB.SelectedItem)) as BindingList<string>;
            CurrentBBPListLB.DisplayMember = "DisplayName";
            CurrentBBPListLB.ValueMember = "Value";
            CurrentBBPListLB.DataSource = CurrentList;
            useraction = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
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
                    CurrentList.Add(l);

                }
                RaGBBConfig.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            if (CurrentBBPListLB.SelectedItems.Count < 1) return;
            List<string> removeitems = new List<string>();
            foreach (var item in CurrentBBPListLB.SelectedItems)
            {
                removeitems.Add(item.ToString());
            }
            foreach (string removeitem in removeitems)
            {
                CurrentList.Remove(removeitem);
                RaGBBConfig.isDirty = true;
            }
            CurrentBBPListLB.Refresh();
        }

        private void BBMaterailsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBMaterailsLB.SelectedItems.Count < 1) return;
            useraction = false;
            creentRaGBBPartCost = BBMaterailsLB.SelectedItem as RaGBBPartCost;
            BBPNAILSNUD.Value = (decimal)creentRaGBBPartCost.NAILS;
            BBPLANKSNUD.Value = (decimal)creentRaGBBPartCost.PLANKS;
            BBPLOGSNUD.Value = (decimal)creentRaGBBPartCost.LOGS;
            useraction = true;
        }
        private void BBPCraftingNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            creentRaGBBPartCost.NAILS = (float)BBPNAILSNUD.Value;
            RaGBBConfig.isDirty = true;
        }
        private void BBMaterialsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            creentRaGBBPartCost.PLANKS = (float)BBPLANKSNUD.Value;
            RaGBBConfig.isDirty = true;
        }

        private void BBPLOGSNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            creentRaGBBPartCost.LOGS = (float)BBPLOGSNUD.Value;
            RaGBBConfig.isDirty = true;
        }
        private void BBPCraftingLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPCraftingLB.SelectedItems.Count < 1) return;
            useraction = false;
            currentRaGBBCraftToggle = BBPCraftingLB.SelectedItem as RaGBBCraftToggle;
            BBPCraftingCB.Checked = currentRaGBBCraftToggle.ENABLED;
            useraction = true;
        }
        private void BBPCraftingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentRaGBBCraftToggle.ENABLED = BBPCraftingCB.Checked;
            RaGBBConfig.isDirty = true;
        }

        private void RAGCoreConfigLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RAGCoreConfigLB.SelectedItems.Count < 1) return;
            useraction = false;
            RAGCoreConfigCB.Checked = (bool)Helper.GetPropValue(RaGCoreConfig, RAGCoreConfigLB.GetItemText(RAGCoreConfigLB.SelectedItem));
            useraction = true;
        }

        private void RAGCoreConfigCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(RaGCoreConfig, RAGCoreConfigLB.GetItemText(RAGCoreConfigLB.SelectedItem), RAGCoreConfigCB.Checked);
            RaGCoreConfig.isDirty = true;
        }
    }
}
