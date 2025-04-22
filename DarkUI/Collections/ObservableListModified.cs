using System;
using System.Collections.Generic;

namespace DarkUI.Collections
{
	// Token: 0x0200004F RID: 79
	public class ObservableListModified<T> : EventArgs
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00014597 File Offset: 0x00012797
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0001459F File Offset: 0x0001279F
		public IEnumerable<T> Items { get; private set; }

		// Token: 0x06000382 RID: 898 RVA: 0x000145A8 File Offset: 0x000127A8
		public ObservableListModified(IEnumerable<T> items)
		{
			this.Items = items;
		}
	}
}
