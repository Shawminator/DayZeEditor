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
        public string RagTysonBaseBuildingPath { get; private set; }
        public RagBasebuilding RagBasebuilding { get; set; }
        public string Projectname { get; private set; }

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
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (RagBasebuilding.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(RagBasebuilding.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RagBasebuilding.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(RagBasebuilding.Filename, Path.GetDirectoryName(RagBasebuilding.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(RagBasebuilding.Filename) + ".bak", true);
                }
                RagBasebuilding.isDirty = false;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = { new BoolConverter() }
                };
                string jsonString = JsonSerializer.Serialize(RagBasebuilding, options);
                File.WriteAllText(RagBasebuilding.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(RagBasebuilding.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_BaseBuilding");
        }
        private void RagTysonBaseBuildingManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RagBasebuilding.isDirty)
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
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();
            bool needtosave = false;
            useraction = false;
            RagTysonBaseBuildingPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_BaseBuilding\\RaG_BaseBuilding.json";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() }
            };
            RagBasebuilding = JsonSerializer.Deserialize<RagBasebuilding>(File.ReadAllText(RagTysonBaseBuildingPath), options);
            RagBasebuilding.isDirty = false;
            RagBasebuilding.Filename = RagTysonBaseBuildingPath;
            if (RagBasebuilding.checkver())
                needtosave = true;

            PropertyInfo[] properties = typeof(RagBasebuilding).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "version" || property.Name == "Filename" || property.Name == "BaseBuildTools" || property.Name == "BaseDismantleTools" || property.Name == "BaseDestroyTools" || property.Name == "Materials" || property.Name == "Crafting")
                    continue;
                BBPConfigLB.Items.Add(property.Name);
            }
            BBPConfigLB.SelectedIndex = 0;
            List<string> bbplists = Helper.GetPropertiesNameOfClass<BindingList<string>>(RagBasebuilding);
            BBPlistsLB.DisplayMember = "DisplayName";
            BBPlistsLB.ValueMember = "Value";
            BBPlistsLB.DataSource = bbplists;

            PropertyInfo[] properties2 = typeof(RaG_BB_Materials).GetProperties();
            foreach (PropertyInfo property in properties2)
            {
                BBMaterailsLB.Items.Add(property.Name);
            }
            BBMaterailsLB.SelectedIndex = 0;
            PropertyInfo[] properties3 = typeof(RaG_BB_Crafting).GetProperties();
            foreach (PropertyInfo property in properties3)
            {
                BBPCraftingLB.Items.Add(property.Name);
            }
            BBPCraftingLB.SelectedIndex = 0;
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
            if (RagBasebuilding.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(bool))
            {
                BBPBoolsCB.Visible = true;
                BBPBoolsCB.Checked = (bool)Helper.GetPropValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem));
            }
            else if (RagBasebuilding.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(int))
            {
                BBPIntsNUD.Visible = true;
                BBPIntsNUD.Value = (int)Helper.GetPropValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem));
            }
            else if (RagBasebuilding.GetType().GetProperty(BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)).PropertyType == typeof(float))
            {
                BBPDecimalNUD.Visible = true;
                BBPDecimalNUD.Value = Convert.ToDecimal(Helper.GetPropValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem)));
            }
            useraction = true;
        }
        private void BBPBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), BBPBoolsCB.Checked);
            RagBasebuilding.isDirty = true;
        }

        private void BBPIntsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), (int)BBPIntsNUD.Value);
            RagBasebuilding.isDirty = true;
        }
        private void BBPDecimalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetSingleValue(RagBasebuilding, BBPConfigLB.GetItemText(BBPConfigLB.SelectedItem), Convert.ToSingle(BBPDecimalNUD.Value));
            RagBasebuilding.isDirty = true;
        }
        private void BBPlistsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPlistsLB.SelectedItems.Count < 1) return;
            useraction = false;
            CurrentList = Helper.GetPropValue(RagBasebuilding, BBPlistsLB.GetItemText(BBPlistsLB.SelectedItem)) as BindingList<string>;
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
                RagBasebuilding.isDirty = true;
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
                RagBasebuilding.isDirty = true;
            }
            CurrentBBPListLB.Refresh();
        }

        private void BBMaterailsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBMaterailsLB.SelectedItems.Count < 1) return;
            useraction = false;
            PropertyInfo[] properties = typeof(RaG_BB_Materials).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == BBMaterailsLB.GetItemText(BBMaterailsLB.SelectedItem))
                {
                    BBMaterialsNUD.Value = Convert.ToInt32(Helper.GetPropValue(RagBasebuilding.Materials, property.Name));
                }
            }
            useraction = true;
        }
        private void BBMaterialsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(RagBasebuilding.Materials, BBPCraftingLB.GetItemText(BBMaterailsLB.SelectedItem), (int)BBMaterialsNUD.Value);
            RagBasebuilding.isDirty = true;
        }


        private void BBPCraftingLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPCraftingLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPCraftingNUD.Visible = false;
            BBPCraftingCB.Visible = false;
            if (RagBasebuilding.Crafting.GetType().GetProperty(BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem)).PropertyType == typeof(bool))
            {
                BBPCraftingCB.Visible = true;
                BBPCraftingCB.Checked = (bool)Helper.GetPropValue(RagBasebuilding.Crafting, BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem));
            }
            else if (RagBasebuilding.Crafting.GetType().GetProperty(BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem)).PropertyType == typeof(float))
            {
                BBPCraftingNUD.Visible = true;
                BBPCraftingNUD.Value = Convert.ToDecimal(Helper.GetPropValue(RagBasebuilding.Crafting, BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem)));
            }
            useraction = true;
        }
        private void BBPCraftingNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetSingleValue(RagBasebuilding.Crafting, BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem), Convert.ToSingle(BBPCraftingNUD.Value));
            RagBasebuilding.isDirty = true;
        }

        private void BBPCraftingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(RagBasebuilding.Crafting, BBPCraftingLB.GetItemText(BBPCraftingLB.SelectedItem), BBPCraftingCB.Checked);
            RagBasebuilding.isDirty = true;
        }
    }
}
