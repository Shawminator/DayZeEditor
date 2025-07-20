using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DarkUI.Collections
{
	// Token: 0x0200004E RID: 78
	public class ObservableList<T> : List<T>, IDisposable
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000375 RID: 885 RVA: 0x00014354 File Offset: 0x00012554
		// (remove) Token: 0x06000376 RID: 886 RVA: 0x0001438C File Offset: 0x0001258C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ObservableListModified<T>> ItemsAdded;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000377 RID: 887 RVA: 0x000143C4 File Offset: 0x000125C4
		// (remove) Token: 0x06000378 RID: 888 RVA: 0x000143FC File Offset: 0x000125FC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<ObservableListModified<T>> ItemsRemoved;

		// Token: 0x06000379 RID: 889 RVA: 0x00014434 File Offset: 0x00012634
		~ObservableList()
		{
			this.Dispose(false);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014468 File Offset: 0x00012668
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001447C File Offset: 0x0001267C
		protected virtual void Dispose(bool disposing)
		{
			bool flag = !this._disposed;
			if (flag)
			{
				bool flag2 = this.ItemsAdded != null;
				if (flag2)
				{
					this.ItemsAdded = null;
				}
				bool flag3 = this.ItemsRemoved != null;
				if (flag3)
				{
					this.ItemsRemoved = null;
				}
				this._disposed = true;
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000144C8 File Offset: 0x000126C8
		public new void Add(T item)
		{
			base.Add(item);
			bool flag = this.ItemsAdded != null;
			if (flag)
			{
				this.ItemsAdded(this, new ObservableListModified<T>(new List<T> { item }));
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001450C File Offset: 0x0001270C
		public new void AddRange(IEnumerable<T> collection)
		{
			List<T> list = collection.ToList<T>();
			base.AddRange(list);
			bool flag = this.ItemsAdded != null;
			if (flag)
			{
				this.ItemsAdded(this, new ObservableListModified<T>(list));
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001454C File Offset: 0x0001274C
		public new void Remove(T item)
		{
			base.Remove(item);
			bool flag = this.ItemsRemoved != null;
			if (flag)
			{
				this.ItemsRemoved(this, new ObservableListModified<T>(new List<T> { item }));
			}
		}

		// Token: 0x04000239 RID: 569
		private bool _disposed;
	}
}
