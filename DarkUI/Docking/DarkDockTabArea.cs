using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Controls;

namespace DarkUI.Docking
{
	// Token: 0x02000023 RID: 35
	internal class DarkDockTabArea
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000095CC File Offset: 0x000077CC
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000095D4 File Offset: 0x000077D4
		public DarkDockArea DockArea { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000095DD File Offset: 0x000077DD
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000095E5 File Offset: 0x000077E5
		public Rectangle ClientRectangle { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600012E RID: 302 RVA: 0x000095EE File Offset: 0x000077EE
		// (set) Token: 0x0600012F RID: 303 RVA: 0x000095F6 File Offset: 0x000077F6
		public Rectangle DropdownRectangle { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000095FF File Offset: 0x000077FF
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00009607 File Offset: 0x00007807
		public bool DropdownHot { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00009610 File Offset: 0x00007810
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00009618 File Offset: 0x00007818
		public int Offset { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00009621 File Offset: 0x00007821
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00009629 File Offset: 0x00007829
		public int TotalTabSize { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00009632 File Offset: 0x00007832
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000963A File Offset: 0x0000783A
		public bool Visible { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00009643 File Offset: 0x00007843
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000964B File Offset: 0x0000784B
		public DarkDockTab ClickedCloseButton { get; set; }

		// Token: 0x0600013A RID: 314 RVA: 0x00009654 File Offset: 0x00007854
		public DarkDockTabArea(DarkDockArea dockArea)
		{
			this.DockArea = dockArea;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009687 File Offset: 0x00007887
		public void ShowMenu(Control control, Point location)
		{
			this._tabMenu.Show(control, location);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009698 File Offset: 0x00007898
		public void AddMenuItem(ToolStripMenuItem menuItem)
		{
			this._menuItems.Add(menuItem);
			this.RebuildMenu();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000096AF File Offset: 0x000078AF
		public void RemoveMenuItem(ToolStripMenuItem menuItem)
		{
			this._menuItems.Remove(menuItem);
			this.RebuildMenu();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000096C8 File Offset: 0x000078C8
		public ToolStripMenuItem GetMenuItem(DarkDockContent content)
		{
			ToolStripMenuItem menuItem = null;
			foreach (ToolStripMenuItem item in this._menuItems)
			{
				DarkDockContent menuContent = item.Tag as DarkDockContent;
				bool flag = menuContent == null;
				if (!flag)
				{
					bool flag2 = menuContent == content;
					if (flag2)
					{
						menuItem = item;
					}
				}
			}
			return menuItem;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009748 File Offset: 0x00007948
		public void RebuildMenu()
		{
			this._tabMenu.Items.Clear();
			List<ToolStripMenuItem> orderedItems = new List<ToolStripMenuItem>();
			int index = 0;
			for (int i = 0; i < this._menuItems.Count; i++)
			{
				foreach (ToolStripMenuItem item in this._menuItems)
				{
					DarkDockContent content = (DarkDockContent)item.Tag;
					bool flag = content.Order == index;
					if (flag)
					{
						orderedItems.Add(item);
					}
				}
				index++;
			}
			foreach (ToolStripMenuItem item2 in orderedItems)
			{
				this._tabMenu.Items.Add(item2);
			}
		}

		// Token: 0x04000171 RID: 369
		private Dictionary<DarkDockContent, DarkDockTab> _tabs = new Dictionary<DarkDockContent, DarkDockTab>();

		// Token: 0x04000172 RID: 370
		private List<ToolStripMenuItem> _menuItems = new List<ToolStripMenuItem>();

		// Token: 0x04000173 RID: 371
		private DarkContextMenu _tabMenu = new DarkContextMenu();
	}
}
