using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Docking
{
	// Token: 0x02000020 RID: 32
	[ToolboxItem(false)]
	public class DarkToolWindow : DarkDockContent
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00008A64 File Offset: 0x00006C64
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00008A7C File Offset: 0x00006C7C
		public DarkToolWindow()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			this.BackColor = Colors.GreyBackground;
			base.Padding = new Padding(0, 25, 0, 0);
			this.UpdateCloseButton();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008AD0 File Offset: 0x00006CD0
		private bool IsActive()
		{
			bool flag = base.DockPanel == null;
			return !flag && base.DockPanel.ActiveContent == this;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008B04 File Offset: 0x00006D04
		private void UpdateCloseButton()
		{
			this._headerRect = new Rectangle
			{
				X = base.ClientRectangle.Left,
				Y = base.ClientRectangle.Top,
				Width = base.ClientRectangle.Width,
				Height = 25
			};
			this._closeButtonRect = new Rectangle
			{
				X = base.ClientRectangle.Right - DockIcons.tw_close.Width - 5 - 3,
				Y = base.ClientRectangle.Top + 12 - DockIcons.tw_close.Height / 2,
				Width = DockIcons.tw_close.Width,
				Height = DockIcons.tw_close.Height
			};
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008BEB File Offset: 0x00006DEB
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.UpdateCloseButton();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008C00 File Offset: 0x00006E00
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool flag = this._closeButtonRect.Contains(e.Location) || this._closeButtonPressed;
			if (flag)
			{
				bool flag2 = !this._closeButtonHot;
				if (flag2)
				{
					this._closeButtonHot = true;
					base.Invalidate();
				}
			}
			else
			{
				bool closeButtonHot = this._closeButtonHot;
				if (closeButtonHot)
				{
					this._closeButtonHot = false;
					base.Invalidate();
				}
				bool shouldDrag = this._shouldDrag;
				if (shouldDrag)
				{
					base.DockPanel.DragContent(this);
				}
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008C90 File Offset: 0x00006E90
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool flag = this._closeButtonRect.Contains(e.Location);
			if (flag)
			{
				this._closeButtonPressed = true;
				this._closeButtonHot = true;
				base.Invalidate();
			}
			else
			{
				bool flag2 = this._headerRect.Contains(e.Location);
				if (flag2)
				{
					this._shouldDrag = true;
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008CF4 File Offset: 0x00006EF4
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			bool flag = this._closeButtonRect.Contains(e.Location) && this._closeButtonPressed;
			if (flag)
			{
				this.Close();
			}
			this._closeButtonPressed = false;
			this._closeButtonHot = false;
			this._shouldDrag = false;
			base.Invalidate();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008D50 File Offset: 0x00006F50
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (SolidBrush b = new SolidBrush(Colors.GreyBackground))
			{
				g.FillRectangle(b, base.ClientRectangle);
			}
			bool isActive = this.IsActive();
			Color bgColor = (isActive ? Colors.BlueBackground : Colors.HeaderBackground);
			Color darkColor = (isActive ? Colors.DarkBlueBorder : Colors.DarkBorder);
			Color lightColor = (isActive ? Colors.LightBlueBorder : Colors.LightBorder);
			using (SolidBrush b2 = new SolidBrush(bgColor))
			{
				Rectangle bgRect = new Rectangle(0, 0, base.ClientRectangle.Width, 25);
				g.FillRectangle(b2, bgRect);
			}
			using (Pen p = new Pen(darkColor))
			{
				g.DrawLine(p, base.ClientRectangle.Left, 0, base.ClientRectangle.Right, 0);
				g.DrawLine(p, base.ClientRectangle.Left, 24, base.ClientRectangle.Right, 24);
			}
			using (Pen p2 = new Pen(lightColor))
			{
				g.DrawLine(p2, base.ClientRectangle.Left, 1, base.ClientRectangle.Right, 1);
			}
			int xOffset = 2;
			bool flag = base.Icon != null;
			if (flag)
			{
				g.DrawImageUnscaled(base.Icon, base.ClientRectangle.Left + 5, base.ClientRectangle.Top + 12 - base.Icon.Height / 2 + 1);
				xOffset = base.Icon.Width + 8;
			}
			using (SolidBrush b3 = new SolidBrush(Colors.LightText))
			{
				Rectangle textRect = new Rectangle(xOffset, 0, base.ClientRectangle.Width - 4 - xOffset, 25);
				StringFormat format = new StringFormat
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center,
					FormatFlags = StringFormatFlags.NoWrap,
					Trimming = StringTrimming.EllipsisCharacter
				};
				g.DrawString(base.DockText, this.Font, b3, textRect, format);
			}
			Bitmap img = (this._closeButtonHot ? DockIcons.tw_close_selected : DockIcons.tw_close);
			bool flag2 = isActive;
			if (flag2)
			{
				img = (this._closeButtonHot ? DockIcons.tw_active_close_selected : DockIcons.tw_active_close);
			}
			g.DrawImageUnscaled(img, this._closeButtonRect.Left, this._closeButtonRect.Top);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000043C9 File Offset: 0x000025C9
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		// Token: 0x0400015E RID: 350
		private Rectangle _closeButtonRect;

		// Token: 0x0400015F RID: 351
		private bool _closeButtonHot = false;

		// Token: 0x04000160 RID: 352
		private bool _closeButtonPressed = false;

		// Token: 0x04000161 RID: 353
		private Rectangle _headerRect;

		// Token: 0x04000162 RID: 354
		private bool _shouldDrag;
	}
}
