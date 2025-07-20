using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000045 RID: 69
	public class DarkSectionPanel : Panel
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00013234 File Offset: 0x00011434
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0001324C File Offset: 0x0001144C
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00013264 File Offset: 0x00011464
		[Category("Appearance")]
		[Description("The section header text associated with this control.")]
		public string SectionHeader
		{
			get
			{
				return this._sectionHeader;
			}
			set
			{
				this._sectionHeader = value;
				base.Invalidate();
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00013275 File Offset: 0x00011475
		public DarkSectionPanel()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.Padding = new Padding(1, 25, 1, 1);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0001329D File Offset: 0x0001149D
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			base.Invalidate();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000132AF File Offset: 0x000114AF
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			base.Invalidate();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000132C4 File Offset: 0x000114C4
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = base.Controls.Count > 0;
			if (flag)
			{
				base.Controls[0].Focus();
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00013300 File Offset: 0x00011500
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = base.ClientRectangle;
			using (SolidBrush b = new SolidBrush(Colors.GreyBackground))
			{
				g.FillRectangle(b, rect);
			}
			Color bgColor = (base.ContainsFocus ? Colors.BlueBackground : Colors.HeaderBackground);
			Color darkColor = (base.ContainsFocus ? Colors.DarkBlueBorder : Colors.DarkBorder);
			Color lightColor = (base.ContainsFocus ? Colors.LightBlueBorder : Colors.LightBorder);
			using (SolidBrush b2 = new SolidBrush(bgColor))
			{
				Rectangle bgRect = new Rectangle(0, 0, rect.Width, 25);
				g.FillRectangle(b2, bgRect);
			}
			using (Pen p = new Pen(darkColor))
			{
				g.DrawLine(p, rect.Left, 0, rect.Right, 0);
				g.DrawLine(p, rect.Left, 24, rect.Right, 24);
			}
			using (Pen p2 = new Pen(lightColor))
			{
				g.DrawLine(p2, rect.Left, 1, rect.Right, 1);
			}
			int xOffset = 3;
			using (SolidBrush b3 = new SolidBrush(Colors.LightText))
			{
				Rectangle textRect = new Rectangle(xOffset, 0, rect.Width - 4 - xOffset, 25);
				StringFormat format = new StringFormat
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center,
					FormatFlags = StringFormatFlags.NoWrap,
					Trimming = StringTrimming.EllipsisCharacter
				};
				g.DrawString(this.SectionHeader, this.Font, b3, textRect, format);
			}
			using (Pen p3 = new Pen(Colors.DarkBorder, 1f))
			{
				Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
				g.DrawRectangle(p3, modRect);
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		// Token: 0x04000216 RID: 534
		private string _sectionHeader;
	}
}
