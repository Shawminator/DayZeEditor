using System;

namespace DarkUI.Controls
{
	// Token: 0x0200004B RID: 75
	public class ScrollValueEventArgs : EventArgs
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0001401F File Offset: 0x0001221F
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00014027 File Offset: 0x00012227
		public int Value { get; private set; }

		// Token: 0x0600035B RID: 859 RVA: 0x00014030 File Offset: 0x00012230
		public ScrollValueEventArgs(int value)
		{
			this.Value = value;
		}
	}
}
