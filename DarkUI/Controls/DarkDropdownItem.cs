using System;
using System.Drawing;

namespace DarkUI.Controls
{
	// Token: 0x02000030 RID: 48
	public class DarkDropdownItem
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000B02C File Offset: 0x0000922C
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000B034 File Offset: 0x00009234
		public string Text { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000B03D File Offset: 0x0000923D
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000B045 File Offset: 0x00009245
		public Bitmap Icon { get; set; }

		// Token: 0x060001BA RID: 442 RVA: 0x00002050 File Offset: 0x00000250
		public DarkDropdownItem()
		{
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B04E File Offset: 0x0000924E
		public DarkDropdownItem(string text)
		{
			this.Text = text;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B060 File Offset: 0x00009260
		public DarkDropdownItem(string text, Bitmap icon)
			: this(text)
		{
			this.Icon = icon;
		}
	}
}
