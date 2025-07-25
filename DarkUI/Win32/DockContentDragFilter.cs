using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Docking;
using DarkUI.Forms;

namespace DarkUI.Win32
{
	// Token: 0x0200000A RID: 10
	public class DockContentDragFilter : IMessageFilter
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public DockContentDragFilter(DarkDockPanel dockPanel)
		{
			this._dockPanel = dockPanel;
			this._highlightForm = new DarkTranslucentForm(Colors.BlueSelection, 0.6);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002D04 File Offset: 0x00000F04
		public bool PreFilterMessage(ref Message m)
		{
			bool flag = !this._isDragging;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = m.Msg != 512 && m.Msg != 513 && m.Msg != 514 && m.Msg != 515 && m.Msg != 516 && m.Msg != 517 && m.Msg != 518;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					bool flag4 = m.Msg == 512;
					if (flag4)
					{
						this.HandleDrag();
						flag2 = false;
					}
					else
					{
						bool flag5 = m.Msg == 514;
						if (flag5)
						{
							bool flag6 = this._targetRegion != null;
							if (flag6)
							{
								this._dockPanel.RemoveContent(this._dragContent);
								this._dragContent.DockArea = this._targetRegion.DockArea;
								this._dockPanel.AddContent(this._dragContent);
							}
							else
							{
								bool flag7 = this._targetGroup != null;
								if (flag7)
								{
									this._dockPanel.RemoveContent(this._dragContent);
									DockInsertType insertType = this._insertType;
									DockInsertType dockInsertType = insertType;
									if (dockInsertType != DockInsertType.None)
									{
										if (dockInsertType - DockInsertType.Before <= 1)
										{
											this._dockPanel.InsertContent(this._dragContent, this._targetGroup, this._insertType);
										}
									}
									else
									{
										this._dockPanel.AddContent(this._dragContent, this._targetGroup);
									}
								}
							}
							this.StopDrag();
							flag2 = false;
						}
						else
						{
							flag2 = true;
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002EA0 File Offset: 0x000010A0
		public void StartDrag(DarkDockContent content)
		{
			this._regionDropAreas = new Dictionary<DarkDockRegion, DockDropArea>();
			this._groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();
			foreach (DarkDockRegion region in this._dockPanel.Regions.Values)
			{
				bool flag = region.DockArea == DarkDockArea.Document;
				if (!flag)
				{
					bool visible = region.Visible;
					if (visible)
					{
						foreach (DarkDockGroup group in region.Groups)
						{
							DockDropCollection collection = new DockDropCollection(this._dockPanel, group);
							this._groupDropAreas.Add(group, collection);
						}
					}
					else
					{
						DockDropArea area = new DockDropArea(this._dockPanel, region);
						this._regionDropAreas.Add(region, area);
					}
				}
			}
			this._dragContent = content;
			this._isDragging = true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FC4 File Offset: 0x000011C4
		private void StopDrag()
		{
			Cursor.Current = Cursors.Default;
			this._highlightForm.Hide();
			this._dragContent = null;
			this._isDragging = false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FEC File Offset: 0x000011EC
		private void UpdateHighlightForm(Rectangle rect)
		{
			Cursor.Current = Cursors.SizeAll;
			this._highlightForm.SuspendLayout();
			this._highlightForm.Size = new Size(rect.Width, rect.Height);
			this._highlightForm.Location = new Point(rect.X, rect.Y);
			this._highlightForm.ResumeLayout();
			bool flag = !this._highlightForm.Visible;
			if (flag)
			{
				this._highlightForm.Show();
				this._highlightForm.BringToFront();
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003088 File Offset: 0x00001288
		private void HandleDrag()
		{
			Point location = Cursor.Position;
			this._insertType = DockInsertType.None;
			this._targetRegion = null;
			this._targetGroup = null;
			foreach (DockDropArea area in this._regionDropAreas.Values)
			{
				bool flag = area.DropArea.Contains(location);
				if (flag)
				{
					this._insertType = DockInsertType.None;
					this._targetRegion = area.DockRegion;
					this.UpdateHighlightForm(area.HighlightArea);
					return;
				}
			}
			foreach (DockDropCollection collection in this._groupDropAreas.Values)
			{
				bool sameRegion = false;
				bool sameGroup = false;
				bool groupHasOtherContent = false;
				bool flag2 = collection.DropArea.DockGroup == this._dragContent.DockGroup;
				if (flag2)
				{
					sameGroup = true;
				}
				bool flag3 = collection.DropArea.DockGroup.DockRegion == this._dragContent.DockRegion;
				if (flag3)
				{
					sameRegion = true;
				}
				bool flag4 = this._dragContent.DockGroup.ContentCount > 1;
				if (flag4)
				{
					groupHasOtherContent = true;
				}
				bool flag5 = !sameGroup || groupHasOtherContent;
				if (flag5)
				{
					bool skipBefore = false;
					bool skipAfter = false;
					bool flag6 = sameRegion && !groupHasOtherContent;
					if (flag6)
					{
						bool flag7 = collection.InsertBeforeArea.DockGroup.Order == this._dragContent.DockGroup.Order + 1;
						if (flag7)
						{
							skipBefore = true;
						}
						bool flag8 = collection.InsertAfterArea.DockGroup.Order == this._dragContent.DockGroup.Order - 1;
						if (flag8)
						{
							skipAfter = true;
						}
					}
					bool flag9 = !skipBefore;
					if (flag9)
					{
						bool flag10 = collection.InsertBeforeArea.DropArea.Contains(location);
						if (flag10)
						{
							this._insertType = DockInsertType.Before;
							this._targetGroup = collection.InsertBeforeArea.DockGroup;
							this.UpdateHighlightForm(collection.InsertBeforeArea.HighlightArea);
							return;
						}
					}
					bool flag11 = !skipAfter;
					if (flag11)
					{
						bool flag12 = collection.InsertAfterArea.DropArea.Contains(location);
						if (flag12)
						{
							this._insertType = DockInsertType.After;
							this._targetGroup = collection.InsertAfterArea.DockGroup;
							this.UpdateHighlightForm(collection.InsertAfterArea.HighlightArea);
							return;
						}
					}
				}
				bool flag13 = !sameGroup;
				if (flag13)
				{
					bool flag14 = collection.DropArea.DropArea.Contains(location);
					if (flag14)
					{
						this._insertType = DockInsertType.None;
						this._targetGroup = collection.DropArea.DockGroup;
						this.UpdateHighlightForm(collection.DropArea.HighlightArea);
						return;
					}
				}
			}
			bool visible = this._highlightForm.Visible;
			if (visible)
			{
				this._highlightForm.Hide();
			}
			Cursor.Current = Cursors.No;
		}

		// Token: 0x04000012 RID: 18
		private DarkDockPanel _dockPanel;

		// Token: 0x04000013 RID: 19
		private DarkDockContent _dragContent;

		// Token: 0x04000014 RID: 20
		private DarkTranslucentForm _highlightForm;

		// Token: 0x04000015 RID: 21
		private bool _isDragging = false;

		// Token: 0x04000016 RID: 22
		private DarkDockRegion _targetRegion;

		// Token: 0x04000017 RID: 23
		private DarkDockGroup _targetGroup;

		// Token: 0x04000018 RID: 24
		private DockInsertType _insertType = DockInsertType.None;

		// Token: 0x04000019 RID: 25
		private Dictionary<DarkDockRegion, DockDropArea> _regionDropAreas = new Dictionary<DarkDockRegion, DockDropArea>();

		// Token: 0x0400001A RID: 26
		private Dictionary<DarkDockGroup, DockDropCollection> _groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();
	}
}
