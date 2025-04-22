using System;

namespace DarkUI.Docking
{
	// Token: 0x02000024 RID: 36
	public class DockContentEventArgs : EventArgs
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000984C File Offset: 0x00007A4C
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00009854 File Offset: 0x00007A54
		public DarkDockContent Content { get; private set; }

		// Token: 0x06000142 RID: 322 RVA: 0x0000985D File Offset: 0x00007A5D
		public DockContentEventArgs(DarkDockContent content)
		{
			this.Content = content;
		}
	}
}
