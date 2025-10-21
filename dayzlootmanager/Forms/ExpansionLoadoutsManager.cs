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
using System.Xml.Linq;
using TreeViewMS;

namespace DayZeEditor
{
    //public partial class ExpansionLoadoutsManager : DarkForm
    //{
    //    public Project currentproject { get; internal set; }
    //    public AILoadoutsConfig AILoadoutsConfig { get; private set; }

    //    public string AILoadoutsPath;
    //    public string LootDropOnDeathListPath;

    //    public TypesFile vanillatypes;
    //    public TypesFile Expansiontypes;
    //    public List<TypesFile> ModTypes;

    //    public AILoadouts CurrentAILoadoutsFile { get; private set; }
    //    public AILootDrops currentAILootDropsFile { get; set; }
    //    public Inventoryattachment CurrentInventoryattachment { get; private set; }
    //    public AILoadouts CurrentAIloadouts { get; private set; }
    //    public Health Currenthealth { get; private set; }

    //    public TreeNode CurrentTreenode { get; set; }

    //    private bool useraction;

    //    private void listBox_DrawItem(object sender, DrawItemEventArgs e)
    //    {
    //        if (e.Index < 0) return;
    //        ListBox lb = sender as ListBox;
    //        e.DrawBackground();
    //        Brush myBrush = Brushes.Black;
    //        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
    //        {
    //            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
    //        }
    //        else
    //        {
    //            myBrush = Brushes.White;
    //            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 63, 65)), e.Bounds);
    //        }
    //        e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds);
    //        e.DrawFocusRectangle();
    //    }

    //    public ExpansionLoadoutsManager()
    //    {
    //        InitializeComponent();
    //    }

    //    private void ExpansionLoadoutsManager_Load(object sender, EventArgs e)
    //    {
    //        vanillatypes = currentproject.getvanillatypes();
    //        ModTypes = currentproject.getModList();

    //        AILoadoutsConfig = new AILoadoutsConfig();
    //        AILoadoutsConfig.LoadoutsData = new BindingList<AILoadouts>();
    //        AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
    //        DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
    //        FileInfo[] Files = dinfo.GetFiles("*.json");
    //        foreach (FileInfo file in Files)
    //        {
    //            try
    //            {
    //                Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
    //                AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
    //                AILoadouts.Filename = file.FullName;
    //                AILoadouts.Setname();
    //                AILoadouts.isDirty = false;
    //                AILoadoutsConfig.LoadoutsData.Add(AILoadouts);
    //            }
    //            catch { }
    //        }

    //        AILoadoutsConfig.AILootDropsData = new BindingList<AILootDrops>();
    //        LootDropOnDeathListPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\LootDrops";
    //        DirectoryInfo dinfo1 = new DirectoryInfo(LootDropOnDeathListPath);
    //        FileInfo[] Files1 = dinfo1.GetFiles("*.json");
    //        foreach (FileInfo file in Files1)
    //        {
    //            try
    //            {
    //                Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
    //                AILootDrops AILootDrops = new AILootDrops();
    //                AILootDrops.LootdropList = JsonSerializer.Deserialize<BindingList<AILoadouts>>(File.ReadAllText(file.FullName));
    //                AILootDrops.Filename = file.FullName;
    //                AILootDrops.Setname();
    //                AILootDrops.isDirty = false;
    //                AILoadoutsConfig.AILootDropsData.Add(AILootDrops);
    //            }
    //            catch { }
    //        }

    //        SetupAILoadouts();
    //    }
    //    private void SetupAILoadouts()
    //    {
    //        useraction = false;


    //        treeViewMS1.Nodes.Clear();
    //        TreeNode Loadoutroot = new TreeNode("Loadouts")
    //        {
    //            Tag = "LoadoutsRoot"
    //        };
    //        foreach(AILoadouts AILoadouts in AILoadoutsConfig.LoadoutsData)
    //        {
    //            Loadoutroot.Nodes.Add(SetupLoadoutTreeView(AILoadouts));
    //        }

    //        treeViewMS1.Nodes.Add(Loadoutroot);

    //        TreeNode LootDropsroot = new TreeNode("LootDrops")
    //        {
    //            Tag = "LootDropsRoot"
    //        };
    //        foreach (AILootDrops AILootDrops in AILoadoutsConfig.AILootDropsData)
    //        {
    //            TreeNode lootdropfile = new TreeNode(AILootDrops.Name)
    //            {
    //                Tag = AILootDrops
    //            };
    //            foreach (AILoadouts AILoadouts in AILootDrops.LootdropList)
    //            {
    //                lootdropfile.Nodes.Add(inventoryAttchemntsItemsTN(AILoadouts));
    //            }
    //            LootDropsroot.Nodes.Add(lootdropfile);
    //        }

    //        treeViewMS1.Nodes.Add(LootDropsroot);



    //        useraction = true;
    //    }

    //    public object CurrentTreeNodeTag;
    //    private TreeNode SetupLoadoutTreeView(AILoadouts AILoadouts)
    //    {

    //        TreeNode root = new TreeNode(Path.GetFileName(AILoadouts.ToString()))
    //        {
    //            Tag = AILoadouts
    //        };
    //        TreeNode inventoryAttchemnts = new TreeNode("inventoryAttchemnts")
    //        {
    //            Tag = "inventoryAttchemnts"
    //        };
    //        foreach (Inventoryattachment IA in AILoadouts.InventoryAttachments)
    //        {

    //            inventoryAttchemnts.Nodes.Add(inventoryAttchmentsTN(IA));
    //        }
    //        TreeNode InventoryCargo = new TreeNode("InventoryCargo")
    //        {
    //            Tag = "InventoryCargo"
    //        };
    //        foreach (AILoadouts IA in AILoadouts.InventoryCargo)
    //        {

    //            InventoryCargo.Nodes.Add(inventoryAttchemntsItemsTN(IA));
    //        }
    //        TreeNode Sets = new TreeNode("Sets")
    //        {
    //            Tag = "Sets"
    //        };
    //        foreach (AILoadouts IA in AILoadouts.Sets)
    //        {
    //            Sets.Nodes.Add(inventoryAttchemntsItemsTN(IA));
    //        }
    //        root.Nodes.Add(inventoryAttchemnts);
    //        root.Nodes.Add(InventoryCargo);
    //        root.Nodes.Add(Sets);

    //        return root;
    //    }
    //    TreeNode inventoryAttchmentsTN(Inventoryattachment Inventoryattachment)
    //    {
    //        string slotname;
    //        if (Inventoryattachment.SlotName == "")
    //            slotname = "Default Slot";
    //        else
    //            slotname = Inventoryattachment.SlotName;
    //        TreeNode IAnode = new TreeNode(slotname)
    //        {
    //            Tag = Inventoryattachment
    //        };
    //        foreach (AILoadouts AIL in Inventoryattachment.Items)
    //        {
    //            IAnode.Nodes.Add(inventoryAttchemntsItemsTN(AIL));
    //        }
    //        return IAnode;
    //    }
    //    TreeNode inventoryAttchemntsItemsTN(AILoadouts AILoadouts)
    //    {
    //        string slotname;
    //        if (AILoadouts.ClassName == "")
    //            slotname = "Set";
    //        else
    //            slotname = AILoadouts.ClassName;
    //        TreeNode AILitems = new TreeNode(slotname)
    //        {
    //            Tag = AILoadouts
    //        };
    //        foreach (Inventoryattachment IA in AILoadouts.InventoryAttachments)
    //        {
    //            AILitems.Nodes.Add(inventoryAttchmentsTN(IA));
    //        }
    //        TreeNode cargotn = InventoryCargo(AILoadouts.InventoryCargo);
    //        if (cargotn != null)
    //            AILitems.Nodes.Add(cargotn);
    //        foreach (AILoadouts IA in AILoadouts.Sets)
    //        {
    //            AILitems.Nodes.Add(inventoryAttchemntsItemsTN(IA));
    //        }
    //        return AILitems;
    //    }
    //    TreeNode InventoryCargo(BindingList<AILoadouts> aILoadoutsList)
    //    {
    //        if (aILoadoutsList.Count == 0) return null;
    //        TreeNode tn = new TreeNode("Cargo");
    //        tn.Tag = "Cargo";
    //        foreach (AILoadouts IA in aILoadoutsList)
    //        {
    //            tn.Nodes.Add(inventoryAttchemntsItemsTN(IA));
    //        }
    //        return tn;
    //    }
    //    private void darkButton15_Click(object sender, EventArgs e)
    //    {
    //        string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
    //        AILoadouts newAILoadouts = new AILoadouts()
    //        {
    //            Name = UserAnswer,
    //            Filename = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts\\" + UserAnswer + ".json",
    //            ClassName = "",
    //            Chance = (decimal)1.0,
    //            Quantity = new Quantity(),
    //            Health = new BindingList<Health>(),
    //            InventoryAttachments = new BindingList<Inventoryattachment>(),
    //            InventoryCargo = new BindingList<AILoadouts>(),
    //            ConstructionPartsBuilt = new BindingList<object>(),
    //            Sets = new BindingList<AILoadouts>(),
    //            isDirty = true
    //        };
    //        AILoadoutsConfig.LoadoutsData.Add(newAILoadouts);
    //        CurrentTreenode.Nodes.Add(new TreeNode(newAILoadouts.Name)
    //        {
    //            Tag = newAILoadouts
    //        });
    //    }
    //    private void darkButton16_Click(object sender, EventArgs e)
    //    {
    //        if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
    //        {
    //            File.Delete(CurrentAILoadoutsFile.Filename);
    //            AILoadoutsConfig.LoadoutsData.Remove(CurrentAILoadoutsFile);
    //        }
    //    }
    //    private void treeViewMS1_AfterSelect(object sender, TreeViewEventArgs e)
    //    {
    //        useraction = false;
    //        treeViewMS1.SelectedNode = e.Node;
    //        CurrentTreeNodeTag = e.Node.Tag;
    //        CurrentTreenode = e.Node;

    //        AddNewAttachmentItemToolStripMenuItem.Visible = false;
    //        RemoveAttachemtItemToolStripMenuItem.Visible = false;
    //        AddNewCargoItemToolStripMenuItem.Visible = false;
    //        RemoveCargoItemToolStripMenuItem.Visible = false;
    //        AddNewSetItemToolStripMenuItem.Visible = false;
    //        RemoveSetItemToolStripMenuItem.Visible = false;
    //        addNewItemToolStripMenuItem.Visible = false;
    //        removeItemToolStripMenuItem.Visible = false;

    //        LoadOutGB.Visible = false;
    //        InventoryattchemntGB.Visible = false;
    //        //HealthGB.Visible = false;
    //        LoadOutGB.Visible = false;

    //        CurrentAIloadouts = null;

    //        CurrentAILoadoutsFile = null;
    //        currentAILootDropsFile = null;
    //        currentAILootDropsFile = e.Node.FindParentOfType<AILootDrops>();
    //        if (currentAILootDropsFile == null)
    //        {
    //            CurrentAILoadoutsFile = e.Node.FindLastParentOfType<AILoadouts>();
    //        }

    //        if (e.Node.Parent != null && e.Node.Tag is Inventoryattachment)
    //        {
    //            InventoryattchemntGB.Visible = true;
    //            CurrentAIloadouts = e.Node.Parent.Tag as AILoadouts;
    //            CurrentInventoryattachment = e.Node.Tag as Inventoryattachment;
    //            useraction = false;
    //            string slotname = CurrentInventoryattachment.SlotName;
    //            if (CurrentInventoryattachment.SlotName == "")
    //                slotname = "Default Slot";
    //            ItemAttachmentSlotNameCB.SelectedIndex = ItemAttachmentSlotNameCB.FindStringExact(slotname);
    //            useraction = true;
    //        }
    //        else if (e.Node.Parent != null && e.Node.Tag is AILoadouts)
    //        {
    //            LoadOutGB.Visible = true;
    //            CurrentAIloadouts = e.Node.Tag as AILoadouts;


    //            useraction = false;
    //            textBox1.Text = CurrentAIloadouts.ClassName;
    //            numericUpDown1.Value = CurrentAIloadouts.Chance;
    //            numericUpDown2.Value = CurrentAIloadouts.Quantity.Min;
    //            numericUpDown3.Value = CurrentAIloadouts.Quantity.Max;


    //            listBox1.DisplayMember = "DisplayName";
    //            listBox1.ValueMember = "Value";
    //            listBox1.DataSource = CurrentAIloadouts.Health;
    //            if (CurrentAIloadouts.Health.Count > 0)
    //                groupBox1.Visible = true;
    //            else
    //                groupBox1.Visible = false;
    //            useraction = false;

    //        }

    //        useraction = true;
    //    }
    //    private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    //    {
    //        useraction = false;
    //        treeViewMS1.SelectedNode = e.Node;
    //        CurrentTreeNodeTag = e.Node.Tag;

    //        foreach(ToolStripMenuItem c in contextMenuStrip1.Items)
    //        {
    //            c.Visible = false;
    //        }

    //        LoadOutGB.Visible = false;
    //        InventoryattchemntGB.Visible = false;
    //        //HealthGB.Visible = false;
    //        LoadOutGB.Visible = false;

    //        if (e.Node.Tag is string)
    //        {

    //            switch (e.Node.Tag.ToString())
    //            {
    //                case "LootDropsRoot":
    //                    if (e.Button == MouseButtons.Right)
    //                    {
    //                        addNewLootDropFileToolStripMenuItem.Visible = true;
    //                        contextMenuStrip1.Show(Cursor.Position);
    //                    }
    //                    break;
    //                case "inventoryAttchemnts":
    //                    if (e.Button == MouseButtons.Right)
    //                    {
    //                        AddNewAttachmentItemToolStripMenuItem.Visible = true;
    //                        contextMenuStrip1.Show(Cursor.Position);
    //                    }
    //                    break;
    //                case "InventoryCargo":
    //                    if (e.Button == MouseButtons.Right)
    //                    {
    //                        AddNewCargoItemToolStripMenuItem.Visible = true;
    //                        contextMenuStrip1.Show(Cursor.Position);
    //                    }
    //                    break;
    //                case "Sets":
    //                    if (e.Button == MouseButtons.Right)
    //                    {
    //                        AddNewSetItemToolStripMenuItem.Visible = true;
    //                        contextMenuStrip1.Show(Cursor.Position);
    //                    }
    //                    break;
    //                case "Cargo":
    //                    if (e.Button == MouseButtons.Right)
    //                    {
    //                        RemoveCargoItemToolStripMenuItem.Visible = true;
    //                        contextMenuStrip1.Show(Cursor.Position);
    //                    }
    //                    break;
    //            }
    //        }
    //        else if (e.Node.Tag is AILootDrops)
    //        {
    //            if (e.Button == MouseButtons.Right)
    //            {
    //                addNewItemToolStripMenuItem.Visible = true;
    //                RemoveAttachemtItemToolStripMenuItem.Visible = true;
    //                contextMenuStrip1.Show(Cursor.Position);
    //            }
    //        }
    //        else if (e.Node.Parent != null && e.Node.Tag is Inventoryattachment)
    //        {
    //            InventoryattchemntGB.Visible = true;
    //            CurrentAIloadouts = e.Node.Parent.Tag as AILoadouts;
    //            CurrentInventoryattachment = e.Node.Tag as Inventoryattachment;
    //            useraction = false;
    //            string slotname = CurrentInventoryattachment.SlotName;
    //            if (CurrentInventoryattachment.SlotName == "")
    //                slotname = "Default Slot";
    //            ItemAttachmentSlotNameCB.SelectedIndex = ItemAttachmentSlotNameCB.FindStringExact(slotname);
    //            useraction = true;

    //            if (e.Button == MouseButtons.Right)
    //            {
    //                addNewItemToolStripMenuItem.Visible = true;
    //                RemoveAttachemtItemToolStripMenuItem.Visible = true;
    //                contextMenuStrip1.Show(Cursor.Position);
    //            }


    //        }
    //        else if (e.Node.Parent != null && e.Node.Tag is AILoadouts)
    //        {
    //            LoadOutGB.Visible = true;
    //            CurrentAIloadouts = e.Node.Tag as AILoadouts;

    //            useraction = false;
    //            textBox1.Text = CurrentAIloadouts.ClassName;
    //            numericUpDown1.Value = CurrentAIloadouts.Chance;
    //            numericUpDown2.Value = CurrentAIloadouts.Quantity.Min;
    //            numericUpDown3.Value = CurrentAIloadouts.Quantity.Max;

    //            //if (CurrentAIloadouts.Health.Count > 0)
    //            //{
    //            //    HealthGB.Visible = true;
    //            //}

    //            listBox1.DisplayMember = "DisplayName";
    //            listBox1.ValueMember = "Value";
    //            listBox1.DataSource = CurrentAIloadouts.Health;
    //            if (CurrentAIloadouts.Health.Count > 0)
    //                groupBox1.Visible = true;
    //            else
    //                groupBox1.Visible = false;
    //            useraction = false;

    //            if (e.Button == MouseButtons.Right)
    //            {

    //                removeItemToolStripMenuItem.Visible = true;
    //                AddNewAttachmentItemToolStripMenuItem.Visible = true;
    //                AddNewCargoItemToolStripMenuItem.Visible = true;
    //                contextMenuStrip1.Show(Cursor.Position);
    //            }
    //        }

    //        useraction = true;
    //    }
    //    private void AddNewAttachmentItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        Inventoryattachment newIA = new Inventoryattachment()
    //        {
    //            SlotName = "Back",
    //            Items = new BindingList<AILoadouts>()
    //        };
    //        TreeNode newnode = new TreeNode(newIA.SlotName)
    //        {
    //            Tag = newIA
    //        };
    //        if (treeViewMS1.SelectedNode.Text == "inventoryAttchemnts")
    //        {
    //            CurrentAILoadoutsFile.InventoryAttachments.Add(newIA);
    //        }
    //        else
    //        {
    //            CurrentAIloadouts.InventoryAttachments.Add(newIA);
    //        }
    //        treeViewMS1.SelectedNode.Nodes.Add(newnode);

    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void RemoveAttachemtItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        CurrentAILoadoutsFile.InventoryAttachments.Remove(CurrentInventoryattachment);
    //        treeViewMS1.SelectedNode.Remove();
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void AddNewCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        AILoadouts ailoadout = new AILoadouts()
    //        {
    //            ClassName = "New Item, Replace me.",
    //            Chance = (decimal)1.0,
    //            Quantity = new Quantity(),
    //            Health = new BindingList<Health>(),
    //            InventoryAttachments = new BindingList<Inventoryattachment>(),
    //            InventoryCargo = new BindingList<AILoadouts>(),
    //            ConstructionPartsBuilt = new BindingList<object>(),
    //            Sets = new BindingList<AILoadouts>()
    //        };
    //        TreeNode newailoadit = new TreeNode("New Item, Replace me.")
    //        {
    //            Tag = ailoadout
    //        };


    //        if (treeViewMS1.SelectedNode.Text == "InventoryCargo")
    //        {
    //            CurrentAILoadoutsFile.InventoryCargo.Add(ailoadout);
    //            treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
    //        }
    //        else
    //        {
    //            CurrentAIloadouts = treeViewMS1.SelectedNode.Tag as AILoadouts;
    //            TreeNode newtreenode = new TreeNode("Cargo");
    //            newtreenode.Tag = "Cargo";
    //            if (CurrentAIloadouts.InventoryCargo.Count == 0)
    //            {
    //                newtreenode.Nodes.Add(newailoadit);
    //                treeViewMS1.SelectedNode.Nodes.Add(newtreenode);
    //            }
    //            else
    //            {
    //                foreach (TreeNode node in treeViewMS1.SelectedNode.Nodes)
    //                {
    //                    if (node.Text == "Cargo")
    //                    {
    //                        node.Nodes.Add(newailoadit);
    //                        break;
    //                    }
    //                }
    //            }

    //            CurrentAIloadouts.InventoryCargo.Add(ailoadout);
    //        }
    //        if (CurrentAILoadoutsFile != null)
    //            CurrentAILoadoutsFile.isDirty = true;
    //        else if (currentAILootDropsFile != null)
    //            currentAILootDropsFile.isDirty = true;
    //    }
    //    private void RemoveCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        AILoadouts Parentloadout = treeViewMS1.SelectedNode.Parent.Tag as AILoadouts;
    //        Parentloadout.InventoryCargo = new BindingList<AILoadouts>();

    //        treeViewMS1.SelectedNode.Remove();
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void AddNewSetItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        AILoadouts ailoadout = new AILoadouts()
    //        {
    //            ClassName = "",
    //            Chance = (decimal)1.0,
    //            Quantity = new Quantity(),
    //            Health = new BindingList<Health>(),
    //            InventoryAttachments = new BindingList<Inventoryattachment>(),
    //            InventoryCargo = new BindingList<AILoadouts>(),
    //            ConstructionPartsBuilt = new BindingList<object>(),
    //            Sets = new BindingList<AILoadouts>()
    //        };
    //        CurrentAILoadoutsFile.Sets.Add(ailoadout);
    //        TreeNode newailoadit = new TreeNode("Set")
    //        {
    //            Tag = ailoadout
    //        };
    //        treeViewMS1.SelectedNode.Nodes.Add(newailoadit);
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void RemoveSetItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {

    //    }
    //    private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        AILoadouts ailoadout = new AILoadouts()
    //        {
    //            ClassName = "New Item, Replace me.",
    //            Chance = (decimal)1.0,
    //            Quantity = new Quantity(),
    //            Health = new BindingList<Health>(),
    //            InventoryAttachments = new BindingList<Inventoryattachment>(),
    //            InventoryCargo = new BindingList<AILoadouts>(),
    //            ConstructionPartsBuilt = new BindingList<object>(),
    //            Sets = new BindingList<AILoadouts>()
    //        };
    //        ailoadout.Health.Add(new Health() { Zone = "", Min = (decimal)0.7, Max = (decimal)1.0 });

    //        TreeNode newailoadit = new TreeNode("New Item, Replace me.")
    //        {
    //            Tag = ailoadout
    //        };
    //        treeViewMS1.SelectedNode.Nodes.Add(newailoadit);

    //        if(CurrentTreenode.Tag is AILootDrops)
    //        {
    //            currentAILootDropsFile.LootdropList.Add(ailoadout);
    //            currentAILootDropsFile.isDirty = true;
    //        }

    //        //if (CurrentAILoadoutsFile != null)
    //        //{
    //        //    CurrentInventoryattachment.Items.Add(ailoadout);
    //        //    CurrentAILoadoutsFile.isDirty = true;
    //        //}
    //        //else if (currentAILootDropsFile != null)
    //        //{

    //        //    currentAILootDropsFile.isDirty = true;
    //        //}
    //    }
    //    private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        TreeNode Parent = treeViewMS1.SelectedNode.Parent;
    //        if (treeViewMS1.SelectedNode.Parent.Tag is Inventoryattachment)
    //        {
    //            Inventoryattachment IA = treeViewMS1.SelectedNode.Parent.Tag as Inventoryattachment;
    //            IA.Items.Remove(CurrentAIloadouts);
    //            treeViewMS1.SelectedNode.Remove();
    //            CurrentAILoadoutsFile.isDirty = true;
    //        }
    //        else if (treeViewMS1.SelectedNode.Tag is AILoadouts)
    //        {
    //            if (treeViewMS1.SelectedNode.Parent.Text == "InventoryCargo")
    //            {
    //                CurrentAILoadoutsFile.InventoryCargo.Remove(CurrentAIloadouts);
    //                treeViewMS1.SelectedNode.Remove();
    //                CurrentAILoadoutsFile.isDirty = true;
    //            }
    //            if (treeViewMS1.SelectedNode.Parent.Text == "Sets")
    //            {
    //                CurrentAILoadoutsFile.Sets.Remove(CurrentAIloadouts);
    //                treeViewMS1.SelectedNode.Remove();
    //                CurrentAILoadoutsFile.isDirty = true;
    //            }
    //            else if (treeViewMS1.SelectedNode.Parent.Text == "Attachment")
    //            {
    //                AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
    //            }
    //            else if (treeViewMS1.SelectedNode.Parent.Text == "Cargo")
    //            {
    //                AILoadouts IA = treeViewMS1.SelectedNode.Parent.Parent.Tag as AILoadouts;
    //                IA.InventoryCargo.Remove(CurrentAIloadouts);
    //                treeViewMS1.SelectedNode.Remove();
    //                if (IA.InventoryCargo.Count == 0)
    //                    Parent.Remove();
    //                if (CurrentAILoadoutsFile != null)
    //                    CurrentAILoadoutsFile.isDirty = true;
    //                else if (currentAILootDropsFile != null)
    //                    currentAILootDropsFile.isDirty = true;
    //            }
    //        }
    //    }
    //    private void textBox1_TextChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        CurrentAIloadouts.ClassName = treeViewMS1.SelectedNode.Text = textBox1.Text;
    //        if (CurrentAILoadoutsFile != null)
    //            CurrentAILoadoutsFile.isDirty = true;
    //        else if (currentAILootDropsFile != null)
    //            currentAILootDropsFile.isDirty = true;
    //    }
    //    private void darkButton11_Click(object sender, EventArgs e)
    //    {
    //        AddItemfromTypes form = new AddItemfromTypes
    //        {
    //            vanillatypes = vanillatypes,
    //            ModTypes = ModTypes,
    //            currentproject = currentproject,
    //            UseOnlySingleitem = true
    //        };
    //        DialogResult result = form.ShowDialog();
    //        if (result == DialogResult.OK)
    //        {
    //            List<string> addedtypes = form.addedtypes.ToList();
    //            foreach (string l in addedtypes)
    //            {
    //                textBox1.Text = l;
    //            }
    //        }
    //    }
    //    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        foreach (TreeNode tn in treeViewMS1.SelectedNodes)
    //        {
    //            if (tn.Tag is AILoadouts)
    //            {
    //                AILoadouts looptype = tn.Tag as AILoadouts;
    //                looptype.Chance = numericUpDown1.Value;
    //                CurrentAILoadoutsFile.isDirty = true;
    //            }
    //        }
    //    }
    //    private void numericUpDown2_ValueChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        foreach (TreeNode tn in treeViewMS1.SelectedNodes)
    //        {
    //            if (tn.Tag is AILoadouts)
    //            {
    //                AILoadouts looptype = tn.Tag as AILoadouts;
    //                looptype.Quantity.Min = numericUpDown2.Value;
    //                CurrentAILoadoutsFile.isDirty = true;
    //            }
    //        }
    //    }
    //    private void numericUpDown3_ValueChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        foreach (TreeNode tn in treeViewMS1.SelectedNodes)
    //        {
    //            if (tn.Tag is AILoadouts)
    //            {
    //                AILoadouts looptype = tn.Tag as AILoadouts;
    //                looptype.Quantity.Max = numericUpDown3.Value;
    //                CurrentAILoadoutsFile.isDirty = true;
    //            }
    //        }
    //    }
    //    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (listBox1.SelectedItems.Count < 1) return;
    //        Currenthealth = listBox1.SelectedItem as Health;
    //        useraction = false;
    //        groupBox1.Visible = true;
    //        numericUpDown4.Value = Currenthealth.Min;
    //        numericUpDown5.Value = Currenthealth.Max;
    //        textBox2.Text = Currenthealth.Zone;

    //        useraction = true;
    //    }
    //    private void darkButton8_Click(object sender, EventArgs e)
    //    {
    //        Health newhealth = new Health()
    //        {
    //            Min = (decimal)0.7,
    //            Max = (decimal)1.0,
    //            Zone = ""
    //        };
    //        CurrentAIloadouts.Health.Add(newhealth);
    //        CurrentAILoadoutsFile.isDirty = true;
    //        groupBox1.Visible = true;
    //        listBox1.SelectedIndex = -1;
    //        listBox1.SelectedIndex = CurrentAIloadouts.Health.Count - 1;
    //    }
    //    private void darkButton9_Click(object sender, EventArgs e)
    //    {
    //        int index = listBox1.SelectedIndex;
    //        CurrentAIloadouts.Health.Remove(Currenthealth);
    //        CurrentAILoadoutsFile.isDirty = true;
    //        listBox1.SelectedIndex = -1;
    //        if (CurrentAIloadouts.Health.Count > 0)
    //            listBox1.SelectedIndex = index - 1;
    //        else
    //            groupBox1.Visible = false;
    //    }
    //    private void numericUpDown4_ValueChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        Currenthealth.Min = numericUpDown4.Value;
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void numericUpDown5_ValueChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        Currenthealth.Max = numericUpDown5.Value;
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void textBox2_TextChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        Currenthealth.Zone = textBox2.Text;
    //        CurrentAILoadoutsFile.isDirty = true;
    //        listBox1.Invalidate();
    //    }
    //    private void ItemAttachmentSlotNameCB_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (!useraction) return;
    //        string Slot = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
    //        if (Slot == "Default Slot") Slot = "";
    //        CurrentInventoryattachment.SlotName = Slot;
    //        treeViewMS1.SelectedNode.Text = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
    //        CurrentAILoadoutsFile.isDirty = true;
    //    }
    //    private void toolStripButton2_Click(object sender, EventArgs e)
    //    {
    //        Process.Start(AILoadoutsPath);
    //    }
    //    private void SaveFileButton_Click(object sender, EventArgs e)
    //    {
    //        savefiles();
    //    }
    //    public void savefiles(bool updated = false)
    //    {
    //        List<string> midifiedfiles = new List<string>();
    //        string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
    //        foreach (AILoadouts AILO in AILoadoutsConfig.LoadoutsData)
    //        {
    //            if (AILO.isDirty)
    //            {
    //                if (currentproject.Createbackups && File.Exists(AILO.Filename))
    //                {
    //                    Directory.CreateDirectory(Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime);
    //                    File.Copy(AILO.Filename, Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak", true);
    //                }
    //                AILO.isDirty = false;
    //                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    //                string jsonString = JsonSerializer.Serialize(AILO, options);
    //                File.WriteAllText(AILO.Filename, jsonString);
    //                midifiedfiles.Add(Path.GetFileName(AILO.Filename));
    //            }
    //        }
    //        foreach (AILootDrops AILO in AILoadoutsConfig.AILootDropsData)
    //        {
    //            if (AILO.isDirty)
    //            {
    //                if (currentproject.Createbackups && File.Exists(AILO.Filename))
    //                {
    //                    Directory.CreateDirectory(Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime);
    //                    File.Copy(AILO.Filename, Path.GetDirectoryName(AILO.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak", true);
    //                }
    //                AILO.isDirty = false;
    //                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    //                string jsonString = JsonSerializer.Serialize(AILO.LootdropList, options);
    //                File.WriteAllText(AILO.Filename, jsonString);
    //                midifiedfiles.Add(Path.GetFileName(AILO.Filename));
    //            }
    //        }

    //        string message = "The Following Files were saved....\n";
    //        if (updated)
    //        {
    //            message = "The following files were either Created or Updated...\n";
    //        }
    //        int i = 0;
    //        foreach (string l in midifiedfiles)
    //        {
    //            if (i == 5)
    //            {
    //                message += l + "\n";
    //                i = 0;
    //            }
    //            else
    //            {
    //                message += l + ", ";
    //                i++;
    //            }

    //        }
    //        if (midifiedfiles.Count > 0)
    //            MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //        else
    //            MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    //    }
    //    private void ExpansionLoadoutsManager_FormClosing(object sender, FormClosingEventArgs e)
    //    {
    //        bool needtosave = false;
    //        foreach (AILoadouts AILO in AILoadoutsConfig.LoadoutsData)
    //        {
    //            if (AILO.isDirty)
    //            {
    //                needtosave = true;
    //            }
    //        }
    //        if (needtosave)
    //        {
    //            DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
    //            if (dialogResult == DialogResult.Yes)
    //            {
    //                savefiles();
    //            }
    //        }
    //    }
    //    private void darkLabel1_Click(object sender, EventArgs e)
    //    {
    //        treeViewMS1.SelectedNode.ExpandAll();
    //        treeViewMS1.Focus();
    //    }
    //    private void darkLabel2_Click(object sender, EventArgs e)
    //    {
    //        treeViewMS1.SelectedNode.Collapse();
    //    }
    //    private void addNewLoadoutFileToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
    //        AILoadouts newAILoadouts = new AILoadouts()
    //        {
    //            Name = UserAnswer,
    //            Filename = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts\\" + UserAnswer + ".json",
    //            ClassName = "",
    //            Chance = (decimal)1.0,
    //            Quantity = new Quantity(),
    //            Health = new BindingList<Health>(),
    //            InventoryAttachments = new BindingList<Inventoryattachment>(),
    //            InventoryCargo = new BindingList<AILoadouts>(),
    //            ConstructionPartsBuilt = new BindingList<object>(),
    //            Sets = new BindingList<AILoadouts>(),
    //            isDirty = true
    //        };
    //        AILoadoutsConfig.LoadoutsData.Add(newAILoadouts);
    //        CurrentTreenode.Nodes.Add(new TreeNode(newAILoadouts.Name)
    //        {
    //            Tag = newAILoadouts
    //        });
    //    }
    //    private void removeLoadoutFileToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
    //        {
    //            File.Delete(CurrentAILoadoutsFile.Filename);
    //            AILoadoutsConfig.LoadoutsData.Remove(CurrentAILoadoutsFile);
    //            CurrentTreenode.Parent.Nodes.Remove(CurrentTreenode);
    //        }
    //    }
    //    private void addNewLootDropFileToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
    //        AILootDrops newAILootDrops = new AILootDrops()
    //        {
    //            Name = UserAnswer,
    //            Filename = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\AI\\LootDrops\\" + UserAnswer + ".json",
    //            LootdropList = new BindingList<AILoadouts>(),
    //            isDirty = true
    //        };
    //        AILoadoutsConfig.AILootDropsData.Add(newAILootDrops);
    //        CurrentTreenode.Nodes.Add(new TreeNode(newAILootDrops.Name)
    //        {
    //            Tag = newAILootDrops
    //        });
    //    }
    //    private void RemoveLootDropFileToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //        if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
    //        {
    //            File.Delete(currentAILootDropsFile.Filename);
    //            AILoadoutsConfig.AILootDropsData.Remove(currentAILootDropsFile);
    //            CurrentTreenode.Parent.Nodes.Remove(CurrentTreenode);
    //        }
    //    }
    //}

    public partial class ExpansionLoadoutsManager : DarkForm
    {
        #region Tag constants
        private const string TAG_LOADOUTS_ROOT = "LoadoutsRoot";
        private const string TAG_LOOTDROPS_ROOT = "LootDropsRoot";
        private const string TAG_INVENTORY_ATTACHMENTS = "InventoryAttachments";
        private const string TAG_INVENTORY_CARGO = "InventoryCargo";
        private const string TAG_SETS = "Sets";
        private const string TAG_CARGO = "Cargo";
        #endregion

        public Project currentproject { get; internal set; }
        public AILoadoutsConfig AILoadoutsConfig { get; private set; }

        public string AILoadoutsPath;
        public string LootDropOnDeathListPath;

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;

        public AILoadouts CurrentAILoadoutsFile { get; private set; }
        public AILootDrops currentAILootDropsFile { get; set; }
        public Inventoryattachment CurrentInventoryattachment { get; private set; }
        public AILoadouts CurrentAIloadouts { get; private set; }
        public Health Currenthealth { get; private set; }

        public TreeNode CurrentTreenode { get; set; }
        public object CurrentTreeNodeTag;

        private bool useraction;

        public ExpansionLoadoutsManager()
        {
            InitializeComponent();
        }

        #region UI helpers
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

        private void SafeSetUserAction(bool value)
        {
            useraction = value;
        }

        private void SafeFileDelete(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SafeFileDelete error: " + ex.Message);
                MessageBox.Show("Failed to delete file: " + path + "\n\n" + ex.Message, "File Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LogDeserializeError(string path, Exception ex)
        {
            Debug.WriteLine("Failed to parse JSON: " + path + " -> " + ex.Message);
        }

        private bool EnsureDirectoryExists(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return false;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EnsureDirectoryExists error: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Load / Setup
        private void ExpansionLoadoutsManager_Load(object sender, EventArgs e)
        {
            // Defensive guards
            if (currentproject == null)
            {
                MessageBox.Show("currentproject is null. Cannot initialize.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            AILoadoutsConfig = new AILoadoutsConfig();
            AILoadoutsConfig.LoadoutsData = new BindingList<AILoadouts>();

            AILoadoutsPath = Path.Combine(currentproject.projectFullName, currentproject.ProfilePath, "ExpansionMod", "Loadouts");
            LoadAILoadoutsFiles();

            AILoadoutsConfig.AILootDropsData = new BindingList<AILootDrops>();
            LootDropOnDeathListPath = Path.Combine(currentproject.projectFullName, currentproject.ProfilePath, "ExpansionMod", "AI", "LootDrops");
            LoadAILootDropsFiles();

            SetupAILoadouts();
        }

        private void LoadAILoadoutsFiles()
        {
            try
            {
                if (!Directory.Exists(AILoadoutsPath))
                    return;

                DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
                FileInfo[] files = dinfo.GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        Debug.WriteLine("Deserializing " + file.FullName);
                        string txt = File.ReadAllText(file.FullName);
                        AILoadouts loaded = JsonSerializer.Deserialize<AILoadouts>(txt);
                        if (loaded == null)
                        {
                            Debug.WriteLine("Deserialized null for " + file.FullName);
                            continue;
                        }

                        loaded.Filename = file.FullName;
                        loaded.Setname();
                        loaded.isDirty = false;
                        AILoadoutsConfig.LoadoutsData.Add(loaded);
                    }
                    catch (Exception ex)
                    {
                        LogDeserializeError(file.FullName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadAILoadoutsFiles error: " + ex.Message);
            }
        }

        private void LoadAILootDropsFiles()
        {
            try
            {
                if (!Directory.Exists(LootDropOnDeathListPath))
                    return;

                DirectoryInfo dinfo = new DirectoryInfo(LootDropOnDeathListPath);
                FileInfo[] files = dinfo.GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        Debug.WriteLine("Deserializing loot drops: " + file.FullName);
                        string txt = File.ReadAllText(file.FullName);

                        BindingList<AILoadouts> lootlist = JsonSerializer.Deserialize<BindingList<AILoadouts>>(txt) ?? new BindingList<AILoadouts>();

                        AILootDrops drops = new AILootDrops
                        {
                            LootdropList = lootlist,
                            Filename = file.FullName,
                            isDirty = false
                        };
                        drops.Setname();
                        AILoadoutsConfig.AILootDropsData.Add(drops);
                    }
                    catch (Exception ex)
                    {
                        LogDeserializeError(file.FullName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadAILootDropsFiles error: " + ex.Message);
            }
        }

        private void SetupAILoadouts()
        {
            SafeSetUserAction(false);

            treeViewMS1.Nodes.Clear();

            TreeNode loadoutsRoot = new TreeNode("Loadouts") { Tag = TAG_LOADOUTS_ROOT };
            foreach (AILoadouts load in AILoadoutsConfig.LoadoutsData)
            {
                loadoutsRoot.Nodes.Add(SetupLoadoutTreeView(load));
            }
            treeViewMS1.Nodes.Add(loadoutsRoot);

            TreeNode lootDropsRoot = new TreeNode("LootDrops") { Tag = TAG_LOOTDROPS_ROOT };
            foreach (AILootDrops loot in AILoadoutsConfig.AILootDropsData)
            {
                TreeNode fileNode = new TreeNode(loot.Name) { Tag = loot };
                foreach (AILoadouts entry in loot.LootdropList)
                {
                    fileNode.Nodes.Add(BuildAILoadoutsNode(entry));
                }
                lootDropsRoot.Nodes.Add(fileNode);
            }
            treeViewMS1.Nodes.Add(lootDropsRoot);

            SafeSetUserAction(true);
        }
        #endregion

        #region TreeNode Builders
        private TreeNode SetupLoadoutTreeView(AILoadouts load)
        {
            string display = !string.IsNullOrWhiteSpace(load.Name) ? load.Name : Path.GetFileNameWithoutExtension(load.Filename ?? string.Empty);
            TreeNode root = new TreeNode(display) { Tag = load };

            TreeNode invAttachments = new TreeNode("inventoryAttachments") { Tag = TAG_INVENTORY_ATTACHMENTS };
            foreach (Inventoryattachment ia in load.InventoryAttachments ?? new BindingList<Inventoryattachment>())
                invAttachments.Nodes.Add(BuildInventoryAttachmentNode(ia));
            root.Nodes.Add(invAttachments);

            TreeNode invCargo = new TreeNode("InventoryCargo") { Tag = TAG_INVENTORY_CARGO };
            foreach (AILoadouts cargo in load.InventoryCargo ?? new BindingList<AILoadouts>())
                invCargo.Nodes.Add(BuildAILoadoutsNode(cargo));
            root.Nodes.Add(invCargo);

            TreeNode sets = new TreeNode("Sets") { Tag = TAG_SETS };
            foreach (AILoadouts set in load.Sets ?? new BindingList<AILoadouts>())
                sets.Nodes.Add(BuildAILoadoutsNode(set));
            root.Nodes.Add(sets);

            return root;
        }

        private TreeNode BuildInventoryAttachmentNode(Inventoryattachment ia)
        {
            string slotname = string.IsNullOrWhiteSpace(ia.SlotName) ? "Default Slot" : ia.SlotName;
            TreeNode tn = new TreeNode(slotname) { Tag = ia };
            foreach (AILoadouts child in ia.Items ?? new BindingList<AILoadouts>())
                tn.Nodes.Add(BuildAILoadoutsNode(child));
            return tn;
        }

        private TreeNode BuildAILoadoutsNode(AILoadouts a)
        {
            string label = string.IsNullOrWhiteSpace(a.ClassName) ? "Set" : a.ClassName;
            TreeNode tn = new TreeNode(label) { Tag = a };

            foreach (Inventoryattachment ia in a.InventoryAttachments ?? new BindingList<Inventoryattachment>())
                tn.Nodes.Add(BuildInventoryAttachmentNode(ia));

            TreeNode cargoNode = BuildCargoNode(a.InventoryCargo);
            if (cargoNode != null) tn.Nodes.Add(cargoNode);

            foreach (AILoadouts set in a.Sets ?? new BindingList<AILoadouts>())
                tn.Nodes.Add(BuildAILoadoutsNode(set));

            return tn;
        }

        private TreeNode BuildCargoNode(BindingList<AILoadouts> cargoList)
        {
            if (cargoList == null || cargoList.Count == 0) return null;
            TreeNode tn = new TreeNode("Cargo") { Tag = TAG_CARGO };
            foreach (AILoadouts c in cargoList)
                tn.Nodes.Add(BuildAILoadoutsNode(c));
            return tn;
        }
        #endregion

        #region Selection handling (centralized)
        private void treeViewMS1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            SafeSetUserAction(false);

            CurrentTreenode = e.Node;
            CurrentTreeNodeTag = e.Node.Tag;

            // reset visibility
            LoadOutGB.Visible = false;
            InventoryattchemntGB.Visible = false;
            groupBox1.Visible = false;

            // determine current file context (topmost loot drops or loadouts file)
            currentAILootDropsFile = e.Node.FindParentOfType<AILootDrops>();
            if (currentAILootDropsFile == null)
                CurrentAILoadoutsFile = e.Node.FindLastParentOfType<AILoadouts>();
            else
                CurrentAILoadoutsFile = null;

            // ---- Node-specific UI logic ----

            // AILootDrops node
            if (e.Node.Tag is AILootDrops)
            {
                // (show loot-drop-level UI if needed)
                SafeSetUserAction(true);
                return;
            }

            // Inventoryattachment node (has parent)
            if (e.Node.Parent != null && e.Node.Tag is Inventoryattachment iaTag)
            {
                InventoryattchemntGB.Visible = true;
                CurrentAIloadouts = e.Node.Parent.Tag as AILoadouts;
                CurrentInventoryattachment = iaTag;

                string slotname = string.IsNullOrWhiteSpace(CurrentInventoryattachment.SlotName)
                    ? "Default Slot"
                    : CurrentInventoryattachment.SlotName;
                ItemAttachmentSlotNameCB.SelectedIndex = ItemAttachmentSlotNameCB.FindStringExact(slotname);

                SafeSetUserAction(true);
                return;
            }

            // AILoadouts node (item)
            if (e.Node.Parent != null && e.Node.Tag is AILoadouts aiTag)
            {
                LoadOutGB.Visible = true;
                CurrentAIloadouts = aiTag;

                textBox1.Text = CurrentAIloadouts.ClassName;
                numericUpDown1.Value = CurrentAIloadouts.Chance;
                numericUpDown2.Value = CurrentAIloadouts.Quantity?.Min ?? numericUpDown2.Minimum;
                numericUpDown3.Value = CurrentAIloadouts.Quantity?.Max ?? numericUpDown3.Minimum;

                listBox1.DisplayMember = "DisplayName";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = CurrentAIloadouts.Health;
                groupBox1.Visible = (CurrentAIloadouts.Health != null && CurrentAIloadouts.Health.Count > 0);

                SafeSetUserAction(true);
                return;
            }

            SafeSetUserAction(true);
        }

        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (treeViewMS1.SelectedNode != e.Node)
                treeViewMS1.SelectedNode = e.Node;

            foreach (ToolStripMenuItem item in contextMenuStrip1.Items)
                item.Visible = false;

            // category/tag nodes
            if (e.Node.Tag is string tagStr)
            {
                switch (tagStr)
                {
                    case TAG_LOADOUTS_ROOT:
                        addNewLoadoutFileToolStripMenuItem.Visible = true;
                        break;
                    case TAG_LOOTDROPS_ROOT:
                        addNewLootDropFileToolStripMenuItem.Visible = true;
                        break;
                    case TAG_INVENTORY_ATTACHMENTS:
                        AddNewAttachmentItemToolStripMenuItem.Visible = true;
                        break;
                    case TAG_INVENTORY_CARGO:
                        AddNewCargoItemToolStripMenuItem.Visible = true;
                        break;
                    case TAG_SETS:
                        AddNewSetItemToolStripMenuItem.Visible = true;
                        break;
                    case TAG_CARGO:
                        RemoveCargoItemToolStripMenuItem.Visible = true;
                        break;
                }

                contextMenuStrip1.Show(Cursor.Position);
                return;
            }

            // AILootDrops node
            if (e.Node.Tag is AILootDrops)
            {
                addNewItemToolStripMenuItem.Visible = true;
                RemoveLootDropFileToolStripMenuItem.Visible = true;
                contextMenuStrip1.Show(Cursor.Position);
                return;
            }

            // Inventoryattachment node (has parent)
            if (e.Node.Parent != null && e.Node.Tag is Inventoryattachment)
            {
                addNewItemToolStripMenuItem.Visible = true;
                RemoveAttachemtItemToolStripMenuItem.Visible = true;
                contextMenuStrip1.Show(Cursor.Position);
                return;
            }

            // AILoadouts node (item)
            if (e.Node.Parent != null && e.Node.Parent.Tag.ToString() != "LoadoutsRoot" && e.Node.Tag is AILoadouts)
            {
                removeItemToolStripMenuItem.Visible = true;
                AddNewAttachmentItemToolStripMenuItem.Visible = true;
                AddNewCargoItemToolStripMenuItem.Visible = true;
                contextMenuStrip1.Show(Cursor.Position);
                return;
            }
        }
        #endregion

        #region Context menu / Actions
        private void AddNewAttachmentItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventoryattachment newIA = new Inventoryattachment()
            {
                SlotName = "Back",
                Items = new BindingList<AILoadouts>()
            };

            TreeNode newnode = new TreeNode(newIA.SlotName) { Tag = newIA };

            // If selected node is the "inventoryAttachments" category under a file
            if (treeViewMS1.SelectedNode != null && treeViewMS1.SelectedNode.Tag is string tag && tag == TAG_INVENTORY_ATTACHMENTS && CurrentAILoadoutsFile != null)
            {
                CurrentAILoadoutsFile.InventoryAttachments.Add(newIA);
                treeViewMS1.SelectedNode.Nodes.Add(newnode);
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if (CurrentAIloadouts != null)
            {
                CurrentAIloadouts.InventoryAttachments.Add(newIA);
                treeViewMS1.SelectedNode?.Nodes.Add(newnode);

                // If parent is inside a loot drops file, mark that file dirty
                if (currentAILootDropsFile != null)
                    currentAILootDropsFile.isDirty = true;
                else if (CurrentAILoadoutsFile != null)
                    CurrentAILoadoutsFile.isDirty = true;
            }
        }

        private void RemoveAttachemtItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentInventoryattachment == null) return;

            // If the inventory attachment belongs to an AILoadouts file
            if (CurrentAILoadoutsFile != null)
            {
                CurrentAILoadoutsFile.InventoryAttachments.Remove(CurrentInventoryattachment);
                if (treeViewMS1.SelectedNode != null)
                    treeViewMS1.SelectedNode.Remove();
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if (currentAILootDropsFile != null)
            {
                // If this inventory attachment is inside an AILoadouts that belongs to a loot drops file,
                // find that specific AILoadouts parent and update it and the loot drops file's dirty flag.
                TreeNode selected = treeViewMS1.SelectedNode;
                TreeNode parent = selected?.Parent;
                if (parent != null && parent.Tag is AILoadouts parentLoadout)
                {
                    parentLoadout.InventoryAttachments.Remove(CurrentInventoryattachment);
                    selected.Remove();
                    currentAILootDropsFile.isDirty = true;
                }
            }
        }

        private void AddNewCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts newItem = new AILoadouts()
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
            TreeNode newNode = new TreeNode(newItem.ClassName) { Tag = newItem };

            TreeNode sel = treeViewMS1.SelectedNode;
            if (sel == null) return;

            // If user clicked on an InventoryCargo category node
            if (sel.Tag is string tag && tag == TAG_INVENTORY_CARGO && CurrentAILoadoutsFile != null)
            {
                CurrentAILoadoutsFile.InventoryCargo.Add(newItem);
                sel.Nodes.Add(newNode);
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if (sel.Tag is AILoadouts selectedAILoadouts)
            {
                // find/create Cargo child
                TreeNode cargoNode = sel.Nodes.Cast<TreeNode>().FirstOrDefault(n => string.Equals(n.Tag as string, TAG_CARGO));
                if (cargoNode == null)
                {
                    cargoNode = new TreeNode("Cargo") { Tag = TAG_CARGO };
                    sel.Nodes.Add(cargoNode);
                }
                cargoNode.Nodes.Add(newNode);
                selectedAILoadouts.InventoryCargo.Add(newItem);

                // If this AILoadouts is inside a loot drops file, mark the loot drops file dirty
                AILootDrops owningLootFile = sel.FindParentOfType<AILootDrops>();
                if (owningLootFile != null)
                {
                    owningLootFile.isDirty = true;
                }
                else if (CurrentAILoadoutsFile != null)
                {
                    CurrentAILoadoutsFile.isDirty = true;
                }
            }
        }

        private void RemoveCargoItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode sel = treeViewMS1.SelectedNode;
            if (sel == null || sel.Parent == null) return;

            // If the parent node's Tag is AILoadouts, we expect to clear its InventoryCargo
            AILoadouts parentLoadout = sel.Parent.Tag as AILoadouts;
            if (parentLoadout != null)
            {
                parentLoadout.InventoryCargo = new BindingList<AILoadouts>();
                sel.Remove();
                // mark correct file dirty
                AILootDrops owningLootFile = sel.FindParentOfType<AILootDrops>();
                if (owningLootFile != null)
                    owningLootFile.isDirty = true;
                else if (CurrentAILoadoutsFile != null)
                    CurrentAILoadoutsFile.isDirty = true;
            }
        }

        private void AddNewSetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts newSet = new AILoadouts()
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

            if (CurrentAILoadoutsFile != null)
            {
                CurrentAILoadoutsFile.Sets.Add(newSet);
                TreeNode newNode = new TreeNode("Set") { Tag = newSet };
                treeViewMS1.SelectedNode.Nodes.Add(newNode);
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if (currentAILootDropsFile != null && CurrentAIloadouts != null)
            {
                // If the selected node is a Sets container inside an AILoadouts that belongs to a loot drops file:
                CurrentAIloadouts.Sets.Add(newSet);
                treeViewMS1.SelectedNode?.Nodes.Add(new TreeNode("Set") { Tag = newSet });
                currentAILootDropsFile.isDirty = true;
            }
        }

        private void RemoveSetItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode sel = treeViewMS1.SelectedNode;
            if (sel == null || sel.Parent == null) return;

            TreeNode parent = sel.Parent;
            // If parent is the "Sets" container directly under a file root
            if (parent.Tag is string tag && tag == TAG_SETS)
            {
                // parent.Parent is the AILoadouts it belongs to
                AILoadouts fileRoot = parent.Parent?.Tag as AILoadouts;
                if (fileRoot != null)
                {
                    fileRoot.Sets.Remove(sel.Tag as AILoadouts);
                    sel.Remove();
                    // mark owning file dirty (could be either loadouts file or loot drops file)
                    AILootDrops ld = parent.FindParentOfType<AILootDrops>();
                    if (ld != null) ld.isDirty = true;
                    else fileRoot.isDirty = true;
                }
            }
            else if (parent.Tag is AILoadouts parentLoadout)
            {
                parentLoadout.Sets.Remove(sel.Tag as AILoadouts);
                sel.Remove();
                AILootDrops ld = sel.FindParentOfType<AILootDrops>();
                if (ld != null) ld.isDirty = true;
                else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
            }
        }

        private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AILoadouts newItem = new AILoadouts()
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
            newItem.Health.Add(new Health() { Zone = "", Min = (decimal)0.7, Max = (decimal)1.0 });

            TreeNode newNode = new TreeNode(newItem.ClassName) { Tag = newItem };
            treeViewMS1.SelectedNode?.Nodes.Add(newNode);

            // If adding under AILootDrops file node
            if (CurrentTreenode != null && CurrentTreenode.Tag is AILootDrops drops)
            {
                drops.LootdropList.Add(newItem);
                drops.isDirty = true;
            }
            else if (CurrentAILoadoutsFile != null)
            {
                // If adding under a loadouts file
                if (CurrentTreenode.Tag is Inventoryattachment Inventoryattachment)
                    Inventoryattachment.Items.Add(newItem);
                CurrentAILoadoutsFile.isDirty = true;
            }
            else if(currentAILootDropsFile != null)
            {
                if (CurrentTreenode.Tag is Inventoryattachment Inventoryattachment)
                    Inventoryattachment.Items.Add(newItem);
                
                currentAILootDropsFile.isDirty = true;
            }
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selected = treeViewMS1.SelectedNode;
            if (selected == null || selected.Parent == null) return;

            TreeNode parent = selected.Parent;

            // --- 1) Inventoryattachment parent ---
            if (parent.Tag is Inventoryattachment ia)
            {
                AILoadouts item = selected.Tag as AILoadouts;
                AILootDrops owningLootFile = selected.FindParentOfType<AILootDrops>(); // capture before removal

                ia.Items.Remove(item);
                selected.Remove();

                if (owningLootFile != null)
                    owningLootFile.isDirty = true;
                else if (CurrentAILoadoutsFile != null)
                    CurrentAILoadoutsFile.isDirty = true;

                return;
            }

            // --- 2) AILootDrops file parent (top-level loadout under LootDrops file) ---
            if (parent.Tag is AILootDrops parentLootFile)
            {
                AILoadouts toRemove = selected.Tag as AILoadouts;
                if (toRemove != null)
                {
                    parentLootFile.LootdropList.Remove(toRemove);
                    selected.Remove();
                    parentLootFile.isDirty = true;
                }
                return;
            }

            // --- 3) All other cases where selected.Tag is AILoadouts ---
            if (selected.Tag is AILoadouts selectedLoadout)
            {
                AILootDrops owningLootFile = selected.FindParentOfType<AILootDrops>(); // capture before removal

                // (a) InventoryCargo category
                if (string.Equals(parent.Text, "InventoryCargo", StringComparison.OrdinalIgnoreCase))
                {
                    AILoadouts fileRoot = parent.Parent?.Tag as AILoadouts;
                    if (fileRoot != null)
                    {
                        fileRoot.InventoryCargo.Remove(selectedLoadout);
                        selected.Remove();

                        if (owningLootFile != null)
                            owningLootFile.isDirty = true;
                        else if (CurrentAILoadoutsFile != null)
                            CurrentAILoadoutsFile.isDirty = true;
                    }
                }
                // (b) Sets category
                else if (string.Equals(parent.Text, "Sets", StringComparison.OrdinalIgnoreCase))
                {
                    AILoadouts fileRoot = parent.Parent?.Tag as AILoadouts;
                    if (fileRoot != null)
                    {
                        fileRoot.Sets.Remove(selectedLoadout);
                        selected.Remove();

                        if (owningLootFile != null)
                            owningLootFile.isDirty = true;
                        else if (CurrentAILoadoutsFile != null)
                            CurrentAILoadoutsFile.isDirty = true;
                    }
                }
                // (c) Cargo category (nested under another AILoadouts)
                else if (string.Equals(parent.Text, "Cargo", StringComparison.OrdinalIgnoreCase))
                {
                    AILoadouts parentLoadout = parent.Parent?.Tag as AILoadouts;
                    if (parentLoadout != null)
                    {
                        parentLoadout.InventoryCargo.Remove(selectedLoadout);

                        // Remove tree node after all data updates
                        selected.Remove();

                        // If cargo is now empty, remove the "Cargo" container node
                        if (parentLoadout.InventoryCargo.Count == 0)
                            parent.Remove();

                        if (owningLootFile != null)
                            owningLootFile.isDirty = true;
                        else if (CurrentAILoadoutsFile != null)
                            CurrentAILoadoutsFile.isDirty = true;
                    }
                }
            }
        }
        #endregion

        #region UI event bindings (controls)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AILoadouts ai = treeViewMS1.SelectedNode?.Tag as AILoadouts;
            if (ai == null) return;
            ai.ClassName = treeViewMS1.SelectedNode.Text = textBox1.Text;

            // mark correct owning file dirty
            AILootDrops ld = treeViewMS1.SelectedNode.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
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
                    textBox1.Text = l; // keep original behaviour
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                if (tn.Tag is AILoadouts)
                {
                    AILoadouts looptype = tn.Tag as AILoadouts;
                    looptype.Chance = numericUpDown1.Value;
                    AILootDrops ld = tn.FindParentOfType<AILootDrops>();
                    if (ld != null) ld.isDirty = true;
                    else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
                }
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                if (tn.Tag is AILoadouts)
                {
                    AILoadouts looptype = tn.Tag as AILoadouts;
                    if (looptype.Quantity == null) looptype.Quantity = new Quantity();
                    looptype.Quantity.Min = numericUpDown2.Value;
                    AILootDrops ld = tn.FindParentOfType<AILootDrops>();
                    if (ld != null) ld.isDirty = true;
                    else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
                }
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                if (tn.Tag is AILoadouts)
                {
                    AILoadouts looptype = tn.Tag as AILoadouts;
                    if (looptype.Quantity == null) looptype.Quantity = new Quantity();
                    looptype.Quantity.Max = numericUpDown3.Value;
                    AILootDrops ld = tn.FindParentOfType<AILootDrops>();
                    if (ld != null) ld.isDirty = true;
                    else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1) return;
            Currenthealth = listBox1.SelectedItem as Health;
            SafeSetUserAction(false);
            groupBox1.Visible = true;
            numericUpDown4.Value = Currenthealth.Min;
            numericUpDown5.Value = Currenthealth.Max;
            textBox2.Text = Currenthealth.Zone;
            SafeSetUserAction(true);
        }

        private void darkButton8_Click(object sender, EventArgs e)
        {
            if (CurrentAIloadouts == null) return;
            Health newhealth = new Health()
            {
                Min = (decimal)0.7,
                Max = (decimal)1.0,
                Zone = ""
            };
            CurrentAIloadouts.Health.Add(newhealth);

            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;

            groupBox1.Visible = true;
            listBox1.SelectedIndex = CurrentAIloadouts.Health.Count - 1;
        }

        private void darkButton9_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (CurrentAIloadouts == null || Currenthealth == null) return;
            CurrentAIloadouts.Health.Remove(Currenthealth);
            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;

            if (CurrentAIloadouts.Health.Count > 0)
                listBox1.SelectedIndex = Math.Max(0, index - 1);
            else
                groupBox1.Visible = false;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction || Currenthealth == null) return;
            Currenthealth.Min = numericUpDown4.Value;
            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction || Currenthealth == null) return;
            Currenthealth.Max = numericUpDown5.Value;
            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!useraction || Currenthealth == null) return;
            Currenthealth.Zone = textBox2.Text;
            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
            listBox1.Invalidate();
        }

        private void ItemAttachmentSlotNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction || CurrentInventoryattachment == null) return;
            string Slot = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            if (Slot == "Default Slot") Slot = "";
            CurrentInventoryattachment.SlotName = Slot;
            if (treeViewMS1.SelectedNode != null)
                treeViewMS1.SelectedNode.Text = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
            AILootDrops ld = treeViewMS1.SelectedNode?.FindParentOfType<AILootDrops>();
            if (ld != null) ld.isDirty = true;
            else if (CurrentAILoadoutsFile != null) CurrentAILoadoutsFile.isDirty = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (currentAILootDropsFile != null)
            {
                if (!string.IsNullOrWhiteSpace(LootDropOnDeathListPath) && Directory.Exists(LootDropOnDeathListPath))
                    Process.Start(LootDropOnDeathListPath);
            }
            else if (CurrentAILoadoutsFile != null)
            {
                if (!string.IsNullOrWhiteSpace(AILoadoutsPath) && Directory.Exists(AILoadoutsPath))
                    Process.Start(AILoadoutsPath);
            }
            else
                MessageBox.Show("Loadouts path not found: " + AILoadoutsPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Save / close
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }

        public void savefiles(bool updated = false)
        {
            List<string> modifiedFiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");

            // Save AILoadouts files
            foreach (AILoadouts AILO in AILoadoutsConfig.LoadoutsData)
            {
                if (AILO == null) continue;
                if (!AILO.isDirty) continue;

                try
                {
                    if (currentproject.Createbackups && File.Exists(AILO.Filename))
                    {
                        string backupDir = Path.Combine(Path.GetDirectoryName(AILO.Filename) ?? string.Empty, "Backup", SaveTime);
                        EnsureDirectoryExists(backupDir);
                        File.Copy(AILO.Filename, Path.Combine(backupDir, Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak"), true);
                    }

                    AILO.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(AILO, options);
                    File.WriteAllText(AILO.Filename, jsonString);
                    modifiedFiles.Add(Path.GetFileName(AILO.Filename));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Save AILoadouts error: " + ex.Message);
                }
            }

            // Save AILootDrops files
            foreach (AILootDrops AILO in AILoadoutsConfig.AILootDropsData)
            {
                if (AILO == null) continue;
                if (!AILO.isDirty) continue;

                try
                {
                    if (currentproject.Createbackups && File.Exists(AILO.Filename))
                    {
                        string backupDir = Path.Combine(Path.GetDirectoryName(AILO.Filename) ?? string.Empty, "Backup", SaveTime);
                        EnsureDirectoryExists(backupDir);
                        File.Copy(AILO.Filename, Path.Combine(backupDir, Path.GetFileNameWithoutExtension(AILO.Filename) + ".bak"), true);
                    }

                    AILO.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(AILO.LootdropList, options);
                    File.WriteAllText(AILO.Filename, jsonString);
                    modifiedFiles.Add(Path.GetFileName(AILO.Filename));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Save AILootDrops error: " + ex.Message);
                }
            }

            string message;
            if (updated)
                message = "The following files were either Created or Updated...\n";
            else
                message = "The Following Files were saved....\n";

            if (modifiedFiles.Count > 0)
            {
                message += string.Join(", ", modifiedFiles);
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ExpansionLoadoutsManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            foreach (AILoadouts AILO in AILoadoutsConfig.LoadoutsData)
            {
                if (AILO.isDirty)
                {
                    needtosave = true;
                    break;
                }
            }
            foreach (AILootDrops ALD in AILoadoutsConfig.AILootDropsData)
            {
                if (ALD.isDirty)
                {
                    needtosave = true;
                    break;
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
        #endregion

        #region Misc helpers (expand/collapse, add/remove files)
        private void darkLabel1_Click(object sender, EventArgs e)
        {
            if (treeViewMS1.SelectedNode != null)
                treeViewMS1.SelectedNode.ExpandAll();
            treeViewMS1.Focus();
        }

        private void darkLabel2_Click(object sender, EventArgs e)
        {
            if (treeViewMS1.SelectedNode != null)
                treeViewMS1.SelectedNode.Collapse();
        }

        private void addNewLoadoutFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
            if (string.IsNullOrWhiteSpace(UserAnswer)) return;

            string filename = Path.Combine(currentproject.projectFullName, currentproject.ProfilePath, "ExpansionMod", "Loadouts", UserAnswer + ".json");
            AILoadouts newAILoadouts = new AILoadouts()
            {
                // default constructor already initializes lists
                ClassName = "",
                Chance = (decimal)1.0,
                Quantity = new Quantity(),
                isDirty = true
            };
            newAILoadouts.Filename = filename;
            newAILoadouts.Name = UserAnswer;

            AILoadoutsConfig.LoadoutsData.Add(newAILoadouts);

            // Add to UI under Loadouts root
            TreeNode root = treeViewMS1.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Tag is string && (string)n.Tag == TAG_LOADOUTS_ROOT);
            if (root != null)
            {
                root.Nodes.Add(SetupLoadoutTreeView(newAILoadouts));
            }
        }

        private void removeLoadoutFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentAILoadoutsFile == null) return;
            if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SafeFileDelete(CurrentAILoadoutsFile.Filename);
                AILoadoutsConfig.LoadoutsData.Remove(CurrentAILoadoutsFile);
                // remove node from tree
                if (CurrentTreenode != null && CurrentTreenode.Parent != null)
                    CurrentTreenode.Parent.Nodes.Remove(CurrentTreenode);
                CurrentAILoadoutsFile = null;
            }
        }

        private void addNewLootDropFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your New Loadout Name ", "Loadout Name", "");
            if (string.IsNullOrWhiteSpace(UserAnswer)) return;

            string filename = Path.Combine(currentproject.projectFullName, currentproject.ProfilePath, "ExpansionMod", "AI", "LootDrops", UserAnswer + ".json");
            AILootDrops newAILootDrops = new AILootDrops()
            {
                LootdropList = new BindingList<AILoadouts>(),
                isDirty = true
            };
            newAILootDrops.Filename = filename;
            newAILootDrops.Name = UserAnswer;

            AILoadoutsConfig.AILootDropsData.Add(newAILootDrops);

            TreeNode root = treeViewMS1.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Tag is string && (string)n.Tag == TAG_LOOTDROPS_ROOT);
            if (root != null)
            {
                TreeNode fileNode = new TreeNode(newAILootDrops.Name) { Tag = newAILootDrops };
                root.Nodes.Add(fileNode);
            }
        }

        private void RemoveLootDropFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentAILootDropsFile == null) return;
            if (MessageBox.Show("This Will Remove The All reference to this Loadout, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SafeFileDelete(currentAILootDropsFile.Filename);
                AILoadoutsConfig.AILootDropsData.Remove(currentAILootDropsFile);
                if (CurrentTreenode != null && CurrentTreenode.Parent != null)
                    CurrentTreenode.Parent.Nodes.Remove(CurrentTreenode);
                currentAILootDropsFile = null;
            }
        }
        #endregion
    }
}
