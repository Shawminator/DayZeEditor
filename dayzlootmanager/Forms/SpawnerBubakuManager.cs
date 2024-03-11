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
    public partial class SpawnerBubakuManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string SpawnerBubakuConfigPath { get; private set; }
        public SpawnerBubaku SpawnerBubakuConfig;
        public string Projectname;
        private Possibleboxitem currentPossibleboxitem;
        private Possibleboxposition currentPossibleboxposition;

        public Bubaklocation CurrentBubaklocation { get; private set; }

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

        public SpawnerBubakuManager()
        {
            InitializeComponent();
        }

        private void SpawnerBukakuManager_Load(object sender, EventArgs e)
        {
            useraction = false;

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            SpawnerBubakuConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\SpawnerBubaku\\SpawnerBubaku.json";
            SpawnerBubakuConfig = JsonSerializer.Deserialize<SpawnerBubaku>(File.ReadAllText(SpawnerBubakuConfigPath));
            SpawnerBubakuConfig.isDirty = false;
            SpawnerBubakuConfig.Filename = SpawnerBubakuConfigPath;

            LogLevelNUD.Value = (int)SpawnerBubakuConfig.getLogLevel();

            SpawnerBukakuLocationsLB.DisplayMember = "DisplayName";
            SpawnerBukakuLocationsLB.ValueMember = "Value";
            SpawnerBukakuLocationsLB.DataSource = SpawnerBubakuConfig.BubakLocations;

            useraction = true;
        }
        private void MysteryBoxManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SpawnerBubakuConfig.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (SpawnerBubakuConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(SpawnerBubakuConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SpawnerBubakuConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SpawnerBubakuConfig.Filename, Path.GetDirectoryName(SpawnerBubakuConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SpawnerBubakuConfig.Filename) + ".bak", true);
                }
                SpawnerBubakuConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(SpawnerBubakuConfig, options);
                File.WriteAllText(SpawnerBubakuConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SpawnerBubakuConfig.Filename));
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
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }

        private void LogLevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnerBubakuConfig.SetLogLevel((int)LogLevelNUD.Value);
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            SpawnerBubakuConfig.AddNewLocation();
            SpawnerBukakuLocationsLB.Refresh();
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    SpawnerBubakuConfig.AddNewLocation(importfile);
                    SpawnerBukakuLocationsLB.Refresh();
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (SpawnerBukakuLocationsLB.Items.Count <= 0) return;
            SpawnerBubakuConfig.removeLocation(SpawnerBukakuLocationsLB.SelectedItem as Bubaklocation);
            SpawnerBukakuLocationsLB.Refresh();
        }

        private void SpawnerBukakuLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnerBukakuLocationsLB.SelectedItems.Count < 1) return;
            CurrentBubaklocation = SpawnerBukakuLocationsLB.SelectedItem as Bubaklocation;
            useraction = false;
            BubakLocationNameTB.Text = CurrentBubaklocation.GetName();
            BubakLocationWorkingHoursStartNUD.Value = CurrentBubaklocation.getworkinghours()[0];
            BubakLocationWorkingHoursEndNUD.Value = CurrentBubaklocation.getworkinghours()[1];
            BukakuPosRot posrot = CurrentBubaklocation.gettriggerposition();
            BubakLocationTriggerPosXNUD.Value = posrot.Position[0];
            BubakLocationTriggerPosYNUD.Value = posrot.Position[1];
            BubakLocationTriggerPosZNUD.Value = posrot.Position[2];
            
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked = posrot.RotationSpecifioed)
            {
                BubakLocationTriggerRotXNUD.Value = posrot.Rotation[0];
                BubakLocationTriggerRotYNUD.Value = posrot.Rotation[1];
                BubakLocationTriggerRotZNUD.Value = posrot.Rotation[2];
            }


            useraction = true;
        }

        private void BubakLocationTriggerPositionRotSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            BukakuPosRot posrot = CurrentBubaklocation.gettriggerposition();
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked)
            {
                TrigerposRotationLabel.Visible = true;
                BubakLocationTriggerRotXNUD.Visible = true;
                BubakLocationTriggerRotYNUD.Visible = true;
                BubakLocationTriggerRotZNUD.Visible = true;
                posrot.RotationSpecifioed = true;
                BubakLocationTriggerRotXNUD.Value = posrot.Rotation[0];
                BubakLocationTriggerRotYNUD.Value = posrot.Rotation[1];
                BubakLocationTriggerRotZNUD.Value = posrot.Rotation[2];
                   
            }
            else
            {
                posrot.RotationSpecifioed = false;
                TrigerposRotationLabel.Visible = false;
                BubakLocationTriggerRotXNUD.Visible = false;
                BubakLocationTriggerRotYNUD.Visible = false;
                BubakLocationTriggerRotZNUD.Visible = false;
            }
            CurrentBubaklocation.setTriggerposition(posrot);
            SpawnerBubakuConfig.isDirty = true;
        }
    }
}
