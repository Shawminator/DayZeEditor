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

    public partial class ExpansionAI : DarkForm
    {
        public Project currentproject { get; internal set; }

        public AISettings AISettings { get; set; }
        public AIPatrolSettings AIPatrolSettings { get; set; }
        public BindingList<AILoadouts> LoadoutList { get; private set; }
        public BindingList<string> LoadoutNameList1 { get; private set; }
        public BindingList<string> LoadoutNameList2 { get; private set; }

        public string AISettingsPath;
        public string AILoadoutsPath;
        public string AIPatrolSettingsPath;

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
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
        public ExpansionAI()
        {
            InitializeComponent();
        }
        private void ExpansionAI_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            bool needtosave = false;

            LoadoutList = new BindingList<AILoadouts>();
            AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
                    AILoadouts.Filename = file.FullName;
                    AILoadouts.Setname();
                    AILoadouts.isDirty = false;
                    LoadoutList.Add(AILoadouts);
                }
                catch { }
            }
            SetupAILoadouts();
            SetupLoadoutList();

            AISettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings\\AISettings.json";
            if (!File.Exists(AISettingsPath))
            {
                AISettings = new AISettings();
                needtosave = true;
            }
            else
            {
                AISettings = JsonSerializer.Deserialize<AISettings>(File.ReadAllText(AISettingsPath));
                AISettings.isDirty = false;
                if (AISettings.checkver())
                    needtosave = true;
            }
            AISettings.Filename = AISettingsPath;
            SetupAISettings();



            AIPatrolSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\AIPatrolSettings.json";
            if (!File.Exists(AIPatrolSettingsPath))
            {
                AIPatrolSettings = new AIPatrolSettings();
                needtosave = true;
            }
            else
            {
                AIPatrolSettings = JsonSerializer.Deserialize<AIPatrolSettings>(File.ReadAllText(AIPatrolSettingsPath));
                AIPatrolSettings.isDirty = false;
                if (AIPatrolSettings.checkver())
                    needtosave = true;
            }
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.Filename = AIPatrolSettingsPath;
            SetupAIPatrolSettings();

            tabControl1.ItemSize = new Size(0, 1);

            if (needtosave)
            {
                savefiles(true);
            }


        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles(bool updated = false)
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (AISettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AISettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AISettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AISettings.Filename, Path.GetDirectoryName(AISettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AISettings.Filename) + ".bak", true);
                }
                AISettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AISettings, options);
                File.WriteAllText(AISettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AISettings.Filename));
            }
            if (AIPatrolSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AIPatrolSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AIPatrolSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AIPatrolSettings.Filename, Path.GetDirectoryName(AIPatrolSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AIPatrolSettings.Filename) + ".bak", true);
                }
                AIPatrolSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AIPatrolSettings, options);
                File.WriteAllText(AIPatrolSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AIPatrolSettings.Filename));
            }

            foreach(AILoadouts AILO in LoadoutList)
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
        private void SetupLoadoutList()
        {
            useraction = false;
            LoadoutNameList1 = new BindingList<string>();
            foreach(AILoadouts lo in LoadoutList)
            {
                LoadoutNameList1.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }
            CrashLoadoutFileCB.DisplayMember = "DisplayName";
            CrashLoadoutFileCB.ValueMember = "Value";
            CrashLoadoutFileCB.DataSource = LoadoutNameList1;
            StaticPatrolLB.Refresh();

            LoadoutNameList2 = new BindingList<string>();
            foreach (AILoadouts lo in LoadoutList)
            {
                LoadoutNameList2.Add(Path.GetFileNameWithoutExtension(lo.Filename));
            }

            StaticPatrolLoadoutsCB.DisplayMember = "DisplayName";
            StaticPatrolLoadoutsCB.ValueMember = "Value";
            StaticPatrolLoadoutsCB.DataSource = LoadoutNameList2;
            EventCrachPatrolLB.Refresh();
            useraction = true;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton7.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton8.Checked = true;
                    break;
                case 1:
                    toolStripButton3.Checked = true;
                    break;
                case 2:
                    toolStripButton7.Checked = true;
                    break;

            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                File.Delete(CurrentAILoadouts.Filename);
                LoadoutList.Remove(CurrentAILoadouts);
                SetupLoadoutList();
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
            AILoadouts newAILoadouts = new AILoadouts()
            {
                Name = UserAnswer,
                Filename = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts\\" + UserAnswer + ".Json",
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
            SetupLoadoutList();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings");
                    break;
                case 1:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
                case 2:
                    Process.Start(AILoadoutsPath);
                    break;
            }
        }
        #region Loadouts
        public AILoadouts CurrentAILoadouts;
        public Inventoryattachment CurrentInventoryattachment;
        public AILoadouts CurrentInventoryAttchmentItems;
        public AILoadouts Currentcargoitem;
        public Health Currenthealth;
        public Quantity CurrentQuantity;
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
            CurrentAILoadouts = loadoutsLB.SelectedItem as AILoadouts;
            useraction = false;
            SetupLoadoutTreeView();
            useraction = true;
        }
        public object CurrentTreeNodeTag;
        private void SetupLoadoutTreeView()
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(CurrentAILoadouts.Name))
            {
                Tag = "Parent"
            };
            TreeNode inventoryAttchemnts = new TreeNode("inventoryAttchemnts")
            {
                Tag = "inventoryAttchemnts"
            };
            foreach (Inventoryattachment IA in CurrentAILoadouts.InventoryAttachments)
            {

                inventoryAttchemnts.Nodes.Add(inventoryAttchmentsTN(IA));
            }
            TreeNode InventoryCargo = new TreeNode("InventoryCargo")
            {
                Tag = "InventoryCargo"
            };
            foreach (AILoadouts IA in CurrentAILoadouts.InventoryCargo)
            {

                InventoryCargo.Nodes.Add(inventoryAttchemntsItemsTN(IA));
            }
            TreeNode Sets = new TreeNode("Sets")
            {
                Tag = "Sets"
            };
            foreach (AILoadouts IA in CurrentAILoadouts.Sets)
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
            foreach(Inventoryattachment IA in AILoadouts.InventoryAttachments)
            {
                AILitems.Nodes.Add(inventoryAttchmentsTN(IA));
            }
            TreeNode cargotn = InventoryCargo(AILoadouts.InventoryCargo);
            if(cargotn != null)
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
            else if(e.Node.Parent != null && e.Node.Tag is Inventoryattachment)
            {
                InventoryattchemntGB.Visible = true;
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
            else if (e.Node.Parent != null  && e.Node.Tag is AILoadouts)
            {
                LoadOutGB.Visible = true;
                Currentcargoitem = e.Node.Tag as AILoadouts;

                useraction = false;
                textBox1.Text = Currentcargoitem.ClassName;
                numericUpDown1.Value = Currentcargoitem.Chance;
                numericUpDown2.Value = Currentcargoitem.Quantity.Min;
                numericUpDown3.Value = Currentcargoitem.Quantity.Max;

                if (Currentcargoitem.Health.Count > 0)
                {
                    HealthGB.Visible = true;
                }
                
                listBox1.DisplayMember = "DisplayName";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = Currentcargoitem.Health;
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string Slot = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            if (Slot == "Default Slot") Slot = "";
            CurrentInventoryattachment.SlotName = Slot;
            treeViewMS1.SelectedNode.Text = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            CurrentAILoadouts.isDirty = true;
        }
        private void AddNewAttachmentItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventoryattachment newIA = new Inventoryattachment()
            {
                SlotName = "Back",
                Items = new BindingList<AILoadouts>()
            };
            CurrentAILoadouts.InventoryAttachments.Add(newIA);
            TreeNode newnode = new TreeNode(newIA.SlotName)
            {
                Tag = newIA
            };
            treeViewMS1.SelectedNode.Nodes.Add(newnode);
            CurrentAILoadouts.isDirty = true;
        }
        private void RemoveAttachemtItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentAILoadouts.InventoryAttachments.Remove(CurrentInventoryattachment);
            treeViewMS1.SelectedNode.Remove();
            CurrentAILoadouts.isDirty = true;
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
                CurrentAILoadouts.InventoryCargo.Add(ailoadout);
                treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            }
            else
            {
                Currentcargoitem = treeViewMS1.SelectedNode.Tag as AILoadouts;
                TreeNode newtreenode = new TreeNode("Cargo");
                newtreenode.Tag = "Cargo";
                if (Currentcargoitem.InventoryCargo.Count == 0)
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
               
                Currentcargoitem.InventoryCargo.Add(ailoadout);
                
            }
            CurrentAILoadouts.isDirty = true;
        }
        private void RemoveCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts Parentloadout = treeViewMS1.SelectedNode.Parent.Tag as AILoadouts;
            Parentloadout.Sets.Remove(Currentcargoitem);
            treeViewMS1.SelectedNode.Remove();
            CurrentAILoadouts.isDirty = true;
        }
        private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void LoadOut_Enter(object sender, EventArgs e)
        {

        }
        private void addNewItemToolStripMenuItem_Click_1(object sender, EventArgs e)
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
            ailoadout.Health.Add(new Health() {Zone = "", Min = (decimal)0.7, Max = (decimal)1.0 });
            CurrentInventoryattachment.Items.Add(ailoadout);
            TreeNode newailoadit = new TreeNode("New Item, Replace me.")
            {
                Tag = ailoadout
            };
            treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            CurrentAILoadouts.isDirty = true;


        }
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode Parent = treeViewMS1.SelectedNode.Parent;
            if (treeViewMS1.SelectedNode.Parent.Tag is Inventoryattachment)
            {
                Inventoryattachment IA = treeViewMS1.SelectedNode.Parent.Tag as Inventoryattachment;
                IA.Items.Remove(Currentcargoitem);
                treeViewMS1.SelectedNode.Remove();
                if (IA.Items.Count == 0)
                    Parent.Remove();
                CurrentAILoadouts.isDirty = true;
            }
            else if (treeViewMS1.SelectedNode.Tag is AILoadouts)
            {
                if (treeViewMS1.SelectedNode.Parent.Text == "InventoryCargo")
                {
                    CurrentAILoadouts.InventoryCargo.Remove(Currentcargoitem);
                    treeViewMS1.SelectedNode.Remove();
                    CurrentAILoadouts.isDirty = true;
                }
                if (treeViewMS1.SelectedNode.Parent.Text == "Sets")
                {
                    CurrentAILoadouts.Sets.Remove(Currentcargoitem);
                    treeViewMS1.SelectedNode.Remove();
                    CurrentAILoadouts.isDirty = true;
                }
                else if (treeViewMS1.SelectedNode.Parent.Text == "Attachment")
                {
                    AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
                }
                else if (treeViewMS1.SelectedNode.Parent.Text == "Cargo")
                {
                    AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
                    IA.InventoryCargo.Remove(Currentcargoitem);
                    treeViewMS1.SelectedNode.Remove();
                    if (IA.InventoryCargo.Count == 0)
                        Parent.Remove();
                    CurrentAILoadouts.isDirty = true;
                }
            }
        }
        private void RemoveSetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            CurrentAILoadouts.Sets.Add(ailoadout);
            TreeNode newailoadit = new TreeNode("Set")
            {
                Tag = ailoadout
            };
            treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
            CurrentAILoadouts.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcargoitem.ClassName = treeViewMS1.SelectedNode.Text = textBox1.Text;
            CurrentAILoadouts.isDirty = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcargoitem.Chance = numericUpDown1.Value;
            CurrentAILoadouts.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcargoitem.Quantity.Min = numericUpDown2.Value;
            CurrentAILoadouts.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currentcargoitem.Quantity.Max = numericUpDown2.Value;
            CurrentAILoadouts.isDirty = true;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Min = numericUpDown4.Value;
            CurrentAILoadouts.isDirty = true;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Max = numericUpDown5.Value;
            CurrentAILoadouts.isDirty = true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Currenthealth.Zone = textBox2.Text;
            CurrentAILoadouts.isDirty = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            Health newhealth = new Health()
            {
                Min = (decimal)0.7,
                Max = (decimal)1.0,
                Zone = ""
            };
            Currentcargoitem.Health.Add(newhealth);
            CurrentAILoadouts.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            Currentcargoitem.Health.Remove(Currenthealth);
            CurrentAILoadouts.isDirty = true;
        }
        #endregion Loadouts
        #region aipatrolsettings
        public ObjectPatrols CurrentEventcrashpatrol;
        public Patrols CurrentPatrol;
        public float[] CurrentWapypoint;
        private void SetupAIPatrolSettings()
        {
            useraction = false;



            AIPatrolSettingsEnabledCB.Checked = AIPatrolSettings.Enabled == 1 ? true : false;
            RespawnTimeNUD.Value = AIPatrolSettings.RespawnTime;
            MinDistRadiusNUD.Value = AIPatrolSettings.MinDistRadius;
            MaxDistRadiusNUD.Value = AIPatrolSettings.MaxDistRadius;
            DespawnRadiusNUD.Value = AIPatrolSettings.DespawnRadius;

            EventCrachPatrolLB.DisplayMember = "DisplayName";
            EventCrachPatrolLB.ValueMember = "Value";
            EventCrachPatrolLB.DataSource = AIPatrolSettings.ObjectPatrols;

            StaticPatrolLB.DisplayMember = "DisplayName";
            StaticPatrolLB.ValueMember = "Value";
            StaticPatrolLB.DataSource = AIPatrolSettings.Patrols;


            useraction = true;
        }
        private void AIPatrolSettingsEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.Enabled = AIPatrolSettingsEnabledCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void RespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.RespawnTime = RespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void MinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.MinDistRadius = MinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void MaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.MaxDistRadius = MaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void DespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AIPatrolSettings.DespawnRadius = DespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        /// <summary>
        /// Event Crash Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventCrachPatrolLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventCrachPatrolLB.SelectedItems.Count < 1) return;
            CurrentEventcrashpatrol = EventCrachPatrolLB.SelectedItem as ObjectPatrols;
            useraction = false;
            CrashEventNameTB.Text = CurrentEventcrashpatrol.ClassName;
            CrashFactionCB.SelectedIndex = CrashFactionCB.FindStringExact(CurrentEventcrashpatrol.Faction);
            CrashNumberOfAINUD.Value = CurrentEventcrashpatrol.NumberOfAI;
            CrashBehaviourCB.SelectedIndex = CrashBehaviourCB.FindStringExact(CurrentEventcrashpatrol.Behaviour);
            CrashSpeedCB.SelectedIndex = CrashSpeedCB.FindStringExact(CurrentEventcrashpatrol.Speed);
            CrashUnderThreatSpeedCB.SelectedIndex = CrashUnderThreatSpeedCB.FindStringExact(CurrentEventcrashpatrol.UnderThreatSpeed);
            CrashMinDistRadiusNUD.Value = CurrentEventcrashpatrol.MinDistRadius;
            CrashMaxDistRadiusNUD.Value = CurrentEventcrashpatrol.MaxDistRadius;
            CrashDespawnRadiusNUD.Value = CurrentEventcrashpatrol.DespawnRadius;
            CrashMinSpreadRadiusNUD.Value = CurrentEventcrashpatrol.MinSpreadRadius;
            CrashMaxSpreadRadiusNUD.Value = CurrentEventcrashpatrol.MaxSpreadRadius;
            CrashChanceNUD.Value = CurrentEventcrashpatrol.Chance;
            CrashCanBeLootedCB.Checked = CurrentEventcrashpatrol.CanBeLooted == 1 ? true : false;
            CrashUnlimitedReloadCB.Checked = CurrentEventcrashpatrol.UnlimitedReload == 1 ? true : false;
            CrashLoadoutFileCB.SelectedIndex = CrashLoadoutFileCB.FindStringExact(CurrentEventcrashpatrol.LoadoutFile);
            CrashFormationCB.SelectedIndex = CrashFormationCB.FindStringExact(CurrentEventcrashpatrol.Formation);
            CrashWaypointInterpolationCB.SelectedIndex = CrashWaypointInterpolationCB.FindStringExact(CurrentEventcrashpatrol.WaypointInterpolation);
            useraction = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            ObjectPatrols newpatrol = new ObjectPatrols()
            {
                Name = "NewName",
                Faction = "WEST",
                ClassName = "Your Classname Goes Here",
                LoadoutFile = "",
                NumberOfAI = 5,
                Behaviour = "PATROL",
                Speed = "WALK",
                UnderThreatSpeed = "SPRINT",
                MinDistRadius = (decimal)-2.0,
                MaxDistRadius = (decimal)-2.0,
                CanBeLooted = 1,
                UnlimitedReload = 0,
                Chance = (decimal)1.0
            };
            
            AIPatrolSettings.ObjectPatrols.Add(newpatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AIPatrolSettings.ObjectPatrols.Remove(CurrentEventcrashpatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
        }
        private void CrashEventNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.ClassName = CrashEventNameTB.Text;
            AIPatrolSettings.isDirty = true;
            EventCrachPatrolLB.Refresh();
        }
        private void CrashFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Faction = CrashFactionCB.GetItemText(CrashFactionCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashLoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.LoadoutFile = CrashLoadoutFileCB.GetItemText(CrashLoadoutFileCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.NumberOfAI = (int)CrashNumberOfAINUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashBehaviourCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Behaviour = CrashBehaviourCB.GetItemText(CrashBehaviourCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Speed = CrashSpeedCB.GetItemText(CrashSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashUnderThreatSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.UnderThreatSpeed = CrashUnderThreatSpeedCB.GetItemText(CrashUnderThreatSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MaxDistRadius = CrashMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MaxDistRadius = CrashMaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.DespawnRadius = CrashDespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMinSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MinSpreadRadius = CrashMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashMaxSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.MaxSpreadRadius = CrashMaxSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Chance = CrashChanceNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashUnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.UnlimitedReload = CrashUnlimitedReloadCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashCanBeLootedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.CanBeLooted = CrashCanBeLootedCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void CrashFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.Formation = CrashFormationCB.GetItemText(CrashFormationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void CrashWaypointInterpolationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentEventcrashpatrol.WaypointInterpolation = CrashWaypointInterpolationCB.GetItemText(CrashWaypointInterpolationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        /// <summary>
        /// Stataic Patrol Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StaticPatrolLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolLB.SelectedItems.Count < 1) return;
            CurrentPatrol = StaticPatrolLB.SelectedItem as Patrols;
            useraction = false;
            StaticPatrolFactionCB.SelectedIndex = StaticPatrolFactionCB.FindStringExact(CurrentPatrol.Faction);
            StaticPatrolNumberOfAINUD.Value = CurrentPatrol.NumberOfAI;
            StaticPatrolBehaviorCB.SelectedIndex = StaticPatrolBehaviorCB.FindStringExact(CurrentPatrol.Behaviour);
            StaticPatrolSpeedCB.SelectedIndex = StaticPatrolSpeedCB.FindStringExact(CurrentPatrol.Speed);
            StaticPatrolUnderThreatSpeedCB.SelectedIndex = StaticPatrolUnderThreatSpeedCB.FindStringExact(CurrentPatrol.UnderThreatSpeed);
            StaticPatrolRespawnTimeNUD.Value = CurrentPatrol.RespawnTime;
            StaticPatrolMinDistRadiusNUD.Value = CurrentPatrol.MinDistRadius;
            StaticPatrolMaxDistRadiusNUD.Value = CurrentPatrol.MaxDistRadius;
            StaticPatrolDespawnRadiusNUD.Value = CurrentPatrol.DespawnRadius;
            StaticPatrolChanceCB.Value = CurrentPatrol.Chance;
            StaticPatrolCanBeLotedCB.Checked = CurrentPatrol.CanBeLooted == 1 ? true : false;
            StaticPatrolUnlimitedReloadCB.Checked = CurrentPatrol.UnlimitedReload == 1 ? true : false;
            StaticPatrolLoadoutsCB.SelectedIndex = StaticPatrolLoadoutsCB.FindStringExact(CurrentPatrol.LoadoutFile);
            StaticPatrolMinSpreadRadiusNUD.Value = CurrentPatrol.MinSpreadRadius;
            StaticPatrolMaxSpreadRadiusNUD.Value = CurrentPatrol.MaxSpreadRadius;
            StaticPatrolFormationCB.SelectedIndex = StaticPatrolFormationCB.FindStringExact(CurrentPatrol.Formation);
            StaticPatrolWaypointInterpolationCB.SelectedIndex = StaticPatrolWaypointInterpolationCB.FindStringExact(CurrentPatrol.WaypointInterpolation);

            StaticPatrolWayPointsLB.DisplayMember = "DisplayName";
            StaticPatrolWayPointsLB.ValueMember = "Value";
            StaticPatrolWayPointsLB.DataSource = CurrentPatrol.Waypoints;

            useraction = true;
        }
        private void StaticPatrolWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = StaticPatrolWayPointsLB.SelectedItem as float[];
            useraction = false;
            StaticPatrolWaypointPOSXNUD.Value = (decimal)CurrentWapypoint[0];
            StaticPatrolWaypointPOSYNUD.Value = (decimal)CurrentWapypoint[1];
            StaticPatrolWaypointPOSZNUD.Value = (decimal)CurrentWapypoint[2];
            useraction = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CurrentPatrol.Waypoints.Clear();
                     }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        float[] newfloatarray = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                        CurrentPatrol.Waypoints.Add(newfloatarray);

                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                }
            }


            AIPatrolSettings.SetPatrolNames();
        }
        private void darkButton17_Click(object sender, EventArgs e)
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
                        CurrentPatrol.Waypoints.Clear();
                    }
                    foreach(Editorobject eo in importfile.EditorObjects)
                    {
                        float[] newfloatarray = new float[] { Convert.ToSingle(eo.Position[0]), Convert.ToSingle(eo.Position[1]), Convert.ToSingle(eo.Position[2]) };
                        CurrentPatrol.Waypoints.Add(newfloatarray);
                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                }
            }
        }
        private void StaticPatrolFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Faction = StaticPatrolFactionCB.GetItemText(StaticPatrolFactionCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolLoadoutsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.LoadoutFile = StaticPatrolLoadoutsCB.GetItemText(StaticPatrolLoadoutsCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.NumberOfAI = (int)StaticPatrolNumberOfAINUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolBehaviorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Behaviour = StaticPatrolBehaviorCB.GetItemText(StaticPatrolBehaviorCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Speed = StaticPatrolSpeedCB.GetItemText(StaticPatrolSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolUnderThreatSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.UnderThreatSpeed = StaticPatrolUnderThreatSpeedCB.GetItemText(StaticPatrolUnderThreatSpeedCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolRespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.RespawnTime = StaticPatrolRespawnTimeNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MinDistRadius = StaticPatrolMinDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MaxDistRadius = StaticPatrolMaxDistRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.DespawnRadius = StaticPatrolDespawnRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolChanceCB_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Chance = StaticPatrolChanceCB.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMinSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MinSpreadRadius = (int)StaticPatrolMinSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolMaxSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.MaxDistRadius = (int)StaticPatrolMaxSpreadRadiusNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolUnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.UnlimitedReload = StaticPatrolUnlimitedReloadCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolCanBeLotedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.CanBeLooted = StaticPatrolCanBeLotedCB.Checked == true ? 1 : 0;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[0] = (float)StaticPatrolWaypointPOSXNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[1] = (float)StaticPatrolWaypointPOSYNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            CurrentWapypoint[2] = (float)StaticPatrolWaypointPOSZNUD.Value;
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.Formation = StaticPatrolFormationCB.GetItemText(StaticPatrolFormationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void StaticPatrolWaypointInterpolationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentPatrol.WaypointInterpolation = StaticPatrolWaypointInterpolationCB.GetItemText(StaticPatrolWaypointInterpolationCB.SelectedItem);
            AIPatrolSettings.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            Patrols newpatrol = new Patrols()
            {
                Name = "NEwPatrol",
                Faction = "WEST",
                LoadoutFile = "",
                NumberOfAI = 5,
                Behaviour = "PATROL",
                Speed = "WALK",
                UnderThreatSpeed = "SPRINT",
                MinDistRadius = (decimal)-2.0,
                MaxDistRadius = (decimal)-2.0,
                MinSpreadRadius = (decimal)5.0,
                MaxSpreadRadius = (decimal)20.0,
                CanBeLooted = 1,
                UnlimitedReload = 0,
                Chance = (decimal)1.0,
                Waypoints = new BindingList<float[]>()
            };
            AIPatrolSettings.Patrols.Add(newpatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
            StaticPatrolLB.Refresh();
            StaticPatrolWayPointsLB.Refresh();
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            AIPatrolSettings.Patrols.Remove(CurrentPatrol);
            AIPatrolSettings.SetPatrolNames();
            AIPatrolSettings.isDirty = true;
            StaticPatrolLB.Refresh();
            StaticPatrolWayPointsLB.Refresh();
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            foreach (float[] array in CurrentPatrol.Waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }

        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            foreach (float[] array in CurrentPatrol.Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = array,
                    Orientation = new float[] {0,0,0},
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }

        }
        #endregion aipatrolsettings
        #region AISettings
        private void SetupAISettings()
        {
            useraction = false;
            AccuracyMinNUD.Value = AISettings.AccuracyMin;
            AccuracyMaxNUD.Value = AISettings.AccuracyMax;
            MaximumDynamicPatrolsNUD.Value = AISettings.MaximumDynamicPatrols;
            VaultingCB.Checked = AISettings.Vaulting == 1 ? true : false;
            MannersCB.Checked = AISettings.Manners == 1 ? true : false;

            AISettingsAdminsLB.DisplayMember = "DisplayName";
            AISettingsAdminsLB.ValueMember = "Value";
            AISettingsAdminsLB.DataSource = AISettings.Admins;

            PlayerFactionsLB.DisplayMember = "DisplayName";
            PlayerFactionsLB.ValueMember = "Value";
            PlayerFactionsLB.DataSource = AISettings.PlayerFactions;

            useraction = true;
        }
        private void AccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.AccuracyMin = AccuracyMinNUD.Value;
            AISettings.isDirty = true;
        }
        private void AccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.AccuracyMax = AccuracyMaxNUD.Value;
            AISettings.isDirty = true;
        }
        private void MaximumDynamicPatrolsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.MaximumDynamicPatrols = (int)MaximumDynamicPatrolsNUD.Value;
            AISettings.isDirty = true;
        }
        private void MannersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.Manners = MannersCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void VaultingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AISettings.Vaulting = VaultingCB.Checked == true ? 1 : 0;
            AISettings.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    AISettings.Admins.Add(l.ToLower());
                    AISettings.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            AISettings.Admins.Remove(AISettingsAdminsLB.GetItemText(AISettingsAdminsLB.SelectedItem));
            AISettings.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            AISettings.PlayerFactions.Add(PlayerFactionCB.GetItemText(PlayerFactionCB.SelectedItem));
            AISettings.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            AISettings.PlayerFactions.Remove(PlayerFactionsLB.GetItemText(PlayerFactionsLB.SelectedItem));
            AISettings.isDirty = true;
        }





        #endregion AISettings


    }
}
