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
    public partial class CapareWorkBenchManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string CapareWorkBenchConfigPath { get; private set; }
        public CapareWorkBench CapareWorkBench;
        public Workbenchrecipe CurrentWorkbenchrecipe;
        public Recipe_Ingredients CurrentRecipe_Ingredients;
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

        public CapareWorkBenchManager()
        {
            InitializeComponent();
        }
        private void CapareWorkBenchManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CapareWorkBench.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void CapareWorkBenchManager_Load(object sender, EventArgs e)
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
            CapareWorkBenchConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare_Workbench\\CapareWorkBenchConfig.json";
            var options = new JsonSerializerOptions {Converters = { new BoolConverter() } };
            CapareWorkBench = JsonSerializer.Deserialize<CapareWorkBench>(File.ReadAllText(CapareWorkBenchConfigPath), options);
            CapareWorkBench.isDirty = false;
            CapareWorkBench.Filename = CapareWorkBenchConfigPath;

            AllowRepairFromRuinedCB.Checked = CapareWorkBench.Repairing.AllowRepairFromRuined;
            RuinedPunishmentMultiplierNUD.Value = CapareWorkBench.Repairing.RuinedPunishmentMultiplier;
            SewingKitUsageNUD.Value = CapareWorkBench.Repairing.SewingKitUsage;
            LeatherSewingKitUsageNUD.Value = CapareWorkBench.Repairing.LeatherSewingKitUsage;
            WeaponCleaningKitUsageNUD.Value = CapareWorkBench.Repairing.WeaponCleaningKitUsage;
            WhetstoneUsageNUD.Value = CapareWorkBench.Repairing.WhetstoneUsage;
            TireRepairKitUsageNUD.Value = CapareWorkBench.Repairing.TireRepairKitUsage;
            ElectronicRepairKitUsageNUD.Value =CapareWorkBench.Repairing.ElectronicRepairKitUsage;
            EpoxyPuttyUsageNUD.Value = CapareWorkBench.Repairing.EpoxyPuttyUsage;

            RecipesLB.DisplayMember = "DisplayName";
            RecipesLB.ValueMember = "Value";
            RecipesLB.DataSource = CapareWorkBench.WorkbenchRecipes;

            useraction = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (CapareWorkBench.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(CapareWorkBench.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(CapareWorkBench.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(CapareWorkBench.Filename, Path.GetDirectoryName(CapareWorkBench.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(CapareWorkBench.Filename) + ".bak", true);
                }
                CapareWorkBench.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Converters = { new BoolConverter() } };
                string jsonString = JsonSerializer.Serialize(CapareWorkBench, options);
                File.WriteAllText(CapareWorkBench.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(CapareWorkBench.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Capare_Workbench");
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
                    Workbenchrecipe newitem = new Workbenchrecipe()
                    {
                        Recipe_Name = "Craft " + l,
                        ResultName = l,
                        isQuantity =false,
                        ResultCount = 1,
                        CraftType = "craft",
                        RecipeCategory = "",
                        CraftTimeSeconds =  60,
                        Recipe_Ingredients = new BindingList<Recipe_Ingredients>(),
                        AttachmentsNeeded = new BindingList<string>()
                    };
                    CapareWorkBench.WorkbenchRecipes.Add(newitem);
                }
                CapareWorkBench.isDirty = true;
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
                List<Workbenchrecipe> removeitems = new List<Workbenchrecipe>();
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Workbenchrecipe citem = item as Workbenchrecipe;
                    removeitems.Add(citem);
                }
                foreach (Workbenchrecipe item in removeitems)
                {
                    CapareWorkBench.WorkbenchRecipes.Remove(item);
                }
                CapareWorkBench.isDirty = true;
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
            CurrentWorkbenchrecipe = RecipesLB.SelectedItem as Workbenchrecipe;

            Recipe_NameTB.Text = CurrentWorkbenchrecipe.Recipe_Name;
            ResultNameTB.Text = CurrentWorkbenchrecipe.ResultName;
            isQuantityCB.Checked = CurrentWorkbenchrecipe.isQuantity;
            ResultCountNUD.Value = CurrentWorkbenchrecipe.ResultCount;
            CraftTypeCB.SelectedIndex = CraftTypeCB.FindStringExact(CurrentWorkbenchrecipe.CraftType);
            if (CurrentWorkbenchrecipe.RecipeCategory == "")
                RecipeCategoryCB.SelectedIndex = RecipeCategoryCB.FindStringExact("None");
            else
                RecipeCategoryCB.SelectedIndex = RecipeCategoryCB.FindStringExact(CurrentWorkbenchrecipe.RecipeCategory);
            CraftTimeSecondsNUD.Value = CurrentWorkbenchrecipe.CraftTimeSeconds;
            CapareDrillCB.Checked = CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareDrill");
            CapareSawCB.Checked = CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareSaw");
            CapareGrinderCB.Checked = CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareGrinder");

            Recipe_IngredientsLB.DisplayMember = "DisplayName";
            Recipe_IngredientsLB.ValueMember = "Value";
            Recipe_IngredientsLB.DataSource = CurrentWorkbenchrecipe.Recipe_Ingredients;
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
                    Recipe_Ingredients newitem = new Recipe_Ingredients()
                    {
                        IngredientName = l,
                        isQuantity = false,
                        IngredientAmount = 1,
                        DestroyAfterUse = true,
                        HealthDecrease = 0
                    };
                    CurrentWorkbenchrecipe.Recipe_Ingredients.Add(newitem);
                }
                CapareWorkBench.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                List<Recipe_Ingredients> removeitems = new List<Recipe_Ingredients>();
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    removeitems.Add(citem);
                }
                foreach (Recipe_Ingredients item in removeitems)
                {
                    CurrentWorkbenchrecipe.Recipe_Ingredients.Remove(item);
                }
                CapareWorkBench.isDirty = true;
                if (Recipe_IngredientsLB.Items.Count == 0)
                    Recipe_IngredientsLB.SelectedIndex = -1;
                else
                    Recipe_IngredientsLB.SelectedIndex = 0;
            }
        }
        private void Recipe_NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWorkbenchrecipe.Recipe_Name = Recipe_NameTB.Text;
            CapareWorkBench.isDirty = true;
        }
        private void ResultNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWorkbenchrecipe.ResultName = ResultNameTB.Text;
            CapareWorkBench.isDirty = true;
        }
        private void isQuantityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWorkbenchrecipe.isQuantity = isQuantityCB.Checked;
            CapareWorkBench.isDirty = true;
        }
        private void ResultCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWorkbenchrecipe.ResultCount = (int)ResultCountNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void CraftTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (RecipesLB.SelectedItems.Count > 0)
            {
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Workbenchrecipe citem = item as Workbenchrecipe;
                    citem.CraftType = CraftTypeCB.GetItemText(CraftTypeCB.SelectedItem);
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void RecipeCategoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (RecipesLB.SelectedItems.Count > 0)
            {
                foreach (var item in RecipesLB.SelectedItems)
                {
                    Workbenchrecipe citem = item as Workbenchrecipe;
                    string text = RecipeCategoryCB.GetItemText(RecipeCategoryCB.SelectedItem);
                    if (text == "None")
                        citem.RecipeCategory = "";
                    else
                        citem.RecipeCategory = text;
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void CraftTimeSecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWorkbenchrecipe.CraftTimeSeconds = (int)CraftTimeSecondsNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void Recipe_IngredientsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Recipe_IngredientsLB.SelectedItems.Count <= 0) return;
            useraction = false;
            CurrentRecipe_Ingredients = Recipe_IngredientsLB.SelectedItem as Recipe_Ingredients;

            IngredientNameTB.Text = CurrentRecipe_Ingredients.IngredientName;
            IngredientisQuantityCB.Checked = CurrentRecipe_Ingredients.isQuantity;
            IngediantAmountNUD.Value = CurrentRecipe_Ingredients.IngredientAmount;
            DestroyAfterUseCB.Checked = CurrentRecipe_Ingredients.DestroyAfterUse;
            HealthDecreaseNUD.Value = (decimal)CurrentRecipe_Ingredients.HealthDecrease;

            useraction = true;
        }
        private void IngredientNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    citem.IngredientName = IngredientNameTB.Text;
                }
                Recipe_IngredientsLB.Refresh();
            }
            CapareWorkBench.isDirty = true;
        }
        private void IngredientisQuantityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    citem.isQuantity = IngredientisQuantityCB.Checked;
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void IngediantAmountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    citem.IngredientAmount = (int)IngediantAmountNUD.Value;
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void DestroyAfterUseCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    citem.DestroyAfterUse = DestroyAfterUseCB.Checked;
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void HealthDecreaseNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Recipe_IngredientsLB.SelectedItems.Count > 0)
            {
                foreach (var item in Recipe_IngredientsLB.SelectedItems)
                {
                    Recipe_Ingredients citem = item as Recipe_Ingredients;
                    citem.HealthDecrease = (int)HealthDecreaseNUD.Value;
                }
            }
            CapareWorkBench.isDirty = true;
        }
        private void BPGrinderCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CapareGrinderCB.Checked)
            {
                if (!CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareGrinder"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Add("CapareGrinder");
            }
            else
            {
                if (CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareGrinder"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Remove("CapareGrinder");
            }
            CapareWorkBench.isDirty = true;
        }
        private void BPDrillCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CapareDrillCB.Checked)
            {
                if (!CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareDrill"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Add("CapareDrill");
            }
            else
            {
                if (CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareDrill"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Remove("CapareDrill");
            }
            CapareWorkBench.isDirty = true;
        }
        private void BPCutting_sawCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CapareSawCB.Checked)
            {
                if (!CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareSaw"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Add("CapareSaw");
            }
            else
            {
                if (CurrentWorkbenchrecipe.AttachmentsNeeded.Contains("CapareSaw"))
                    CurrentWorkbenchrecipe.AttachmentsNeeded.Remove("CapareSaw");
            }
            CapareWorkBench.isDirty = true;
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
                    ResultNameTB.Text = l;
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
                    IngredientNameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        private void AllowRepairFromRuinedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.AllowRepairFromRuined = AllowRepairFromRuinedCB.Checked;
            CapareWorkBench.isDirty = true;
        }
        private void RuinedPunishmentMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.RuinedPunishmentMultiplier = (int)RuinedPunishmentMultiplierNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void SewingKitUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.SewingKitUsage = (int)SewingKitUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void LeatherSewingKitUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.LeatherSewingKitUsage = (int)LeatherSewingKitUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void WeaponCleaningKitUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.WeaponCleaningKitUsage = (int)WeaponCleaningKitUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void WhetstoneUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.WhetstoneUsage = (int)WhetstoneUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void TireRepairKitUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.TireRepairKitUsage = (int)TireRepairKitUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void ElectronicRepairKitUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.ElectronicRepairKitUsage = (int)ElectronicRepairKitUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }
        private void EpoxyPuttyUsageNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CapareWorkBench.Repairing.EpoxyPuttyUsage = (int)EpoxyPuttyUsageNUD.Value;
            CapareWorkBench.isDirty = true;
        }

        private void darkButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if(openfile.ShowDialog() == DialogResult.OK)
            {
                AdvancedWorkBenchConfig AdvancedWorkBenchConfig = new AdvancedWorkBenchConfig();
                AdvancedWorkBenchConfig = JsonSerializer.Deserialize<AdvancedWorkBenchConfig>(File.ReadAllText(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Advanced_Workbench\\AdvancedWorkBenchConfig.json"));
                AdvancedWorkBenchConfig.isDirty = false;
                foreach(Craftitem Craftitem in AdvancedWorkBenchConfig.m_CraftClasses.CraftItems)
                {
                    Workbenchrecipe workbenchrecipe = new Workbenchrecipe();
                    workbenchrecipe.Recipe_Name = Craftitem.RecipeName;
                    workbenchrecipe.ResultName = Craftitem.Result;
                    workbenchrecipe.isQuantity = false;
                    workbenchrecipe.ResultCount = Craftitem.ResultCount;
                    workbenchrecipe.CraftType = Craftitem.CraftType;
                    workbenchrecipe.CraftTimeSeconds = 60;
                    workbenchrecipe.AttachmentsNeeded = new BindingList<string>();
                    foreach(string attchment in Craftitem.AttachmentsNeed)
                    {
                        if (attchment == "BPDrill")
                            workbenchrecipe.AttachmentsNeeded.Add("CapareDrill");
                        else if (attchment == "BPCutting_saw")
                            workbenchrecipe.AttachmentsNeeded.Add("CapareSaw");
                        else if (attchment == "BPGrinder")
                            workbenchrecipe.AttachmentsNeeded.Add("CapareGrinder");
                    }
                    workbenchrecipe.Recipe_Ingredients = new BindingList<Recipe_Ingredients>();
                    foreach(Craftcomponent Craftcomponent in Craftitem.CraftComponents)
                    {
                        Recipe_Ingredients Recipe_Ingredients = new Recipe_Ingredients();
                        Recipe_Ingredients.IngredientName = Craftcomponent.Classname;
                        Recipe_Ingredients.isQuantity = false;
                        Recipe_Ingredients.IngredientAmount = Craftcomponent.Amount;
                        Recipe_Ingredients.DestroyAfterUse = Craftcomponent.Destroy == 1 ? true : false;
                        Recipe_Ingredients.HealthDecrease = Craftcomponent.Changehealth;
                        workbenchrecipe.Recipe_Ingredients.Add(Recipe_Ingredients);
                    }
                    CapareWorkBench.WorkbenchRecipes.Add(workbenchrecipe);
                    RecipesLB.Invalidate();
                }
                CapareWorkBench.isDirty = true;
            }
           
        }
    }
}
