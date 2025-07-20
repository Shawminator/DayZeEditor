using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Extensions;
using DarkUI.Forms;

namespace DarkUI.Controls
{
	// Token: 0x0200003B RID: 59
	public class DarkTreeView : DarkScrollView
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000224 RID: 548 RVA: 0x0000D100 File Offset: 0x0000B300
		// (remove) Token: 0x06000225 RID: 549 RVA: 0x0000D138 File Offset: 0x0000B338
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler SelectedNodesChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000226 RID: 550 RVA: 0x0000D170 File Offset: 0x0000B370
		// (remove) Token: 0x06000227 RID: 551 RVA: 0x0000D1A8 File Offset: 0x0000B3A8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler AfterNodeExpand;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000228 RID: 552 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		// (remove) Token: 0x06000229 RID: 553 RVA: 0x0000D218 File Offset: 0x0000B418
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler AfterNodeCollapse;

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000D250 File Offset: 0x0000B450
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000D268 File Offset: 0x0000B468
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
					foreach (DarkTreeNode node in this._nodes)
					{
						this.UnhookNodeEvents(node);
					}
				}
				this._nodes = value;
				this._nodes.ItemsAdded += this.Nodes_ItemsAdded;
				this._nodes.ItemsRemoved += this.Nodes_ItemsRemoved;
				foreach (DarkTreeNode node2 in this._nodes)
				{
					this.HookNodeEvents(node2);
				}
				this.UpdateNodes();
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000D384 File Offset: 0x0000B584
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ObservableCollection<DarkTreeNode> SelectedNodes
		{
			get
			{
				return this._selectedNodes;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000D39C File Offset: 0x0000B59C
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		[Category("Appearance")]
		[Description("Determines the height of tree nodes.")]
		[DefaultValue(20)]
		public int ItemHeight
		{
			get
			{
				return this._itemHeight;
			}
			set
			{
				this._itemHeight = value;
				base.MaxDragChange = this._itemHeight;
				this.UpdateNodes();
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000D3D4 File Offset: 0x0000B5D4
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000D3EC File Offset: 0x0000B5EC
		[Category("Appearance")]
		[Description("Determines the amount of horizontal space given by parent node.")]
		[DefaultValue(20)]
		public int Indent
		{
			get
			{
				return this._indent;
			}
			set
			{
				this._indent = value;
				this.UpdateNodes();
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000D3FD File Offset: 0x0000B5FD
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000D405 File Offset: 0x0000B605
		[Category("Behavior")]
		[Description("Determines whether multiple tree nodes can be selected at once.")]
		[DefaultValue(false)]
		public bool MultiSelect { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000D40E File Offset: 0x0000B60E
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000D416 File Offset: 0x0000B616
		[Category("Behavior")]
		[Description("Determines whether nodes can be moved within this tree view.")]
		[DefaultValue(false)]
		public bool AllowMoveNodes { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000D41F File Offset: 0x0000B61F
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000D427 File Offset: 0x0000B627
		[Category("Appearance")]
		[Description("Determines whether icons are rendered with the tree nodes.")]
		[DefaultValue(false)]
		public bool ShowIcons { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000D430 File Offset: 0x0000B630
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000D438 File Offset: 0x0000B638
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int VisibleNodeCount { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000D441 File Offset: 0x0000B641
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000D449 File Offset: 0x0000B649
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComparer<DarkTreeNode> TreeViewNodeSorter { get; set; }

		// Token: 0x0600023B RID: 571 RVA: 0x0000D454 File Offset: 0x0000B654
		public DarkTreeView()
		{
			this.Nodes = new ObservableList<DarkTreeNode>();
			this._selectedNodes = new ObservableCollection<DarkTreeNode>();
			this._selectedNodes.CollectionChanged += this.SelectedNodes_CollectionChanged;
			base.MaxDragChange = this._itemHeight;
			this.LoadIcons();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		protected override void Dispose(bool disposing)
		{
			bool flag = !this._disposed;
			if (flag)
			{
				this.DisposeIcons();
				bool flag2 = this.SelectedNodesChanged != null;
				if (flag2)
				{
					this.SelectedNodesChanged = null;
				}
				bool flag3 = this.AfterNodeExpand != null;
				if (flag3)
				{
					this.AfterNodeExpand = null;
				}
				bool flag4 = this.AfterNodeCollapse != null;
				if (flag4)
				{
					this.AfterNodeExpand = null;
				}
				bool flag5 = this._nodes != null;
				if (flag5)
				{
					this._nodes.Dispose();
				}
				bool flag6 = this._selectedNodes != null;
				if (flag6)
				{
					this._selectedNodes.CollectionChanged -= this.SelectedNodes_CollectionChanged;
				}
				this._disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D580 File Offset: 0x0000B780
		private void Nodes_ItemsAdded(object sender, ObservableListModified<DarkTreeNode> e)
		{
			foreach (DarkTreeNode node in e.Items)
			{
				node.ParentTree = this;
				node.IsRoot = true;
				this.HookNodeEvents(node);
			}
			bool flag = this.TreeViewNodeSorter != null;
			if (flag)
			{
				this.Nodes.Sort(this.TreeViewNodeSorter);
			}
			this.UpdateNodes();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D60C File Offset: 0x0000B80C
		private void Nodes_ItemsRemoved(object sender, ObservableListModified<DarkTreeNode> e)
		{
			foreach (DarkTreeNode node in e.Items)
			{
				node.ParentTree = this;
				node.IsRoot = true;
				this.HookNodeEvents(node);
			}
			this.UpdateNodes();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D678 File Offset: 0x0000B878
		private void ChildNodes_ItemsAdded(object sender, ObservableListModified<DarkTreeNode> e)
		{
			foreach (DarkTreeNode node in e.Items)
			{
				this.HookNodeEvents(node);
			}
			this.UpdateNodes();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000D6D0 File Offset: 0x0000B8D0
		private void ChildNodes_ItemsRemoved(object sender, ObservableListModified<DarkTreeNode> e)
		{
			foreach (DarkTreeNode node in e.Items)
			{
				bool flag = this.SelectedNodes.Contains(node);
				if (flag)
				{
					this.SelectedNodes.Remove(node);
				}
				this.UnhookNodeEvents(node);
			}
			this.UpdateNodes();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D748 File Offset: 0x0000B948
		private void SelectedNodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			bool flag = this.SelectedNodesChanged != null;
			if (flag)
			{
				this.SelectedNodesChanged(this, null);
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D771 File Offset: 0x0000B971
		private void Nodes_TextChanged(object sender, EventArgs e)
		{
			this.UpdateNodes();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D77C File Offset: 0x0000B97C
		private void Nodes_NodeExpanded(object sender, EventArgs e)
		{
			this.UpdateNodes();
			bool flag = this.AfterNodeExpand != null;
			if (flag)
			{
				this.AfterNodeExpand(this, null);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		private void Nodes_NodeCollapsed(object sender, EventArgs e)
		{
			this.UpdateNodes();
			bool flag = this.AfterNodeCollapse != null;
			if (flag)
			{
				this.AfterNodeCollapse(this, null);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D7DC File Offset: 0x0000B9DC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			bool provisionalDragging = this._provisionalDragging;
			if (provisionalDragging)
			{
				bool flag = base.OffsetMousePosition != this._dragPos;
				if (flag)
				{
					this.StartDrag();
					this.HandleDrag();
					return;
				}
			}
			bool isDragging = base.IsDragging;
			if (isDragging)
			{
				bool flag2 = this._dropNode != null;
				if (flag2)
				{
					bool flag3 = !this.GetNodeFullRowArea(this._dropNode).Contains(base.OffsetMousePosition);
					if (flag3)
					{
						this._dropNode = null;
						base.Invalidate();
					}
				}
			}
			this.CheckHover();
			bool isDragging2 = base.IsDragging;
			if (isDragging2)
			{
				this.HandleDrag();
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D88F File Offset: 0x0000BA8F
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.CheckHover();
			base.OnMouseWheel(e);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		protected override void OnMouseDown(MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left || e.Button == MouseButtons.Right;
			if (flag)
			{
				foreach (DarkTreeNode node in this.Nodes)
				{
					this.CheckNodeClick(node, base.OffsetMousePosition, e.Button);
				}
			}
			base.OnMouseDown(e);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000D934 File Offset: 0x0000BB34
		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool isDragging = base.IsDragging;
			if (isDragging)
			{
				this.HandleDrop();
			}
			bool provisionalDragging = this._provisionalDragging;
			if (provisionalDragging)
			{
				bool flag = this._provisionalNode != null;
				if (flag)
				{
					Point pos = this._dragPos;
					bool flag2 = base.OffsetMousePosition == pos;
					if (flag2)
					{
						this.SelectNode(this._provisionalNode);
					}
				}
				this._provisionalDragging = false;
			}
			base.OnMouseUp(e);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			bool flag = Control.ModifierKeys == Keys.Control;
			if (!flag)
			{
				bool flag2 = e.Button == MouseButtons.Left;
				if (flag2)
				{
					foreach (DarkTreeNode node in this.Nodes)
					{
						this.CheckNodeDoubleClick(node, base.OffsetMousePosition);
					}
				}
				base.OnMouseDoubleClick(e);
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000DA34 File Offset: 0x0000BC34
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			foreach (DarkTreeNode node in this.Nodes)
			{
				this.NodeMouseLeave(node);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000DA94 File Offset: 0x0000BC94
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool isDragging = base.IsDragging;
			if (!isDragging)
			{
				bool flag = this.Nodes.Count == 0;
				if (!flag)
				{
					bool flag2 = e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right;
					if (!flag2)
					{
						bool flag3 = this._anchoredNodeEnd == null;
						if (flag3)
						{
							bool flag4 = this.Nodes.Count > 0;
							if (flag4)
							{
								this.SelectNode(this.Nodes[0]);
							}
						}
						else
						{
							bool flag5 = e.KeyCode == Keys.Down || e.KeyCode == Keys.Up;
							if (flag5)
							{
								bool flag6 = this.MultiSelect && Control.ModifierKeys == Keys.Shift;
								if (flag6)
								{
									bool flag7 = e.KeyCode == Keys.Up;
									if (flag7)
									{
										bool flag8 = this._anchoredNodeEnd.PrevVisibleNode != null;
										if (flag8)
										{
											this.SelectAnchoredRange(this._anchoredNodeEnd.PrevVisibleNode);
											this.EnsureVisible();
										}
									}
									else
									{
										bool flag9 = e.KeyCode == Keys.Down;
										if (flag9)
										{
											bool flag10 = this._anchoredNodeEnd.NextVisibleNode != null;
											if (flag10)
											{
												this.SelectAnchoredRange(this._anchoredNodeEnd.NextVisibleNode);
												this.EnsureVisible();
											}
										}
									}
								}
								else
								{
									bool flag11 = e.KeyCode == Keys.Up;
									if (flag11)
									{
										bool flag12 = this._anchoredNodeEnd.PrevVisibleNode != null;
										if (flag12)
										{
											this.SelectNode(this._anchoredNodeEnd.PrevVisibleNode);
											this.EnsureVisible();
										}
									}
									else
									{
										bool flag13 = e.KeyCode == Keys.Down;
										if (flag13)
										{
											bool flag14 = this._anchoredNodeEnd.NextVisibleNode != null;
											if (flag14)
											{
												this.SelectNode(this._anchoredNodeEnd.NextVisibleNode);
												this.EnsureVisible();
											}
										}
									}
								}
							}
							bool flag15 = e.KeyCode == Keys.Left || e.KeyCode == Keys.Right;
							if (flag15)
							{
								bool flag16 = e.KeyCode == Keys.Left;
								if (flag16)
								{
									bool flag17 = this._anchoredNodeEnd.Expanded && this._anchoredNodeEnd.Nodes.Count > 0;
									if (flag17)
									{
										this._anchoredNodeEnd.Expanded = false;
									}
									else
									{
										bool flag18 = this._anchoredNodeEnd.ParentNode != null;
										if (flag18)
										{
											this.SelectNode(this._anchoredNodeEnd.ParentNode);
											this.EnsureVisible();
										}
									}
								}
								else
								{
									bool flag19 = e.KeyCode == Keys.Right;
									if (flag19)
									{
										bool flag20 = !this._anchoredNodeEnd.Expanded;
										if (flag20)
										{
											this._anchoredNodeEnd.Expanded = true;
										}
										else
										{
											bool flag21 = this._anchoredNodeEnd.Nodes.Count > 0;
											if (flag21)
											{
												this.SelectNode(this._anchoredNodeEnd.Nodes[0]);
												this.EnsureVisible();
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000DDA8 File Offset: 0x0000BFA8
		private void DragTimer_Tick(object sender, EventArgs e)
		{
			bool flag = !base.IsDragging;
			if (flag)
			{
				this.StopDrag();
			}
			else
			{
				bool flag2 = Control.MouseButtons != MouseButtons.Left;
				if (flag2)
				{
					this.StopDrag();
				}
				else
				{
					Point pos = base.PointToClient(Control.MousePosition);
					bool visible = this._vScrollBar.Visible;
					if (visible)
					{
						bool flag3 = pos.Y < base.ClientRectangle.Top;
						if (flag3)
						{
							int difference = (pos.Y - base.ClientRectangle.Top) * -1;
							bool flag4 = difference > this.ItemHeight;
							if (flag4)
							{
								difference = this.ItemHeight;
							}
							this._vScrollBar.Value = this._vScrollBar.Value - difference;
						}
						bool flag5 = pos.Y > base.ClientRectangle.Bottom;
						if (flag5)
						{
							int difference2 = pos.Y - base.ClientRectangle.Bottom;
							bool flag6 = difference2 > this.ItemHeight;
							if (flag6)
							{
								difference2 = this.ItemHeight;
							}
							this._vScrollBar.Value = this._vScrollBar.Value + difference2;
						}
					}
					bool visible2 = this._hScrollBar.Visible;
					if (visible2)
					{
						bool flag7 = pos.X < base.ClientRectangle.Left;
						if (flag7)
						{
							int difference3 = (pos.X - base.ClientRectangle.Left) * -1;
							bool flag8 = difference3 > this.ItemHeight;
							if (flag8)
							{
								difference3 = this.ItemHeight;
							}
							this._hScrollBar.Value = this._hScrollBar.Value - difference3;
						}
						bool flag9 = pos.X > base.ClientRectangle.Right;
						if (flag9)
						{
							int difference4 = pos.X - base.ClientRectangle.Right;
							bool flag10 = difference4 > this.ItemHeight;
							if (flag10)
							{
								difference4 = this.ItemHeight;
							}
							this._hScrollBar.Value = this._hScrollBar.Value + difference4;
						}
					}
				}
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		private void HookNodeEvents(DarkTreeNode node)
		{
			node.Nodes.ItemsAdded += this.ChildNodes_ItemsAdded;
			node.Nodes.ItemsRemoved += this.ChildNodes_ItemsRemoved;
			node.TextChanged += this.Nodes_TextChanged;
			node.NodeExpanded += this.Nodes_NodeExpanded;
			node.NodeCollapsed += this.Nodes_NodeCollapsed;
			foreach (DarkTreeNode childNode in node.Nodes)
			{
				this.HookNodeEvents(childNode);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E098 File Offset: 0x0000C298
		private void UnhookNodeEvents(DarkTreeNode node)
		{
			node.Nodes.ItemsAdded -= this.ChildNodes_ItemsAdded;
			node.Nodes.ItemsRemoved -= this.ChildNodes_ItemsRemoved;
			node.TextChanged -= this.Nodes_TextChanged;
			node.NodeExpanded -= this.Nodes_NodeExpanded;
			node.NodeCollapsed -= this.Nodes_NodeCollapsed;
			foreach (DarkTreeNode childNode in node.Nodes)
			{
				this.UnhookNodeEvents(childNode);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E158 File Offset: 0x0000C358
		private void UpdateNodes()
		{
			bool isDragging = base.IsDragging;
			if (!isDragging)
			{
				bool flag = this.Nodes.Count == 0;
				if (!flag)
				{
					int yOffset = 0;
					bool isOdd = false;
					int index = 0;
					DarkTreeNode prevNode = null;
					base.ContentSize = new Size(0, 0);
					for (int i = 0; i <= this.Nodes.Count - 1; i++)
					{
						DarkTreeNode node = this.Nodes[i];
						this.UpdateNode(node, ref prevNode, 0, ref yOffset, ref isOdd, ref index);
					}
					base.ContentSize = new Size(base.ContentSize.Width, yOffset);
					this.VisibleNodeCount = index;
					base.Invalidate();
				}
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E21C File Offset: 0x0000C41C
		private void UpdateNode(DarkTreeNode node, ref DarkTreeNode prevNode, int indent, ref int yOffset, ref bool isOdd, ref int index)
		{
			this.UpdateNodeBounds(node, yOffset, indent);
			yOffset += this.ItemHeight;
			node.Odd = isOdd;
			isOdd = !isOdd;
			node.VisibleIndex = index;
			index++;
			node.PrevVisibleNode = prevNode;
			bool flag = prevNode != null;
			if (flag)
			{
				prevNode.NextVisibleNode = node;
			}
			prevNode = node;
			bool expanded = node.Expanded;
			if (expanded)
			{
				foreach (DarkTreeNode childNode in node.Nodes)
				{
					this.UpdateNode(childNode, ref prevNode, indent + this.Indent, ref yOffset, ref isOdd, ref index);
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000E2EC File Offset: 0x0000C4EC
		private void UpdateNodeBounds(DarkTreeNode node, int yOffset, int indent)
		{
			int expandTop = yOffset + this.ItemHeight / 2 - this._expandAreaSize / 2;
			node.ExpandArea = new Rectangle(indent + 3, expandTop, this._expandAreaSize, this._expandAreaSize);
			int iconTop = yOffset + this.ItemHeight / 2 - this._iconSize / 2;
			bool showIcons = this.ShowIcons;
			if (showIcons)
			{
				node.IconArea = new Rectangle(node.ExpandArea.Right + 2, iconTop, this._iconSize, this._iconSize);
			}
			else
			{
				node.IconArea = new Rectangle(node.ExpandArea.Right, iconTop, 0, 0);
			}
			using (Graphics g = base.CreateGraphics())
			{
				int textSize = (int)g.MeasureString(node.Text, this.Font).Width;
				node.TextArea = new Rectangle(node.IconArea.Right + 2, yOffset, textSize + 1, this.ItemHeight);
			}
			node.FullArea = new Rectangle(indent, yOffset, node.TextArea.Right - indent, this.ItemHeight);
			bool flag = base.ContentSize.Width < node.TextArea.Right + 2;
			if (flag)
			{
				base.ContentSize = new Size(node.TextArea.Right + 2, base.ContentSize.Height);
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000E478 File Offset: 0x0000C678
		private void LoadIcons()
		{
			this.DisposeIcons();
			this._nodeClosed = TreeViewIcons.node_closed_empty.SetColor(Colors.LightText);
			this._nodeClosedHover = TreeViewIcons.node_closed_empty.SetColor(Colors.BlueHighlight);
			this._nodeClosedHoverSelected = TreeViewIcons.node_closed_full.SetColor(Colors.LightText);
			this._nodeOpen = TreeViewIcons.node_open.SetColor(Colors.LightText);
			this._nodeOpenHover = TreeViewIcons.node_open.SetColor(Colors.BlueHighlight);
			this._nodeOpenHoverSelected = TreeViewIcons.node_open_empty.SetColor(Colors.LightText);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000E50C File Offset: 0x0000C70C
		private void DisposeIcons()
		{
			bool flag = this._nodeClosed != null;
			if (flag)
			{
				this._nodeClosed.Dispose();
			}
			bool flag2 = this._nodeClosedHover != null;
			if (flag2)
			{
				this._nodeClosedHover.Dispose();
			}
			bool flag3 = this._nodeClosedHoverSelected != null;
			if (flag3)
			{
				this._nodeClosedHoverSelected.Dispose();
			}
			bool flag4 = this._nodeOpen != null;
			if (flag4)
			{
				this._nodeOpen.Dispose();
			}
			bool flag5 = this._nodeOpenHover != null;
			if (flag5)
			{
				this._nodeOpenHover.Dispose();
			}
			bool flag6 = this._nodeOpenHoverSelected != null;
			if (flag6)
			{
				this._nodeOpenHoverSelected.Dispose();
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		private void CheckHover()
		{
			bool flag = !base.ClientRectangle.Contains(base.PointToClient(Control.MousePosition));
			if (flag)
			{
				bool isDragging = base.IsDragging;
				if (isDragging)
				{
					bool flag2 = this._dropNode != null;
					if (flag2)
					{
						this._dropNode = null;
						base.Invalidate();
					}
				}
			}
			else
			{
				foreach (DarkTreeNode node in this.Nodes)
				{
					this.CheckNodeHover(node, base.OffsetMousePosition);
				}
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000E660 File Offset: 0x0000C860
		private void NodeMouseLeave(DarkTreeNode node)
		{
			node.ExpandAreaHot = false;
			foreach (DarkTreeNode childNode in node.Nodes)
			{
				this.NodeMouseLeave(childNode);
			}
			base.Invalidate();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		private void CheckNodeHover(DarkTreeNode node, Point location)
		{
			bool isDragging = base.IsDragging;
			if (isDragging)
			{
				bool flag = this.GetNodeFullRowArea(node).Contains(base.OffsetMousePosition);
				if (flag)
				{
					DarkTreeNode newDropNode = (this._dragNodes.Contains(node) ? null : node);
					bool flag2 = this._dropNode != newDropNode;
					if (flag2)
					{
						this._dropNode = newDropNode;
						base.Invalidate();
					}
				}
			}
			else
			{
				bool hot = node.ExpandArea.Contains(location);
				bool flag3 = node.ExpandAreaHot != hot;
				if (flag3)
				{
					node.ExpandAreaHot = hot;
					base.Invalidate();
				}
			}
			foreach (DarkTreeNode childNode in node.Nodes)
			{
				this.CheckNodeHover(childNode, location);
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000E7BC File Offset: 0x0000C9BC
		private void CheckNodeClick(DarkTreeNode node, Point location, MouseButtons button)
		{
			bool flag = this.GetNodeFullRowArea(node).Contains(location);
			if (flag)
			{
				bool flag2 = node.ExpandArea.Contains(location);
				if (flag2)
				{
					bool flag3 = button == MouseButtons.Left;
					if (flag3)
					{
						node.Expanded = !node.Expanded;
					}
				}
				else
				{
					bool flag4 = button == MouseButtons.Left;
					if (flag4)
					{
						bool flag5 = this.MultiSelect && Control.ModifierKeys == Keys.Shift;
						if (flag5)
						{
							this.SelectAnchoredRange(node);
						}
						else
						{
							bool flag6 = this.MultiSelect && Control.ModifierKeys == Keys.Control;
							if (flag6)
							{
								this.ToggleNode(node);
							}
							else
							{
								bool flag7 = !this.SelectedNodes.Contains(node);
								if (flag7)
								{
									this.SelectNode(node);
								}
								this._dragPos = base.OffsetMousePosition;
								this._provisionalDragging = true;
								this._provisionalNode = node;
							}
						}
						return;
					}
					bool flag8 = button == MouseButtons.Right;
					if (flag8)
					{
						bool flag9 = this.MultiSelect && Control.ModifierKeys == Keys.Shift;
						if (flag9)
						{
							return;
						}
						bool flag10 = this.MultiSelect && Control.ModifierKeys == Keys.Control;
						if (flag10)
						{
							return;
						}
						bool flag11 = !this.SelectedNodes.Contains(node);
						if (flag11)
						{
							this.SelectNode(node);
						}
						return;
					}
				}
			}
			bool expanded = node.Expanded;
			if (expanded)
			{
				foreach (DarkTreeNode childNode in node.Nodes)
				{
					this.CheckNodeClick(childNode, location, button);
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000E98C File Offset: 0x0000CB8C
		private void CheckNodeDoubleClick(DarkTreeNode node, Point location)
		{
			bool flag = this.GetNodeFullRowArea(node).Contains(location);
			if (flag)
			{
				bool flag2 = !node.ExpandArea.Contains(location);
				if (flag2)
				{
					node.Expanded = !node.Expanded;
				}
			}
			else
			{
				bool expanded = node.Expanded;
				if (expanded)
				{
					foreach (DarkTreeNode childNode in node.Nodes)
					{
						this.CheckNodeDoubleClick(childNode, location);
					}
				}
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000EA34 File Offset: 0x0000CC34
		public void SelectNode(DarkTreeNode node)
		{
			this._selectedNodes.Clear();
			this._selectedNodes.Add(node);
			this._anchoredNodeStart = node;
			this._anchoredNodeEnd = node;
			base.Invalidate();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000EA68 File Offset: 0x0000CC68
		public void SelectNodes(DarkTreeNode startNode, DarkTreeNode endNode)
		{
			List<DarkTreeNode> nodes = new List<DarkTreeNode>();
			bool flag = startNode == endNode;
			if (flag)
			{
				nodes.Add(startNode);
			}
			bool flag2 = startNode.VisibleIndex < endNode.VisibleIndex;
			if (flag2)
			{
				DarkTreeNode node = startNode;
				nodes.Add(node);
				while (node != endNode && node != null)
				{
					node = node.NextVisibleNode;
					nodes.Add(node);
				}
			}
			else
			{
				bool flag3 = startNode.VisibleIndex > endNode.VisibleIndex;
				if (flag3)
				{
					DarkTreeNode node2 = startNode;
					nodes.Add(node2);
					while (node2 != endNode && node2 != null)
					{
						node2 = node2.PrevVisibleNode;
						nodes.Add(node2);
					}
				}
			}
			this.SelectNodes(nodes, false);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000EB24 File Offset: 0x0000CD24
		public void SelectNodes(List<DarkTreeNode> nodes, bool updateAnchors = true)
		{
			this._selectedNodes.Clear();
			foreach (DarkTreeNode node in nodes)
			{
				this._selectedNodes.Add(node);
			}
			bool flag = updateAnchors && this._selectedNodes.Count > 0;
			if (flag)
			{
				this._anchoredNodeStart = this._selectedNodes[this._selectedNodes.Count - 1];
				this._anchoredNodeEnd = this._selectedNodes[this._selectedNodes.Count - 1];
			}
			base.Invalidate();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
		private void SelectAnchoredRange(DarkTreeNode node)
		{
			this._anchoredNodeEnd = node;
			this.SelectNodes(this._anchoredNodeStart, this._anchoredNodeEnd);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000EC04 File Offset: 0x0000CE04
		public void ToggleNode(DarkTreeNode node)
		{
			bool flag = this._selectedNodes.Contains(node);
			if (flag)
			{
				this._selectedNodes.Remove(node);
				bool flag2 = this._anchoredNodeStart == node && this._anchoredNodeEnd == node;
				if (flag2)
				{
					bool flag3 = this._selectedNodes.Count > 0;
					if (flag3)
					{
						this._anchoredNodeStart = this._selectedNodes[0];
						this._anchoredNodeEnd = this._selectedNodes[0];
					}
					else
					{
						this._anchoredNodeStart = null;
						this._anchoredNodeEnd = null;
					}
				}
				bool flag4 = this._anchoredNodeStart == node;
				if (flag4)
				{
					bool flag5 = this._anchoredNodeEnd.VisibleIndex < node.VisibleIndex;
					if (flag5)
					{
						this._anchoredNodeStart = node.PrevVisibleNode;
					}
					else
					{
						bool flag6 = this._anchoredNodeEnd.VisibleIndex > node.VisibleIndex;
						if (flag6)
						{
							this._anchoredNodeStart = node.NextVisibleNode;
						}
						else
						{
							this._anchoredNodeStart = this._anchoredNodeEnd;
						}
					}
				}
				bool flag7 = this._anchoredNodeEnd == node;
				if (flag7)
				{
					bool flag8 = this._anchoredNodeStart.VisibleIndex < node.VisibleIndex;
					if (flag8)
					{
						this._anchoredNodeEnd = node.PrevVisibleNode;
					}
					else
					{
						bool flag9 = this._anchoredNodeStart.VisibleIndex > node.VisibleIndex;
						if (flag9)
						{
							this._anchoredNodeEnd = node.NextVisibleNode;
						}
						else
						{
							this._anchoredNodeEnd = this._anchoredNodeStart;
						}
					}
				}
			}
			else
			{
				this._selectedNodes.Add(node);
				this._anchoredNodeStart = node;
				this._anchoredNodeEnd = node;
			}
			base.Invalidate();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000ED94 File Offset: 0x0000CF94
		public Rectangle GetNodeFullRowArea(DarkTreeNode node)
		{
			bool flag = node.ParentNode != null && !node.ParentNode.Expanded;
			Rectangle rectangle;
			if (flag)
			{
				rectangle = new Rectangle(-1, -1, -1, -1);
			}
			else
			{
				int width = Math.Max(base.ContentSize.Width, base.Viewport.Width);
				Rectangle rect = new Rectangle(0, node.FullArea.Top, width, this.ItemHeight);
				rectangle = rect;
			}
			return rectangle;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000EE14 File Offset: 0x0000D014
		public void EnsureVisible()
		{
			bool flag = this.SelectedNodes.Count == 0;
			if (!flag)
			{
				foreach (DarkTreeNode node in this.SelectedNodes)
				{
					node.EnsureVisible();
				}
				bool flag2 = !this.MultiSelect;
				int itemTop;
				if (flag2)
				{
					itemTop = this.SelectedNodes[0].FullArea.Top;
				}
				else
				{
					itemTop = this._anchoredNodeEnd.FullArea.Top;
				}
				int itemBottom = itemTop + this.ItemHeight;
				bool flag3 = itemTop < base.Viewport.Top;
				if (flag3)
				{
					base.VScrollTo(itemTop);
				}
				bool flag4 = itemBottom > base.Viewport.Bottom;
				if (flag4)
				{
					base.VScrollTo(itemBottom - base.Viewport.Height);
				}
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public void Sort()
		{
			bool flag = this.TreeViewNodeSorter == null;
			if (!flag)
			{
				this.Nodes.Sort(this.TreeViewNodeSorter);
				foreach (DarkTreeNode node in this.Nodes)
				{
					this.SortChildNodes(node);
				}
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000EF94 File Offset: 0x0000D194
		private void SortChildNodes(DarkTreeNode node)
		{
			node.Nodes.Sort(this.TreeViewNodeSorter);
			foreach (DarkTreeNode childNode in node.Nodes)
			{
				this.SortChildNodes(childNode);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000F000 File Offset: 0x0000D200
		public DarkTreeNode FindNode(string path)
		{
			foreach (DarkTreeNode node in this.Nodes)
			{
				DarkTreeNode compNode = this.FindNode(node, path, true);
				bool flag = compNode != null;
				if (flag)
				{
					return compNode;
				}
			}
			return null;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000F070 File Offset: 0x0000D270
		private DarkTreeNode FindNode(DarkTreeNode parentNode, string path, bool recursive = true)
		{
			bool flag = parentNode.FullPath == path;
			DarkTreeNode darkTreeNode;
			if (flag)
			{
				darkTreeNode = parentNode;
			}
			else
			{
				foreach (DarkTreeNode node in parentNode.Nodes)
				{
					bool flag2 = node.FullPath == path;
					if (flag2)
					{
						return node;
					}
					if (recursive)
					{
						DarkTreeNode compNode = this.FindNode(node, path, true);
						bool flag3 = compNode != null;
						if (flag3)
						{
							return compNode;
						}
					}
				}
				darkTreeNode = null;
			}
			return darkTreeNode;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000F114 File Offset: 0x0000D314
		protected override void StartDrag()
		{
			bool flag = !this.AllowMoveNodes;
			if (flag)
			{
				this._provisionalDragging = false;
			}
			else
			{
				this._dragNodes = new List<DarkTreeNode>();
				foreach (DarkTreeNode node in this.SelectedNodes)
				{
					this._dragNodes.Add(node);
				}
				foreach (DarkTreeNode node2 in this._dragNodes.ToList<DarkTreeNode>())
				{
					bool flag2 = node2.ParentNode == null;
					if (!flag2)
					{
						bool flag3 = this._dragNodes.Contains(node2.ParentNode);
						if (flag3)
						{
							this._dragNodes.Remove(node2);
						}
					}
				}
				this._provisionalDragging = false;
				this.Cursor = Cursors.SizeAll;
				base.StartDrag();
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F228 File Offset: 0x0000D428
		private void HandleDrag()
		{
			bool flag = !this.AllowMoveNodes;
			if (!flag)
			{
				DarkTreeNode dropNode = this._dropNode;
				bool flag2 = dropNode == null;
				if (flag2)
				{
					bool flag3 = this.Cursor != Cursors.No;
					if (flag3)
					{
						this.Cursor = Cursors.No;
					}
				}
				else
				{
					bool flag4 = this.ForceDropToParent(dropNode);
					if (flag4)
					{
						dropNode = dropNode.ParentNode;
					}
					bool flag5 = !this.CanMoveNodes(this._dragNodes, dropNode, false);
					if (flag5)
					{
						bool flag6 = this.Cursor != Cursors.No;
						if (flag6)
						{
							this.Cursor = Cursors.No;
						}
					}
					else
					{
						bool flag7 = this.Cursor != Cursors.SizeAll;
						if (flag7)
						{
							this.Cursor = Cursors.SizeAll;
						}
					}
				}
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		private void HandleDrop()
		{
			bool flag = !this.AllowMoveNodes;
			if (!flag)
			{
				DarkTreeNode dropNode = this._dropNode;
				bool flag2 = dropNode == null;
				if (flag2)
				{
					this.StopDrag();
				}
				else
				{
					bool flag3 = this.ForceDropToParent(dropNode);
					if (flag3)
					{
						dropNode = dropNode.ParentNode;
					}
					bool flag4 = this.CanMoveNodes(this._dragNodes, dropNode, true);
					if (flag4)
					{
						List<DarkTreeNode> cachedSelectedNodes = this.SelectedNodes.ToList<DarkTreeNode>();
						this.MoveNodes(this._dragNodes, dropNode);
						foreach (DarkTreeNode node in this._dragNodes)
						{
							bool flag5 = node.ParentNode == null;
							if (flag5)
							{
								this.Nodes.Remove(node);
							}
							else
							{
								node.ParentNode.Nodes.Remove(node);
							}
							dropNode.Nodes.Add(node);
						}
						bool flag6 = this.TreeViewNodeSorter != null;
						if (flag6)
						{
							dropNode.Nodes.Sort(this.TreeViewNodeSorter);
						}
						dropNode.Expanded = true;
						this.NodesMoved(this._dragNodes);
						foreach (DarkTreeNode node2 in cachedSelectedNodes)
						{
							this._selectedNodes.Add(node2);
						}
					}
					this.StopDrag();
					this.UpdateNodes();
				}
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F484 File Offset: 0x0000D684
		protected override void StopDrag()
		{
			this._dragNodes = null;
			this._dropNode = null;
			this.Cursor = Cursors.Default;
			base.Invalidate();
			base.StopDrag();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		protected virtual bool ForceDropToParent(DarkTreeNode node)
		{
			return false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		protected virtual bool CanMoveNodes(List<DarkTreeNode> dragNodes, DarkTreeNode dropNode, bool isMoving = false)
		{
			bool flag = dropNode == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				foreach (DarkTreeNode node in dragNodes)
				{
					bool flag3 = node == dropNode;
					if (flag3)
					{
						if (isMoving)
						{
							DarkMessageBox.ShowError("Cannot move " + node.Text + ". The destination folder is the same as the source folder.", Application.ProductName, DarkDialogButton.Ok);
						}
						return false;
					}
					bool flag4 = node.ParentNode != null && node.ParentNode == dropNode;
					if (flag4)
					{
						if (isMoving)
						{
							DarkMessageBox.ShowError("Cannot move " + node.Text + ". The destination folder is the same as the source folder.", Application.ProductName, DarkDialogButton.Ok);
						}
						return false;
					}
					for (DarkTreeNode parentNode = dropNode.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
					{
						bool flag5 = node == parentNode;
						if (flag5)
						{
							if (isMoving)
							{
								DarkMessageBox.ShowError("Cannot move " + node.Text + ". The destination folder is a subfolder of the source folder.", Application.ProductName, DarkDialogButton.Ok);
							}
							return false;
						}
					}
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000043C9 File Offset: 0x000025C9
		protected virtual void MoveNodes(List<DarkTreeNode> dragNodes, DarkTreeNode dropNode)
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000043C9 File Offset: 0x000025C9
		protected virtual void NodesMoved(List<DarkTreeNode> nodesMoved)
		{
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F604 File Offset: 0x0000D804
		protected override void PaintContent(Graphics g)
		{
			foreach (DarkTreeNode node in this.Nodes)
			{
				this.DrawNode(node, g);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F660 File Offset: 0x0000D860
		private void DrawNode(DarkTreeNode node, Graphics g)
		{
			Rectangle rect = this.GetNodeFullRowArea(node);
			Color bgColor = (node.Odd ? Colors.HeaderBackground : Colors.GreyBackground);
			bool flag = this.SelectedNodes.Count > 0 && this.SelectedNodes.Contains(node);
			if (flag)
			{
				bgColor = (this.Focused ? Colors.BlueSelection : Colors.GreySelection);
			}
			bool flag2 = base.IsDragging && this._dropNode == node;
			if (flag2)
			{
				bgColor = (this.Focused ? Colors.BlueSelection : Colors.GreySelection);
			}
			using (SolidBrush b = new SolidBrush(bgColor))
			{
				g.FillRectangle(b, rect);
			}
			bool flag3 = node.Nodes.Count > 0;
			if (flag3)
			{
				Point pos = new Point(node.ExpandArea.Location.X - 1, node.ExpandArea.Location.Y - 1);
				Bitmap icon = this._nodeOpen;
				bool flag4 = node.Expanded && !node.ExpandAreaHot;
				if (flag4)
				{
					icon = this._nodeOpen;
				}
				else
				{
					bool flag5 = node.Expanded && node.ExpandAreaHot && !this.SelectedNodes.Contains(node);
					if (flag5)
					{
						icon = this._nodeOpenHover;
					}
					else
					{
						bool flag6 = node.Expanded && node.ExpandAreaHot && this.SelectedNodes.Contains(node);
						if (flag6)
						{
							icon = this._nodeOpenHoverSelected;
						}
						else
						{
							bool flag7 = !node.Expanded && !node.ExpandAreaHot;
							if (flag7)
							{
								icon = this._nodeClosed;
							}
							else
							{
								bool flag8 = !node.Expanded && node.ExpandAreaHot && !this.SelectedNodes.Contains(node);
								if (flag8)
								{
									icon = this._nodeClosedHover;
								}
								else
								{
									bool flag9 = !node.Expanded && node.ExpandAreaHot && this.SelectedNodes.Contains(node);
									if (flag9)
									{
										icon = this._nodeClosedHoverSelected;
									}
								}
							}
						}
					}
				}
				g.DrawImageUnscaled(icon, pos);
			}
			bool flag10 = this.ShowIcons && node.Icon != null;
			if (flag10)
			{
				bool flag11 = node.Expanded && node.ExpandedIcon != null;
				if (flag11)
				{
					g.DrawImageUnscaled(node.ExpandedIcon, node.IconArea.Location);
				}
				else
				{
					g.DrawImageUnscaled(node.Icon, node.IconArea.Location);
				}
			}
			using (SolidBrush b2 = new SolidBrush(Colors.LightText))
			{
				StringFormat stringFormat = new StringFormat
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center
				};
				g.DrawString(node.Text, this.Font, b2, node.TextArea, stringFormat);
			}
			bool expanded = node.Expanded;
			if (expanded)
			{
				foreach (DarkTreeNode childNode in node.Nodes)
				{
					this.DrawNode(childNode, g);
				}
			}
		}

		// Token: 0x040001C5 RID: 453
		private bool _disposed;

		// Token: 0x040001C6 RID: 454
		private readonly int _expandAreaSize = 16;

		// Token: 0x040001C7 RID: 455
		private readonly int _iconSize = 16;

		// Token: 0x040001C8 RID: 456
		private int _itemHeight = 20;

		// Token: 0x040001C9 RID: 457
		private int _indent = 20;

		// Token: 0x040001CA RID: 458
		private ObservableList<DarkTreeNode> _nodes;

		// Token: 0x040001CB RID: 459
		private ObservableCollection<DarkTreeNode> _selectedNodes;

		// Token: 0x040001CC RID: 460
		private DarkTreeNode _anchoredNodeStart;

		// Token: 0x040001CD RID: 461
		private DarkTreeNode _anchoredNodeEnd;

		// Token: 0x040001CE RID: 462
		private Bitmap _nodeClosed;

		// Token: 0x040001CF RID: 463
		private Bitmap _nodeClosedHover;

		// Token: 0x040001D0 RID: 464
		private Bitmap _nodeClosedHoverSelected;

		// Token: 0x040001D1 RID: 465
		private Bitmap _nodeOpen;

		// Token: 0x040001D2 RID: 466
		private Bitmap _nodeOpenHover;

		// Token: 0x040001D3 RID: 467
		private Bitmap _nodeOpenHoverSelected;

		// Token: 0x040001D4 RID: 468
		private DarkTreeNode _provisionalNode;

		// Token: 0x040001D5 RID: 469
		private DarkTreeNode _dropNode;

		// Token: 0x040001D6 RID: 470
		private bool _provisionalDragging;

		// Token: 0x040001D7 RID: 471
		private List<DarkTreeNode> _dragNodes;

		// Token: 0x040001D8 RID: 472
		private Point _dragPos;
	}
}
