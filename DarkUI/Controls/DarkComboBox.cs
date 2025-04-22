using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000032 RID: 50
	public class DarkComboBox : ComboBox
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000BF08 File Offset: 0x0000A108
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000BF10 File Offset: 0x0000A110
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000BF19 File Offset: 0x0000A119
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000BF21 File Offset: 0x0000A121
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000BF2A File Offset: 0x0000A12A
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000BF32 File Offset: 0x0000A132
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FlatStyle FlatStyle { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000BF3B File Offset: 0x0000A13B
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000BF43 File Offset: 0x0000A143
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ComboBoxStyle DropDownStyle { get; set; }

		// Token: 0x060001E7 RID: 487 RVA: 0x0000BF4C File Offset: 0x0000A14C
		public DarkComboBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.DrawMode = DrawMode.OwnerDrawVariable;
			base.FlatStyle = FlatStyle.Flat;
			base.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000BF7C File Offset: 0x0000A17C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._buffer = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A876 File Offset: 0x00008A76
		protected override void OnTabStopChanged(EventArgs e)
		{
			base.OnTabStopChanged(e);
			base.Invalidate();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A888 File Offset: 0x00008A88
		protected override void OnTabIndexChanged(EventArgs e)
		{
			base.OnTabIndexChanged(e);
			base.Invalidate();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A89A File Offset: 0x00008A9A
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A8AC File Offset: 0x00008AAC
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A8BE File Offset: 0x00008ABE
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			base.Invalidate();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		protected override void OnTextUpdate(EventArgs e)
		{
			base.OnTextUpdate(e);
			base.Invalidate();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A8E2 File Offset: 0x00008AE2
		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged(e);
			base.Invalidate();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000BF9E File Offset: 0x0000A19E
		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
			this.PaintCombobox();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000BFB0 File Offset: 0x0000A1B0
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this._buffer = null;
			base.Invalidate();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		private void PaintCombobox()
		{
			bool flag = this._buffer == null;
			if (flag)
			{
				this._buffer = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height);
			}
			using (Graphics g = Graphics.FromImage(this._buffer))
			{
				Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
				Color textColor = Colors.LightText;
				Color borderColor = Colors.GreySelection;
				Color fillColor = Colors.LightBackground;
				bool flag2 = this.Focused && base.TabStop;
				if (flag2)
				{
					borderColor = Colors.BlueHighlight;
				}
				using (SolidBrush b = new SolidBrush(fillColor))
				{
					g.FillRectangle(b, rect);
				}
				using (Pen p = new Pen(borderColor, 1f))
				{
					Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
					g.DrawRectangle(p, modRect);
				}
				Bitmap icon = ScrollIcons.scrollbar_arrow_hot;
				g.DrawImageUnscaled(icon, rect.Right - icon.Width - Consts.Padding / 2, rect.Height / 2 - icon.Height / 2);
				string text = ((base.SelectedItem != null) ? base.SelectedItem.ToString() : this.Text);
				using (SolidBrush b2 = new SolidBrush(textColor))
				{
					int padding = 2;
					Rectangle modRect2 = new Rectangle(rect.Left + padding, rect.Top + padding, rect.Width - icon.Width - Consts.Padding / 2 - padding * 2, rect.Height - padding * 2);
					StringFormat stringFormat = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Near,
						FormatFlags = StringFormatFlags.NoWrap,
						Trimming = StringTrimming.EllipsisCharacter
					};
					g.DrawString(text, this.Font, b2, modRect2, stringFormat);
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C258 File Offset: 0x0000A458
		protected override void OnPaint(PaintEventArgs e)
		{
			bool flag = this._buffer == null;
			if (flag)
			{
				this.PaintCombobox();
			}
			Graphics g = e.Graphics;
			g.DrawImageUnscaled(this._buffer, Point.Empty);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C294 File Offset: 0x0000A494
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = e.Bounds;
			Color textColor = Colors.LightText;
			Color fillColor = Colors.LightBackground;
			bool flag = (e.State & DrawItemState.Selected) == DrawItemState.Selected || (e.State & DrawItemState.Focus) == DrawItemState.Focus || (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect;
			if (flag)
			{
				fillColor = Colors.BlueSelection;
			}
			using (SolidBrush b = new SolidBrush(fillColor))
			{
				g.FillRectangle(b, rect);
			}
			bool flag2 = e.Index >= 0 && e.Index < base.Items.Count;
			if (flag2)
			{
				string text = base.Items[e.Index].ToString();
				using (SolidBrush b2 = new SolidBrush(textColor))
				{
					int padding = 2;
					Rectangle modRect = new Rectangle(rect.Left + padding, rect.Top + padding, rect.Width - padding * 2, rect.Height - padding * 2);
					StringFormat stringFormat = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Near,
						FormatFlags = StringFormatFlags.NoWrap,
						Trimming = StringTrimming.EllipsisCharacter
					};
					g.DrawString(text, this.Font, b2, modRect, stringFormat);
				}
			}
		}

		// Token: 0x040001B0 RID: 432
		private Bitmap _buffer;
	}
}
