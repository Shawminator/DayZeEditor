using DayZeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public partial class ExpansionLootControl : UserControl
    {
        [Browsable(true)]
        public event PropertyChangedEventHandler IsDirtyChanged;
        private void NotifyPropertyChanged(string info)
        {
            IsDirtyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        private bool _isdirty;
        public bool isDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                if (_isdirty != value)
                {
                    _isdirty = value;
                    NotifyPropertyChanged("isdirty");
                }
            }
        }
        private bool useraction { get; set; }
        public string LootparentName { get; set; }
        private BindingList<ExpansionLoot> _currentExpansionLoot { get; set; }
        public BindingList<ExpansionLoot> currentExpansionLoot
        {
            get
            {
                return _currentExpansionLoot;
            }
            set
            {
                _currentExpansionLoot = value;
                updatevalues();
            }
        }

        public TypesFile vanillatypes { get; set; }
        public List<TypesFile> ModTypes { get; set; }
        public Project currentproject { get; set; }

        public ExpansionLoot currentExpanionLootItem;
        public ExpansionLootVariant CurrentLootVArient;
        

        public ExpansionLootControl()
        {
            InitializeComponent();
        }

        private void updatevalues()
        {
            if (_currentExpansionLoot == null) return;
            useraction = false;
            expansionLootItemGB.Visible = false;
            expansionLootVarientGB.Visible = false;

            addLootItemsToolStripMenuItem.Visible = false;
            addLootAttachmentsToolStripMenuItem.Visible = false;
            addLootVariantsToolStripMenuItem.Visible = false;
            removeLootItemToolStripMenuItem.Visible = false;
            removeLootVariantToolStripMenuItem.Visible = false;
            removeLootAttachemntToolStripMenuItem.Visible = false;
            ExpansionLootitemSetAllChanceButton.Visible = false;
            ExpansionLootitemSetAllRandomChanceButton.Visible = false;
            ExpansionLootTV.Nodes.Clear();
            TreeNode root = new TreeNode(LootparentName)
            {
                Tag = "Parent"
            };
            foreach (ExpansionLoot EL in _currentExpansionLoot)
            {
                root.Nodes.Add(CreateLootNode(EL));
            }
            ExpansionLootTV.Nodes.Add(root);
            root.Expand();
            useraction = true;
        }
        private TreeNode CreateLootNode(ExpansionLoot eL)
        {
            TreeNode ExpansionLootTN = new TreeNode(eL.Name)
            {
                Tag = eL
            };
            TreeNode AttachmentTN = new TreeNode("Attachments")
            {
                Tag = "Attachments"
            };
            foreach(ExpansionLootVariant elv in eL.Attachments)
            {
                AttachmentTN.Nodes.Add(getLootVarients(elv));
            }
            TreeNode VariantsTN = new TreeNode("Variants")
            {
                Tag = "Variants"
            };
            foreach (ExpansionLootVariant elv in eL.Variants)
            {
                VariantsTN.Nodes.Add(getLootVarients(elv));
            }
            ExpansionLootTN.Nodes.Add(AttachmentTN);
            ExpansionLootTN.Nodes.Add(VariantsTN);
            return ExpansionLootTN;
        }
        private TreeNode getLootVarients(ExpansionLootVariant elv)
        {
            TreeNode ExpansionLootVarientTN = new TreeNode(elv.Name)
            {
                Tag = elv
            };
            TreeNode AttachmentTN = new TreeNode("Attachments")
            {
                Tag = "Attachments"
            };
            foreach (ExpansionLootVariant elv2 in elv.Attachments)
            {
                AttachmentTN.Nodes.Add(getLootVarients(elv2));
            }
            ExpansionLootVarientTN.Nodes.Add(AttachmentTN);
            return ExpansionLootVarientTN;
        }

        private void ExpansionLootTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            ExpansionLootTV.SelectedNode = e.Node;
            expansionLootItemGB.Visible = false;
            expansionLootVarientGB.Visible = false;
            ExpansionLootitemSetAllChanceButton.Visible = false;
            ExpansionLootitemSetAllRandomChanceButton.Visible = false;
            addLootItemsToolStripMenuItem.Visible = false;
            addLootAttachmentsToolStripMenuItem.Visible = false;
            addLootVariantsToolStripMenuItem.Visible = false;
            removeLootItemToolStripMenuItem.Visible = false;
            removeLootVariantToolStripMenuItem.Visible = false;
            removeLootAttachemntToolStripMenuItem.Visible = false;

            currentExpanionLootItem = null;
            CurrentLootVArient = null;

            if (e.Node.Tag is string)
            {
                if (e.Node.Tag.ToString() == "Parent")
                {
                    expansionLootVarientGB.Visible = true;
                    expansionLootVarientGB.Text = "Set All Chance";
                    ExpansionLootitemSetAllChanceButton.Visible = true;
                    ExpansionLootitemSetAllRandomChanceButton.Visible = true;
                    if (e.Button == MouseButtons.Right)
                    {
                        addLootItemsToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
                if (e.Node.Tag.ToString() == "Attachments")
                {
                    if (e.Node.Parent.Tag is ExpansionLoot)
                        currentExpanionLootItem = e.Node.Parent.Tag as ExpansionLoot;
                    else if (e.Node.Parent.Tag is ExpansionLootVariant)
                        CurrentLootVArient = e.Node.Parent.Tag as ExpansionLootVariant;
                    if (e.Button == MouseButtons.Right)
                    {
                        addLootAttachmentsToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
                if (e.Node.Tag.ToString() == "Variants")
                {
                    currentExpanionLootItem = e.Node.Parent.Tag as ExpansionLoot;
                    if (e.Button == MouseButtons.Right)
                    {
                        addLootVariantsToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Node.Tag is ExpansionLoot)
            {
                expansionLootItemGB.Visible = true;
                currentExpanionLootItem = e.Node.Tag as ExpansionLoot;
                SetLootitem();
                if (e.Button == MouseButtons.Right)
                {
                    removeLootItemToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is ExpansionLootVariant)
            {
                expansionLootVarientGB.Visible = true;
                CurrentLootVArient = e.Node.Tag as ExpansionLootVariant;
                setvarient();
                if (e.Node.Parent.Tag.ToString() == "Attachments")
                {
                    expansionLootVarientGB.Text = "Expansion Loot Attachment";
                    if (e.Button == MouseButtons.Right)
                    {
                        removeLootAttachemntToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
                else if (e.Node.Parent.Tag.ToString() == "Variants")
                {
                    expansionLootVarientGB.Text = "Expansion Loot Variant";
                    if (e.Button == MouseButtons.Right)
                    {
                        removeLootVariantToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
            useraction = true;
        }
        private void SetLootitem()
        {
            useraction = false;

            if (currentExpanionLootItem.Chance > 1)
                currentExpanionLootItem.Chance = 1;
            trackBar1.Value = (int)(currentExpanionLootItem.Chance * 100); 
            numericUpDown31.Value = currentExpanionLootItem.QuantityPercent;
            numericUpDown12.Value = currentExpanionLootItem.Max;
            numericUpDown33.Value = currentExpanionLootItem.Min;


            useraction = true;
        }
        private void setvarient()
        {
            useraction = false;
            if (CurrentLootVArient.Chance > 1)
                CurrentLootVArient.Chance = 1;
            trackBar2.Value = (int)(CurrentLootVArient.Chance * 100);


            useraction = true;
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            darkLabel23.Text = ((decimal)(trackBar1.Value)).ToString() + "%";
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            darkLabel23.Text = ((decimal)(trackBar1.Value)).ToString() + "%";
        }
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentExpanionLootItem == null) return;
            currentExpanionLootItem.Chance = ((decimal)trackBar1.Value) / 100;
            isDirty = true;
        }
        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                currentExpanionLootItem.QuantityPercent = (int)numericUpDown31.Value;
                isDirty = true;
            }
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                currentExpanionLootItem.Max = (int)numericUpDown12.Value;
                isDirty = true;
            }
        }
        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            if (useraction)
            {
                currentExpanionLootItem.Min = (int)numericUpDown33.Value;
                isDirty = true;
            }
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            darkLabel1.Text = ((decimal)(trackBar2.Value)).ToString() + "%";
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            darkLabel1.Text = ((decimal)(trackBar2.Value)).ToString() + "%";
        }
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentLootVArient == null) return;
            CurrentLootVArient.Chance = ((decimal)trackBar2.Value) / 100;
            isDirty = true;
        }
        private void addLootItemsToolStripMenuItem_Click(object sender, EventArgs e)
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
                TreeNode FocusNode = new TreeNode();
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ExpansionLoot Newloot = new ExpansionLoot()
                    {
                        Name = l,
                        Attachments = new BindingList<ExpansionLootVariant>(),
                        Chance = (decimal)0.5,
                        Max = -1,
                        Min = 0,
                        Variants = new BindingList<ExpansionLootVariant>()
                    };
                    _currentExpansionLoot.Add(Newloot);
                    TreeNode tn = CreateLootNode(Newloot);
                    ExpansionLootTV.SelectedNode.Nodes.Add(tn);
                    FocusNode = tn;
                    isDirty = true;
                }
                ExpansionLootTV.SelectedNode = FocusNode;
                ExpansionLootTV.Focus();
                currentExpanionLootItem = ExpansionLootTV.SelectedNode.Tag as ExpansionLoot;
                SetLootitem();
            }
        }
        private void addLootVariantsToolStripMenuItem_Click(object sender, EventArgs e)
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
                TreeNode FocusNode = new TreeNode();
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ExpansionLootVariant Newloot = new ExpansionLootVariant(l);
                    ExpansionLoot loot = ExpansionLootTV.SelectedNode.Parent.Tag as ExpansionLoot;
                    loot.Variants.Add(Newloot);
                    TreeNode tn = getLootVarients(Newloot);
                    ExpansionLootTV.SelectedNode.Nodes.Add(tn);
                    FocusNode = tn;
                    isDirty = true;
                }
                ExpansionLootTV.SelectedNode = FocusNode;
                ExpansionLootTV.Focus();
                setvarient();
            }
        }
        private void addLootAttachmentsToolStripMenuItem_Click(object sender, EventArgs e)
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
                TreeNode FocusNode = new TreeNode();
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (ExpansionLootTV.SelectedNode.Parent.Tag is ExpansionLoot)
                    {
                        ExpansionLootVariant Newloot = new ExpansionLootVariant(l);
                        ExpansionLoot loot = ExpansionLootTV.SelectedNode.Parent.Tag as ExpansionLoot;
                        loot.Attachments.Add(Newloot);
                        TreeNode tn = getLootVarients(Newloot);
                        ExpansionLootTV.SelectedNode.Nodes.Add(tn);
                        FocusNode = tn;
                    }
                    else if (ExpansionLootTV.SelectedNode.Parent.Tag is ExpansionLootVariant)
                    {
                        ExpansionLootVariant Newloot = new ExpansionLootVariant(l);
                        ExpansionLootVariant loot = ExpansionLootTV.SelectedNode.Parent.Tag as ExpansionLootVariant;
                        loot.Attachments.Add(Newloot);
                        TreeNode tn = getLootVarients(Newloot);
                        ExpansionLootTV.SelectedNode.Nodes.Add(tn);
                        FocusNode = tn;
                    }
                    isDirty = true;
                }
                ExpansionLootTV.SelectedNode = FocusNode;
                ExpansionLootTV.Focus();
                setvarient();
            }
        }
        private void removeLootItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentExpansionLoot.Remove(currentExpanionLootItem);
            ExpansionLootTV.SelectedNode.Remove();
            isDirty = true;
        }
        private void removeLootAttachemntToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ExpansionLootTV.SelectedNode.Parent.Parent.Tag is ExpansionLoot)
            {
                ExpansionLoot loot = ExpansionLootTV.SelectedNode.Parent.Parent.Tag as ExpansionLoot;
                loot.Attachments.Remove(CurrentLootVArient);
                ExpansionLootTV.SelectedNode.Remove();
            }
            else if (ExpansionLootTV.SelectedNode.Parent.Parent.Tag is ExpansionLootVariant)
            {
                ExpansionLootVariant loot = ExpansionLootTV.SelectedNode.Parent.Parent.Tag as ExpansionLootVariant;
                loot.Attachments.Remove(CurrentLootVArient);
                ExpansionLootTV.SelectedNode.Remove();
            }
            isDirty = true;
        }
        private void removeLootVariantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpansionLoot loot = ExpansionLootTV.SelectedNode.Parent.Parent.Tag as ExpansionLoot;
            loot.Variants.Remove(CurrentLootVArient);
            ExpansionLootTV.SelectedNode.Remove();
            isDirty = true;
        }
        private void ExpansionLootitemSetAllChanceButton_Click(object sender, EventArgs e)
        {
            foreach(ExpansionLoot el in _currentExpansionLoot)
            {
                el.Chance = ((decimal)trackBar2.Value) / 100;
            }
            isDirty = true;
        }
        private void ExpansionLootitemSetAllRandomChanceButton_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            foreach (ExpansionLoot el in _currentExpansionLoot)
            {
                typesType type = vanillatypes.Gettypebyname(el.Name);
                if(type == null)
                {
                    foreach (TypesFile tf in ModTypes)
                    {
                        type = tf.Gettypebyname(el.Name);
                        if (type != null)
                            break;
                    }
                }
                if (type == null) continue;
                int chancemax;
                int chancemin;
                if (type.nominal <= 1)
                {
                    chancemin = 1;
                    chancemax = 11;
                }
                else if (type.nominal <= 5)
                {
                    chancemin = 11;
                    chancemax = 26;
                }
                else if (type.nominal <= 10)
                {
                    chancemin = 21;
                    chancemax = 51;
                }
                else if (type.nominal <= 15)
                {
                    chancemin = 31;
                    chancemax = 76;
                }
                else
                {
                    chancemin = 41;
                    chancemax = 101;
                }

                el.Chance = (decimal)rnd.Next(chancemin, chancemax) / 100;
            }
            isDirty = true;
        }
    }
}
