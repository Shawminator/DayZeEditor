using System;
using System.Drawing;

namespace DarkUI.Docking
{
	// Token: 0x02000025 RID: 37
	internal class DockDropArea
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000986F File Offset: 0x00007A6F
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00009877 File Offset: 0x00007A77
		internal DarkDockPanel DockPanel { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00009880 File Offset: 0x00007A80
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00009888 File Offset: 0x00007A88
		internal Rectangle DropArea { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00009891 File Offset: 0x00007A91
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00009899 File Offset: 0x00007A99
		internal Rectangle HighlightArea { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000098A2 File Offset: 0x00007AA2
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000098AA File Offset: 0x00007AAA
		internal DarkDockRegion DockRegion { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000098B3 File Offset: 0x00007AB3
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000098BB File Offset: 0x00007ABB
		internal DarkDockGroup DockGroup { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000098C4 File Offset: 0x00007AC4
		// (set) Token: 0x0600014E RID: 334 RVA: 0x000098CC File Offset: 0x00007ACC
		internal DockInsertType InsertType { get; private set; }

		// Token: 0x0600014F RID: 335 RVA: 0x000098D5 File Offset: 0x00007AD5
		internal DockDropArea(DarkDockPanel dockPanel, DarkDockRegion region)
		{
			this.DockPanel = dockPanel;
			this.DockRegion = region;
			this.InsertType = DockInsertType.None;
			this.BuildAreas();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000098FE File Offset: 0x00007AFE
		internal DockDropArea(DarkDockPanel dockPanel, DarkDockGroup group, DockInsertType insertType)
		{
			this.DockPanel = dockPanel;
			this.DockGroup = group;
			this.InsertType = insertType;
			this.BuildAreas();
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00009928 File Offset: 0x00007B28
		internal void BuildAreas()
		{
			bool flag = this.DockRegion != null;
			if (flag)
			{
				this.BuildRegionAreas();
			}
			else
			{
				bool flag2 = this.DockGroup != null;
				if (flag2)
				{
					this.BuildGroupAreas();
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009960 File Offset: 0x00007B60
		private void BuildRegionAreas()
		{
			switch (this.DockRegion.DockArea)
			{
			case DarkDockArea.Left:
			{
				Rectangle leftRect = new Rectangle
				{
					X = this.DockPanel.PointToScreen(Point.Empty).X,
					Y = this.DockPanel.PointToScreen(Point.Empty).Y,
					Width = 50,
					Height = this.DockPanel.Height
				};
				this.DropArea = leftRect;
				this.HighlightArea = leftRect;
				break;
			}
			case DarkDockArea.Right:
			{
				Rectangle rightRect = new Rectangle
				{
					X = this.DockPanel.PointToScreen(Point.Empty).X + this.DockPanel.Width - 50,
					Y = this.DockPanel.PointToScreen(Point.Empty).Y,
					Width = 50,
					Height = this.DockPanel.Height
				};
				this.DropArea = rightRect;
				this.HighlightArea = rightRect;
				break;
			}
			case DarkDockArea.Bottom:
			{
				int x = this.DockPanel.PointToScreen(Point.Empty).X;
				int width = this.DockPanel.Width;
				bool visible = this.DockPanel.Regions[DarkDockArea.Left].Visible;
				if (visible)
				{
					x += this.DockPanel.Regions[DarkDockArea.Left].Width;
					width -= this.DockPanel.Regions[DarkDockArea.Left].Width;
				}
				bool visible2 = this.DockPanel.Regions[DarkDockArea.Right].Visible;
				if (visible2)
				{
					width -= this.DockPanel.Regions[DarkDockArea.Right].Width;
				}
				Rectangle bottomRect = new Rectangle
				{
					X = x,
					Y = this.DockPanel.PointToScreen(Point.Empty).Y + this.DockPanel.Height - 50,
					Width = width,
					Height = 50
				};
				this.DropArea = bottomRect;
				this.HighlightArea = bottomRect;
				break;
			}
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009BC0 File Offset: 0x00007DC0
		private void BuildGroupAreas()
		{
			switch (this.InsertType)
			{
			case DockInsertType.None:
			{
				Rectangle dropRect = new Rectangle
				{
					X = this.DockGroup.PointToScreen(Point.Empty).X,
					Y = this.DockGroup.PointToScreen(Point.Empty).Y,
					Width = this.DockGroup.Width,
					Height = this.DockGroup.Height
				};
				this.DropArea = dropRect;
				this.HighlightArea = dropRect;
				break;
			}
			case DockInsertType.Before:
			{
				int beforeDropWidth = this.DockGroup.Width;
				int beforeDropHeight = this.DockGroup.Height;
				DarkDockArea dockArea = this.DockGroup.DockArea;
				DarkDockArea darkDockArea = dockArea;
				if (darkDockArea - DarkDockArea.Left > 1)
				{
					if (darkDockArea == DarkDockArea.Bottom)
					{
						beforeDropWidth = this.DockGroup.Width / 4;
					}
				}
				else
				{
					beforeDropHeight = this.DockGroup.Height / 4;
				}
				Rectangle beforeDropRect = new Rectangle
				{
					X = this.DockGroup.PointToScreen(Point.Empty).X,
					Y = this.DockGroup.PointToScreen(Point.Empty).Y,
					Width = beforeDropWidth,
					Height = beforeDropHeight
				};
				this.DropArea = beforeDropRect;
				this.HighlightArea = beforeDropRect;
				break;
			}
			case DockInsertType.After:
			{
				int afterDropX = this.DockGroup.PointToScreen(Point.Empty).X;
				int afterDropY = this.DockGroup.PointToScreen(Point.Empty).Y;
				int afterDropWidth = this.DockGroup.Width;
				int afterDropHeight = this.DockGroup.Height;
				DarkDockArea dockArea2 = this.DockGroup.DockArea;
				DarkDockArea darkDockArea2 = dockArea2;
				if (darkDockArea2 - DarkDockArea.Left > 1)
				{
					if (darkDockArea2 == DarkDockArea.Bottom)
					{
						afterDropWidth = this.DockGroup.Width / 4;
						afterDropX = this.DockGroup.PointToScreen(Point.Empty).X + this.DockGroup.Width - afterDropWidth;
					}
				}
				else
				{
					afterDropHeight = this.DockGroup.Height / 4;
					afterDropY = this.DockGroup.PointToScreen(Point.Empty).Y + this.DockGroup.Height - afterDropHeight;
				}
				Rectangle afterDropRect = new Rectangle
				{
					X = afterDropX,
					Y = afterDropY,
					Width = afterDropWidth,
					Height = afterDropHeight
				};
				this.DropArea = afterDropRect;
				this.HighlightArea = afterDropRect;
				break;
			}
			}
		}
	}
}
