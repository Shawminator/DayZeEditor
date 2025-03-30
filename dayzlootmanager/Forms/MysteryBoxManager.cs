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
    public partial class MysteryBoxManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string MysteryBoxConfigPath { get; private set; }
        public MysteryBox MysteryBoxConfig;
        public string Projectname;
        private Possibleboxitem currentPossibleboxitem;
        private Possibleboxposition currentPossibleboxposition;
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

        public MysteryBoxManager()
        {
            InitializeComponent();
        }

        private void MysteryBoxManager_Load(object sender, EventArgs e)
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
            MysteryBoxConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MagicCrateManagerSettings.json";
            MysteryBoxConfig = JsonSerializer.Deserialize<MysteryBox>(File.ReadAllText(MysteryBoxConfigPath));
            MysteryBoxConfig.SetBoxNames();
            MysteryBoxConfig.isDirty = false;
            MysteryBoxConfig.Filename = MysteryBoxConfigPath;

            MagicCrateExchangeTypeTB.Text = MysteryBoxConfig.MagicCrateExchangeType;
            MagicCratePriceNUD.Value = (decimal)MysteryBoxConfig.MagicCratePrice;
            CanCrateChangeLocationCB.Checked = MysteryBoxConfig.CanCrateChangeLocation == 1 ? true : false;
            MinimumRollBeforeFailChanceNUD.Value = (decimal)MysteryBoxConfig.MinimumRollBeforeFailChance;
            BoxFailChanceNUD.Value = (decimal)MysteryBoxConfig.BoxFailChance;

            PossibleBoxPositionsLB.DisplayMember = "DisplayName";
            PossibleBoxPositionsLB.ValueMember = "Value";
            PossibleBoxPositionsLB.DataSource = MysteryBoxConfig.PossibleBoxPositions;

            PossibleBoxItemsLB.DisplayMember = "DisplayName";
            PossibleBoxItemsLB.ValueMember = "Value";
            PossibleBoxItemsLB.DataSource = MysteryBoxConfig.PossibleBoxItems;

            useraction = true;
        }
        private void MysteryBoxManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MysteryBoxConfig.isDirty)
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
            if (MysteryBoxConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(MysteryBoxConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MysteryBoxConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(MysteryBoxConfig.Filename, Path.GetDirectoryName(MysteryBoxConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MysteryBoxConfig.Filename) + ".bak", true);
                }
                MysteryBoxConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MysteryBoxConfig, options);
                File.WriteAllText(MysteryBoxConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(MysteryBoxConfig.Filename));
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
        private void darkButton5_Click(object sender, EventArgs e)
        {
            Possibleboxposition newbox = new Possibleboxposition()
            {
                BoxName = MysteryBoxConfig.GetNextName(),
                Position = new float[] { 0, 0, 0 },
                Orientation = new float[] { 0, 0, 0 }
            };
            MysteryBoxConfig.PossibleBoxPositions.Add(newbox);
            MysteryBoxConfig.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (PossibleBoxPositionsLB.SelectedItems.Count > 0)
            {
                List<Possibleboxposition> removeitems = new List<Possibleboxposition>();
                foreach (var item in PossibleBoxPositionsLB.SelectedItems)
                {
                    Possibleboxposition citem = item as Possibleboxposition;
                    removeitems.Add(citem);
                }
                foreach (Possibleboxposition item in removeitems)
                {
                    MysteryBoxConfig.PossibleBoxPositions.Remove(item);
                }
                MysteryBoxConfig.isDirty = true;
                if (PossibleBoxPositionsLB.Items.Count == 0)
                    PossibleBoxPositionsLB.SelectedIndex = -1;
                else
                    PossibleBoxPositionsLB.SelectedIndex = 0;

            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            Possibleboxitem newitem = new Possibleboxitem()
            {
                Item = "New Item",
                Mag = "",
                Attachments = new BindingList<string>()
            };
            MysteryBoxConfig.PossibleBoxItems.Add(newitem);
            PossibleBoxItemsLB.SelectedIndex = -1;
            PossibleBoxItemsLB.SelectedIndex = PossibleBoxItemsLB.Items.Count - 1;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (PossibleBoxItemsLB.SelectedItems.Count > 0)
            {
                List<Possibleboxitem> removeitems = new List<Possibleboxitem>();
                foreach (var item in PossibleBoxItemsLB.SelectedItems)
                {
                    Possibleboxitem citem = item as Possibleboxitem;
                    removeitems.Add(citem);
                }
                foreach (Possibleboxitem item in removeitems)
                {
                    MysteryBoxConfig.PossibleBoxItems.Remove(item);
                }
                MysteryBoxConfig.isDirty = true;
                if (PossibleBoxItemsLB.Items.Count == 0)
                    PossibleBoxItemsLB.SelectedIndex = -1;
                else
                    PossibleBoxItemsLB.SelectedIndex = 0;
            }
        }
        private void PossibleBoxItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PossibleBoxItemsLB.SelectedItems.Count < 1) return;
            currentPossibleboxitem = PossibleBoxItemsLB.SelectedItem as Possibleboxitem;
            useraction = false;
            SetItemInfo();
            useraction = true;
        }
        private void SetItemInfo()
        {
            weaponTB.Text = currentPossibleboxitem.Item;
            magazineTB.Text = currentPossibleboxitem.Mag;
            attachmentsLB.DisplayMember = "DisplayName";
            attachmentsLB.ValueMember = "Value";
            attachmentsLB.DataSource = currentPossibleboxitem.Attachments;
        }
        private void PossibleBoxPositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PossibleBoxPositionsLB.SelectedItems.Count < 1) return;
            currentPossibleboxposition = PossibleBoxPositionsLB.SelectedItem as Possibleboxposition;
            useraction = false;
            SetPossibleBoxInfo();
            useraction = true;
        }
        private void SetPossibleBoxInfo()
        {
            PossibleBoxPositionsPOSXNUD.Value = (decimal)currentPossibleboxposition.Position[0];
            PossibleBoxPositionsPOSYNUD.Value = (decimal)currentPossibleboxposition.Position[1];
            PossibleBoxPositionsPOSZNUD.Value = (decimal)currentPossibleboxposition.Position[2];

            PossibleBoxPositionsOXNUD.Value = (decimal)currentPossibleboxposition.Orientation[0];
            PossibleBoxPositionsOYNUD.Value = (decimal)currentPossibleboxposition.Orientation[1];
            PossibleBoxPositionsOZNUD.Value = (decimal)currentPossibleboxposition.Orientation[2];
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
                    if (!MysteryBoxConfig.PossibleBoxItems.Any(x => x.Item == l))
                    {
                        currentPossibleboxitem.Item = l;
                        SetItemInfo();
                        MysteryBoxConfig.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("There is allready a pre defined weapon set up for " + l);
                    }
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
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
                    currentPossibleboxitem.Mag = l;
                    SetItemInfo();
                    MysteryBoxConfig.isDirty = true;
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
                    if (!currentPossibleboxitem.Attachments.Any(x => x == l))
                    {
                        currentPossibleboxitem.Attachments.Add(l);
                        SetItemInfo();
                        MysteryBoxConfig.isDirty = true;
                    }

                }
            }
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            currentPossibleboxitem.Attachments.Remove(attachmentsLB.GetItemText(attachmentsLB.SelectedItem));
            MysteryBoxConfig.isDirty = true;
        }
        private void MagicCrateExchangeTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MysteryBoxConfig.MagicCrateExchangeType = MagicCrateExchangeTypeTB.Text;
            MysteryBoxConfig.isDirty = true;
        }
        private void MagicCratePriceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MysteryBoxConfig.MagicCratePrice = (int)MagicCratePriceNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void CanCrateChangeLocationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MysteryBoxConfig.CanCrateChangeLocation = CanCrateChangeLocationCB.Checked == true ? 1 : 0;
            MysteryBoxConfig.isDirty = true;
        }
        private void MinimumRollBeforeFailChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MysteryBoxConfig.MinimumRollBeforeFailChance = (int)MinimumRollBeforeFailChanceNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void BoxFailChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MysteryBoxConfig.BoxFailChance = (int)BoxFailChanceNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Mystery Box positions";
            openFileDialog.Filter = "Expansion Map|*.map|Object Spawner|*.json|DayZ Editor|*.dze";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MysteryBoxConfig.PossibleBoxPositions.Clear();
                }
                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        string[] fileContent = File.ReadAllLines(filePath);
                        for (int i = 0; i < fileContent.Length; i++)
                        {
                            if (fileContent[i] == "") continue;
                            string[] linesplit = fileContent[i].Split('|');
                            string[] XYZ = linesplit[1].Split(' ');
                            string[] YPR = linesplit[2].Split(' ');
                            Possibleboxposition newbox = new Possibleboxposition()
                            {
                                BoxName = MysteryBoxConfig.GetNextName(),
                                Position = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) },
                                Orientation = new float[] { Convert.ToSingle(YPR[0]), Convert.ToSingle(YPR[1]), Convert.ToSingle(YPR[2]) }
                            };
                            MysteryBoxConfig.PossibleBoxPositions.Add(newbox);
                            SetPossibleBoxInfo();
                        }
                        break;
                    case 2:
                        ObjectSpawnerArr newobjectspawner = JsonSerializer.Deserialize<ObjectSpawnerArr>(File.ReadAllText(filePath));
                        foreach (SpawnObjects so in newobjectspawner.Objects)
                        {
                            Possibleboxposition newbox = new Possibleboxposition()
                            {
                                BoxName = MysteryBoxConfig.GetNextName(),
                                Position = new float[] { so.pos[0], so.pos[1], so.pos[2] },
                                Orientation = new float[] { so.ypr[0], so.ypr[1], so.ypr[2] }
                            };
                            MysteryBoxConfig.PossibleBoxPositions.Add(newbox);
                            SetPossibleBoxInfo();
                        }
                        break;
                    case 3:
                        DZE importfile = DZEHelpers.LoadFile(filePath);
                        foreach (Editorobject eo in importfile.EditorObjects)
                        {
                            Possibleboxposition newbox = new Possibleboxposition()
                            {
                                BoxName = MysteryBoxConfig.GetNextName(),
                                Position = new float[] { eo.Position[0], eo.Position[1], eo.Position[2] },
                                Orientation = new float[] { eo.Orientation[0], eo.Orientation[1], eo.Orientation[2] }
                            };
                            MysteryBoxConfig.PossibleBoxPositions.Add(newbox);
                            SetPossibleBoxInfo();
                        }
                        break;
                }
                PossibleBoxPositionsLB.SelectedIndex = -1;
                PossibleBoxPositionsLB.SelectedIndex = PossibleBoxPositionsLB.Items.Count - 1;
                PossibleBoxPositionsLB.Refresh();
                MysteryBoxConfig.isDirty = true;
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath);
        }
        private void PossibleBoxPositionsPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Position[0] = (float)PossibleBoxPositionsPOSXNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void PossibleBoxPositionsPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Position[1] = (float)PossibleBoxPositionsPOSYNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void PossibleBoxPositionsPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Position[2] = (float)PossibleBoxPositionsPOSZNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void PossibleBoxPositionsOXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Orientation[0] = (float)PossibleBoxPositionsOXNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void PossibleBoxPositionsOYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Orientation[1] = (float)PossibleBoxPositionsOYNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }
        private void PossibleBoxPositionsOZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPossibleboxposition.Orientation[2] = (float)PossibleBoxPositionsOZNUD.Value;
            MysteryBoxConfig.isDirty = true;
        }

    }
}
