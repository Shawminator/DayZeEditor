using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000041 RID: 65
	public class DarkLabel : Label
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000121B8 File Offset: 0x000103B8
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000121D0 File Offset: 0x000103D0
		[Category("Layout")]
		[Description("Enables automatic height sizing based on the contents of the label.")]
		[DefaultValue(false)]
		public bool AutoUpdateHeight
		{
			get
			{
				return this._autoUpdateHeight;
			}
			set
			{
				this._autoUpdateHeight = value;
				bool autoUpdateHeight = this._autoUpdateHeight;
				if (autoUpdateHeight)
				{
					this.AutoSize = false;
					this.ResizeLabel();
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00012200 File Offset: 0x00010400
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00012218 File Offset: 0x00010418
		public new bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				bool autoSize = this.AutoSize;
				if (autoSize)
				{
					this.AutoUpdateHeight = false;
				}
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00012240 File Offset: 0x00010440
		public DarkLabel()
		{
			this.ForeColor = Colors.LightText;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00012258 File Offset: 0x00010458
		private void ResizeLabel()
		{
			bool flag = !this._autoUpdateHeight || this._isGrowing;
			if (!flag)
			{
				try
				{
					this._isGrowing = true;
					Size sz = new Size(base.Width, int.MaxValue);
					sz = TextRenderer.MeasureText(this.Text, this.Font, sz, TextFormatFlags.WordBreak);
					base.Height = sz.Height + base.Padding.Vertical;
				}
				finally
				{
					this._isGrowing = false;
				}
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000122E8 File Offset: 0x000104E8
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.ResizeLabel();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000122FA File Offset: 0x000104FA
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.ResizeLabel();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001230C File Offset: 0x0001050C
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.ResizeLabel();
		}

		// Token: 0x04000200 RID: 512
		private bool _autoUpdateHeight;

		// Token: 0x04000201 RID: 513
		private bool _isGrowing;
	}
}
