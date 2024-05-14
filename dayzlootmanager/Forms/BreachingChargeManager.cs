using Cyotek.Windows.Forms;
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

namespace DayZeEditor
{
    public partial class BreachingChargeManager : DarkForm
    {
        public Project currentproject { get; set; }
        public Breachingcharge Breachingcharge { get; set; }
        public string breachingchargePath { get; set; }
        public string Projectname { get; private set; }

        private TypesFile vanillatypes;

        public List<TypesFile> ModTypes { get; private set; }
        public Charge Currentcharge { get; set; }
        public Tier CurrentTier { get; set; }

        private bool useraction;

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
        public BreachingChargeManager()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\BreachingCharge");
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (Breachingcharge.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(Breachingcharge.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Breachingcharge.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(Breachingcharge.Filename, Path.GetDirectoryName(Breachingcharge.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Breachingcharge.Filename) + ".bak", true);
                }
                Breachingcharge.ConvertListToDict();
                Breachingcharge.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(Breachingcharge, options);
                File.WriteAllText(Breachingcharge.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Breachingcharge.Filename));
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
        private void BreachingChargeManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            breachingchargePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\BreachingCharge\\breachingcharge.json";
            Breachingcharge = JsonSerializer.Deserialize<Breachingcharge>(File.ReadAllText(breachingchargePath));
            Breachingcharge.ConvertDicttoList();
            Breachingcharge.isDirty = false;
            Breachingcharge.Filename = breachingchargePath;

            CreateLogsCB.Checked = Breachingcharge.CreateLogs == 1 ? true : false;

            BCChargesLB.DisplayMember = "DisplayName";
            BCChargesLB.ValueMember = "Value";
            BCChargesLB.DataSource = Breachingcharge.Charges;

            SetupChargelist();

            TiersLB.DisplayMember = "DisplayName";
            TiersLB.ValueMember = "Value";
            TiersLB.DataSource = Breachingcharge.Tiers;

            useraction = true;
        }
        private void SetupChargelist()
        {
            List<Charge> chargelist = new List<Charge>(Breachingcharge.Charges);
            AcceptedChargesCB.DisplayMember = "DisplayName";
            AcceptedChargesCB.ValueMember = "Value";
            AcceptedChargesCB.DataSource = chargelist;
        }
        private void CreateLogsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Breachingcharge.CreateLogs = CreateLogsCB.Checked == true ? 1 : 0;
            Breachingcharge.isDirty = true;
        }
        private void BCChargesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BCChargesLB.SelectedItems.Count == 0) return;
            useraction = false;
            Currentcharge = BCChargesLB.SelectedItem as Charge;

            ChargeLightColorStartPB.Invalidate();
            ChargeLightColorHalfwayPB.Invalidate();
            ChargeLightColorEndPB.Invalidate();
            ChargeClassNameTB.Text = Currentcharge.Classname;
            ChargeDamageToObjectsNUD.Value = Currentcharge.DamageToObjects;
            ChargeDamageToDestroyableObjectsRadiusNUD.Value = Currentcharge.DamageToPlayersRadius;
            ChargeVerticalDistanceModeObjectsNUD.Value = Currentcharge.VerticalDistanceModeObjects;
            ChargeMaxVerticalDistanceObjectsNUD.Value = Currentcharge.MaxVerticalDistanceObjects;
            ChargeMaxDamageToPlayersNUD.Value = Currentcharge.MaxDamageToPlayers;
            ChargeMinDamageToPlayersNUD.Value = Currentcharge.MinDamageToPlayers;
            ChargeDamageToPlayersRadiusNUD.Value = Currentcharge.DamageToPlayersRadius;
            ChargeMaxDamageToPlayersRadiusNUD.Value = Currentcharge.MaxDamageToPlayersRadius;
            ChargeMaxVerticalDistancePlayersNUD.Value = Currentcharge.MaxVerticalDistancePlayers;
            ChargeOnlyDestroyLocksCB.Checked = Currentcharge.OnlyDestroyLocks == 1 ? true : false;
            ChargeDeleteObjectsDirectlyCB.Checked = Currentcharge.DeleteObjectsDirectly == 1 ? true : false;
            ChargeDestroyLocksFirstCB.Checked = Currentcharge.DestroyLocksFirst == 1 ? true : false;
            ChargeDestroyOtherChargesCB.Checked = Currentcharge.DestroyOtherCharges == 1 ? true : false;
            ChargeMinPlacementDistanceNUD.Value = Currentcharge.PlacementDistance;
            ChargeMinPlacementDistanceNUD.Value = Currentcharge.MinPlacementDistance;
            ChargeToolDamageOnDefuseNUD.Value = Currentcharge.ToolDamageOnDefuse;
            ChargeTimeToPlantNUD.Value = Currentcharge.TimeToPlant;
            ChargeTimeToExplodeNUD.Value = Currentcharge.TimeToExplode;
            ChargeTimeToDefuseNUD.Value = Currentcharge.TimeToDefuse;
            ChargeLightBrightnessNUD.Value = Currentcharge.LightBrightness;
            CharegLightRadiusNUD.Value = Currentcharge.LightRadius;
            ChargeBeepingSoundSetTB.Text = Currentcharge.BeepingSoundSet;
            ChargeBeepingSoundEndTimeNUD.Value = Currentcharge.BeepingSoundEndTime;
            ChargeSwitchIntervalNUD.Value = Currentcharge.SwitchInterval;
            textBox1.Text = Currentcharge.ExplosionSoundSet;

            if (Currentcharge.DefuseTools.Count == 1 && Currentcharge.DefuseTools[0] == "Unarmed")
            {
                ChargeDefusetoolsLB.Visible = false;
            }
            else
            {
                darkButton31.Visible = true;
                darkButton32.Visible = true;
                ChargeDefusetoolsLB.Visible = true;
                ChargeDefusetoolsLB.DisplayMember = "DisplayName";
                ChargeDefusetoolsLB.ValueMember = "Value";
                ChargeDefusetoolsLB.DataSource = Currentcharge.DefuseTools;
            }

            useraction = true;
        }
        private void ChargeLightColorStartPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Helper.ConverToColor(Currentcharge.LightColorStart);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                Currentcharge.LightColorStart = Helper.convertToRGBFloat(cpick.Color);
                ChargeLightColorStartPB.Invalidate();
                Breachingcharge.isDirty = true;
            }
        }
        private void ChargeLightColorStartPB_Paint(object sender, PaintEventArgs e)
        {
            if (Currentcharge == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Helper.ConverToColor(Currentcharge.LightColorStart);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void ChargeLightColorHalfwayPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Helper.ConverToColor(Currentcharge.LightColorHalfway);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                Currentcharge.LightColorHalfway = Helper.convertToRGBFloat(cpick.Color);
                ChargeLightColorHalfwayPB.Invalidate();
                Breachingcharge.isDirty = true;
            }
        }
        private void ChargeLightColorHalfwayPB_Paint(object sender, PaintEventArgs e)
        {
            if (Currentcharge == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Helper.ConverToColor(Currentcharge.LightColorHalfway);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void ChargeLightColorEndPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Helper.ConverToColor(Currentcharge.LightColorEnd);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                Currentcharge.LightColorEnd = Helper.convertToRGBFloat(cpick.Color);
                ChargeLightColorEndPB.Invalidate();
                Breachingcharge.isDirty = true;
            }
        }
        private void ChargeLightColorEndPB_Paint(object sender, PaintEventArgs e)
        {
            if (Currentcharge == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Helper.ConverToColor(Currentcharge.LightColorEnd);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void TiersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TiersLB.SelectedItems.Count == 0) return;
            useraction = false;
            CurrentTier = TiersLB.SelectedItem as Tier;
            TiersNameTB.Text = CurrentTier.Name;
            TiersHealthNUD.Value = (int)CurrentTier.Health;

            TiersAcceptedChargesLB.DisplayMember = "DisplayName";
            TiersAcceptedChargesLB.ValueMember = "Value";
            TiersAcceptedChargesLB.DataSource = CurrentTier.AcceptedChargeTypes;

            DestroyableObjectsLB.DisplayMember = "DisplayName";
            DestroyableObjectsLB.ValueMember = "Value";
            DestroyableObjectsLB.DataSource = CurrentTier.TierDestroyableObjectsList;

            useraction = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (CurrentTier == null) { return; }
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!CurrentTier.TierDestroyableObjectsList.Contains(l))
                    {
                        CurrentTier.TierDestroyableObjectsList.Add(l);
                    }
                    else
                    {
                        MessageBox.Show(l + " Allready in list.....");
                    }
                }
                Breachingcharge.isDirty = true;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (CurrentTier == null) { return; }
            CurrentTier.TierDestroyableObjectsList.Remove(DestroyableObjectsLB.GetItemText(DestroyableObjectsLB.SelectedItem));
            Breachingcharge.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (CurrentTier == null) { return; }
            CurrentTier.AcceptedChargeTypes.Remove(TiersAcceptedChargesLB.GetItemText(TiersAcceptedChargesLB.SelectedItem));
            Breachingcharge.isDirty = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            Charge charge = AcceptedChargesCB.SelectedItem as Charge;
            string chargename = charge.Classname;
            if (!CurrentTier.AcceptedChargeTypes.Contains(chargename))
                CurrentTier.AcceptedChargeTypes.Add(chargename);
            Breachingcharge.isDirty = true;
        }
        private void TiersHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentTier.Health = (int)TiersHealthNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void TiersNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentTier.Name = TiersNameTB.Text;
            TiersLB.Refresh();
            Breachingcharge.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            if (TiersLB.SelectedItems.Count == 0) return;
            Tier newtier = new Tier()
            {
                Name = "New Tier",
                Health = 1,
                AcceptedChargeTypes = new BindingList<string>()
            };
            Breachingcharge.Tiers.Add(newtier);
            Breachingcharge.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (TiersLB.SelectedItems.Count == 0) return;
            Breachingcharge.Tiers.Remove(CurrentTier);
            Breachingcharge.isDirty = true;
        }
        private void defuseToolUnarmedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (defuseToolUnarmedCB.Checked)
            {
                ChargeDefusetoolsLB.Visible = false;
                darkButton32.Visible = false;
                darkButton31.Visible = false;
            }
            else
            {
                darkButton32.Visible = true;
                darkButton31.Visible = true;
                ChargeDefusetoolsLB.Visible = true;
            }
            if (!useraction) return;
            ChargeDefusetoolsLB.DisplayMember = "DisplayName";
            ChargeDefusetoolsLB.ValueMember = "Value";
            if (defuseToolUnarmedCB.Checked)
            {
                Currentcharge.DefuseTools = new BindingList<string>();
                Currentcharge.DefuseTools.Add("Unarmed");
                ChargeDefusetoolsLB.DataSource = null;
            }
            else
            {
                Currentcharge.DefuseTools = new BindingList<string>();
                ChargeDefusetoolsLB.DataSource = Currentcharge.DefuseTools;
            }
            Breachingcharge.isDirty = true;
        }
        private void darkButton32_Click(object sender, EventArgs e)
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
                    if (!Currentcharge.DefuseTools.Contains(l))
                    {
                        Currentcharge.DefuseTools.Add(l);
                        Breachingcharge.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(l + " Allready exists in defuse tools list.....");
                    }
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            if (Currentcharge == null) { return; }
            Currentcharge.DefuseTools.Remove(ChargeDefusetoolsLB.GetItemText(ChargeDefusetoolsLB.SelectedItem));
            Breachingcharge.isDirty = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.ExplosionSoundSet = textBox1.Text;
            Breachingcharge.isDirty = true;
        }
        private void ChargeSwitchIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.SwitchInterval = ChargeSwitchIntervalNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeBeepingSoundEndTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.BeepingSoundEndTime = ChargeBeepingSoundEndTimeNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeBeepingSoundSetTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.BeepingSoundSet = ChargeBeepingSoundSetTB.Text;
            Breachingcharge.isDirty = true;
        }
        private void CharegLightRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.LightRadius = CharegLightRadiusNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeLightBrightnessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.LightBrightness = ChargeLightBrightnessNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeTimeToDefuseNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.TimeToDefuse = ChargeTimeToDefuseNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeTimeToExplodeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.TimeToExplode = ChargeTimeToExplodeNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeTimeToPlantNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.TimeToPlant = ChargeTimeToPlantNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeToolDamageOnDefuseNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.ToolDamageOnDefuse = ChargeToolDamageOnDefuseNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMinPlacementDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MinPlacementDistance = ChargeMinPlacementDistanceNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargePlacementDistanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.PlacementDistance = ChargeMinPlacementDistanceNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDestroyOtherChargesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DestroyOtherCharges = ChargeDestroyOtherChargesCB.Checked == true ? 1 : 0;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDeleteObjectsDirectlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DeleteObjectsDirectly = ChargeDeleteObjectsDirectlyCB.Checked == true ? 1 : 0;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDestroyLocksFirstCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DestroyLocksFirst = ChargeDestroyLocksFirstCB.Checked == true ? 1 : 0;
            Breachingcharge.isDirty = true;
        }
        private void ChargeOnlyDestroyLocksCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.OnlyDestroyLocks = ChargeOnlyDestroyLocksCB.Checked == true ? 1 : 0;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMaxVerticalDistancePlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MaxVerticalDistancePlayers = ChargeMaxVerticalDistancePlayersNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMaxDamageToPlayersRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MaxDamageToPlayersRadius = ChargeMaxDamageToPlayersRadiusNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDamageToPlayersRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DamageToPlayersRadius = ChargeMaxDamageToPlayersRadiusNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMinDamageToPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MinDamageToPlayers = ChargeMinDamageToPlayersNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMaxDamageToPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MaxDamageToPlayers = ChargeMaxDamageToPlayersNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeMaxVerticalDistanceObjectsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.MaxVerticalDistanceObjects = ChargeMaxVerticalDistancePlayersNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeVerticalDistanceModeObjectsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.VerticalDistanceModeObjects = ChargeMaxVerticalDistanceObjectsNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDamageToDestroyableObjectsRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DamageToDestroyableObjectsRadius = ChargeDamageToDestroyableObjectsRadiusNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeDamageToObjectsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.DamageToObjects = (int)ChargeDamageToObjectsNUD.Value;
            Breachingcharge.isDirty = true;
        }
        private void ChargeClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcharge.Classname = ChargeClassNameTB.Text;
            Breachingcharge.isDirty = true;
        }

        private void BreachingChargeManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Breachingcharge.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
    }
}
