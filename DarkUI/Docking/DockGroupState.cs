using System;
using System.Collections.Generic;

namespace DarkUI.Docking
{
	// Token: 0x02000027 RID: 39
	public class DockGroupState
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00009EE0 File Offset: 0x000080E0
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00009EE8 File Offset: 0x000080E8
		public List<string> Contents { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00009EF1 File Offset: 0x000080F1
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00009EF9 File Offset: 0x000080F9
		public string VisibleContent { get; set; }

		// Token: 0x0600015F RID: 351 RVA: 0x00009F02 File Offset: 0x00008102
		public DockGroupState()
		{
			this.Contents = new List<string>();
		}
	}
}
