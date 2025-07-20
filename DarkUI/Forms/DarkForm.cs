using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Forms
{
	// Token: 0x02000012 RID: 18
	public partial class DarkForm : Form
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004F58 File Offset: 0x00003158
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00004F70 File Offset: 0x00003170
		[Category("Appearance")]
		[Description("Determines whether a single pixel border should be rendered around the form.")]
		[DefaultValue(false)]
		public bool FlatBorder
		{
			get
			{
				return this._flatBorder;
			}
			set
			{
				this._flatBorder = value;
				base.Invalidate();
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004F81 File Offset: 0x00003181
		public DarkForm()
		{
			this.BackColor = Colors.GreyBackground;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004F98 File Offset: 0x00003198
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			bool flag = !this._flatBorder;
			if (!flag)
			{
				Graphics g = e.Graphics;
				using (Pen p = new Pen(Colors.DarkBorder))
				{
					Rectangle modRect = new Rectangle(base.ClientRectangle.Location, new Size(base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1));
					g.DrawRectangle(p, modRect);
				}
			}
		}

		// Token: 0x04000124 RID: 292
		private bool _flatBorder;
	}
}
