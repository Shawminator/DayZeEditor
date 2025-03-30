using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class AddItemfromTypes : DarkForm
    {
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem == null) return;
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

        public typesType currentlootpart;

        public bool UseMultipleofSameItem = false;
        public bool UseOnlySingleitem = false;
        public bool LowerCase = false;

        public Project currentproject { get; set; }
        public TypesFile vanillatypes { get; set; }
        public List<TypesFile> ModTypes { get; set; }
        public Dictionary<string, bool> usedtypes { get; set; }
        public BindingList<string> addedtypes { get; set; }
        public AddItemfromTypes()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
            addedtypes = new BindingList<string>();
        }
        public bool HideUsed = false;

        private void AddItemfromTypes_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = addedtypes;
            if (usedtypes == null)
                darkButton5.Visible = false;
            if (UseOnlySingleitem)
                treeViewMS1.SetMultiselect = false;

            PopulateTreeView();
        }
        private void PopulateTreeView()
        {
            treeViewMS1.Nodes.Clear();
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types");
            vanilla.Tag = "VanillaTypes";
            foreach (typesType type in vanillatypes.types.type)
            {
                if (usedtypes != null && usedtypes.ContainsKey(type.name.ToLower()) && HideUsed == true) { continue; }
                string cat = "other";
                if (type.category != null)
                    cat = type.category.name;
                TreeNode typenode = new TreeNode(type.name);
                typenode.Tag = type;
                if (!vanilla.Nodes.ContainsKey(cat))
                {
                    TreeNode newcatnode = new TreeNode(cat);
                    newcatnode.Name = cat;
                    newcatnode.Tag = cat;
                    vanilla.Nodes.Add(newcatnode);
                }
                vanilla.Nodes[cat].Nodes.Add(typenode);
            }
            treeViewMS1.Nodes.Add(vanilla);
            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {

                    TreeNode ModTypes = new TreeNode(tf.modname);
                    ModTypes.Tag = tf.modname;
                    foreach (typesType type in tf.types.type)
                    {
                        if (usedtypes != null && usedtypes.ContainsKey(type.name.ToLower()) && HideUsed == true) { continue; }
                        string cat = "other";
                        if (type.category != null)
                            cat = type.category.name;
                        TreeNode typenode = new TreeNode(type.name);
                        typenode.Tag = type;
                        if (!ModTypes.Nodes.ContainsKey(cat))
                        {
                            TreeNode newcatnode = new TreeNode(cat);
                            newcatnode.Name = cat;
                            newcatnode.Tag = cat;
                            ModTypes.Nodes.Add(newcatnode);
                        }
                        ModTypes.Nodes[cat].Nodes.Add(typenode);
                    }
                    treeViewMS1.Nodes.Add(ModTypes);
                }
            }
        }
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (currentlootpart == null) return;
            Additem(currentlootpart.name);
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                AddItem(tn);
            }
        }
        public void AddItem(TreeNode tn)
        {
            if (tn.Tag is string)
            {
                foreach (TreeNode tn1 in tn.Nodes)
                {
                    AddItem(tn1);
                }
            }
            else if (tn.Tag is typesType)
            {
                typesType looptype = tn.Tag as typesType;
                Additem(looptype.name);
            }
        }
        public void Additem(string item)
        {
            if (UseOnlySingleitem)
                addedtypes.Clear();
            if (LowerCase)
            {
                if (!UseMultipleofSameItem && !addedtypes.Contains(item.ToLower()))
                {
                    addedtypes.Add(item.ToLower());
                }
                else if (UseMultipleofSameItem)
                {
                    addedtypes.Add(item.ToLower());
                }
                else
                {
                    MessageBox.Show(item + " Allready in the list");
                }
            }
            else
            {
                if (!UseMultipleofSameItem && !addedtypes.Contains(item))
                {
                    addedtypes.Add(item);
                }
                else if (UseMultipleofSameItem)
                {
                    addedtypes.Add(item);
                }
                else
                {
                    MessageBox.Show(item + " Allready in the list");
                }
            }
        }
        private void RemoveItemsButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            List<string> removeitems = new List<string>();
            foreach (var item in listBox1.SelectedItems)
            {
                removeitems.Add(item.ToString());
            }
            foreach (string item in removeitems)
            {
                addedtypes.Remove(item);
                if (listBox1.Items.Count == 0)
                    listBox1.SelectedIndex = -1;
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is typesType)
            {
                currentlootpart = e.Node.Tag as typesType;
            }
        }
        public bool manualchange = false;
        private int searchnum;
        private List<TreeNode> searchtreeNodes;
        private List<TreeNode> searchtreeparts;
        private List<typesType> foundparts;

        private void CheckTreeViewNode(TreeNode node, bool isChecked)
        {
            manualchange = true;
            if (node.Nodes.Count > 0)
            {
                switch (isChecked)
                {
                    case false:
                        node.Tag = null;
                        break;
                    case true:
                        node.Tag = "SelectAll";
                        break;
                }
            }
            foreach (TreeNode treeNode in node.Nodes)
            {
                treeNode.Checked = isChecked;
                if (treeNode.Nodes.Count > 0)
                {
                    CheckTreeViewNode(treeNode, isChecked);
                }
            }
            manualchange = false;
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (darkButton5.Tag.ToString() == "HideUsed")
            {
                darkButton5.Text = "Show Used Types";
                darkButton5.Tag = "ShowUsed";
                HideUsed = true;

            }
            else if (darkButton5.Tag.ToString() == "ShowUsed")
            {
                darkButton5.Text = "Hide Used Types";
                darkButton5.Tag = "HideUsed";
                HideUsed = false;
            }
            PopulateTreeView();
        }

        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (treeViewMS1.Nodes.Count < 1)
                return;
            string text = darkTextBox1.Text;
            searchnum = 0;
            searchtreeNodes = new List<TreeNode>();
            searchtreeparts = new List<TreeNode>();
            foundparts = new List<typesType>();
            foreach (typesType type in vanillatypes.types.type)
            {
                if (type.name.ToLower().Contains(text.ToLower()))
                {
                    foundparts.Add(type);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.types.type)
                {
                    if (type.name.ToLower().Contains(text.ToLower()))
                    {
                        foundparts.Add(type);
                    }
                }
            }
            if (foundparts.Count == 0) { return; }
            foreach (typesType obj in foundparts)
            {
                foreach (TreeNode node in treeViewMS1.Nodes)
                {
                    searchtree(obj.name.ToLower(), node, searchtreeNodes);
                }
            }
            //foreach (TreeNode tn in searchtreeNodes)
            //{
            //    if (!tn.Checked)
            //        tn.Checked = true;
            //}
            //foreach (TreeNode tn in searchtreeNodes)
            //{
            //    searchtree(text, tn, searchtreeparts);
            //}
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
            }
            if (searchtreeNodes.Count > 1)
                darkButton7.Visible = true;
        }
        private void searchtree(string str, TreeNode tree, List<TreeNode> List)
        {
            if (tree.Text.ToString().ToLower().Contains(str))
                if (!List.Contains(tree))
                    List.Add(tree);
            foreach (TreeNode tn in tree.Nodes)
            {
                if (tn.Text.ToString().ToLower().Contains(str))
                    if (!List.Contains(tn))
                        List.Add(tn);
                if (tn.Nodes.Count > 0)
                {
                    foreach (TreeNode ctn in tn.Nodes)
                        searchtree(str, ctn, List);
                }
            }
        }

        private void darkButton7_Click(object sender, EventArgs e)
        {
            searchnum++;
            if (searchnum == searchtreeNodes.Count)
            {
                MessageBox.Show("No More Items found");
                darkButton7.Visible = false;
                return;
            }
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
            }
        }
    }
}
