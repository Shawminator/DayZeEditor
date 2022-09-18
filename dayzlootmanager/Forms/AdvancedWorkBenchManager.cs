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
    public partial class AdvancedWorkBenchManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string AdvancedWorkBenchConfigPath { get; private set; }
        public AdvancedWorkBenchConfig AdvancedWorkBenchConfig;
        public Craftitem CurrentCraftItem;
        public Craftcomponent CurrentCraftComponent;
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

        public AdvancedWorkBenchManager()
        {
            InitializeComponent();
        }
        private void AdvancedWorkBenchManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AdvancedWorkBenchConfig.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void AdvancedWorkBenchManager_Load(object sender, EventArgs e)
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
            AdvancedWorkBenchConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Advanced_Workbench\\AdvancedWorkBenchConfig.json";
            AdvancedWorkBenchConfig = JsonSerializer.Deserialize<AdvancedWorkBenchConfig>(File.ReadAllText(AdvancedWorkBenchConfigPath));
            AdvancedWorkBenchConfig.isDirty = false;
            AdvancedWorkBenchConfig.Filename = AdvancedWorkBenchConfigPath;

            RecipesLB.DisplayMember = "DisplayName";
            RecipesLB.ValueMember = "Value";
            RecipesLB.DataSource = AdvancedWorkBenchConfig.CraftItems;

            useraction = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
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
                    Craftitem newitem = new Craftitem()
                    {
                        Result = l,
                        ResultCount = 1,
                        CraftType = "craft",
                        RecipeName = "Craft " + l,
                        RecipeCategory = "",
                        CraftComponents = new BindingList<Craftcomponent>(),
                        AttachmentsNeed = new BindingList<string>()
                    };
                    AdvancedWorkBenchConfig.CraftItems.Add(newitem);
                }
                AdvancedWorkBenchConfig.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (RecipesLB.SelectedItems.Count > 0)
            {
                List<Craftitem> removeitems = new List<Craftitem>();
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Craftitem citem = item as Craftitem;
                    removeitems.Add(citem);
                }
                foreach (Craftitem item in removeitems)
                {
                    AdvancedWorkBenchConfig.CraftItems.Remove(item);
                }
                AdvancedWorkBenchConfig.isDirty = true;
                if (RecipesLB.Items.Count == 0)
                    RecipesLB.SelectedIndex = -1;
                else
                    RecipesLB.SelectedIndex = 0;
            }
        }
        private void RecipesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecipesLB.SelectedItems.Count <= 0) return;
            useraction = false;
            CurrentCraftItem = RecipesLB.SelectedItem as Craftitem;

            CIRecipeNameTB.Text = CurrentCraftItem.RecipeName;
            CIresultTB.Text = CurrentCraftItem.Result;
            CIresultCountNUD.Value = CurrentCraftItem.ResultCount;
            CICraftTypeTB.Text = CurrentCraftItem.CraftType;
            CIRecipeCategoryCB.SelectedIndex = CIRecipeCategoryCB.FindStringExact(CurrentCraftItem.RecipeCategory);
            BPDrillCB.Checked = CurrentCraftItem.AttachmentsNeed.Contains("BPDrill");
            BPCutting_sawCB.Checked = CurrentCraftItem.AttachmentsNeed.Contains("BPCutting_saw");
            BPGrinderCB.Checked = CurrentCraftItem.AttachmentsNeed.Contains("BPGrinder");

            CICompentsLB.DisplayMember = "DisplayName";
            CICompentsLB.ValueMember = "Value";
            CICompentsLB.DataSource = CurrentCraftItem.CraftComponents;
            useraction = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
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
                    Craftcomponent newitem = new Craftcomponent()
                    {
                        Classname = l,
                        Amount = 1,
                        Destroy = 1,
                        Changehealth = 0
                    };
                    CurrentCraftItem.CraftComponents.Add(newitem);
                }
                AdvancedWorkBenchConfig.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CICompentsLB.SelectedItems.Count > 0)
            {
                List<Craftcomponent> removeitems = new List<Craftcomponent>();
                foreach (var item in CICompentsLB.SelectedItems)
                {
                    Craftcomponent citem = item as Craftcomponent;
                    removeitems.Add(citem);
                }
                foreach (Craftcomponent item in removeitems)
                {
                    CurrentCraftItem.CraftComponents.Remove(item);
                }
                AdvancedWorkBenchConfig.isDirty = true;
                if (CICompentsLB.Items.Count == 0)
                    CICompentsLB.SelectedIndex = -1;
                else
                    CICompentsLB.SelectedIndex = 0;
            }
        }
        private void CIRecipeNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentCraftItem.RecipeName = CIRecipeNameTB.Text;
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CIresultTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentCraftItem.Result = CIresultTB.Text;
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CIresultCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentCraftItem.ResultCount = (int)CIresultCountNUD.Value;
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CICraftTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (RecipesLB.SelectedItems.Count > 0)
            {
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Craftitem citem = item as Craftitem;
                    citem.CraftType = CICraftTypeTB.Text;
                }
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CIRecipeCategoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (RecipesLB.SelectedItems.Count > 0)
            {
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Craftitem citem = item as Craftitem;
                    citem.RecipeCategory = CIRecipeCategoryCB.GetItemText(CIRecipeCategoryCB.SelectedItem);
                }
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CICompentsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CICompentsLB.SelectedItems.Count <= 0) return;
            useraction = false;
            CurrentCraftComponent = CICompentsLB.SelectedItem as Craftcomponent;

            CICPClassnameTB.Text = CurrentCraftComponent.Classname;
            CICPAmountNUD.Value = CurrentCraftComponent.Amount;
            CICPDestoryCB.Checked = CurrentCraftComponent.Destroy == 1 ? true : false;
            CICPChangeHealthNUD.Value = CurrentCraftComponent.Changehealth;

            useraction = true;
        }
        private void CICPClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CICompentsLB.SelectedItems.Count > 0)
            {
                foreach (var item in CICompentsLB.SelectedItems)
                {
                    Craftcomponent citem = item as Craftcomponent;
                    citem.Classname = CICPClassnameTB.Text;
                }
                CICompentsLB.Refresh();
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CICPAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CICompentsLB.SelectedItems.Count > 0)
            {
                foreach (var item in CICompentsLB.SelectedItems)
                {
                    Craftcomponent citem = item as Craftcomponent;
                    citem.Amount = (int)CICPAmountNUD.Value;
                }
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CICPDestoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CICompentsLB.SelectedItems.Count > 0)
            {
                foreach (var item in CICompentsLB.SelectedItems)
                {
                    Craftcomponent citem = item as Craftcomponent;
                    citem.Destroy = CICPDestoryCB.Checked == true ? 1 : 0;
                }
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void CICPChangeHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CICompentsLB.SelectedItems.Count > 0)
            {
                foreach (var item in CICompentsLB.SelectedItems)
                {
                    Craftcomponent citem = item as Craftcomponent;
                    citem.Changehealth = CICPChangeHealthNUD.Value;
                }
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void BPGrinderCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (BPGrinderCB.Checked)
            {
                if (!CurrentCraftItem.AttachmentsNeed.Contains("BPGrinder"))
                    CurrentCraftItem.AttachmentsNeed.Add("BPGrinder");
            }
            else
            {
                if(CurrentCraftItem.AttachmentsNeed.Contains("BPGrinder"))
                    CurrentCraftItem.AttachmentsNeed.Remove("BPGrinder");
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void BPDrillCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (BPDrillCB.Checked)
            {
                if (!CurrentCraftItem.AttachmentsNeed.Contains("BPDrill"))
                    CurrentCraftItem.AttachmentsNeed.Add("BPDrill");
            }
            else
            {
                if (CurrentCraftItem.AttachmentsNeed.Contains("BPDrill"))
                    CurrentCraftItem.AttachmentsNeed.Remove("BPDrill");
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void BPCutting_sawCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (BPCutting_sawCB.Checked)
            {
                if (!CurrentCraftItem.AttachmentsNeed.Contains("BPCutting_saw"))
                    CurrentCraftItem.AttachmentsNeed.Add("BPCutting_saw");
            }
            else
            {
                if (CurrentCraftItem.AttachmentsNeed.Contains("BPCutting_saw"))
                    CurrentCraftItem.AttachmentsNeed.Remove("BPCutting_saw");
            }
            AdvancedWorkBenchConfig.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
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
                    CIresultTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton20_Click(object sender, EventArgs e)
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
                    CICPClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
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
            if (AdvancedWorkBenchConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AdvancedWorkBenchConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AdvancedWorkBenchConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AdvancedWorkBenchConfig.Filename, Path.GetDirectoryName(AdvancedWorkBenchConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AdvancedWorkBenchConfig.Filename) + ".bak", true);
                }
                AdvancedWorkBenchConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AdvancedWorkBenchConfig, options);
                File.WriteAllText(AdvancedWorkBenchConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AdvancedWorkBenchConfig.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Advanced_Workbench");
        }


    }
}
