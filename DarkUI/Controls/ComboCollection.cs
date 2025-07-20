using System;
using System.Collections;
using System.Windows.Forms;

namespace DarkUI.Controls
{
	// Token: 0x0200002C RID: 44
	public class ComboCollection<TComboBoxItem> : CollectionBase
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000A06D File Offset: 0x0000826D
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000A075 File Offset: 0x00008275
		public ComboBox.ObjectCollection ItemsBase { get; set; }

		// Token: 0x17000072 RID: 114
		public ComboBoxItem this[int index]
		{
			get
			{
				return (ComboBoxItem)this.ItemsBase[index];
			}
			set
			{
				this.ItemsBase[index] = value;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A0B4 File Offset: 0x000082B4
		public int Add(ComboBoxItem value)
		{
			int result = this.ItemsBase.Add(value);
			this.UpdateItems(this, null);
			return result;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A0E4 File Offset: 0x000082E4
		public int IndexOf(ComboBoxItem value)
		{
			return this.ItemsBase.IndexOf(value);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A102 File Offset: 0x00008302
		public void Insert(int index, ComboBoxItem value)
		{
			this.ItemsBase.Insert(index, value);
			this.UpdateItems(this, null);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A121 File Offset: 0x00008321
		public void Remove(ComboBoxItem value)
		{
			this.ItemsBase.Remove(value);
			this.UpdateItems(this, null);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A140 File Offset: 0x00008340
		public bool Contains(ComboBoxItem value)
		{
			return this.ItemsBase.Contains(value);
		}

		// Token: 0x04000192 RID: 402
		public EventHandler UpdateItems;
	}
}
