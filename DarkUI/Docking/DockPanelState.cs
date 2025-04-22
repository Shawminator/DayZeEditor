using System;
using System.Collections.Generic;

namespace DarkUI.Docking
{
	// Token: 0x02000028 RID: 40
	public class DockPanelState
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00009F18 File Offset: 0x00008118
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00009F20 File Offset: 0x00008120
		public List<DockRegionState> Regions { get; set; }

		// Token: 0x06000162 RID: 354 RVA: 0x00009F29 File Offset: 0x00008129
		public DockPanelState()
		{
			this.Regions = new List<DockRegionState>();
		}
	}
}
