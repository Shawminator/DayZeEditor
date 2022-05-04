using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool isCategoryitem { get; set; }
        public typesType currentlootpart;
        public bool UseMultiple;
        public bool LowerCase { get; set; }
        public Project currentproject { get; internal set; }
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
            LowerCase = true;

           
        }
        public bool HideUsed = false;

        private void AddItemfromTypes_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = addedtypes;
            if (isCategoryitem)
                treeView1.CheckBoxes = true;
            if (usedtypes == null)
                darkButton5.Visible = false;
            PopulateTreeView();
        }
        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types");
            vanilla.Tag = "VanillaTypes";
            foreach (typesType type in vanillatypes.types.type)
            {
                if(usedtypes != null && usedtypes.ContainsKey(type.name.ToLower()) && HideUsed == true) { continue; }
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
            treeView1.Nodes.Add(vanilla);
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
                    treeView1.Nodes.Add(ModTypes);
                }
            }
        }
        public TreeNode returntreenode(List<LootPart> partlist, string name, bool ignoredisabled = false)
        {
            TreeNode tn = new TreeNode(name);
            tn.Tag = name;
            foreach (LootPart lp in partlist)
            {
                if (lp.Disabled == true && ignoredisabled == true)
                {

                }
                else
                {
                    TreeNode partnod = new TreeNode(lp.name);
                    partnod.Tag = lp;
                    tn.Nodes.Add(partnod);
                }
            }
            return tn;
        }

        private void darkButton4_Click(object sender, EventArgs e)
        {
            if (isCategoryitem)
            {
                var selectedNodes = treeView1.Nodes.Descendants()
                    .Where(n => n.Checked)
                    .ToList();


                foreach(TreeNode node in selectedNodes)
                {
                    if (node.Tag.ToString() == "SelectAll") continue;
                    string now = node.Text;
                    if (now == "Clothes" || now == "Containers" || now == "Explosives" || now == "Food" || now == "Tools" || now == "Vehiclesparts" || now == "Weapons" || now == "others") continue;
                    addedtypes.Add(now);
                }

            }
            else
            {
                if (UseMultiple)
                {
                    if(LowerCase)
                        addedtypes.Add(currentlootpart.name.ToLower());
                    else
                        addedtypes.Add(currentlootpart.name);
                }
                else if (!addedtypes.Contains(currentlootpart.name.ToLower()) && !addedtypes.Contains(currentlootpart.name))
                {
                    if(LowerCase)
                        addedtypes.Add(currentlootpart.name.ToLower());
                    else
                        addedtypes.Add(currentlootpart.name);
                }
            }
        }
        private IEnumerable<TreeNode> Descendants(TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            addedtypes.Remove(listBox1.GetItemText(listBox1.SelectedItem));
            if (addedtypes.Count == 0)
                listBox1.SelectedIndex = -1;
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

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            {
                if (!this.manualchange)
                {
                    this.CheckTreeViewNode(e.Node, e.Node.Checked);
                }
            }
        }
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

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (currentlootpart == null) return;
            addedtypes.Add(currentlootpart.name);
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            if(darkButton5.Tag.ToString() == "HideUsed")
            {
                darkButton5.Text = "Show Used Types";
                darkButton5.Tag = "ShowUsed";
                HideUsed = true;
               
            }
            else if(darkButton5.Tag.ToString() == "ShowUsed")
            {
                darkButton5.Text = "Hide Used Types";
                darkButton5.Tag = "HideUsed";
                HideUsed = false;
            }
            PopulateTreeView();
        }
        
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count < 1)
                return;
            string text = darkTextBox1.Text;
            searchnum = 0;
            searchtreeNodes = new List<TreeNode>();
            searchtreeparts = new List<TreeNode>();
            foundparts = new List<typesType>();
            foreach(typesType type in vanillatypes.types.type)
            {
                if(type.name.ToLower().Contains(text.ToLower()))
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
            if(foundparts.Count == 0) { return; }
            foreach (typesType obj in foundparts)
            {
                foreach (TreeNode node in treeView1.Nodes)
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
            treeView1.SelectedNode = searchtreeNodes[searchnum];
            treeView1.Focus();
            if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is typesType)
            {
                currentlootpart = treeView1.SelectedNode.Tag as typesType;
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
                return;
            }
            treeView1.SelectedNode = searchtreeNodes[searchnum];
            treeView1.Focus();
            if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is typesType)
            {
                currentlootpart = treeView1.SelectedNode.Tag as typesType;
            }
        }

        private void darkTextBox1_TextChanged(object sender, EventArgs e)
        {
            darkButton7.Visible = false;
        }
    }
}
