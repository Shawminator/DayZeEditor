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

            useraction = false;
            RagTysonBaseBuildingPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\RaG_BaseBuilding\\RaG_BaseBuilding.json";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() }
            };
            RagBasebuilding = JsonSerializer.Deserialize<RagBasebuilding>(File.ReadAllText(RagTysonBaseBuildingPath), options);
            RagBasebuilding.isDirty = false;
            RagBasebuilding.Filename = RagTysonBaseBuildingPath;

            List<string> bbpbools = Helper.GetPropertiesNameOfClass<bool>(RagBasebuilding);
            BBPBoolsLB.DisplayMember = "DisplayName";
            BBPBoolsLB.ValueMember = "Value";
            BBPBoolsLB.DataSource = bbpbools;

            List<string> questints = Helper.GetPropertiesNameOfClass<int>(RagBasebuilding);
            BBPIntsLB.DisplayMember = "DisplayName";
            BBPIntsLB.ValueMember = "Value";
            BBPIntsLB.DataSource = questints;

            List<string> bbplists = Helper.GetPropertiesNameOfClass<BindingList<string>>(RagBasebuilding);
            BBPlistsLB.DisplayMember = "DisplayName";
            BBPlistsLB.ValueMember = "Value";
            BBPlistsLB.DataSource = bbplists;

            useraction = true;
        }
        private void BBPBoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPBoolsLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPBoolsCB.Checked = (bool)Helper.GetPropValue(RagBasebuilding, BBPBoolsLB.GetItemText(BBPBoolsLB.SelectedItem));
            useraction = true;
        }
        private void BBPBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(RagBasebuilding, BBPBoolsLB.GetItemText(BBPBoolsLB.SelectedItem), BBPBoolsCB.Checked);
            RagBasebuilding.isDirty = true;
        }
        private void BBPIntsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPIntsLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPIntsNUD.Value = (int)Helper.GetPropValue(RagBasebuilding, BBPIntsLB.GetItemText(BBPIntsLB.SelectedItem));
            useraction = true;
        }
        private void BBPIntsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(RagBasebuilding, BBPIntsLB.GetItemText(BBPIntsLB.SelectedItem), (int)BBPIntsNUD.Value);
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
    }
}
