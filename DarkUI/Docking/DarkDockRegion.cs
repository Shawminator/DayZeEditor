using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Docking
{
	// Token: 0x0200001C RID: 28
	[ToolboxItem(false)]
	public class DarkDockRegion : Panel
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00007F28 File Offset: 0x00006128
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00007F30 File Offset: 0x00006130
		public DarkDockPanel DockPanel { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00007F39 File Offset: 0x00006139
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00007F41 File Offset: 0x00006141
		public DarkDockArea DockArea { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00007F4C File Offset: 0x0000614C
		public DarkDockContent ActiveDocument
		{
			get
			{
				bool flag = this.DockArea != DarkDockArea.Document || this._groups.Count == 0;
				DarkDockContent darkDockContent;
				if (flag)
				{
					darkDockContent = null;
				}
				else
				{
					darkDockContent = this._groups[0].VisibleContent;
				}
				return darkDockContent;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00007F94 File Offset: 0x00006194
		public List<DarkDockGroup> Groups
		{
			get
			{
				return this._groups.ToList<DarkDockGroup>();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007FB1 File Offset: 0x000061B1
		public DarkDockRegion(DarkDockPanel dockPanel, DarkDockArea dockArea)
		{
			this._groups = new List<DarkDockGroup>();
			this.DockPanel = dockPanel;
			this.DockArea = dockArea;
			this.BuildProperties();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007FDD File Offset: 0x000061DD
		internal void AddContent(DarkDockContent dockContent)
		{
			this.AddContent(dockContent, null);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007FEC File Offset: 0x000061EC
		internal void AddContent(DarkDockContent dockContent, DarkDockGroup dockGroup)
		{
			bool flag = dockGroup == null;
			if (flag)
			{
				bool flag2 = this.DockArea == DarkDockArea.Document && this._groups.Count > 0;
				if (flag2)
				{
					dockGroup = this._groups[0];
				}
				else
				{
					dockGroup = this.CreateGroup();
				}
			}
			dockContent.DockRegion = this;
			dockGroup.AddContent(dockContent);
			bool flag3 = !base.Visible;
			if (flag3)
			{
				base.Visible = true;
				this.CreateSplitter();
			}
			this.PositionGroups();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00008070 File Offset: 0x00006270
		internal void InsertContent(DarkDockContent dockContent, DarkDockGroup dockGroup, DockInsertType insertType)
		{
			int order = dockGroup.Order;
			bool flag = insertType == DockInsertType.After;
			if (flag)
			{
				order++;
			}
			DarkDockGroup newGroup = this.InsertGroup(order);
			dockContent.DockRegion = this;
			newGroup.AddContent(dockContent);
			bool flag2 = !base.Visible;
			if (flag2)
			{
				base.Visible = true;
				this.CreateSplitter();
			}
			this.PositionGroups();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000080D0 File Offset: 0x000062D0
		internal void RemoveContent(DarkDockContent dockContent)
		{
			dockContent.DockRegion = null;
			DarkDockGroup group = dockContent.DockGroup;
			group.RemoveContent(dockContent);
			dockContent.DockArea = DarkDockArea.None;
			bool flag = group.ContentCount == 0;
			if (flag)
			{
				this.RemoveGroup(group);
			}
			bool flag2 = this._groups.Count == 0 && this.DockArea != DarkDockArea.Document;
			if (flag2)
			{
				base.Visible = false;
				this.RemoveSplitter();
			}
			this.PositionGroups();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000814C File Offset: 0x0000634C
		public List<DarkDockContent> GetContents()
		{
			List<DarkDockContent> result = new List<DarkDockContent>();
			foreach (DarkDockGroup group in this._groups)
			{
				result.AddRange(group.GetContents());
			}
			return result;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000081B4 File Offset: 0x000063B4
		private DarkDockGroup CreateGroup()
		{
			int order = 0;
			bool flag = this._groups.Count >= 1;
			if (flag)
			{
				order = -1;
				foreach (DarkDockGroup group in this._groups)
				{
					bool flag2 = group.Order >= order;
					if (flag2)
					{
						order = group.Order + 1;
					}
				}
			}
			DarkDockGroup newGroup = new DarkDockGroup(this.DockPanel, this, order);
			this._groups.Add(newGroup);
			base.Controls.Add(newGroup);
			return newGroup;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008270 File Offset: 0x00006470
		private DarkDockGroup InsertGroup(int order)
		{
			foreach (DarkDockGroup group in this._groups)
			{
				bool flag = group.Order >= order;
				if (flag)
				{
					DarkDockGroup darkDockGroup = group;
					int order2 = darkDockGroup.Order;
					darkDockGroup.Order = order2 + 1;
				}
			}
			DarkDockGroup newGroup = new DarkDockGroup(this.DockPanel, this, order);
			this._groups.Add(newGroup);
			base.Controls.Add(newGroup);
			return newGroup;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00008314 File Offset: 0x00006514
		private void RemoveGroup(DarkDockGroup group)
		{
			int lastOrder = group.Order;
			this._groups.Remove(group);
			base.Controls.Remove(group);
			foreach (DarkDockGroup otherGroup in this._groups)
			{
				bool flag = otherGroup.Order > lastOrder;
				if (flag)
				{
					DarkDockGroup darkDockGroup = otherGroup;
					int order = darkDockGroup.Order;
					darkDockGroup.Order = order - 1;
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000083A8 File Offset: 0x000065A8
		private void PositionGroups()
		{
			DockStyle dockStyle;
			switch (this.DockArea)
			{
			default:
				dockStyle = DockStyle.Fill;
				break;
			case DarkDockArea.Left:
			case DarkDockArea.Right:
				dockStyle = DockStyle.Top;
				break;
			case DarkDockArea.Bottom:
				dockStyle = DockStyle.Left;
				break;
			}
			bool flag = this._groups.Count == 1;
			if (flag)
			{
				this._groups[0].Dock = DockStyle.Fill;
			}
			else
			{
				bool flag2 = this._groups.Count > 1;
				if (flag2)
				{
					DarkDockGroup lastGroup = this._groups.OrderByDescending((DarkDockGroup g) => g.Order).First<DarkDockGroup>();
					foreach (DarkDockGroup group in this._groups.OrderByDescending((DarkDockGroup g) => g.Order))
					{
						group.SendToBack();
						bool flag3 = group.Order == lastGroup.Order;
						if (flag3)
						{
							group.Dock = DockStyle.Fill;
						}
						else
						{
							group.Dock = dockStyle;
						}
					}
					this.SizeGroups();
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000084FC File Offset: 0x000066FC
		private void SizeGroups()
		{
			bool flag = this._groups.Count <= 1;
			if (!flag)
			{
				Size size = new Size(0, 0);
				switch (this.DockArea)
				{
				default:
					return;
				case DarkDockArea.Left:
				case DarkDockArea.Right:
					size = new Size(base.ClientRectangle.Width, base.ClientRectangle.Height / this._groups.Count);
					break;
				case DarkDockArea.Bottom:
					size = new Size(base.ClientRectangle.Width / this._groups.Count, base.ClientRectangle.Height);
					break;
				}
				foreach (DarkDockGroup group in this._groups)
				{
					group.Size = size;
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00008608 File Offset: 0x00006808
		private void BuildProperties()
		{
			this.MinimumSize = new Size(50, 50);
			switch (this.DockArea)
			{
			default:
				this.Dock = DockStyle.Fill;
				base.Padding = new Padding(0, 1, 0, 0);
				break;
			case DarkDockArea.Left:
				this.Dock = DockStyle.Left;
				base.Padding = new Padding(0, 0, 1, 0);
				base.Visible = false;
				break;
			case DarkDockArea.Right:
				this.Dock = DockStyle.Right;
				base.Padding = new Padding(1, 0, 0, 0);
				base.Visible = false;
				break;
			case DarkDockArea.Bottom:
				this.Dock = DockStyle.Bottom;
				base.Padding = new Padding(0, 0, 0, 0);
				base.Visible = false;
				break;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000086CC File Offset: 0x000068CC
		private void CreateSplitter()
		{
			bool flag = this._splitter != null && this.DockPanel.Splitters.Contains(this._splitter);
			if (flag)
			{
				this.DockPanel.Splitters.Remove(this._splitter);
			}
			switch (this.DockArea)
			{
			case DarkDockArea.Left:
				this._splitter = new DarkDockSplitter(this.DockPanel, this, DarkSplitterType.Right);
				break;
			case DarkDockArea.Right:
				this._splitter = new DarkDockSplitter(this.DockPanel, this, DarkSplitterType.Left);
				break;
			case DarkDockArea.Bottom:
				this._splitter = new DarkDockSplitter(this.DockPanel, this, DarkSplitterType.Top);
				break;
			default:
				return;
			}
			this.DockPanel.Splitters.Add(this._splitter);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008790 File Offset: 0x00006990
		private void RemoveSplitter()
		{
			bool flag = this.DockPanel.Splitters.Contains(this._splitter);
			if (flag)
			{
				this.DockPanel.Splitters.Remove(this._splitter);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000087CF File Offset: 0x000069CF
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			this._parentForm = base.FindForm();
			this._parentForm.ResizeEnd += this.ParentForm_ResizeEnd;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000087FD File Offset: 0x000069FD
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			this.SizeGroups();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00008810 File Offset: 0x00006A10
		private void ParentForm_ResizeEnd(object sender, EventArgs e)
		{
			bool flag = this._splitter != null;
			if (flag)
			{
				this._splitter.UpdateBounds();
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008838 File Offset: 0x00006A38
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			bool flag = this._splitter != null;
			if (flag)
			{
				this._splitter.UpdateBounds();
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008868 File Offset: 0x00006A68
		public void Redraw()
		{
			base.Invalidate();
			foreach (DarkDockGroup group in this._groups)
			{
				group.Redraw();
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000088C8 File Offset: 0x00006AC8
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			bool flag = !base.Visible;
			if (!flag)
			{
				using (SolidBrush b = new SolidBrush(Colors.GreyBackground))
				{
					g.FillRectangle(b, base.ClientRectangle);
				}
				using (Pen p = new Pen(Colors.DarkBorder))
				{
					bool flag2 = this.DockArea == DarkDockArea.Document;
					if (flag2)
					{
						g.DrawLine(p, base.ClientRectangle.Left, 0, base.ClientRectangle.Right, 0);
					}
					bool flag3 = this.DockArea == DarkDockArea.Right;
					if (flag3)
					{
						g.DrawLine(p, base.ClientRectangle.Left, 0, base.ClientRectangle.Left, base.ClientRectangle.Height);
					}
					bool flag4 = this.DockArea == DarkDockArea.Left;
					if (flag4)
					{
						g.DrawLine(p, base.ClientRectangle.Right - 1, 0, base.ClientRectangle.Right - 1, base.ClientRectangle.Height);
					}
				}
			}
		}

		// Token: 0x04000154 RID: 340
		private List<DarkDockGroup> _groups;

		// Token: 0x04000155 RID: 341
		private Form _parentForm;

		// Token: 0x04000156 RID: 342
		private DarkDockSplitter _splitter;
	}
}
