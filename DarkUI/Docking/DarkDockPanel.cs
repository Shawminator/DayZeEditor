using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Win32;

namespace DarkUI.Docking
{
	// Token: 0x0200001B RID: 27
	public class DarkDockPanel : UserControl
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000CA RID: 202 RVA: 0x0000753C File Offset: 0x0000573C
		// (remove) Token: 0x060000CB RID: 203 RVA: 0x00007574 File Offset: 0x00005774
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DockContentEventArgs> ActiveContentChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000CC RID: 204 RVA: 0x000075AC File Offset: 0x000057AC
		// (remove) Token: 0x060000CD RID: 205 RVA: 0x000075E4 File Offset: 0x000057E4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DockContentEventArgs> ContentAdded;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000CE RID: 206 RVA: 0x0000761C File Offset: 0x0000581C
		// (remove) Token: 0x060000CF RID: 207 RVA: 0x00007654 File Offset: 0x00005854
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DockContentEventArgs> ContentRemoved;

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000768C File Offset: 0x0000588C
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000076A4 File Offset: 0x000058A4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockContent ActiveContent
		{
			get
			{
				return this._activeContent;
			}
			set
			{
				bool switchingContent = this._switchingContent;
				if (!switchingContent)
				{
					this._switchingContent = true;
					this._activeContent = value;
					this.ActiveGroup = this._activeContent.DockGroup;
					this.ActiveRegion = this.ActiveGroup.DockRegion;
					foreach (DarkDockRegion region in this._regions.Values)
					{
						region.Redraw();
					}
					bool flag = this.ActiveContentChanged != null;
					if (flag)
					{
						this.ActiveContentChanged(this, new DockContentEventArgs(this._activeContent));
					}
					this._switchingContent = false;
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000776C File Offset: 0x0000596C
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00007774 File Offset: 0x00005974
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockRegion ActiveRegion { get; internal set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000777D File Offset: 0x0000597D
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00007785 File Offset: 0x00005985
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockGroup ActiveGroup { get; internal set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00007790 File Offset: 0x00005990
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockContent ActiveDocument
		{
			get
			{
				return this._regions[DarkDockArea.Document].ActiveDocument;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000077B3 File Offset: 0x000059B3
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x000077BB File Offset: 0x000059BB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockContentDragFilter DockContentDragFilter { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000077C4 File Offset: 0x000059C4
		// (set) Token: 0x060000DA RID: 218 RVA: 0x000077CC File Offset: 0x000059CC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockResizeFilter DockResizeFilter { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000077D5 File Offset: 0x000059D5
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000077DD File Offset: 0x000059DD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<DarkDockSplitter> Splitters { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000077E8 File Offset: 0x000059E8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MouseButtons MouseButtonState
		{
			get
			{
				return Control.MouseButtons;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007804 File Offset: 0x00005A04
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Dictionary<DarkDockArea, DarkDockRegion> Regions
		{
			get
			{
				return this._regions;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000781C File Offset: 0x00005A1C
		public DarkDockPanel()
		{
			this.Splitters = new List<DarkDockSplitter>();
			this.DockContentDragFilter = new DockContentDragFilter(this);
			this.DockResizeFilter = new DockResizeFilter(this);
			this._regions = new Dictionary<DarkDockArea, DarkDockRegion>();
			this._contents = new List<DarkDockContent>();
			this.BackColor = Colors.GreyBackground;
			this.CreateRegions();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007887 File Offset: 0x00005A87
		public void AddContent(DarkDockContent dockContent)
		{
			this.AddContent(dockContent, null);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007894 File Offset: 0x00005A94
		public void AddContent(DarkDockContent dockContent, DarkDockGroup dockGroup)
		{
			bool flag = this._contents.Contains(dockContent);
			if (flag)
			{
				this.RemoveContent(dockContent);
			}
			dockContent.DockPanel = this;
			this._contents.Add(dockContent);
			bool flag2 = dockGroup != null;
			if (flag2)
			{
				dockContent.DockArea = dockGroup.DockArea;
			}
			bool flag3 = dockContent.DockArea == DarkDockArea.None;
			if (flag3)
			{
				dockContent.DockArea = dockContent.DefaultDockArea;
			}
			DarkDockRegion region = this._regions[dockContent.DockArea];
			region.AddContent(dockContent, dockGroup);
			bool flag4 = this.ContentAdded != null;
			if (flag4)
			{
				this.ContentAdded(this, new DockContentEventArgs(dockContent));
			}
			dockContent.Select();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007944 File Offset: 0x00005B44
		public void InsertContent(DarkDockContent dockContent, DarkDockGroup dockGroup, DockInsertType insertType)
		{
			bool flag = this._contents.Contains(dockContent);
			if (flag)
			{
				this.RemoveContent(dockContent);
			}
			dockContent.DockPanel = this;
			this._contents.Add(dockContent);
			dockContent.DockArea = dockGroup.DockArea;
			DarkDockRegion region = this._regions[dockGroup.DockArea];
			region.InsertContent(dockContent, dockGroup, insertType);
			bool flag2 = this.ContentAdded != null;
			if (flag2)
			{
				this.ContentAdded(this, new DockContentEventArgs(dockContent));
			}
			dockContent.Select();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000079D0 File Offset: 0x00005BD0
		public void RemoveContent(DarkDockContent dockContent)
		{
			bool flag = !this._contents.Contains(dockContent);
			if (!flag)
			{
				dockContent.DockPanel = null;
				this._contents.Remove(dockContent);
				DarkDockRegion region = this._regions[dockContent.DockArea];
				region.RemoveContent(dockContent);
				bool flag2 = this.ContentRemoved != null;
				if (flag2)
				{
					this.ContentRemoved(this, new DockContentEventArgs(dockContent));
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007A44 File Offset: 0x00005C44
		public bool ContainsContent(DarkDockContent dockContent)
		{
			return this._contents.Contains(dockContent);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007A64 File Offset: 0x00005C64
		public List<DarkDockContent> GetDocuments()
		{
			return this._regions[DarkDockArea.Document].GetContents();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007A88 File Offset: 0x00005C88
		private void CreateRegions()
		{
			DarkDockRegion documentRegion = new DarkDockRegion(this, DarkDockArea.Document);
			this._regions.Add(DarkDockArea.Document, documentRegion);
			DarkDockRegion leftRegion = new DarkDockRegion(this, DarkDockArea.Left);
			this._regions.Add(DarkDockArea.Left, leftRegion);
			DarkDockRegion rightRegion = new DarkDockRegion(this, DarkDockArea.Right);
			this._regions.Add(DarkDockArea.Right, rightRegion);
			DarkDockRegion bottomRegion = new DarkDockRegion(this, DarkDockArea.Bottom);
			this._regions.Add(DarkDockArea.Bottom, bottomRegion);
			base.Controls.Add(documentRegion);
			base.Controls.Add(bottomRegion);
			base.Controls.Add(leftRegion);
			base.Controls.Add(rightRegion);
			documentRegion.TabIndex = 0;
			rightRegion.TabIndex = 1;
			bottomRegion.TabIndex = 2;
			leftRegion.TabIndex = 3;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007B42 File Offset: 0x00005D42
		public void DragContent(DarkDockContent content)
		{
			this.DockContentDragFilter.StartDrag(content);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007B54 File Offset: 0x00005D54
		public DockPanelState GetDockPanelState()
		{
			DockPanelState state = new DockPanelState();
			state.Regions.Add(new DockRegionState(DarkDockArea.Document));
			state.Regions.Add(new DockRegionState(DarkDockArea.Left, this._regions[DarkDockArea.Left].Size));
			state.Regions.Add(new DockRegionState(DarkDockArea.Right, this._regions[DarkDockArea.Right].Size));
			state.Regions.Add(new DockRegionState(DarkDockArea.Bottom, this._regions[DarkDockArea.Bottom].Size));
			Dictionary<DarkDockGroup, DockGroupState> _groupStates = new Dictionary<DarkDockGroup, DockGroupState>();
			IOrderedEnumerable<DarkDockContent> orderedContent = this._contents.OrderBy((DarkDockContent c) => c.Order);
			foreach (DarkDockContent content in orderedContent)
			{
				foreach (DockRegionState region in state.Regions)
				{
					bool flag = region.Area == content.DockArea;
					if (flag)
					{
						bool flag2 = _groupStates.ContainsKey(content.DockGroup);
						DockGroupState groupState;
						if (flag2)
						{
							groupState = _groupStates[content.DockGroup];
						}
						else
						{
							groupState = new DockGroupState();
							region.Groups.Add(groupState);
							_groupStates.Add(content.DockGroup, groupState);
						}
						groupState.Contents.Add(content.SerializationKey);
						groupState.VisibleContent = content.DockGroup.VisibleContent.SerializationKey;
					}
				}
			}
			return state;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007D34 File Offset: 0x00005F34
		public void RestoreDockPanelState(DockPanelState state, Func<string, DarkDockContent> getContentBySerializationKey)
		{
			foreach (DockRegionState region in state.Regions)
			{
				switch (region.Area)
				{
				case DarkDockArea.Left:
					this._regions[DarkDockArea.Left].Size = region.Size;
					break;
				case DarkDockArea.Right:
					this._regions[DarkDockArea.Right].Size = region.Size;
					break;
				case DarkDockArea.Bottom:
					this._regions[DarkDockArea.Bottom].Size = region.Size;
					break;
				}
				foreach (DockGroupState group in region.Groups)
				{
					DarkDockContent previousContent = null;
					DarkDockContent visibleContent = null;
					foreach (string contentKey in group.Contents)
					{
						DarkDockContent content = getContentBySerializationKey(contentKey);
						bool flag = content == null;
						if (!flag)
						{
							content.DockArea = region.Area;
							bool flag2 = previousContent == null;
							if (flag2)
							{
								this.AddContent(content);
							}
							else
							{
								this.AddContent(content, previousContent.DockGroup);
							}
							previousContent = content;
							bool flag3 = group.VisibleContent == contentKey;
							if (flag3)
							{
								visibleContent = content;
							}
						}
					}
					bool flag4 = visibleContent != null;
					if (flag4)
					{
						visibleContent.Select();
					}
				}
			}
		}

		// Token: 0x0400014B RID: 331
		private List<DarkDockContent> _contents;

		// Token: 0x0400014C RID: 332
		private Dictionary<DarkDockArea, DarkDockRegion> _regions;

		// Token: 0x0400014D RID: 333
		private DarkDockContent _activeContent;

		// Token: 0x0400014E RID: 334
		private bool _switchingContent = false;
	}
}
