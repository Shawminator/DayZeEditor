using System;
using System.Diagnostics;
using System.Drawing;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x0200003C RID: 60
	public class DarkListItem
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600026E RID: 622 RVA: 0x0000F9C4 File Offset: 0x0000DBC4
		// (remove) Token: 0x0600026F RID: 623 RVA: 0x0000F9FC File Offset: 0x0000DBFC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler TextChanged;

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000FA34 File Offset: 0x0000DC34
		// (set) Token: 0x06000271 RID: 625 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				bool flag = this.TextChanged != null;
				if (flag)
				{
					this.TextChanged(this, new EventArgs());
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000FA80 File Offset: 0x0000DC80
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public Rectangle Area { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000FA91 File Offset: 0x0000DC91
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000FA99 File Offset: 0x0000DC99
		public Color TextColor { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000FAA2 File Offset: 0x0000DCA2
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000FAAA File Offset: 0x0000DCAA
		public FontStyle FontStyle { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000FAB3 File Offset: 0x0000DCB3
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000FABB File Offset: 0x0000DCBB
		public Bitmap Icon { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000FAC4 File Offset: 0x0000DCC4
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000FACC File Offset: 0x0000DCCC
		public object Tag { get; set; }

		// Token: 0x0600027C RID: 636 RVA: 0x0000FAD5 File Offset: 0x0000DCD5
		public DarkListItem()
		{
			this.TextColor = Colors.LightText;
			this.FontStyle = FontStyle.Regular;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000FAF3 File Offset: 0x0000DCF3
		public DarkListItem(string text)
			: this()
		{
			this.Text = text;
		}

		// Token: 0x040001DF RID: 479
		private string _text;
	}
}
