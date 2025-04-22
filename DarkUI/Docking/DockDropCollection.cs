using System;

namespace DarkUI.Docking
{
	// Token: 0x02000026 RID: 38
	internal class DockDropCollection
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00009E76 File Offset: 0x00008076
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00009E7E File Offset: 0x0000807E
		internal DockDropArea DropArea { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00009E87 File Offset: 0x00008087
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00009E8F File Offset: 0x0000808F
		internal DockDropArea InsertBeforeArea { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00009E98 File Offset: 0x00008098
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00009EA0 File Offset: 0x000080A0
		internal DockDropArea InsertAfterArea { get; private set; }

		// Token: 0x0600015A RID: 346 RVA: 0x00009EA9 File Offset: 0x000080A9
		internal DockDropCollection(DarkDockPanel dockPanel, DarkDockGroup group)
		{
			this.DropArea = new DockDropArea(dockPanel, group, DockInsertType.None);
			this.InsertBeforeArea = new DockDropArea(dockPanel, group, DockInsertType.Before);
			this.InsertAfterArea = new DockDropArea(dockPanel, group, DockInsertType.After);
		}
	}
}
