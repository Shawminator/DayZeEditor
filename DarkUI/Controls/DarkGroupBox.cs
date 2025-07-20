using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000035 RID: 53
	public class DarkGroupBox : GroupBox
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000C410 File Offset: 0x0000A610
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000C428 File Offset: 0x0000A628
		[Category("Appearance")]
		[Description("Determines the color of the border.")]
		public Color BorderColor
		{
			get
			{
				return this._borderColor;
			}
			set
			{
				this._borderColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000C439 File Offset: 0x0000A639
		public DarkGroupBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.ResizeRedraw = true;
			this.DoubleBuffered = true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000C46C File Offset: 0x0000A66C
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			SizeF stringSize = g.MeasureString(this.Text, this.Font);
			Color textColor = Colors.LightText;
			Color fillColor = Colors.BlackHighlight;
			using (SolidBrush b = new SolidBrush(fillColor))
			{
				g.FillRectangle(b, rect);
			}
			using (Pen p = new Pen(this.BorderColor, 1f))
			{
				Rectangle borderRect = new Rectangle(0, (int)stringSize.Height / 2, rect.Width - 1, rect.Height - (int)stringSize.Height / 2 - 1);
				g.DrawRectangle(p, borderRect);
			}
			Rectangle textRect = new Rectangle(rect.Left + Consts.Padding, rect.Top, rect.Width - Consts.Padding * 2, (int)stringSize.Height);
			using (SolidBrush b2 = new SolidBrush(fillColor))
			{
				Rectangle modRect = new Rectangle(textRect.Left, textRect.Top, Math.Min(textRect.Width, (int)stringSize.Width), textRect.Height);
				g.FillRectangle(b2, modRect);
			}
			using (SolidBrush b3 = new SolidBrush(textColor))
			{
				StringFormat stringFormat = new StringFormat
				{
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Near,
					FormatFlags = StringFormatFlags.NoWrap,
					Trimming = StringTrimming.EllipsisCharacter
				};
				g.DrawString(this.Text, this.Font, b3, textRect, stringFormat);
			}
		}

		// Token: 0x040001B9 RID: 441
		private Color _borderColor = Colors.DarkBorder;
	}
}
