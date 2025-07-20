using System;
using System.Collections.Generic;
using System.Drawing;

namespace DarkUI.Docking
{
	// Token: 0x02000029 RID: 41
	public class DockRegionState
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00009F3F File Offset: 0x0000813F
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00009F47 File Offset: 0x00008147
		public DarkDockArea Area { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00009F50 File Offset: 0x00008150
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00009F58 File Offset: 0x00008158
		public Size Size { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00009F61 File Offset: 0x00008161
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00009F69 File Offset: 0x00008169
		public List<DockGroupState> Groups { get; set; }

		// Token: 0x06000169 RID: 361 RVA: 0x00009F72 File Offset: 0x00008172
		public DockRegionState()
		{
			this.Groups = new List<DockGroupState>();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009F88 File Offset: 0x00008188
		public DockRegionState(DarkDockArea area)
			: this()
		{
			this.Area = area;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009F9A File Offset: 0x0000819A
		public DockRegionState(DarkDockArea area, Size size)
			: this(area)
		{
			this.Size = size;
		}
	}
}
