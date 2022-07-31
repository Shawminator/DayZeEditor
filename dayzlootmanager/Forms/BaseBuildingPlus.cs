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
    public partial class BaseBuildingPlus : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public BindingList<string> CurrentList { get; set; }
        public BBP_Cementmixerlocations BBP_Cementmixerlocations { get; set; }
        public string BBPSettingsPath { get; private set; }
        public BBPSettings BBPSettings;
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
        public BaseBuildingPlus()
        {
            InitializeComponent();
        }
        private void BaseBuildingPlus_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            BBPSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\BaseBuildingPlus\\BBP_Settings.json";
            BBPSettings = JsonSerializer.Deserialize<BBPSettings>(File.ReadAllText(BBPSettingsPath));
            BBPSettings.isDirty = false;
            BBPSettings.Filename = BBPSettingsPath;

            string[] boolignorenames = new string[] 
            {
                "BBP_BuildTime",
                "BBP_DismantleTime",
                "BBP_Tier1RaidTime",
                "BBP_Tier2RaidTime",
                "BBP_Tier3RaidTime",
                "BBP_Tier1RaidToolDamage",
                "BBP_Tier2RaidToolDamage",
                "BBP_Tier3RaidToolDamage",
                "BBP_BuildTools",
                "BBP_DismantleTools",
                "BBP_BaseBuildToolDamage",
                "BBP_BaseDismantleToolDamage",
                "BBP_Tier1RaidTools",
                "BBP_Tier2RaidTools",
                "BBP_Tier3RaidTools",
                "BBP_CementMixerTime",
                "BBP_CementMixerLocations"
            };
            List<string> bbpbools = Helper.GetPropertiesNameOfClass<int>(BBPSettings, boolignorenames);
            BBPBoolsLB.DisplayMember = "DisplayName";
            BBPBoolsLB.ValueMember = "Value";
            BBPBoolsLB.DataSource = bbpbools;

            string[] intIgnoreNames = new string[] { "BBP_BuildTime", "BBP_DismantleTime", "BBP_Tier1RaidTime", "BBP_Tier2RaidTime", "BBP_Tier3RaidTime", "BBP_BaseBuildToolDamage", "BBP_BaseDismantleToolDamage", "BBP_Tier1RaidToolDamage", "BBP_Tier2RaidToolDamage", "BBP_Tier3RaidToolDamage", "BBP_CementMixerTime" };
            List<string> questints = Helper.GetPropertiesNameOfClass<int>(BBPSettings, intIgnoreNames, true);
            BBPIntsLB.DisplayMember = "DisplayName";
            BBPIntsLB.ValueMember = "Value";
            BBPIntsLB.DataSource = questints;

            List<string> bbplists = Helper.GetPropertiesNameOfClass<BindingList<string>>(BBPSettings);
            BBPlistsLB.DisplayMember = "DisplayName";
            BBPlistsLB.ValueMember = "Value";
            BBPlistsLB.DataSource = bbplists;

            BBP_CementMixerLocationsLB.DisplayMember = "DisplayName";
            BBP_CementMixerLocationsLB.ValueMember = "Value";
            BBP_CementMixerLocationsLB.DataSource = BBPSettings.BBP_CementMixerLocations;

            useraction = true;
        }
        private void BBPBoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPBoolsLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPBoolsCB.Checked = (int)Helper.GetPropValue(BBPSettings, BBPBoolsLB.GetItemText(BBPBoolsLB.SelectedItem)) == 1 ? true : false;
            useraction = true;
        }
        private void BBPBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetBoolValue(BBPSettings, BBPBoolsLB.GetItemText(BBPBoolsLB.SelectedItem), BBPBoolsCB.Checked);
            BBPSettings.isDirty = true;
        }
        private void BBPIntsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPIntsLB.SelectedItems.Count < 1) return;
            useraction = false;
            BBPIntsNUD.Value = (int)Helper.GetPropValue(BBPSettings, BBPIntsLB.GetItemText(BBPIntsLB.SelectedItem));
            useraction = true;
        }
        private void BBPIntsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helper.SetIntValue(BBPSettings, BBPIntsLB.GetItemText(BBPIntsLB.SelectedItem), (int)BBPIntsNUD.Value);
            BBPSettings.isDirty = true;
        }
        private void BBPlistsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBPlistsLB.SelectedItems.Count < 1) return;
            useraction = false;
            CurrentList = Helper.GetPropValue(BBPSettings, BBPlistsLB.GetItemText(BBPlistsLB.SelectedItem)) as BindingList<string>;
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
                    CurrentList.Add(l);
                   
                }
                BBPSettings.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            if (CurrentBBPListLB.SelectedItems.Count < 1) return;
            CurrentList.Remove(CurrentBBPListLB.GetItemText(CurrentBBPListLB.SelectedItems[0]));
            BBPSettings.isDirty = true;
            CurrentBBPListLB.Refresh();
            if (CurrentBBPListLB.Items.Count == 0)
                CurrentBBPListLB.SelectedIndex = -1;
            else
                CurrentBBPListLB.SelectedIndex = 0;
        }
        private void BBP_CementMixerLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BBP_CementMixerLocationsLB.SelectedItems.Count < 1) return;
            BBP_Cementmixerlocations = BBP_CementMixerLocationsLB.SelectedItem as BBP_Cementmixerlocations;
            useraction = false;
            BBPMixerPOSXNUD.Value = (decimal)BBP_Cementmixerlocations.position[0];
            BBPMixerPOSYNUD.Value = (decimal)BBP_Cementmixerlocations.position[1];
            BBPMixerPOSZNUD.Value = (decimal)BBP_Cementmixerlocations.position[2];

            BBPMixerOXNUD.Value = (decimal)BBP_Cementmixerlocations.orientation[0];
            BBPMixerOYNUD.Value = (decimal)BBP_Cementmixerlocations.orientation[1];
            BBPMixerOZNUD.Value = (decimal)BBP_Cementmixerlocations.orientation[2];

            useraction = true;

        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            BBP_Cementmixerlocations newmixer = new BBP_Cementmixerlocations()
            {
                position = new float[] {0,0,0 },
                orientation = new float[] {0,0,0 }
            };
            BBPSettings.BBP_CementMixerLocations.Add(newmixer);
            BBPSettings.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (BBP_CementMixerLocationsLB.SelectedItems.Count < 1) return;
            BBPSettings.BBP_CementMixerLocations.Remove(BBP_Cementmixerlocations);
            BBPSettings.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = JsonSerializer.Deserialize<DZE>(File.ReadAllText(filePath));
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        BBPSettings.BBP_CementMixerLocations.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        BBP_Cementmixerlocations newmix = new BBP_Cementmixerlocations()
                        {
                            position = eo.Position,
                            orientation = eo.Orientation
                        };
                        BBPSettings.BBP_CementMixerLocations.Add(newmix);
                    }
                    BBP_CementMixerLocationsLB.SelectedIndex = -1;
                    BBP_CementMixerLocationsLB.SelectedIndex = BBP_CementMixerLocationsLB.Items.Count - 1;
                    BBP_CementMixerLocationsLB.Refresh();
                    BBPSettings.isDirty = true;
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (BBPSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(BBPSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(BBPSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(BBPSettings.Filename, Path.GetDirectoryName(BBPSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(BBPSettings.Filename) + ".bak", true);
                }
                BBPSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(BBPSettings, options);
                File.WriteAllText(BBPSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(BBPSettings.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\BaseBuildingPlus");
        }
        private void BBPMixerPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.position[0] = (float)BBPMixerPOSXNUD.Value;
            BBPSettings.isDirty = true;
        }
        private void BBPMixerPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.position[1] = (float)BBPMixerPOSYNUD.Value;
            BBPSettings.isDirty = true;
        }
        private void BBPMixerPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.position[2] = (float)BBPMixerPOSZNUD.Value;
            BBPSettings.isDirty = true;
        }
        private void BBPMixerOXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.orientation[0] = (float)BBPMixerOXNUD.Value;
            BBPSettings.isDirty = true;
        }
        private void BBPMixerOYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.orientation[1] = (float)BBPMixerOYNUD.Value;
            BBPSettings.isDirty = true;
        }
        private void BBPMixerOZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            BBP_Cementmixerlocations.orientation[2] = (float)BBPMixerOZNUD.Value;
            BBPSettings.isDirty = true;
        }
    }
}
