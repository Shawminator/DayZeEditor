using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Docking
{
	// Token: 0x0200001A RID: 26
	[ToolboxItem(false)]
	public class DarkDockGroup : Panel
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000593E File Offset: 0x00003B3E
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00005946 File Offset: 0x00003B46
		public DarkDockPanel DockPanel { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000594F File Offset: 0x00003B4F
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00005957 File Offset: 0x00003B57
		public DarkDockRegion DockRegion { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005960 File Offset: 0x00003B60
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00005968 File Offset: 0x00003B68
		public DarkDockArea DockArea { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005971 File Offset: 0x00003B71
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00005979 File Offset: 0x00003B79
		public DarkDockContent VisibleContent { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005982 File Offset: 0x00003B82
		// (set) Token: 0x060000AF RID: 175 RVA: 0x0000598A File Offset: 0x00003B8A
		public int Order { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005994 File Offset: 0x00003B94
		public int ContentCount
		{
			get
			{
				return this._contents.Count;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000059B4 File Offset: 0x00003BB4
		public DarkDockGroup(DarkDockPanel dockPanel, DarkDockRegion dockRegion, int order)
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			this.DockPanel = dockPanel;
			this.DockRegion = dockRegion;
			this.DockArea = dockRegion.DockArea;
			this.Order = order;
			this._tabArea = new DarkDockTabArea(this.DockArea);
			this.DockPanel.ActiveContentChanged += this.DockPanel_ActiveContentChanged;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005A44 File Offset: 0x00003C44
		public void AddContent(DarkDockContent dockContent)
		{
			dockContent.DockGroup = this;
			dockContent.Dock = DockStyle.Fill;
			dockContent.Order = 0;
			bool flag = this._contents.Count > 0;
			if (flag)
			{
				int order = -1;
				foreach (DarkDockContent otherContent in this._contents)
				{
					bool flag2 = otherContent.Order >= order;
					if (flag2)
					{
						order = otherContent.Order + 1;
					}
				}
				dockContent.Order = order;
			}
			this._contents.Add(dockContent);
			base.Controls.Add(dockContent);
			dockContent.DockTextChanged += this.DockContent_DockTextChanged;
			this._tabs.Add(dockContent, new DarkDockTab(dockContent));
			bool flag3 = this.VisibleContent == null;
			if (flag3)
			{
				dockContent.Visible = true;
				this.VisibleContent = dockContent;
			}
			else
			{
				dockContent.Visible = false;
			}
			ToolStripMenuItem menuItem = new ToolStripMenuItem(dockContent.DockText);
			menuItem.Tag = dockContent;
			menuItem.Click += this.TabMenuItem_Select;
			menuItem.Image = dockContent.Icon;
			this._tabArea.AddMenuItem(menuItem);
			this.UpdateTabArea();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005BA0 File Offset: 0x00003DA0
		public void RemoveContent(DarkDockContent dockContent)
		{
			dockContent.DockGroup = null;
			int order = dockContent.Order;
			this._contents.Remove(dockContent);
			base.Controls.Remove(dockContent);
			foreach (DarkDockContent otherContent in this._contents)
			{
				bool flag = otherContent.Order > order;
				if (flag)
				{
					DarkDockContent darkDockContent = otherContent;
					int order2 = darkDockContent.Order;
					darkDockContent.Order = order2 - 1;
				}
			}
			dockContent.DockTextChanged -= this.DockContent_DockTextChanged;
			bool flag2 = this._tabs.ContainsKey(dockContent);
			if (flag2)
			{
				this._tabs.Remove(dockContent);
			}
			bool flag3 = this.VisibleContent == dockContent;
			if (flag3)
			{
				this.VisibleContent = null;
				bool flag4 = this._contents.Count > 0;
				if (flag4)
				{
					DarkDockContent newContent = this._contents[0];
					newContent.Visible = true;
					this.VisibleContent = newContent;
				}
			}
			ToolStripMenuItem menuItem = this._tabArea.GetMenuItem(dockContent);
			menuItem.Click -= this.TabMenuItem_Select;
			this._tabArea.RemoveMenuItem(menuItem);
			this.UpdateTabArea();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public List<DarkDockContent> GetContents()
		{
			return this._contents.OrderBy((DarkDockContent c) => c.Order).ToList<DarkDockContent>();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005D38 File Offset: 0x00003F38
		private void UpdateTabArea()
		{
			bool flag = this.DockArea == DarkDockArea.Document;
			if (flag)
			{
				this._tabArea.Visible = this._contents.Count > 0;
			}
			else
			{
				this._tabArea.Visible = this._contents.Count > 1;
			}
			switch (this.DockArea)
			{
			case DarkDockArea.Document:
			{
				int size = (this._tabArea.Visible ? 24 : 0);
				base.Padding = new Padding(0, size, 0, 0);
				this._tabArea.ClientRectangle = new Rectangle(base.Padding.Left, 0, base.ClientRectangle.Width - base.Padding.Horizontal, size);
				break;
			}
			case DarkDockArea.Left:
			case DarkDockArea.Right:
			{
				int size = (this._tabArea.Visible ? 21 : 0);
				base.Padding = new Padding(0, 0, 0, size);
				this._tabArea.ClientRectangle = new Rectangle(base.Padding.Left, base.ClientRectangle.Bottom - size, base.ClientRectangle.Width - base.Padding.Horizontal, size);
				break;
			}
			case DarkDockArea.Bottom:
			{
				int size = (this._tabArea.Visible ? 21 : 0);
				base.Padding = new Padding(1, 0, 0, size);
				this._tabArea.ClientRectangle = new Rectangle(base.Padding.Left, base.ClientRectangle.Bottom - size, base.ClientRectangle.Width - base.Padding.Horizontal, size);
				break;
			}
			}
			bool flag2 = this.DockArea == DarkDockArea.Document;
			if (flag2)
			{
				int dropdownSize = 24;
				this._tabArea.DropdownRectangle = new Rectangle(this._tabArea.ClientRectangle.Right - dropdownSize, 0, dropdownSize, dropdownSize);
			}
			this.BuildTabs();
			this.EnsureVisible();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005F58 File Offset: 0x00004158
		private void BuildTabs()
		{
			bool flag = !this._tabArea.Visible;
			if (!flag)
			{
				base.SuspendLayout();
				int closeButtonSize = DockIcons.close.Width;
				int totalSize = 0;
				IOrderedEnumerable<DarkDockContent> orderedContent = this._contents.OrderBy((DarkDockContent c) => c.Order);
				foreach (DarkDockContent content in orderedContent)
				{
					DarkDockTab tab6 = this._tabs[content];
					int width;
					using (Graphics g = base.CreateGraphics())
					{
						width = tab6.CalculateWidth(g, this.Font);
					}
					bool flag2 = this.DockArea == DarkDockArea.Document;
					if (flag2)
					{
						width += 5;
						width += closeButtonSize;
						bool flag3 = tab6.DockContent.Icon != null;
						if (flag3)
						{
							width += tab6.DockContent.Icon.Width + 5;
						}
					}
					tab6.ShowSeparator = true;
					width++;
					int y = ((this.DockArea == DarkDockArea.Document) ? 0 : (base.ClientRectangle.Height - 21));
					int height = ((this.DockArea == DarkDockArea.Document) ? 24 : 21);
					Rectangle tabRect = new Rectangle(this._tabArea.ClientRectangle.Left + totalSize, y, width, height);
					tab6.ClientRectangle = tabRect;
					totalSize += width;
				}
				bool flag4 = this.DockArea != DarkDockArea.Document;
				if (flag4)
				{
					bool flag5 = totalSize > this._tabArea.ClientRectangle.Width;
					if (flag5)
					{
						int difference = totalSize - this._tabArea.ClientRectangle.Width;
						DarkDockTab lastTab = this._tabs[orderedContent.Last<DarkDockContent>()];
						Rectangle tabRect2 = lastTab.ClientRectangle;
						lastTab.ClientRectangle = new Rectangle(tabRect2.Left, tabRect2.Top, tabRect2.Width - 1, tabRect2.Height);
						lastTab.ShowSeparator = false;
						int differenceMadeUp = 1;
						while (differenceMadeUp < difference)
						{
							int largest = this._tabs.Values.OrderByDescending((DarkDockTab tab) => tab.ClientRectangle.Width).First<DarkDockTab>().ClientRectangle.Width;
							foreach (DarkDockContent content2 in orderedContent)
							{
								DarkDockTab tab2 = this._tabs[content2];
								bool flag6 = differenceMadeUp >= difference;
								if (flag6)
								{
									break;
								}
								bool flag7 = tab2.ClientRectangle.Width >= largest;
								if (flag7)
								{
									Rectangle rect = tab2.ClientRectangle;
									tab2.ClientRectangle = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height);
									differenceMadeUp++;
								}
							}
						}
						int xOffset = 0;
						foreach (DarkDockContent content3 in orderedContent)
						{
							DarkDockTab tab3 = this._tabs[content3];
							Rectangle rect2 = tab3.ClientRectangle;
							tab3.ClientRectangle = new Rectangle(this._tabArea.ClientRectangle.Left + xOffset, rect2.Top, rect2.Width, rect2.Height);
							xOffset += rect2.Width;
						}
					}
				}
				bool flag8 = this.DockArea == DarkDockArea.Document;
				if (flag8)
				{
					foreach (DarkDockContent content4 in orderedContent)
					{
						DarkDockTab tab4 = this._tabs[content4];
						Rectangle closeRect = new Rectangle(tab4.ClientRectangle.Right - 7 - closeButtonSize - 1, tab4.ClientRectangle.Top + tab4.ClientRectangle.Height / 2 - closeButtonSize / 2 - 1, closeButtonSize, closeButtonSize);
						tab4.CloseButtonRectangle = closeRect;
					}
				}
				totalSize = 0;
				foreach (DarkDockContent content5 in orderedContent)
				{
					DarkDockTab tab5 = this._tabs[content5];
					totalSize += tab5.ClientRectangle.Width;
				}
				this._tabArea.TotalTabSize = totalSize;
				base.ResumeLayout();
				base.Invalidate();
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000064C4 File Offset: 0x000046C4
		public void EnsureVisible()
		{
			bool flag = this.DockArea != DarkDockArea.Document;
			if (!flag)
			{
				bool flag2 = this.VisibleContent == null;
				if (!flag2)
				{
					int width = base.ClientRectangle.Width - base.Padding.Horizontal - this._tabArea.DropdownRectangle.Width;
					Rectangle offsetArea = new Rectangle(base.Padding.Left, 0, width, 0);
					DarkDockTab tab = this._tabs[this.VisibleContent];
					bool isEmpty = tab.ClientRectangle.IsEmpty;
					if (!isEmpty)
					{
						bool flag3 = this.RectangleToTabArea(tab.ClientRectangle).Left < offsetArea.Left;
						if (flag3)
						{
							this._tabArea.Offset = tab.ClientRectangle.Left;
						}
						else
						{
							bool flag4 = this.RectangleToTabArea(tab.ClientRectangle).Right > offsetArea.Right;
							if (flag4)
							{
								this._tabArea.Offset = tab.ClientRectangle.Right - width;
							}
						}
						bool flag5 = this._tabArea.TotalTabSize < offsetArea.Width;
						if (flag5)
						{
							this._tabArea.Offset = 0;
						}
						bool flag6 = this._tabArea.TotalTabSize > offsetArea.Width;
						if (flag6)
						{
							IOrderedEnumerable<DarkDockContent> orderedContent = this._contents.OrderBy((DarkDockContent x) => x.Order);
							DarkDockTab lastTab = this._tabs[orderedContent.Last<DarkDockContent>()];
							bool flag7 = lastTab != null;
							if (flag7)
							{
								bool flag8 = this.RectangleToTabArea(lastTab.ClientRectangle).Right < offsetArea.Right;
								if (flag8)
								{
									this._tabArea.Offset = lastTab.ClientRectangle.Right - width;
								}
							}
						}
						base.Invalidate();
					}
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000066D4 File Offset: 0x000048D4
		public void SetVisibleContent(DarkDockContent content)
		{
			bool flag = !this._contents.Contains(content);
			if (!flag)
			{
				bool flag2 = this.VisibleContent != content;
				if (flag2)
				{
					this.VisibleContent = content;
					content.Visible = true;
					foreach (DarkDockContent otherContent in this._contents)
					{
						bool flag3 = otherContent != content;
						if (flag3)
						{
							otherContent.Visible = false;
						}
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006778 File Offset: 0x00004978
		private Point PointToTabArea(Point point)
		{
			return new Point(point.X - this._tabArea.Offset, point.Y);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000067AC File Offset: 0x000049AC
		private Rectangle RectangleToTabArea(Rectangle rectangle)
		{
			return new Rectangle(this.PointToTabArea(rectangle.Location), rectangle.Size);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000067D7 File Offset: 0x000049D7
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			this.UpdateTabArea();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000067EC File Offset: 0x000049EC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool flag = this._dragTab != null;
			if (flag)
			{
				int offsetX = e.Location.X + this._tabArea.Offset;
				bool flag2 = offsetX < this._dragTab.ClientRectangle.Left;
				if (flag2)
				{
					bool flag3 = this._dragTab.DockContent.Order > 0;
					if (flag3)
					{
						List<DarkDockTab> otherTabs = this._tabs.Values.Where((DarkDockTab t) => t.DockContent.Order == this._dragTab.DockContent.Order - 1).ToList<DarkDockTab>();
						bool flag4 = otherTabs.Count == 0;
						if (!flag4)
						{
							DarkDockTab otherTab = otherTabs.First<DarkDockTab>();
							bool flag5 = otherTab == null;
							if (!flag5)
							{
								int oldIndex = this._dragTab.DockContent.Order;
								this._dragTab.DockContent.Order = oldIndex - 1;
								otherTab.DockContent.Order = oldIndex;
								this.BuildTabs();
								this.EnsureVisible();
								this._tabArea.RebuildMenu();
							}
						}
					}
				}
				else
				{
					bool flag6 = offsetX > this._dragTab.ClientRectangle.Right;
					if (flag6)
					{
						int maxOrder = this._contents.Count;
						bool flag7 = this._dragTab.DockContent.Order < maxOrder;
						if (flag7)
						{
							List<DarkDockTab> otherTabs2 = this._tabs.Values.Where((DarkDockTab t) => t.DockContent.Order == this._dragTab.DockContent.Order + 1).ToList<DarkDockTab>();
							bool flag8 = otherTabs2.Count == 0;
							if (!flag8)
							{
								DarkDockTab otherTab2 = otherTabs2.First<DarkDockTab>();
								bool flag9 = otherTab2 == null;
								if (!flag9)
								{
									int oldIndex2 = this._dragTab.DockContent.Order;
									this._dragTab.DockContent.Order = oldIndex2 + 1;
									otherTab2.DockContent.Order = oldIndex2;
									this.BuildTabs();
									this.EnsureVisible();
									this._tabArea.RebuildMenu();
								}
							}
						}
					}
				}
			}
			else
			{
				bool flag10 = this._tabArea.DropdownRectangle.Contains(e.Location);
				if (flag10)
				{
					this._tabArea.DropdownHot = true;
					foreach (DarkDockTab tab in this._tabs.Values)
					{
						tab.Hot = false;
					}
					base.Invalidate();
				}
				else
				{
					this._tabArea.DropdownHot = false;
					foreach (DarkDockTab tab2 in this._tabs.Values)
					{
						bool hot = this.RectangleToTabArea(tab2.ClientRectangle).Contains(e.Location);
						bool flag11 = tab2.Hot != hot;
						if (flag11)
						{
							tab2.Hot = hot;
							base.Invalidate();
						}
						bool closeHot = this.RectangleToTabArea(tab2.CloseButtonRectangle).Contains(e.Location);
						bool flag12 = tab2.CloseButtonHot != closeHot;
						if (flag12)
						{
							tab2.CloseButtonHot = closeHot;
							base.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006B7C File Offset: 0x00004D7C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = this._tabArea.DropdownRectangle.Contains(e.Location);
			if (flag)
			{
				this._tabArea.DropdownHot = true;
			}
			else
			{
				foreach (DarkDockTab tab in this._tabs.Values)
				{
					bool flag2 = this.RectangleToTabArea(tab.ClientRectangle).Contains(e.Location);
					if (flag2)
					{
						bool flag3 = e.Button == MouseButtons.Middle;
						if (flag3)
						{
							tab.DockContent.Close();
							return;
						}
						bool flag4 = this.RectangleToTabArea(tab.CloseButtonRectangle).Contains(e.Location);
						if (flag4)
						{
							this._tabArea.ClickedCloseButton = tab;
							return;
						}
						this.DockPanel.ActiveContent = tab.DockContent;
						this.EnsureVisible();
						this._dragTab = tab;
						return;
					}
				}
				bool flag5 = this.VisibleContent != null;
				if (flag5)
				{
					this.DockPanel.ActiveContent = this.VisibleContent;
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006CCC File Offset: 0x00004ECC
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this._dragTab = null;
			bool flag = this._tabArea.DropdownRectangle.Contains(e.Location);
			if (flag)
			{
				bool dropdownHot = this._tabArea.DropdownHot;
				if (dropdownHot)
				{
					this._tabArea.ShowMenu(this, new Point(this._tabArea.DropdownRectangle.Left, this._tabArea.DropdownRectangle.Bottom - 2));
				}
			}
			else
			{
				bool flag2 = this._tabArea.ClickedCloseButton == null;
				if (!flag2)
				{
					bool flag3 = this.RectangleToTabArea(this._tabArea.ClickedCloseButton.CloseButtonRectangle).Contains(e.Location);
					if (flag3)
					{
						this._tabArea.ClickedCloseButton.DockContent.Close();
					}
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006DA8 File Offset: 0x00004FA8
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			foreach (DarkDockTab tab in this._tabs.Values)
			{
				tab.Hot = false;
			}
			base.Invalidate();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006E14 File Offset: 0x00005014
		private void TabMenuItem_Select(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			bool flag = menuItem == null;
			if (!flag)
			{
				DarkDockContent content = menuItem.Tag as DarkDockContent;
				bool flag2 = content == null;
				if (!flag2)
				{
					this.DockPanel.ActiveContent = content;
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006E58 File Offset: 0x00005058
		private void DockPanel_ActiveContentChanged(object sender, DockContentEventArgs e)
		{
			bool flag = !this._contents.Contains(e.Content);
			if (!flag)
			{
				bool flag2 = e.Content == this.VisibleContent;
				if (flag2)
				{
					this.VisibleContent.Focus();
				}
				else
				{
					this.VisibleContent = e.Content;
					foreach (DarkDockContent content in this._contents)
					{
						content.Visible = content == this.VisibleContent;
					}
					this.VisibleContent.Focus();
					this.EnsureVisible();
					base.Invalidate();
				}
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006F1C File Offset: 0x0000511C
		private void DockContent_DockTextChanged(object sender, EventArgs e)
		{
			this.BuildTabs();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006F28 File Offset: 0x00005128
		public void Redraw()
		{
			base.Invalidate();
			foreach (DarkDockContent content in this._contents)
			{
				content.Invalidate();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006F88 File Offset: 0x00005188
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.GreySelection))
			{
				g.FillRectangle(b, base.ClientRectangle);
			}
			bool flag = !this._tabArea.Visible;
			if (!flag)
			{
				using (SolidBrush b2 = new SolidBrush(Colors.GreySelection))
				{
					g.FillRectangle(b2, this._tabArea.ClientRectangle);
				}
				foreach (DarkDockTab tab in this._tabs.Values)
				{
					bool flag2 = this.DockArea == DarkDockArea.Document;
					if (flag2)
					{
						this.PaintDocumentTab(g, tab);
					}
					else
					{
						this.PaintToolWindowTab(g, tab);
					}
				}
				bool flag3 = this.DockArea == DarkDockArea.Document;
				if (flag3)
				{
					Color divColor = ((this.DockPanel.ActiveGroup == this) ? Colors.GreySelection : Colors.GreySelection);
					using (SolidBrush b3 = new SolidBrush(divColor))
					{
						Rectangle divRect = new Rectangle(this._tabArea.ClientRectangle.Left, this._tabArea.ClientRectangle.Bottom - 2, this._tabArea.ClientRectangle.Width, 2);
						g.FillRectangle(b3, divRect);
					}
				}
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007140 File Offset: 0x00005340
		private void PaintDocumentTab(Graphics g, DarkDockTab tab)
		{
			Rectangle tabRect = this.RectangleToTabArea(tab.ClientRectangle);
			bool isVisibleTab = this.VisibleContent == tab.DockContent;
			bool isActiveGroup = this.DockPanel.ActiveGroup == this;
			Color bgColor = (isVisibleTab ? Colors.GreySelection : Colors.DarkBackground);
			bool flag = !isActiveGroup;
			if (flag)
			{
				bgColor = (isVisibleTab ? Colors.GreySelection : Colors.DarkBackground);
			}
			bool flag2 = tab.Hot && !isVisibleTab;
			if (flag2)
			{
				bgColor = Colors.GreySelection;
			}
			using (SolidBrush b = new SolidBrush(bgColor))
			{
				g.FillRectangle(b, tabRect);
			}
			bool showSeparator = tab.ShowSeparator;
			if (showSeparator)
			{
				using (Pen p = new Pen(Colors.GreySelection))
				{
					g.DrawLine(p, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom);
				}
			}
			int xOffset = 0;
			bool flag3 = tab.DockContent.Icon != null;
			if (flag3)
			{
				g.DrawImageUnscaled(tab.DockContent.Icon, tabRect.Left + 5, tabRect.Top + 4);
				xOffset += tab.DockContent.Icon.Width + 2;
			}
			StringFormat tabTextFormat = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center,
				FormatFlags = StringFormatFlags.NoWrap,
				Trimming = StringTrimming.EllipsisCharacter
			};
			Color textColor = (isVisibleTab ? Colors.LightText : Colors.DisabledText);
			using (SolidBrush b2 = new SolidBrush(textColor))
			{
				Rectangle textRect = new Rectangle(tabRect.Left + 5 + xOffset, tabRect.Top, tabRect.Width - tab.CloseButtonRectangle.Width - 7 - 5 - xOffset, tabRect.Height);
				g.DrawString(tab.DockContent.DockText, this.Font, b2, textRect, tabTextFormat);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000736C File Offset: 0x0000556C
		private void PaintToolWindowTab(Graphics g, DarkDockTab tab)
		{
			Rectangle tabRect = tab.ClientRectangle;
			bool isVisibleTab = this.VisibleContent == tab.DockContent;
			Color bgColor = (isVisibleTab ? Colors.GreyBackground : Colors.DarkBackground);
			bool flag = tab.Hot && !isVisibleTab;
			if (flag)
			{
				bgColor = Colors.MediumBackground;
			}
			using (SolidBrush b = new SolidBrush(bgColor))
			{
				g.FillRectangle(b, tabRect);
			}
			bool showSeparator = tab.ShowSeparator;
			if (showSeparator)
			{
				using (Pen p = new Pen(Colors.DarkBorder))
				{
					g.DrawLine(p, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom);
				}
			}
			StringFormat tabTextFormat = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center,
				FormatFlags = StringFormatFlags.NoWrap,
				Trimming = StringTrimming.EllipsisCharacter
			};
			Color textColor = (isVisibleTab ? Colors.BlackHighlight : Colors.DisabledText);
			using (SolidBrush b2 = new SolidBrush(textColor))
			{
				Rectangle textRect = new Rectangle(tabRect.Left + 5, tabRect.Top, tabRect.Width - 5, tabRect.Height);
				g.DrawString(tab.DockContent.DockText, this.Font, b2, textRect, tabTextFormat);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		// Token: 0x0400013F RID: 319
		private List<DarkDockContent> _contents = new List<DarkDockContent>();

		// Token: 0x04000140 RID: 320
		private Dictionary<DarkDockContent, DarkDockTab> _tabs = new Dictionary<DarkDockContent, DarkDockTab>();

		// Token: 0x04000141 RID: 321
		private DarkDockTabArea _tabArea;

		// Token: 0x04000142 RID: 322
		private DarkDockTab _dragTab = null;
	}
}
