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
    public partial class ExpansionLoadoutsManager : DarkForm
    {
        public Project currentproject { get; internal set; }
        public BindingList<AILoadouts> LoadoutList { get; private set; }

        public string AILoadoutsPath;

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public AILoadouts CurrentAILoadoutsFile { get; private set; }
        public Inventoryattachment CurrentInventoryattachment { get; private set; }
        public AILoadouts CurrentAIloadouts { get; private set; }
        public Health Currenthealth { get; private set; }

        private bool useraction;

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

        public ExpansionLoadoutsManager()
        {
            InitializeComponent();
        }

        private void ExpansionLoadoutsManager_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LoadoutList = new BindingList<AILoadouts>();
            AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
                    AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
                    AILoadouts.Filename = file.FullName;
                    AILoadouts.Setname();
                    AILoadouts.isDirty = false;
                    LoadoutList.Add(AILoadouts);
                }
                catch { }
            }
            SetupAILoadouts();
        }
        private void SetupAILoadouts()
        {
            useraction = false;

            loadoutsLB.DisplayMember = "DisplayName";
            loadoutsLB.ValueMember = "Value";
            loadoutsLB.DataSource = LoadoutList;

            useraction = true;
        }

        private void loadoutsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadoutsLB.SelectedItems.Count < 1) return;
            CurrentAILoadoutsFile = loadoutsLB.SelectedItem as AILoadouts;
            useraction = false;
            SetupLoadoutTreeView();
            useraction = true;
        }
        public object CurrentTreeNodeTag;
        private void SetupLoadoutTreeView()
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(CurrentAILoadoutsFile.Name))
            {
                Tag = "Parent"
            };
            TreeNode inventoryAttchemnts = new TreeNode("inventoryAttchemnts")
            {
                Tag = "inventoryAttchemnts"
            };
            foreach (Inventoryattachment IA in CurrentAILoadoutsFile.InventoryAttachments)
            {

                inventoryAttchemnts.Nodes.Add(inventoryAttchmentsTN(IA));
            }
            TreeNode InventoryCargo = new TreeNode("InventoryCargo")
            {
                Tag = "InventoryCargo"
            };
            foreach (AILoadouts IA in CurrentAILoadoutsFile.InventoryCargo)
            {

                InventoryCargo.Nodes.Add(inventoryAttchemntsItemsTN(IA));
            }
            TreeNode Sets = new TreeNode("Sets")
            {
                Tag = "Sets"
            };
            foreach (AILoadouts IA in CurrentAILoadoutsFile.Sets)
            {
                Sets.Nodes.Add(inventoryAttchemntsItemsTN(IA));
            }
            root.Nodes.Add(inventoryAttchemnts);
            root.Nodes.Add(InventoryCargo);
            root.Nodes.Add(Sets);
            treeViewMS1.Nodes.Add(root);
        }
        TreeNode inventoryAttchmentsTN(Inventoryattachment Inventoryattachment)
        {
            string slotname;
            if (Inventoryattachment.SlotName == "")
                slotname = "Default Slot";
            else
                slotname = Inventoryattachment.SlotName;
            TreeNode IAnode = new TreeNode(slotname)
            {
                Tag = Inventoryattachment
            };
            foreach (AILoadouts AIL in Inventoryattachment.Items)
            {
                IAnode.Nodes.Add(inventoryAttchemntsItemsTN(AIL));
            }
            return IAnode;
        }
        TreeNode inventoryAttchemntsItemsTN(AILoadouts AILoadouts)
        {
            string slotname;
            if (AILoadouts.ClassName == "")
                slotname = "Set";
            else
                slotname = AILoadouts.ClassName;
            TreeNode AILitems = new TreeNode(slotname)
            {
                Tag = AILoadouts
            };
            foreach (Inventoryattachment IA in AILoadouts.InventoryAttachments)
            {
                AILitems.Nodes.Add(inventoryAttchmentsTN(IA));
            }
            TreeNode cargotn = InventoryCargo(AILoadouts.InventoryCargo);
            if (cargotn != null)
                AILitems.Nodes.Add(cargotn);
            foreach (AILoadouts IA in AILoadouts.Sets)
            {
                AILitems.Nodes.Add(inventoryAttchemntsItemsTN(IA));
            }
            return AILitems;
        }
        TreeNode InventoryCargo(BindingList<AILoadouts> aILoadoutsList)
        {
            if (aILoadoutsList.Count == 0) return null;
            TreeNode tn = new TreeNode("Cargo");
            tn.Tag = "Cargo";
            foreach (AILoadouts IA in aILoadoutsList)
            {
                tn.Nodes.Add(inventoryAttchemntsItemsTN(IA));
            }
            return tn;
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
            AILoadouts newAILoadouts = new AILoadouts()
            {
                Name = UserAnswer,
                Filename = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts\\" + UserAnswer + ".json",
                ClassName = "",
                Chance = (decimal)1.0,
                Quantity = new Quantity(),
                Health = new BindingList<Health>(),
                InventoryAttachments = new BindingList<Inventoryattachment>(),
                InventoryCargo = new BindingList<AILoadouts>(),
                ConstructionPartsBuilt = new BindingList<object>(),
                Sets = new BindingList<AILoadouts>(),
                isDirty = true
            };
            LoadoutList.Add(newAILoadouts);
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                File.Delete(CurrentAILoadoutsFile.Filename);
                LoadoutList.Remove(CurrentAILoadoutsFile);
            }
        }
        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            treeViewMS1.SelectedNode = e.Node;
            CurrentTreeNodeTag = e.Node.Tag;

            AddNewAttachmentItemToolStripMenuItem.Visible = false;
            RemoveAttachemtItemToolStripMenuItem.Visible = false;
            AddNewCargoItemToolStripMenuItem.Visible = false;
            RemoveCargoItemToolStripMenuItem.Visible = false;
            AddNewSetItemToolStripMenuItem.Visible = false;
            RemoveSetItemToolStripMenuItem.Visible = false;
            addNewItemToolStripMenuItem.Visible = false;
            removeItemToolStripMenuItem.Visible = false;

            LoadOutGB.Visible = false;
            InventoryattchemntGB.Visible = false;
            HealthGB.Visible = false;
            LoadOutGB.Visible = false;

            if (e.Node.Tag is string)
            {

                switch (e.Node.Tag.ToString())
                {
                    case "inventoryAttchemnts":
                        if (e.Button == MouseButtons.Right)
                        {
                            AddNewAttachmentItemToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "InventoryCargo":
                        if (e.Button == MouseButtons.Right)
                        {
                            AddNewCargoItemToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Sets":
                        if (e.Button == MouseButtons.Right)
                        {
                            AddNewSetItemToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Cargo":
                        if (e.Button == MouseButtons.Right)
                        {
                            RemoveCargoItemToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                }
            }
            else if (e.Node.Parent != null && e.Node.Tag is Inventoryattachment)
            {
                InventoryattchemntGB.Visible = true;
                CurrentAIloadouts = e.Node.Parent.Tag as AILoadouts;
                CurrentInventoryattachment = e.Node.Tag as Inventoryattachment;
                useraction = false;
                string slotname = CurrentInventoryattachment.SlotName;
                if (CurrentInventoryattachment.SlotName == "")
                    slotname = "Default Slot";
                ItemAttachmentSlotNameCB.SelectedIndex = ItemAttachmentSlotNameCB.FindStringExact(slotname);
                useraction = true;

                if (e.Button == MouseButtons.Right)
                {
                    addNewItemToolStripMenuItem.Visible = true;
                    RemoveAttachemtItemToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }


            }
            else if (e.Node.Parent != null && e.Node.Tag is AILoadouts)
            {
                LoadOutGB.Visible = true;
                CurrentAIloadouts = e.Node.Tag as AILoadouts;

                useraction = false;
                textBox1.Text = CurrentAIloadouts.ClassName;
                numericUpDown1.Value = CurrentAIloadouts.Chance;
                numericUpDown2.Value = CurrentAIloadouts.Quantity.Min;
                numericUpDown3.Value = CurrentAIloadouts.Quantity.Max;

                if (CurrentAIloadouts.Health.Count > 0)
                {
                    HealthGB.Visible = true;
                }

                listBox1.DisplayMember = "DisplayName";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = CurrentAIloadouts.Health;
                useraction = false;

                if (e.Button == MouseButtons.Right)
                {

                    removeItemToolStripMenuItem.Visible = true;
                    AddNewAttachmentItemToolStripMenuItem.Visible = true;
                    AddNewCargoItemToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }

            useraction = true;
        }
        private void AddNewAttachmentItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventoryattachment newIA = new Inventoryattachment()
            {
                SlotName = "Back",
                Items = new BindingList<AILoadouts>()
            };
            TreeNode newnode = new TreeNode(newIA.SlotName)
            {
                Tag = newIA
            };
            if (treeViewMS1.SelectedNode.Text == "inventoryAttchemnts")
            {
                CurrentAILoadoutsFile.InventoryAttachments.Add(newIA);
            }
            else
            {
                CurrentAIloadouts.InventoryAttachments.Add(newIA);
            }
            treeViewMS1.SelectedNode.Nodes.Add(newnode);

            CurrentAILoadoutsFile.isDirty = true;
        }
        private void RemoveAttachemtItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentAILoadoutsFile.InventoryAttachments.Remove(CurrentInventoryattachment);
            treeViewMS1.SelectedNode.Remove();
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void AddNewCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts ailoadout = new AILoadouts()
            {
                ClassName = "New Item, Replace me.",
                Chance = (decimal)1.0,
                Quantity = new Quantity(),
                Health = new BindingList<Health>(),
                InventoryAttachments = new BindingList<Inventoryattachment>(),
                InventoryCargo = new BindingList<AILoadouts>(),
                ConstructionPartsBuilt = new BindingList<object>(),
                Sets = new BindingList<AILoadouts>()
            };
            TreeNode newailoadit = new TreeNode("New Item, Replace me.")
            {
                Tag = ailoadout
            };


            if (treeViewMS1.SelectedNode.Text == "InventoryCargo")
            {
                CurrentAILoadoutsFile.InventoryCargo.Add(ailoadout);
                treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            }
            else
            {
                CurrentAIloadouts = treeViewMS1.SelectedNode.Tag as AILoadouts;
                TreeNode newtreenode = new TreeNode("Cargo");
                newtreenode.Tag = "Cargo";
                if (CurrentAIloadouts.InventoryCargo.Count == 0)
                {
                    newtreenode.Nodes.Add(newailoadit);
                    treeViewMS1.SelectedNode.Nodes.Add(newtreenode);
                }
                else
                {
                    foreach (TreeNode node in treeViewMS1.SelectedNode.Nodes)
                    {
                        if (node.Text == "Cargo")
                        {
                            node.Nodes.Add(newailoadit);
                            break;
                        }
                    }
                }

                CurrentAIloadouts.InventoryCargo.Add(ailoadout);

            }
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void RemoveCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts Parentloadout = treeViewMS1.SelectedNode.Parent.Tag as AILoadouts;
            Parentloadout.InventoryCargo = new BindingList<AILoadouts>();

            treeViewMS1.SelectedNode.Remove();
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void AddNewSetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts ailoadout = new AILoadouts()
            {
                ClassName = "",
                Chance = (decimal)1.0,
                Quantity = new Quantity(),
                Health = new BindingList<Health>(),
                InventoryAttachments = new BindingList<Inventoryattachment>(),
                InventoryCargo = new BindingList<AILoadouts>(),
                ConstructionPartsBuilt = new BindingList<object>(),
                Sets = new BindingList<AILoadouts>()
            };
            CurrentAILoadoutsFile.Sets.Add(ailoadout);
            TreeNode newailoadit = new TreeNode("Set")
            {
                Tag = ailoadout
            };
            treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void RemoveSetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts ailoadout = new AILoadouts()
            {
                ClassName = "New Item, Replace me.",
                Chance = (decimal)1.0,
                Quantity = new Quantity(),
                Health = new BindingList<Health>(),
                InventoryAttachments = new BindingList<Inventoryattachment>(),
                InventoryCargo = new BindingList<AILoadouts>(),
                ConstructionPartsBuilt = new BindingList<object>(),
                Sets = new BindingList<AILoadouts>()
            };
            ailoadout.Health.Add(new Health() { Zone = "", Min = (decimal)0.7, Max = (decimal)1.0 });
            CurrentInventoryattachment.Items.Add(ailoadout);
            TreeNode newailoadit = new TreeNode("New Item, Replace me.")
            {
                Tag = ailoadout
            };
            treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode Parent = treeViewMS1.SelectedNode.Parent;
            if (treeViewMS1.SelectedNode.Parent.Tag is Inventoryattachment)
            {
                Inventoryattachment IA = treeViewMS1.SelectedNode.Parent.Tag as Inventoryattachment;
                IA.Items.Remove(CurrentAIloadouts);
                treeViewMS1.SelectedNode.Remove();
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if (treeViewMS1.SelectedNode.Tag is AILoadouts)
            {
                if (treeViewMS1.SelectedNode.Parent.Text == "InventoryCargo")
                {
                    CurrentAILoadoutsFile.InventoryCargo.Remove(CurrentAIloadouts);
                    treeViewMS1.SelectedNode.Remove();
                    CurrentAILoadoutsFile.isDirty = true;
                }
                if (treeViewMS1.SelectedNode.Parent.Text == "Sets")
                {
                    CurrentAILoadoutsFile.Sets.Remove(CurrentAIloadouts);
                    treeViewMS1.SelectedNode.Remove();
                    CurrentAILoadoutsFile.isDirty = true;
                }
                else if (treeViewMS1.SelectedNode.Parent.Text == "Attachment")
                {
                    AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
                }
                else if (treeViewMS1.SelectedNode.Parent.Text == "Cargo")
                {
                    AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
                    IA.InventoryCargo.Remove(CurrentAIloadouts);
                    treeViewMS1.SelectedNode.Remove();
                    if (IA.InventoryCargo.Count == 0)
                        Parent.Remove();
                    CurrentAILoadoutsFile.isDirty = true;
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAIloadouts.ClassName = treeViewMS1.SelectedNode.Text = textBox1.Text;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
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
                    textBox1.Text = l;
                }
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAIloadouts.Chance = numericUpDown1.Value;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAIloadouts.Quantity.Min = numericUpDown2.Value;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentAIloadouts.Quantity.Max = numericUpDown2.Value;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            Currenthealth = listBox1.SelectedItem as Health;
            useraction = false;

            numericUpDown4.Value = Currenthealth.Min;
            numericUpDown5.Value = Currenthealth.Max;
            textBox2.Text = Currenthealth.Zone;

            useraction = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            Health newhealth = new Health()
            {
                Min = (decimal)0.7,
                Max = (decimal)1.0,
                Zone = ""
            };
            CurrentAIloadouts.Health.Add(newhealth);
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            CurrentAIloadouts.Health.Remove(Currenthealth);
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Min = numericUpDown4.Value;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Max = numericUpDown5.Value;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Zone = textBox2.Text;
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void ItemAttachmentSlotNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string Slot = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            if (Slot == "Default Slot") Slot = "";
            CurrentInventoryattachment.SlotName = Slot;
            treeViewMS1.SelectedNode.Text = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            CurrentAILoadoutsFile.isDirty = true;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(AILoadoutsPath);
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles(bool updated = false)
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            foreach (AILoadouts AILO in LoadoutList)
            {
                if (AILO.isDirty)
                {
                    if (currentproject.Createbackups && File.Exists(AILO.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(AILO.Filename, Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak", true);
                    }
                    AILO.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(AILO, options);
                    File.WriteAllText(AILO.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(AILO.Filename));
                }
            }


            string message = "The Following Files were saved....\n";
            if (updated)
            {
                message = "The following files were either Created or Updated...\n";
            }
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
        private void ExpansionLoadoutsManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            foreach (AILoadouts AILO in LoadoutList)
            {
                if (AILO.isDirty)
                {
                    needtosave = true;
                }
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    savefiles();
                }
            }
        }
    }
}
