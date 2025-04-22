using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000036 RID: 54
	public class DarkNumericUpDown : NumericUpDown
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000C668 File Offset: 0x0000A868
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000C670 File Offset: 0x0000A870
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000C679 File Offset: 0x0000A879
		// (set) Token: 0x060001FC RID: 508 RVA: 0x0000C681 File Offset: 0x0000A881
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor { get; set; }

		// Token: 0x060001FD RID: 509 RVA: 0x0000C68C File Offset: 0x0000A88C
		public DarkNumericUpDown()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			base.ForeColor = Color.Gainsboro;
			base.BackColor = Colors.LightBackground;
			base.Controls[0].Paint += this.DarkNumericUpDown_Paint;
			try
			{
				Type type = base.Controls[0].GetType();
				BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
				MethodInfo method = type.GetMethod("SetStyle", flags);
				bool flag = method != null;
				if (flag)
				{
					object[] param = new object[]
					{
						ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer,
						true
					};
					method.Invoke(base.Controls[0], param);
				}
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000C760 File Offset: 0x0000A960
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.Invalidate();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000C76A File Offset: 0x0000A96A
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this._mouseDown = true;
			base.Invalidate();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000C77B File Offset: 0x0000A97B
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			this._mouseDown = false;
			base.Invalidate();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000C78C File Offset: 0x0000A98C
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			base.Invalidate();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000C79E File Offset: 0x0000A99E
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			base.Invalidate();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B95A File Offset: 0x00009B5A
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000C7B0 File Offset: 0x0000A9B0
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000C7C2 File Offset: 0x0000A9C2
		protected override void OnTextBoxLostFocus(object source, EventArgs e)
		{
			base.OnTextBoxLostFocus(source, e);
			base.Invalidate();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		private void DarkNumericUpDown_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = e.ClipRectangle;
			Color fillColor = Colors.HeaderBackground;
			using (SolidBrush b = new SolidBrush(fillColor))
			{
				g.FillRectangle(b, rect);
			}
			Point mousePos = base.Controls[0].PointToClient(Cursor.Position);
			Rectangle upArea = new Rectangle(0, 0, rect.Width, rect.Height / 2);
			bool upHot = upArea.Contains(mousePos);
			Bitmap upIcon = (upHot ? ScrollIcons.scrollbar_arrow_small_hot : ScrollIcons.scrollbar_arrow_small_standard);
			bool flag = upHot && this._mouseDown;
			if (flag)
			{
				upIcon = ScrollIcons.scrollbar_arrow_small_clicked;
			}
			upIcon.RotateFlip(RotateFlipType.Rotate180FlipX);
			g.DrawImageUnscaled(upIcon, upArea.Width / 2 - upIcon.Width / 2, upArea.Height / 2 - upIcon.Height / 2);
			Rectangle downArea = new Rectangle(0, rect.Height / 2, rect.Width, rect.Height / 2);
			bool downHot = downArea.Contains(mousePos);
			Bitmap downIcon = (downHot ? ScrollIcons.scrollbar_arrow_small_hot : ScrollIcons.scrollbar_arrow_small_standard);
			bool flag2 = downHot && this._mouseDown;
			if (flag2)
			{
				downIcon = ScrollIcons.scrollbar_arrow_small_clicked;
			}
			g.DrawImageUnscaled(downIcon, downArea.Width / 2 - downIcon.Width / 2, downArea.Top + downArea.Height / 2 - downIcon.Height / 2);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000C95C File Offset: 0x0000AB5C
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height);
			Color borderColor = Colors.GreySelection;
			bool flag = this.Focused && base.TabStop;
			if (flag)
			{
				borderColor = Colors.BlueHighlight;
			}
			using (Pen p = new Pen(borderColor, 1f))
			{
				Rectangle modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
				g.DrawRectangle(p, modRect);
			}
		}

		// Token: 0x040001BC RID: 444
		private bool _mouseDown;
	}
}
