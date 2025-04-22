using System;
using System.Diagnostics;
using System.Drawing;
using DarkUI.Collections;

namespace DarkUI.Controls
{
	// Token: 0x0200004A RID: 74
	public class DarkTreeNode
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000320 RID: 800 RVA: 0x00013810 File Offset: 0x00011A10
		// (remove) Token: 0x06000321 RID: 801 RVA: 0x00013848 File Offset: 0x00011A48
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ObservableListModified<DarkTreeNode>> ItemsAdded;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000322 RID: 802 RVA: 0x00013880 File Offset: 0x00011A80
		// (remove) Token: 0x06000323 RID: 803 RVA: 0x000138B8 File Offset: 0x00011AB8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ObservableListModified<DarkTreeNode>> ItemsRemoved;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000324 RID: 804 RVA: 0x000138F0 File Offset: 0x00011AF0
		// (remove) Token: 0x06000325 RID: 805 RVA: 0x00013928 File Offset: 0x00011B28
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler TextChanged;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000326 RID: 806 RVA: 0x00013960 File Offset: 0x00011B60
		// (remove) Token: 0x06000327 RID: 807 RVA: 0x00013998 File Offset: 0x00011B98
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler NodeExpanded;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000328 RID: 808 RVA: 0x000139D0 File Offset: 0x00011BD0
		// (remove) Token: 0x06000329 RID: 809 RVA: 0x00013A08 File Offset: 0x00011C08
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler NodeCollapsed;

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00013A40 File Offset: 0x00011C40
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00013A58 File Offset: 0x00011C58
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				bool flag = this._text == value;
				if (!flag)
				{
					this._text = value;
					this.OnTextChanged();
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00013A86 File Offset: 0x00011C86
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00013A8E File Offset: 0x00011C8E
		public Rectangle ExpandArea { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00013A97 File Offset: 0x00011C97
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00013A9F File Offset: 0x00011C9F
		public Rectangle IconArea { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00013AA8 File Offset: 0x00011CA8
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00013AB0 File Offset: 0x00011CB0
		public Rectangle TextArea { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00013AB9 File Offset: 0x00011CB9
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00013AC1 File Offset: 0x00011CC1
		public Rectangle FullArea { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00013ACA File Offset: 0x00011CCA
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00013AD2 File Offset: 0x00011CD2
		public bool ExpandAreaHot { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00013ADB File Offset: 0x00011CDB
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00013AE3 File Offset: 0x00011CE3
		public Bitmap Icon { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00013AEC File Offset: 0x00011CEC
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00013AF4 File Offset: 0x00011CF4
		public Bitmap ExpandedIcon { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00013B00 File Offset: 0x00011D00
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00013B18 File Offset: 0x00011D18
		public bool Expanded
		{
			get
			{
				return this._expanded;
			}
			set
			{
				bool flag = this._expanded == value;
				if (!flag)
				{
					bool flag2 = value && this.Nodes.Count == 0;
					if (!flag2)
					{
						this._expanded = value;
						bool expanded = this._expanded;
						if (expanded)
						{
							bool flag3 = this.NodeExpanded != null;
							if (flag3)
							{
								this.NodeExpanded(this, null);
							}
						}
						else
						{
							bool flag4 = this.NodeCollapsed != null;
							if (flag4)
							{
								this.NodeCollapsed(this, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00013BA0 File Offset: 0x00011DA0
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00013BB8 File Offset: 0x00011DB8
		public ObservableList<DarkTreeNode> Nodes
		{
			get
			{
				return this._nodes;
			}
			set
			{
				bool flag = this._nodes != null;
				if (flag)
				{
					this._nodes.ItemsAdded -= this.Nodes_ItemsAdded;
					this._nodes.ItemsRemoved -= this.Nodes_ItemsRemoved;
				}
				this._nodes = value;
				this._nodes.ItemsAdded += this.Nodes_ItemsAdded;
				this._nodes.ItemsRemoved += this.Nodes_ItemsRemoved;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00013C3C File Offset: 0x00011E3C
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00013C54 File Offset: 0x00011E54
		public bool IsRoot
		{
			get
			{
				return this._isRoot;
			}
			set
			{
				this._isRoot = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00013C60 File Offset: 0x00011E60
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00013C78 File Offset: 0x00011E78
		public DarkTreeView ParentTree
		{
			get
			{
				return this._parentTree;
			}
			set
			{
				bool flag = this._parentTree == value;
				if (!flag)
				{
					this._parentTree = value;
					foreach (DarkTreeNode node in this.Nodes)
					{
						node.ParentTree = this._parentTree;
					}
				}
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00013CEC File Offset: 0x00011EEC
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00013D04 File Offset: 0x00011F04
		public DarkTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00013D0E File Offset: 0x00011F0E
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00013D16 File Offset: 0x00011F16
		public bool Odd { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00013D1F File Offset: 0x00011F1F
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00013D27 File Offset: 0x00011F27
		public object NodeType { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00013D30 File Offset: 0x00011F30
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00013D38 File Offset: 0x00011F38
		public object Tag { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00013D44 File Offset: 0x00011F44
		public string FullPath
		{
			get
			{
				DarkTreeNode parent = this.ParentNode;
				string path = this.Text;
				while (parent != null)
				{
					path = string.Format("{0}{1}{2}", parent.Text, "\\", path);
					parent = parent.ParentNode;
				}
				return path;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00013D8F File Offset: 0x00011F8F
		// (set) Token: 0x0600034C RID: 844 RVA: 0x00013D97 File Offset: 0x00011F97
		public DarkTreeNode PrevVisibleNode { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00013DA0 File Offset: 0x00011FA0
		// (set) Token: 0x0600034E RID: 846 RVA: 0x00013DA8 File Offset: 0x00011FA8
		public DarkTreeNode NextVisibleNode { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00013DB1 File Offset: 0x00011FB1
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00013DB9 File Offset: 0x00011FB9
		public int VisibleIndex { get; set; }

		// Token: 0x06000351 RID: 849 RVA: 0x00013DC4 File Offset: 0x00011FC4
		public bool IsNodeAncestor(DarkTreeNode node)
		{
			for (DarkTreeNode parent = this.ParentNode; parent != null; parent = parent.ParentNode)
			{
				bool flag = parent == node;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00013DFD File Offset: 0x00011FFD
		public DarkTreeNode()
		{
			this.Nodes = new ObservableList<DarkTreeNode>();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00013E13 File Offset: 0x00012013
		public DarkTreeNode(string text)
			: this()
		{
			this.Text = text;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00013E28 File Offset: 0x00012028
		public void Remove()
		{
			bool flag = this.ParentNode != null;
			if (flag)
			{
				this.ParentNode.Nodes.Remove(this);
			}
			else
			{
				this.ParentTree.Nodes.Remove(this);
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00013E6C File Offset: 0x0001206C
		public void EnsureVisible()
		{
			for (DarkTreeNode parent = this.ParentNode; parent != null; parent = parent.ParentNode)
			{
				parent.Expanded = true;
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00013E9C File Offset: 0x0001209C
		private void OnTextChanged()
		{
			bool flag = this.ParentTree != null && this.ParentTree.TreeViewNodeSorter != null;
			if (flag)
			{
				bool flag2 = this.ParentNode != null;
				if (flag2)
				{
					this.ParentNode.Nodes.Sort(this.ParentTree.TreeViewNodeSorter);
				}
				else
				{
					this.ParentTree.Nodes.Sort(this.ParentTree.TreeViewNodeSorter);
				}
			}
			bool flag3 = this.TextChanged != null;
			if (flag3)
			{
				this.TextChanged(this, null);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00013F2C File Offset: 0x0001212C
		private void Nodes_ItemsAdded(object sender, ObservableListModified<DarkTreeNode> e)
		{
			foreach (DarkTreeNode node in e.Items)
			{
				node.ParentNode = this;
				node.ParentTree = this.ParentTree;
			}
			bool flag = this.ParentTree != null && this.ParentTree.TreeViewNodeSorter != null;
			if (flag)
			{
				this.Nodes.Sort(this.ParentTree.TreeViewNodeSorter);
			}
			bool flag2 = this.ItemsAdded != null;
			if (flag2)
			{
				this.ItemsAdded(this, e);
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00013FDC File Offset: 0x000121DC
		private void Nodes_ItemsRemoved(object sender, ObservableListModified<DarkTreeNode> e)
		{
			bool flag = this.Nodes.Count == 0;
			if (flag)
			{
				this.Expanded = false;
			}
			bool flag2 = this.ItemsRemoved != null;
			if (flag2)
			{
				this.ItemsRemoved(this, e);
			}
		}

		// Token: 0x0400021C RID: 540
		private string _text;

		// Token: 0x0400021D RID: 541
		private bool _isRoot;

		// Token: 0x0400021E RID: 542
		private DarkTreeView _parentTree;

		// Token: 0x0400021F RID: 543
		private DarkTreeNode _parentNode;

		// Token: 0x04000220 RID: 544
		private ObservableList<DarkTreeNode> _nodes;

		// Token: 0x04000221 RID: 545
		private bool _expanded;
	}
}
