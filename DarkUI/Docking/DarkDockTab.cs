using System;
using System.Drawing;

namespace DarkUI.Docking
{
	// Token: 0x02000022 RID: 34
	internal class DarkDockTab
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000951F File Offset: 0x0000771F
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00009527 File Offset: 0x00007727
		public DarkDockContent DockContent { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00009530 File Offset: 0x00007730
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00009538 File Offset: 0x00007738
		public Rectangle ClientRectangle { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00009541 File Offset: 0x00007741
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00009549 File Offset: 0x00007749
		public Rectangle CloseButtonRectangle { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009552 File Offset: 0x00007752
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000955A File Offset: 0x0000775A
		public bool Hot { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00009563 File Offset: 0x00007763
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000956B File Offset: 0x0000776B
		public bool CloseButtonHot { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00009574 File Offset: 0x00007774
		// (set) Token: 0x06000127 RID: 295 RVA: 0x0000957C File Offset: 0x0000777C
		public bool ShowSeparator { get; set; }

		// Token: 0x06000128 RID: 296 RVA: 0x00009585 File Offset: 0x00007785
		public DarkDockTab(DarkDockContent content)
		{
			this.DockContent = content;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009598 File Offset: 0x00007798
		public int CalculateWidth(Graphics g, Font font)
		{
			int width = (int)g.MeasureString(this.DockContent.DockText, font).Width;
			return width + 10;
		}
	}
}
